using NUnit.Framework;
using SPT_AKI_Profile_Editor.Core;
using SPT_AKI_Profile_Editor.Tests.Hepers;

namespace SPT_AKI_Profile_Editor.Tests.ViewModelsTests
{
    internal class MainWindowViewModelTests
    {
        private static readonly TestsDialogManager dialogManager = new();
        private static readonly TestsApplicationManager applicationManager = new();
        private static readonly TestsWorker worker = new();

        [Test]
        public void InitializeCorrectly()
        {
            MainWindowViewModel _viewModel = new(dialogManager, applicationManager);
            Assert.That(_viewModel, Is.Not.Null, "MainWindowViewModel is null");
            Assert.That(_viewModel.BackupsTabViewModel, Is.Not.Null, "BackupsTabViewModel is null");
            Assert.That(_viewModel.StashTabViewModel, Is.Not.Null, "StashTabViewModel is null");
            Assert.That(_viewModel.WeaponBuildsViewModel, Is.Not.Null, "WeaponBuildsViewModel is null");
            Assert.That(App.DialogCoordinator, Is.Not.Null, "DialogCoordinator is null");
            Assert.That(MainWindowViewModel.WindowTitle, Is.Not.Empty, "WindowTitle is empty");
            Assert.AreEqual(_viewModel, MainWindowViewModel.Instance, "MainWindowViewModel.Instance is not MainWindowViewModel");
        }

        [Test]
        public void CanOpenCloseFastMode()
        {
            MainWindowViewModel.OpenFastModeCommand.Execute(null);
            Assert.That(AppData.AppSettings.FastModeOpened, Is.True, "FastModeOpened is false");
            MainWindowViewModel.OpenFastModeCommand.Execute(null);
            Assert.That(AppData.AppSettings.FastModeOpened, Is.False, "FastModeOpened is true");
        }

        [Test]
        public void CanOpenFAQ()
        {
            applicationManager.LastOpenedUrl = null;
            MainWindowViewModel _viewModel = new(dialogManager, applicationManager);
            _viewModel.OpenFAQ.Execute(null);
            Assert.That(string.IsNullOrEmpty(applicationManager.LastOpenedUrl), Is.False);
        }

        [Test]
        public void CanOpenSettings()
        {
            dialogManager.SettingsDialogOpened = false;
            MainWindowViewModel _viewModel = new(dialogManager, applicationManager);
            _viewModel.OpenSettingsCommand.Execute(null);
            Assert.That(dialogManager.SettingsDialogOpened, Is.True);
        }

        [Test]
        public void CanInitializeViewModel()
        {
            dialogManager.SettingsDialogOpened = false;
            MainWindowViewModel _viewModel = new(dialogManager, applicationManager, worker);
            _viewModel.InitializeViewModelCommand.Execute(null);
            Assert.That(applicationManager.ThemeChanged, Is.True, "Theme Not Changed");
            Assert.That(dialogManager.SettingsDialogOpened, Is.False, "SettingsDialog Opened");
            Assert.That(dialogManager.UpdateDialogOpened, Is.False, "UpdateDialog Opened");
            Assert.That(dialogManager.ShutdownCozServerRunnedOpened, Is.False, "ShutdownCozServerRunned Opened");
            Assert.That(applicationManager.ItemViewWindowsClosed, Is.True, "ItemViewWindows Not Closed");
            Assert.That(AppData.Profile.Characters?.Pmc, Is.Not.Null, "Pmc not loaded");
        }

        [Test]
        public void CanOpenSettingsIfServerPathWrong()
        {
            AppData.AppSettings.ServerPath = TestHelpers.wrongServerPath;
            dialogManager.SettingsDialogOpened = false;
            MainWindowViewModel _viewModel = new(dialogManager, applicationManager, worker);
            _viewModel.InitializeViewModelCommand.Execute(null);
            Assert.That(dialogManager.SettingsDialogOpened, Is.True, "SettingsDialog not Opened");
            AppData.AppSettings.ServerPath = TestHelpers.serverPath;
        }

        [Test]
        public void CanReload()
        {
            MainWindowViewModel _viewModel = new(dialogManager, applicationManager, worker);
            _viewModel.InitializeViewModelCommand.Execute(null);
            var expected = AppData.Profile.Characters.Pmc.Info.Nickname;
            AppData.Profile.Characters.Pmc.Info.Nickname = "TestNickname";
            _viewModel.ReloadButtonCommand.Execute(null);
            Assert.That(AppData.Profile.Characters.Pmc.Info.Nickname, Is.EqualTo(expected).And.Not.EqualTo("TestNickname"));
        }
    }
}