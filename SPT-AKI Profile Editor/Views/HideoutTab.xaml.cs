using SPT_AKI_Profile_Editor.Core;
using SPT_AKI_Profile_Editor.Core.ProfileClasses;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace SPT_AKI_Profile_Editor.Views
{
    /// <summary>
    /// Логика взаимодействия для HideoutTab.xaml
    /// </summary>
    public partial class HideoutTab : UserControl
    {
        public HideoutTab()
        {
            InitializeComponent();
            DataContext = new HideoutTabViewModel();
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void FilterBox_TextChanged(object sender, TextChangedEventArgs e) => ApplyHideoutFilter();

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
    }
}