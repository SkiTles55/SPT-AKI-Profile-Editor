using System.Text.Json.Serialization;

namespace SPT_AKI_Profile_Editor.Core.ServerClasses
{
    public class GridProps
    {
        [JsonPropertyName("cellsH")]
        public int CellsH { get; set; }

        [JsonPropertyName("cellsV")]
        public int CellsV { get; set; }

        [JsonPropertyName("filters")]
        public Filters[] Filters { get; set; }
    }
}