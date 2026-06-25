using System.Collections.Generic;
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
        public bool IsModded => Mods.Count > 0;
        public int ModCount => Mods.Count;

        public string Address => EndPoint != null
            ? $"{EndPoint.Address}:{EndPoint.Port}"
            : string.Empty;
    }
}
