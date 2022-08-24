using NUnit.Framework;
using SPT_AKI_Profile_Editor.Core;
using SPT_AKI_Profile_Editor.Core.HelperClasses;
using SPT_AKI_Profile_Editor.Views;
using System.Linq;
using System.Windows.Data;

namespace SPT_AKI_Profile_Editor.Tests.ViewModelsTests
{
    internal class LocalizationEditorViewModelTests
    {
        [Test]
        public void InitializeCorrectlyForEdit()
        {
            LocalizationEditorViewModel lEditor = TestViewModel();
            Assert.That(lEditor, Is.Not.Null, "LocalizationEditorViewModel is null");
            Assert.That(string.IsNullOrEmpty(lEditor.SelectedLocalizationKey), Is.False, "SelectedLocalizationKey is null or empty");
            Assert.That(string.IsNullOrEmpty(lEditor.SelectedLocalizationValue), Is.False, "SelectedLocalizationValue is null or empty");
            Assert.That(lEditor.Translations, Is.Not.Null, "Translations is null");
            Assert.That(lEditor.IsEdit, Is.True, "IsEdit is false");
            Assert.That(lEditor.AvailableKeys, Is.Not.Null, "AvailableKeys is null");
            Assert.That(lEditor.AvailableKeys.Count, Is.GreaterThanOrEqualTo(1), "AvailableKeys.Count is empty");
            Assert.That(lEditor.AvailableKeys.Any(x => AppData.AppLocalization.Translations.ContainsKey(x.Key)), Is.False, "AvailableKeys contains exist localization keys");
            Assert.That(string.IsNullOrEmpty(lEditor.KeyFilter), Is.True, "KeyFilter not null or empty");
            Assert.That(string.IsNullOrEmpty(lEditor.ValueFilter), Is.True, "ValueFilter null or not empty");
        }

        [Test]
        public void InitializeCorrectlyForNew()
        {
            LocalizationEditorViewModel lEditor = TestViewModel(false);
            Assert.That(lEditor, Is.Not.Null, "LocalizationEditorViewModel is null");
            Assert.That(string.IsNullOrEmpty(lEditor.SelectedLocalizationKey), Is.False, "SelectedLocalizationKey is null or empty");
            Assert.That(string.IsNullOrEmpty(lEditor.SelectedLocalizationValue), Is.False, "SelectedLocalizationValue is null or empty");
            Assert.That(lEditor.Translations, Is.Not.Null, "Translations is null");
            Assert.That(lEditor.IsEdit, Is.False, "IsEdit is true");
            Assert.That(lEditor.AvailableKeys, Is.Not.Null, "AvailableKeys is null");
            Assert.That(lEditor.AvailableKeys.Count, Is.GreaterThanOrEqualTo(1), "AvailableKeys.Count is empty");
            Assert.That(string.IsNullOrEmpty(lEditor.KeyFilter), Is.True, "KeyFilter not null or empty");
            Assert.That(string.IsNullOrEmpty(lEditor.ValueFilter), Is.True, "ValueFilter null or not empty");
        }

        [Test]
        public void CanSaveEditedLocalization()
        {
            LocalizationEditorViewModel lEditor = TestViewModel();
            lEditor.Translations.Where(x => x.Key == "button_yes").FirstOrDefault().Value = "Yes, baby";
            lEditor.SaveCommand.Execute(null);
            Assert.That(AppData.AppLocalization.Translations["button_yes"], Is.EqualTo("Yes, baby"), "button_yes translation not changed");
            AppData.AppLocalization.LoadLocalization(AppData.AppSettings.Language);
            Assert.That(AppData.AppLocalization.Translations["button_yes"], Is.EqualTo("Yes, baby"), "button_yes translation after reload not changed");
        }

        [Test]
        public void CanSaveNewLocalization()
        {
            SettingsDialogViewModel settingsDialog = new(null, null, null, null);
            LocalizationEditorViewModel lEditor = TestViewModel(false, settingsDialog);
            var newKey = lEditor.SelectedLocalizationKey;
            Assert.That(newKey, Is.Not.EqualTo(AppData.AppSettings.Language), "Wrong new localization key");
            lEditor.Translations.Where(x => x.Key == "button_yes").FirstOrDefault().Value = "No, baby";
            lEditor.SaveCommand.Execute(null);
            Assert.That(AppData.AppLocalization.Translations["button_yes"], Is.EqualTo("No, baby"), "button_yes translation not changed");
            AppData.AppLocalization.LoadLocalization(AppData.AppSettings.Language);
            Assert.That(AppData.AppLocalization.Translations["button_yes"], Is.EqualTo("No, baby"), "button_yes translation after reload not changed");
            Assert.That(AppData.AppSettings.Language, Is.EqualTo(newKey), "New localization not selected");
            Assert.That(AppData.AppLocalization.Localizations.ContainsKey(newKey), Is.True, "Localizations does not contains new localization");
        }

        [Test]
        public void CanFilterByKey()
        {
            var filterText = "button";
            LocalizationEditorViewModel lEditor = TestViewModel();
            lEditor.KeyFilter = filterText;
            var filtered = CollectionViewSource.GetDefaultView(lEditor.Translations).Cast<Translation>();
            Assert.That(filtered.Any(x => !x.Key.Contains(filterText)), Is.False, "Translations is not filtered");
            lEditor.KeyFilter = "";
            Assert.That(filtered.Any(x => !x.Key.Contains(filterText)), Is.True, "Translations is still filtered after remove filter text");
        }

        [Test]
        public void CanFilterByValue()
        {
            var filterText = "SPT";
            LocalizationEditorViewModel lEditor = TestViewModel();
            lEditor.ValueFilter = filterText;
            var filtered = CollectionViewSource.GetDefaultView(lEditor.Translations).Cast<Translation>();
            Assert.That(filtered.Any(x => !x.Value.Contains(filterText)), Is.False, "Translations is not filtered");
            lEditor.ValueFilter = "";
            Assert.That(filtered.Any(x => !x.Value.Contains(filterText)), Is.True, "Translations is still filtered after remove filter text");
        }

        private static LocalizationEditorViewModel TestViewModel(bool isEdit = true, SettingsDialogViewModel settingsDialog = null) => new(isEdit, settingsDialog, null);
    }
}