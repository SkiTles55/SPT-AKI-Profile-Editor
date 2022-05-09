using SPT_AKI_Profile_Editor.Core;
using SPT_AKI_Profile_Editor.Core.ProfileClasses;
using SPT_AKI_Profile_Editor.Helpers;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Data;

namespace SPT_AKI_Profile_Editor.Views.ExtendedControls
{
    /// <summary>
    /// Логика взаимодействия для ItemsGrid.xaml
    /// </summary>
    public partial class ItemsGrid : GridControl
    {
        public ItemsGrid()
        {
            InitializeComponent();
        }

        private void FilterBoxStash_TextChanged(object sender, TextChangedEventArgs e) =>
            ApplyStashFilter();

        private void ApplyStashFilter()
        {
            ICollectionView cv = CollectionViewSource.GetDefaultView(itemsGrid.ItemsSource);
            if (cv == null)
                return;
            if (string.IsNullOrEmpty(AppData.GridFilters.StashTab.StashItemName)
                && string.IsNullOrEmpty(AppData.GridFilters.StashTab.Id))
                cv.Filter = null;
            else
            {
                cv.Filter = o =>
                {
                    InventoryItem p = o as InventoryItem;
                    return (string.IsNullOrEmpty(AppData.GridFilters.StashTab.StashItemName)
                    || p.LocalizedName.ToUpper().Contains(AppData.GridFilters.StashTab.StashItemName.ToUpper()))
                    && (string.IsNullOrEmpty(AppData.GridFilters.StashTab.Id)
                    || p.Id.ToUpper().Contains(AppData.GridFilters.StashTab.Id.ToUpper()));
                };
            }
        }
    }
}