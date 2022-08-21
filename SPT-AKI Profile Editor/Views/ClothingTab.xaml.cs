using SPT_AKI_Profile_Editor.Core;
using SPT_AKI_Profile_Editor.Core.ServerClasses;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Data;

namespace SPT_AKI_Profile_Editor.Views
{
    /// <summary>
    /// Логика взаимодействия для ClothingTab.xaml
    /// </summary>
    public partial class ClothingTab : UserControl
    {
        public ClothingTab()
        {
            InitializeComponent();
        }

        private void FilterBox_TextChanged(object sender, TextChangedEventArgs e) =>
            ApplyFilter();

        private void ApplyFilter()
        {
            ICollectionView cv = CollectionViewSource.GetDefaultView(clothingGrid.ItemsSource);
            if (cv == null)
                return;
            if (string.IsNullOrEmpty(AppData.GridFilters.ClothingNameFilter))
                cv.Filter = null;
            else
            {
                cv.Filter = o =>
                {
                    TraderSuit p = o as TraderSuit;
                    return p.LocalizedName.ToUpper().Contains(AppData.GridFilters.ClothingNameFilter.ToUpper());
                };
            }
        }
    }
}