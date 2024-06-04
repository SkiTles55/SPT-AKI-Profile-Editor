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
        public void Setup() => TestHelpers.LoadDatabase();

        [Test]
        public void IssuesServiceCanFixAllIssues()
        {
            AppData.Profile.Load(TestHelpers.profileWithDuplicatedItems);
            AppData.Profile.Characters.Pmc.Info.Level = 1;
            AppData.Profile.Characters.Pmc.SetAllTradersMax();
            AppData.Profile.Characters.Pmc.AddAllMisingQuests(false);
            AppData.Profile.Characters.Pmc.SetAllQuests(Core.Enums.QuestStatus.AvailableForStart);
            AppData.IssuesService.GetIssues();
            Assert.That(AppData.IssuesService.HasIssues, Is.True, "Profile Issues is empty");
            Assert.That(AppData.IssuesService.ProfileIssues, Is.Not.Empty, "Profile Issues is empty");
            Assert.That(AppData.IssuesService.ProfileIssues.Any(x => string.IsNullOrEmpty(x.Description)),
                        Is.False,
                        "Profile Issues has no description");
            AppData.IssuesService.FixAllIssues();
            AppData.IssuesService.GetIssues();
            Assert.That(AppData.IssuesService.HasIssues, Is.False, "Profile Issues is not empty");
            Assert.That(AppData.IssuesService.ProfileIssues, Is.Empty, "Profile Issues is not empty");
        }

        [Test]
        public void PMCLevelIssuesByTradersNotEmpty()
        {
            LoadProfileSetLevel1AndAllTradersMax(TestHelpers.profileFile);
            CheckIssuesNotEmptyAfterGet();
            CheckIssues<PMCLevelIssue>("Traders");
            Assert.That(AppData.IssuesService.ProfileIssues.Any(x => AppData.ServerDatabase.TraderInfos.ContainsKey(x.TargetId)),
                        Is.True,
                        "ProfileIssues does not have PMC Level Issues by Tarders");
        }

        [Test]
        public void PMCLevelIssuesByQuestsNotEmpty()
        {
            AppData.Profile.Load(TestHelpers.profileFile);
            AppData.Profile.Characters.Pmc.Info.Level = 1;
            AppData.Profile.Characters.Pmc.AddAllMisingQuests(false);
            AppData.Profile.Characters.Pmc.SetAllQuests(Core.Enums.QuestStatus.AvailableForStart);
            CheckIssuesNotEmptyAfterGet();
            CheckIssues<PMCLevelIssue>("Quests");
            Assert.That(AppData.IssuesService.ProfileIssues.Any(x => AppData.ServerDatabase.QuestsData.ContainsKey(x.TargetId)),
                        Is.True,
                        "ProfileIssues does not have PMC Level Issues by Quests");
        }

        [Test]
        public void QuestStatusIssuesByQuestNotEmpty()
        {
            AppData.Profile.Load(TestHelpers.profileFile);
            AppData.Profile.Characters.Pmc.AddAllMisingQuests(false);
            AppData.Profile.Characters.Pmc.SetAllQuests(Core.Enums.QuestStatus.Success);
            CheckIssuesNotEmptyAfterGet();
            CheckIssues<QuestStatusIssue>("Quests");
            Assert.That(AppData.IssuesService.ProfileIssues.Any(x => AppData.ServerDatabase.QuestsData.ContainsKey(x.TargetId)),
                        Is.True,
                        "ProfileIssues does not have Quest Status Issues");
        }

        [Test]
        public void TraderLoyaltyIssuesByQuestNotEmpty()
        {
            AppData.Profile.Load(TestHelpers.profileFile);
            AppData.Profile.Characters.Pmc.AddAllMisingQuests(false);
            foreach (var trader in AppData.Profile.Characters.Pmc.TraderStandingsExt)
                trader.LoyaltyLevel = 1;
            AppData.Profile.Characters.Pmc.SetAllQuests(Core.Enums.QuestStatus.Success);
            CheckIssuesNotEmptyAfterGet();
            CheckIssues<TraderLoyaltyIssue>("Quests & Traders");
            Assert.That(AppData.IssuesService.ProfileIssues.Any(x => AppData.ServerDatabase.QuestsData.ContainsKey(x.TargetId)),
                        Is.True,
                        "ProfileIssues does not have Trader Loyalty Issues");
        }

        [Test]
        public void TraderStandingIssuesByQuestNotEmpty()
        {
            AppData.Profile.Load(TestHelpers.profileFile);
            AppData.Profile.Characters.Pmc.AddAllMisingQuests(false);
            foreach (var trader in AppData.Profile.Characters.Pmc.TraderStandingsExt)
                trader.LoyaltyLevel = 1;
            AppData.Profile.Characters.Pmc.SetAllQuests(Core.Enums.QuestStatus.Success);
            CheckIssuesNotEmptyAfterGet();
            CheckIssues<TraderStandingIssue>("Quests & Traders");
            Assert.That(AppData.IssuesService.ProfileIssues.Any(x => AppData.ServerDatabase.QuestsData.ContainsKey(x.TargetId)),
                        Is.True,
                        "ProfileIssues does not have Trader Standing Issues");
        }

        [Test]
        public void IssuesServiceCanFixPMCLevelIssue()
        {
            LoadProfileSetLevel1AndAllTradersMax(TestHelpers.profileFile);
            CheckIssuesNotEmptyAfterGet();
            Assert.That(AppData.IssuesService.ProfileIssues.Any(x => string.IsNullOrEmpty(x.Description)),
                        Is.False,
                        "Profile Issues has no description");
            var firstIssue = AppData.IssuesService.ProfileIssues.Where(x => x is PMCLevelIssue).First();
            Assert.That(firstIssue, Is.Not.Null, "First PMC Level Issue is null");
            firstIssue.FixAction.Invoke();
            AppData.IssuesService.GetIssues();
            Assert.That(AppData.IssuesService.ProfileIssues.Where(x => x.TargetId == firstIssue.TargetId && x is PMCLevelIssue),
                        Is.Empty,
                        "First PMC Level Issue not fixed");
        }

        [Test]
        public void IssuesServiceCanFixAllPMCLevelIssues()
        {
            LoadProfileSetLevel1AndAllTradersMax(TestHelpers.profileFile);
            CheckIssuesNotEmptyAfterGet();
            Assert.That(AppData.IssuesService.ProfileIssues.Any(x => string.IsNullOrEmpty(x.Description)),
                        Is.False,
                        "Profile Issues has no description");
            AppData.IssuesService.FixAllIssues();
            AppData.IssuesService.GetIssues();
            Assert.That(AppData.IssuesService.ProfileIssues.Where(x => x is PMCLevelIssue),
                        Is.Empty,
                        "PMC Level Issues not fixed");
        }

        [Test]
        public void DuplicateItemsIDIssuesNotEmpty()
        {
            AppData.Profile.Load(TestHelpers.profileWithDuplicatedItems);
            CheckIssuesNotEmptyAfterGet();
            Assert.That(AppData.IssuesService.ProfileIssues.Any(x => string.IsNullOrEmpty(x.Description)),
                        Is.False,
                        "Profile Issues has no description");
            Assert.That(AppData.IssuesService.ProfileIssues.Any(x => x is DuplicateItemsIDIssue && x.TargetId == "PMC"),
                        Is.True,
                        "ProfileIssues does not have Duplicate Items ID Issues");
        }

        [Test]
        public void IssuesServiceCanFixDuplicateItemsIDIssue()
        {
            AppData.Profile.Load(TestHelpers.profileWithDuplicatedItems);
            CheckIssuesNotEmptyAfterGet();
            Assert.That(AppData.IssuesService.ProfileIssues.Any(x => string.IsNullOrEmpty(x.Description)),
                        Is.False,
                        "Profile Issues has no description");
            var firstIssue = AppData.IssuesService.ProfileIssues.Where(x => x is DuplicateItemsIDIssue).First();
            Assert.That(firstIssue, Is.Not.Null, "First Duplicate Items ID Issues is null");
            Assert.That(firstIssue.TargetId == "PMC", Is.True, "First Duplicate Items ID Issue is not PMC");
            firstIssue.FixAction.Invoke();
            AppData.IssuesService.GetIssues();
            Assert.That(AppData.IssuesService.ProfileIssues.Where(x => x.TargetId == firstIssue.TargetId && x is DuplicateItemsIDIssue),
                        Is.Empty,
                        "First Duplicate Items ID Issues not fixed");
        }

        [Test]
        public void IssuesServiceCanFixQuestStatusIssue()
        {
            AppData.Profile.Load(TestHelpers.profileFile);
            AppData.Profile.Characters.Pmc.AddAllMisingQuests(false);
            AppData.Profile.Characters.Pmc.Info.Level = 2;
            AppData.Profile.Characters.Pmc.SetAllQuests(Core.Enums.QuestStatus.Success);
            CheckIssuesNotEmptyAfterGet();
            Assert.That(AppData.IssuesService.ProfileIssues.Any(x => string.IsNullOrEmpty(x.Description)),
                        Is.False,
                        "Profile Issues has no description");
            var firstIssue = AppData.IssuesService.ProfileIssues.Where(x => x is QuestStatusIssue).First();
            Assert.That(firstIssue, Is.Not.Null, "First Quest Status Issue is null");
            firstIssue.FixAction.Invoke();
            AppData.IssuesService.GetIssues();
            Assert.That(AppData.IssuesService.ProfileIssues.Where(x => x.TargetId == firstIssue.TargetId && x is QuestStatusIssue),
                        Is.Empty,
                        "First Quest Status Issue not fixed");
        }

        [Test]
        public void IssuesServiceCanFixAllQuestStatusIssues()
        {
            AppData.Profile.Load(TestHelpers.profileFile);
            AppData.Profile.Characters.Pmc.AddAllMisingQuests(false);
            AppData.Profile.Characters.Pmc.Info.Level = 2;
            AppData.Profile.Characters.Pmc.SetAllQuests(Core.Enums.QuestStatus.Success);
            CheckIssuesNotEmptyAfterGet();
            CheckIssues<QuestStatusIssue>("Quests");
            AppData.IssuesService.FixAllIssues();
            AppData.IssuesService.GetIssues();
            Assert.That(AppData.IssuesService.ProfileIssues.Where(x => x is QuestStatusIssue),
                        Is.Empty,
                        "Quest Status Issues not fixed");
        }

        [Test]
        public void IssuesServiceCanFixTraderLoyaltyIssue()
        {
            PrepareTraderAndQuestIssues();
            Assert.That(AppData.IssuesService.ProfileIssues.Any(x => string.IsNullOrEmpty(x.Description)),
                        Is.False,
                        "Profile Issues has no description");
            var firstIssue = AppData.IssuesService.ProfileIssues.Where(x => x is TraderLoyaltyIssue).FirstOrDefault();
            Assert.That(firstIssue, Is.Not.Null, "First Trader Loyalty Issue is null");
            firstIssue.FixAction.Invoke();
            AppData.IssuesService.GetIssues();
            Assert.That(AppData.IssuesService.ProfileIssues.Where(x => x.TargetId == firstIssue.TargetId && x is TraderLoyaltyIssue),
                        Is.Empty,
                        "First Trader Loyalty Issue not fixed");
        }

        [Test]
        public void IssuesServiceCanFixAllTraderLoyaltyIssues()
        {
            PrepareTraderAndQuestIssues();
            CheckIssues<TraderLoyaltyIssue>("Quests & Traders");
            AppData.IssuesService.FixAllIssues();
            AppData.IssuesService.GetIssues();
            Assert.That(AppData.IssuesService.ProfileIssues.Where(x => x is TraderLoyaltyIssue),
                        Is.Empty,
                        "Trader Loyalty Issues not fixed");
        }

        [Test]
        public void IssuesServiceCanFixTraderStandingIssue()
        {
            PrepareTraderAndQuestIssues();
            Assert.That(AppData.IssuesService.ProfileIssues.Any(x => string.IsNullOrEmpty(x.Description)),
                        Is.False,
                        "Profile Issues has no description");
            var firstIssue = AppData.IssuesService.ProfileIssues.Where(x => x is TraderStandingIssue).FirstOrDefault();
            Assert.That(firstIssue, Is.Not.Null, "First Trader Standing Issue is null");
            firstIssue.FixAction.Invoke();
            AppData.IssuesService.GetIssues();
            Assert.That(AppData.IssuesService.ProfileIssues.Where(x => x.TargetId == firstIssue.TargetId && x is TraderStandingIssue),
                        Is.Empty,
                        "First Trader Standing Issue not fixed");
        }

        [Test]
        public void IssuesServiceCanFixAllTraderStandingIssues()
        {
            PrepareTraderAndQuestIssues();
            CheckIssues<TraderStandingIssue>("Quests & Traders");
            AppData.IssuesService.FixAllIssues();
            AppData.IssuesService.GetIssues();
            Assert.That(AppData.IssuesService.ProfileIssues.Where(x => x is TraderStandingIssue),
                        Is.Empty,
                        "Trader Standing Issues not fixed");
        }

        private static void CheckIssues<T>(string source) where T : ProfileIssue
        {
            Assert.That(AppData.IssuesService.ProfileIssues.Any(x => string.IsNullOrEmpty(x.Description)),
                        Is.False,
                        $"Profile Issues has no {source}");
            Assert.That(AppData.IssuesService.ProfileIssues.Any(x => x is T),
                        Is.True,
                        $"ProfileIssues does not have {nameof(T)} by {source}");
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
            Assert.That(AppData.IssuesService.HasIssues, Is.True, "Profile Issues is empty");
            Assert.That(AppData.IssuesService.ProfileIssues, Is.Not.Empty, "Profile Issues is empty");
        }

        private static void PrepareTraderAndQuestIssues()
        {
            AppData.Profile.Load(TestHelpers.profileFile);
            AppData.Profile.Characters.Pmc.AddAllMisingQuests(false);
            foreach (var trader in AppData.Profile.Characters.Pmc.TraderStandingsExt)
                trader.LoyaltyLevel = 1;
            AppData.Profile.Characters.Pmc.SetAllQuests(Core.Enums.QuestStatus.Success);
            CheckIssuesNotEmptyAfterGet();
        }
    }
}