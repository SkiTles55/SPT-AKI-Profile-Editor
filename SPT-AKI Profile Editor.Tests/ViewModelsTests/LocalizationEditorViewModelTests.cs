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
            Assert.That(lEditor, Is.Not.Null);
            Assert.That(lEditor.Localization, Is.Not.Null);
            Assert.That(lEditor.CanSelectKey, Is.False);
        }

        [Test]
        public void LocalizationEditorViewModelForNewInitializeCorrectly()
        {
            LocalizationEditorViewModel lEditor = TestViewModel();
            Assert.That(lEditor, Is.Not.Null);
            Assert.That(lEditor.Localization, Is.Not.Null);
            Assert.That(lEditor.CanSelectKey, Is.True);
            Assert.That(lEditor.AvailableKeys, Is.Not.Null);
            Assert.That(lEditor.AvailableKeys.Count, Is.GreaterThanOrEqualTo(1));
        }

        private static LocalizationEditorViewModel TestViewModel(AppLocalization appLocalization = null) => new(appLocalization);
    }
}