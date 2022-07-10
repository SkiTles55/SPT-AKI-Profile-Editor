using MahApps.Metro.Controls.Dialogs;
using SPT_AKI_Profile_Editor.Core;
using SPT_AKI_Profile_Editor.Helpers;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace SPT_AKI_Profile_Editor.Views
{
    public class LocalizationEditorViewModel : BindableViewModel
    {
        private string selectedLocalizationKey;

        public LocalizationEditorViewModel(bool isEdit = true)
        {
            CanSelectKey = !isEdit;
            Translations = new(AppLocalization.Translations.Select(x => new Translation() { Key = x.Key, Value = x.Value }));
            AvailableKeys = AppData.GetAvailableKeys();
            SelectedLocalizationKey = AppLocalization.Key;
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
        public string SelectedLocalizationValue { get; set; }
        public static RelayCommand CancelCommand => new(obj => Cancel());
        public bool CanSelectKey { get; set; }
        public Dictionary<string, string> AvailableKeys { get; set; }
        public ObservableCollection<Translation> Translations { get; set; }
        public RelayCommand SaveCommand => new(obj => Save());

        private static async void Cancel()
        {
            BaseMetroDialog dialog = await App.DialogCoordinator.GetCurrentDialogAsync<BaseMetroDialog>(MainWindowViewModel.Instance);
            await App.DialogCoordinator.HideMetroDialogAsync(MainWindowViewModel.Instance, dialog);
        }

        private void Save()
        {
        }
    }

    public class Translation
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }
}