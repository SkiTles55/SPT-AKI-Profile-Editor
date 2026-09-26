using NUnit.Framework;
using SPT_AKI_Profile_Editor.Core;
using SPT_AKI_Profile_Editor.Core.Enums;
using SPT_AKI_Profile_Editor.Core.HelperClasses;
using SPT_AKI_Profile_Editor.Tests.Hepers;
using System.IO;

namespace SPT_AKI_Profile_Editor.Tests
{
    internal class AppSettingsTests
    {
        private AppSettings settings;

        [OneTimeSetUp]
        public void Setup()
        {
            settings = new(Path.Combine(TestHelpers.appDataPath, "AppSettings.json"));
            settings.Load();
        }

        [Test]
        public void PathIsServerBaseTrue() => Assert.That(settings.PathIsServerFolder(TestHelpers.serverPath), Is.True);

        [Test]
        public void PathIsServerBaseFalse() => Assert.That(settings.PathIsServerFolder(TestHelpers.wrongServerPath), Is.False);

        [Test]
        public void LanguageNotEmpty() => Assert.That(settings.Language, Is.Not.Null, "Language is empty");

        [Test]
        public void ColorSchemeNotEmpty() => Assert.That(settings.ColorScheme, Is.Not.Null, "ColorScheme is empty");

        [Test]
        public void DirsListCorrect() => Assert.That(settings.DirsList, Is.EqualTo(DefaultValues.DefaultDirsList), "Default dir list not correct");

        [Test]
        public void FilesListCorrect() => Assert.That(settings.FilesList, Is.EqualTo(DefaultValues.DefaultFilesList), "Files list not correct");

        [Test]
        public void ServerDirectoryDefault() => Assert.That(settings.ServerDirectory, Is.EqualTo(DefaultValues.DefaultServerDirectory));

        [Test]
        public void ServerDirectoryNullSetToDefault()
        {
            settings.ServerDirectory = null;
            Assert.That(settings.ServerDirectory, Is.EqualTo(DefaultValues.DefaultServerDirectory));
        }

        [Test]
        public void GetDefaultDirsListCustomServerDir()
        {
            var dirs = DefaultValues.GetDefaultDirsList("CustomDir");
            Assert.That(dirs[SPTServerDir.profiles], Is.EqualTo(Path.Combine("CustomDir", "user", "profiles")));
            Assert.That(dirs[SPTServerDir.globals], Is.EqualTo(Path.Combine("CustomDir", "SPT_Data", "database", "locales", "global")));
        }

        [Test]
        public void GetDefaultDirsListEmptyServerDir()
        {
            var dirs = DefaultValues.GetDefaultDirsList("");
            Assert.That(dirs[SPTServerDir.profiles], Is.EqualTo(Path.Combine("user", "profiles")));
        }

        [Test]
        public void GetDefaultFilesListCustomServerDir()
        {
            var files = DefaultValues.GetDefaultFilesList("CustomDir");
            Assert.That(files[SPTServerFile.serverexe], Is.EqualTo(Path.Combine("CustomDir", "SPT.Server.exe")));
            Assert.That(files[SPTServerFile.globals], Is.EqualTo(Path.Combine("CustomDir", "SPT_Data", "database", "globals.json")));
        }

        [Test]
        public void TryAutoDetectReturnsNullForInvalidPath() => Assert.That(AppSettings.TryAutoDetectServerDirectory(TestHelpers.wrongServerPath), Is.Null);

        [Test]
        public void TryAutoDetectFindsKnownServerDirectory()
        {
            string temp = TestHelpers.CreateFakeServerFolder("SPT_Runtime");
            try
            {
                Assert.That(AppSettings.TryAutoDetectServerDirectory(temp), Is.EqualTo("SPT_Runtime"));
            }
            finally
            {
                Directory.Delete(temp, true);
            }
        }

        [Test]
        public void TryAutoDetectFindsFlatLayout()
        {
            string temp = TestHelpers.CreateFakeServerFolder("");
            try
            {
                Assert.That(AppSettings.TryAutoDetectServerDirectory(temp), Is.EqualTo(""));
            }
            finally
            {
                Directory.Delete(temp, true);
            }
        }

        [Test]
        public void RebuildServerPathsUpdatesDictionaries()
        {
            settings.ServerDirectory = "CustomDir";
            settings.RebuildServerPaths();
            Assert.That(settings.DirsList[SPTServerDir.profiles], Is.EqualTo(Path.Combine("CustomDir", "user", "profiles")));
            Assert.That(settings.FilesList[SPTServerFile.serverexe], Is.EqualTo(Path.Combine("CustomDir", "SPT.Server.exe")));
            settings.ServerDirectory = DefaultValues.DefaultServerDirectory;
            settings.RebuildServerPaths();
            settings.Save();
        }

        [Test]
        public void IssuesActionAlwaysShowSavesCorrectly() => IssuesActionSavesCorrectly(IssuesAction.AlwaysShow);

        [Test]
        public void IssuesActionAlwaysFixSavesCorrectly() => IssuesActionSavesCorrectly(IssuesAction.AlwaysFix);

        [Test]
        public void IssuesActionAlwaysIgnoreSavesCorrectly() => IssuesActionSavesCorrectly(IssuesAction.AlwaysIgnore);

        private void IssuesActionSavesCorrectly(IssuesAction action)
        {
            settings.IssuesAction = action;
            settings.Load();
            Assert.That(settings.IssuesAction, Is.EqualTo(action), $"IssuesAction is not {action}");
        }
    }
}