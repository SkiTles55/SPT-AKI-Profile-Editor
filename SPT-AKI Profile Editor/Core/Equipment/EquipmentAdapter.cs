using SPT_AKI_Profile_Editor.Core.ProfileClasses;
using System.Collections.Generic;
using System.Linq;

namespace SPT_AKI_Profile_Editor.Core.Equipment
{
    public class EquipmentAdapter
    {
        public static List<EquipmentSlot> GetCharacterEquipment(CharacterInventory inventory,
                                                                string equipmentSlot,
                                                                Dictionary<string, string> localesGlobal,
                                                                AppLocalization localization,
                                                                AppSettings appSettings)
        {
            var emptySlotItemText = localization?.GetLocalizedString("tab_stash_empty_slot");
            return new()
            {
                new EquipmentSlotItem(GetSlotName("Earpiece", localesGlobal),
                                      GetEquipment(appSettings.EarpieceSlotId, inventory.Items, equipmentSlot),
                                      emptySlotItemText),
                new EquipmentSlotItem(GetSlotName("Headwear", localesGlobal),
                                      GetEquipment(appSettings.HeadwearSlotId, inventory.Items, equipmentSlot),
                                      emptySlotItemText),
                new EquipmentSlotItem(GetSlotName("FaceCover", localesGlobal),
                                      GetEquipment(appSettings.FaceCoverSlotId, inventory.Items, equipmentSlot),
                                      emptySlotItemText),
                new EquipmentSlotItem(GetSlotName("Eyewear", localesGlobal),
                                      GetEquipment(appSettings.EyewearSlotId, inventory.Items, equipmentSlot),
                                      emptySlotItemText),
                new EquipmentSlotItem(GetSlotName("Armband", localesGlobal),
                                      GetEquipment(appSettings.ArmBandSlotId, inventory.Items, equipmentSlot),
                                      emptySlotItemText),
                new EquipmentSlotItem(GetSlotName("ArmorVest", localesGlobal),
                                      GetEquipment(appSettings.ArmorVestSlotId, inventory.Items, equipmentSlot),
                                      emptySlotItemText),
                new EquipmentSlotItem(GetSlotName("TacticalVest", localesGlobal),
                                      GetEquipment(appSettings.TacticalVestSlotId, inventory.Items, equipmentSlot),
                                      emptySlotItemText),
                new EquipmentSlotItem(GetSlotName("PrimaryWeaponFirst", localesGlobal),
                                      GetEquipment(appSettings.FirstPrimaryWeaponSlotId, inventory.Items, equipmentSlot),
                                      emptySlotItemText),
                new EquipmentSlotItem(GetSlotName("PrimaryWeaponSecond", localesGlobal),
                                      GetEquipment(appSettings.SecondPrimaryWeaponSlotId, inventory.Items, equipmentSlot),
                                      emptySlotItemText),
                new EquipmentSlotItem(GetSlotName("Holster", localesGlobal),
                                      GetEquipment(appSettings.HolsterSlotId, inventory.Items, equipmentSlot),
                                      emptySlotItemText),
                new EquipmentSlotItem(GetSlotName("Scabbard", localesGlobal),
                                      GetEquipment(appSettings.ScabbardSlotId, inventory.Items, equipmentSlot),
                                      emptySlotItemText),
                new EquipmentSlotPockets(GetSlotName("Pockets", localesGlobal), inventory?.PocketsItems),
                new EquipmentSlotItem(GetSlotName("Backpack", localesGlobal),
                                      GetEquipment(appSettings.BackpackSlotId, inventory.Items, equipmentSlot),
                                      emptySlotItemText),
                new EquipmentSlotItem(GetSlotName("SecuredContainer", localesGlobal),
                                      GetEquipment(appSettings.SecuredContainerSlotId, inventory.Items, equipmentSlot),
                                      emptySlotItemText)
            };
        }

        private static string GetSlotName(string key, Dictionary<string, string> localesGlobal)
            => localesGlobal?.ContainsKey(key) == true ? localesGlobal[key] : key;

        private static InventoryItem GetEquipment(string slotId,
                                                  IEnumerable<InventoryItem> items,
                                                  string equipmentSlot)
            => items?.Where(x => x.ParentId == equipmentSlot && x.SlotId == slotId)?.FirstOrDefault();
    }
}