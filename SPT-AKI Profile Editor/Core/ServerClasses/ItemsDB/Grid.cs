using Newtonsoft.Json;

namespace SPT_AKI_Profile_Editor.Core.ServerClasses
{
    public class Grid
    {
        [JsonProperty("_props")]
        public GridProps Props { get; set; }
    }
}