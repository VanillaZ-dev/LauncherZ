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

        public async Task<ServerInfo?>
