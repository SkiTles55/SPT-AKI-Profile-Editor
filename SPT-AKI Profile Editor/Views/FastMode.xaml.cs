using MahApps.Metro.Controls;
using System.Text.RegularExpressions;
using System.Windows.Input;

namespace SPT_AKI_Profile_Editor.Views
{
    /// <summary>
    /// Логика взаимодействия для FastMode.xaml
    /// </summary>
    public sealed partial class FastMode : Flyout
    {
        public FastMode()
        {
            InitializeComponent();
            DataContext = new FastModeViewModel();
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}