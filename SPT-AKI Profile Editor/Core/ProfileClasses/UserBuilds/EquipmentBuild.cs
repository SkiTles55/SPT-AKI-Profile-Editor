using Newtonsoft.Json;
using SPT_AKI_Profile_Editor.Core.Equipment;
using System.Collections.Generic;
using System.Linq;

namespace SPT_AKI_Profile_Editor.Core.ProfileClasses
{
    public class EquipmentBuild : Build
    {
        [JsonConstructor]
        public EquipmentBuild(string id,
                     string name,
                     string root,
                     object[] items,
                     object[] fastPanel) : base(id, name, root, items)
        {
            FastPanel = fastPanel;
            BuildItems = items.Select(x => JsonConvert.DeserializeObject<InventoryItem>(x.ToString()));
        }

        [JsonProperty("BuildType")]
        public string Type { get; set; }

        [JsonProperty("fastPanel")]
        public object[] FastPanel { get; set; }

        [JsonIgnore]
        public List<EquipmentSlot> EquipmentSlots
            => EquipmentAdapter.GetEquipmentFromBuild(this,
                                                      AppData.ServerDatabase.LocalesGlobal,
                                                      AppData.AppLocalization,
                                                      AppData.AppSettings);

        public CharacterInventory MakeInventory()
            => new() { Items = [.. BuildItems] };
    }
}