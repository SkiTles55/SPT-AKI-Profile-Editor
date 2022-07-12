using NUnit.Framework;

namespace SPT_AKI_Profile_Editor.Tests.ViewModelsTests
{
    internal class SettingsDialogViewModelTest
    {
        [Test]
        public void SettingsDialogViewModelInitializeCorrectly()
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

        private static SettingsDialogViewModel TestViewModel(int index = 0) => new(null, index);
    }
}