using System;
using System.IO;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using LauncherZ.Models;

namespace LauncherZ.Services
{
    public class SettingsService
    {
        private static readonly string AppDataFolder =
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "LauncherZ");

        private static readonly string SettingsFile =
            Path.Combine(AppDataFolder, "settings.json");

        private static readonly string DayZCfgPath =
            Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                "DayZ", "DayZ.cfg");

        public AppSettings Load()
        {
            try
            {
                if (File.Exists(SettingsFile))
                {
                    var json = File.ReadAllText(SettingsFile);
                    var s = JsonConvert.DeserializeObject<AppSettings>(json) ?? new AppSettings();
                    s.PlayerName = ReadPlayerName() ?? s.PlayerName;
                    if (string.IsNullOrEmpty(s.DayZPath)) s.DayZPath = FindDayZPath();
                    return s;
                }
            }
            catch { }

            return new AppSettings
            {
                PlayerName = ReadPlayerName() ?? "Survivor",
                DayZPath = FindDayZPath()
            };
        }

        public void Save(AppSettings settings)
        {
            try
            {
                Directory.CreateDirectory(AppDataFolder);
                File.WriteAllText(SettingsFile, JsonConvert.SerializeObject(settings, Formatting.Indented));
                WritePlayerName(settings.PlayerName);
            }
            catch { }
        }

        private static string? ReadPlayerName()
        {
            try
            {
                if (!File.Exists(DayZCfgPath)) return null;
                var text = File.ReadAllText(DayZCfgPath);
                var m = Regex.Match(text, @"playerName\s*=\s*""([^""]+)""");
                return m.Success ? m.Groups[1].Value : null;
            }
            catch { return null; }
        }

        private static void WritePlayerName(string name)
        {
            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(DayZCfgPath)!);
                string content = File.Exists(DayZCfgPath) ? File.ReadAllText(DayZCfgPath) : "";
                const string pattern = @"playerName\s*=\s*""[^""]*"";";
                string replacement = $"playerName = \"{name}\";";
                content = Regex.IsMatch(content, pattern)
                    ? Regex.Replace(content, pattern, replacement)
                    : content + $"\nplayerName = \"{name}\";\n";
                File.WriteAllText(DayZCfgPath, content);
            }
            catch { }
        }

        public static string FindDayZPath()
        {
            var paths = new[]
            {
                @"C:\Program Files (x86)\Steam\steamapps\common\DayZ",
                @"C:\Program Files\Steam\steamapps\common\DayZ",
                @"D:\Steam\steamapps\common\DayZ",
                @"D:\SteamLibrary\steamapps\common\DayZ",
                @"E:\Steam\steamapps\common\DayZ",
                @"E:\SteamLibrary\steamapps\common\DayZ",
            };
            foreach (var p in paths)
                if (File.Exists(Path.Combine(p, "DayZ.exe"))) return p;

            try
            {
                var key = Microsoft.Win32.Registry.LocalMachine
                    .OpenSubKey(@"SOFTWARE\WOW6432Node\Valve\Steam");
                if (key?.GetValue("InstallPath") is string steamPath)
                {
                    var vdf = Path.Combine(steamPath, "steamapps", "libraryfolders.vdf");
                    if (File.Exists(vdf))
                    {
                        foreach (Match m in Regex.Matches(File.ReadAllText(vdf), @"""path""\s+""([^""]+)"""))
                        {
                            var candidate = Path.Combine(m.Groups[1].Value, "steamapps", "common", "DayZ");
                            if (File.Exists(Path.Combine(candidate, "DayZ.exe"))) return candidate;
                        }
                    }
                }
            }
            catch { }
            return string.Empty;
        }
    }
}
