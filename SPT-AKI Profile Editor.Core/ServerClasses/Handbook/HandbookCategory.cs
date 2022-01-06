using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace SPT_AKI_Profile_Editor.Core.ServerClasses
{
    public class HandbookCategory
    {
        [JsonPropertyName("Id")]
        public string Id { get; set; }

        [JsonPropertyName("ParentId")]
        public string ParentId { get; set; }

        [JsonPropertyName("Icon")]
        public string Icon { get; set; }

        [JsonIgnore]
        public string LocalizedName =>
            AppData.ServerDatabase.LocalesGlobal.Handbook.ContainsKey(Id) ? AppData.ServerDatabase.LocalesGlobal.Handbook[Id] : Id;

        [JsonIgnore]
        public List<HandbookCategory> Categories =>
            AppData.ServerDatabase?.Handbook?.Categories?
            .Where(x => x.ParentId == Id && x.IsNotHidden)
            .ToList();

        [JsonIgnore]
        public List<TarkovItem> Items =>
            AppData.ServerDatabase?.Handbook?.Items?
            .Where(x => x.ParentId == Id)
            .Select(x => x.Item)
            .Where(x => x.CanBeAddedToStash)
            .ToList();

        [JsonIgnore]
        public bool IsNotHidden =>
            Items.Count > 0 || Categories.Any(y => y.Items.Count > 0);
    }
}