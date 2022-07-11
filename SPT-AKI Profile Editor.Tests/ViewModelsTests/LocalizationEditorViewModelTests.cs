using NUnit.Framework;
using SPT_AKI_Profile_Editor.Core;
using SPT_AKI_Profile_Editor.Views;
using System.Linq;

namespace SPT_AKI_Profile_Editor.Tests.ViewModelsTests
{
    internal class LocalizationEditorViewModelTests
    {
        [Test]
        public void LocalizationEditorViewModelForEditInitializeCorrectly()
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
        }

        [Test]
        public void LocalizationEditorViewModelForNewInitializeCorrectly()
        {
            LocalizationEditorViewModel lEditor = TestViewModel(false);
            Assert.That(lEditor, Is.Not.Null, "LocalizationEditorViewModel is null");
            Assert.That(string.IsNullOrEmpty(lEditor.SelectedLocalizationKey), Is.False, "SelectedLocalizationKey is null or empty");
            Assert.That(string.IsNullOrEmpty(lEditor.SelectedLocalizationValue), Is.False, "SelectedLocalizationValue is null or empty");
            Assert.That(lEditor.Translations, Is.Not.Null, "Translations is null");
            Assert.That(lEditor.IsEdit, Is.False, "IsEdit is true");
            Assert.That(lEditor.AvailableKeys, Is.Not.Null, "AvailableKeys is null");
            Assert.That(lEditor.AvailableKeys.Count, Is.GreaterThanOrEqualTo(1), "AvailableKeys.Count is empty");
        }

        [Test]
        public void LocalizationEditorCanSaveEditLocalization()
        {
            LocalizationEditorViewModel lEditor = TestViewModel();
            lEditor.Translations.Where(x => x.Key == "button_yes").FirstOrDefault().Value = "Yes, baby";
            lEditor.SaveCommand.Execute(null);
            Assert.That(AppData.AppLocalization.Translations["button_yes"], Is.EqualTo("Yes, baby"), "button_yes translation not changed");
            AppData.AppLocalization.LoadLocalization(AppData.AppSettings.Language);
            Assert.That(AppData.AppLocalization.Translations["button_yes"], Is.EqualTo("Yes, baby"), "button_yes translation after reload not changed");
        }

        private static LocalizationEditorViewModel TestViewModel(bool isEdit = true) => new(isEdit);
    }
}