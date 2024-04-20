using Newtonsoft.Json;

#nullable enable

namespace SPT_AKI_Profile_Editor.Core.ProfileClasses
{
    public class CharacterBonus
    {
        public static readonly string StashRowsType = "StashRows";

        [JsonProperty("id")]
        public string? Id { get; set; }

        [JsonProperty("type")]
        public string? Type { get; set; }

        [JsonProperty("value")]
        public float? Value { get; set; }

        [JsonProperty("templateId")]
        public string? TemplateId { get; set; }

        [JsonProperty("passive")]
        public bool? Passive { get; set; }

        [JsonProperty("visible")]
        public bool? Visible { get; set; }

        [JsonProperty("production")]
        public bool? Production { get; set; }

        public static CharacterBonus CreateStashRowsBonus(int rows) => new()
        {
            Id = "fac7b58e9883cd4945a8d5b0",
            Value = rows,
            Type = StashRowsType,
            Passive = true,
            Visible = true,
            Production = false
        };
    }
}