using Newtonsoft.Json;

namespace SPT_AKI_Profile_Editor.Core.ProfileClasses
{
    public class ItemUpd
    {
        [JsonProperty("StackObjectsCount")]
        public long? StackObjectsCount { get; set; }
    }
}