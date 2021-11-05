using System.Text.Json.Serialization;

namespace SPT_AKI_Profile_Editor.Core
{
    class ServerProfile
    {
        [JsonPropertyName("characters")]
        public Characters Characters { get; set; }
        public override string ToString() => $"{Characters?.Pmc?.Info?.Nickname} ({Characters?.Pmc?.Info?.Side} {Characters?.Pmc?.Info?.Level} lvl)";
    }
    class Characters
    {
        [JsonPropertyName("pmc")]
        public Pmc Pmc { get; set; }
    }
    class Pmc
    {
        [JsonPropertyName("aid")]
        public string Aid { get; set; }
        [JsonPropertyName("Info")]
        public Info Info { get; set; }
    }
    class Info
    {
        [JsonPropertyName("Nickname")]
        public string Nickname { get; set; }
        [JsonPropertyName("Side")]
        public string Side { get; set; }
        [JsonPropertyName("Level")]
        public int Level { get; set; }
    }
}