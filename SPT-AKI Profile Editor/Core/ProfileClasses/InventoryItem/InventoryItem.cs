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
        public string LocalizedName => (!string.IsNullOrEmpty(Tag) ? $"[{Tag}] " : string.Empty) + GlobalName;

        [JsonIgnore]
        public bool IsAddedByMods =>
            !AppData.ServerDatabase.ItemsDB.ContainsKey(Tpl);

        [JsonIgnore]
        public bool IsPockets => SlotId == AppData.AppSettings.PocketsSlotId;

        [JsonIgnore]
        public string Tag => Upd?.Tag?.Name;

        [JsonIgnore]
        public string GlobalName =>
            AppData.ServerDatabase.LocalesGlobal.Templates.ContainsKey(Tpl) ? AppData.ServerDatabase.LocalesGlobal.Templates[Tpl].Name : Tpl;
    }
}