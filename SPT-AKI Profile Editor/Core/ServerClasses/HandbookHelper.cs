using SPT_AKI_Profile_Editor.Core.ProfileClasses;
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
        private readonly ObservableCollection<WeaponBuildCategory> globalBuilds;

        public HandbookHelper(List<HandbookCategory> categories,
                              Dictionary<string, TarkovItem> itemsDB,
                              ObservableCollection<KeyValuePair<string, WeaponBuild>> globalBuilds)
        {
            this.categories = categories;
            this.itemsDB = itemsDB;
            this.globalBuilds = GlobalBuildsCategories(categories, globalBuilds);
        }

        public ObservableCollection<AddableCategory> CategoriesForItemsAdding => CategoriesForItemsAddingWithFilter("");

        public ObservableCollection<AddableCategory> CategoriesForItemsAddingWithFilter(string tpl) => CreateCompositeCollection(tpl);

        public AddableCategory GetItemCategory(string itemTpl) => categories.Where(x => x.Items.Any(i => i.Id == itemTpl)).FirstOrDefault();

        private static ObservableCollection<WeaponBuildCategory> GlobalBuildsCategories(List<HandbookCategory> categories,
                                                                                 ObservableCollection<KeyValuePair<string, WeaponBuild>> globalBuilds)
        {
            var buildCategories = new ObservableCollection<AddableCategory>(categories
                .Where(x => string.IsNullOrEmpty(x.ParentId))
                .Select(x => new WeaponBuildCategory(x, globalBuilds))
                .Where(x => x.IsNotHidden));
            var globalBuildsCategory = new List<WeaponBuildCategory>() {
                new WeaponBuildCategory(AppData.AppLocalization.GetLocalizedString("tab_stash_global_items_presets"), buildCategories)
            };
            return new ObservableCollection<WeaponBuildCategory>(globalBuildsCategory);
        }

        private ObservableCollection<AddableCategory> CreateCompositeCollection(string tpl)
        {
            var compositeCollection = new ObservableCollection<AddableCategory>();
            foreach (var item in categories
                .Select(x => FilterForConatiner(HandbookCategory.CopyFrom(x), tpl))
                .Where(x => string.IsNullOrEmpty(x.ParentId) && x.IsNotHidden))
                compositeCollection.Add(item);
            foreach (var item in globalBuilds
                .Select(x => FilterForConatiner(WeaponBuildCategory.CopyFrom(x), tpl))
                .Where(x => string.IsNullOrEmpty(x.ParentId) && x.IsNotHidden))
                compositeCollection.Add(item);
            return compositeCollection;
        }

        private AddableCategory FilterForConatiner(AddableCategory category, string tpl)
        {
            category.Items = new ObservableCollection<AddableItem>(category.Items
                    .Where(x => x.CanBeAddedToStash && ItemFilterForConatiner(tpl, x)));
            category.Categories = new ObservableCollection<AddableCategory>(category.Categories
                .Select(x => FilterForConatiner(x, tpl))
                .Where(x => x.IsNotHidden));
            return category;
        }

        private bool ItemFilterForConatiner(string containerTpl, AddableItem item)
        {
            if (!string.IsNullOrEmpty(containerTpl))
                return !item.IsQuestItem
                    && itemsDB.ContainsKey(containerTpl)
                    && item.CanBeAddedToContainer(itemsDB[containerTpl]);
            return true;
        }
    }
}