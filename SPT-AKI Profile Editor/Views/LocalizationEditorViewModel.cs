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
            IsEdit = isEdit;
            Translations = new(AppLocalization.Translations.Select(x => new Translation() { Key = x.Key, Value = x.Value }));
            AvailableKeys = AppData.GetAvailableKeys();
            SelectedLocalizationKey = AvailableKeys.FirstOrDefault().Key;
        }

        public static RelayCommand CancelCommand => new(obj => CloseDialog());

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
        public bool IsEdit { get; set; }
        public Dictionary<string, string> AvailableKeys { get; set; }
        public ObservableCollection<Translation> Translations { get; set; }
        public RelayCommand SaveCommand => new(obj => Save());

        private static async void CloseDialog()
        {
            BaseMetroDialog dialog = await App.DialogCoordinator.GetCurrentDialogAsync<BaseMetroDialog>(MainWindowViewModel.Instance);
            await App.DialogCoordinator.HideMetroDialogAsync(MainWindowViewModel.Instance, dialog);
        }

        private void Save()
        {
            if (IsEdit)
                AppLocalization.Update(Translations.ToDictionary(x => x.Key, x => x.Value));
            else
                AppLocalization.AddNew(SelectedLocalizationKey,
                                       SelectedLocalizationValue,
                                       Translations.ToDictionary(x => x.Key, x => x.Value));
            CloseDialog();
        }
    }

    public class Translation
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }
}