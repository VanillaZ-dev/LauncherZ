using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using LauncherZ.Models;
using Newtonsoft.Json;

namespace LauncherZ.Services
{
    public class ServerQueryService
    {
        private const int TimeoutMs = 1500;
        private const int DayZAppId = 221100;
        private static readonly HttpClient Http = new() { Timeout = TimeSpan.FromSeconds(10) };

        private static readonly byte[] A2SInfo =
        {
            0xFF,0xFF,0xFF,0xFF,0x54,
            0x53,0x6F,0x75,0x72,0x63,0x65,0x20,0x45,0x6E,0x67,
            0x69,0x6E,0x65,0x20,0x51,0x75,0x65,0x72,0x79,0x00
        };

        private static readonly byte[] A2SRulesChallenge =
            { 0xFF,0xFF,0xFF,0xFF,0x56,0xFF,0xFF,0xFF,0xFF };

        private static readonly List<string> OfficialSubnets = new()
        {
            "185.60.","162.254.","216.52.","192.223.","103.28.",
            "209.222.","146.66.","155.133.","205.196."
        };

        public async IAsyncEnumerable<ServerInfo> QueryAllServersAsync(
            IProgress<(int validated, int total)>? progress = null,
            [System.Runtime.CompilerServices.EnumeratorCancellation]
            CancellationToken ct = default)
        {
            var endpoints = await GetServerListAsync(ct);
            int total = endpoints.Count, validated = 0;
            var sem = new SemaphoreSlim(64);
            var channel = System.Threading.Channels.Channel.CreateUnbounded<ServerInfo>();

            var tasks = endpoints.ConvertAll(async ep =>
            {
                await sem.WaitAsync(ct);
                try
                {
                    var s = await QueryServerAsync(ep, ct);
                    if (s != null) await channel.Writer.WriteAsync(s, ct);
                }
                finally
                {
                    Interlocked.Increment(ref validated);
                    progress?.Report((validated, total));
                    sem.Release();
                }
            });

            _ = Task.WhenAll(tasks).ContinueWith(_ => channel.Writer.Complete(), ct);

            await foreach (var s in channel.Reader.ReadAllAsync(ct))
                yield return s;
        }

        public async Task<ServerInfo?> QueryServerAsync(IPEndPoint ep, CancellationToken ct = default)
        {
            try
            {
                var start = DateTime.UtcNow;
                var data = await SendUdp(ep, A2SInfo, ct);
                if (data == null) return null;
                int ping = (int)(DateTime.UtcNow - start).TotalMilliseconds;
                var s = ParseInfo(data, ep, ping);
                if (s == null || !Validate(s)) return null;
                var rules = await QueryRules(ep, ct);
                if (rules != null) ApplyRules(s, rules);
                return s;
            }
            catch { return null; }
        }

        private static bool Validate(ServerInfo s)
        {
            if (!s.GameDir.Equals("dayz", StringComparison.OrdinalIgnoreCase)) return false;
            if (s.MaxPlayers == 0 || s.MaxPlayers > 256) return false;
            if (string.IsNullOrWhiteSpace(s.Name) || s.Name.Length < 3) return false;
            return true;
        }

        private static ServerInfo? ParseInfo(byte[] data, IPEndPoint ep, int ping)
        {
            try
            {
                if (data.Length < 6 || data[4] != 0x49) return null;
                int pos = 6;
                string name    = ReadStr(data, ref pos);
                string map     = ReadStr(data, ref pos);
                string gameDir = ReadStr(data, ref pos);
                ReadStr(data, ref pos);
                if (pos + 2 > data.Length) return null;
                int appId = BitConverter.ToUInt16(data, pos); pos += 2;
                if (appId != DayZAppId) return null;
                int players    = data[pos++];
                int maxPlayers = data[pos++];
                pos++; pos++; pos++;
                bool pw = data[pos++] == 1;

                string ipStr = ep.Address.ToString();
                bool official = OfficialSubnets.Any(sub => ipStr.StartsWith(sub, StringComparison.Ordinal));

                return new ServerInfo
                {
                    Name = name, Map = map, GameDir = gameDir,
                    EndPoint = ep, Players = players, MaxPlayers = maxPlayers,
                    Ping = ping, IsPasswordProtected = pw, IsOfficial = official
                };
            }
            catch { return null; }
        }

        private static void ApplyRules(ServerInfo s, Dictionary<string, string> rules)
        {
            if (rules.TryGetValue("queueCount", out var q) && int.TryParse(q, out int qi)) s.QueueCount = qi;
            if (rules.TryGetValue("Queue", out var q2) && int.TryParse(q2, out int q2i)) s.QueueCount = q2i;
            if (rules.TryGetValue("timeOfDay", out var tod)) s.GameTime = tod;
            if (rules.TryGetValue("firstPerson", out var fp))
                s.IsFirstPerson = fp == "1" || fp.Equals("true", StringComparison.OrdinalIgnoreCase);
            if (rules.TryGetValue("Mods", out var mods) && !string.IsNullOrEmpty(mods))
                s.Mods = new List<string>(mods.Split(';', StringSplitOptions.RemoveEmptyEntries));
        }

        private async Task<Dictionary<string, string>?> QueryRules(IPEndPoint ep, CancellationToken ct)
        {
            try
            {
                var cr = await SendUdp(ep, A2SRulesChallenge, ct);
                if (cr == null || cr.Length < 9) return null;
                byte[] req = { 0xFF,0xFF,0xFF,0xFF,0x56,cr[5],cr[6],cr[7],cr[8] };
                var data = await SendUdp(ep, req, ct);
                if (data == null || data.Length < 7 || data[4] != 0x45) return null;
                var result = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
                int pos = 5;
                if (pos + 1 >= data.Length) return result;
                int count = BitConverter.ToUInt16(data, pos); pos += 2;
                for (int i = 0; i < count && pos < data.Length; i++)
                {
                    string k = ReadStr(data, ref pos);
                    string v = ReadStr(data, ref pos);
                    if (!string.IsNullOrEmpty(k)) result[k] = v;
                }
                return result;
            }
            catch { return null; }
        }

        private async Task<byte[]?> SendUdp(IPEndPoint ep, byte[] req, CancellationToken ct)
        {
            using var udp = new UdpClient();
            udp.Client.ReceiveTimeout = TimeoutMs;
            udp.Client.SendTimeout    = TimeoutMs;
            try
            {
                await udp.SendAsync(req, req.Length, ep)
                         .WaitAsync(TimeSpan.FromMilliseconds(TimeoutMs), ct);
                var r = await udp.ReceiveAsync(ct).AsTask()
                                 .WaitAsync(TimeSpan.FromMilliseconds(TimeoutMs), ct);
                return r.Buffer;
            }
            catch { return null; }
        }

        private static string ReadStr(byte[] data, ref int pos)
        {
            int start = pos;
            while (pos < data.Length && data[pos] != 0x00) pos++;
            string s = Encoding.UTF8.GetString(data, start, pos - start);
            if (pos < data.Length) pos++;
            return s;
        }

        /// <summary>
        /// Gets DayZ server list via Steam Web API over HTTPS.
        /// This avoids UDP firewall issues with direct master server queries.
        /// </summary>
        private static async Task<List<IPEndPoint>> GetServerListAsync(CancellationToken ct)
        {
            var list = new List<IPEndPoint>();
            try
            {
                int start = 0;
                const int batchSize = 20000;
                string url = $"https://api.steampowered.com/IGameServersService/GetServerList/v1/?filter=\\appid\\221100&limit={batchSize}&key=";

                var response = await Http.GetStringAsync(url, ct);
                var json = JsonConvert.DeserializeObject<SteamServerListResponse>(response);

                if (json?.Response?.Servers == null) return list;

                foreach (var server in json.Response.Servers)
                {
                    if (string.IsNullOrEmpty(server.Addr)) continue;
                    var parts = server.Addr.Split(':');
                    if (parts.Length != 2) continue;
                    if (!IPAddress.TryParse(parts[0], out var ip)) continue;
                    if (!int.TryParse(parts[1], out var port)) continue;
                    list.Add(new IPEndPoint(ip, port));
                }
            }
            catch
            {
                // Fall back to master server if API fails
                list = await GetMasterListAsync(ct);
            }
            return list;
        }

        private static async Task<List<IPEndPoint>> GetMasterListAsync(CancellationToken ct)
        {
            var list = new List<IPEndPoint>();
            string[] hosts = { "hl2master.steampowered.com", "208.64.200.65", "208.64.200.52" };

            foreach (var host in hosts)
            {
                try
                {
                    var addrs = await Dns.GetHostAddressesAsync(host, ct);
                    if (addrs.Length == 0) continue;
                    var master = new IPEndPoint(addrs[0], 27011);
                    using var udp = new UdpClient();
                    udp.Client.ReceiveTimeout = 5000;

                    string lastIp = "0.0.0.0";
                    int lastPort = 0;
                    bool done = false;

                    while (!done && !ct.IsCancellationRequested)
                    {
                        byte[] filter = BuildFilter(lastIp, lastPort);
                        await udp.SendAsync(filter, filter.Length, master)
                                 .WaitAsync(TimeSpan.FromSeconds(5), ct);
                        var resp = await udp.ReceiveAsync(ct).AsTask()
                                           .WaitAsync(TimeSpan.FromSeconds(5), ct);
                        var data = resp.Buffer;
                        if (data.Length < 6) break;
                        int pos = 6;
                        while (pos + 5 < data.Length)
                        {
                            var ipb = new byte[4];
                            Array.Copy(data, pos, ipb, 0, 4); pos += 4;
                            int port = (data[pos] << 8) | data[pos + 1]; pos += 2;
                            string ip = $"{ipb[0]}.{ipb[1]}.{ipb[2]}.{ipb[3]}";
                            if (ip == "0.0.0.0" && port == 0) { done = true; break; }
                            if (ip == lastIp && port == lastPort) { done = true; break; }
                            lastIp = ip; lastPort = port;
                            if (IPAddress.TryParse(ip, out var addr))
                                list.Add(new IPEndPoint(addr, port));
                        }
                    }
                    if (list.Count > 0) break;
                }
                catch { continue; }
            }
            return list;
        }

        private static byte[] BuildFilter(string lastIp = "0.0.0.0", int lastPort = 0)
        {
            string addr   = $"{lastIp}:{lastPort}";
            string filter = @"\gamedir\dayz\appid\221100";
            byte[] ab = Encoding.ASCII.GetBytes(addr);
            byte[] fb = Encoding.ASCII.GetBytes(filter);
            byte[] pkt = new byte[2 + ab.Length + 1 + fb.Length + 1];
            pkt[0] = 0x31; pkt[1] = 0xFF;
            Array.Copy(ab, 0, pkt, 2, ab.Length);
            pkt[2 + ab.Length] = 0x00;
            Array.Copy(fb, 0, pkt, 2 + ab.Length + 1, fb.Length);
            pkt[^1] = 0x00;
            return pkt;
        }

        private class SteamServerListResponse
        {
            [JsonProperty("response")]
            public SteamServerListInner? Response { get; set; }
        }

        private class SteamServerListInner
        {
            [JsonProperty("servers")]
            public List<SteamServer>? Servers { get; set; }
        }

        private class SteamServer
        {
            [JsonProperty("addr")]
            public string? Addr { get; set; }
        }
    }
}
