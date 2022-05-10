using Newtonsoft.Json;

namespace SPT_AKI_Profile_Editor.Core.ProfileClasses
{
    public class Tag
    {
        [JsonProperty("Name")]
        public string Name { get; set; }
    }
}