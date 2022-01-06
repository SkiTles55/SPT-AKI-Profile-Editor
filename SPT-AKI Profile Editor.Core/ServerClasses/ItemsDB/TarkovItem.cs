using System.Linq;
using System.Text.Json.Serialization;

namespace SPT_AKI_Profile_Editor.Core.ServerClasses
{
    public class TarkovItem
    {
        [JsonPropertyName("_id")]
        public string Id { get; set; }

        [JsonPropertyName("_props")]
        public TarkovItemProperties Properties { get; set; }

        [JsonPropertyName("_parent")]
        public string Parent { get; set; }

        [JsonPropertyName("_type")]
        public string Type { get; set; }

        [JsonIgnore]
        public bool CanBeAddedToStash =>
            AppData.ServerDatabase.LocalesGlobal.Templates.ContainsKey(Parent)
            && !Properties.QuestItem
            && !AppData.AppSettings.BannedItems.Contains(Parent)
            && !AppData.AppSettings.BannedItems.Contains(Id);

        [JsonIgnore]
        public int AddingQuantity { get; set; } = 1;

        [JsonIgnore]
        public bool AddingFir { get; set; } = false;

        [JsonIgnore]
        public string LocalizedName =>
            AppData.ServerDatabase.LocalesGlobal.Templates.ContainsKey(Id) ? AppData.ServerDatabase.LocalesGlobal.Templates[Id].Name : Id;

        [JsonIgnore]
        public string LocalizedGroupName =>
            AppData.ServerDatabase.LocalesGlobal.Templates.ContainsKey(AppData.ServerDatabase.ItemsDB[Parent].Parent)
            ? AppData.ServerDatabase.LocalesGlobal.Templates[AppData.ServerDatabase.ItemsDB[Parent].Parent].Name
            : AppData.ServerDatabase.ItemsDB[Parent].Parent;
        
        [JsonIgnore]
        public string LocalizedSubGroupName =>
            AppData.ServerDatabase.LocalesGlobal.Templates.ContainsKey(Parent) ? AppData.ServerDatabase.LocalesGlobal.Templates[Parent].Name : Parent;

        [JsonIgnore]
        public int GetSlotsCount => CalculateSlotsCount();

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
    }
}