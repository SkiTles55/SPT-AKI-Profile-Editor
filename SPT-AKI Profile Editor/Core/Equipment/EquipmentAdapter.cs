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
        {
            var emptySlotItemText = localization?.GetLocalizedString(emptySlotKey);
            return new()
            {
                new EquipmentSlotItem(EarpieceSlotName(localesGlobal),
                                      GetEquipment(appSettings.EarpieceSlotId, inventory.Items, equipmentSlot),
                                      emptySlotItemText),
                new EquipmentSlotItem(HeadwearSlotName(localesGlobal),
                                      GetEquipment(appSettings.HeadwearSlotId, inventory.Items, equipmentSlot),
                                      emptySlotItemText),
                new EquipmentSlotItem(FaceCoverSlotName(localesGlobal),
                                      GetEquipment(appSettings.FaceCoverSlotId, inventory.Items, equipmentSlot),
                                      emptySlotItemText),
                new EquipmentSlotItem(EyewearSlotName(localesGlobal),
                                      GetEquipment(appSettings.EyewearSlotId, inventory.Items, equipmentSlot),
                                      emptySlotItemText),
                new EquipmentSlotItem(ArmbandSlotName(localesGlobal),
                                      GetEquipment(appSettings.ArmBandSlotId, inventory.Items, equipmentSlot),
                                      emptySlotItemText),
                new EquipmentSlotItem(ArmorVestSlotName(localesGlobal),
                                      GetEquipment(appSettings.ArmorVestSlotId, inventory.Items, equipmentSlot),
                                      emptySlotItemText),
                new EquipmentSlotItem(TacticalVestSlotName(localesGlobal),
                                      GetEquipment(appSettings.TacticalVestSlotId, inventory.Items, equipmentSlot),
                                      emptySlotItemText),
                new EquipmentSlotItem(PrimaryWeaponFirstSlotName(localesGlobal),
                                      GetEquipment(appSettings.FirstPrimaryWeaponSlotId, inventory.Items, equipmentSlot),
                                      emptySlotItemText),
                new EquipmentSlotItem(PrimaryWeaponSecondSlotName(localesGlobal),
                                      GetEquipment(appSettings.SecondPrimaryWeaponSlotId, inventory.Items, equipmentSlot),
                                      emptySlotItemText),
                new EquipmentSlotItem(HolsterSlotName(localesGlobal),
                                      GetEquipment(appSettings.HolsterSlotId, inventory.Items, equipmentSlot),
                                      emptySlotItemText),
                new EquipmentSlotItem(ScabbardSlotName(localesGlobal),
                                      GetEquipment(appSettings.ScabbardSlotId, inventory.Items, equipmentSlot),
                                      emptySlotItemText),
                new EquipmentSlotPockets(PocketsSlotName(localesGlobal), inventory?.PocketsItems),
                new EquipmentSlotItem(BackpackSlotName(localesGlobal),
                                      GetEquipment(appSettings.BackpackSlotId, inventory.Items, equipmentSlot),
                                      emptySlotItemText),
                new EquipmentSlotItem(SecuredContainerSlotName(localesGlobal),
                                      GetEquipment(appSettings.SecuredContainerSlotId, inventory.Items, equipmentSlot),
                                      emptySlotItemText)
            };
        }

        public static List<EquipmentSlot> GetEquipmentFromBuild(EquipmentBuild build,
                                                                Dictionary<string, string> localesGlobal,
                                                                AppLocalization localization,
                                                                AppSettings appSettings)
        {
            var emptySlotItemText = localization?.GetLocalizedString(emptySlotKey);
            return new()
            {
                new EquipmentSlotItem(EarpieceSlotName(localesGlobal),
                                      GetEquipment(appSettings.EarpieceSlotId, build.BuildItems, build.Root),
                                      emptySlotItemText),
                new EquipmentSlotItem(HeadwearSlotName(localesGlobal),
                                      GetEquipment(appSettings.HeadwearSlotId, build.BuildItems, build.Root),
                                      emptySlotItemText),
                new EquipmentSlotItem(FaceCoverSlotName(localesGlobal),
                                      GetEquipment(appSettings.FaceCoverSlotId, build.BuildItems, build.Root),
                                      emptySlotItemText),
                new EquipmentSlotItem(EyewearSlotName(localesGlobal),
                                      GetEquipment(appSettings.EyewearSlotId, build.BuildItems, build.Root),
                                      emptySlotItemText),
                new EquipmentSlotItem(ArmbandSlotName(localesGlobal),
                                      GetEquipment(appSettings.ArmBandSlotId, build.BuildItems, build.Root),
                                      emptySlotItemText),
                new EquipmentSlotItem(ArmorVestSlotName(localesGlobal),
                                      GetEquipment(appSettings.ArmorVestSlotId, build.BuildItems, build.Root),
                                      emptySlotItemText),
                new EquipmentSlotItem(TacticalVestSlotName(localesGlobal),
                                      GetEquipment(appSettings.TacticalVestSlotId, build.BuildItems, build.Root),
                                      emptySlotItemText),
                new EquipmentSlotItem(PrimaryWeaponFirstSlotName(localesGlobal),
                                      GetEquipment(appSettings.FirstPrimaryWeaponSlotId, build.BuildItems, build.Root),
                                      emptySlotItemText),
                new EquipmentSlotItem(PrimaryWeaponSecondSlotName(localesGlobal),
                                      GetEquipment(appSettings.SecondPrimaryWeaponSlotId, build.BuildItems, build.Root),
                                      emptySlotItemText),
                new EquipmentSlotItem(HolsterSlotName(localesGlobal),
                                      GetEquipment(appSettings.HolsterSlotId, build.BuildItems, build.Root),
                                      emptySlotItemText),
                new EquipmentSlotItem(ScabbardSlotName(localesGlobal),
                                      GetEquipment(appSettings.ScabbardSlotId, build.BuildItems, build.Root),
                                      emptySlotItemText),
                new EquipmentSlotPockets(PocketsSlotName(localesGlobal), build.BuildItems),
                new EquipmentSlotItem(BackpackSlotName(localesGlobal),
                                      GetEquipment(appSettings.BackpackSlotId, build.BuildItems, build.Root),
                                      emptySlotItemText),
                new EquipmentSlotItem(SecuredContainerSlotName(localesGlobal),
                                      GetEquipment(appSettings.SecuredContainerSlotId, build.BuildItems, build.Root),
                                      emptySlotItemText)
            };
        }

        private static string EarpieceSlotName(Dictionary<string, string> localesGlobal)
            => GetSlotName("Earpiece", localesGlobal);

        private static string HeadwearSlotName(Dictionary<string, string> localesGlobal)
            => GetSlotName("Headwear", localesGlobal);

        private static string FaceCoverSlotName(Dictionary<string, string> localesGlobal)
            => GetSlotName("FaceCover", localesGlobal);

        private static string EyewearSlotName(Dictionary<string, string> localesGlobal)
            => GetSlotName("Eyewear", localesGlobal);

        private static string ArmbandSlotName(Dictionary<string, string> localesGlobal)
            => GetSlotName("Armband", localesGlobal);

        private static string ArmorVestSlotName(Dictionary<string, string> localesGlobal)
            => GetSlotName("ArmorVest", localesGlobal);

        private static string TacticalVestSlotName(Dictionary<string, string> localesGlobal)
            => GetSlotName("TacticalVest", localesGlobal);

        private static string PrimaryWeaponFirstSlotName(Dictionary<string, string> localesGlobal)
            => GetSlotName("PrimaryWeaponFirst", localesGlobal);

        private static string PrimaryWeaponSecondSlotName(Dictionary<string, string> localesGlobal)
            => GetSlotName("PrimaryWeaponSecond", localesGlobal);

        private static string HolsterSlotName(Dictionary<string, string> localesGlobal)
            => GetSlotName("Holster", localesGlobal);

        private static string ScabbardSlotName(Dictionary<string, string> localesGlobal)
            => GetSlotName("Scabbard", localesGlobal);

        private static string PocketsSlotName(Dictionary<string, string> localesGlobal)
            => GetSlotName("Pockets", localesGlobal);

        private static string BackpackSlotName(Dictionary<string, string> localesGlobal)
            => GetSlotName("Backpack", localesGlobal);

        private static string SecuredContainerSlotName(Dictionary<string, string> localesGlobal)
            => GetSlotName("SecuredContainer", localesGlobal);

        private static string GetSlotName(string key, Dictionary<string, string> localesGlobal)
            => localesGlobal?.ContainsKey(key) == true ? localesGlobal[key] : key;

        private static InventoryItem GetEquipment(string slotId,
                                                  IEnumerable<InventoryItem> items,
                                                  string equipmentSlot)
            => items?.Where(x => x.ParentId == equipmentSlot && x.SlotId == slotId)?.FirstOrDefault();
    }
}