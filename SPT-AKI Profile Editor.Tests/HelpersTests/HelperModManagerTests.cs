using NUnit.Framework;
using SPT_AKI_Profile_Editor.Core;
using SPT_AKI_Profile_Editor.Core.HelperClasses;
using SPT_AKI_Profile_Editor.Helpers;
using SPT_AKI_Profile_Editor.Tests.Hepers;
using System;
using System.IO;
using System.Threading.Tasks;

namespace SPT_AKI_Profile_Editor.Tests.HelpersTests
{
    [NonParallelizable]
    internal class HelperModManagerTests
    {
        private static readonly string testDirectoryWithoutMod = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "HelperModTestDirectoryWithoutMod");
        private static readonly string testDirectoryWithMod = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "HelperModTestDirectoryWithMod");
        private static readonly string fakeUpdateUrl = "https://raw.githubusercontent.com/SkiTles55/SPT-AKI-Profile-Editor/";
        private static readonly string testUpdateUrl = "https://raw.githubusercontent.com/SkiTles55/SPT-AKI-Profile-Editor/master/SPT-AKI%20Profile%20Editor.Tests/ModHelperTestUpdate/";
        private static readonly string testDirectoryForUpdateDownload = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "HelperModTestDirectoryForUpdateDownload");

        private static string originalServerPath;

        [OneTimeSetUp]
        public void SetupServerPath()
        {
            originalServerPath = AppData.AppSettings.ServerPath;
            AppData.AppSettings.ServerPath = AppDomain.CurrentDomain.BaseDirectory;
        }

        [OneTimeTearDown]
        public void RestoreServerPath() => AppData.AppSettings.ServerPath = originalServerPath;

        private static Func<string> ModPathIn(string path) => () => path;

        [Test]
        public void CanInitialize()
        {
            HelperModManager helperModManager = new(fakeUpdateUrl, "", ModPathIn(testDirectoryWithoutMod));
            Assert.That(helperModManager, Is.Not.Null);
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
            HelperModManager helperModManager = new(fakeUpdateUrl, "test", ModPathIn(testDirectoryWithoutMod));
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
            HelperModManager helperModManager = new(fakeUpdateUrl, "test", ModPathIn(testDirectoryWithMod));
            helperModManager.RemoveMod();
            CheckCreatedHelperManager(false,
                                      HelperModStatus.NotInstalled,
                                      false,
                                      testDirectoryWithMod,
                                      false,
                                      helperModManager);
        }

        [Test]
        public void CanRemoveMissingModWithoutError()
        {
            PrepareTestDirectoryWithoutMod();
            HelperModManager helperModManager = new(fakeUpdateUrl, "test", ModPathIn(testDirectoryWithoutMod));
            Assert.DoesNotThrow(helperModManager.RemoveMod, "RemoveMod on missing directory threw");
            Assert.That(helperModManager.IsInstalled, Is.False, "IsInstalled after removing missing mod");
        }

        [Test]
        public void FullDbPathIsNullWithoutServerPath()
        {
            string originalServerPath = AppData.AppSettings.ServerPath;
            try
            {
                AppData.AppSettings.ServerPath = null;
                HelperModManager helperModManager = new(fakeUpdateUrl, "test");

                Assert.That(helperModManager.FullDbPath, Is.Null, "FullDbPath without server path");
                Assert.That(helperModManager.IsInstalled, Is.False, "IsInstalled without server path");
                Assert.That(helperModManager.DbFilesExist, Is.False, "DbFilesExist without server path");
                Assert.DoesNotThrow(helperModManager.RemoveMod, "RemoveMod without server path threw");
                Assert.DoesNotThrow(helperModManager.InstallMod, "InstallMod without server path threw");
            }
            finally
            {
                AppData.AppSettings.ServerPath = originalServerPath;
            }
        }

        [Test]
        public void ResolvesModPathFromConfiguredServerPaths()
        {
            string root = TestHelpers.CreateFakeServerFolder("CustomRuntime");
            string originalServerPath = AppData.AppSettings.ServerPath;
            string originalServerDirectory = AppData.AppSettings.ServerDirectory;
            try
            {
                AppData.AppSettings.ServerPath = root;
                AppData.AppSettings.ServerDirectory = "CustomRuntime";
                HelperModManager helperModManager = new(fakeUpdateUrl, "test");
                string expectedModPath = Path.Combine(root, "CustomRuntime", "user", "mods", "SPT-AKI Profile Editor.ModHelper");

                Assert.That(helperModManager.FullDbPath, Is.EqualTo(Path.Combine(expectedModPath, DefaultValues.ModHelperDbDirectoryName)), "FullDbPath not resolved from settings");
                Assert.That(helperModManager.IsInstalled, Is.False, "Mod should not be installed yet");

                helperModManager.InstallMod();
                Assert.That(File.Exists(Path.Combine(expectedModPath, "SPT-AKI Profile Editor.ModHelper.dll")),
                            Is.True, "Mod was not installed into the configured folder");
                Assert.That(helperModManager.IsInstalled, Is.True, "Mod status after install");
            }
            finally
            {
                AppData.AppSettings.ServerPath = originalServerPath;
                AppData.AppSettings.ServerDirectory = originalServerDirectory;
                Directory.Delete(root, true);
            }
        }

        [Test]
        public void PicksUpServerPathChangesWithoutRestart()
        {
            string originalServerPath = AppData.AppSettings.ServerPath;
            string originalServerDirectory = AppData.AppSettings.ServerDirectory;
            string originalModsPath = AppData.AppSettings.DirsList[SPTServerDir.mods];
            try
            {
                HelperModManager helperModManager = new(fakeUpdateUrl, "test");
                PrepareTestDirectoryWithoutMod();
                AppData.AppSettings.ServerPath = AppDomain.CurrentDomain.BaseDirectory;
                AppData.AppSettings.ServerDirectory = DefaultValues.DefaultServerDirectory;
                AppData.AppSettings.DirsList[SPTServerDir.mods] = testDirectoryWithoutMod;

                Assert.That(helperModManager.IsInstalled, Is.False, "Mod status before install");

                helperModManager.InstallMod();
                Assert.That(helperModManager.IsInstalled, Is.True, "Mod status after install");
                Assert.That(helperModManager.FullDbPath,
                            Is.EqualTo(Path.Combine(testDirectoryWithoutMod,
                                                    DefaultValues.ModHelperDirectoryName,
                                                    DefaultValues.ModHelperDbDirectoryName)),
                            "FullDbPath not following configured entry");
            }
            finally
            {
                AppData.AppSettings.ServerPath = originalServerPath;
                AppData.AppSettings.ServerDirectory = originalServerDirectory;
                AppData.AppSettings.DirsList[SPTServerDir.mods] = originalModsPath;
            }
        }

        [Test]
        public async Task CanDownloadUpdates()
        {
            PrepareTestDirectoryForUpdateDownload();
            PrepareFakeDbFiles(false);
            HelperModManager helperModManager = new(testUpdateUrl,
                                                    testDirectoryForUpdateDownload,
                                                    ModPathIn(testDirectoryWithMod));
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
                                                    ModPathIn(testDirectoryWithMod));
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
            HelperModManager helperModManager = new(fakeUpdateUrl, "test", ModPathIn(path));
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
            var expectedPath = Path.Combine(path, DefaultValues.ModHelperDbDirectoryName);
            Assert.That(helperModManager.FullDbPath, Is.EqualTo(expectedPath), $"FullDbPath is not {expectedPath}");
            Assert.That(helperModManager.DbFilesExist,
                        Is.EqualTo(expectedDbFilesExist),
                        $"DbFilesExist is not {expectedDbFilesExist}");
        }

        private static void PrepareTestDirectoryWithoutMod()
        {
            if (Directory.Exists(testDirectoryWithoutMod))
                Directory.Delete(testDirectoryWithoutMod, true);
            Directory.CreateDirectory(testDirectoryWithoutMod);
        }

        private static void PrepareFakeDbFiles(bool haveFiles)
        {
            if (!Directory.Exists(testDirectoryWithMod))
                Directory.CreateDirectory(testDirectoryWithMod);
            var directoryPath = Path.Combine(testDirectoryWithMod, DefaultValues.ModHelperDbDirectoryName);
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