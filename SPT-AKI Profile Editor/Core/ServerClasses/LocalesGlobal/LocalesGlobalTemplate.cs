using Newtonsoft.Json;

namespace SPT_AKI_Profile_Editor.Core.ServerClasses
{
    public class LocalesGlobalTemplate
    {
        [JsonProperty("Name")]
        public string Name { get; set; }

        [JsonProperty("ShortName")]
        public object ShortName { get; set; }

        [JsonProperty("Description")]
        public string Description { get; set; }
    }
}