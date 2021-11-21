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
        public string LocalizedName =>
            AppData.ServerDatabase.LocalesGlobal.Templates.ContainsKey(Id) ? AppData.ServerDatabase.LocalesGlobal.Templates[Id].Name : Id;

        [JsonIgnore]
        public int GetSlotsCount
        {
            get
            {
                int slots = 0;
                //foreach (var grid in Properties.Grids)
                //    slots += grid.gridProps.cellsH * grid.gridProps.cellsV;
                return slots;
            }
        }
    }
}