using SPT_AKI_Profile_Editor.Core;
using SPT_AKI_Profile_Editor.Core.ProfileClasses;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Data;

namespace SPT_AKI_Profile_Editor.Views
{
    /// <summary>
    /// Логика взаимодействия для HideoutTab.xaml
    /// </summary>
    public partial class HideoutTab : UserControl
    {
        public HideoutTab() => InitializeComponent();

        private static bool CraftFiltersIsEmpty()
        {
            return string.IsNullOrEmpty(AppData.GridFilters.HideoutTab.CraftProductionNameFilter)
                && string.IsNullOrEmpty(AppData.GridFilters.HideoutTab.CraftAreaNameFilter);
        }

        private void FilterBox_TextChanged(object sender, TextChangedEventArgs e) => ApplyHideoutFilter();

        private void CraftsFilterBox_TextChanged(object sender, TextChangedEventArgs e) => ApplyCraftsFilter();

        private void ApplyHideoutFilter()
        {
            ICollectionView cv = CollectionViewSource.GetDefaultView(hideoutGrid.ItemsSource);
            if (cv == null)
                return;
            if (string.IsNullOrEmpty(AppData.GridFilters.HideoutTab.AreaNameFilter))
                cv.Filter = null;
            else
            {
                cv.Filter = o =>
                {
                    HideoutArea p = o as HideoutArea;
                    return p.LocalizedName.ToUpper().Contains(AppData.GridFilters.HideoutTab.AreaNameFilter.ToUpper());
                };
            }
        }

        private void ApplyCraftsFilter()
        {
            ICollectionView cv = CollectionViewSource.GetDefaultView(hideoutProductions.ItemsSource);
            if (cv == null)
                return;
            if (CraftFiltersIsEmpty())
                cv.Filter = null;
            else
            {
                cv.Filter = o =>
                {
                    CharacterHideoutProduction p = o as CharacterHideoutProduction;
                    return ProductItemNameContainsFilterText() && AreaLocalizedNameContainsFilterText();

                    bool ProductItemNameContainsFilterText()
                    {
                        return string.IsNullOrEmpty(AppData.GridFilters.HideoutTab.CraftProductionNameFilter)
                        || p.ProductItem.Name.ToUpper().Contains(AppData.GridFilters.HideoutTab.CraftProductionNameFilter.ToUpper());
                    }

                    bool AreaLocalizedNameContainsFilterText()
                    {
                        return string.IsNullOrEmpty(AppData.GridFilters.HideoutTab.CraftAreaNameFilter)
                        || p.AreaLocalizedName.ToUpper().Contains(AppData.GridFilters.HideoutTab.CraftAreaNameFilter.ToUpper());
                    }
                };
            }
        }
    }
}