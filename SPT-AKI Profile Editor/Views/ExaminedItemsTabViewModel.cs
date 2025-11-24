using SPT_AKI_Profile_Editor.Core.HelperClasses;
using SPT_AKI_Profile_Editor.Core.ProfileClasses;
using SPT_AKI_Profile_Editor.Helpers;
using System.Collections.ObjectModel;
using System.Linq;

namespace SPT_AKI_Profile_Editor.Views
{
    public class ExaminedItemsTabViewModel(IDialogManager dialogManager) : PmcBindableViewModel
    {
        private ObservableCollection<ExaminedItem> examinedItems = [];
        private string nameFilter;

        public static RelayCommand ExamineAllCommand => new(obj => Profile.Characters.Pmc.ExamineAll());

        public RelayCommand RemoveItemCommand => new(obj =>
        {
            if (obj is string id)
                RemoveItem(id);
        });

        public ObservableCollection<ExaminedItem> ExaminedItems
        {
            get => examinedItems;
            set
            {
                examinedItems = value;
                OnPropertyChanged(nameof(ExaminedItems));
            }
        }

        public string NameFilter
        {
            get => nameFilter;
            set
            {
                nameFilter = value;
                OnPropertyChanged(nameof(NameFilter));
                ApplyFilter();
            }
        }

        public override void ApplyFilter()
        {
            ObservableCollection<ExaminedItem> filteredItems;

            if (Profile?.Characters?.Pmc?.ExaminedItems == null || !Profile.Characters.Pmc.ExaminedItems.Any())
                filteredItems = [];
            else if (string.IsNullOrEmpty(NameFilter))
                filteredItems = new(Profile.Characters.Pmc.ExaminedItems);
            else
                filteredItems = new(Profile.Characters.Pmc.ExaminedItems.Where(x => x.Name.Contains(NameFilter, System.StringComparison.CurrentCultureIgnoreCase)));

            ExaminedItems = filteredItems;
        }

        private async void RemoveItem(string id)
        {
            if (await dialogManager.YesNoDialog("remove_examined_item_title", "remove_examined_item_caption"))
                Profile?.Characters?.Pmc?.RemoveExaminedItem(id);
        }
    }
}