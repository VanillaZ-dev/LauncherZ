using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using LauncherZ.Models;

namespace LauncherZ.Services
{
    public class GameLaunchService
    {
        public bool LaunchAndConnect(ServerInfo server, AppSettings settings)
        {
            try
            {
                string exe = Path.Combine(settings.DayZPath, "DayZ.exe");
                if (!File.Exists(exe))
                    throw new FileNotFoundException("DayZ.exe not found.", exe);

                string address = $"{server.EndPoint!.Address}:{server.EndPoint.Port}";

                var args = new List<string>
                {
                    $"-connect={address}",
                    $"-name={Esc(settings.PlayerName)}",
                    "-nolauncher",
                    "-nosplash",
                    "-skipIntro",
                    "-noPause"
                    // -filePatching is deliberately never added
                };

                var gameMods = server.Mods.Where(m => !ServerInfo.AdminMods.Contains(m)).ToList();
                if (gameMods.Count > 0)
                {
                    string modBase = string.IsNullOrEmpty(settings.ModsPath)
                        ? settings.DayZPath : settings.ModsPath;
                    var paths = gameMods
                        .Select(m => Path.Combine(modBase, m))
                        .Where(Directory.Exists)
                        .ToList();
                    if (paths.Count > 0)
                        args.Add($"-mod={string.Join(";", paths)}");
                }

                Process.Start(new ProcessStartInfo
                {
                    FileName = exe,
                    Arguments = string.Join(" ", args),
                    WorkingDirectory = settings.DayZPath,
                    UseShellExecute = false
                });
                return true;
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(
                    $"Failed to launch DayZ:\n\n{ex.Message}",
                    "LauncherZ",
                    System.Windows.MessageBoxButton.OK,
                    System.Windows.MessageBoxImage.Error);
                return false;
            }
        }

        public static string GetGameVersion(string dayZPath)
        {
            try
            {
                string exe = Path.Combine(dayZPath, "DayZ.exe");
                if (!File.Exists(exe)) return string.Empty;
                var info = FileVersionInfo.GetVersionInfo(exe);
                return $"{info.FileMajorPart}.{info.FileMinorPart:D2}";
            }
            catch { return string.Empty; }
        }

        private static string Esc(string s) => s.Contains(' ') ? $"\"{s}\"" : s;
    }
}
