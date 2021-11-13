using System.Text.Json.Serialization;

namespace SPT_AKI_Profile_Editor.Core.ProfileClasses
{
    public class CharacterQuest
    {
        [JsonPropertyName("qid")]
        public string Qid { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; }
    }
}