using System.Windows.Controls;

namespace SPT_AKI_Profile_Editor.Views
{
    /// <summary>
    /// Логика взаимодействия для BackupsTab.xaml
    /// </summary>
    public partial class BackupsTab : UserControl
    {
        public BackupsTab()
        {
            InitializeComponent();
            DataContext = new BackupsTabViewModel();
        }
    }
}
