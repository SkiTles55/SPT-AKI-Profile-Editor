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
            SettingsDialogViewModel settingsVM = new(null, null, null, applicationManager, null, null, null, 1);
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
            SettingsDialogViewModel settingsVM = new(null, dialogManager, null, null, null, null, null);
            settingsVM.OpenLocalizationEditor.Execute(true);
            Assert.That(dialogManager.LocalizationEditorDialogOpened, Is.True, "LocalizationEditorDialog not opened");
        }

        [Test]
        public void CanExecuteCloseCommand()
        {
            bool closeCommandCalled = false;
            RelayCommand closeCommand = new(obj => closeCommandCalled = true);
            SettingsDialogViewModel settingsVM = new(closeCommand, null, null, null, null, null, null);
            settingsVM.CloseCommand.Execute(null);
            Assert.That(closeCommandCalled, Is.True);
        }

        [Test]
        public void CanOpenAppData()
        {
            TestsApplicationManager applicationManager = new();
            SettingsDialogViewModel settingsVM = new(null, null, null, applicationManager, null, null, null);
            settingsVM.OpenAppData.Execute(null);
            Assert.That(applicationManager.LastOpenedUrl, Is.EqualTo(DefaultValues.AppDataFolder));
        }

        [Test]
        public void CanExecuteQuitCommand()
        {
            TestsApplicationManager applicationManager = new();
            SettingsDialogViewModel settingsVM = new(null, null, null, applicationManager, null, null, null);
            settingsVM.QuitCommand.Execute(null);
            Assert.That(applicationManager.CloseApplicationExecuted, Is.True);
        }

        [Test]
        public void CanChangeColorScheme()
        {
            TestsApplicationManager applicationManager = new();
            SettingsDialogViewModel settingsVM = new(null, null, null, applicationManager, null, null, null);
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
            SettingsDialogViewModel settingsVM = new(null, dialogManager, windowsDialogs, null, null, null, null);
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
            SettingsDialogViewModel settingsVM = new(null, dialogManager, windowsDialogs, null, null, null, null);
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
            SettingsDialogViewModel settingsVM = new(null, dialogManager, windowsDialogs, null, null, null, null);
            settingsVM.ServerSelect.Execute(null);
            Assert.That(dialogManager.ServerPathEditorDialogOpened, Is.True);
            Assert.That(settingsVM.AppSettings.FilesList[SPTServerFile.serverexe], Is.EqualTo("Test.exe"));
            settingsVM.AppSettings.FilesList[SPTServerFile.serverexe] = "SPT\\SPT.Server.exe";
            settingsVM.AppSettings.Save();
        }

        [Test]
        public void CanResetLocalizations()
        {
            TestsApplicationManager applicationManager = new();
            SettingsDialogViewModel settingsVM = new(null, null, null, applicationManager, null, null, null);
            settingsVM.ResetLocalizations.Execute(null);
            Assert.That(applicationManager.LocalizationsDeleted, Is.True);
        }

        [Test]
        public void CanResetLocalizationsAndReload()
        {
            TestsApplicationManager applicationManager = new();
            SettingsDialogViewModel settingsVM = new(null, null, null, applicationManager, null, null, null);
            settingsVM.ResetAndReload.Execute(settingsVM.ResetLocalizations);
            Assert.That(applicationManager.LocalizationsDeleted, Is.True);
            Assert.That(applicationManager.ApplicationRestarted, Is.True);
        }

        [Test]
        public void CanResetSettings()
        {
            TestsApplicationManager applicationManager = new();
            SettingsDialogViewModel settingsVM = new(null, null, null, applicationManager, null, null, null);
            settingsVM.ResetSettings.Execute(null);
            Assert.That(applicationManager.SettingsDeleted, Is.True);
        }

        [Test]
        public void CanResetSettingsAndReload()
        {
            TestsApplicationManager applicationManager = new();
            SettingsDialogViewModel settingsVM = new(null, null, null, applicationManager, null, null, null);
            settingsVM.ResetAndReload.Execute(settingsVM.ResetSettings);
            Assert.That(applicationManager.SettingsDeleted, Is.True);
            Assert.That(applicationManager.ApplicationRestarted, Is.True);
        }

        [Test]
        public void CanCatchExceptionInResetAndReload()
        {
            dialogManager.LastOkMessage = null;
            RelayCommand badCommand = new(obj => throw new System.Exception("Bad command exception"));
            SettingsDialogViewModel settingsVM = new(null, dialogManager, null, null, null, null, null);
            settingsVM.ResetAndReload.Execute(badCommand);
            Assert.That(dialogManager.LastOkMessage, Is.EqualTo("Bad command exception"));
        }

        [Test]
        public void CanInstallMod()
        {
            SettingsDialogViewModel settingsVM = PrepareVmWith(new TestsHelperModManager());
            settingsVM.InstallMod.Execute(null);
            CheckHelperModIn(settingsVM, true, HelperModStatus.Installed);
        }

        [Test]
        public void CanReinstallMod()
        {
            TestsHelperModManager helperModManager = new(HelperModStatus.Installed);
            SettingsDialogViewModel settingsVM = PrepareVmWith(helperModManager);
            settingsVM.ReinstallMod.Execute(null);
            CheckHelperModIn(settingsVM, true, HelperModStatus.Installed);
            Assert.That(helperModManager.RemoveModCalled, Is.True, "RemoveMod not called");
        }

        [Test]
        public void CanUpdateMod()
        {
            TestsHelperModManager helperModManager = new(HelperModStatus.UpdateAvailable);
            SettingsDialogViewModel settingsVM = PrepareVmWith(helperModManager);
            settingsVM.UpdateMod.Execute(null);
            Assert.That(settingsVM.HelperModManager.HelperModStatus,
                        Is.EqualTo(HelperModStatus.Installed),
                        $"HelperMod not updated");
            Assert.That(helperModManager.UpdateModCalled, Is.True, "UpdateMod not called");
        }

        [Test]
        public void CanRemoveMod()
        {
            TestsHelperModManager helperModManager = new(HelperModStatus.Installed);
            SettingsDialogViewModel settingsVM = PrepareVmWith(helperModManager);
            settingsVM.RemoveMod.Execute(null);
            CheckHelperModIn(settingsVM, false, HelperModStatus.NotInstalled);
            Assert.That(helperModManager.RemoveModCalled, Is.True, "RemoveMod not called");
        }

        [Test]
        public void CanOpenHowToUseHelperMod()
        {
            TestsApplicationManager applicationManager = new();
            SettingsDialogViewModel settingsVM = new(null, null, null, applicationManager, null, null, null);
            settingsVM.OpenHowToUseHelperMod.Execute(null);
            Assert.That(string.IsNullOrEmpty(applicationManager.LastOpenedUrl),
                        Is.False,
                        "HowToUseHelperMod not opened");
        }

        [Test]
        public void SuccessColorNotNull() => Assert.That(SettingsDialogViewModel.SuccessColor, Is.Not.Null, "SuccessColor is null");

        private static SettingsDialogViewModel PrepareVmWith(TestsHelperModManager helperModManager)
                                            => new(null,
                                                   dialogManager,
                                                   null,
                                                   null,
                                                   helperModManager,
                                                   null,
                                                   new TestsWorker());

        private static void CheckHelperModIn(SettingsDialogViewModel settingsVM,
                                             bool expectedUsingModHelper,
                                             HelperModStatus expectedStatus)
        {
            Assert.That(settingsVM.UsingModHelper,
                        Is.EqualTo(expectedUsingModHelper),
                        $"UsingModHelper is not {expectedUsingModHelper}");
            Assert.That(settingsVM.HelperModManager.HelperModStatus,
                        Is.EqualTo(expectedStatus),
                        $"HelperMod status not equal to {expectedStatus}");
        }
    }
}