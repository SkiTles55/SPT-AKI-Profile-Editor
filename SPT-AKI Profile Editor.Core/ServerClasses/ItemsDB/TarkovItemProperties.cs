using System.Text.Json.Serialization;

namespace SPT_AKI_Profile_Editor.Core.ServerClasses
{
    public class TarkovItemProperties
    {
        [JsonPropertyName("Grids")]
        public Grid[] Grids { get; set; }
        [JsonPropertyName("ExaminedByDefault")]
        public bool ExaminedByDefault { get; set; }
    }
}