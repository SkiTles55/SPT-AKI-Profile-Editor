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
        Profile profile;

        [OneTimeSetUp]
        public void Setup()
        {
            AppData.AppSettings.ServerPath = @"C:\SPT";
            AppData.LoadDatabase();
            profile = new();
            profile.Load(profileFile);
        }

        [Test]
        public void CharactersNotNull() => Assert.IsNotNull(profile.Characters);

        [Test]
        public void PmcNotNull() => Assert.IsNotNull(profile.Characters.Pmc);

        [Test]
        public void IdNotEmpty() => Assert.IsNotNull(profile.Characters.Pmc.Aid, "Id is empty");

        [Test]
        public void InfoNotNull() => Assert.IsNotNull(profile.Characters.Pmc.Info);

        [Test]
        public void NicknameNotEmpty() => Assert.IsNotNull(profile.Characters.Pmc.Info.Nickname, "Nickname is empty");

        [Test]
        public void SideNotEmpty() => Assert.IsNotNull(profile.Characters.Pmc.Info.Side, "Side is empty");

        [Test]
        public void VoiceNotEmpty() => Assert.IsNotNull(profile.Characters.Pmc.Info.Voice, "Voice is empty");

        [Test]
        public void ExperienceNotZero() => Assert.AreNotEqual(0, profile.Characters.Pmc.Info.Experience, "Experience is zero");

        [Test]
        public void LevelNotZero() => Assert.AreNotEqual(0, profile.Characters.Pmc.Info.Level, "Level is zero");

        [Test]
        public void GameVersionNotEmpty() => Assert.IsNotNull(profile.Characters.Pmc.Info.GameVersion, "GameVersion is empty");

        [Test]
        public void CustomizationNotNully() => Assert.IsNotNull(profile.Characters.Pmc.Customization);

        [Test]
        public void HeadNotEmpty() => Assert.IsNotNull(profile.Characters.Pmc.Customization.Head, "Head is empty");

        [Test]
        public void TraderStandingsNotEmpty() => Assert.AreNotEqual(new Dictionary<string, CharacterTraderStanding>(), profile.Characters.Pmc.TraderStandings, "TraderStandings is empty");

        [Test]
        public void TraderStandingsNotNull() => Assert.IsNotNull(profile.Characters.Pmc.TraderStandings, "TraderStandings is null");

        [Test]
        public void QuestsNotNull() => Assert.IsNotNull(profile.Characters.Pmc.Quests, "Quests is null");

        [Test]
        public void QuestsNotEmpty() => Assert.IsFalse(profile.Characters.Pmc.Quests.Length == 0, "Quests is empty");

        [Test]
        public void ProfileSavesCorrectly()
        {
            var expected = File.ReadAllText(profileFile);
            string testFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "test.json");
            profile.Save(profileFile, testFile);
            var result = File.ReadAllText(profileFile);
            Assert.AreEqual(expected, result);
        }
    }
}