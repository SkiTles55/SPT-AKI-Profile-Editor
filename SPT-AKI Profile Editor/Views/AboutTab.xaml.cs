using System.Windows.Controls;

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
            DataContext = new AboutTabViewModel(App.ApplicationManager);
        }
    }
}