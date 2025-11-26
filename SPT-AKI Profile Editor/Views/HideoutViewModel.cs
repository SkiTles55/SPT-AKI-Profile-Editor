using SPT_AKI_Profile_Editor.Core.ProfileClasses;
using SPT_AKI_Profile_Editor.Helpers;
using System.Collections.ObjectModel;
using System.Linq;

namespace SPT_AKI_Profile_Editor.Views
{
    public class HideoutTabViewModel(IDialogManager dialogManager) : PmcBindableViewModel
    {
        private ObservableCollection<HideoutArea> areas = [];
        private ObservableCollection<CharacterHideoutProduction> productions = [];
        private ObservableCollection<StartedHideoutProduction> startedProductions = [];
        private string areaNameFilter;
        private string productionNameFilter;
        private string productionAreaFilter;
        private string startedProductionNameFilter;
        private string startedProductionAreaFilter;

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

        public ObservableCollection<StartedHideoutProduction> StartedProductions
        {
            get => startedProductions;
            set
            {
                startedProductions = value;
                OnPropertyChanged(nameof(StartedProductions));
            }
        }

        public bool CanRemoveAnyStartedProduction => StartedProductions.Any();

        public bool CanFinishAnyStartedProduction => StartedProductions.Where(x => !x.IsFinished).Any();

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

        public string StartedProductionNameFilter
        {
            get => startedProductionNameFilter;
            set
            {
                startedProductionNameFilter = value;
                OnPropertyChanged(nameof(StartedProductionNameFilter));
                ApplyFilter();
            }
        }

        public string StartedProductionAreaFilter
        {
            get => startedProductionAreaFilter;
            set
            {
                startedProductionAreaFilter = value;
                OnPropertyChanged(nameof(StartedProductionAreaFilter));
                ApplyFilter();
            }
        }

        public RelayCommand SetCraftFinishedCommand => new(obj =>
        {
            if (obj is string id)
                SetCraftFinished(id);
        });

        public RelayCommand RemoveStartedCraftCommand => new(obj =>
        {
            if (obj is string id)
                RemoveStartedCraft(id);
        });

        public RelayCommand SetAllCraftsFinishedCommand => new(_ => SetAllCraftsFinished());

        public RelayCommand RemoveAllStartedCraftsCommand => new(_ => RemoveAllStartedCrafts());

        public override void ApplyFilter()
        {
            ApplyAreasFilter();
            ApplyProductionsFilter();
            ApplyStartedProductionsFilter();
        }

        private void ApplyProductionsFilter()
        {
            ObservableCollection<CharacterHideoutProduction> filteredItems;

            if (Profile?.Characters?.Pmc?.HideoutProductions == null || Profile.Characters.Pmc.HideoutProductions.Count == 0)
                filteredItems = [];
            else if (string.IsNullOrEmpty(ProductionNameFilter) && string.IsNullOrEmpty(ProductionAreaFilter))
                filteredItems = new(Profile.Characters.Pmc.HideoutProductions);
            else
                filteredItems = new(Profile.Characters.Pmc.HideoutProductions.Where(x => CanShow(ProductionNameFilter, x.ProductItem.Name, ProductionAreaFilter, x.AreaLocalizedName)));

            Productions = filteredItems;
        }

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

        private void ApplyStartedProductionsFilter()
        {
            ObservableCollection<StartedHideoutProduction> filteredProductions;
            if (Profile?.Characters?.Pmc?.Hideout?.Production == null || Profile.Characters.Pmc.Hideout.Production.Count == 0)
                filteredProductions = [];
            else
            {
                var values = Profile.Characters.Pmc.Hideout.Production.Values;
                if (string.IsNullOrEmpty(StartedProductionNameFilter) && string.IsNullOrEmpty(StartedProductionAreaFilter))
                    filteredProductions = new(values);
                else
                    filteredProductions = new(values.Where(x => CanShow(StartedProductionNameFilter, x.ProductItem.Name, StartedProductionAreaFilter, x.AreaLocalizedName)));
            }

            StartedProductions = filteredProductions;
            OnPropertyChanged(nameof(CanFinishAnyStartedProduction));
            OnPropertyChanged(nameof(CanRemoveAnyStartedProduction));
        }

        private static bool CanShow(string productNameFilter, string productName, string areaNameFilter, string areaName)
            => (string.IsNullOrEmpty(productNameFilter) || productName.Contains(productNameFilter, System.StringComparison.CurrentCultureIgnoreCase))
            && (string.IsNullOrEmpty(areaNameFilter) || areaName.Contains(areaNameFilter, System.StringComparison.CurrentCultureIgnoreCase));

        private void SetCraftFinished(string id)
        {
            var craft = Profile?.Characters?.Pmc?.Hideout?.Production?.Values.FirstOrDefault(x => x.RecipeId == id);
            craft?.SetFinished();
            ApplyStartedProductionsFilter();
        }

        private async void RemoveStartedCraft(string id)
        {
            if (await dialogManager.YesNoDialog("remove_started_craft_title", "remove_started_craft_caption"))
            {
                Profile?.Characters?.Pmc?.Hideout?.RemoveCraft(id);
                ApplyStartedProductionsFilter();
            }
        }

        private void SetAllCraftsFinished()
        {
            Profile?.Characters?.Pmc?.Hideout?.SetAllCraftsFinished();
            ApplyStartedProductionsFilter();
        }

        private async void RemoveAllStartedCrafts()
        {
            if (await dialogManager.YesNoDialog("remove_started_crafts_title", "remove_started_crafts_caption"))
            {
                Profile?.Characters?.Pmc?.Hideout?.RemoveAllCrafts();
                ApplyStartedProductionsFilter();
            }
        }
    }
}