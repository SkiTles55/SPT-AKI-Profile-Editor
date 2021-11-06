using NUnit.Framework;
using SPT_AKI_Profile_Editor.Core;
using SPT_AKI_Profile_Editor.Core.ServerClasses;
using System.Collections.Generic;

namespace SPT_AKI_Profile_Editor.Tests
{
    class ServerDatabaseTests
    {
        [OneTimeSetUp]
        public void Setup()
        {
            AppData.AppSettings.ServerPath = @"C:\SPT";
            AppData.LoadDatabase();
        }

        [Test]
        public void BotTypesHeadsNotEmpty() => Assert.AreNotEqual(new Dictionary<string, string>(), AppData.ServerDatabase.Heads);

        [Test]
        public void BotTypesVoicesNotEmpty() => Assert.AreNotEqual(new Dictionary<string, string>(), AppData.ServerDatabase.Voices);

        [Test]
        public void GlobalCustomizationNotEmpty() => Assert.AreNotEqual(new Dictionary<string, Template>(), AppData.ServerDatabase.Global.Customization);
    }
}