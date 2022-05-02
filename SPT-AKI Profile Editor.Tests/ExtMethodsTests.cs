using NUnit.Framework;
using SPT_AKI_Profile_Editor.Core;

namespace SPT_AKI_Profile_Editor.Tests
{
    internal class ExtMethodsTests
    {
        private AppSettings settings;

        [OneTimeSetUp]
        public void Setup()
        {
            settings = new();
            settings.Load();
        }

        [Test]
        public void AppSettingsServerPathIsServerBase() => Assert.IsTrue(ExtMethods.PathIsServerFolder(settings));

        [Test]
        public void PathIsServerBaseTrue() => Assert.IsTrue(ExtMethods.PathIsServerFolder(settings, TestConstants.serverPath));

        [Test]
        public void PathIsServerBaseFalse() => Assert.IsFalse(ExtMethods.PathIsServerFolder(settings, @"D:\WinSetupFromUSB"));

        [Test]
        public void IdNotEmpty() => Assert.IsNotEmpty(ExtMethods.GenerateNewId(new string[] { "testid" }));
    }
}