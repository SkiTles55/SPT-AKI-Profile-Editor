using Newtonsoft.Json;

namespace SPT_AKI_Profile_Editor.Core.ServerClasses
{
    public class Mastering
    {
        [JsonProperty("Name")]
        public string Name { get; set; }

        [JsonProperty("Templates")]
        public string[] Templates { get; set; }

        [JsonProperty("Level2")]
        public int Level2 { get; set; }

        [JsonProperty("Level3")]
        public int Level3 { get; set; }
    }
}