using System;
using System.Windows;
using System.Windows.Media;
using LauncherZ.Models;

namespace LauncherZ.Services
{
    public class ThemeService
    {
        public void Apply(ThemeDefinition t)
        {
            var d = Application.Current.Resources;
            Set(d, "BgRoot",       t.BgRoot);
            Set(d, "BgPanel",      t.BgPanel);
            Set(d, "BgRaised",     t.BgRaised);
            Set(d, "BgHover",      t.BgHover);
            Set(d, "BgSelected",   t.BgSelected);
            Set(d, "Border",       t.Border);
            Set(d, "BorderLit",    t.BorderLit);
            Set(d, "Accent",       t.Accent);
            Set(d, "AccentDim",    Alpha(t.Accent, 0.14));
            Set(d, "ConnectBtn",   t.ConnectBtn);
            Set(d, "ConnectBtnH",  Lighten(t.ConnectBtn, 20));
            Set(d, "TextPrimary",  t.TextPrimary);
            Set(d, "TextSecond",   t.TextSecondary);
            Set(d, "TextDim",      t.TextDim);
            Set(d, "TextVeryDim",  Lighten(t.TextDim, -20));
            Set(d, "TabActiveBg",  t.TabActiveBg);
            Set(d, "TabActiveTxt", t.TabActiveText);
            Set(d, "TabInactBg",   t.TabInactiveBg);
            Set(d, "TabInactTxt",  t.TabInactiveText);
            Set(d, "PingGood",     t.PingGood);
            Set(d, "PingMid",      t.PingMid);
            Set(d, "PingBad",      t.PingBad);
            Set(d, "QueueColor",   t.QueueColor);
            Set(d, "TagOffTxt",    t.TagOfficialText);
            Set(d, "TagVanTxt",    t.TagVanillaText);
            Set(d, "TagModTxt",    t.TagModdedText);
            Set(d, "TagOffBg",     Alpha(t.TagOfficialText, 0.12));
            Set(d, "TagVanBg",     Alpha(t.TagVanillaText,  0.12));
            Set(d, "TagModBg",     Alpha(t.TagModdedText,   0.12));
            Set(d, "TagOffBdr",    Alpha(t.TagOfficialText, 0.35));
            Set(d, "TagVanBdr",    Alpha(t.TagVanillaText,  0.35));
            Set(d, "TagModBdr",    Alpha(t.TagModdedText,   0.35));
        }

        private static void Set(ResourceDictionary d, string key, Color c)
        {
            d[key] = c;
            d[key + "Brush"] = new SolidColorBrush(c);
        }

        private static void Set(ResourceDictionary d, string key, string hex) =>
            Set(d, key, Parse(hex));

        private static Color Parse(string hex)
        {
            hex = hex.TrimStart('#');
            if (hex.Length == 6)
                return Color.FromRgb(
                    Convert.ToByte(hex[0..2], 16),
                    Convert.ToByte(hex[2..4], 16),
                    Convert.ToByte(hex[4..6], 16));
            return Colors.Transparent;
        }

        private static Color Alpha(string hex, double a)
        {
            var c = Parse(hex);
            return Color.FromArgb((byte)(a * 255), c.R, c.G, c.B);
        }

        private static string Lighten(string hex, int amt)
        {
            var c = Parse(hex);
            int r = Math.Clamp(c.R + amt, 0, 255);
            int g = Math.Clamp(c.G + amt, 0, 255);
            int b = Math.Clamp(c.B + amt, 0, 255);
            return $"#{r:X2}{g:X2}{b:X2}";
        }
    }
}
