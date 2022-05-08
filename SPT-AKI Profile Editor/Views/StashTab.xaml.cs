using System.Windows.Controls;

namespace SPT_AKI_Profile_Editor.Views
{
    /// <summary>
    /// Логика взаимодействия для StashTab.xaml
    /// </summary>
    public partial class StashTab : UserControl
    {
        public StashTab()
        {
            InitializeComponent();
            DataContext = new StashTabViewModel();
        }
    }
}