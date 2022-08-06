using NUnit.Framework;
using SPT_AKI_Profile_Editor.Tests.Hepers;

namespace SPT_AKI_Profile_Editor.Tests.ViewModelsTests
{
    internal class SettingsDialogViewModelTests
    {
        private static readonly TestsDialogManager dialogManager = new();

        [Test]
        public void InitializeCorrectly()
        {
            SettingsDialogViewModel settingsVM = TestViewModel(1);
            Assert.Multiple(() =>
            {
                Assert.That(settingsVM, Is.Not.Null, "SettingsDialogViewModel is null");
                Assert.That(settingsVM.SelectedTab, Is.EqualTo(1), "SelectedTab is not 1");
                Assert.That(string.IsNullOrEmpty(settingsVM.CurrentLocalization), Is.False, "CurrentLocalization is null or empty");
                Assert.That(string.IsNullOrEmpty(settingsVM.ServerPath), Is.False, "ServerPath is null or empty");
                Assert.That(string.IsNullOrEmpty(settingsVM.ColorScheme), Is.False, "ColorScheme is null or empty");
                Assert.That(settingsVM.ServerPathValid, Is.True, "ServerPathValid is false");
                Assert.That(settingsVM.ServerHasAccounts, Is.True, "ServerHasAccounts is false");
            });
        }

        [Test]
        public void CanOpenLocalizationEditorForEdit()
        {
            SettingsDialogViewModel settingsVM = TestViewModel();
            settingsVM.OpenLocalizationEditor.Execute(true);
            Assert.That(dialogManager.LocalizationEditorDialogOpened, Is.True, "LocalizationEditorDialog not opened");
        }

        private static SettingsDialogViewModel TestViewModel(int index = 0) => new(null, dialogManager, index);
    }
}