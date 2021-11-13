using System.Text.Json.Serialization;

namespace SPT_AKI_Profile_Editor.Core.ProfileClasses
{
    public class CharacterTraderStandings
    {
        [JsonPropertyName("loyaltyLevel")]
        public int LoyaltyLevel { get; set; }
        [JsonPropertyName("salesSum")]
        public long SalesSum { get; set; }
        [JsonPropertyName("standing")]
        public float Standing { get; set; }
        [JsonPropertyName("unlocked")]
        public bool Unlocked { get; set; }
    }
}