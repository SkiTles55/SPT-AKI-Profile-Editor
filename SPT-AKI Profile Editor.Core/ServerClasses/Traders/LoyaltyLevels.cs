using System.Text.Json.Serialization;

namespace SPT_AKI_Profile_Editor.Core.ServerClasses
{
    [JsonNumberHandling(JsonNumberHandling.AllowReadingFromString)]
    public class LoyaltyLevels
    {
        [JsonPropertyName("minLevel")]
        public int MinLevel { get; set; }

        [JsonPropertyName("minSalesSum")]
        public long MinSalesSum { get; set; }

        [JsonPropertyName("minStanding")]
        public float MinStanding { get; set; }
    }
}