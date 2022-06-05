using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace SPT_AKI_Profile_Editor.Core.ServerClasses
{
    public class Handbook
    {
        [JsonProperty("Categories")]
        public List<HandbookCategory> Categories { get; set; }

        [JsonProperty("Items")]
        public List<HandbookItem> Items { get; set; }

        [JsonIgnore]
        public IEnumerable<HandbookCategory> CategoriesForItemsAdding => Categories
                    .Where(x => string.IsNullOrEmpty(x.ParentId) && x.IsNotHidden);
    }
}