using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace SPT_AKI_Profile_Editor.Core.ServerClasses
{
    public class Handbook
    {
        [JsonPropertyName("Categories")]
        public List<HandbookCategory> Categories { get; set; }

        [JsonPropertyName("Items")]
        public List<HandbookItem> Items { get; set; }

        [JsonIgnore]
        public List<HandbookCategory> CategoriesForItemsAdding =>
            Categories?
            .Where(x => string.IsNullOrEmpty(x.ParentId))
            .ToList();
    }
}