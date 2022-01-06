using SPT_AKI_Profile_Editor.Core;
using SPT_AKI_Profile_Editor.Core.ProfileClasses;
using SPT_AKI_Profile_Editor.Core.ServerClasses;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace SPT_AKI_Profile_Editor.Views
{
    /// <summary>
    /// Логика взаимодействия для StashTab.xaml
    /// </summary>
    public partial class StashTab : UserControl
    {
        public StashTab()
        {
            InitializeComponent();
            DataContext = new StashTabViewModel();
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void FilterBoxStash_TextChanged(object sender, TextChangedEventArgs e) =>
            ApplyStashFilter();

        private void FilterBoxAdding_TextChanged(object sender, TextChangedEventArgs e) =>
            ApplyAddingFilter();

        private void ApplyAddingFilter()
        {
            ICollectionView cv = CollectionViewSource.GetDefaultView(itemsGrid.ItemsSource);
            if (cv == null)
                return;
            if (string.IsNullOrEmpty(AppData.GridFilters.StashTab.AddingItemName))
                cv.Filter = null;
            else
            {
                cv.Filter = o =>
                {
                    TarkovItem p = o as TarkovItem;
                    return string.IsNullOrEmpty(AppData.GridFilters.StashTab.AddingItemName)
                    || p.LocalizedName.ToUpper().Contains(AppData.GridFilters.StashTab.AddingItemName.ToUpper())
                    || p.LocalizedSubGroupName.ToUpper().Contains(AppData.GridFilters.StashTab.AddingItemName.ToUpper())
                    || p.LocalizedGroupName.ToUpper().Contains(AppData.GridFilters.StashTab.AddingItemName.ToUpper());
                };
            }
        }

        private void ApplyStashFilter()
        {
            ICollectionView cv = CollectionViewSource.GetDefaultView(stashGrid.ItemsSource);
            if (cv == null)
                return;
            if (string.IsNullOrEmpty(AppData.GridFilters.StashTab.StashItemName)
                && string.IsNullOrEmpty(AppData.GridFilters.StashTab.Id))
                cv.Filter = null;
            else
            {
                cv.Filter = o =>
                {
                    InventoryItem p = o as InventoryItem;
                    return (string.IsNullOrEmpty(AppData.GridFilters.StashTab.StashItemName)
                    || p.LocalizedName.ToUpper().Contains(AppData.GridFilters.StashTab.StashItemName.ToUpper()))
                    && (string.IsNullOrEmpty(AppData.GridFilters.StashTab.Id)
                    || p.Id.ToUpper().Contains(AppData.GridFilters.StashTab.Id.ToUpper()));
                };
            }
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

        private void ScrollViewer_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            ScrollViewer scrollviewer = sender as ScrollViewer;
            if (e.Delta > 0)
                scrollviewer.LineUp();
            else
                scrollviewer.LineDown();
            e.Handled = true;
        }
    }
}
