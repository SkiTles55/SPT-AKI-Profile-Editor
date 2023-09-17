using SPT_AKI_Profile_Editor.Core.ProfileClasses;
using SPT_AKI_Profile_Editor.Helpers;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace SPT_AKI_Profile_Editor.Views.ExtendedControls
{
    /// <summary>
    /// Логика взаимодействия для ItemsGrid.xaml
    /// </summary>
    public partial class ItemsGrid : GridControl
    {
        public static readonly DependencyProperty FilterNameProperty =
            DependencyProperty.Register(nameof(FilterName), typeof(string), typeof(ItemsGrid), new PropertyMetadata(null));

        public static readonly DependencyProperty FilterIdProperty =
            DependencyProperty.Register(nameof(FilterId), typeof(string), typeof(ItemsGrid), new PropertyMetadata(null));

        public static readonly DependencyProperty ShowHeadersProperty =
            DependencyProperty.Register(nameof(ShowHeaders), typeof(DataGridHeadersVisibility), typeof(ItemsGrid), new PropertyMetadata(DataGridHeadersVisibility.Column, null));

        public static readonly DependencyProperty HasItemsProperty =
            DependencyProperty.Register(nameof(HasItems), typeof(bool), typeof(ItemsGrid), new PropertyMetadata(false, null));

        public static readonly DependencyProperty PromptHorizontalAlignmentProperty =
            DependencyProperty.Register(nameof(PromptHorizontalAlignment), typeof(HorizontalAlignment), typeof(ItemsGrid), new PropertyMetadata(HorizontalAlignment.Center, null));

        public static readonly DependencyProperty PromptFontSizeProperty =
            DependencyProperty.Register(nameof(PromptFontSize), typeof(int), typeof(ItemsGrid), new PropertyMetadata(20, null));

        public static readonly DependencyProperty PromptFontWeightProperty =
            DependencyProperty.Register(nameof(PromptFontWeight), typeof(FontWeight), typeof(ItemsGrid), new PropertyMetadata(FontWeights.Bold, null));

        public static readonly DependencyProperty RemovingAllowedProperty =
            DependencyProperty.Register(nameof(RemovingAllowed), typeof(bool), typeof(ItemsGrid), new PropertyMetadata(true, null));

        public ItemsGrid()
        {
            InitializeComponent();
        }

        public string FilterName
        {
            get { return (string)GetValue(FilterNameProperty); }
            set { SetValue(FilterNameProperty, value); }
        }

        public string FilterId
        {
            get { return (string)GetValue(FilterIdProperty); }
            set { SetValue(FilterIdProperty, value); }
        }

        public DataGridHeadersVisibility ShowHeaders
        {
            get { return (DataGridHeadersVisibility)GetValue(ShowHeadersProperty); }
            set { SetValue(ShowHeadersProperty, value); }
        }

        public bool HasItems
        {
            get { return (bool)GetValue(HasItemsProperty); }
            set { SetValue(HasItemsProperty, value); }
        }

        public HorizontalAlignment PromptHorizontalAlignment
        {
            get { return (HorizontalAlignment)GetValue(PromptHorizontalAlignmentProperty); }
            set { SetValue(PromptHorizontalAlignmentProperty, value); }
        }

        public int PromptFontSize
        {
            get { return (int)GetValue(PromptFontSizeProperty); }
            set { SetValue(PromptFontSizeProperty, value); }
        }

        public FontWeight PromptFontWeight
        {
            get { return (FontWeight)GetValue(PromptFontWeightProperty); }
            set { SetValue(PromptFontWeightProperty, value); }
        }

        public bool RemovingAllowed
        {
            get { return (bool)GetValue(RemovingAllowedProperty); }
            set { SetValue(RemovingAllowedProperty, value); }
        }

        private void FilterBoxStash_TextChanged(object sender, TextChangedEventArgs e) =>
            ApplyStashFilter();

        private void ApplyStashFilter()
        {
            ICollectionView cv = CollectionViewSource.GetDefaultView(itemsGrid.ItemsSource);
            if (cv == null)
                return;
            if (string.IsNullOrEmpty(FilterName)
                && string.IsNullOrEmpty(FilterId))
                cv.Filter = null;
            else
            {
                cv.Filter = o =>
                {
                    InventoryItem p = o as InventoryItem;
                    return (string.IsNullOrEmpty(FilterName)
                    || p.LocalizedName.ToUpper().Contains(FilterName.ToUpper()))
                    && (string.IsNullOrEmpty(FilterId)
                    || p.Id.ToUpper().Contains(FilterId.ToUpper()));
                };
            }
        }
    }
}