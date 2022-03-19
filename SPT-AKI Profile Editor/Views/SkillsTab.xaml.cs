using SPT_AKI_Profile_Editor.Core;
using SPT_AKI_Profile_Editor.Core.ProfileClasses;
using System.Collections;
using System.ComponentModel;
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

        private static void ApplyFilter(IEnumerable source, string filter)
        {
            ICollectionView cv = CollectionViewSource.GetDefaultView(source);
            if (cv == null)
                return;
            if (string.IsNullOrEmpty(filter))
                cv.Filter = null;
            else
            {
                cv.Filter = o =>
                {
                    CharacterSkill p = o as CharacterSkill;
                    return p.LocalizedName.ToUpper().Contains(filter.ToUpper());
                };
            }
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void FilterBoxPmc_TextChanged(object sender, TextChangedEventArgs e) =>
            ApplyFilter(skillsPmcGrid.ItemsSource, AppData.GridFilters.SkillsTab.SkillNamePmcFilter);

        private void FilterBoxScav_TextChanged(object sender, TextChangedEventArgs e) =>
            ApplyFilter(skillsScavGrid.ItemsSource, AppData.GridFilters.SkillsTab.SkillNameScavFilter);
    }
}