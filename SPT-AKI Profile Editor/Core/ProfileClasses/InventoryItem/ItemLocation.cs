using Newtonsoft.Json;
using SPT_AKI_Profile_Editor.Core.Enums;

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
        public ItemRotation R { get; set; }

        [JsonProperty("isSearched")]
        public bool? IsSearched { get; set; }
    }
}