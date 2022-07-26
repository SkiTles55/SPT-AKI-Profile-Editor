using SPT_AKI_Profile_Editor.Helpers;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace SPT_AKI_Profile_Editor.Core.ServerClasses
{
    public class HandbookHelper
    {
        private readonly List<HandbookCategory> categories;
        private readonly Dictionary<string, TarkovItem> itemsDB;

        public HandbookHelper(List<HandbookCategory> categories, Dictionary<string, TarkovItem> itemsDB)
        {
            this.categories = categories;
            this.itemsDB = itemsDB;
        }

        public ObservableCollection<AddableCategory> CategoriesForItemsAdding => categories != null ? new(categories
                    .Where(x => string.IsNullOrEmpty(x.ParentId) && x.IsNotHidden)) : new();

        public ObservableCollection<AddableCategory> CategoriesForItemsAddingWithFilter(string tpl) => new(categories
                        .Select(x => FilterForConatiner(HandbookCategory.CopyFrom(x), tpl))
                        .Where(x => string.IsNullOrEmpty(x.ParentId) && x.IsNotHidden));

        private AddableCategory FilterForConatiner(AddableCategory category, string tpl)
        {
            if (itemsDB.ContainsKey(tpl))
            {
                category.Items = new ObservableCollection<AddableItem>(category.Items.Where(x => x.CanBeAddedToContainer(itemsDB[tpl])));
                category.Categories = new ObservableCollection<AddableCategory>(category.Categories.Select(x => FilterForConatiner(x, tpl)).Where(x => x.IsNotHidden));
            }
            return category;
        }
    }
}