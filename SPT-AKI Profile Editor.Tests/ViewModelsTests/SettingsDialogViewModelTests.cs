using NUnit.Framework;
using SPT_AKI_Profile_Editor.Core;
using SPT_AKI_Profile_Editor.Core.HelperClasses;
using SPT_AKI_Profile_Editor.Helpers;
using SPT_AKI_Profile_Editor.Tests.Hepers;
using SPT_AKI_Profile_Editor.Views;
using System.Linq;

namespace SPT_AKI_Profile_Editor.Tests.ViewModelsTests
{
    internal class SettingsDialogViewModelTests
    {
        private static readonly TestsDialogManager dialogManager = new();

        [Test]
        public void InitializeCorrectly()
        {
            TestsApplicationManager applicationManager = new();
            SettingsDialogViewModel settingsVM = new(null, null, null, applicationManager, 1);
            Assert.Multiple(() =>
            {
                Assert.That(settingsVM, Is.Not.Null, "SettingsDialogViewModel is null");
                Assert.That(settingsVM.ColorSchemes, Is.Not.Empty, "SettingsDialogViewModel ColorSchemes is empty");
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
            dialogManager.LocalizationEditorDialogOpened = false;
            SettingsDialogViewModel settingsVM = new(null, dialogManager, null, null);
            settingsVM.OpenLocalizationEditor.Execute(true);
            Assert.That(dialogManager.LocalizationEditorDialogOpened, Is.True, "LocalizationEditorDialog not opened");
        }

        [Test]
        public void CanExecuteCloseCommand()
        {
            bool closeCommandCalled = false;
            RelayCommand closeCommand = new(obj => closeCommandCalled = true);
            SettingsDialogViewModel settingsVM = new(closeCommand, null, null, null);
            settingsVM.CloseCommand.Execute(null);
            Assert.That(closeCommandCalled, Is.True);
        }

        [Test]
        public void CanOpenAppData()
        {
            TestsApplicationManager applicationManager = new();
            SettingsDialogViewModel settingsVM = new(null, null, null, applicationManager);
            settingsVM.OpenAppData.Execute(null);
            Assert.That(applicationManager.LastOpenedUrl, Is.EqualTo(DefaultValues.AppDataFolder));
        }

        [Test]
        public void CanExecuteQuitCommand()
        {
            TestsApplicationManager applicationManager = new();
            SettingsDialogViewModel settingsVM = new(null, null, null, applicationManager);
            settingsVM.QuitCommand.Execute(null);
            Assert.That(applicationManager.CloseApplicationExecuted, Is.True);
        }

        [Test]
        public void CanChangeColorScheme()
        {
            TestsApplicationManager applicationManager = new();
            SettingsDialogViewModel settingsVM = new(null, null, null, applicationManager);
            applicationManager.ThemeChanged = false;
            var colorScheme = settingsVM.ColorSchemes.Where(x => x.Scheme != settingsVM.ColorScheme).FirstOrDefault();
            settingsVM.ColorScheme = colorScheme.Scheme;
            Assert.That(applicationManager.ThemeChanged, Is.True);
        }

        [Test]
        public void CanServerSelect()
        {
            TestsWindowsDialogs windowsDialogs = new()
            {
                folderBrowserDialogMode = FolderBrowserDialogMode.serverFolder
            };
            SettingsDialogViewModel settingsVM = new(null, dialogManager, windowsDialogs, null);
            AppData.AppSettings.ServerPath = null;
            settingsVM.ServerSelect.Execute(null);
            Assert.That(settingsVM.ServerPath, Is.EqualTo(TestHelpers.serverPath));
            Assert.That(AppData.AppSettings.ServerPath, Is.EqualTo(TestHelpers.serverPath));
        }

        [Test]
        public void CanServerSelectWithWrongPath()
        {
            dialogManager.ServerPathEditorDialogOpened = false;
            TestsWindowsDialogs windowsDialogs = new()
            {
                folderBrowserDialogMode = FolderBrowserDialogMode.wrongServerFolder
            };
            SettingsDialogViewModel settingsVM = new(null, dialogManager, windowsDialogs, null);
            settingsVM.ServerSelect.Execute(null);
            Assert.That(dialogManager.ServerPathEditorDialogOpened, Is.True);
            Assert.That(settingsVM.ServerPath, Is.Not.EqualTo(TestHelpers.wrongServerPath));
            Assert.That(AppData.AppSettings.ServerPath, Is.Not.EqualTo(TestHelpers.wrongServerPath));
        }

        [Test]
        public void CanServerSelectWithWrongPathAndPathsUpdate()
        {
            dialogManager.ServerPathEditorDialogOpened = false;
            dialogManager.ShouldExecuteServerPathEditorRetryCommand = true;
            TestsWindowsDialogs windowsDialogs = new()
            {
                folderBrowserDialogMode = FolderBrowserDialogMode.wrongServerFolder
            };
            SettingsDialogViewModel settingsVM = new(null, dialogManager, windowsDialogs, null);
            settingsVM.ServerSelect.Execute(null);
            Assert.That(dialogManager.ServerPathEditorDialogOpened, Is.True);
            Assert.That(settingsVM.AppSettings.FilesList[SPTServerFile.serverexe], Is.EqualTo("Test.exe"));
            settingsVM.AppSettings.FilesList[SPTServerFile.serverexe] = "Aki.Server.exe";
            settingsVM.AppSettings.Save();
        }

        [Test]
        public void CanResetLocalizations()
        {
            TestsApplicationManager applicationManager = new();
            SettingsDialogViewModel settingsVM = new(null, null, null, applicationManager);
            settingsVM.ResetLocalizations.Execute(null);
            Assert.That(applicationManager.LocalizationsDeleted, Is.True);
        }

        [Test]
        public void CanResetLocalizationsAndReload()
        {
            TestsApplicationManager applicationManager = new();
            SettingsDialogViewModel settingsVM = new(null, null, null, applicationManager);
            settingsVM.ResetAndReload.Execute(settingsVM.ResetLocalizations);
            Assert.That(applicationManager.LocalizationsDeleted, Is.True);
            Assert.That(applicationManager.ApplicationRestarted, Is.True);
        }

        [Test]
        public void CanResetSettings()
        {
            TestsApplicationManager applicationManager = new();
            SettingsDialogViewModel settingsVM = new(null, null, null, applicationManager);
            settingsVM.ResetSettings.Execute(null);
            Assert.That(applicationManager.SettingsDeleted, Is.True);
        }

        [Test]
        public void CanResetSettingsAndReload()
        {
            TestsApplicationManager applicationManager = new();
            SettingsDialogViewModel settingsVM = new(null, null, null, applicationManager);
            settingsVM.ResetAndReload.Execute(settingsVM.ResetSettings);
            Assert.That(applicationManager.SettingsDeleted, Is.True);
            Assert.That(applicationManager.ApplicationRestarted, Is.True);
        }

        [Test]
        public void CanCatchExceptionInResetAndReload()
        {
            dialogManager.LastOkMessage = null;
            RelayCommand badCommand = new(obj => throw new System.Exception("Bad command exception"));
            SettingsDialogViewModel settingsVM = new(null, dialogManager, null, null);
            settingsVM.ResetAndReload.Execute(badCommand);
            Assert.That(dialogManager.LastOkMessage, Is.EqualTo("Bad command exception"));
        }
    }
}