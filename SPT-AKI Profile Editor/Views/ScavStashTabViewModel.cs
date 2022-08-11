using SPT_AKI_Profile_Editor.Classes;
using SPT_AKI_Profile_Editor.Core.Enums;
using SPT_AKI_Profile_Editor.Helpers;

namespace SPT_AKI_Profile_Editor.Views
{
    public class ScavStashTabViewModel : BindableViewModel
    {
        private readonly IDialogManager _dialogManager;
        private readonly IWorker _worker;
        private readonly IApplicationManager _applicationManager;

        public ScavStashTabViewModel(IDialogManager dialogManager, IWorker worker, IApplicationManager applicationManager)
        {
            _dialogManager = dialogManager;
            _worker = worker;
            _applicationManager = applicationManager;
        }

        public RelayCommand OpenContainer => new(obj => _applicationManager.OpenContainerWindow(obj, StashEditMode.Scav));

        public RelayCommand InspectWeapon => new(obj => _applicationManager.OpenWeaponBuildWindow(obj, StashEditMode.Scav));

        public RelayCommand RemoveItem => new(async obj =>
        {
            if (obj is string id && await _dialogManager.YesNoDialog(this, "remove_stash_item_title", "remove_stash_item_caption"))
                Profile.Characters.Scav.Inventory.RemoveItems(new() { id });
        });

        public RelayCommand RemoveAllEquipment => new(async obj =>
        {
            if (await _dialogManager.YesNoDialog(this, "remove_stash_item_title", "remove_equipment_items_caption"))
            {
                _worker.AddTask(new WorkerTask
                {
                    Action = () => { Profile.Characters.Scav.Inventory.RemoveAllEquipment(); },
                    Title = AppLocalization.GetLocalizedString("progress_dialog_title"),
                    Description = AppLocalization.GetLocalizedString("remove_stash_item_title")
                });
            }
        });
    }
}