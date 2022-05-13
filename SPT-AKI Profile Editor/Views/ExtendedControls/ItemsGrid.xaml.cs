using SPT_AKI_Profile_Editor.Core;
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