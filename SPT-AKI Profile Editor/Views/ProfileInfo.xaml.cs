using System.Windows.Controls;

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
    }
}
