using NUnit.Framework;
using SPT_AKI_Profile_Editor.Tests.Hepers;
using SPT_AKI_Profile_Editor.Views;

namespace SPT_AKI_Profile_Editor.Tests.ViewModelsTests
{
    public class ProgressTransferTabViewModelTests
    {
        private TestsWorker worker;
        private TestsWindowsDialogs dialogs;
        private ProgressTransferTabViewModel viewModel;

        [OneTimeSetUp]
        public void Setup()
        {
            TestHelpers.LoadDatabaseAndProfile();
            worker = new TestsWorker();
            dialogs = new TestsWindowsDialogs();
            viewModel = new ProgressTransferTabViewModel(dialogs, worker);
        }

        [Test]
        public void WhenInitialized_ShouldHaveDefaultSettingsModel()
            => Assert.That(viewModel.SettingsModel, Is.Not.Null);

        [Test]
        public void WhenExportProgressCalled_ShouldCallSaveProfileProgressDialogAndAddTaskToWorker()
        {
            dialogs.SaveProfileProgressDialogCalled = false;
            worker.AddTaskCalled = false;
            viewModel.ExportProgress.Execute(null);
            Assert.That(dialogs.SaveProfileProgressDialogCalled, Is.True);
            Assert.That(worker.AddTaskCalled, Is.True);
        }

        [Test]
        public void WhenImportProgressCalled_ShouldCallOpenBuildDialogAndAddTaskToWorker()
        {
            dialogs.OpenBuildDialogCalled = false;
            dialogs.folderBrowserDialogMode = FolderBrowserDialogMode.profileProgressExport;
            worker.AddTaskCalled = false;
            viewModel.ImportProgress.Execute(null);
            Assert.That(dialogs.OpenBuildDialogCalled, Is.True);
            Assert.That(worker.AddTaskCalled, Is.True);
        }

        [Test]
        public void WhenSelectAllCommandExecuted_ShouldSelectAllItems()
        {
            viewModel.SelectAll.Execute(null);
            var allSelected = viewModel.SettingsModel.Info.GroupState == true
            && viewModel.SettingsModel.Merchants
            && viewModel.SettingsModel.Quests
            && viewModel.SettingsModel.Hideout
            && viewModel.SettingsModel.Crafts
            && viewModel.SettingsModel.ExaminedItems
            && viewModel.SettingsModel.Clothing
            && viewModel.SettingsModel.Skills.GroupState == true
            && viewModel.SettingsModel.Masterings.GroupState == true
            && viewModel.SettingsModel.Builds.GroupState == true;
            Assert.That(allSelected, Is.True);
        }

        [Test]
        public void WhenDeselectAllCommandExecuted_ShouldDeselectAllItems()
        {
            viewModel.DeselectAll.Execute(null);
            var allDeselected = viewModel.SettingsModel.Info.GroupState == false
            && !viewModel.SettingsModel.Merchants
            && !viewModel.SettingsModel.Quests
            && !viewModel.SettingsModel.Hideout
            && !viewModel.SettingsModel.Crafts
            && !viewModel.SettingsModel.ExaminedItems
            && !viewModel.SettingsModel.Clothing
            && viewModel.SettingsModel.Skills.GroupState == false
            && viewModel.SettingsModel.Masterings.GroupState == false
            && viewModel.SettingsModel.Builds.GroupState == false;
            Assert.That(allDeselected, Is.True);
        }
    }
}