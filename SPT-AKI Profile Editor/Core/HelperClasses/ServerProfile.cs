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
        [JsonProperty("Info")]
        public Info Info { get; set; }
    }

    internal class Info
    {
        [JsonProperty("Nickname")]
        public string Nickname { get; set; }

        [JsonProperty("Side")]
        public string Side { get; set; }

        [JsonProperty("Level")]
        public int Level { get; set; }
    }
}