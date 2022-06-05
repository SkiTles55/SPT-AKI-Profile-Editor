using System.Collections.Generic;
using Newtonsoft.Json;

namespace SPT_AKI_Profile_Editor.Core.ServerClasses
{
    public class TraderBase : BindableEntity
    {
        [JsonProperty("_id")]
        public string Id { get; set; }

        [JsonProperty("loyaltyLevels")]
        public List<LoyaltyLevels> LoyaltyLevels { get; set; }
    }
}