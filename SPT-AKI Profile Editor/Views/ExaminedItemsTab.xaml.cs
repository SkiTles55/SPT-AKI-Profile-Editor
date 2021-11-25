using SPT_AKI_Profile_Editor.Core;
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
            DataContext = new ExaminedItemsTabViewModel();
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
                    string p = o as string;
                    return p.ToUpper().Contains(AppData.GridFilters.ExaminedItemsFilter.ToUpper());
                };
            }
        }
    }
}
