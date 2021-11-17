using SPT_AKI_Profile_Editor.Core;
using System;
using System.ComponentModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace SPT_AKI_Profile_Editor.Views
{
    /// <summary>
    /// Логика взаимодействия для SkillsTab.xaml
    /// </summary>
    public partial class SkillsTab : UserControl
    {
        public SkillsTab()
        {
            InitializeComponent();
            DataContext = new SkillsTabViewModel();
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void FilterBoxPmc_TextChanged(object sender, TextChangedEventArgs e) => ApplySkillsPmcFilter();
        private void ApplySkillsPmcFilter()
        {
            //ICollectionView cv = CollectionViewSource.GetDefaultView(skillsPmcGrid.ItemsSource);
            //if (string.IsNullOrEmpty(AppData.GridFilters.QuestsTab.QuestNameFilter)
            //    && string.IsNullOrEmpty(AppData.GridFilters.QuestsTab.QuestStatusFilter)
            //    && string.IsNullOrEmpty(AppData.GridFilters.QuestsTab.TraderNameFilter))
            //    cv.Filter = null;
            //else
            //{
            //    cv.Filter = o =>
            //    {
            //        CharacterQuest p = o as CharacterQuest;
            //        return (string.IsNullOrEmpty(AppData.GridFilters.QuestsTab.QuestNameFilter) || p.LocalizedQuestName.ToUpper().Contains(AppData.GridFilters.QuestsTab.QuestNameFilter.ToUpper()))
            //        && (string.IsNullOrEmpty(AppData.GridFilters.QuestsTab.TraderNameFilter) || p.LocalizedTraderName.ToUpper().Contains(AppData.GridFilters.QuestsTab.TraderNameFilter.ToUpper()))
            //        && (string.IsNullOrEmpty(AppData.GridFilters.QuestsTab.QuestStatusFilter) || p.Status.ToUpper().Contains(AppData.GridFilters.QuestsTab.QuestStatusFilter.ToUpper()));
            //    };
            //}
        }

        private void FilterBoxScav_TextChanged(object sender, TextChangedEventArgs e) => ApplySkillsScavFilter();
        private void ApplySkillsScavFilter()
        {
            //ICollectionView cv = CollectionViewSource.GetDefaultView(skillsScavGrid.ItemsSource);
            //if (string.IsNullOrEmpty(AppData.GridFilters.QuestsTab.QuestNameFilter)
            //    && string.IsNullOrEmpty(AppData.GridFilters.QuestsTab.QuestStatusFilter)
            //    && string.IsNullOrEmpty(AppData.GridFilters.QuestsTab.TraderNameFilter))
            //    cv.Filter = null;
            //else
            //{
            //    cv.Filter = o =>
            //    {
            //        CharacterQuest p = o as CharacterQuest;
            //        return (string.IsNullOrEmpty(AppData.GridFilters.QuestsTab.QuestNameFilter) || p.LocalizedQuestName.ToUpper().Contains(AppData.GridFilters.QuestsTab.QuestNameFilter.ToUpper()))
            //        && (string.IsNullOrEmpty(AppData.GridFilters.QuestsTab.TraderNameFilter) || p.LocalizedTraderName.ToUpper().Contains(AppData.GridFilters.QuestsTab.TraderNameFilter.ToUpper()))
            //        && (string.IsNullOrEmpty(AppData.GridFilters.QuestsTab.QuestStatusFilter) || p.Status.ToUpper().Contains(AppData.GridFilters.QuestsTab.QuestStatusFilter.ToUpper()));
            //    };
            //}
        }
    }
}
