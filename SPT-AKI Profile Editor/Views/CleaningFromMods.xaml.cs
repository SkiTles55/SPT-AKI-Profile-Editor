using SPT_AKI_Profile_Editor.Core;
using SPT_AKI_Profile_Editor.Helpers;
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
            var idFilter = AppData.GridFilters.CleaningFromModsTab.IdFilter;
            var tplFilter = AppData.GridFilters.CleaningFromModsTab.TplFilter;
            if (string.IsNullOrEmpty(idFilter) && string.IsNullOrEmpty(tplFilter))
                cv.Filter = null;
            else
            {
                cv.Filter = o =>
                {
                    ModdedEntity p = o as ModdedEntity;
                    return (string.IsNullOrEmpty(idFilter) || p.Id.ToUpper().Contains(idFilter.ToUpper()))
                    && (string.IsNullOrEmpty(tplFilter) || (p.Tpl?.ToUpper().Contains(tplFilter.ToUpper()) ?? false));
                };
            }
        }
    }
}