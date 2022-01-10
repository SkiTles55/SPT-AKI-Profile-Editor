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
            Category = category;
        }
        public HandbookCategory Category
        {
            get => category;
            set
            {
                category = value;
                OnPropertyChanged("Category");
            }
        }
        public string LocalizedName =>
            ServerDatabase.LocalesGlobal.Handbook.ContainsKey(Category.Id) ? ServerDatabase.LocalesGlobal.Handbook[Category.Id] : Category.Id;

        public List<HandbookCategoryViewModel> Categories =>
            ServerDatabase.Handbook.Categories
            .Where(x => x.ParentId == Category.Id)
            .Select(x => new HandbookCategoryViewModel(x))
            .Where(x => x.IsNotHidden)
            .ToList();

        public List<TarkovItem> Items =>
            ServerDatabase.Handbook.Items
            .Where(x => x.ParentId == Category.Id)
            .Select(x => x.Item)
            .Where(x => x.CanBeAddedToStash)
            .ToList();

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

        private HandbookCategory category;
    }
}