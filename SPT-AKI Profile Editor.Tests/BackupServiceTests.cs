using Newtonsoft.Json;
using NUnit.Framework;
using SPT_AKI_Profile_Editor.Core;
using System;
using System.IO;
using System.Linq;

namespace SPT_AKI_Profile_Editor.Tests
{
    internal class BackupServiceTests
    {
        private BackupService backupService;

        [OneTimeSetUp]
        public void Setup() => backupService = new();

        [Test]
        public void BackupServiceIsNotNull() => Assert.IsNotNull(backupService);

        [Test]
        public void BackupCreatingCorrectly()
        {
            var expected = JsonConvert.DeserializeObject(File.ReadAllText(TestConstants.profileFile));
            backupService.CreateBackup(TestConstants.profileFile);
            backupService.LoadBackupsList(Path.GetFileNameWithoutExtension(TestConstants.profileFile));
            var result = JsonConvert.DeserializeObject(File.ReadAllText(backupService.BackupList.First().Path));
            Assert.AreEqual(expected.ToString(), result.ToString());
        }

        [Test]
        public void BackupListNotEmpty()
        {
            backupService.LoadBackupsList(Path.GetFileNameWithoutExtension(TestConstants.profileFile));
            Assert.IsTrue(backupService.BackupList.Any());
        }

        [Test]
        public void BackupRestoringCorrectly()
        {
            backupService.CreateBackup(TestConstants.profileFile);
            backupService.LoadBackupsList(Path.GetFileNameWithoutExtension(TestConstants.profileFile));
            var expected = JsonConvert.DeserializeObject(File.ReadAllText(backupService.BackupList.First().Path));
            string testPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "testBackup.json");
            backupService.RestoreBackup(backupService.BackupList.First().Path, testPath);
            var result = JsonConvert.DeserializeObject(File.ReadAllText(testPath));
            Assert.AreEqual(expected.ToString(), result.ToString());
        }

        [Test]
        public void BackupRemovingCorrectly()
        {
            backupService.LoadBackupsList(Path.GetFileNameWithoutExtension(TestConstants.profileFile));
            var expected = backupService.BackupList.Count();
            backupService.RemoveBackup(backupService.BackupList.Last().Path);
            backupService.LoadBackupsList(Path.GetFileNameWithoutExtension(TestConstants.profileFile));
            Assert.AreNotEqual(expected, backupService.BackupList.Count());
        }
    }
}