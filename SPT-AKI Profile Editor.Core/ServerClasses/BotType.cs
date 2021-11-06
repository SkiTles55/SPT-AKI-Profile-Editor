using System.Text.Json.Serialization;

namespace SPT_AKI_Profile_Editor.Core.ServerClasses
{
    class BotType
    {
        [JsonPropertyName("appearance")]
        public Appearance Appearance { get; set; }
    }
}