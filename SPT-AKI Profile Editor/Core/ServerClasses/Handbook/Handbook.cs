using Newtonsoft.Json;
using System.Collections.Generic;

namespace SPT_AKI_Profile_Editor.Core.ServerClasses
{
    public class Handbook
    {
        [JsonProperty("Categories")]
        public List<HandbookCategory> Categories { get; set; }

        [JsonProperty("Items")]
        public List<HandbookItem> Items { get; set; }
    }
}