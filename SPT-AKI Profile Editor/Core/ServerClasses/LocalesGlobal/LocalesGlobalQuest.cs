using Newtonsoft.Json;

namespace SPT_AKI_Profile_Editor.Core.ServerClasses
{
    public class LocalesGlobalQuest
    {
        [JsonProperty("name")]
        public string Name { get; set; }
    }
}