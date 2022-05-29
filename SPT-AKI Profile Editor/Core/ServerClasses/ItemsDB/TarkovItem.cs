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