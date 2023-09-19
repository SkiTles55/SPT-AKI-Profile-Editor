using SPT_AKI_Profile_Editor.Core.ProfileClasses;
using System.Collections.Generic;
using System.Linq;

namespace SPT_AKI_Profile_Editor.Core.Equipment
{
    public class EquipmentAdapter
    {
        private static readonly string emptySlotKey = "tab_stash_empty_slot";

        public static List<EquipmentSlot> GetCharacterEquipment(CharacterInventory inventory,
                                                                string equipmentSlot,
                                                                Dictionary<string, string> localesGlobal,
                                                                AppLocalization localization,
                                                                AppSettings appSettings)
            => GetEquipmentSlots(inventory.Items,
                                 equipmentSlot,
                                 localesGlobal,
                                 localization,
                                 appSettings);

        public static List<EquipmentSlot> GetEquipmentFromBuild(EquipmentBuild build,
                                                                Dictionary<string, string> localesGlobal,
                                                                AppLocalization localization,
                                                                AppSettings appSettings)
            => GetEquipmentSlots(build.BuildItems,
                                 build.Root,
                                 localesGlobal,
                                 localization,
                                 appSettings);

        private static List<EquipmentSlot> GetEquipmentSlots(IEnumerable<InventoryItem> items,
                                                             string root,
                                                             Dictionary<string, string> localesGlobal,
                                                             AppLocalization localization,
                                                             AppSettings appSettings)
        {
            var emptySlotItemText = localization?.GetLocalizedString(emptySlotKey);
            var pocketsId = items?.Where(x => x.IsPockets).FirstOrDefault()?.Id;
            return new()
            {
                new EquipmentSlotItem(GetSlotName("Earpiece", localesGlobal),
                                      GetEquipment(appSettings.EarpieceSlotId, items, root),
                                      emptySlotItemText),
                new EquipmentSlotItem(GetSlotName("Headwear", localesGlobal),
                                      GetEquipment(appSettings.HeadwearSlotId, items, root),
                                      emptySlotItemText),
                new EquipmentSlotItem(GetSlotName("FaceCover", localesGlobal),
                                      GetEquipment(appSettings.FaceCoverSlotId, items, root),
                                      emptySlotItemText),
                new EquipmentSlotItem(GetSlotName("Eyewear", localesGlobal),
                                      GetEquipment(appSettings.EyewearSlotId, items, root),
                                      emptySlotItemText),
                new EquipmentSlotItem(GetSlotName("Armband", localesGlobal),
                                      GetEquipment(appSettings.ArmBandSlotId, items, root),
                                      emptySlotItemText),
                new EquipmentSlotItem(GetSlotName("ArmorVest", localesGlobal),
                                      GetEquipment(appSettings.ArmorVestSlotId, items, root),
                                      emptySlotItemText),
                new EquipmentSlotItem(GetSlotName("TacticalVest", localesGlobal),
                                      GetEquipment(appSettings.TacticalVestSlotId, items, root),
                                      emptySlotItemText),
                new EquipmentSlotItem(GetSlotName("PrimaryWeaponFirst", localesGlobal),
                                      GetEquipment(appSettings.FirstPrimaryWeaponSlotId, items, root),
                                      emptySlotItemText),
                new EquipmentSlotItem(GetSlotName("PrimaryWeaponSecond", localesGlobal),
                                      GetEquipment(appSettings.SecondPrimaryWeaponSlotId, items, root),
                                      emptySlotItemText),
                new EquipmentSlotItem(GetSlotName("Holster", localesGlobal),
                                      GetEquipment(appSettings.HolsterSlotId, items, root),
                                      emptySlotItemText),
                new EquipmentSlotItem(GetSlotName("Scabbard", localesGlobal),
                                      GetEquipment(appSettings.ScabbardSlotId, items, root),
                                      emptySlotItemText),
                new EquipmentSlotPockets(GetSlotName("Pockets", localesGlobal),
                                         items.Where(x => x.ParentId == pocketsId)),
                new EquipmentSlotItem(GetSlotName("Backpack", localesGlobal),
                                      GetEquipment(appSettings.BackpackSlotId, items, root),
                                      emptySlotItemText),
                new EquipmentSlotItem(GetSlotName("SecuredContainer", localesGlobal),
                                      GetEquipment(appSettings.SecuredContainerSlotId, items, root),
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