using System.Text.Json.Serialization;

namespace SPT_AKI_Profile_Editor.Core.ServerClasses
{
    public class Mastering
    {
        [JsonPropertyName("Name")]
        public string Name { get; set; }

        [JsonPropertyName("Templates")]
        public string[] Templates { get; set; }

        [JsonPropertyName("Level2")]
        public int Level2 { get; set; }

        [JsonPropertyName("Level3")]
        public int Level3 { get; set; }
    }
}