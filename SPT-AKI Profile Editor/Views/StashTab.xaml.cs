using SPT_AKI_Profile_Editor.Core;
using SPT_AKI_Profile_Editor.Core.ProfileClasses;
using System.Collections;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Data;

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

        private void FilterBox_TextChanged(object sender, TextChangedEventArgs e) =>
            ApplyFilter(stashGrid.ItemsSource);

        private static void ApplyFilter(IEnumerable source)
        {
            ICollectionView cv = CollectionViewSource.GetDefaultView(source);
            if (string.IsNullOrEmpty(AppData.GridFilters.StashTab.Name)
                && string.IsNullOrEmpty(AppData.GridFilters.StashTab.Id))
                cv.Filter = null;
            else
            {
                cv.Filter = o =>
                {
                    InventoryItem p = o as InventoryItem;
                    return (string.IsNullOrEmpty(AppData.GridFilters.StashTab.Name)
                    || p.LocalizedName.ToUpper().Contains(AppData.GridFilters.StashTab.Name.ToUpper()))
                    && (string.IsNullOrEmpty(AppData.GridFilters.StashTab.Id)
                    || p.Id.ToUpper().Contains(AppData.GridFilters.StashTab.Id.ToUpper()));
                };
            }
        }
    }
}
