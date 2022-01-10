using System.Text.Json.Serialization;

namespace SPT_AKI_Profile_Editor.Core.ServerClasses
{
    public class HandbookCategory
    {
        [JsonPropertyName("Id")]
        public string Id { get; set; }

        [JsonPropertyName("ParentId")]
        public string ParentId { get; set; }

        [JsonPropertyName("Icon")]
        public string Icon { get; set; }
    }
}