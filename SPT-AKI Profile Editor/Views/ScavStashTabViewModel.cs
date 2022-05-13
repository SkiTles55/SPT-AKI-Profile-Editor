using SPT_AKI_Profile_Editor.Classes;
using SPT_AKI_Profile_Editor.Core.Enums;
using SPT_AKI_Profile_Editor.Helpers;

namespace SPT_AKI_Profile_Editor.Views
{
    internal class ScavStashTabViewModel : BindableViewModel
    {
        public static RelayCommand OpenContainer => new(obj => App.OpenContainerWindow(obj, StashEditMode.Scav));

        public RelayCommand RemoveItem => new(async obj =>
         {
             if (obj == null)
                 return;
             if (await Dialogs.YesNoDialog(this, "remove_stash_item_title", "remove_stash_item_caption"))
                 Profile.Characters.Scav.Inventory.RemoveItems(new() { obj.ToString() });
         });

        public RelayCommand RemoveAllEquipment => new(async obj =>
        {
            if (await Dialogs.YesNoDialog(this, "remove_stash_item_title", "remove_stash_items_caption"))
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