using SPT_AKI_Profile_Editor.Core.ProfileClasses;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace SPT_AKI_Profile_Editor.Views.ExtendedControls
{
    /// <summary>
    /// Логика взаимодействия для WeaponBuildsList.xaml
    /// </summary>
    public partial class WeaponBuildsList : UserControl
    {
        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register(nameof(ItemsSource), typeof(ObservableCollection<KeyValuePair<string, WeaponBuild>>), typeof(WeaponBuildsList), new PropertyMetadata(null));

        public static readonly DependencyProperty FilterValueProperty =
            DependencyProperty.Register(nameof(FilterValue), typeof(string), typeof(WeaponBuildsList), new PropertyMetadata(null));

        public static readonly DependencyProperty RemoveAllowedProperty =
            DependencyProperty.Register(nameof(RemoveAllowed), typeof(bool), typeof(WeaponBuildsList), new PropertyMetadata(true, null));

        public WeaponBuildsList()
        {
            InitializeComponent();
        }

        public ObservableCollection<KeyValuePair<string, WeaponBuild>> ItemsSource
        {
            get { return (ObservableCollection<KeyValuePair<string, WeaponBuild>>)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        public string FilterValue
        {
            get { return (string)GetValue(FilterValueProperty); }
            set { SetValue(FilterValueProperty, value); }
        }

        public bool RemoveAllowed
        {
            get { return (bool)GetValue(FilterValueProperty); }
            set { SetValue(FilterValueProperty, value); }
        }

        private static void ApplyFilter(IEnumerable source, string filter)
        {
            ICollectionView cv = CollectionViewSource.GetDefaultView(source);
            if (cv == null)
                return;
            if (string.IsNullOrEmpty(filter))
                cv.Filter = null;
            else
            {
                cv.Filter = o =>
                {
                    WeaponBuild p = o as WeaponBuild;
                    return p.Name.ToUpper().Contains(filter.ToUpper());
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

        private void FilterBox_TextChanged(object sender, TextChangedEventArgs e) =>
            ApplyFilter(itemsList.ItemsSource, FilterValue);
    }
}