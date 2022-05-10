using SPT_AKI_Profile_Editor.Classes;
using SPT_AKI_Profile_Editor.Core.ProfileClasses;
using SPT_AKI_Profile_Editor.Helpers;
using System.Collections.ObjectModel;
using System.Linq;

namespace SPT_AKI_Profile_Editor
{
    public class ContainerWindowViewModel : BindableViewModel
    {
        private readonly InventoryItem _item;

        public ContainerWindowViewModel(InventoryItem item)
        {
            WindowTitle = item.LocalizedName;
            _item = item;
        }

        public static RelayCommand OpenContainer => new(obj => { App.OpenContainerWindow(obj); });

        public string WindowTitle { get; }

        public ObservableCollection<InventoryItem> Items => new(Profile.Characters.Pmc.Inventory.Items?.Where(x => x.ParentId == _item.Id));

        public RelayCommand RemoveItem => new(async obj =>
        {
            if (obj == null)
                return;
            if (await Dialogs.YesNoDialog(this, "remove_stash_item_title", "remove_stash_item_caption"))
            {
                Profile.Characters.Pmc.Inventory.RemoveItems(new() { obj.ToString() });
                OnPropertyChanged("Items");
            }
        });

        public RelayCommand RemoveAllItems => new(async obj =>
        {
            if (await Dialogs.YesNoDialog(this, "remove_stash_item_title", "remove_stash_items_caption"))
            {
                App.Worker.AddAction(new WorkerTask
                {
                    Action = () => { Profile.Characters.Pmc.Inventory.RemoveItems(Items.Select(x => x.Id).ToList()); OnPropertyChanged("Items"); },
                    Title = AppLocalization.GetLocalizedString("progress_dialog_title"),
                    Description = AppLocalization.GetLocalizedString("remove_stash_item_title")
                });
            }
        });
    }
}