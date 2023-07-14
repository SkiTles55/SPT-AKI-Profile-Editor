namespace SPT_AKI_Profile_Editor.Core.Enums
{
    public enum ModdedEntityType
    {
        PmcInventoryItem,
        ScavInventoryItem,
        Quest
    }

    public static class ModdedEntityTypeExtension
    {
        public static string LocalizedName(this ModdedEntityType type) => type switch
        {
            ModdedEntityType.PmcInventoryItem => AppData.AppLocalization.GetLocalizedString("tab_clearing_from_mods_pmc_inventory_item"),
            ModdedEntityType.ScavInventoryItem => AppData.AppLocalization.GetLocalizedString("tab_clearing_from_mods_scav_inventory_item"),
            ModdedEntityType.Quest => AppData.AppLocalization.GetLocalizedString("tab_clearing_from_mods_scav_quest"),
            _ => "unknown"
        };
    }
}