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
        public AppLocalization Localization { get; set; }
        public bool CanSelectKey { get; set; }
        public Dictionary<string, string> AvailableKeys { get; set; }

        public ObservableCollection<Translation> Translations { get; set; }

        public LocalizationEditorViewModel(AppLocalization appLocalization = null)
        {
            Localization = appLocalization ?? AppLocalization;
            CanSelectKey = appLocalization == null;
            Translations = new(Localization.Translations.Select(x => new Translation() { Key = x.Key, Value = x.Value }));
        }

        public RelayCommand SaveCommand => new(obj => Save());

        public static RelayCommand CancelCommand => new(obj => Cancel());

        private void Save()
        {

        }

        private static async void Cancel()
        {
            BaseMetroDialog dialog = await App.DialogCoordinator.GetCurrentDialogAsync<BaseMetroDialog>(MainWindowViewModel.Instance);
            await App.DialogCoordinator.HideMetroDialogAsync(MainWindowViewModel.Instance, dialog);
        }
    }

    public class Translation
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }
}