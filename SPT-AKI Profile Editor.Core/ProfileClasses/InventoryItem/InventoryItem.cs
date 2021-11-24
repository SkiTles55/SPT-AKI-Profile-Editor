using Newtonsoft.Json;

namespace SPT_AKI_Profile_Editor.Core.ProfileClasses
{
    public class InventoryItem
    {
        [JsonProperty("_id")]
        public string Id { get; set; }

        [JsonProperty("_tpl")]
        public string Tpl { get; set; }

        [JsonProperty("slotId")]
        public string SlotId { get; set; }

        [JsonProperty("parentId")]
        public string ParentId { get; set; }

        [JsonProperty("upd")]
        public ItemUpd Upd { get; set; }

        [JsonIgnore]
        public string LocalizedName =>
            AppData.ServerDatabase.LocalesGlobal.Templates.ContainsKey(Tpl) ? AppData.ServerDatabase.LocalesGlobal.Templates[Tpl].Name : Tpl;
    }
}