using SPT_AKI_Profile_Editor.Core.Enums;
using SPT_AKI_Profile_Editor.Core.ProfileClasses;
using SPT_AKI_Profile_Editor.Helpers;

namespace SPT_AKI_Profile_Editor.Views
{
    public class ScavStashTabViewModel : BindableViewModel
    {
        private readonly IDialogManager _dialogManager;
        private readonly IWorker _worker;
        private readonly IApplicationManager _applicationManager;

        public ScavStashTabViewModel(IDialogManager dialogManager,
                                     IWorker worker,
                                     IApplicationManager applicationManager)
        {
            _dialogManager = dialogManager;
            _worker = worker;
            _applicationManager = applicationManager;
        }

        public RelayCommand OpenContainer => new(obj =>
        {
            if (obj is InventoryItem item)
                _applicationManager.OpenContainerWindow(item, GetInventory());
        });

        public RelayCommand InspectWeapon => new(obj =>
        {
            if (obj is InventoryItem item)
                _applicationManager.OpenWeaponBuildWindow(item, Profile.Characters.GetInventory(StashEditMode.Scav));
        });

        public RelayCommand RemoveItem => new(async obj =>
        {
            if (obj is string id && await _dialogManager.YesNoDialog("remove_stash_item_title",
                                                                     "remove_stash_item_caption"))
                Profile.Characters.Scav.Inventory.RemoveItems(new() { id });
        });

        public RelayCommand RemoveAllEquipment => new(async obj =>
        {
            if (await _dialogManager.YesNoDialog("remove_stash_item_title", "remove_equipment_items_caption"))
                _worker.AddTask(new(() => { Profile.Characters.Scav.Inventory.RemoveAllEquipment(); },
                                    AppLocalization.GetLocalizedString("progress_dialog_title"),
                                    AppLocalization.GetLocalizedString("remove_stash_item_title")));
        });

        private static CharacterInventory GetInventory()
                                    => Profile.Characters.GetInventory(StashEditMode.Scav);
    }
}