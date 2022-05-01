using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace SPT_AKI_Profile_Editor.Core.ServerClasses
{
    public class TraderBase : BindableEntity
    {
        [JsonPropertyName("_id")]
        public string Id { get; set; }

        [JsonPropertyName("loyaltyLevels")]
        public List<LoyaltyLevels> LoyaltyLevels { get; set; }
    }
}