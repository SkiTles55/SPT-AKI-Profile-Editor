using SPT_AKI_Profile_Editor.Core;
using SPT_AKI_Profile_Editor.Core.ServerClasses;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace SPT_AKI_Profile_Editor.Views.ExtendedControls
{
    /// <summary>
    /// Логика взаимодействия для ItemsAdding.xaml
    /// </summary>
    public partial class ItemsAdding : UserControl
    {
        public ItemsAdding()
        {
            InitializeComponent();
        }

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
                    HandbookCategory p = o as HandbookCategory;
                    return p.ContainsItemsWithTextInName(AppData.GridFilters.StashTab.AddingItemName);
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