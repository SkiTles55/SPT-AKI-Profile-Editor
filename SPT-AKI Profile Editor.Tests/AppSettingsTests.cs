using NUnit.Framework;
using SPT_AKI_Profile_Editor.Core;
using SPT_AKI_Profile_Editor.Core.Enums;
using System.IO;

namespace SPT_AKI_Profile_Editor.Tests
{
    internal class AppSettingsTests
    {
        private AppSettings settings;

        [OneTimeSetUp]
        public void Setup()
        {
            settings = new(Path.Combine(TestConstants.AppDataPath, "AppSettings.json"));
            settings.Load();
        }

        [Test]
        public void PathIsServerBaseTrue() => Assert.IsTrue(settings.PathIsServerFolder(TestConstants.serverPath));

        [Test]
        public void PathIsServerBaseFalse() => Assert.IsFalse(settings.PathIsServerFolder(@"D:\WinSetupFromUSB"));

        [Test]
        public void LanguageNotEmpty() => Assert.IsNotNull(settings.Language, "Language is empty");

        [Test]
        public void ColorSchemeNotEmpty() => Assert.IsNotNull(settings.ColorScheme, "ColorScheme is empty");

        [Test]
        public void DirsListCorrect() => Assert.AreEqual(DefaultValues.DefaultDirsList, settings.DirsList, "Default dir list not correct");

        [Test]
        public void FilesListCorrect() => Assert.AreEqual(DefaultValues.DefaultFilesList, settings.FilesList, "Files list not correct");

        [Test]
        public void IssuesActionAlwaysShowSavesCorrectly()
        {
            settings.IssuesAction = IssuesAction.AlwaysShow;
            settings.Load();
            Assert.True(settings.IssuesAction == IssuesAction.AlwaysShow, "IssuesAction is not AlwaysShow");
        }

        [Test]
        public void IssuesActionAlwaysFixSavesCorrectly()
        {
            settings.IssuesAction = IssuesAction.AlwaysFix;
            settings.Load();
            Assert.True(settings.IssuesAction == IssuesAction.AlwaysFix, "IssuesAction is not AlwaysFix");
        }

        [Test]
        public void IssuesActionAlwaysIgnoreSavesCorrectly()
        {
            settings.IssuesAction = IssuesAction.AlwaysIgnore;
            settings.Load();
            Assert.True(settings.IssuesAction == IssuesAction.AlwaysIgnore, "IssuesAction is not AlwaysIgnore");
        }
    }
}