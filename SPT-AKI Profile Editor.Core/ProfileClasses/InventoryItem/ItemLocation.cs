using Newtonsoft.Json;

namespace SPT_AKI_Profile_Editor.Core.ProfileClasses
{
    public class ItemLocation
    {
        [JsonIgnore]
        public int? SimpleNumber { get; set; } = null;

        [JsonProperty("x")]
        public int X { get; set; }

        [JsonProperty("y")]
        public int Y { get; set; }

        [JsonProperty("r")]
        public string R { get; set; }

        [JsonProperty("isSearched")]
        public bool? IsSearched { get; set; }
    }
}