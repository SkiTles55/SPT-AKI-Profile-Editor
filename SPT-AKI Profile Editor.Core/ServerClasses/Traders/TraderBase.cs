using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace SPT_AKI_Profile_Editor.Core.ServerClasses
{
    public class TraderBase
    {
        [JsonPropertyName("loyaltyLevels")]
        public List<LoyaltyLevels> LoyaltyLevels { get; set; }
    }
}