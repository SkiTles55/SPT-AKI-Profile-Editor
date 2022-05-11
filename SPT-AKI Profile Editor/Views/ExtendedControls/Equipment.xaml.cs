using SPT_AKI_Profile_Editor.Core.ProfileClasses;
using System.Windows;
using System.Windows.Controls;

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
    }
}