using System;
using System.Windows;
using System.Windows.Controls;

namespace SPT_AKI_Profile_Editor.Views.ExtendedControls
{
    /// <summary>
    /// Логика взаимодействия для ItemsFilter.xaml
    /// </summary>
    public partial class ItemsSearchBox : UserControl
    {
        public static readonly DependencyProperty SearchTitleProperty =
            DependencyProperty.Register(nameof(SearchTitle), typeof(string), typeof(ItemsSearchBox), new PropertyMetadata(""));

        public static readonly DependencyProperty SearchTextProperty =
            DependencyProperty.Register(nameof(SearchText), typeof(string), typeof(ItemsSearchBox), new PropertyMetadata(""));

        public static readonly DependencyProperty SearchInDescriptionsProperty =
            DependencyProperty.Register(nameof(SearchInDescriptions), typeof(bool), typeof(ItemsSearchBox), new PropertyMetadata(false));

        public static readonly DependencyProperty SearchInDescriptionsTitleProperty =
            DependencyProperty.Register(nameof(SearchInDescriptionsTitle), typeof(string), typeof(ItemsSearchBox), new PropertyMetadata(""));

        public ItemsSearchBox() => InitializeComponent();

        public event EventHandler SearchParamsChanged;

        public string SearchText
        {
            get { return (string)GetValue(SearchTextProperty); }
            set { SetValue(SearchTextProperty, value); }
        }

        public string SearchTitle
        {
            get { return (string)GetValue(SearchTitleProperty); }
            set { SetValue(SearchTitleProperty, value); }
        }

        public bool SearchInDescriptions
        {
            get { return (bool)GetValue(SearchInDescriptionsProperty); }
            set { SetValue(SearchInDescriptionsProperty, value); }
        }

        public string SearchInDescriptionsTitle
        {
            get { return (string)GetValue(SearchInDescriptionsTitleProperty); }
            set { SetValue(SearchInDescriptionsTitleProperty, value); }
        }

        private void FilterBoxAdding_TextChanged(object sender, TextChangedEventArgs e) =>
            SearchParamsChanged?.Invoke(this, EventArgs.Empty);

        private void CheckBox_StateChanged(object sender, RoutedEventArgs e) =>
            SearchParamsChanged?.Invoke(this, EventArgs.Empty);
    }
}