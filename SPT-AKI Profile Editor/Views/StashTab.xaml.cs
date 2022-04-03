using SPT_AKI_Profile_Editor.Core;
using SPT_AKI_Profile_Editor.Core.ProfileClasses;
using SPT_AKI_Profile_Editor.Views.ExtendedControls;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace SPT_AKI_Profile_Editor.Views
{
    /// <summary>
    /// Логика взаимодействия для StashTab.xaml
    /// </summary>
    public partial class StashTab : UserControl
    {
        public StashTab()
        {
            InitializeComponent();
            DataContext = new StashTabViewModel();
        }

        private void FilterBoxStash_TextChanged(object sender, TextChangedEventArgs e) =>
            ApplyStashFilter();

        private void FilterBoxAdding_TextChanged(object sender, TextChangedEventArgs e) =>
            ApplyAddingFilter();

        private void ApplyAddingFilter()
        {
            ICollectionView cv = CollectionViewSource.GetDefaultView(itemsList.ItemsSource);
            if (cv == null)
                return;
            else
            {
                cv.Filter = o =>
                {
                    HandbookCategoryViewModel p = o as HandbookCategoryViewModel;
                    return p.ContainsItemsWithTextInName(AppData.GridFilters.StashTab.AddingItemName);
                };
            }
        }

        private void ApplyStashFilter()
        {
            ICollectionView cv = CollectionViewSource.GetDefaultView(stashGrid.ItemsSource);
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

        private void ScrollViewer_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (sender is ScrollViewer scrollViewer)
            {
                if (e.Delta > 0)
                    scrollViewer.LineUp();
                else
                    scrollViewer.LineDown();
                e.Handled = true;
            }
        }
    }
}