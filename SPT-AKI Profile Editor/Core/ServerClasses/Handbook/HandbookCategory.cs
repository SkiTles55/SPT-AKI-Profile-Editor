using SPT_AKI_Profile_Editor.Classes;
using SPT_AKI_Profile_Editor.Helpers;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text.Json.Serialization;
using System.Windows.Data;

namespace SPT_AKI_Profile_Editor.Core.ServerClasses
{
    public class HandbookCategory : BindableViewModel
    {
        private bool isExpanded;
        private string localizedName;
        private ObservableCollection<HandbookCategory> categories;
        private ObservableCollection<TarkovItem> items;

        [JsonConstructor]
        public HandbookCategory(string id, string parentId)
        {
            Id = id;
            ParentId = parentId;
            LocalizedName = AppData.ServerDatabase.LocalesGlobal.Handbook.ContainsKey(Id) ? AppData.ServerDatabase.LocalesGlobal.Handbook[Id] : Id;
        }

        public static RelayCommand AddItem => new(obj =>
        {
            if (obj == null || obj is not TarkovItem item)
                return;
            App.Worker.AddAction(new WorkerTask
            {
                Action = () => { AppData.Profile.Characters.Pmc.Inventory.AddNewItems(item.Id, item.AddingQuantity, item.AddingFir); }
            });
        });

        [JsonPropertyName("Id")]
        public string Id { get; set; }

        [JsonPropertyName("ParentId")]
        public string ParentId { get; set; }

        [JsonIgnore]
        public string LocalizedName
        {
            get => localizedName;
            set
            {
                localizedName = value;
                OnPropertyChanged("LocalizedName");
            }
        }

        [JsonIgnore]
        public bool IsExpanded
        {
            get => isExpanded;
            set
            {
                isExpanded = value;
                OnPropertyChanged("IsExpanded");
            }
        }

        [JsonIgnore]
        public ObservableCollection<HandbookCategory> Categories
        {
            get
            {
                if (categories == null)
                    categories = new(AppData.ServerDatabase.Handbook.Categories.Where(x => x.ParentId == Id && x.IsNotHidden));
                return categories;
            }
            set
            {
                categories = value;
                OnPropertyChanged("Categories");
            }
        }

        [JsonIgnore]
        public ObservableCollection<TarkovItem> Items
        {
            get
            {
                if (items == null)
                    items = new(AppData.ServerDatabase.Handbook.Items
                        .Where(x => x.ParentId == Id)
                        .Select(x => x.Item)
                        .Where(x => x.CanBeAddedToStash));
                return items;
            }
            set
            {
                items = value;
                OnPropertyChanged("Items");
            }
        }

        [JsonIgnore]
        public bool IsNotHidden => Items.Count > 0 || Categories.Count > 0;

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
                HandbookCategory p = o as HandbookCategory;
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