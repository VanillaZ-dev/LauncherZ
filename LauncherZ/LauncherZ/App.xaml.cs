using System.Windows;
using LauncherZ.Services;
using LauncherZ.ViewModels;

namespace LauncherZ
{
    public partial class App : Application
    {
        public static MainViewModel ViewModel { get; private set; } = null!;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            var settings = new SettingsService();
            var theme    = new ThemeService();
            var query    = new ServerQueryService();
            var launch   = new GameLaunchService();
            ViewModel    = new MainViewModel(query, settings, launch, theme);
        }
    }
}
