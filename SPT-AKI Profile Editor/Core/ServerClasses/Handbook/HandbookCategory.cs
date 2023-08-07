using Newtonsoft.Json;
using SPT_AKI_Profile_Editor.Core.HelperClasses;
using SPT_AKI_Profile_Editor.Helpers;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows.Media.Imaging;

namespace SPT_AKI_Profile_Editor.Core.ServerClasses
{
    public class HandbookCategory : AddableCategory
    {
        private ObservableCollection<AddableCategory> categories;
        private ObservableCollection<AddableItem> items;

        [JsonConstructor]
        public HandbookCategory(string id, string parentId, string icon)
        {
            Id = id;
            ParentId = parentId;
            Icon = icon;
            LocalizedName = AppData.ServerDatabase.LocalesGlobal.ContainsKey(Id) ? AppData.ServerDatabase.LocalesGlobal[Id] : Id;
            LoadBitmapIcon();
        }

        public string Id { get; set; }

        [JsonIgnore]
        public override ObservableCollection<AddableCategory> Categories
        {
            get
            {
                categories ??= new(AppData.ServerDatabase.Handbook.Categories.Where(x => x.ParentId == Id && x.IsNotHidden));
                return categories;
            }
            set
            {
                categories = value;
                OnPropertyChanged(nameof(Categories));
            }
        }

        [JsonIgnore]
        public override ObservableCollection<AddableItem> Items
        {
            get
            {
                items ??= new(AppData.ServerDatabase.Handbook.Items
                        .Where(x => x.ParentId == Id)
                        .Select(x => x.Item));
                return items;
            }
            set
            {
                items = value;
                OnPropertyChanged(nameof(Items));
            }
        }

        public static HandbookCategory CopyFrom(HandbookCategory category) => new(category.Id, category.ParentId, category.Icon)
        {
            LocalizedName = category.LocalizedName,
            IsExpanded = false,
            categories = new ObservableCollection<AddableCategory>(category.Categories.Select(x => CopyFrom((HandbookCategory)x))),
            items = new ObservableCollection<AddableItem>(category.Items.Select(x => TarkovItem.CopyFrom((TarkovItem)x)))
        };

        private void LoadBitmapIcon()
        {
            var iconPath = Path.Combine(AppData.AppSettings.ServerPath, AppData.AppSettings.DirsList[SPTServerDir.handbookIcons], Path.GetFileName(Icon));
            if (File.Exists(iconPath))
            {
                try
                {
                    BitmapIcon = new BitmapImage(new Uri(iconPath));
                    BitmapIcon.Freeze();
                }
                catch { }
            }
        }
    }
}