using SPT_AKI_Profile_Editor.Core;
using SPT_AKI_Profile_Editor.Core.ServerClasses;
using SPT_AKI_Profile_Editor.Helpers;
using SPT_AKI_Profile_Editor.Views.ExtendedControls;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Data;

namespace SPT_AKI_Profile_Editor.Views
{
    /// <summary>
    /// Логика взаимодействия для CleaningFromMods.xaml
    /// </summary>
    public partial class CleaningFromMods : UserControl
    {
        public CleaningFromMods() => InitializeComponent();

        private void Expander_Expanded(object sender, System.Windows.RoutedEventArgs e)
        {
            if (sender is Expander entityType && entityType.DataContext is CollectionViewGroup grid)
            {
                var groupName = grid.Name.ToString();
                if (!AppData.GridFilters.CleaningFromModsTab.TypesExpander.ContainsKey(groupName))
                    AppData.GridFilters.CleaningFromModsTab.TypesExpander.Add(groupName, entityType.IsExpanded);
                else
                    AppData.GridFilters.CleaningFromModsTab.TypesExpander[groupName] = entityType.IsExpanded;
            }
        }

        private void Expander_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            if (sender is Expander entityType && entityType.DataContext is CollectionViewGroup grid)
            {
                var groupName = grid.Name.ToString();
                if (!AppData.GridFilters.CleaningFromModsTab.TypesExpander.ContainsKey(groupName))
                    AppData.GridFilters.CleaningFromModsTab.TypesExpander.Add(groupName, true);
                entityType.IsExpanded = AppData.GridFilters.CleaningFromModsTab.TypesExpander[groupName];
            }
        }

        private void FilterBox_TextChanged(object sender, TextChangedEventArgs e) =>
            ApplyFilter();

        private void ApplyFilter()
        {
            ICollectionView cv = CollectionViewSource.GetDefaultView(itemsGrid.ItemsSource);
            if (cv == null)
                return;
            var IdFilter = AppData.GridFilters.CleaningFromModsTab.IdFilter;
            if (string.IsNullOrEmpty(IdFilter))
                cv.Filter = null;
            else
            {
                cv.Filter = o =>
                {
                    ModdedEntity p = o as ModdedEntity;
                    return string.IsNullOrEmpty(IdFilter)
                    || p.Id.ToUpper().Contains(IdFilter.ToUpper());
                };
            }
        }
    }
}