using System.Diagnostics;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace SPT_AKI_Profile_Editor.Views
{
    /// <summary>
    /// Логика взаимодействия для AboutTab.xaml
    /// </summary>
    public partial class AboutTab : UserControl
    {
        public AboutTab()
        {
            InitializeComponent();
            DataContext = new AboutTabViewModel();
        }

        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            ProcessStartInfo link = new (e.Uri.AbsoluteUri);
            link.UseShellExecute = true;
            Process.Start(link);
            e.Handled = true;
        }
    }
}
