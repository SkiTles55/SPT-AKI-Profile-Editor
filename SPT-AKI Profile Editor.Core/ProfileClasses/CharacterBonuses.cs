using Newtonsoft.Json;

namespace SPT_AKI_Profile_Editor.Core.ProfileClasses
{
    public class CharacterBonuses
    {
        [JsonProperty("value")]
        public int Value { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("templateId")]
        public string TemplateId { get; set; }
    }
}