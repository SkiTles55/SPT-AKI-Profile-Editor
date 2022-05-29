using Newtonsoft.Json;

namespace SPT_AKI_Profile_Editor.Core.ProfileClasses
{
    public class InventoryItem : BindableEntity
    {
        [JsonProperty("_id")]
        public string Id { get; set; }

        [JsonProperty("_tpl")]
        public string Tpl { get; set; }

        [JsonProperty("slotId")]
        public string SlotId { get; set; }

        [JsonConverter(typeof(LocationJsonConverter))]
        [JsonProperty("location")]
        public ItemLocation Location { get; set; }

        [JsonProperty("parentId")]
        public string ParentId { get; set; }

        [JsonProperty("upd")]
        public ItemUpd Upd { get; set; }

        [JsonIgnore]
        public string LocalizedName => (!string.IsNullOrEmpty(Tag) ? $"[{Tag}] " : string.Empty) + GlobalName + CountString;

        [JsonIgnore]
        public bool IsAddedByMods =>
            !AppData.ServerDatabase.ItemsDB.ContainsKey(Tpl);

        [JsonIgnore]
        public bool IsPockets => SlotId == AppData.AppSettings.PocketsSlotId;

        [JsonIgnore]
        public string Tag => Upd?.Tag?.Name;

        [JsonIgnore]
        public string CountString => (Upd?.StackObjectsCount ?? 1) > 1 ? $" [{Upd.StackObjectsCount}]" : string.Empty;

        [JsonIgnore]
        public string GlobalName =>
            AppData.ServerDatabase.LocalesGlobal.Templates.ContainsKey(Tpl) ? AppData.ServerDatabase.LocalesGlobal.Templates[Tpl].Name : Tpl;

        [JsonIgnore]
        public bool IsContainer => AppData.ServerDatabase.ItemsDB.ContainsKey(Tpl) && AppData.ServerDatabase.ItemsDB[Tpl].Properties?.Grids?.Length > 0;

        [JsonIgnore]
        public bool IsWeapon => AppData.ServerDatabase.ItemsDB.ContainsKey(Tpl) && AppData.ServerDatabase.ItemsDB[Tpl].Properties?.RecoilForceUp != 0;

        public static InventoryItem CopyFrom(InventoryItem item)
        {
            InventoryItem copy = new()
            {
                Id = item.Id,
                Tpl = item.Tpl,
                SlotId = item.SlotId,
                Location = item.Location,
                ParentId = item.ParentId,
                Upd = item.Upd
            };
            return copy;
        }
    }
}