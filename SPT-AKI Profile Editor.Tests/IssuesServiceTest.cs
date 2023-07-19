using NUnit.Framework;
using SPT_AKI_Profile_Editor.Core;
using SPT_AKI_Profile_Editor.Core.Issues;
using SPT_AKI_Profile_Editor.Tests.Hepers;
using System.Linq;

namespace SPT_AKI_Profile_Editor.Tests
{
    internal class IssuesServiceTest
    {
        [OneTimeSetUp]
        public void Setup()
        {
            AppData.AppSettings.AutoAddMissingQuests = true;
            TestHelpers.LoadDatabase();
        }

        [Test]
        public void IssuesServiceCanFixAllIssues()
        {
            AppData.Profile.Load(TestHelpers.profileWithDuplicatedItems);
            AppData.Profile.Characters.Pmc.Info.Level = 1;
            AppData.Profile.Characters.Pmc.SetAllTradersMax();
            AppData.Profile.Characters.Pmc.SetAllQuests(Core.Enums.QuestStatus.AvailableForStart);
            AppData.IssuesService.GetIssues();
            Assert.True(AppData.IssuesService.HasIssues, "Profile Issues is empty");
            Assert.IsNotEmpty(AppData.IssuesService.ProfileIssues, "Profile Issues is empty");
            Assert.False(AppData.IssuesService.ProfileIssues.Any(x => string.IsNullOrEmpty(x.Description)), "Profile Issues has no description");
            AppData.IssuesService.FixAllIssues();
            AppData.IssuesService.GetIssues();
            Assert.False(AppData.IssuesService.HasIssues, "Profile Issues is not empty");
            Assert.IsEmpty(AppData.IssuesService.ProfileIssues, "Profile Issues is not empty");
        }

        [Test]
        public void PMCLevelIssuesByTradersNotEmpty()
        {
            LoadProfileSetLevel1AndAllTradersMax(TestHelpers.profileFile);
            CheckIssuesNotEmptyAfterGet();
            CheckIssues<PMCLevelIssue>("Traders");
            Assert.True(AppData.IssuesService.ProfileIssues.Any(x => AppData.ServerDatabase.TraderInfos.ContainsKey(x.TargetId)), "ProfileIssues does not have PMC Level Issues by Tarders");
        }

        [Test]
        public void PMCLevelIssuesByQuestsNotEmpty()
        {
            AppData.Profile.Load(TestHelpers.profileFile);
            AppData.Profile.Characters.Pmc.Info.Level = 1;
            AppData.Profile.Characters.Pmc.SetAllQuests(Core.Enums.QuestStatus.AvailableForStart);
            CheckIssuesNotEmptyAfterGet();
            CheckIssues<PMCLevelIssue>("Quests");
            Assert.True(AppData.IssuesService.ProfileIssues.Any(x => AppData.ServerDatabase.QuestsData.ContainsKey(x.TargetId)), "ProfileIssues does not have PMC Level Issues by Quests");
        }

        [Test]
        public void QuestStatusIssuesByQuestNotEmpty()
        {
            AppData.Profile.Load(TestHelpers.profileFile);
            AppData.Profile.Characters.Pmc.SetAllQuests(Core.Enums.QuestStatus.Success);
            CheckIssuesNotEmptyAfterGet();
            CheckIssues<QuestStatusIssue>("Quests");
            Assert.True(AppData.IssuesService.ProfileIssues.Any(x => AppData.ServerDatabase.QuestsData.ContainsKey(x.TargetId)), "ProfileIssues does not have Quest Status Issues");
        }

        [Test]
        public void TraderLoyaltyIssuesByQuestNotEmpty()
        {
            AppData.Profile.Load(TestHelpers.profileFile);
            foreach (var trader in AppData.Profile.Characters.Pmc.TraderStandingsExt)
                trader.LoyaltyLevel = 1;
            AppData.Profile.Characters.Pmc.SetAllQuests(Core.Enums.QuestStatus.Success);
            CheckIssuesNotEmptyAfterGet();
            CheckIssues<TraderLoyaltyIssue>("Quests & Traders");
            Assert.True(AppData.IssuesService.ProfileIssues.Any(x => AppData.ServerDatabase.QuestsData.ContainsKey(x.TargetId)), "ProfileIssues does not have Trader Loyalty Issues");
        }

        [Test]
        public void TraderStandingIssuesByQuestNotEmpty()
        {
            AppData.Profile.Load(TestHelpers.profileFile);
            foreach (var trader in AppData.Profile.Characters.Pmc.TraderStandingsExt)
                trader.LoyaltyLevel = 1;
            AppData.Profile.Characters.Pmc.SetAllQuests(Core.Enums.QuestStatus.Success);
            CheckIssuesNotEmptyAfterGet();
            CheckIssues<TraderStandingIssue>("Quests & Traders");
            Assert.True(AppData.IssuesService.ProfileIssues.Any(x => AppData.ServerDatabase.QuestsData.ContainsKey(x.TargetId)), "ProfileIssues does not have Trader Standing Issues");
        }

        [Test]
        public void IssuesServiceCanFixPMCLevelIssue()
        {
            LoadProfileSetLevel1AndAllTradersMax(TestHelpers.profileFile);
            CheckIssuesNotEmptyAfterGet();
            Assert.False(AppData.IssuesService.ProfileIssues.Any(x => string.IsNullOrEmpty(x.Description)), "Profile Issues has no description");
            var firstIssue = AppData.IssuesService.ProfileIssues.Where(x => x is PMCLevelIssue).First();
            Assert.NotNull(firstIssue, "First PMC Level Issue is null");
            firstIssue.FixAction.Invoke();
            AppData.IssuesService.GetIssues();
            Assert.IsEmpty(AppData.IssuesService.ProfileIssues.Where(x => x.TargetId == firstIssue.TargetId && x is PMCLevelIssue), "First PMC Level Issue not fixed");
        }

        [Test]
        public void IssuesServiceCanFixAllPMCLevelIssues()
        {
            LoadProfileSetLevel1AndAllTradersMax(TestHelpers.profileFile);
            CheckIssuesNotEmptyAfterGet();
            Assert.False(AppData.IssuesService.ProfileIssues.Any(x => string.IsNullOrEmpty(x.Description)), "Profile Issues has no description");
            AppData.IssuesService.FixAllIssues();
            AppData.IssuesService.GetIssues();
            Assert.IsEmpty(AppData.IssuesService.ProfileIssues.Where(x => x is PMCLevelIssue), "PMC Level Issues not fixed");
        }

        [Test]
        public void DuplicateItemsIDIssuesNotEmpty()
        {
            AppData.Profile.Load(TestHelpers.profileWithDuplicatedItems);
            CheckIssuesNotEmptyAfterGet();
            Assert.False(AppData.IssuesService.ProfileIssues.Any(x => string.IsNullOrEmpty(x.Description)), "Profile Issues has no description");
            Assert.True(AppData.IssuesService.ProfileIssues.Any(x => x is DuplicateItemsIDIssue && x.TargetId == "PMC"), "ProfileIssues does not have Duplicate Items ID Issues");
        }

        [Test]
        public void IssuesServiceCanFixDuplicateItemsIDIssue()
        {
            AppData.Profile.Load(TestHelpers.profileWithDuplicatedItems);
            CheckIssuesNotEmptyAfterGet();
            Assert.False(AppData.IssuesService.ProfileIssues.Any(x => string.IsNullOrEmpty(x.Description)), "Profile Issues has no description");
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
            AppData.Profile.Load(TestHelpers.profileFile);
            AppData.Profile.Characters.Pmc.Info.Level = 2;
            AppData.Profile.Characters.Pmc.SetAllQuests(Core.Enums.QuestStatus.Success);
            CheckIssuesNotEmptyAfterGet();
            Assert.False(AppData.IssuesService.ProfileIssues.Any(x => string.IsNullOrEmpty(x.Description)), "Profile Issues has no description");
            var firstIssue = AppData.IssuesService.ProfileIssues.Where(x => x is QuestStatusIssue).First();
            Assert.NotNull(firstIssue, "First Quest Status Issue is null");
            firstIssue.FixAction.Invoke();
            AppData.IssuesService.GetIssues();
            Assert.IsEmpty(AppData.IssuesService.ProfileIssues.Where(x => x.TargetId == firstIssue.TargetId && x is QuestStatusIssue), "First Quest Status Issue not fixed");
        }

        [Test]
        public void IssuesServiceCanFixAllQuestStatusIssues()
        {
            AppData.Profile.Load(TestHelpers.profileFile);
            AppData.Profile.Characters.Pmc.Info.Level = 2;
            AppData.Profile.Characters.Pmc.SetAllQuests(Core.Enums.QuestStatus.Success);
            CheckIssuesNotEmptyAfterGet();
            CheckIssues<QuestStatusIssue>("Quests");
            AppData.IssuesService.FixAllIssues();
            AppData.IssuesService.GetIssues();
            Assert.IsEmpty(AppData.IssuesService.ProfileIssues.Where(x => x is QuestStatusIssue), "Quest Status Issues not fixed");
        }

        [Test]
        public void IssuesServiceCanFixTraderLoyaltyIssue()
        {
            AppData.Profile.Load(TestHelpers.profileFile);
            foreach (var trader in AppData.Profile.Characters.Pmc.TraderStandingsExt)
                trader.LoyaltyLevel = 1;
            AppData.Profile.Characters.Pmc.SetAllQuests(Core.Enums.QuestStatus.Success);
            CheckIssuesNotEmptyAfterGet();
            Assert.False(AppData.IssuesService.ProfileIssues.Any(x => string.IsNullOrEmpty(x.Description)), "Profile Issues has no description");
            var firstIssue = AppData.IssuesService.ProfileIssues.Where(x => x is TraderLoyaltyIssue).FirstOrDefault();
            Assert.NotNull(firstIssue, "First Trader Loyalty Issue is null");
            firstIssue.FixAction.Invoke();
            AppData.IssuesService.GetIssues();
            Assert.IsEmpty(AppData.IssuesService.ProfileIssues.Where(x => x.TargetId == firstIssue.TargetId && x is TraderLoyaltyIssue), "First Trader Loyalty Issue not fixed");
        }

        [Test]
        public void IssuesServiceCanFixAllTraderLoyaltyIssues()
        {
            AppData.Profile.Load(TestHelpers.profileFile);
            foreach (var trader in AppData.Profile.Characters.Pmc.TraderStandingsExt)
                trader.LoyaltyLevel = 1;
            AppData.Profile.Characters.Pmc.SetAllQuests(Core.Enums.QuestStatus.Success);
            CheckIssuesNotEmptyAfterGet();
            CheckIssues<TraderLoyaltyIssue>("Quests & Traders");
            AppData.IssuesService.FixAllIssues();
            AppData.IssuesService.GetIssues();
            Assert.IsEmpty(AppData.IssuesService.ProfileIssues.Where(x => x is TraderLoyaltyIssue), "Trader Loyalty Issues not fixed");
        }

        [Test]
        public void IssuesServiceCanFixTraderStandingIssue()
        {
            AppData.Profile.Load(TestHelpers.profileFile);
            foreach (var trader in AppData.Profile.Characters.Pmc.TraderStandingsExt)
                trader.LoyaltyLevel = 1;
            AppData.Profile.Characters.Pmc.SetAllQuests(Core.Enums.QuestStatus.Success);
            CheckIssuesNotEmptyAfterGet();
            Assert.False(AppData.IssuesService.ProfileIssues.Any(x => string.IsNullOrEmpty(x.Description)), "Profile Issues has no description");
            var firstIssue = AppData.IssuesService.ProfileIssues.Where(x => x is TraderStandingIssue).FirstOrDefault();
            Assert.NotNull(firstIssue, "First Trader Standing Issue is null");
            firstIssue.FixAction.Invoke();
            AppData.IssuesService.GetIssues();
            Assert.IsEmpty(AppData.IssuesService.ProfileIssues.Where(x => x.TargetId == firstIssue.TargetId && x is TraderStandingIssue), "First Trader Standing Issue not fixed");
        }

        [Test]
        public void IssuesServiceCanFixAllTraderStandingIssues()
        {
            AppData.Profile.Load(TestHelpers.profileFile);
            foreach (var trader in AppData.Profile.Characters.Pmc.TraderStandingsExt)
                trader.LoyaltyLevel = 1;
            AppData.Profile.Characters.Pmc.SetAllQuests(Core.Enums.QuestStatus.Success);
            CheckIssuesNotEmptyAfterGet();
            CheckIssues<TraderStandingIssue>("Quests & Traders");
            AppData.IssuesService.FixAllIssues();
            AppData.IssuesService.GetIssues();
            Assert.IsEmpty(AppData.IssuesService.ProfileIssues.Where(x => x is TraderStandingIssue), "Trader Standing Issues not fixed");
        }

        private static void CheckIssues<T>(string source) where T : ProfileIssue
        {
            Assert.False(AppData.IssuesService.ProfileIssues.Any(x => string.IsNullOrEmpty(x.Description)), $"Profile Issues has no {source}");
            Assert.True(AppData.IssuesService.ProfileIssues.Any(x => x is T), $"ProfileIssues does not have {nameof(T)} by {source}");
        }

        private static void LoadProfileSetLevel1AndAllTradersMax(string profilePath)
        {
            AppData.Profile.Load(profilePath);
            AppData.Profile.Characters.Pmc.Info.Level = 1;
            AppData.Profile.Characters.Pmc.SetAllTradersMax();
        }

        private static void CheckIssuesNotEmptyAfterGet()
        {
            AppData.IssuesService.GetIssues();
            Assert.True(AppData.IssuesService.HasIssues, "Profile Issues is empty");
            Assert.IsNotEmpty(AppData.IssuesService.ProfileIssues, "Profile Issues is empty");
        }
    }
}