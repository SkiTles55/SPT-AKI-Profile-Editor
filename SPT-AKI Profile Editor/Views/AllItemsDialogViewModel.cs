using SPT_AKI_Profile_Editor.Core.ServerClasses;
using SPT_AKI_Profile_Editor.Helpers;
using System.Collections.Generic;
using System.Linq;

namespace SPT_AKI_Profile_Editor.Views
{
    public class AllItemsDialogViewModel(RelayCommand addCommand,
        bool stashSelectorVisible,
        object context) : ClosableDialogViewModel(context)
    {
        public static IEnumerable<AddableItem> AddableItems
            => ServerDatabase?.ItemsDB?.Values
            .Where(x => x.Properties.StackMaxSize > 0)
            .Select(x => TarkovItem.CopyFrom(x));

        public bool StashSelectorVisible { get; } = stashSelectorVisible;

        public RelayCommand AddCommand { get; set; } = addCommand;
    }
}