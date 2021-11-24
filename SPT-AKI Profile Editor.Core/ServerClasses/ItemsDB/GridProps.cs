using Newtonsoft.Json;

namespace SPT_AKI_Profile_Editor.Core.ServerClasses
{
    public class GridProps
    {
        [JsonProperty("cellsH")]
        public int CellsH { get; set; }
        [JsonProperty("cellsV")]
        public int CellsV { get; set; }
    }
}