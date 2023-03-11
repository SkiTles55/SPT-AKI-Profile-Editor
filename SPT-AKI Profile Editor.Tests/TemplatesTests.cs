using Newtonsoft.Json;
using NUnit.Framework;
using SPT_AKI_Profile_Editor.Core;
using SPT_AKI_Profile_Editor.Core.HelperClasses;
using SPT_AKI_Profile_Editor.Tests.Hepers;
using System;
using System.Collections.Generic;
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
            Assert.That(AppData.Profile.Characters.Pmc.Skills.GetAllChanges().Count,
                        Is.EqualTo(1),
                        "Changes wrong count");
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
            Assert.That(AppData.Profile.Characters.Pmc.Skills.GetAllChanges().Count,
                        Is.EqualTo(1),
                        "Changes wrong count");
            skill.Progress += 100;
            Assert.That(AppData.Profile.Characters.Pmc.Skills.GetAllChanges().Count,
                        Is.EqualTo(1),
                        "Updated change duplicated");
        }

        [Test]
        public void CanCreateTemplateFromCommonSkills()
        {
            AppData.Profile.Load(TestHelpers.profileFile);
            var skills = AppData.Profile.Characters.Pmc.Skills.Common.Take(3);
            foreach (var skill in skills)
                skill.Progress = 900;
            var changes = AppData.Profile.Characters.Pmc.Skills.GetAllChanges();
            Assert.That(changes.Count, Is.EqualTo(1), "Changes wrong count");
            Assert.That(changes.First().TemplateEntities.Count, Is.EqualTo(3), "TemplateEntities wrong count");
            var profileChanges = AppData.Profile.GetAllChanges();
            Assert.That(profileChanges.Count, Is.EqualTo(1), "Profile changes wrong count");

            string testFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "testChangesCommonSkills.json");
            string json = JsonConvert.SerializeObject(profileChanges, new JsonSerializerSettings() { Formatting = Formatting.Indented, NullValueHandling = NullValueHandling.Ignore });
            File.WriteAllText(testFile, json);
            var result = JsonConvert.DeserializeObject<List<TemplateEntity>>(File.ReadAllText(testFile));
            Assert.That(result == null, Is.False, "Saved template not loaded");

            AppData.Profile.Load(TestHelpers.profileFile);
            var newChanges = AppData.Profile.Characters.Pmc.Skills.GetAllChanges();
            Assert.That(newChanges, Is.Null, "Changes after reload not null");

            AppData.Profile.ApplyTemplates(result);
            var appliedChanges = AppData.Profile.Characters.Pmc.Skills.GetAllChanges();
            Assert.That(appliedChanges.Count, Is.EqualTo(1), "Changes after apply template wrong count");
            Assert.That(appliedChanges.First().TemplateEntities.Count, Is.EqualTo(3), "TemplateEntities after apply template wrong count");

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
            var changes = AppData.Profile.Characters.Pmc.Skills.GetAllChanges();
            Assert.That(changes.Count, Is.EqualTo(1), "Changes wrong count");
            Assert.That(changes.First().TemplateEntities.Count, Is.EqualTo(5), "TemplateEntities wrong count");
            var profileChanges = AppData.Profile.GetAllChanges();
            Assert.That(profileChanges.Count, Is.EqualTo(1), "Profile changes wrong count");

            string testFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "testChangesMasterings.json");
            string json = JsonConvert.SerializeObject(profileChanges, new JsonSerializerSettings() { Formatting = Formatting.Indented, NullValueHandling = NullValueHandling.Ignore });
            File.WriteAllText(testFile, json);
            var result = JsonConvert.DeserializeObject<List<TemplateEntity>>(File.ReadAllText(testFile));
            Assert.That(result == null, Is.False, "Saved template not loaded");

            AppData.Profile.Load(TestHelpers.profileFile);
            var newChanges = AppData.Profile.Characters.Pmc.Skills.GetAllChanges();
            Assert.That(newChanges, Is.Null, "Changes after reload not null");

            AppData.Profile.ApplyTemplates(result);
            var appliedChanges = AppData.Profile.Characters.Pmc.Skills.GetAllChanges();
            Assert.That(appliedChanges.Count, Is.EqualTo(1), "Changes after apply template wrong count");
            Assert.That(appliedChanges.First().TemplateEntities.Count, Is.EqualTo(5), "TemplateEntities after apply template wrong count");

            Assert.That(AppData.Profile.Characters.Pmc.Skills.Mastering.All(x => !skills.Any(s => s.Id == x.Id) || x.Progress == 300),
                        Is.True,
                        "Skills progress changes not applied");
        }

        [Test]
        public void CanCreateTemplateFromCharacterInfo()
        {
            AppData.Profile.Load(TestHelpers.profileFile);
            AppData.Profile.Characters.Pmc.Info.Nickname = "TestCharacterInfoChange";
            var existExp = AppData.Profile.Characters.Pmc.Info.Experience;
            var existLevel = AppData.Profile.Characters.Pmc.Info.Level;
            AppData.Profile.Characters.Pmc.Info.Level = existLevel + 2;
            var changes = AppData.Profile.Characters.Pmc.Info.GetAllChanges();
            Assert.That(changes.Count, Is.EqualTo(1), "Changes wrong count");
            Assert.That(changes.First().Values.Count, Is.EqualTo(3), "Values wrong count");
            var profileChanges = AppData.Profile.GetAllChanges();
            Assert.That(profileChanges.Count, Is.EqualTo(1), "Profile changes wrong count");

            string testFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "testChangesCharacterInfo.json");
            string json = JsonConvert.SerializeObject(profileChanges, new JsonSerializerSettings() { Formatting = Formatting.Indented, NullValueHandling = NullValueHandling.Ignore });
            File.WriteAllText(testFile, json);
            var result = JsonConvert.DeserializeObject<List<TemplateEntity>>(File.ReadAllText(testFile));
            Assert.That(result == null, Is.False, "Saved template not loaded");
        }
    }
}