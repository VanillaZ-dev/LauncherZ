using System.Collections.Generic;

namespace LauncherZ.Models
{
    public class AppSettings
    {
        public string PlayerName { get; set; } = "Survivor";
        public string DayZPath { get; set; } = string.Empty;
        public string ModsPath { get; set; } = string.Empty;
        public string ActiveThemeId { get; set; } = "forest";
        public bool CloseOnConnect { get; set; } = true;
        public bool AutoRefresh { get; set; } = true;
        public bool ShowEmptyServers { get; set; } = false;
        public List<string> FavoriteAddresses { get; set; } = new();
        public List<string> RecentAddresses { get; set; } = new();
        public string SortColumn { get; set; } = "map";
        public bool SortAscending { get; set; } = true;
        public bool FilePatchingEnabled => false;
    }
}
