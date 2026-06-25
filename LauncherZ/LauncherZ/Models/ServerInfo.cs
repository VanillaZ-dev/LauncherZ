using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace LauncherZ.Models
{
    public class ServerInfo
    {
        public string Name { get; set; } = string.Empty;
        public string Map { get; set; } = string.Empty;
        public string GameDir { get; set; } = string.Empty;
        public string Version { get; set; } = string.Empty;
        public IPEndPoint? EndPoint { get; set; }
        public int Players { get; set; }
        public int MaxPlayers { get; set; }
        public int QueueCount { get; set; }
        public int Ping { get; set; }
        public string GameTime { get; set; } = string.Empty;
        public bool IsPasswordProtected { get; set; }
        public bool IsFirstPerson { get; set; }
        public bool IsOfficial { get; set; }
        public bool IsFavorite { get; set; }
        public bool IsRecent { get; set; }
        public List<string> Mods { get; set; } = new();

        public bool IsVanilla => !IsOfficial && Mods.All(m => AdminMods.Contains(m));
        public bool IsModded  => !IsOfficial && Mods.Any(m => !AdminMods.Contains(m));
        public int  ModCount  => Mods.Count(m => !AdminMods.Contains(m));

        public string Address => EndPoint != null
            ? $"{EndPoint.Address}:{EndPoint.Port}"
            : string.Empty;

        public static readonly System.Collections.Generic.HashSet<string> AdminMods =
            new(System.StringComparer.OrdinalIgnoreCase)
            {
                "CF", "CFTools", "CommunityOnlineTools", "VPPAdminTools",
                "DAMerge", "ServerMonitor", "DayZCodeLock", "Admin Tools",
                "Community-Online-Tools", "VPP Admin Tools"
            };
    }
}
