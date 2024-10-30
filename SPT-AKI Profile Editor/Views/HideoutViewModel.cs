﻿using SPT_AKI_Profile_Editor.Core.ProfileClasses;
using SPT_AKI_Profile_Editor.Helpers;
using System.Collections.ObjectModel;
using System.Linq;

namespace SPT_AKI_Profile_Editor.Views
{
    public class HideoutTabViewModel : PmcBindableViewModel
    {
        private ObservableCollection<HideoutArea> areas = new();
        private ObservableCollection<CharacterHideoutProduction> productions = new();
        private ObservableCollection<StartedHideoutProduction> startedProductions = new();
        private string areaNameFilter;
        private string productionNameFilter;
        private string productionAreaFilter;
        private string startedProductionNameFilter;

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

        public override void ApplyFilter()
        {
            ApplyAreasFilter();
            ApplyProductionsFilter();
            ApplyStartedProductionsFilter();
        }

        private void ApplyProductionsFilter()
        {
            ObservableCollection<CharacterHideoutProduction> filteredItems;

            if (Profile?.Characters?.Pmc?.HideoutProductions == null || !Profile.Characters.Pmc.HideoutProductions.Any())
                filteredItems = new();
            else if (string.IsNullOrEmpty(ProductionNameFilter) && string.IsNullOrEmpty(ProductionAreaFilter))
                filteredItems = new(Profile.Characters.Pmc.HideoutProductions);
            else
                filteredItems = new(Profile.Characters.Pmc.HideoutProductions.Where(x => CanShow(x)));

            Productions = filteredItems;
        }

        private bool CanShow(CharacterHideoutProduction x)
            => (string.IsNullOrEmpty(ProductionNameFilter) || x.ProductItem.Name.ToUpper().Contains(ProductionNameFilter.ToUpper()))
            && (string.IsNullOrEmpty(ProductionAreaFilter) || x.AreaLocalizedName.ToUpper().Contains(ProductionAreaFilter.ToUpper()));

        private void ApplyAreasFilter()
        {
            ObservableCollection<HideoutArea> filteredAreas;

            if (Profile?.Characters?.Pmc?.Hideout?.Areas == null || !Profile.Characters.Pmc.Hideout.Areas.Any())
                filteredAreas = new();
            else if (string.IsNullOrEmpty(AreaNameFilter))
                filteredAreas = new(Profile.Characters.Pmc.Hideout.Areas);
            else
                filteredAreas = new(Profile.Characters.Pmc.Hideout.Areas.Where(x => x.LocalizedName.ToUpper().Contains(AreaNameFilter.ToUpper())));

            Areas = filteredAreas;
        }

        private void ApplyStartedProductionsFilter()
        {
            ObservableCollection<StartedHideoutProduction> filteredProductions;
            if (Profile?.Characters?.Pmc?.Hideout?.Production == null || !Profile.Characters.Pmc.Hideout.Production.Any())
                filteredProductions = new();
            else
            {
                var values = Profile.Characters.Pmc.Hideout.Production.Values;
                if (string.IsNullOrEmpty(StartedProductionNameFilter))
                    filteredProductions = new(values);
                else
                    filteredProductions = new(values.Where(x => x.ProductItem.Name.ToUpper().Contains(StartedProductionNameFilter.ToUpper())));
            }

            StartedProductions = filteredProductions;
        }
    }
}