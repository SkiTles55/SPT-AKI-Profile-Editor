using NUnit.Framework;
using SPT_AKI_Profile_Editor.Core;
using SPT_AKI_Profile_Editor.Views;

namespace SPT_AKI_Profile_Editor.Tests.ViewModelsTests
{
    internal class LocalizationEditorViewModelTests
    {
        [Test]
        public void LocalizationEditorViewModelForEnInitializeCorrectly()
        {
            LocalizationEditorViewModel lEditor = TestViewModel(new AppLocalization("en"));
            Assert.That(lEditor, Is.Not.Null, "LocalizationEditorViewModel is null");
            Assert.That(lEditor.Localization, Is.Not.Null, "Localization is null");
            Assert.That(lEditor.Translations, Is.Not.Null, "Translations is null");
            Assert.That(lEditor.CanSelectKey, Is.False, "CanSelectKey is true");
            Assert.That(lEditor.AvailableKeys, Is.Not.Null, "AvailableKeys is null");
            Assert.That(lEditor.AvailableKeys.Count, Is.GreaterThanOrEqualTo(1), "AvailableKeys.Count is empty");
        }

        [Test]
        public void LocalizationEditorViewModelForNewInitializeCorrectly()
        {
            LocalizationEditorViewModel lEditor = TestViewModel();
            Assert.That(lEditor, Is.Not.Null, "LocalizationEditorViewModel is null");
            Assert.That(lEditor.Localization, Is.Not.Null, "Localization is null");
            Assert.That(lEditor.Translations, Is.Not.Null, "Translations is null");
            Assert.That(lEditor.CanSelectKey, Is.True, "CanSelectKey is false");
            Assert.That(lEditor.AvailableKeys, Is.Not.Null, "AvailableKeys is null");
            Assert.That(lEditor.AvailableKeys.Count, Is.GreaterThanOrEqualTo(1), "AvailableKeys.Count is empty");
        }

        private static LocalizationEditorViewModel TestViewModel(AppLocalization appLocalization = null) => new(appLocalization);
    }
}