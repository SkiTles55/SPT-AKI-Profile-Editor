using Newtonsoft.Json;

namespace SPT_AKI_Profile_Editor.Core.ProfileClasses
{
    public class CharacterBonuses
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("templateId")]
        public string TemplateId { get; set; }
    }
}