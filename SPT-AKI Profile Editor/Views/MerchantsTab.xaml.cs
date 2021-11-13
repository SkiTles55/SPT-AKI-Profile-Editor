using System.Windows.Controls;

namespace SPT_AKI_Profile_Editor.Views
{
    /// <summary>
    /// Логика взаимодействия для MerchantsTab.xaml
    /// </summary>
    public partial class MerchantsTab : UserControl
    {
        public MerchantsTab()
        {
            InitializeComponent();
            DataContext = new MerchantsTabViewModel();
        }
    }
}
