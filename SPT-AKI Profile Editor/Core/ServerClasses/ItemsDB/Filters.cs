using System.Text.Json.Serialization;

namespace SPT_AKI_Profile_Editor.Core.ServerClasses
{
    public class Filters
    {
        [JsonPropertyName("Filter")]
        public string[] Filter { get; set; }

        [JsonPropertyName("ExcludedFilter")]
        public string[] ExcludedFilter { get; set; }
    }
}