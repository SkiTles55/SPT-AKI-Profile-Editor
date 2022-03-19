using SPT_AKI_Profile_Editor.Classes;
using SPT_AKI_Profile_Editor.Core.ServerClasses;
using SPT_AKI_Profile_Editor.Helpers;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;

namespace SPT_AKI_Profile_Editor.Views.ExtendedControls
{
    internal class HandbookCategoryViewModel : BindableViewModel
    {
        private bool isExpanded;

        private string localizedName;

        private ObservableCollection<HandbookCategoryViewModel> categories;

        private ObservableCollection<TarkovItem> items;

        public HandbookCategoryViewModel(HandbookCategory category)
        {
            LocalizedName = ServerDatabase.LocalesGlobal.Handbook.ContainsKey(category.Id) ? ServerDatabase.LocalesGlobal.Handbook[category.Id] : category.Id;
            Categories = new ObservableCollection<HandbookCategoryViewModel>(ServerDatabase.Handbook.Categories
                .Where(x => x.ParentId == category.Id)
                .Select(x => new HandbookCategoryViewModel(x))
                .Where(x => x.IsNotHidden));
            Items = new ObservableCollection<TarkovItem>(ServerDatabase.Handbook.Items
                .Where(x => x.ParentId == category.Id)
                .Select(x => x.Item)
                .Where(x => x.CanBeAddedToStash));
        }

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

        public bool IsExpanded
        {
            get => isExpanded;
            set
            {
                isExpanded = value;
                OnPropertyChanged("IsExpanded");
            }
        }

        public string LocalizedName
        {
            get => localizedName;
            set
            {
                localizedName = value;
                OnPropertyChanged("LocalizedName");
            }
        }

        public ObservableCollection<HandbookCategoryViewModel> Categories
        {
            get => categories;
            set
            {
                categories = value;
                OnPropertyChanged("Categories");
            }
        }

        public ObservableCollection<TarkovItem> Items
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

        public bool ApplyFilter(string text)
        {
            bool categories = false;
            foreach (var category in Categories)
            {
                if (category.ContainsItemsWithTextInName(text))
                    categories = true;
            }
            ICollectionView cv = CollectionViewSource.GetDefaultView(Categories);
            if (cv == null)
                return categories;
            if (string.IsNullOrEmpty(text))
                cv.Filter = null;
            cv.Filter = o =>
            {
                HandbookCategoryViewModel p = o as HandbookCategoryViewModel;
                return p.ContainsItemsWithTextInName(text);
            };
            return categories;
        }

        public bool ContainsItemsWithTextInName(string text)
        {
            FilterItems();
            return Items.Any(x => x.LocalizedName.ToUpper().Contains(text.ToUpper()))
            || ApplyFilter(text);

            void FilterItems()
            {
                ICollectionView cv = CollectionViewSource.GetDefaultView(Items);
                if (cv == null)
                    return;
                if (string.IsNullOrEmpty(text))
                    cv.Filter = null;
                else
                {
                    cv.Filter = o =>
                    {
                        TarkovItem p = o as TarkovItem;
                        return p.LocalizedName.ToUpper().Contains(text.ToUpper());
                    };
                }
            }
        }
    }
}