using System.Text.Json.Serialization;

namespace SPT_AKI_Profile_Editor.Core.ServerClasses
{
    public class LoyaltyLevels
    {
        [JsonPropertyName("minLevel")]
        public object MinLevel { get; set; } //WTF SPT-AKI? why string in minLevel????
        [JsonPropertyName("minSalesSum")]
        public object MinSalesSum { get; set; }
        [JsonPropertyName("minStanding")]
        public object MinStanding { get; set; }
    }
}