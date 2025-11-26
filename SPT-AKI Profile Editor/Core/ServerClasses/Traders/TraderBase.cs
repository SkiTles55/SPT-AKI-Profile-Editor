using Newtonsoft.Json;
using SPT_AKI_Profile_Editor.Core.HelperClasses;
using System.Collections.Generic;

namespace SPT_AKI_Profile_Editor.Core.ServerClasses
{
    public class TraderBase : BindableEntity
    {
        [JsonProperty("_id")]
        public string Id { get; set; }

        [JsonProperty("avatar")]
        public string ImageUrl { get; set; }

        [JsonProperty("loyaltyLevels")]
        public List<LoyaltyLevels> LoyaltyLevels { get; set; }

        [JsonProperty("nickname")]
        public string Nickname { get; set; }
    }
}