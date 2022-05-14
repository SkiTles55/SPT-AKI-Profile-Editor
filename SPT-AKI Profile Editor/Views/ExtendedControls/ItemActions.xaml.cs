using SPT_AKI_Profile_Editor.Core.ProfileClasses;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace SPT_AKI_Profile_Editor.Views.ExtendedControls
{
    /// <summary>
    /// Логика взаимодействия для ItemActions.xaml
    /// </summary>
    public partial class ItemActions : UserControl
    {
        public static readonly DependencyProperty LocalizationProperty =
            DependencyProperty.Register(nameof(LocalizationDict), typeof(Dictionary<string, string>), typeof(ItemActions), new PropertyMetadata(null));

        public static readonly DependencyProperty OpenContainerProperty =
            DependencyProperty.Register(nameof(OpenContainer), typeof(ICommand), typeof(ItemActions), new PropertyMetadata(null));

        public static readonly DependencyProperty RemoveItemProperty =
            DependencyProperty.Register(nameof(RemoveItem), typeof(ICommand), typeof(ItemActions), new PropertyMetadata(null));

        public static readonly DependencyProperty ItemProperty =
            DependencyProperty.Register(nameof(Item), typeof(InventoryItem), typeof(ItemActions), new PropertyMetadata(null));

        public ItemActions()
        {
            InitializeComponent();
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

        public ICommand RemoveItem
        {
            get { return (ICommand)GetValue(RemoveItemProperty); }
            set { SetValue(RemoveItemProperty, value); }
        }

        public InventoryItem Item
        {
            get { return (InventoryItem)GetValue(ItemProperty); }
            set { SetValue(ItemProperty, value); }
        }
    }
}