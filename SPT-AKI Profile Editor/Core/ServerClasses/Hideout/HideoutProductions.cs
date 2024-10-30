using Newtonsoft.Json;

namespace SPT_AKI_Profile_Editor.Core.ServerClasses.Hideout
{
    public class HideoutProductions
    {
        [JsonProperty("recipes")]
        public HideoutProduction[] Recipes { get; set; }
    }
}