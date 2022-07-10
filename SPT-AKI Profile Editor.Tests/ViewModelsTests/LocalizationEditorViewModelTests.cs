using NUnit.Framework;
using SPT_AKI_Profile_Editor.Core;
using SPT_AKI_Profile_Editor.Views;

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
            Assert.That(lEditor.CanSelectKey, Is.False, "CanSelectKey is true");
            Assert.That(lEditor.AvailableKeys, Is.Not.Null, "AvailableKeys is null");
            Assert.That(lEditor.AvailableKeys.Count, Is.GreaterThanOrEqualTo(1), "AvailableKeys.Count is empty");
        }

        [Test]
        public void LocalizationEditorViewModelForNewInitializeCorrectly()
        {
            LocalizationEditorViewModel lEditor = TestViewModel(false);
            Assert.That(lEditor, Is.Not.Null, "LocalizationEditorViewModel is null");
            Assert.That(string.IsNullOrEmpty(lEditor.SelectedLocalizationKey), Is.False, "SelectedLocalizationKey is null or empty");
            Assert.That(string.IsNullOrEmpty(lEditor.SelectedLocalizationValue), Is.False, "SelectedLocalizationValue is null or empty");
            Assert.That(lEditor.Translations, Is.Not.Null, "Translations is null");
            Assert.That(lEditor.CanSelectKey, Is.True, "CanSelectKey is false");
            Assert.That(lEditor.AvailableKeys, Is.Not.Null, "AvailableKeys is null");
            Assert.That(lEditor.AvailableKeys.Count, Is.GreaterThanOrEqualTo(1), "AvailableKeys.Count is empty");
        }

        private static LocalizationEditorViewModel TestViewModel(bool isEdit = true) => new(isEdit);
    }
}