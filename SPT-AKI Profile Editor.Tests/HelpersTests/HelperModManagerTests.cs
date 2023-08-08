using NUnit.Framework;
using SPT_AKI_Profile_Editor.Helpers;
using System;
using System.IO;

namespace SPT_AKI_Profile_Editor.Tests.HelpersTests
{
    internal class HelperModManagerTests
    {
        private static readonly string testDirectoryWithoutMod = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "HelperModTestDirectoryWithoutMod");
        private static readonly string testDirectoryWithMod = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "HelperModTestDirectoryWithMod");
        private static readonly string fakeUpdateUrl = "https://raw.githubusercontent.com/SkiTles55/SPT-AKI-Profile-Editor/";

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

        private static void CheckHelperModManager(bool expectedIsInstalled,
                                                          HelperModStatus expectedStatus,
                                                  bool expectedUpdateAvailable,
                                                  string path,
                                                  bool expectedDbFilesExist)
        {
            HelperModManager helperModManager = new(fakeUpdateUrl, "test", path);
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
            if (!Directory.Exists(testDirectoryWithoutMod))
            {
                Directory.CreateDirectory(testDirectoryWithoutMod);
                Directory.CreateDirectory(Path.Combine(testDirectoryWithoutMod, "src"));
            }
        }

        private static void PrepareFakeDbFiles(bool haveFiles)
        {

        }
    }
}