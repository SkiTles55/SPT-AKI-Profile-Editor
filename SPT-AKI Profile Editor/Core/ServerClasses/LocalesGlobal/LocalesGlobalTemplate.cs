using System.Text.Json.Serialization;

namespace SPT_AKI_Profile_Editor.Core.ServerClasses
{
    public class LocalesGlobalTemplate
    {
        [JsonPropertyName("Name")]
        public string Name { get; set; }

        [JsonPropertyName("ShortName")]
        public object ShortName { get; set; }

        [JsonPropertyName("Description")]
        public string Description { get; set; }
    }
}