using System.Text.Json.Serialization;

namespace SPT_AKI_Profile_Editor.Core.ServerClasses
{
    public class LocalesGlobalTrading
    {
        [JsonPropertyName("Nickname")]
        public string Nickname { get; set; }
    }
}