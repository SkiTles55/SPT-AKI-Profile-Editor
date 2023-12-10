using NUnit.Framework;
using SPT_AKI_Profile_Editor.Core;
using SPT_AKI_Profile_Editor.Core.Enums;
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