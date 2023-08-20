using SPT_AKI_Profile_Editor.Core.Equipment;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace SPT_AKI_Profile_Editor.Views.ExtendedControls
{
    /// <summary>
    /// Логика взаимодействия для EquipmentSlotsList.xaml
    /// </summary>
    public partial class EquipmentSlotsList : UserControl
    {
        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register(nameof(ItemsSource), typeof(IEnumerable<EquipmentSlot>), typeof(EquipmentSlotsList), new PropertyMetadata(null));

        public EquipmentSlotsList() => InitializeComponent();

        public IEnumerable<EquipmentSlot> ItemsSource
        {
            get { return (IEnumerable<EquipmentSlot>)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }
    }
}