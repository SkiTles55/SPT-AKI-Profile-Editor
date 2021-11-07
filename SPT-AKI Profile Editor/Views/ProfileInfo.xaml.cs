using System.Text.RegularExpressions;
using System.Windows.Controls;
using System.Windows.Input;

namespace SPT_AKI_Profile_Editor.Views
{
    /// <summary>
    /// Логика взаимодействия для ProfileInfo.xaml
    /// </summary>
    public partial class ProfileInfo : UserControl
    {
        public ProfileInfo()
        {
            InitializeComponent();
            DataContext = new ProfileInfoViewModel();
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
