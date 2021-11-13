using System.Text.RegularExpressions;
using System.Windows.Controls;
using System.Windows.Input;

namespace SPT_AKI_Profile_Editor.Views
{
    /// <summary>
    /// Логика взаимодействия для MerchantsTab.xaml
    /// </summary>
    public partial class MerchantsTab : UserControl
    {
        public MerchantsTab()
        {
            InitializeComponent();
            DataContext = new MerchantsTabViewModel();
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
