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
    }
}