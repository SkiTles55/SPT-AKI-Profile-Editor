using SPT_AKI_Profile_Editor.Core.ServerClasses;
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
            DependencyProperty.Register(nameof(CategoriesForItemsAdding), typeof(IEnumerable<HandbookCategory>), typeof(ItemsAdding), new PropertyMetadata(null));

        public ItemsAdding()
        {
            InitializeComponent();
        }

        public string FilterName
        {
            get { return (string)GetValue(FilterNameProperty); }
            set { SetValue(FilterNameProperty, value); }
        }

        public IEnumerable<HandbookCategory> CategoriesForItemsAdding
        {
            get { return (IEnumerable<HandbookCategory>)GetValue(CategoriesForItemsAddingProperty); }
            set { SetValue(CategoriesForItemsAddingProperty, value); }
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
                    HandbookCategory p = o as HandbookCategory;
                    return p.ContainsItemsWithTextInName(FilterName);
                };
            }
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