using NUnit.Framework;
using SPT_AKI_Profile_Editor.Core;
using SPT_AKI_Profile_Editor.Tests.Hepers;
using System.IO;
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
            appSettings = new(Path.Combine(TestHelpers.appDataPath, "AppSettings.json"));
            appSettings.Load();
            appLocalization = new(appSettings.Language, Path.Combine(TestHelpers.appDataPath, "Localizations"));
        }

        [Test]
        public void CreatedEnLocalizationCorrect()
        {
            appLocalization.LoadLocalization("en");
            var expected = DefaultValues.DefaultLocalizations().Find(x => x.Key == "en");
            Assert.AreEqual(expected.Translations.Count, appLocalization.Translations.Count, "English localization strings count not correct");
            Assert.IsFalse(expected.Translations.Any(x => !appLocalization.Translations.ContainsKey(x.Key)), "English localization does not have all strings");
        }

        [Test]
        public void CreatedRuLocalizationCorrect()
        {
            appLocalization.LoadLocalization("ru");
            var expected = DefaultValues.DefaultLocalizations().Find(x => x.Key == "ru");
            Assert.AreEqual(expected.Translations.Count, appLocalization.Translations.Count, "Russian localization strings count not correct");
            Assert.IsFalse(expected.Translations.Any(x => !appLocalization.Translations.ContainsKey(x.Key)), "Russian localization does not have all strings");
        }

        [Test]
        public void CreatedChLocalizationCorrect()
        {
            appLocalization.LoadLocalization("ch");
            var expected = DefaultValues.DefaultLocalizations().Find(x => x.Key == "ch");
            Assert.AreEqual(expected.Translations.Count, appLocalization.Translations.Count, "Chinese localization strings count not correct");
            Assert.IsFalse(expected.Translations.Any(x => !appLocalization.Translations.ContainsKey(x.Key)), "Chinese localization does not have all strings");
        }

        [Test]
        public void LoadedLocalizationCorrect()
        {
            appLocalization.LoadLocalization(appSettings.Language);
            var expected = DefaultValues.DefaultLocalizations().Find(x => x.Key == appSettings.Language);
            Assert.AreEqual(expected.Translations.Count, appLocalization.Translations.Count, "Loaded localization strings count not correct");
            Assert.IsFalse(expected.Translations.Any(x => !appLocalization.Translations.ContainsKey(x.Key)), "Loaded localization does not have all strings");
        }

        [Test]
        public void LocalizationsDictionaryCorrect()
        {
            var expected = DefaultValues.DefaultLocalizations().ToDictionary(x => x.Key, x => x.Name);
            Assert.AreEqual(expected, appLocalization.Localizations, "Localizations dictionary not correct");
        }

        [Test]
        public void CanGetLocalizedString()
        {
            appLocalization.LoadLocalization("en");
            var expected = DefaultValues.DefaultLocalizations().First(x => x.Key == "en").Translations.First();
            Assert.That(appLocalization.GetLocalizedString(expected.Key), Is.EqualTo(expected.Value), "Existing LocalizedString not correct");
            Assert.That(appLocalization.GetLocalizedString("TestNonExistedKey"), Is.EqualTo("TestNonExistedKey"), "Non existing LocalizedString not correct");
        }

        [Test]
        public void CanLoadEnInsteadOfUnknown()
        {
            appLocalization.LoadLocalization("unknown");
            var expected = DefaultValues.DefaultLocalizations().First(x => x.Key == "en").Translations.First();
            Assert.That(appLocalization.GetLocalizedString(expected.Key), Is.EqualTo(expected.Value), "En LocalizedString not correct");
            Assert.That(AppData.AppSettings.Language, Is.EqualTo("en"), "En not loaded");
        }
    }
}