using SPT_AKI_Profile_Editor.Core.ProfileClasses;
using SPT_AKI_Profile_Editor.Helpers;
using System.Collections.ObjectModel;
using System.Linq;

namespace SPT_AKI_Profile_Editor
{
    public class ContainerWindowViewModel : BindableViewModel
    {
        private ObservableCollection<InventoryItem> items;

        public ContainerWindowViewModel(InventoryItem item)
        {
            WindowTitle = item.LocalizedName;
            Items = new(Profile.Characters.Pmc.Inventory.Items?.Where(x => x.ParentId == item.Id));
        }

        public static RelayCommand OpenContainer => new(obj =>
        {
            if (obj == null || obj is not InventoryItem item)
                return;
            ContainerWindow window = new(item);
            window.Show();
        });

        public string WindowTitle { get; }

        public ObservableCollection<InventoryItem> Items
        {
            get => items;
            set
            {
                items = value;
                OnPropertyChanged("Items");
            }
        }

        public RelayCommand RemoveItem => new(async obj =>
        {
            if (obj == null)
                return;
            if (await Dialogs.YesNoDialog(this, "remove_stash_item_title", "remove_stash_item_caption"))
                Profile.Characters.Pmc.Inventory.RemoveItems(new() { obj.ToString() });
        });
    }
}