using Newtonsoft.Json;
using NUnit.Framework;
using SPT_AKI_Profile_Editor.Core;
using System;
using System.IO;
using System.Linq;

namespace SPT_AKI_Profile_Editor.Tests
{
    class BackupServiceTests
    {
        BackupService backupService;
        const string profileFile = @"C:\SPT\user\profiles\3716db56a1d2caf6e4161b43.json";

        [OneTimeSetUp]
        public void Setup() => backupService = new();

        [Test]
        public void BackupServiceIsNotNull() => Assert.IsNotNull(backupService);

        [Test]
        public void BackupCreatingCorrectly()
        {
            var expected = JsonConvert.DeserializeObject(File.ReadAllText(profileFile));
            backupService.CreateBackup(profileFile);
            backupService.LoadBackupsList(Path.GetFileNameWithoutExtension(profileFile));
            var result = JsonConvert.DeserializeObject(File.ReadAllText(backupService.BackupList.First().Path));
            Assert.AreEqual(expected.ToString(), result.ToString());
        }

        [Test]
        public void BackupListNotEmpty()
        {
            backupService.LoadBackupsList(Path.GetFileNameWithoutExtension(profileFile));
            Assert.IsFalse(backupService.BackupList.Count == 0);
        }

        [Test]
        public void BackupRestoringCorrectly()
        {
            backupService.CreateBackup(profileFile);
            backupService.LoadBackupsList(Path.GetFileNameWithoutExtension(profileFile));
            var expected = JsonConvert.DeserializeObject(File.ReadAllText(backupService.BackupList.First().Path));
            string testPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "testBackup.json");
            backupService.RestoreBackup(backupService.BackupList.First().Path, testPath);
            var result = JsonConvert.DeserializeObject(File.ReadAllText(testPath));
            Assert.AreEqual(expected.ToString(), result.ToString());
        }

        [Test]
        public void BackupRemovingCorrectly()
        {
            backupService.LoadBackupsList(Path.GetFileNameWithoutExtension(profileFile));
            var expected = backupService.BackupList.Count;
            backupService.RemoveBackup(backupService.BackupList.Last().Path);
            backupService.LoadBackupsList(Path.GetFileNameWithoutExtension(profileFile));
            Assert.AreNotEqual(expected, backupService.BackupList.Count);
        }
    }
}
