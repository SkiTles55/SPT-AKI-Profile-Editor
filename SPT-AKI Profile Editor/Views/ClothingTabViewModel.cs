using SPT_AKI_Profile_Editor.Core.ServerClasses;
using SPT_AKI_Profile_Editor.Helpers;
using System.Collections.ObjectModel;
using System.Linq;

namespace SPT_AKI_Profile_Editor.Views
{
    public class ClothingTabViewModel : PmcBindableViewModel
    {
        private ObservableCollection<TraderSuit> suits = new();
        private string nameFilter;

        public static RelayCommand AcquireAllCommand => new(obj => ServerDatabase.AcquireAllClothing());

        public ObservableCollection<TraderSuit> Suits
        {
            get => suits;
            set
            {
                suits = value;
                OnPropertyChanged(nameof(Suits));
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
            ObservableCollection<TraderSuit> filteredItems;

            if (ServerDatabase?.TraderSuits == null || !ServerDatabase.TraderSuits.Any())
                filteredItems = new();
            else if (string.IsNullOrEmpty(NameFilter))
                filteredItems = new(ServerDatabase.TraderSuits);
            else
                filteredItems = new(ServerDatabase.TraderSuits.Where(x => x.LocalizedName.ToUpper().Contains(NameFilter.ToUpper())));

            Suits = filteredItems;
        }
    }
}