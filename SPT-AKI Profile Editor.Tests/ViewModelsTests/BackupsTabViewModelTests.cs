using NUnit.Framework;
using SPT_AKI_Profile_Editor.Core;
using SPT_AKI_Profile_Editor.Tests.Hepers;
using SPT_AKI_Profile_Editor.Views;
using System.Linq;

namespace SPT_AKI_Profile_Editor.Tests.ViewModelsTests
{
    internal class BackupsTabViewModelTests
    {
        [Test]
        public void CanInitialize()
        {
            BackupsTabViewModel viewModel = new(new TestsDialogManager(), null, null);
            Assert.That(viewModel, Is.Not.Null);
        }

        [Test]
        public void HasNeededData() => Assert.That(BackupsTabViewModel.BackupService, Is.Not.Null);

        [Test]
        public void CanCreateBackup()
        {
            BackupsTabViewModel viewModel = new(new TestsDialogManager(), new TestsWorker(), null);
            BackupsTabViewModel.BackupService.LoadBackupsList();
            var startCount = BackupsTabViewModel.BackupService.BackupList.Count();
            BackupsTabViewModel.BackupService.CreateBackup();
            BackupsTabViewModel.BackupService.LoadBackupsList();
            Assert.That(BackupsTabViewModel.BackupService.BackupList.Count(), Is.EqualTo(startCount + 1), "New backup not created");
            Assert.That(BackupsTabViewModel.BackupService.BackupList.All(x => !string.IsNullOrEmpty(x.Path)), Is.True, "Backups does not have path");
            Assert.That(BackupsTabViewModel.BackupService.BackupList.All(x => x.Date < System.DateTime.Now), Is.True, "Backups have wrong date");
            Assert.That(BackupsTabViewModel.BackupService.BackupList.All(x => !string.IsNullOrEmpty(x.FormatedDate)), Is.True, "Backups does not have formatted date");
        }

        [Test]
        public void CanRemoveBackup()
        {
            BackupsTabViewModel viewModel = new(new TestsDialogManager(), new TestsWorker(), null);
            BackupsTabViewModel.BackupService.CreateBackup();
            BackupsTabViewModel.BackupService.LoadBackupsList();
            var backupFile = BackupsTabViewModel.BackupService.BackupList.FirstOrDefault()?.Path;
            Assert.That(backupFile, Is.Not.Null);
            viewModel.RemoveCommand.Execute(backupFile);
            Assert.That(BackupsTabViewModel.BackupService.BackupList.Where(x => x.Path == backupFile).FirstOrDefault(), Is.Null, "Backup not removed");
        }

        [Test]
        public void CanRestoreBackup()
        {
            TestHelpers.LoadDatabaseAndProfile();
            var startValue = AppData.Profile.Characters.Scav.Info.Experience;
            BackupsTabViewModel viewModel = new(new TestsDialogManager(), new TestsWorker(), null);
            BackupsTabViewModel.BackupService.CreateBackup();
            BackupsTabViewModel.BackupService.LoadBackupsList();
            var backupFile = BackupsTabViewModel.BackupService.BackupList.FirstOrDefault()?.Path;
            Assert.That(backupFile, Is.Not.Null);
            AppData.Profile.Characters.Scav.Info.Experience = startValue + 2000;
            Assert.That(AppData.Profile.Characters.Scav.Info.Experience, Is.EqualTo(startValue + 2000));
            viewModel.RestoreCommand.Execute(backupFile);
            Assert.That(AppData.Profile.Characters.Scav.Info.Experience, Is.EqualTo(startValue), "Backup not restored");
            Assert.That(BackupsTabViewModel.BackupService.BackupList.Where(x => x.Path == backupFile).FirstOrDefault(), Is.Null, "Backup not removed after restore");
        }
    }
}