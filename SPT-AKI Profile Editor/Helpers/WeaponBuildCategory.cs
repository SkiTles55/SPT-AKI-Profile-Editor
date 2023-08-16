using SPT_AKI_Profile_Editor.Core.ProfileClasses;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace SPT_AKI_Profile_Editor.Helpers
{
    public class WeaponBuildCategory : AddableCategory
    {
        public WeaponBuildCategory()
        { }

        public WeaponBuildCategory(AddableCategory category, ObservableCollection<WeaponBuild> globalBuilds)
        {
            ParentId = category.ParentId;
            LocalizedName = category.LocalizedName;
            Categories = new(category.Categories
                .Select(x => new WeaponBuildCategory(x, globalBuilds))
                .Where(x => x.IsNotHidden));
            Items = new(globalBuilds.Where(x => category.Items.Any(y => y.Id == x.RootTpl)));
            BitmapIcon = category.BitmapIcon;
        }

        public WeaponBuildCategory(string name, ObservableCollection<AddableCategory> categories)
        {
            LocalizedName = name;
            Categories = categories;
            Items = new();
        }

        public static WeaponBuildCategory CopyFrom(WeaponBuildCategory category) => new()
        {
            ParentId = category.ParentId,
            LocalizedName = category.LocalizedName,
            IsExpanded = false,
            Categories = new ObservableCollection<AddableCategory>(category.Categories.Select(x => CopyFrom((WeaponBuildCategory)x))),
            Items = new ObservableCollection<AddableItem>(category.Items.Select(x => WeaponBuild.CopyFrom((WeaponBuild)x))),
            BitmapIcon = category.BitmapIcon
        };
    }
}