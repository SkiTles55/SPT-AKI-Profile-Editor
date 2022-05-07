using NUnit.Framework;
using SPT_AKI_Profile_Editor.Core;
using SPT_AKI_Profile_Editor.Core.Issues;
using System.Linq;

namespace SPT_AKI_Profile_Editor.Tests
{
    internal class IssuesServiceTest
    {
        [OneTimeSetUp]
        public void Setup()
        {
            AppData.AppSettings.ServerPath = TestConstants.serverPath;
            AppData.LoadDatabase();
        }

        [Test]
        public void IssuesServiceCanFixAllIssues()
        {
            AppData.Profile.Load(TestConstants.profileWithDuplicatedItems);
            AppData.Profile.Characters.Pmc.Info.Level = 1;
            AppData.Profile.Characters.Pmc.SetAllTradersMax();
            AppData.Profile.Characters.Pmc.SetAllQuests(Core.Enums.QuestStatus.AvailableForStart);
            AppData.IssuesService.GetIssues();
            Assert.True(AppData.IssuesService.HasIssues, "Profile Issues is empty");
            Assert.IsNotEmpty(AppData.IssuesService.ProfileIssues, "Profile Issues is empty");
            AppData.IssuesService.FixAllIssues();
            AppData.IssuesService.GetIssues();
            Assert.False(AppData.IssuesService.HasIssues, "Profile Issues is not empty");
            Assert.IsEmpty(AppData.IssuesService.ProfileIssues, "Profile Issues is not empty");
        }

        [Test]
        public void PMCLevelIssuesByTradersNotEmpty()
        {
            AppData.Profile.Load(TestConstants.profileFile);
            AppData.Profile.Characters.Pmc.Info.Level = 1;
            AppData.Profile.Characters.Pmc.SetAllTradersMax();
            AppData.IssuesService.GetIssues();
            Assert.True(AppData.IssuesService.HasIssues, "Profile Issues is empty");
            Assert.IsNotEmpty(AppData.IssuesService.ProfileIssues, "Profile Issues is empty");
            Assert.True(AppData.IssuesService.ProfileIssues.Any(x => x is PMCLevelIssue), "ProfileIssues does not have PMC Level Issues by Tarders");
            Assert.True(AppData.IssuesService.ProfileIssues.Any(x => AppData.ServerDatabase.TraderInfos.ContainsKey(x.TargetId)), "ProfileIssues does not have PMC Level Issues by Tarders");
        }

        [Test]
        public void PMCLevelIssuesByQuestsNotEmpty()
        {
            AppData.Profile.Load(TestConstants.profileFile);
            AppData.Profile.Characters.Pmc.Info.Level = 1;
            AppData.Profile.Characters.Pmc.SetAllQuests(Core.Enums.QuestStatus.AvailableForStart);
            AppData.IssuesService.GetIssues();
            Assert.True(AppData.IssuesService.HasIssues, "Profile Issues is empty");
            Assert.IsNotEmpty(AppData.IssuesService.ProfileIssues, "Profile Issues is empty");
            Assert.True(AppData.IssuesService.ProfileIssues.Any(x => x is PMCLevelIssue), "ProfileIssues does not have PMC Level Issues by Quests");
            Assert.True(AppData.IssuesService.ProfileIssues.Any(x => AppData.ServerDatabase.QuestsData.ContainsKey(x.TargetId)), "ProfileIssues does not have PMC Level Issues by Quests");
        }

        [Test]
        public void QuestStatusIssuesNotEmpty()
        {
            AppData.Profile.Load(TestConstants.profileFile);
            AppData.Profile.Characters.Pmc.SetAllQuests(Core.Enums.QuestStatus.Success);
            AppData.IssuesService.GetIssues();
            Assert.True(AppData.IssuesService.HasIssues, "Profile Issues is empty");
            Assert.IsNotEmpty(AppData.IssuesService.ProfileIssues, "Profile Issues is empty");
            Assert.True(AppData.IssuesService.ProfileIssues.Any(x => x is QuestStatusIssue), "ProfileIssues does not have Quest Status Issues");
            Assert.True(AppData.IssuesService.ProfileIssues.Any(x => AppData.ServerDatabase.QuestsData.ContainsKey(x.TargetId)), "ProfileIssues does not have Quest Status Issues");
        }

        [Test]
        public void IssuesServiceCanFixPMCLevelIssue()
        {
            AppData.Profile.Load(TestConstants.profileFile);
            AppData.Profile.Characters.Pmc.Info.Level = 1;
            AppData.Profile.Characters.Pmc.SetAllTradersMax();
            AppData.IssuesService.GetIssues();
            Assert.True(AppData.IssuesService.HasIssues, "Profile Issues is empty");
            Assert.IsNotEmpty(AppData.IssuesService.ProfileIssues, "Profile Issues is empty");
            var firstIssue = AppData.IssuesService.ProfileIssues.Where(x => x is PMCLevelIssue).First();
            Assert.NotNull(firstIssue, "First PMC Level Issue is null");
            firstIssue.FixAction.Invoke();
            AppData.IssuesService.GetIssues();
            Assert.IsEmpty(AppData.IssuesService.ProfileIssues.Where(x => x.TargetId == firstIssue.TargetId && x is PMCLevelIssue), "First PMC Level Issue not fixed");
        }

        [Test]
        public void IssuesServiceCanFixAllPMCLevelIssues()
        {
            AppData.Profile.Load(TestConstants.profileFile);
            AppData.Profile.Characters.Pmc.Info.Level = 1;
            AppData.Profile.Characters.Pmc.SetAllTradersMax();
            AppData.IssuesService.GetIssues();
            Assert.True(AppData.IssuesService.HasIssues, "Profile Issues is empty");
            Assert.IsNotEmpty(AppData.IssuesService.ProfileIssues, "Profile Issues is empty");
            AppData.IssuesService.FixAllIssues();
            AppData.IssuesService.GetIssues();
            Assert.IsEmpty(AppData.IssuesService.ProfileIssues.Where(x => x is PMCLevelIssue), "PMC Level Issues not fixed");
        }

        [Test]
        public void DuplicateItemsIDIssuesNotEmpty()
        {
            AppData.Profile.Load(TestConstants.profileWithDuplicatedItems);
            AppData.IssuesService.GetIssues();
            Assert.True(AppData.IssuesService.HasIssues, "Profile Issues is empty");
            Assert.IsNotEmpty(AppData.IssuesService.ProfileIssues, "Profile Issues is empty");
            Assert.True(AppData.IssuesService.ProfileIssues.Any(x => x is DuplicateItemsIDIssue), "ProfileIssues does not have Duplicate Items ID Issues");
        }

        [Test]
        public void IssuesServiceCanFixDuplicateItemsIDIssue()
        {
            AppData.Profile.Load(TestConstants.profileWithDuplicatedItems);
            AppData.IssuesService.GetIssues();
            Assert.True(AppData.IssuesService.HasIssues, "Profile Issues is empty");
            Assert.IsNotEmpty(AppData.IssuesService.ProfileIssues, "Profile Issues is empty");
            var firstIssue = AppData.IssuesService.ProfileIssues.Where(x => x is DuplicateItemsIDIssue).First();
            Assert.NotNull(firstIssue, "First Duplicate Items ID Issues is null");
            Assert.True(firstIssue.TargetId == "PMC", "First Duplicate Items ID Issue is not PMC");
            firstIssue.FixAction.Invoke();
            AppData.IssuesService.GetIssues();
            Assert.IsEmpty(AppData.IssuesService.ProfileIssues.Where(x => x.TargetId == firstIssue.TargetId && x is DuplicateItemsIDIssue), "First Duplicate Items ID Issues not fixed");
        }

        [Test]
        public void IssuesServiceCanFixQuestStatusIssue()
        {
            AppData.Profile.Load(TestConstants.profileFile);
            AppData.Profile.Characters.Pmc.SetAllQuests(Core.Enums.QuestStatus.Success);
            AppData.IssuesService.GetIssues();
            Assert.True(AppData.IssuesService.HasIssues, "Profile Issues is empty");
            Assert.IsNotEmpty(AppData.IssuesService.ProfileIssues, "Profile Issues is empty");
            var firstIssue = AppData.IssuesService.ProfileIssues.Where(x => x is QuestStatusIssue).First();
            Assert.NotNull(firstIssue, "First Quest Status Issue is null");
            firstIssue.FixAction.Invoke();
            AppData.IssuesService.GetIssues();
            Assert.IsEmpty(AppData.IssuesService.ProfileIssues.Where(x => x.TargetId == firstIssue.TargetId && x is QuestStatusIssue), "First Quest Status Issue not fixed");
        }

        [Test]
        public void IssuesServiceCanFixAllQuestStatusIssues()
        {
            AppData.Profile.Load(TestConstants.profileFile);
            AppData.Profile.Characters.Pmc.SetAllQuests(Core.Enums.QuestStatus.Success);
            AppData.IssuesService.GetIssues();
            Assert.True(AppData.IssuesService.HasIssues, "Profile Issues is empty");
            Assert.IsNotEmpty(AppData.IssuesService.ProfileIssues, "Profile Issues is empty");
            Assert.True(AppData.IssuesService.ProfileIssues.Any(x => x is QuestStatusIssue), "ProfileIssues does not have Quest Status Issues");
            AppData.IssuesService.FixAllIssues();
            AppData.IssuesService.GetIssues();
            Assert.IsEmpty(AppData.IssuesService.ProfileIssues.Where(x => x is QuestStatusIssue), "Quest Status Issues not fixed");
        }
    }
}