using SPT_AKI_Profile_Editor.Helpers;
using System.Windows;
using System.Windows.Input;

namespace SPT_AKI_Profile_Editor.Views.ExtendedControls
{
    /// <summary>
    /// Логика взаимодействия для ItemsListBox.xaml
    /// </summary>
    public partial class ItemsListBox : GridControl
    {
        public static readonly DependencyProperty AddItemsBlockedProperty =
            DependencyProperty.Register(nameof(AddItemsBlocked), typeof(bool), typeof(ItemsListBox), new PropertyMetadata(false));

        public static readonly DependencyProperty StashSelectorVisibleProperty =
            DependencyProperty.Register(nameof(StashSelectorVisible), typeof(bool), typeof(ItemsListBox), new PropertyMetadata(true));

        public static readonly DependencyProperty AddItemCommandProperty =
            DependencyProperty.Register(nameof(AddItemCommand), typeof(ICommand), typeof(ItemsListBox), new PropertyMetadata(null));

        public ItemsListBox() => InitializeComponent();

        public bool AddItemsBlocked
        {
            get { return (bool)GetValue(AddItemsBlockedProperty); }
            set { SetValue(AddItemsBlockedProperty, value); }
        }

        public bool StashSelectorVisible
        {
            get { return (bool)GetValue(StashSelectorVisibleProperty); }
            set { SetValue(StashSelectorVisibleProperty, value); }
        }

        public ICommand AddItemCommand
        {
            get { return (ICommand)GetValue(AddItemCommandProperty); }
            set { SetValue(AddItemCommandProperty, value); }
        }
    }
}