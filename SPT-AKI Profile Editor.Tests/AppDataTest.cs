using NUnit.Framework;
using SPT_AKI_Profile_Editor.Core;
using System.Collections.Generic;

namespace SPT_AKI_Profile_Editor.Tests
{
    class AppDataTest
    {
        [OneTimeSetUp]
        public void Setup()
        {
            AppData.AppSettings.ServerPath = @"C:\SPT";
            AppData.LoadBotTypes();
        }

        [Test]
        public void BotTypesHeadsNotEmpty() => Assert.AreNotEqual(new Dictionary<string, string>(), AppData.ServerDatabase.Heads);

        [Test]
        public void BotTypesVoicesNotEmpty() => Assert.AreNotEqual(new Dictionary<string, string>(), AppData.ServerDatabase.Voices);
    }
}