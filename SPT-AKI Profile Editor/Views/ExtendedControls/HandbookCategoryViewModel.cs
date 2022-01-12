using SPT_AKI_Profile_Editor.Classes;
using SPT_AKI_Profile_Editor.Core.ServerClasses;
using SPT_AKI_Profile_Editor.Helpers;
using System.Collections.Generic;
using System.Linq;

namespace SPT_AKI_Profile_Editor.Views.ExtendedControls
{
    class HandbookCategoryViewModel : BindableViewModel
    {
        public HandbookCategoryViewModel(HandbookCategory category)
        {
            LocalizedName = ServerDatabase.LocalesGlobal.Handbook.ContainsKey(category.Id) ? ServerDatabase.LocalesGlobal.Handbook[category.Id] : category.Id;
            Categories = ServerDatabase.Handbook.Categories
                .Where(x => x.ParentId == category.Id)
                .Select(x => new HandbookCategoryViewModel(x))
                .Where(x => x.IsNotHidden)
                .ToList();
            Items = ServerDatabase.Handbook.Items
                .Where(x => x.ParentId == category.Id)
                .Select(x => x.Item)
                .Where(x => x.CanBeAddedToStash)
                .ToList();
        }
        public string LocalizedName
        {
            get => localizedName;
            set
            {
                localizedName = value;
                OnPropertyChanged("localizedName");
            }
        }
        public List<HandbookCategoryViewModel> Categories
        {
            get => categories;
            set
            {
                categories = value;
                OnPropertyChanged("Categories");
            }
        }
        public List<TarkovItem> Items
        {
            get => items;
            set
            {
                items = value;
                OnPropertyChanged("Items");
            }
        }
        public bool IsNotHidden =>
            Items.Count > 0 || Categories.Any(y => y.Items.Count > 0);
        public static RelayCommand AddItem => new(obj =>
        {
            if (obj == null)
                return;
            if (obj is not TarkovItem item)
                return;
            App.Worker.AddAction(new WorkerTask
            {
                Action = () => { Profile.Characters.Pmc.Inventory.AddNewItems(item.Id, item.AddingQuantity, item.AddingFir); }
            });
        });

        private string localizedName;
        private List<HandbookCategoryViewModel> categories;
        private List<TarkovItem> items;
    }
}