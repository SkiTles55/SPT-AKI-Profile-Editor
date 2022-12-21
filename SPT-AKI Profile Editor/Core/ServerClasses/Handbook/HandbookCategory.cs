using Newtonsoft.Json;
using SPT_AKI_Profile_Editor.Helpers;
using System.Collections.ObjectModel;
using System.Linq;

namespace SPT_AKI_Profile_Editor.Core.ServerClasses
{
    public class HandbookCategory : AddableCategory
    {
        private ObservableCollection<AddableCategory> categories;
        private ObservableCollection<AddableItem> items;

        [JsonConstructor]
        public HandbookCategory(string id, string parentId)
        {
            Id = id;
            ParentId = parentId;
            LocalizedName = AppData.ServerDatabase.LocalesGlobal.ContainsKey(Id.Name()) ? AppData.ServerDatabase.LocalesGlobal[Id.Name()] : Id;
        }

        [JsonProperty("Id")]
        public string Id { get; set; }

        [JsonIgnore]
        public override ObservableCollection<AddableCategory> Categories
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
        public override ObservableCollection<AddableItem> Items
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

        public static HandbookCategory CopyFrom(HandbookCategory category) => new(category.Id, category.ParentId)
        {
            LocalizedName = category.LocalizedName,
            IsExpanded = false,
            categories = new ObservableCollection<AddableCategory>(category.Categories.Select(x => CopyFrom((HandbookCategory)x))),
            items = new ObservableCollection<AddableItem>(category.Items.Select(x => TarkovItem.CopyFrom((TarkovItem)x)))
        };
    }
}