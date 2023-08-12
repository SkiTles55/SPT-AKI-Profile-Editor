using NUnit.Framework;
using SPT_AKI_Profile_Editor.Helpers;
using System;
using System.IO;
using System.Threading.Tasks;

namespace SPT_AKI_Profile_Editor.Tests.HelpersTests
{
    internal class HelperModManagerTests
    {
        private static readonly string testDirectoryWithoutMod = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "HelperModTestDirectoryWithoutMod");
        private static readonly string testDirectoryWithMod = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "HelperModTestDirectoryWithMod");
        private static readonly string fakeUpdateUrl = "https://raw.githubusercontent.com/SkiTles55/SPT-AKI-Profile-Editor/";
        private static readonly string testUpdateUrl = "https://raw.githubusercontent.com/SkiTles55/SPT-AKI-Profile-Editor/mod-helper/SPT-AKI%20Profile%20Editor.Tests/ModHelperTestUpdate/";
        private static readonly string testDirectoryForUpdateDownload = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "HelperModTestDirectoryForUpdateDownload");

        [Test]
        public void CanInitialize()
        {
            HelperModManager helperModManager = new(fakeUpdateUrl, "", testDirectoryWithoutMod);
            Assert.IsNotNull(helperModManager);
        }

        [Test]
        public void CanDetectNotInstalledMod()
        {
            PrepareTestDirectoryWithoutMod();
            CheckHelperModManager(false, HelperModStatus.NotInstalled, false, testDirectoryWithoutMod, false);
        }

        [Test]
        public void CanDetectInstalledModWithoutDb()
        {
            PrepareFakeDbFiles(false);
            CheckHelperModManager(true, HelperModStatus.Installed, false, testDirectoryWithMod, false);
        }

        [Test]
        public void CanDetectInstalledModWithDb()
        {
            PrepareFakeDbFiles(true);
            CheckHelperModManager(true, HelperModStatus.Installed, false, testDirectoryWithMod, true);
        }

        [Test]
        public void CanInstallMod()
        {
            PrepareTestDirectoryWithoutMod();
            HelperModManager helperModManager = new(fakeUpdateUrl, "test", testDirectoryWithoutMod);
            helperModManager.InstallMod();
            CheckCreatedHelperManager(true,
                                      HelperModStatus.Installed,
                                      false,
                                      testDirectoryWithoutMod,
                                      false,
                                      helperModManager);
        }

        [Test]
        public void CanRemoveMod()
        {
            PrepareFakeDbFiles(false);
            HelperModManager helperModManager = new(fakeUpdateUrl, "test", testDirectoryWithMod);
            helperModManager.RemoveMod();
            CheckCreatedHelperManager(false,
                                      HelperModStatus.NotInstalled,
                                      false,
                                      testDirectoryWithMod,
                                      false,
                                      helperModManager);
        }

        [Test]
        public async Task CanDownloadUpdates()
        {
            PrepareTestDirectoryForUpdateDownload();
            PrepareFakeDbFiles(false);
            HelperModManager helperModManager = new(testUpdateUrl,
                                                    testDirectoryForUpdateDownload,
                                                    testDirectoryWithMod);
            await helperModManager.DownloadUpdates();
            CheckCreatedHelperManager(true,
                                      HelperModStatus.UpdateAvailable,
                                      true,
                                      testDirectoryWithMod,
                                      false,
                                      helperModManager);
        }

        [Test]
        public async Task CanUpdateMod()
        {
            PrepareTestDirectoryForUpdateDownload();
            PrepareFakeDbFiles(false);
            HelperModManager helperModManager = new(testUpdateUrl,
                                                    testDirectoryForUpdateDownload,
                                                    testDirectoryWithMod);
            await helperModManager.DownloadUpdates();
            helperModManager.UpdateMod();
            CheckCreatedHelperManager(true,
                                      HelperModStatus.Installed,
                                      false,
                                      testDirectoryWithMod,
                                      false,
                                      helperModManager);
        }

        private static void CheckHelperModManager(bool expectedIsInstalled,
                                                  HelperModStatus expectedStatus,
                                                  bool expectedUpdateAvailable,
                                                  string path,
                                                  bool expectedDbFilesExist)
        {
            HelperModManager helperModManager = new(fakeUpdateUrl, "test", path);
            CheckCreatedHelperManager(expectedIsInstalled,
                                      expectedStatus,
                                      expectedUpdateAvailable,
                                      path,
                                      expectedDbFilesExist,
                                      helperModManager);
        }

        private static void CheckCreatedHelperManager(bool expectedIsInstalled,
                                                      HelperModStatus expectedStatus,
                                                      bool expectedUpdateAvailable,
                                                      string path,
                                                      bool expectedDbFilesExist,
                                                      HelperModManager helperModManager)
        {
            Assert.That(helperModManager.IsInstalled,
                        Is.EqualTo(expectedIsInstalled),
                        $"IsInstalled is not {expectedIsInstalled}");
            Assert.That(helperModManager.HelperModStatus,
                        Is.EqualTo(expectedStatus),
                        $"Mod status not {expectedStatus}");
            Assert.That(helperModManager.UpdateAvailable,
                        Is.EqualTo(expectedUpdateAvailable),
                        $"UpdateAvailable is not {expectedUpdateAvailable}");
            var expectedPath = Path.Combine(path, "exportedDB");
            Assert.That(helperModManager.DbPath, Is.EqualTo(expectedPath), $"DbPath is not {expectedPath}");
            Assert.That(helperModManager.DbFilesExist,
                        Is.EqualTo(expectedDbFilesExist),
                        $"DbFilesExist is not {expectedDbFilesExist}");
        }

        private static void PrepareTestDirectoryWithoutMod()
        {
            if (Directory.Exists(testDirectoryWithoutMod))
                Directory.Delete(testDirectoryWithoutMod, true);
            Directory.CreateDirectory(testDirectoryWithoutMod);
            Directory.CreateDirectory(Path.Combine(testDirectoryWithoutMod, "src"));
        }

        private static void PrepareFakeDbFiles(bool haveFiles)
        {
            if (!Directory.Exists(testDirectoryWithMod))
                Directory.CreateDirectory(testDirectoryWithMod);
            var directoryPath = Path.Combine(testDirectoryWithMod, "exportedDB");
            if (haveFiles)
            {
                if (!Directory.Exists(directoryPath))
                    Directory.CreateDirectory(directoryPath);
                Directory.CreateDirectory(Path.Combine(directoryPath, "testDir"));
                File.WriteAllText(Path.Combine(directoryPath, "test.json"), "test");
            }
            else
            {
                if (Directory.Exists(directoryPath))
                    Directory.Delete(directoryPath, true);
            }
        }

        private static void PrepareTestDirectoryForUpdateDownload()
        {
            if (!Directory.Exists(testDirectoryForUpdateDownload))
                Directory.CreateDirectory(testDirectoryForUpdateDownload);
            foreach (var file in Directory.GetFiles(testDirectoryForUpdateDownload))
                File.Delete(file);
        }
    }
}