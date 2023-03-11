using Newtonsoft.Json;
using NUnit.Framework;
using SPT_AKI_Profile_Editor.Core;
using SPT_AKI_Profile_Editor.Core.HelperClasses;
using SPT_AKI_Profile_Editor.Tests.Hepers;
using System;
using System.IO;
using System.Linq;

namespace SPT_AKI_Profile_Editor.Tests
{
    internal class TemplatesTests
    {
        [OneTimeSetUp]
        public void Setup() => TestHelpers.LoadDatabase();

        [Test]
        public void RevertedChangeRemoved()
        {
            AppData.Profile.Load(TestHelpers.profileFile);
            var skill = AppData.Profile.Characters.Pmc.Skills.Common.First();
            Assert.IsNotNull(skill, "First skill is null");
            var startValue = skill.Progress;
            skill.Progress = startValue + 100;
            CheckSkillChanges(1, false, "Common");
            skill.Progress = startValue;
            Assert.That(AppData.Profile.Characters.Pmc.Skills.GetAllChanges(),
                        Is.Null,
                        "Reverted change not removed");
        }

        [Test]
        public void UpdatedChangeNotDuplicated()
        {
            AppData.Profile.Load(TestHelpers.profileFile);
            var skill = AppData.Profile.Characters.Pmc.Skills.Common.First();
            Assert.IsNotNull(skill, "First skill is null");
            skill.Progress += 100;
            CheckSkillChanges(1, false, "Common");
            skill.Progress += 100;
            CheckSkillChanges(1, false, "Common");
        }

        [Test]
        public void CanCreateTemplateFromCommonSkills()
        {
            AppData.Profile.Load(TestHelpers.profileFile);
            var skills = AppData.Profile.Characters.Pmc.Skills.Common.Take(3);
            foreach (var skill in skills)
                skill.Progress = 900;
            CheckSkillChanges(3, false, "Common");
            TemplateEntity profileChanges = GetAndCheckProfileChanges();
            string testFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "testChangesCommonSkills.json");
            TemplateEntity result = SaveLoadAndCheckChanges(profileChanges, testFile);
            ReloadProfileAndCheckChanges();
            AppData.Profile.ApplyTemplate(result);
            CheckSkillChanges(3, true, "Common");
            Assert.That(AppData.Profile.Characters.Pmc.Skills.Common.All(x => !skills.Any(s => s.Id == x.Id) || x.Progress == 900),
                        Is.True,
                        "Skills progress changes not applied");
        }

        [Test]
        public void CanCreateTemplateFromMasteringSkills()
        {
            AppData.AppSettings.AutoAddMissingMasterings = true;
            AppData.Profile.Load(TestHelpers.profileFile);
            var skills = AppData.Profile.Characters.Pmc.Skills.Mastering.Take(5);
            foreach (var skill in skills)
                skill.Progress = 300;
            CheckSkillChanges(5, false, "Mastering");
            TemplateEntity profileChanges = GetAndCheckProfileChanges();
            string testFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "testChangesMasterings.json");
            TemplateEntity result = SaveLoadAndCheckChanges(profileChanges, testFile);
            ReloadProfileAndCheckChanges();
            AppData.Profile.ApplyTemplate(result);
            CheckSkillChanges(5, true, "Mastering");
            Assert.That(AppData.Profile.Characters.Pmc.Skills.Mastering.All(x => !skills.Any(s => s.Id == x.Id) || x.Progress == 300),
                        Is.True,
                        "Skills progress changes not applied");
        }

        [Test]
        public void CanCreateTemplateFromCharacterInfo()
        {
            AppData.Profile.Load(TestHelpers.profileFile);
            AppData.Profile.Characters.Pmc.Info.Nickname = "TestCharacterInfoChange";
            var newSide = AppData.Profile.Characters.Pmc.Info.Side == "Bear" ? "Usec" : "Bear";
            AppData.Profile.Characters.Pmc.Info.Side = newSide;
            var newVoice = AppData.ServerDatabase.Voices.Where(x => x.Key != AppData.Profile.Characters.Pmc.Info.Voice).FirstOrDefault().Key;
            AppData.Profile.Characters.Pmc.Info.Voice = newVoice;
            var existExp = AppData.Profile.Characters.Pmc.Info.Experience;
            var existLevel = AppData.Profile.Characters.Pmc.Info.Level;
            AppData.Profile.Characters.Pmc.Info.Level = existLevel + 2;
            var changes = AppData.Profile.Characters.Pmc.Info.GetAllChanges();
            Assert.That(changes, Is.Not.Null, "Changes is null");
            Assert.That(changes.Values, Is.Not.Null, "Changes Values is null");
            Assert.That(changes.Values.Count, Is.EqualTo(5), "Changes Values wrong count");
            TemplateEntity profileChanges = GetAndCheckProfileChanges();
            string testFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "testChangesCharacterInfo.json");
            TemplateEntity result = SaveLoadAndCheckChanges(profileChanges, testFile);
            ReloadProfileAndCheckChanges();
            AppData.Profile.ApplyTemplate(result);
            var newChanges = AppData.Profile.Characters.Pmc.Info.GetAllChanges();
            Assert.That(newChanges, Is.Not.Null, "Changes after apply template is null");
            Assert.That(newChanges.Values, Is.Not.Null, "Changes Values after apply template is null");
            Assert.That(newChanges.Values.Count, Is.EqualTo(5), "Changes Values after apply template wrong count");
            Assert.That(AppData.Profile.Characters.Pmc.Info.Nickname, Is.EqualTo("TestCharacterInfoChange"), "Nickname not changed");
            Assert.That(AppData.Profile.Characters.Pmc.Info.Side, Is.EqualTo(newSide), "Side not changed");
            Assert.That(AppData.Profile.Characters.Pmc.Info.Voice, Is.EqualTo(newVoice), "Voice not changed");
            Assert.That(AppData.Profile.Characters.Pmc.Info.Level, Is.EqualTo(existLevel + 2), "Level not changed");
            Assert.That(AppData.Profile.Characters.Pmc.Info.Experience, Is.Not.EqualTo(existExp), "Experience not changed");
        }

        private static void CheckSkillChanges(int neededCount, bool isApplyed, string prefix)
        {
            var changes = AppData.Profile.Characters.Pmc.Skills.GetAllChanges();
            Assert.That(changes, Is.Not.Null, $"Changes {(isApplyed ? "after apply template" : "")} is null");
            Assert.That(changes.TemplateEntities, Is.Not.Null, $"TemplateEntities {(isApplyed ? "after apply template" : "")} is null");
            Assert.That(changes.TemplateEntities.Count, Is.EqualTo(1), $"TemplateEntities {(isApplyed ? "after apply template" : "")} wrong count");
            Assert.That(changes.TemplateEntities.First().TemplateEntities.Count, Is.EqualTo(neededCount), $"Skills {prefix} TemplateEntities {(isApplyed ? "after apply template" : "")} wrong count");
        }

        private static TemplateEntity SaveLoadAndCheckChanges(TemplateEntity profileChanges, string testFile)
        {
            string json = JsonConvert.SerializeObject(profileChanges, new JsonSerializerSettings() { Formatting = Formatting.Indented, NullValueHandling = NullValueHandling.Ignore });
            File.WriteAllText(testFile, json);
            var result = JsonConvert.DeserializeObject<TemplateEntity>(File.ReadAllText(testFile));
            Assert.That(result == null, Is.False, "Saved template not loaded");
            return result;
        }

        private static void ReloadProfileAndCheckChanges()
        {
            AppData.Profile.Load(TestHelpers.profileFile);
            var newChanges = AppData.Profile.Characters.Pmc.Skills.GetAllChanges();
            Assert.That(newChanges, Is.Null, "Changes after reload not null");
        }

        private static TemplateEntity GetAndCheckProfileChanges()
        {
            var profileChanges = AppData.Profile.GetAllChanges();
            Assert.That(profileChanges, Is.Not.Null, "Profile changes is null");
            Assert.That(profileChanges.TemplateEntities, Is.Not.Null, "Profile changes TemplateEntities is null");
            Assert.That(profileChanges.TemplateEntities.Count, Is.EqualTo(1), "Profile TemplateEntities changes wrong count");
            return profileChanges;
        }
    }
}