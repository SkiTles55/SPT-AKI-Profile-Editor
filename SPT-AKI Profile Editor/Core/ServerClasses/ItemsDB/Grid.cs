using System.Text.Json.Serialization;

namespace SPT_AKI_Profile_Editor.Core.ServerClasses
{
    public class Grid
    {
        [JsonPropertyName("_props")]
        public GridProps Props { get; set; }
    }
}