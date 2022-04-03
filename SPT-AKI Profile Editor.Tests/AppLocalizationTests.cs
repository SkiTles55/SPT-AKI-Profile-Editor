using NUnit.Framework;
using SPT_AKI_Profile_Editor.Core;
using System.Linq;

namespace SPT_AKI_Profile_Editor.Tests
{
    internal class AppLocalizationTests
    {
        private AppSettings appSettings;
        private AppLocalization appLocalization;

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
            Assert.AreEqual(expected.Translations.Count, appLocalization.Translations.Count, "English localization strings count not correct");
            Assert.IsFalse(expected.Translations.Any(x => !appLocalization.Translations.ContainsKey(x.Key)), "English localization does not have all strings");
        }

        [Test]
        public void CreatedRuLocalizationCorrect()
        {
            appLocalization.LoadLocalization("ru");
            var expected = DefaultValues.DefaultLocalizations.Find(x => x.Key == "ru");
            Assert.AreEqual(expected.Translations.Count, appLocalization.Translations.Count, "Russian localization strings count not correct");
            Assert.IsFalse(expected.Translations.Any(x => !appLocalization.Translations.ContainsKey(x.Key)), "Russian localization does not have all strings");
        }

        [Test]
        public void LoadedLocalizationCorrect()
        {
            appLocalization.LoadLocalization(appSettings.Language);
            var expected = DefaultValues.DefaultLocalizations.Find(x => x.Key == appSettings.Language);
            Assert.AreEqual(expected.Translations.Count, appLocalization.Translations.Count, "Loaded localization strings count not correct");
            Assert.IsFalse(expected.Translations.Any(x => !appLocalization.Translations.ContainsKey(x.Key)), "Loaded localization does not have all strings");
        }

        [Test]
        public void LocalizationsDictionaryCorrect()
        {
            var expected = DefaultValues.DefaultLocalizations.ToDictionary(x => x.Key, x => x.Name);
            Assert.AreEqual(expected, appLocalization.Localizations, "Localizations dictionary not correct");
        }
    }
}