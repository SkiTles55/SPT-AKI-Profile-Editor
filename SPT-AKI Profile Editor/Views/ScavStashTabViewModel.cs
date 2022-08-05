using SPT_AKI_Profile_Editor.Classes;
using SPT_AKI_Profile_Editor.Core.Enums;
using SPT_AKI_Profile_Editor.Helpers;

namespace SPT_AKI_Profile_Editor.Views
{
    internal class ScavStashTabViewModel : BindableViewModel
    {
        private readonly IDialogManager _dialogManager;

        public ScavStashTabViewModel(IDialogManager dialogManager) => _dialogManager = dialogManager;

        public static RelayCommand OpenContainer => new(obj => App.OpenContainerWindow(obj, StashEditMode.Scav));

        public static RelayCommand InspectWeapon => new(obj => App.OpenWeaponBuildWindow(obj, StashEditMode.Scav));

        public RelayCommand RemoveItem => new(async obj =>
        {
            if (obj is string id && await _dialogManager.YesNoDialog(this, "remove_stash_item_title", "remove_stash_item_caption"))
                Profile.Characters.Scav.Inventory.RemoveItems(new() { id });
        });

        public RelayCommand RemoveAllEquipment => new(async obj =>
        {
            if (await _dialogManager.YesNoDialog(this, "remove_stash_item_title", "remove_equipment_items_caption"))
            {
                App.Worker.AddAction(new WorkerTask
                {
                    Action = () => { Profile.Characters.Scav.Inventory.RemoveAllEquipment(); },
                    Title = AppLocalization.GetLocalizedString("progress_dialog_title"),
                    Description = AppLocalization.GetLocalizedString("remove_stash_item_title")
                });
            }
        });
    }
}