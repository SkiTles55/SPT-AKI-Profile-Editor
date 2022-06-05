using Newtonsoft.Json;

namespace SPT_AKI_Profile_Editor.Core.ServerClasses
{
    internal class BotTypeAppearance
    {
        [JsonProperty("head")]
        public string[] Heads { get; set; }

        [JsonProperty("voice")]
        public string[] Voices { get; set; }
    }
}