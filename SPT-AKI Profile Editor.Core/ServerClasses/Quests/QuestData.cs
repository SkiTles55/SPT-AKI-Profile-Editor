using System.Text.Json.Serialization;

namespace SPT_AKI_Profile_Editor.Core.ServerClasses
{
    public class QuestData
    {
        [JsonPropertyName("_id")]
        public string Id { get; set; }
        [JsonPropertyName("traderId")]
        public string TraderId { get; set; }
    }
}