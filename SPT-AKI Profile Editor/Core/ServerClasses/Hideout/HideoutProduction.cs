using Newtonsoft.Json;
using System.Linq;

namespace SPT_AKI_Profile_Editor.Core.ServerClasses.Hideout
{
    public class HideoutProduction
    {
        [JsonProperty("_id")]
        public string Id { get; set; }

        [JsonProperty("areaType")]
        public int AreaType { get; set; }

        [JsonProperty("requirements")]
        public HideoutProductionRequirement[] Requirements { get; set; }

        [JsonProperty("locked")]
        public bool Locked { get; set; }

        [JsonProperty("endProduct")]
        public string EndProduct { get; set; }

        [JsonIgnore]
        public bool UnlocksByQuest
            => Locked && Requirements.Any(x => x.Type == RequirementType.QuestComplete);
    }
}