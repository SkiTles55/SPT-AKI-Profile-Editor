using SPT_AKI_Profile_Editor.Core;
using SPT_AKI_Profile_Editor.Core.ProfileClasses;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Data;

namespace SPT_AKI_Profile_Editor.Views
{
    /// <summary>
    /// Логика взаимодействия для QuestsTab.xaml
    /// </summary>
    public partial class QuestsTab : UserControl
    {
        public QuestsTab()
        {
            InitializeComponent();
            DataContext = new QuestsTabViewModel();
        }

        private static bool FiltersIsEmpty() => string.IsNullOrEmpty(AppData.GridFilters.QuestsTab.QuestNameFilter)
                            && string.IsNullOrEmpty(AppData.GridFilters.QuestsTab.QuestStatusFilter)
                            && string.IsNullOrEmpty(AppData.GridFilters.QuestsTab.TraderNameFilter);

        private void FilterBox_TextChanged(object sender, TextChangedEventArgs e) => ApplyQuestFilter();

        private void ApplyQuestFilter()
        {
            ICollectionView cv = CollectionViewSource.GetDefaultView(questsGrid.ItemsSource);
            if (cv == null)
                return;
            if (FiltersIsEmpty())
                cv.Filter = null;
            else
            {
                cv.Filter = o =>
                {
                    CharacterQuest p = o as CharacterQuest;
                    return QuestNameContainsFilterText()
                    && QuestTraderNameContainsFilterText()
                    && QuestStatusContainsFilterText();

                    bool QuestNameContainsFilterText()
                    {
                        return string.IsNullOrEmpty(AppData.GridFilters.QuestsTab.QuestNameFilter)
                        || p.LocalizedQuestName.ToUpper().Contains(AppData.GridFilters.QuestsTab.QuestNameFilter.ToUpper());
                    }

                    bool QuestTraderNameContainsFilterText()
                    {
                        return string.IsNullOrEmpty(AppData.GridFilters.QuestsTab.TraderNameFilter)
                        || p.LocalizedTraderName.ToUpper().Contains(AppData.GridFilters.QuestsTab.TraderNameFilter.ToUpper());
                    }

                    bool QuestStatusContainsFilterText()
                    {
                        return string.IsNullOrEmpty(AppData.GridFilters.QuestsTab.QuestStatusFilter)
                        || p.Status.ToString().ToUpper().Contains(AppData.GridFilters.QuestsTab.QuestStatusFilter.ToUpper());
                    }
                };
            }
        }

        private void Expander_Expanded(object sender, System.Windows.RoutedEventArgs e)
        {
            if (sender is Expander questType && questType.DataContext is CollectionViewGroup grid)
            {
                var groupName = grid.Name.ToString();
                if (!AppData.GridFilters.QuestsTab.QuestTypesExpander.ContainsKey(groupName))
                    AppData.GridFilters.QuestsTab.QuestTypesExpander.Add(groupName, questType.IsExpanded);
                else
                    AppData.GridFilters.QuestsTab.QuestTypesExpander[groupName] = questType.IsExpanded;
            }
        }

        private void Expander_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            if (sender is Expander questType && questType.DataContext is CollectionViewGroup grid)
            {
                var groupName = grid.Name.ToString();
                if (!AppData.GridFilters.QuestsTab.QuestTypesExpander.ContainsKey(groupName))
                    AppData.GridFilters.QuestsTab.QuestTypesExpander.Add(groupName, true);
                questType.IsExpanded = AppData.GridFilters.QuestsTab.QuestTypesExpander[groupName];
            }
        }
    }
}