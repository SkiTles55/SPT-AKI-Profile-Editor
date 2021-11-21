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
        public void BotTypesHeadsNotEmpty() => Assert.IsFalse(AppData.ServerDatabase.Heads.Count == 0);
        [Test]
        public void BotTypesHeadsNotNull() => Assert.IsNotNull(AppData.ServerDatabase.Heads);

        [Test]
        public void BotTypesVoicesNotEmpty() => Assert.IsFalse(AppData.ServerDatabase.Voices.Count == 0);

        [Test]
        public void BotTypesVoicesNotNull() => Assert.IsNotNull(AppData.ServerDatabase.Voices);

        [Test]
        public void LocalesGlobalNotNull() => Assert.IsNotNull(AppData.ServerDatabase.LocalesGlobal);

        [Test]
        public void LocalesGlobalTradingNotEmpty() => Assert.IsFalse(AppData.ServerDatabase.LocalesGlobal.Trading.Count == 0);

        [Test]
        public void LocalesGlobalTemplatesNotEmpty() => Assert.IsFalse(AppData.ServerDatabase.LocalesGlobal.Templates.Count == 0);

        [Test]
        public void LocalesGlobalCustomizationNotEmpty() => Assert.IsFalse(AppData.ServerDatabase.LocalesGlobal.Customization.Count == 0);

        [Test]
        public void LocalesGlobalQuestNotEmpty() => Assert.IsFalse(AppData.ServerDatabase.LocalesGlobal.Quests.Count == 0);
        [Test]
        public void LocalesGlobalInterfaceNotEmpty() => Assert.IsFalse(AppData.ServerDatabase.LocalesGlobal.Interface.Count == 0);

        [Test]
        public void ServerGlobalsNotNull() => Assert.IsNotNull(AppData.ServerDatabase.ServerGlobals);

        [Test]
        public void ServerGlobalsConfigNotNull() => Assert.IsNotNull(AppData.ServerDatabase.ServerGlobals.Config);

        [Test]
        public void ServerGlobalsConfigExpLevelExpTableNotEmpty() => Assert.IsTrue(AppData.ServerDatabase.ServerGlobals.Config.Exp.Level.ExpTable.Length > 0);

        [Test]
        public void ServerGlobalsMasteringNotEmpty() => Assert.IsTrue(AppData.ServerDatabase.ServerGlobals.Config.Mastering.Length > 0);
        
        [Test]
        public void TraderInfosNotEmpty() => Assert.IsFalse(AppData.ServerDatabase.TraderInfos.Count == 0);

        [Test]
        public void TraderInfosNotNul() => Assert.IsNotNull(AppData.ServerDatabase.TraderInfos);

        [Test]
        public void QuestsDataNotNul() => Assert.IsNotNull(AppData.ServerDatabase.QuestsData);

        [Test]
        public void QuestsDataNotEmpty() => Assert.IsFalse(AppData.ServerDatabase.QuestsData.Count == 0);

        [Test]
        public void HideoutAreaInfosNotNul() => Assert.IsNotNull(AppData.ServerDatabase.HideoutAreaInfos);

        [Test]
        public void HideoutAreaInfosDataNotEmpty() => Assert.IsFalse(AppData.ServerDatabase.HideoutAreaInfos.Count == 0);

        [Test]
        public void ItemsDBNotNul() => Assert.IsNotNull(AppData.ServerDatabase.ItemsDB);

        [Test]
        public void ItemsDBNotEmpty() => Assert.IsFalse(AppData.ServerDatabase.ItemsDB.Count == 0);

        [Test]
        public void PocketsNotNul() => Assert.IsNotNull(AppData.ServerDatabase.Pockets);

        [Test]
        public void PocketsNotEmpty() => Assert.IsFalse(AppData.ServerDatabase.Pockets.Count == 0);

        [Test]
        public void TraderSuitsNotNul() => Assert.IsNotNull(AppData.ServerDatabase.TraderSuits);

        [Test]
        public void TraderSuitsNotEmpty() => Assert.IsFalse(AppData.ServerDatabase.TraderSuits.Count == 0);
    }
}