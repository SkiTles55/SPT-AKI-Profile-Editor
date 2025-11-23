using SPT_AKI_Profile_Editor.Core.Enums;
using SPT_AKI_Profile_Editor.Core.ProfileClasses;
using SPT_AKI_Profile_Editor.Helpers;

namespace SPT_AKI_Profile_Editor.Views
{
    public class ScavStashTabViewModel(IDialogManager dialogManager,
        IWorker worker,
        IApplicationManager applicationManager) : BindableViewModel
    {
        public RelayCommand OpenContainer => new(obj =>
        {
            if (obj is InventoryItem item)
                applicationManager.OpenContainerWindow(item, GetInventory(), true);
        });

        public RelayCommand InspectWeapon => new(obj =>
        {
            if (obj is InventoryItem item)
                applicationManager.OpenWeaponBuildWindow(item,
                                                         Profile.Characters.GetInventory(StashEditMode.Scav),
                                                         true);
        });

        public RelayCommand RemoveItem => new(async obj =>
        {
            if (obj is string id && await dialogManager.YesNoDialog("remove_stash_item_title",
                                                                    "remove_stash_item_caption"))
                Profile.Characters.Scav.Inventory.RemoveItems([id]);
        });

        public RelayCommand RemoveAllEquipment => new(async obj =>
        {
            if (await dialogManager.YesNoDialog("remove_stash_item_title", "remove_equipment_items_caption"))
                worker.AddTask(ProgressTask(() => Profile.Characters.Scav.Inventory.RemoveAllEquipment(),
                    AppLocalization.GetLocalizedString("remove_stash_item_title")));
        });

        private static CharacterInventory GetInventory()
                                    => Profile.Characters.GetInventory(StashEditMode.Scav);
    }
}