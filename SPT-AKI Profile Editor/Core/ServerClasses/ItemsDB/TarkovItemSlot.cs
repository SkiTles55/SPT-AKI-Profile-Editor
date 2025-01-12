using Newtonsoft.Json;

namespace SPT_AKI_Profile_Editor.Core.ServerClasses.ItemsDB
{
    public class TarkovItemSlot
    {
        [JsonProperty("_name")]
        public string Name { get; set; }
    }
}