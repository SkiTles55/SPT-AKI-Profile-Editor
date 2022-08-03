using System.Windows.Controls;

namespace SPT_AKI_Profile_Editor.Views
{
    /// <summary>
    /// Логика взаимодействия для SkillsTab.xaml
    /// </summary>
    public partial class SkillsTab : UserControl
    {
        public SkillsTab()
        {
            InitializeComponent();
            DataContext = new SkillsTabViewModel(App.DialogManager);
        }
    }
}