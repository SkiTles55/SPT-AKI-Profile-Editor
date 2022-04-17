using System.Windows.Controls;
using System.Windows.Input;

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

        private void ScrollViewer_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (sender is ScrollViewer scrollViewer)
            {
                if (e.Delta > 0)
                    scrollViewer.LineUp();
                else
                    scrollViewer.LineDown();
                e.Handled = true;
            }
        }
    }
}