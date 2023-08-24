using SPT_AKI_Profile_Editor.Core.ProfileClasses;
using System.Collections;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace SPT_AKI_Profile_Editor.Views.ExtendedControls
{
    /// <summary>
    /// Логика взаимодействия для EquipmentBuildsList.xaml
    /// </summary>
    public partial class EquipmentBuildsList : UserControl
    {
        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register(nameof(ItemsSource), typeof(ObservableCollection<EquipmentBuild>), typeof(EquipmentBuildsList), new PropertyMetadata(null));

        public static readonly DependencyProperty FilterValueProperty =
            DependencyProperty.Register(nameof(FilterValue), typeof(string), typeof(EquipmentBuildsList), new PropertyMetadata(null));

        public static readonly DependencyProperty RemoveAllowedProperty =
            DependencyProperty.Register(nameof(RemoveAllowed), typeof(bool), typeof(EquipmentBuildsList), new PropertyMetadata(true, null));

        public EquipmentBuildsList() => InitializeComponent();

        public ObservableCollection<EquipmentBuild> ItemsSource
        {
            get { return (ObservableCollection<EquipmentBuild>)GetValue(ItemsSourceProperty); }
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
                cv.Filter = o => o is not EquipmentBuild p || p.Name.ToUpper().Contains(filter.ToUpper());
        }

        private void FilterBox_TextChanged(object sender, TextChangedEventArgs e)
            => ApplyFilter(itemsList.ItemsSource, FilterValue);

        private void ListBoxTargetUpdated(object sender, DataTransferEventArgs e)
            => ApplyFilter(itemsList.ItemsSource, FilterValue);
    }
}