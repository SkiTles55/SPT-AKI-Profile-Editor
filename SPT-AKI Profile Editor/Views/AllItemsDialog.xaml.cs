using SPT_AKI_Profile_Editor.Helpers;
using System;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Data;

namespace SPT_AKI_Profile_Editor.Views
{
    /// <summary>
    /// Логика взаимодействия для AllItemsDialog.xaml
    /// </summary>
    public partial class AllItemsDialog : UserControl
    {
        public AllItemsDialog() => InitializeComponent();

        private void ApplyAddingFilter()
        {
            ICollectionView cv = CollectionViewSource.GetDefaultView(itemsList.ItemsSource);
            if (cv == null)
                return;
            cv.Filter = o => (o as AddableItem).ContainsText(searchBox.SearchText, searchBox.SearchInDescriptions);
        }

        private void SearchParamsChanged(object sender, EventArgs e) => ApplyAddingFilter();
    }
}