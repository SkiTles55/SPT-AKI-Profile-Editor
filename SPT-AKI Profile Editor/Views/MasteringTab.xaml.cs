using System.Windows.Controls;

namespace SPT_AKI_Profile_Editor.Views
{
    /// <summary>
    /// Логика взаимодействия для MasteringTab.xaml
    /// </summary>
    public partial class MasteringTab : UserControl
    {
        public MasteringTab()
        {
            InitializeComponent();
            DataContext = new MasteringTabViewModel(App.DialogManager);
        }
    }
}