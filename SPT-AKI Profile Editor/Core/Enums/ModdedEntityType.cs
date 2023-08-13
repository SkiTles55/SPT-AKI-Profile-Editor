namespace SPT_AKI_Profile_Editor.Core.Enums
{
    public enum ModdedEntityType
    {
        PmcInventoryItem,
        ScavInventoryItem,
        Quest,
        ExaminedItem,
        Merchant,
        WeaponBuild
    }

    public static class ModdedEntityTypeExtension
    {
        public static string LocalizedName(this ModdedEntityType type) => type switch
        {
            ModdedEntityType.PmcInventoryItem => AppData.AppLocalization.GetLocalizedString("tab_clearing_from_mods_pmc_inventory_item"),
            ModdedEntityType.ScavInventoryItem => AppData.AppLocalization.GetLocalizedString("tab_clearing_from_mods_scav_inventory_item"),
            ModdedEntityType.Quest => AppData.AppLocalization.GetLocalizedString("tab_clearing_from_mods_quest"),
            ModdedEntityType.ExaminedItem => AppData.AppLocalization.GetLocalizedString("tab_clearing_from_mods_examined_item"),
            ModdedEntityType.Merchant => AppData.AppLocalization.GetLocalizedString("tab_clearing_from_mods_merchant"),
            ModdedEntityType.WeaponBuild => AppData.AppLocalization.GetLocalizedString("tab_clearing_from_mods_weapon_build"),
            _ => "unknown"
        };

        public static bool CanBeRemovedWithoutSave(this ModdedEntityType type) => type switch
        {
            ModdedEntityType.Quest => false,
            ModdedEntityType.Merchant => false,
            _ => true
        };
    }
}