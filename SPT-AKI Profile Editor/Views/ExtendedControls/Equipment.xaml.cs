using SPT_AKI_Profile_Editor.Core.ProfileClasses;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace SPT_AKI_Profile_Editor.Views.ExtendedControls
{
    /// <summary>
    /// Логика взаимодействия для Equipment.xaml
    /// </summary>
    public partial class Equipment : UserControl
    {
        public static readonly DependencyProperty InventoryEquipmentProperty =
            DependencyProperty.Register(nameof(InventoryEquipment), typeof(CharacterInventory), typeof(Equipment), new PropertyMetadata(null));

        public Equipment()
        {
            InitializeComponent();
        }

        public CharacterInventory InventoryEquipment
        {
            get { return (CharacterInventory)GetValue(InventoryEquipmentProperty); }
            set { SetValue(InventoryEquipmentProperty, value); }
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