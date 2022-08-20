using SPT_AKI_Profile_Editor.Core;
using SPT_AKI_Profile_Editor.Core.HelperClasses;
using SPT_AKI_Profile_Editor.Helpers;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;

namespace SPT_AKI_Profile_Editor.Views
{
    public class LocalizationEditorViewModel : ClosableDialogViewModel
    {
        private readonly SettingsDialogViewModel _settingsDialog;
        private string selectedLocalizationKey;
        private string keyFilter;
        private string valueFilter;

        public LocalizationEditorViewModel(bool isEdit = true, SettingsDialogViewModel settingsDialog = null)
        {
            IsEdit = isEdit;
            Translations = new(AppLocalization.Translations.Select(x => new Translation() { Key = x.Key, Value = x.Value }));
            AvailableKeys = AppData.GetAvailableKeys();
            SelectedLocalizationKey = AvailableKeys.FirstOrDefault().Key;
            _settingsDialog = settingsDialog;
        }

        public string SelectedLocalizationKey
        {
            get => selectedLocalizationKey;
            set
            {
                selectedLocalizationKey = value;
                if (!string.IsNullOrEmpty(selectedLocalizationKey) && AvailableKeys.ContainsKey(selectedLocalizationKey))
                    SelectedLocalizationValue = AvailableKeys[selectedLocalizationKey];
                OnPropertyChanged("SelectedLocalizationKey");
            }
        }

        public string KeyFilter
        {
            get => keyFilter;
            set
            {
                keyFilter = value;
                OnPropertyChanged("KeyFilter");
                Filter();
            }
        }

        public string ValueFilter
        {
            get => valueFilter;
            set
            {
                valueFilter = value;
                OnPropertyChanged("ValueFilter");
                Filter();
            }
        }

        public string SelectedLocalizationValue { get; set; }
        public bool IsEdit { get; set; }
        public Dictionary<string, string> AvailableKeys { get; set; }
        public ObservableCollection<Translation> Translations { get; }
        public RelayCommand SaveCommand => new(obj => Save());

        private async void Save()
        {
            if (IsEdit)
                AppLocalization.Update(Translations.ToDictionary(x => x.Key, x => x.Value));
            else
                AppLocalization.AddNew(SelectedLocalizationKey,
                                       SelectedLocalizationValue,
                                       Translations.ToDictionary(x => x.Key, x => x.Value),
                                       _settingsDialog);
            await CloseDialog();
        }

        private void Filter()
        {
            ICollectionView cv = CollectionViewSource.GetDefaultView(Translations);
            if (cv != null)
            {
                if (string.IsNullOrEmpty(KeyFilter) && string.IsNullOrEmpty(ValueFilter))
                    cv.Filter = null;
                else
                {
                    cv.Filter = o =>
                    {
                        Translation p = o as Translation;
                        return ShouldFilterKey(p) && ShouldFilterValue(p);
                    };
                }
            }
        }

        private bool ShouldFilterValue(Translation p) => string.IsNullOrEmpty(ValueFilter) || p.Value.ToLower().Contains(ValueFilter.ToLower());

        private bool ShouldFilterKey(Translation p) => string.IsNullOrEmpty(KeyFilter) || p.Key.ToLower().Contains(KeyFilter.ToLower());
    }
}