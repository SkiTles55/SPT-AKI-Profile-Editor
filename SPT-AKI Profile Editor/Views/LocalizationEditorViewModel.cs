using MahApps.Metro.Controls.Dialogs;
using SPT_AKI_Profile_Editor.Core;
using SPT_AKI_Profile_Editor.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;

namespace SPT_AKI_Profile_Editor.Views
{
    public class LocalizationEditorViewModel : BindableViewModel
    {
        public LocalizationEditorViewModel(AppLocalization appLocalization = null)
        {
            Localization = appLocalization ?? AppLocalization;
            CanSelectKey = appLocalization == null;
            Translations = new(Localization.Translations.Select(x => new Translation() { Key = x.Key, Value = x.Value }));
            LoadAvailableKeys();
        }

        public static RelayCommand CancelCommand => new(obj => Cancel());
        public AppLocalization Localization { get; set; }
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

        private void LoadAvailableKeys()
        {
            Dictionary<string, string> availableKeys = new();
            try
            {
                string path = Path.Combine(AppData.AppSettings.ServerPath, AppData.AppSettings.FilesList["file_languages"]);
            }
            catch (Exception ex) { Logger.Log($"LoadAvailableKeys loading error: {ex.Message}"); }
            AvailableKeys = availableKeys;
        }
    }

    public class Translation
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }
}