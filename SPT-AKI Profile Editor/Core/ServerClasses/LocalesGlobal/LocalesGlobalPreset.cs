using Newtonsoft.Json;

namespace SPT_AKI_Profile_Editor.Core.ServerClasses
{
    public class LocalesGlobalPreset
    {
        [JsonProperty("Name")]
        public string Name { get; set; }
    }
}