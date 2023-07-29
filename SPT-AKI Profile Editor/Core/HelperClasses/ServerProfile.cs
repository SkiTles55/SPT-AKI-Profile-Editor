using Newtonsoft.Json;

namespace SPT_AKI_Profile_Editor.Core.HelperClasses
{
    internal class ServerProfile
    {
        [JsonProperty("characters")]
        public Characters Characters { get; set; }

        public override string ToString() => $"{Characters?.Pmc?.Info?.Nickname} ({Characters?.Pmc?.Info?.Side} {Characters?.Pmc?.Info?.Level} lvl)";
    }

    internal class Characters
    {
        [JsonProperty("pmc")]
        public Pmc Pmc { get; set; }
    }

    internal class Pmc
    {
        public Info Info { get; set; }
    }

    internal class Info
    {
        public string Nickname { get; set; }

        public string Side { get; set; }

        public int Level { get; set; }
    }
}