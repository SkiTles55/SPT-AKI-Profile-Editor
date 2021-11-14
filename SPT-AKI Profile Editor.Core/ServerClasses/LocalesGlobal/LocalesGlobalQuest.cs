using System.Text.Json.Serialization;

namespace SPT_AKI_Profile_Editor.Core.ServerClasses
{
    public class LocalesGlobalQuest
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }
    }
}