using System.Windows;
using System.Windows.Controls;

namespace SPT_AKI_Profile_Editor.Views.ExtendedControls
{
    /// <summary>
    /// Логика взаимодействия для TextField.xaml
    /// </summary>
    public partial class TextField : UserControl
    {
        public TextField() => InitializeComponent();

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is string text)
                Clipboard.SetText(text);
        }
    }
}