using SPT_AKI_Profile_Editor.Core;
using SPT_AKI_Profile_Editor.Core.HelperClasses;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Data;

namespace SPT_AKI_Profile_Editor.Views
{
    /// <summary>
    /// Логика взаимодействия для ExaminedItemsTab.xaml
    /// </summary>
    public partial class ExaminedItemsTab : UserControl
    {
        public ExaminedItemsTab()
        {
            InitializeComponent();
        }

        private void FilterBox_TextChanged(object sender, TextChangedEventArgs e) =>
            ApplyFilter();

        private void ApplyFilter()
        {
            ICollectionView cv = CollectionViewSource.GetDefaultView(examinedGrid.ItemsSource);
            if (cv == null)
                return;
            if (string.IsNullOrEmpty(AppData.GridFilters.ExaminedItemsFilter))
                cv.Filter = null;
            else
            {
                cv.Filter = o =>
                {
                    ExaminedItem p = o as ExaminedItem;
                    return p.Name.ToUpper().Contains(AppData.GridFilters.ExaminedItemsFilter.ToUpper());
                };
            }
        }
    }
}