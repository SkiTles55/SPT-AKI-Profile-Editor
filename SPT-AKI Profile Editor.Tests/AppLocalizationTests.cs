using NUnit.Framework;
using SPT_AKI_Profile_Editor.Core;
using System.Linq;

namespace SPT_AKI_Profile_Editor.Tests
{
    class AppLocalizationTests
    {
        AppSettings appSettings;
        AppLocalization appLocalization;

        [OneTimeSetUp]
        public void Setup()
        {
            appSettings = new();
            appSettings.Load();
            appLocalization = new(appSettings.Language);
        }

        [Test]
        public void CreatedEnLocalizationCorrect()
        {
            appLocalization.LoadLocalization("en");
            var expected = DefaultValues.DefaultLocalizations.Find(x => x.Key == "en");
            Assert.AreEqual(expected.Translations, appLocalization.Translations, "English localization not correct");
        }

        [Test]
        public void CreatedRuLocalizationCorrect()
        {
            appLocalization.LoadLocalization("ru");
            var expected = DefaultValues.DefaultLocalizations.Find(x => x.Key == "ru");
            Assert.AreEqual(expected.Translations, appLocalization.Translations, "Russian localization not correct");
        }

        [Test]
        public void CreatedGeLocalizationCorrect()
        {
            appLocalization.LoadLocalization("ge");
            var expected = DefaultValues.DefaultLocalizations.Find(x => x.Key == "ge");
            Assert.AreEqual(expected.Translations, appLocalization.Translations, "German localization not correct");
        }

        [Test]
        public void CreatedFrLocalizationCorrect()
        {
            appLocalization.LoadLocalization("fr");
            var expected = DefaultValues.DefaultLocalizations.Find(x => x.Key == "fr");
            Assert.AreEqual(expected.Translations, appLocalization.Translations, "French localization not correct");
        }

        [Test]
        public void CreatedChLocalizationCorrect()
        {
            appLocalization.LoadLocalization("ch");
            var expected = DefaultValues.DefaultLocalizations.Find(x => x.Key == "ch");
            Assert.AreEqual(expected.Translations, appLocalization.Translations, "Chinese localization not correct");
        }

        [Test]
        public void CreatedEsLocalizationCorrect()
        {
            appLocalization.LoadLocalization("es");
            var expected = DefaultValues.DefaultLocalizations.Find(x => x.Key == "es");
            Assert.AreEqual(expected.Translations, appLocalization.Translations, "Spanish localization not correct");
        }

        [Test]
        public void LoadedLocalizationCorrect()
        {
            appLocalization.LoadLocalization(appSettings.Language);
            var expected = DefaultValues.DefaultLocalizations.Find(x => x.Key == appSettings.Language);
            Assert.AreEqual(expected.Translations, appLocalization.Translations, "Loaded localization not correct");
        }

        [Test]
        public void LocalizationsDictionaryCorrect()
        {
            var expected = DefaultValues.DefaultLocalizations.ToDictionary(x => x.Key, x => x.Name);
            Assert.AreEqual(expected, appLocalization.Localizations, "Localizations dictionary not correct");
        }
    }
}