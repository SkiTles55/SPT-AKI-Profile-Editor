using Newtonsoft.Json;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace SPT_AKI_Profile_Editor.Core.ServerClasses
{
    public class Handbook
    {
        [JsonProperty("Categories")]
        public List<HandbookCategory> Categories { get; set; }

        [JsonProperty("Items")]
        public List<HandbookItem> Items { get; set; }

        [JsonIgnore]
        public ObservableCollection<HandbookCategory> CategoriesForItemsAdding => Categories != null ? new(Categories
                    .Where(x => string.IsNullOrEmpty(x.ParentId) && x.IsNotHidden)) : new();
    }
}