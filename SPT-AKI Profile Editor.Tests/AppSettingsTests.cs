using NUnit.Framework;
using SPT_AKI_Profile_Editor.Core;

namespace SPT_AKI_Profile_Editor.Tests
{
    internal class AppSettingsTests
    {
        private AppSettings settings;

        [OneTimeSetUp]
        public void Setup()
        {
            settings = new();
            settings.Load();
        }

        [Test]
        public void LanguageNotEmpty() => Assert.IsNotNull(settings.Language, "Language is empty");

        [Test]
        public void ColorSchemeNotEmpty() => Assert.IsNotNull(settings.ColorScheme, "ColorScheme is empty");

        [Test]
        public void DirsListCorrect() => Assert.AreEqual(DefaultValues.DefaultDirsList, settings.DirsList, "Default dir list not correct");

        [Test]
        public void FilesListCorrect() => Assert.AreEqual(DefaultValues.DefaultFilesList, settings.FilesList, "Files list not correct");
    }
}