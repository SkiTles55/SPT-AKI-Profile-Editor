using System.Text.Json.Serialization;

namespace SPT_AKI_Profile_Editor.Core.ServerClasses
{
    class BotTypeAppearance
    {
        [JsonPropertyName("head")]
        public string[] Heads { get; set; }
        [JsonPropertyName("voice")]
        public string[] Voices { get; set; }
    }
}