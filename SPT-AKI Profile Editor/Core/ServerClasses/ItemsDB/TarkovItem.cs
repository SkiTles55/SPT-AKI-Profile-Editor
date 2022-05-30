using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

namespace SPT_AKI_Profile_Editor.Core.ServerClasses
{
    public class TarkovItem
    {
        [JsonConstructor]
        public TarkovItem(string id, TarkovItemProperties properties, string parent, string type)
        {
            Id = id;
            Properties = properties;
            Parent = parent;
            Type = type;
            SlotsCount = CalculateSlotsCount();
            CanBeAddedToStash = AppData.ServerDatabase.LocalesGlobal.Templates.ContainsKey(Id)
                && !Properties.QuestItem
                && !AppData.AppSettings.BannedItems.Contains(Parent)
                && !AppData.AppSettings.BannedItems.Contains(Id);
        }

        [JsonPropertyName("_id")]
        public string Id { get; set; }

        [JsonPropertyName("_props")]
        public TarkovItemProperties Properties { get; set; }

        [JsonPropertyName("_parent")]
        public string Parent { get; set; }

        [JsonPropertyName("_type")]
        public string Type { get; set; }

        [JsonIgnore]
        public bool CanBeAddedToStash { get; }

        [JsonIgnore]
        public int AddingQuantity { get; set; } = 1;

        [JsonIgnore]
        public bool AddingFir { get; set; } = false;

        [JsonIgnore]
        public string LocalizedName =>
            AppData.ServerDatabase.LocalesGlobal.Templates.ContainsKey(Id) ? AppData.ServerDatabase.LocalesGlobal.Templates[Id].Name : Id;

        [JsonIgnore]
        public int SlotsCount { get; }

        public bool CanBeAddedToContainer(TarkovItem container)
        {
            var filters = container.Properties?.Grids?.FirstOrDefault().Props?.Filters;
            if (filters == null || filters.Length == 0)
                return true;
            if (filters[0].ExcludedFilter.Contains(Parent))
                return false;
            if (filters[0].Filter.Length > 0)
            {
                List<string> parents = new() { Parent };
                while (AppData.ServerDatabase.ItemsDB.ContainsKey(parents.Last()))
                    parents.Add(AppData.ServerDatabase.ItemsDB[parents.Last()].Parent);
                return parents.Any(x => filters[0].Filter.Contains(x));
            }
            return true;
        }

        private int CalculateSlotsCount()
        {
            int slots = 0;
            if (Properties?.Grids != null && !Properties.Grids.Any(x => x.Props == null))
            {
                foreach (var grid in Properties.Grids)
                    slots += grid.Props.CellsH * grid.Props.CellsV;
            }
            return slots;
        }

        public static TarkovItem CopyFrom(TarkovItem item) => new(item.Id, item.Properties, item.Parent, item.Type);
    }
}