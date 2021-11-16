using NUnit.Framework;
using SPT_AKI_Profile_Editor.Core;
using SPT_AKI_Profile_Editor.Core.ProfileClasses;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SPT_AKI_Profile_Editor.Tests
{
    class ProfileTests
    {
        const string profileFile = @"C:\SPT\user\profiles\4016faed9f0e6231aaf9be73.json";

        [OneTimeSetUp]
        public void Setup()
        {
            AppData.AppSettings.ServerPath = @"C:\SPT";
            AppData.LoadDatabase();
            AppData.Profile.Load(profileFile);
        }

        [Test]
        public void CharactersNotNull() => Assert.IsNotNull(AppData.Profile.Characters);

        [Test]
        public void PmcNotNull() => Assert.IsNotNull(AppData.Profile.Characters.Pmc);

        [Test]
        public void IdNotEmpty() => Assert.IsNotNull(AppData.Profile.Characters.Pmc.Aid, "Id is empty");

        [Test]
        public void InfoNotNull() => Assert.IsNotNull(AppData.Profile.Characters.Pmc.Info);

        [Test]
        public void NicknameNotEmpty() => Assert.IsNotNull(AppData.Profile.Characters.Pmc.Info.Nickname, "Nickname is empty");

        [Test]
        public void SideNotEmpty() => Assert.IsNotNull(AppData.Profile.Characters.Pmc.Info.Side, "Side is empty");

        [Test]
        public void VoiceNotEmpty() => Assert.IsNotNull(AppData.Profile.Characters.Pmc.Info.Voice, "Voice is empty");

        [Test]
        public void ExperienceNotZero() => Assert.AreNotEqual(0, AppData.Profile.Characters.Pmc.Info.Experience, "Experience is zero");

        [Test]
        public void LevelNotZero() => Assert.AreNotEqual(0, AppData.Profile.Characters.Pmc.Info.Level, "Level is zero");

        [Test]
        public void GameVersionNotEmpty() => Assert.IsNotNull(AppData.Profile.Characters.Pmc.Info.GameVersion, "GameVersion is empty");

        [Test]
        public void CustomizationNotNully() => Assert.IsNotNull(AppData.Profile.Characters.Pmc.Customization);

        [Test]
        public void HeadNotEmpty() => Assert.IsNotNull(AppData.Profile.Characters.Pmc.Customization.Head, "Head is empty");

        [Test]
        public void TraderStandingsNotEmpty() => Assert.AreNotEqual(new Dictionary<string, CharacterTraderStanding>(), AppData.Profile.Characters.Pmc.TraderStandings, "TraderStandings is empty");

        [Test]
        public void TraderStandingsNotNull() => Assert.IsNotNull(AppData.Profile.Characters.Pmc.TraderStandings, "TraderStandings is null");

        [Test]
        public void QuestsNotNull() => Assert.IsNotNull(AppData.Profile.Characters.Pmc.Quests, "Quests is null");

        [Test]
        public void QuestsNotEmpty() => Assert.IsFalse(AppData.Profile.Characters.Pmc.Quests.Length == 0, "Quests is empty");

        [Test]
        public void ProfileSavesCorrectly()
        {
            AppData.Profile.Load(profileFile);
            var expected = File.ReadAllText(profileFile);
            string testFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "test.json");
            AppData.Profile.Save(profileFile, testFile);
            var result = File.ReadAllText(profileFile);
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void TradersSavesCorrectly()
        {
            AppData.Profile.Load(profileFile);
            AppData.ServerDatabase.SetAllTradersMax();
            string testFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "testTraders.json");
            AppData.Profile.Save(profileFile, testFile);
            AppData.Profile.Load(testFile);
            Assert.IsTrue(AppData.Profile.Characters.Pmc.TraderStandings.Where(x => x.Key != "ragfair").All(x => x.Value.LoyaltyLevel == AppData.ServerDatabase.TraderInfos[x.Key].MaxLevel));

        }

        [Test]
        public void QuestsStatusesSavesCorrectly()
        {
            AppData.Profile.Load(profileFile);
            AppData.Profile.Characters.Pmc.SetAllQuests("Fail");
            string testFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "testQuests.json");
            AppData.Profile.Save(profileFile, testFile);
            AppData.Profile.Load(testFile);
            Assert.IsTrue(AppData.Profile.Characters.Pmc.Quests.All(x => x.Status == "Fail"));
        }
    }
}