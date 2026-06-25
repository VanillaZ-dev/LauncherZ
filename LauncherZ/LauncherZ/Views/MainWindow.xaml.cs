using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using LauncherZ.Models;
using LauncherZ.Services;
using LauncherZ.ViewModels;

namespace LauncherZ.Views
{
    public partial class MainWindow : Window
    {
        private readonly MainViewModel _vm;
        private Timer? _autoRefreshTimer;

        public MainWindow()
        {
            InitializeComponent();
            _vm = App.ViewModel;
            DataContext = _vm;
            Loaded += OnLoaded;
            Closing += OnClosing;
        }

        private async void OnLoaded(object sender, RoutedEventArgs e)
        {
            MapFilter.Items.Add(new ComboBoxItem { Content = "Any", Tag = "" });
            foreach (var map in _vm.AllMaps)
                MapFilter.Items.Add(new ComboBoxItem { Content = map, Tag = map });
            MapFilter.SelectedIndex = 0;
            PingFilter.SelectedIndex = 0;

            PlayerNameBox.Text = _vm.Settings.PlayerName;
            PathBox.Text = _vm.Settings.DayZPath;
            CloseOnConnectToggle.IsChecked = _vm.Settings.CloseOnConnect;
            AutoRefreshToggle.IsChecked = _vm.Settings.AutoRefresh;
            ShowEmptyToggle.IsChecked = _vm.Settings.ShowEmptyServers;

            string ver = GameLaunchService.GetGameVersion(_vm.Settings.DayZPath);
            VersionText.Text = $"DayZ {(string.IsNullOrEmpty(ver) ? "--" : ver)}  ·  LauncherZ 1.0.0";

            ServerList.ItemsSource = _vm.ServerView;
            BuildThemeGrid();

            await StartScanAsync();

            if (_vm.Settings.AutoRefresh)
                _autoRefreshTimer = new Timer(async _ =>
                    await Dispatcher.InvokeAsync(async () => await StartScanAsync()),
                    null, TimeSpan.FromMinutes(3), TimeSpan.FromMinutes(3));
        }

        private void OnClosing(object? sender, System.ComponentModel.CancelEventArgs e)
        {
            _vm.StopScan();
            _autoRefreshTimer?.Dispose();
            _vm.SaveSettings();
        }

        private async Task StartScanAsync()
        {
            SetStatus("Scanning servers...");
            ScanBar.Width = 0;
            _vm.PropertyChanged += OnVmPropertyChanged;
            await _vm.StartScanAsync();
            _vm.PropertyChanged -= OnVmPropertyChanged;
            UpdateCounts();
            SetStatus($"{_vm.ScannedCount:N0} scanned");
        }

        private void OnVmPropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(MainViewModel.ScannedCount) ||
                e.PropertyName == nameof(MainViewModel.TotalCount))
            {
                Dispatcher.Invoke(() =>
                {
                    SetStatus($"Scanning -- {_vm.ScannedCount:N0} servers validated");
                    if (_vm.TotalCount > 0)
                        ScanBar.Width = ActualWidth * ((double)_vm.ScannedCount / _vm.TotalCount);
                    UpdateCounts();
                    ScanStatusRun.Text = $"{_vm.ScannedCount:N0} servers validated";
                    FilteredRun.Text = $"{_vm.TotalCount - _vm.ScannedCount:N0} filtered";
                });
            }
        }

        private void UpdateCounts()
        {
            VisibleRun.Text = _vm.VisibleCount.ToString("N0");
            TotalRun.Text = _vm.TotalCount.ToString("N0");
        }

        private void SetStatus(string msg) => StatusText.Text = msg;

        private void TitleBar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left) DragMove();
        }

        private void Minimize_Click(object sender, RoutedEventArgs e) =>
            WindowState = WindowState.Minimized;

        private void Maximize_Click(object sender, RoutedEventArgs e) =>
            WindowState = WindowState == WindowState.Maximized
                ? WindowState.Normal : WindowState.Maximized;

        private void Close_Click(object sender, RoutedEventArgs e) => Close();

        private void Tab_Click(object sender, RoutedEventArgs e)
        {
            if (sender is not Button btn) return;
            string tab = btn.Name switch
            {
                "TabOfficial"  => "official",
                "TabCommunity" => "community",
                "TabFavorites" => "favs",
                "TabRecent"    => "recent",
                _              => "official"
            };
            foreach (var b in new[] { TabOfficial, TabCommunity, TabFavorites, TabRecent })
                b.Tag = null;
            btn.Tag = "active";
            _vm.ActiveTab = tab;
            UpdateCounts();
        }

        private void Search_TextChanged(object sender, TextChangedEventArgs e)
        {
            _vm.SearchQuery = SearchBox.Text;
            UpdateCounts();
        }

        private void MapFilter_Changed(object sender, SelectionChangedEventArgs e)
        {
            if (MapFilter.SelectedItem is ComboBoxItem item)
                _vm.FilterMap = item.Tag?.ToString() ?? "";
        }

        private void PingFilter_Changed(object sender, SelectionChangedEventArgs e)
        {
            if (PingFilter.SelectedItem is ComboBoxItem item &&
                int.TryParse(item.Tag?.ToString(), out int val))
                _vm.FilterMaxPing = val;
        }

        private void ServerList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ServerList.SelectedItem is not ServerInfo server) return;
            _vm.SelectedServer = server;
            UpdateInfoCard(server);
        }

        private void UpdateInfoCard(ServerInfo server)
        {
            ServerNameText.Text = server.Name;
            ServerIpText.Text = server.Address;
            StatPlayers.Text = $"{server.Players}/{server.MaxPlayers}";
            StatQueue.Text = server.QueueCount > 0 ? server.QueueCount.ToString() : "--";
            StatPing.Text = $"{server.Ping}ms";
            StatTime.Text = string.IsNullOrEmpty(server.GameTime) ? "--:--" : server.GameTime;

            StatPing.Foreground = server.Ping < 60
                ? FindResource("PingGoodBrush") as Brush
                : server.Ping < 120
                    ? FindResource("PingMidBrush") as Brush
                    : FindResource("PingBadBrush") as Brush;

            if (server.IsOfficial)
            {
                ServerTagBadge.Background  = FindResource("TagOffBgBrush") as Brush;
                ServerTagBadge.BorderBrush = FindResource("TagOffBdrBrush") as Brush;
                ServerTagText.Foreground   = FindResource("TagOffTxtBrush") as Brush;
                ServerTagText.Text = "OFFICIAL";
            }
            else if (server.IsVanilla)
            {
                ServerTagBadge.Background  = FindResource("TagVanBgBrush") as Brush;
                ServerTagBadge.BorderBrush = FindResource("TagVanBdrBrush") as Brush;
                ServerTagText.Foreground   = FindResource("TagVanTxtBrush") as Brush;
                ServerTagText.Text = "VANILLA";
            }
            else
            {
                ServerTagBadge.Background  = FindResource("TagModBgBrush") as Brush;
                ServerTagBadge.BorderBrush = FindResource("TagModBdrBrush") as Brush;
                ServerTagText.Foreground   = FindResource("TagModTxtBrush") as Brush;
                ServerTagText.Text = $"{server.ModCount} MODS";
            }

            ConnectLabel.Text = server.IsModded ? "INSTALL MODS & CONNECT" : "CONNECT";
        }

        private void Fav_Click(object sender, MouseButtonEventArgs e)
        {
            if (sender is FrameworkElement fe && fe.DataContext is ServerInfo server)
            {
                e.Handled = true;
                _vm.ToggleFavorite(server);
            }
        }

        private void IP_Click(object sender, MouseButtonEventArgs e)
        {
            if (_vm.SelectedServer == null) return;
            string ip = _vm.SelectedServer.Address;
            Clipboard.SetText(ip);
            ServerIpText.Text = "Copied!";
            Task.Delay(1500).ContinueWith(_ =>
                Dispatcher.Invoke(() => ServerIpText.Text = _vm.SelectedServer?.Address ?? ""));
        }

        private void Connect_Click(object sender, RoutedEventArgs e)
        {
            if (_vm.SelectedServer == null) return;
            if (_vm.SelectedServer.IsModded && _vm.SelectedServer.Mods.Any())
                ShowDownloadDialog(_vm.SelectedServer);
            else
                DoConnect(_vm.SelectedServer);
        }

        private void DoConnect(ServerInfo server)
        {
            bool ok = _vm.Connect(server);
            if (ok && _vm.Settings.CloseOnConnect)
                WindowState = WindowState.Minimized;
        }

        private void Mods_Click(object sender, RoutedEventArgs e)
        {
            if (_vm.SelectedServer != null)
                ShowModsPopup(_vm.SelectedServer);
        }

        private void ShowModsPopup(ServerInfo server)
        {
            ModsPopupTitle.Text = server.Name;
            ModsPopupSub.Text = $"{server.Mods.Count} mod{(server.Mods.Count != 1 ? "s" : "")} on this server";
            ModsList.Children.Clear();
            foreach (var mod in server.Mods)
            {
                var border = new Border
                {
                    Margin = new Thickness(0, 0, 0, 6),
                    Padding = new Thickness(10, 8, 10, 8),
                    CornerRadius = new CornerRadius(5),
                    Background = FindResource("BgRaisedBrush") as Brush,
                    BorderThickness = new Thickness(1),
                    BorderBrush = FindResource("BorderBrush") as Brush
                };
                var dp = new DockPanel();
                var dot = new Ellipse
                {
                    Width = 6,
                    Height = 6,
                    Margin = new Thickness(0, 0, 8, 0),
                    Fill = ServerInfo.AdminMods.Contains(mod)
                        ? FindResource("PingMidBrush") as Brush
                        : FindResource("PingGoodBrush") as Brush
                };
                DockPanel.SetDock(dot, Dock.Left);
                var lbl = new TextBlock
                {
                    Text = mod,
                    FontSize = 13,
                    Foreground = FindResource("TextPrimaryBrush") as Brush,
                    VerticalAlignment = VerticalAlignment.Center
                };
                dp.Children.Add(dot);
                dp.Children.Add(lbl);
                border.Child = dp;
                ModsList.Children.Add(border);
            }
            ModsOverlay.Visibility = Visibility.Visible;
        }

        private void CloseModsPopup_Click(object sender, RoutedEventArgs e) =>
            ModsOverlay.Visibility = Visibility.Collapsed;

        private ServerInfo? _pendingServer;

        private void ShowDownloadDialog(ServerInfo server)
        {
            _pendingServer = server;
            var missing = server.Mods.Where(m => !ServerInfo.AdminMods.Contains(m)).ToList();
            DlSubText.Text = $"{missing.Count} mod{(missing.Count != 1 ? "s" : "")} need to be downloaded before you can join.";
            DlModsList.Children.Clear();
            foreach (var mod in server.Mods)
            {
                var border = new Border
                {
                    Margin = new Thickness(0, 0, 0, 7),
                    Padding = new Thickness(12, 9, 12, 9),
                    CornerRadius = new CornerRadius(5),
                    Background = FindResource("BgRaisedBrush") as Brush,
                    BorderThickness = new Thickness(1),
                    BorderBrush = FindResource("BorderLitBrush") as Brush
                };
                var sp = new StackPanel();
                sp.Children.Add(new TextBlock
                {
                    Text = mod,
                    FontSize = 13,
                    FontWeight = FontWeights.Medium,
                    Foreground = FindResource("TextPrimaryBrush") as Brush
                });
                sp.Children.Add(new Border
                {
                    Height = 4,
                    Margin = new Thickness(0, 6, 0, 0),
                    Background = FindResource("BorderBrush") as Brush,
                    CornerRadius = new CornerRadius(2)
                });
                border.Child = sp;
                DlModsList.Children.Add(border);
            }
            DlStartBtn.IsEnabled = true;
            DlStartBtn.Content = "Download & Connect";
            DownloadOverlay.Visibility = Visibility.Visible;
        }

        private async void DlStart_Click(object sender, RoutedEventArgs e)
        {
            if (_pendingServer == null) return;
            DlStartBtn.IsEnabled = false;
            DlStartBtn.Content = "Downloading...";
            await Task.Delay(500);
            DownloadOverlay.Visibility = Visibility.Collapsed;
            DoConnect(_pendingServer);
        }

        private void DlCancel_Click(object sender, RoutedEventArgs e)
        {
            DownloadOverlay.Visibility = Visibility.Collapsed;
            _pendingServer = null;
        }

        private void SortMap_Click(object sender, RoutedEventArgs e)
        {
            _vm.CycleSort("map");
            ArrMap.Text = " " + (_vm.SortAscending ? "↑" : "↓");
            ArrMap.Opacity = 1;
            HdrMap.Foreground = FindResource("AccentBrush") as Brush;
            ArrPlayers.Opacity = 0;
            HdrPlayers.Foreground = FindResource("TextSecondBrush") as Brush;
            ArrPing.Opacity = 0;
            HdrPing.Foreground = FindResource("TextSecondBrush") as Brush;
            UpdateCounts();
        }

        private void SortPlayers_Click(object sender, RoutedEventArgs e)
        {
            _vm.CycleSort("players");
            ArrPlayers.Text = " " + (_vm.SortAscending ? "↑" : "↓");
            ArrPlayers.Opacity = 1;
            HdrPlayers.Foreground = FindResource("AccentBrush") as Brush;
            ArrMap.Opacity = 0;
            HdrMap.Foreground = FindResource("TextSecondBrush") as Brush;
            ArrPing.Opacity = 0;
            HdrPing.Foreground = FindResource("TextSecondBrush") as Brush;
            UpdateCounts();
        }

        private void SortPing_Click(object sender, RoutedEventArgs e)
        {
            _vm.CycleSort("ping");
            ArrPing.Text = " " + (_vm.SortAscending ? "↑" : "↓");
            ArrPing.Opacity = 1;
            HdrPing.Foreground = FindResource("AccentBrush") as Brush;
            ArrMap.Opacity = 0;
            HdrMap.Foreground = FindResource("TextSecondBrush") as Brush;
            ArrPlayers.Opacity = 0;
            HdrPlayers.Foreground = FindResource("TextSecondBrush") as Brush;
            UpdateCounts();
        }

        private async void Refresh_Click(object sender, RoutedEventArgs e) =>
            await StartScanAsync();

        private void Settings_Click(object sender, RoutedEventArgs e)
        {
            bool open = SettingsPanel.Visibility == Visibility.Visible;
            SettingsPanel.Visibility = open ? Visibility.Collapsed : Visibility.Visible;
            SettingsBtn.Tag = open ? null : "active";
        }

        private void CloseSettings_Click(object sender, RoutedEventArgs e)
        {
            SettingsPanel.Visibility = Visibility.Collapsed;
            SettingsBtn.Tag = null;
        }

        private void PlayerName_Changed(object sender, TextChangedEventArgs e)
        {
            _vm.Settings.PlayerName = PlayerNameBox.Text;
        }

        private void BrowsePath_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new Microsoft.Win32.OpenFileDialog
            {
                Title = "Select DayZ.exe",
                Filter = "DayZ Executable|DayZ.exe",
                FileName = "DayZ.exe"
            };
            if (dialog.ShowDialog() == true)
            {
                var dir = System.IO.Path.GetDirectoryName(dialog.FileName) ?? "";
                PathBox.Text = dir;
                _vm.Settings.DayZPath = dir;
                string ver = GameLaunchService.GetGameVersion(dir);
                VersionText.Text = $"DayZ {(string.IsNullOrEmpty(ver) ? "--" : ver)}  ·  LauncherZ 1.0.0";
            }
        }

        private void Toggle_Click(object sender, RoutedEventArgs e)
        {
            _vm.Settings.CloseOnConnect   = CloseOnConnectToggle.IsChecked == true;
            _vm.Settings.AutoRefresh      = AutoRefreshToggle.IsChecked == true;
            _vm.Settings.ShowEmptyServers = ShowEmptyToggle.IsChecked == true;
            _vm.SaveSettings();

            _autoRefreshTimer?.Dispose();
            if (_vm.Settings.AutoRefresh)
                _autoRefreshTimer = new Timer(async _ =>
                    await Dispatcher.InvokeAsync(async () => await StartScanAsync()),
                    null, TimeSpan.FromMinutes(3), TimeSpan.FromMinutes(3));
        }

        private void BuildThemeGrid()
        {
            ThemeGrid.Children.Clear();
            foreach (var preset in ThemeDefinition.Presets)
            {
                var isActive = preset.Id == _vm.Settings.ActiveThemeId;
                var card = new Button
                {
                    Margin = new Thickness(3, 3, 3, 3),
                    Cursor = Cursors.Hand,
                    Tag = preset.Id
                };

                var stack = new StackPanel();

                var preview = new Border
                {
                    Height = 36,
                    Background = new SolidColorBrush(ParseHex(preset.BgRoot)),
                    Padding = new Thickness(6, 4, 6, 4)
                };
                var bars = new StackPanel { VerticalAlignment = VerticalAlignment.Bottom };
                bars.Children.Add(new Rectangle
                {
                    Height = 3,
                    Width = 38,
                    Fill = new SolidColorBrush(ParseHex(preset.TabActiveBg)),
                    RadiusX = 2,
                    RadiusY = 2,
                    Margin = new Thickness(0, 0, 0, 2)
                });
                bars.Children.Add(new Rectangle
                {
                    Height = 3,
                    Width = 20,
                    Fill = new SolidColorBrush(ParseHex(preset.Accent)),
                    RadiusX = 2,
                    RadiusY = 2
                });
                preview.Child = bars;
                stack.Children.Add(preview);

                var labelBorder = new Border
                {
                    Background = new SolidColorBrush(ParseHex(preset.BgPanel)),
                    Padding = new Thickness(6, 4, 6, 4)
                };
                labelBorder.Child = new TextBlock
                {
                    Text = preset.Label,
                    FontSize = 11,
                    FontWeight = FontWeights.SemiBold,
                    Foreground = new SolidColorBrush(
                        preset.IsDark
                            ? Color.FromRgb(0xcc, 0xc8, 0xc0)
                            : Color.FromRgb(0x1a, 0x24, 0x18))
                };
                stack.Children.Add(labelBorder);
                card.Content = stack;
                card.Click += ThemeCard_Click;

                var tpl = new ControlTemplate(typeof(Button));
                var bd = new FrameworkElementFactory(typeof(Border));
                bd.SetValue(Border.CornerRadiusProperty, new CornerRadius(6));
                bd.SetValue(Border.BorderThicknessProperty, new Thickness(isActive ? 2 : 1));
                bd.SetValue(Border.BorderBrushProperty,
                    isActive
                        ? (object)FindResource("AccentBrush")
                        : (object)FindResource("BorderBrush"));
                var cp = new FrameworkElementFactory(typeof(ContentPresenter));
                bd.AppendChild(cp);
                tpl.VisualTree = bd;
                card.Template = tpl;

                ThemeGrid.Children.Add(card);
            }
        }

        private void ThemeCard_Click(object sender, RoutedEventArgs e)
        {
            if (sender is not Button btn) return;
            string id = btn.Tag?.ToString() ?? "forest";
            _vm.ApplyThemeById(id);
            _vm.SaveSettings();
            BuildThemeGrid();
        }

        private static Color ParseHex(string hex)
        {
            hex = hex.TrimStart('#');
            return Color.FromRgb(
                Convert.ToByte(hex[0..2], 16),
                Convert.ToByte(hex[2..4], 16),
                Convert.ToByte(hex[4..6], 16));
        }
    }
}
