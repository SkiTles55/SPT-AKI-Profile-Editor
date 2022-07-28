using SPT_AKI_Profile_Editor.Core.ProfileClasses;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace SPT_AKI_Profile_Editor.Helpers
{
    public class WeaponBuildCategory : AddableCategory
    {
        public WeaponBuildCategory(AddableCategory category, ObservableCollection<KeyValuePair<string, WeaponBuild>> globalBuilds)
        {
            ParentId = category.ParentId;
            LocalizedName = category.LocalizedName;
            Categories = new(category.Categories
                .Select(x => new WeaponBuildCategory(x, globalBuilds))
                .Where(x => x.IsNotHidden));
            Items = new(globalBuilds
                .Where(x => category.Items.Any(y => y.Id == x.Value.RootTpl))
                .Select(x => x.Value));
        }

        public WeaponBuildCategory(string name, ObservableCollection<AddableCategory> categories)
        {
            LocalizedName = name;
            Categories = categories;
            Items = new();
        }
    }
}