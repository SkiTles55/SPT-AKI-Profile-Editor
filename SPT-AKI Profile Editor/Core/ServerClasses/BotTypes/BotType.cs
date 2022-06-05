using Newtonsoft.Json;

namespace SPT_AKI_Profile_Editor.Core.ServerClasses
{
    internal class BotType
    {
        [JsonProperty("appearance")]
        public BotTypeAppearance Appearance { get; set; }
    }
}