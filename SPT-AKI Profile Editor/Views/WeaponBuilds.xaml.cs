using System.Windows.Controls;

namespace SPT_AKI_Profile_Editor.Views
{
    /// <summary>
    /// Логика взаимодействия для WeaponBuilds.xaml
    /// </summary>
    public partial class WeaponBuilds : UserControl
    {
        public WeaponBuilds()
        {
            InitializeComponent();
            DataContext = new WeaponBuildsViewModel();
        }
    }
}