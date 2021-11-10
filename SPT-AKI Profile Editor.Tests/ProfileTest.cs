using NUnit.Framework;
using SPT_AKI_Profile_Editor.Core.ProfileClasses;

namespace SPT_AKI_Profile_Editor.Tests
{
    class ProfileTest
    {
        const string profileFile = @"C:\SPT\user\profiles\4016faed9f0e6231aaf9be73.json";
        Profile profile;

        [OneTimeSetUp]
        public void Setup()
        {
            profile = new Profile();
            profile.Load(profileFile);
        }

        [Test]
        public void IdNotEmpty() => Assert.IsNotNull(profile.Characters.Pmc.Aid, "Id is empty");

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
    }
}