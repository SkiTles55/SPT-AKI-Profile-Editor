using SPT_AKI_Profile_Editor.Core.ProfileClasses;
using SPT_AKI_Profile_Editor.Helpers;
using System.Collections.ObjectModel;
using System.Linq;

namespace SPT_AKI_Profile_Editor.Views
{
    public class HideoutTabViewModel : PmcBindableViewModel
    {
        private ObservableCollection<HideoutArea> areas = [];
        private ObservableCollection<CharacterHideoutProduction> productions = [];
        private string areaNameFilter;
        private string productionNameFilter;
        private string productionAreaFilter;

        public static RelayCommand SetAllMaxCommand => new(obj => Profile.Characters?.Pmc?.SetAllHideoutAreasMax());

        public static RelayCommand AddAllCrafts => new(obj => Profile.Characters?.Pmc?.AddAllCrafts());

        public ObservableCollection<HideoutArea> Areas
        {
            get => areas;
            set
            {
                areas = value;
                OnPropertyChanged(nameof(Areas));
            }
        }

        public ObservableCollection<CharacterHideoutProduction> Productions
        {
            get => productions;
            set
            {
                productions = value;
                OnPropertyChanged(nameof(Productions));
            }
        }

        public string AreaNameFilter
        {
            get => areaNameFilter;
            set
            {
                areaNameFilter = value;
                OnPropertyChanged(nameof(AreaNameFilter));
                ApplyFilter();
            }
        }

        public string ProductionNameFilter
        {
            get => productionNameFilter;
            set
            {
                productionNameFilter = value;
                OnPropertyChanged(nameof(ProductionNameFilter));
                ApplyFilter();
            }
        }

        public string ProductionAreaFilter
        {
            get => productionAreaFilter;
            set
            {
                productionAreaFilter = value;
                OnPropertyChanged(nameof(ProductionAreaFilter));
                ApplyFilter();
            }
        }

        public override void ApplyFilter()
        {
            ApplyAreasFilter();
            ApplyProductionsFilter();
        }

        private void ApplyProductionsFilter()
        {
            ObservableCollection<CharacterHideoutProduction> filteredItems;

            if (Profile?.Characters?.Pmc?.HideoutProductions == null || Profile.Characters.Pmc.HideoutProductions.Count == 0)
                filteredItems = [];
            else if (string.IsNullOrEmpty(ProductionNameFilter) && string.IsNullOrEmpty(ProductionAreaFilter))
                filteredItems = new(Profile.Characters.Pmc.HideoutProductions);
            else
                filteredItems = new(Profile.Characters.Pmc.HideoutProductions.Where(x => CanShow(x)));

            Productions = filteredItems;
        }

        private bool CanShow(CharacterHideoutProduction x)
            => (string.IsNullOrEmpty(ProductionNameFilter) || x.ProductItem.Name.Contains(ProductionNameFilter, System.StringComparison.CurrentCultureIgnoreCase))
            && (string.IsNullOrEmpty(ProductionAreaFilter) || x.AreaLocalizedName.Contains(ProductionAreaFilter, System.StringComparison.CurrentCultureIgnoreCase));

        private void ApplyAreasFilter()
        {
            ObservableCollection<HideoutArea> filteredAreas;

            if (Profile?.Characters?.Pmc?.Hideout?.Areas == null || Profile.Characters.Pmc.Hideout.Areas.Length == 0)
                filteredAreas = [];
            else if (string.IsNullOrEmpty(AreaNameFilter))
                filteredAreas = new(Profile.Characters.Pmc.Hideout.Areas);
            else
                filteredAreas = new(Profile.Characters.Pmc.Hideout.Areas.Where(x => x.LocalizedName.Contains(AreaNameFilter, System.StringComparison.CurrentCultureIgnoreCase)));

            Areas = filteredAreas;
        }
    }
}