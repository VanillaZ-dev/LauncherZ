using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using LauncherZ.Models;
using LauncherZ.Services;

namespace LauncherZ.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private readonly ServerQueryService _query;
        private readonly SettingsService    _settings;
        private readonly GameLaunchService  _launch;
        private readonly ThemeService       _theme;
        private CancellationTokenSource?    _cts;

        public AppSettings Settings { get; private set; }
        public ICollectionView ServerView { get; }

        private readonly ObservableCollection<ServerInfo> _servers = new();

        private ServerInfo? _selected;
        public ServerInfo? SelectedServer { get => _selected; set { _selected = value; OnPC(); } }

        private string _activeTab = "official";
        public string ActiveTab { get => _activeTab; set { _activeTab = value; OnPC(); ServerView.Refresh(); } }

        private string _searchQuery = string.Empty;
        public string SearchQuery { get => _searchQuery; set { _searchQuery = value; OnPC(); ServerView.Refresh(); } }

        private bool _filterNoQueue;
        public bool FilterNoQueue { get => _filterNoQueue; set { _filterNoQueue = value; OnPC(); ServerView.Refresh(); } }

        private bool _filterVanilla;
        public bool FilterVanillaOnly { get => _filterVanilla; set { _filterVanilla = value; OnPC(); ServerView.Refresh(); } }

        private bool _filterFP;
        public bool FilterFirstPerson { get => _filterFP; set { _filterFP = value; OnPC(); ServerView.Refresh(); } }

        private string _filterMap = string.Empty;
        public string FilterMap { get => _filterMap; set { _filterMap = value; OnPC(); ServerView.Refresh(); } }

        private int _filterMaxPing;
        public int FilterMaxPing { get => _filterMaxPing; set { _filterMaxPing = value; OnPC(); ServerView.Refresh(); } }

        private int _scanned;
        public int ScannedCount { get => _scanned; set { _scanned = value; OnPC(); } }

        private int _total;
        public int TotalCount { get => _total; set { _total = value; OnPC(); } }

        private int _visible;
        public int VisibleCount { get => _visible; set { _visible = value; OnPC(); } }

        private string _sortCol = "map";
        public string SortColumn { get => _sortCol; private set { _sortCol = value; OnPC(); } }

        private bool _sortAsc = true;
        public bool SortAscending { get => _sortAsc; private set { _sortAsc = value; OnPC(); } }

        public List<string> AllMaps { get; } = new()
        {
            "Chernarus","Livonia","Sakhal",
            "Alteria","Anastara","Antoria","Area of Decay","Arsteinen","Banov",
            "Bear Island","Bitterroot","Chernobyl Zone","Chiemsee","Deadfall",
            "Deer Isle","DL Malden","DM Ship","Door County","Esseker","FogFall",
            "Gemer","Green County","Heart of Moscow","Iztek","Melkart",
            "Mystery Island","Namalsk","Napf","New York","NH Chernobyl","Nyheim",
            "Panthera","Pripyat","Rio de Janeiro","Rostow","Sarov","Scotland",
            "Stuart Island","Takistan","Takistan Plus","Taviana","Ukraine","Utes",
            "Valning","Winter Deer Isle","Yiprit","Zaha"
        };

        public MainViewModel(
            ServerQueryService query, SettingsService settings,
            GameLaunchService launch, ThemeService theme)
        {
            _query = query; _settings = settings; _launch = launch; _theme = theme;
            Settings = _settings.Load();
            ServerView = CollectionViewSource.GetDefaultView(_servers);
            ServerView.Filter = FilterFn;
            ApplySort();
            ServerView.CollectionChanged += (_, _) =>
                VisibleCount = _servers.Count(FilterFn);
            ApplyThemeById(Settings.ActiveThemeId);
        }

        public async Task StartScanAsync()
        {
            _cts?.Cancel();
            _cts = new CancellationTokenSource();
            var ct = _cts.Token;
            _servers.Clear();
            ScannedCount = 0; TotalCount = 0;
            var progress = new Progress<(int v, int t)>(p =>
                Application.Current.Dispatcher.Invoke(() =>
                { ScannedCount = p.v; TotalCount = p.t; }));
            try
            {
                await foreach (var s in _query.QueryAllServersAsync(progress, ct))
                {
                    if (ct.IsCancellationRequested) break;
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        s.IsFavorite = Settings.FavoriteAddresses.Contains(s.Address);
                        s.IsRecent   = Settings.RecentAddresses.Contains(s.Address);
                        _servers.Add(s);
                        VisibleCount = _servers.Count(FilterFn);
                    });
                }
            }
            catch (OperationCanceledException) { }
        }

        public void StopScan() => _cts?.Cancel();

        public void ToggleFavorite(ServerInfo s)
        {
            s.IsFavorite = !s.IsFavorite;
            if (s.IsFavorite) Settings.FavoriteAddresses.Add(s.Address);
            else Settings.FavoriteAddresses.Remove(s.Address);
            _settings.Save(Settings);
            ServerView.Refresh();
        }

        public bool Connect(ServerInfo s)
        {
            Settings.RecentAddresses.Remove(s.Address);
            Settings.RecentAddresses.Insert(0, s.Address);
            if (Settings.RecentAddresses.Count > 20)
                Settings.RecentAddresses.RemoveAt(Settings.RecentAddresses.Count - 1);
            s.IsRecent = true;
            _settings.Save(Settings);
            return _launch.LaunchAndConnect(s, Settings);
        }

        public void CycleSort(string col)
        {
            if (_sortCol == col) SortAscending = !_sortAsc;
            else { SortColumn = col; SortAscending = true; }
            ApplySort();
        }

        private void ApplySort()
        {
            using (ServerView.DeferRefresh())
            {
                ServerView.SortDescriptions.Clear();
                string prop = _sortCol switch
                {
                    "players" => nameof(ServerInfo.Players),
                    "ping"    => nameof(ServerInfo.Ping),
                    _         => nameof(ServerInfo.Map)
                };
                ServerView.SortDescriptions.Add(new SortDescription(prop,
                    _sortAsc ? ListSortDirection.Ascending : ListSortDirection.Descending));
            }
        }

        private bool FilterFn(object o)
        {
            if (o is not ServerInfo s) return false;
            bool tab = _activeTab switch
            {
                "official"  => s.IsOfficial,
                "community" => !s.IsOfficial,
                "favs"      => s.IsFavorite,
                "recent"    => s.IsRecent,
                _           => true
            };
            if (!tab) return false;
            if (!string.IsNullOrEmpty(_searchQuery))
            {
                var q = _searchQuery;
                if (!s.Name.Contains(q, StringComparison.OrdinalIgnoreCase) &&
                    !s.Map.Contains(q, StringComparison.OrdinalIgnoreCase) &&
                    !s.Address.Contains(q))
                    return false;
            }
            if (_filterNoQueue && s.QueueCount > 0) return false;
            if (_filterVanilla && !s.IsVanilla && !s.IsOfficial) return false;
            if (_filterFP && !s.IsFirstPerson) return false;
            if (!string.IsNullOrEmpty(_filterMap) &&
                !s.Map.Equals(_filterMap, StringComparison.OrdinalIgnoreCase)) return false;
            if (_filterMaxPing > 0 && s.Ping > _filterMaxPing) return false;
            if (!Settings.ShowEmptyServers && s.Players == 0) return false;
            return true;
        }

        public void ApplyThemeById(string id)
        {
            var preset = Array.Find(ThemeDefinition.Presets, t => t.Id == id);
            if (preset == null) return;
            _theme.Apply(preset);
            Settings.ActiveThemeId = id;
        }

        public void SaveSettings() => _settings.Save(Settings);

        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPC([CallerMemberName] string? n = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(n));
    }
}
