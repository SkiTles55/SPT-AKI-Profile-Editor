using SPT_AKI_Profile_Editor.Helpers;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text.RegularExpressions;
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

        public ItemsAdding()
        {
            InitializeComponent();
        }

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

        private void FilterBoxAdding_TextChanged(object sender, TextChangedEventArgs e) =>
            ApplyAddingFilter();

        private void ApplyAddingFilter()
        {
            ICollectionView cv = CollectionViewSource.GetDefaultView(itemsList.ItemsSource);
            if (cv == null)
                return;
            else
            {
                cv.Filter = o =>
                {
                    AddableCategory p = o as AddableCategory;
                    return p.ContainsItemsWithTextInName(FilterName);
                };
            }
        }

        private void SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (sender is TreeView treeView && treeView.SelectedItem != null && treeView.SelectedItem is AddableCategory category)
                selectedCategory.ItemsSource = category.Items;
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var textBox = sender as TextBox;
            if (string.IsNullOrEmpty(textBox.Text))
            {
                textBox.Text = "1";
                textBox.CaretIndex = 1;
                return;
            }
            if (int.TryParse(textBox.Text.Replace(",", ""), out int money))
            {
                if (money < 1)
                {
                    textBox.Text = "1";
                    textBox.CaretIndex = 1;
                }
            }
            else
            {
                textBox.Text = int.MaxValue.ToString();
                textBox.CaretIndex = textBox.Text.Length;
            }
        }
    }
}