using Newtonsoft.Json;

namespace SPT_AKI_Profile_Editor.Core.ServerClasses.Hideout
{
    public class HideoutProduction
    {
        [JsonProperty("_id")]
        public string Id { get; set; }

        [JsonProperty("requirements")]
        public HideoutProductionRequirement[] Requirements { get; set; }
    }
}