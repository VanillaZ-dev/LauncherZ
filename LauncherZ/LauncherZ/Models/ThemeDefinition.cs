namespace LauncherZ.Models
{
    public class ThemeDefinition
    {
        public string Id { get; set; } = string.Empty;
        public string Label { get; set; } = string.Empty;
        public bool IsDark { get; set; } = true;
        public bool IsCustom { get; set; }

        public string BgRoot { get; set; } = "#0d1210";
        public string BgPanel { get; set; } = "#161e1a";
        public string BgRaised { get; set; } = "#1c2822";
        public string BgHover { get; set; } = "#20302a";
        public string BgSelected { get; set; } = "#152218";
        public string Border { get; set; } = "#233028";
        public string BorderLit { get; set; } = "#2e4038";
        public string Accent { get; set; } = "#39ff6a";
        public string ConnectBtn { get; set; } = "#bf5936";
        public string TextPrimary { get; set; } = "#e8e2da";
        public string TextSecondary { get; set; } = "#a8b8b0";
        public string TextDim { get; set; } = "#607068";
        public string TabActiveBg { get; set; } = "#253c2e";
        public string TabActiveText { get; set; } = "#ffffff";
        public string TabInactiveBg { get; set; } = "#1a2620";
        public string TabInactiveText { get; set; } = "#a0b4a8";
        public string PingGood { get; set; } = "#38a858";
        public string PingMid { get; set; } = "#b89030";
        public string PingBad { get; set; } = "#c04040";
        public string QueueColor { get; set; } = "#22d4e8";
        public string TagOfficialText { get; set; } = "#7ac4e8";
        public string TagVanillaText { get; set; } = "#5edd80";
        public string TagModdedText { get; set; } = "#b09ae0";

        public static ThemeDefinition[] Presets => new[]
        {
            new ThemeDefinition
            {
                Id="forest", Label="Forest", IsDark=true,
                BgRoot="#0d1210", BgPanel="#161e1a", BgRaised="#1c2822", BgHover="#20302a",
                BgSelected="#152218", Border="#233028", BorderLit="#2e4038",
                Accent="#39ff6a", ConnectBtn="#bf5936",
                TextPrimary="#e8e2da", TextSecondary="#a8b8b0", TextDim="#607068",
                TabActiveBg="#253c2e", TabActiveText="#ffffff", TabInactiveBg="#1a2620", TabInactiveText="#a0b4a8",
                PingGood="#38a858", PingMid="#b89030", PingBad="#c04040", QueueColor="#22d4e8",
                TagOfficialText="#7ac4e8", TagVanillaText="#5edd80", TagModdedText="#b09ae0"
            },
            new ThemeDefinition
            {
                Id="blood", Label="Blood", IsDark=true,
                BgRoot="#100808", BgPanel="#1c1010", BgRaised="#241414", BgHover="#2c1818",
                BgSelected="#241010", Border="#3a1818", BorderLit="#502020",
                Accent="#ff3a3a", ConnectBtn="#cc3030",
                TextPrimary="#ecdada", TextSecondary="#b09090", TextDim="#6a4848",
                TabActiveBg="#3a1010", TabActiveText="#ff8080", TabInactiveBg="#200e0e", TabInactiveText="#9a7070",
                PingGood="#3a8838", PingMid="#a87828", PingBad="#cc2020", QueueColor="#cc5050",
                TagOfficialText="#e88080", TagVanillaText="#70d090", TagModdedText="#e89090"
            },
            new ThemeDefinition
            {
                Id="slate", Label="Slate", IsDark=true,
                BgRoot="#0c0e12", BgPanel="#141824", BgRaised="#1a2030", BgHover="#1e2838",
                BgSelected="#141e30", Border="#242c40", BorderLit="#303c54",
                Accent="#4a9eff", ConnectBtn="#3a70cc",
                TextPrimary="#d4dcea", TextSecondary="#8898b4", TextDim="#4a5870",
                TabActiveBg="#1e3060", TabActiveText="#ffffff", TabInactiveBg="#141c2c", TabInactiveText="#6a7a98",
                PingGood="#3a9858", PingMid="#a88828", PingBad="#cc3030", QueueColor="#22d4e8",
                TagOfficialText="#7ac4e8", TagVanillaText="#5edd80", TagModdedText="#b09ae0"
            },
            new ThemeDefinition
            {
                Id="midnight", Label="Midnight", IsDark=true,
                BgRoot="#08080e", BgPanel="#10101e", BgRaised="#161626", BgHover="#1c1c2e",
                BgSelected="#101020", Border="#20203a", BorderLit="#2c2c50",
                Accent="#a060ff", ConnectBtn="#7840cc",
                TextPrimary="#dcd4f0", TextSecondary="#9888c0", TextDim="#585078",
                TabActiveBg="#2a1860", TabActiveText="#ffffff", TabInactiveBg="#10102a", TabInactiveText="#6a6090",
                PingGood="#3a9858", PingMid="#a88828", PingBad="#cc3030", QueueColor="#40c0e0",
                TagOfficialText="#90c0f0", TagVanillaText="#70e090", TagModdedText="#c090ff"
            },
            new ThemeDefinition
            {
                Id="field", Label="Field", IsDark=false,
                BgRoot="#e8ede6", BgPanel="#d4ddd0", BgRaised="#c8d4c4", BgHover="#bfcebb",
                BgSelected="#b8d4b0", Border="#aabaa4", BorderLit="#90a88a",
                Accent="#1a8f3a", ConnectBtn="#a04428",
                TextPrimary="#1a2418", TextSecondary="#3a5040", TextDim="#608060",
                TabActiveBg="#1a8f3a", TabActiveText="#ffffff", TabInactiveBg="#c4d4be", TabInactiveText="#3a5040",
                PingGood="#1a7a30", PingMid="#8a6010", PingBad="#9a2020", QueueColor="#0a9aaa",
                TagOfficialText="#2870b0", TagVanillaText="#1a6a30", TagModdedText="#7040b0"
            },
            new ThemeDefinition
            {
                Id="arctic", Label="Arctic", IsDark=false,
                BgRoot="#eef2f8", BgPanel="#d8e0f0", BgRaised="#ccd6ec", BgHover="#c0cce4",
                BgSelected="#b0c8e8", Border="#a0b4d4", BorderLit="#8898c0",
                Accent="#1a6abf", ConnectBtn="#9a3a28",
                TextPrimary="#0e1828", TextSecondary="#284060", TextDim="#506888",
                TabActiveBg="#1a6abf", TabActiveText="#ffffff", TabInactiveBg="#c8d4e8", TabInactiveText="#284060",
                PingGood="#1a7838", PingMid="#8a6010", PingBad="#9a2020", QueueColor="#0a7aaa",
                TagOfficialText="#1a6abf", TagVanillaText="#1a7a38", TagModdedText="#7040b0"
            },
            new ThemeDefinition
            {
                Id="custom", Label="Custom", IsDark=true, IsCustom=true
            }
        };
    }
}
