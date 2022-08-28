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
            MainWindowViewModel _viewModel = new(applicationManager, null, dialogManager);
            Assert.That(_viewModel, Is.Not.Null, "MainWindowViewModel is null");
            Assert.That(_viewModel.ViewModels, Is.Not.Null, "ViewModels Factory is null");
            Assert.That(App.DialogCoordinator, Is.Not.Null, "DialogCoordinator is null");
            Assert.That(_viewModel.WindowTitle, Is.EqualTo("TestTitle"), "MainWindowViewModel has wrong title");
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
            MainWindowViewModel _viewModel = new(applicationManager, null, dialogManager);
            _viewModel.OpenFAQ.Execute(null);
            Assert.That(string.IsNullOrEmpty(applicationManager.LastOpenedUrl), Is.False);
        }

        [Test]
        public void CanOpenSettings()
        {
            dialogManager.SettingsDialogOpened = false;
            MainWindowViewModel _viewModel = new(applicationManager, null, dialogManager);
            _viewModel.OpenSettingsCommand.Execute(null);
            Assert.That(dialogManager.SettingsDialogOpened, Is.True);
        }

        [Test]
        public void CanInitializeViewModel()
        {
            dialogManager.SettingsDialogOpened = false;
            MainWindowViewModel _viewModel = new(applicationManager, null, dialogManager, worker);
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
            MainWindowViewModel _viewModel = new(applicationManager, null, dialogManager, worker);
            _viewModel.InitializeViewModelCommand.Execute(null);
            Assert.That(dialogManager.SettingsDialogOpened, Is.True, "SettingsDialog not Opened");
            AppData.AppSettings.ServerPath = TestHelpers.serverPath;
        }

        [Test]
        public void CanReload()
        {
            MainWindowViewModel _viewModel = new(applicationManager, null, dialogManager, worker);
            _viewModel.InitializeViewModelCommand.Execute(null);
            var expected = AppData.Profile.Characters.Pmc.Info.Nickname;
            AppData.Profile.Characters.Pmc.Info.Nickname = "TestNickname";
            _viewModel.ReloadButtonCommand.Execute(null);
            Assert.That(AppData.Profile.Characters.Pmc.Info.Nickname, Is.EqualTo(expected).And.Not.EqualTo("TestNickname"));
        }

        [Test]
        public void CanShowIssuesDialog()
        {
            dialogManager.IssuesDialogOpened = false;
            AppData.AppSettings.IssuesAction = Core.Enums.IssuesAction.AlwaysShow;
            MainWindowViewModel _viewModel = new(applicationManager, null, dialogManager, worker);
            _viewModel.InitializeViewModelCommand.Execute(null);
            AppData.Profile.Characters.Pmc.Info.Level = 1;
            AppData.Profile.Characters.Pmc.SetAllTradersMax();
            AppData.Profile.Characters.Pmc.SetAllQuests(Core.Enums.QuestStatus.AvailableForStart);
            _viewModel.SaveButtonCommand.Execute(null);
            Assert.That(dialogManager.IssuesDialogOpened, Is.True);
        }

        [Test]
        public void CanShowShutdownCozServerRunned()
        {
            dialogManager.ShutdownCozServerRunnedOpened = false;
            applicationManager.ServerRunned = true;
            MainWindowViewModel _viewModel = new(applicationManager, null, dialogManager, worker);
            _viewModel.InitializeViewModelCommand.Execute(null);
            Assert.That(dialogManager.ShutdownCozServerRunnedOpened, Is.True);
            applicationManager.ServerRunned = false;
        }

        [Test]
        public void CanShowUpdateDialog()
        {
            dialogManager.UpdateDialogOpened = false;
            applicationManager.HasUpdate = true;
            MainWindowViewModel _viewModel = new(applicationManager, null, dialogManager, worker);
            _viewModel.InitializeViewModelCommand.Execute(null);
            Assert.That(dialogManager.UpdateDialogOpened, Is.True);
            applicationManager.HasUpdate = false;
        }
    }
}