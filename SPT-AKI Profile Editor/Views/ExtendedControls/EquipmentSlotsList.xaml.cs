using SPT_AKI_Profile_Editor.Core.Equipment;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace SPT_AKI_Profile_Editor.Views.ExtendedControls
{
    /// <summary>
    /// Логика взаимодействия для EquipmentSlotsList.xaml
    /// </summary>
    public partial class EquipmentSlotsList : UserControl
    {
        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register(nameof(ItemsSource), typeof(IEnumerable<EquipmentSlot>), typeof(EquipmentSlotsList), new PropertyMetadata(null));

        public static readonly DependencyProperty LocalizationProperty =
            DependencyProperty.Register(nameof(LocalizationDict), typeof(Dictionary<string, string>), typeof(EquipmentSlotsList), new PropertyMetadata(null));

        public static readonly DependencyProperty OpenContainerProperty =
            DependencyProperty.Register(nameof(OpenContainer), typeof(ICommand), typeof(EquipmentSlotsList), new PropertyMetadata(null));

        public static readonly DependencyProperty InspectWeaponProperty =
            DependencyProperty.Register(nameof(InspectWeapon), typeof(ICommand), typeof(EquipmentSlotsList), new PropertyMetadata(null));

        public static readonly DependencyProperty RemoveItemProperty =
            DependencyProperty.Register(nameof(RemoveItem), typeof(ICommand), typeof(EquipmentSlotsList), new PropertyMetadata(null));

        public EquipmentSlotsList() => InitializeComponent();

        public IEnumerable<EquipmentSlot> ItemsSource
        {
            get { return (IEnumerable<EquipmentSlot>)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        public Dictionary<string, string> LocalizationDict
        {
            get { return (Dictionary<string, string>)GetValue(LocalizationProperty); }
            set { SetValue(LocalizationProperty, value); }
        }

        public ICommand OpenContainer
        {
            get { return (ICommand)GetValue(OpenContainerProperty); }
            set { SetValue(OpenContainerProperty, value); }
        }

        public ICommand InspectWeapon
        {
            get { return (ICommand)GetValue(InspectWeaponProperty); }
            set { SetValue(InspectWeaponProperty, value); }
        }

        public ICommand RemoveItem
        {
            get { return (ICommand)GetValue(RemoveItemProperty); }
            set { SetValue(RemoveItemProperty, value); }
        }
    }
}