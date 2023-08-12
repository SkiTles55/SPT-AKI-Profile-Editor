using Newtonsoft.Json;

namespace SPT_AKI_Profile_Editor.ModHelper
{
    public class ModPackageInfo
    {
        [JsonProperty("version")]
        public string Version { get; set; }
    }
}