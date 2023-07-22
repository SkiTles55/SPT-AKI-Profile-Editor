using SPT_AKI_Profile_Editor.Helpers;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace SPT_AKI_Profile_Editor.Views.ExtendedControls
{
    /// <summary>
    /// Логика взаимодействия для ItemsAdding.xaml
    /// </summary>
    public partial class ItemsAdding : UserControl
    {
        public static readonly DependencyProperty FilterNameProperty =
            DependencyProperty.Register(nameof(FilterName), typeof(string), typeof(ItemsAdding), new PropertyMetadata(null));

        public static readonly DependencyProperty CategoriesForItemsAddingProperty =
            DependencyProperty.Register(nameof(CategoriesForItemsAdding), typeof(IEnumerable<AddableCategory>), typeof(ItemsAdding), new PropertyMetadata(null));

        public static readonly DependencyProperty AddItemCommandProperty =
            DependencyProperty.Register(nameof(AddItemCommand), typeof(ICommand), typeof(ItemsAdding), new PropertyMetadata(null));

        public static readonly DependencyProperty AddItemsBlockedProperty =
            DependencyProperty.Register(nameof(AddItemsBlocked), typeof(bool), typeof(ItemsAdding), new PropertyMetadata(false));

        public static readonly DependencyProperty FilterDescriptionsProperty =
            DependencyProperty.Register(nameof(FilterDescriptions), typeof(bool), typeof(ItemsAdding), new PropertyMetadata(false));

        public ItemsAdding() => InitializeComponent();

        public string FilterName
        {
            get { return (string)GetValue(FilterNameProperty); }
            set { SetValue(FilterNameProperty, value); }
        }

        public IEnumerable<AddableCategory> CategoriesForItemsAdding
        {
            get { return (IEnumerable<AddableCategory>)GetValue(CategoriesForItemsAddingProperty); }
            set { SetValue(CategoriesForItemsAddingProperty, value); }
        }

        public ICommand AddItemCommand
        {
            get { return (ICommand)GetValue(AddItemCommandProperty); }
            set { SetValue(AddItemCommandProperty, value); }
        }

        public bool AddItemsBlocked
        {
            get { return (bool)GetValue(AddItemsBlockedProperty); }
            set { SetValue(AddItemsBlockedProperty, value); }
        }

        public bool FilterDescriptions
        {
            get { return (bool)GetValue(FilterDescriptionsProperty); }
            set { SetValue(FilterDescriptionsProperty, value); }
        }

        private void ApplyAddingFilter()
        {
            ICollectionView cv = CollectionViewSource.GetDefaultView(itemsList.ItemsSource);
            if (cv == null)
                return;
            cv.Filter = o => (o as AddableCategory).ContainsItemsWithTextInName(FilterName ?? "", FilterDescriptions);
        }

        private void SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (sender is TreeView treeView && treeView.SelectedItem != null && treeView.SelectedItem is AddableCategory category)
                selectedCategory.ItemsSource = category.Items;
        }

        private void SearchParamsChanged(object sender, System.EventArgs e) => ApplyAddingFilter();
    }
}