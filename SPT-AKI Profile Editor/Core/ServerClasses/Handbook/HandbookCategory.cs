using SPT_AKI_Profile_Editor.Helpers;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using Newtonsoft.Json;
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

        [JsonProperty("Id")]
        public string Id { get; set; }

        [JsonProperty("ParentId")]
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

        public static HandbookCategory CopyFrom(HandbookCategory category) => new(category.Id, category.ParentId)
        {
            LocalizedName = category.LocalizedName,
            IsExpanded = false,
            categories = new ObservableCollection<HandbookCategory>(category.Categories.Select(x => CopyFrom(x))),
            items = new ObservableCollection<TarkovItem>(category.Items.Select(x => TarkovItem.CopyFrom(x)))
        };

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