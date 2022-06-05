using Newtonsoft.Json;

namespace SPT_AKI_Profile_Editor.Core.ServerClasses
{
    public class LocalesGlobalTrading
    {
        [JsonProperty("Nickname")]
        public string Nickname { get; set; }
    }
}