using Newtonsoft.Json;

namespace SPT_AKI_Profile_Editor.Core.ServerClasses
{
    public class Filters
    {
        [JsonProperty("Filter")]
        public string[] Filter { get; set; }

        [JsonProperty("ExcludedFilter")]
        public string[] ExcludedFilter { get; set; }
    }
}