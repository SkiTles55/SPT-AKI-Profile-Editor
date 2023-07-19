using Newtonsoft.Json;
using NUnit.Framework;
using SPT_AKI_Profile_Editor.Core;
using SPT_AKI_Profile_Editor.Core.Enums;
using SPT_AKI_Profile_Editor.Core.ProfileClasses;
using SPT_AKI_Profile_Editor.Core.ServerClasses;
using SPT_AKI_Profile_Editor.Tests.Hepers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SPT_AKI_Profile_Editor.Tests
{
    internal class ProfileTests
    {
        [OneTimeSetUp]
        public void Setup() => TestHelpers.LoadDatabaseAndProfile();

        [Test]
        public void ProfileHashNotZero() => Assert.IsFalse(AppData.Profile.ProfileHash == 0);

        [Test]
        public void CharactersNotNull() => Assert.IsNotNull(AppData.Profile.Characters);

        [Test]
        public void ProfileNotEmpty() => Assert.IsFalse(AppData.Profile.IsProfileEmpty, "Profile is empty");

        [Test]
        public void SuitsNotEmpty() => Assert.IsFalse(AppData.Profile.Suits.Length == 0);

        [Test]
        public void PmcNotNull() => Assert.IsNotNull(AppData.Profile.Characters.Pmc);

        [Test]
        public void ScavNotNull() => Assert.IsNotNull(AppData.Profile.Characters.Scav);

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
        public void PmcExperienceNotZero() => Assert.AreNotEqual(0, AppData.Profile.Characters.Pmc.Info.Experience, "Experience is zero");

        [Test]
        public void PmcLevelNotZero() => Assert.AreNotEqual(0, AppData.Profile.Characters.Pmc.Info.Level, "Level is zero");

        [Test]
        public void ScavExperienceNotZero() => Assert.AreNotEqual(0, AppData.Profile.Characters.Scav.Info.Experience, "Experience is zero");

        [Test]
        public void ScavLevelNotZero() => Assert.AreNotEqual(0, AppData.Profile.Characters.Scav.Info.Level, "Level is zero");

        [Test]
        public void GameVersionNotEmpty() => Assert.IsNotNull(AppData.Profile.Characters.Pmc.Info.GameVersion, "GameVersion is empty");

        [Test]
        public void CustomizationNotNull() => Assert.IsNotNull(AppData.Profile.Characters.Pmc.Customization);

        [Test]
        public void HealthNotNull() => Assert.IsNotNull(AppData.Profile.Characters.Pmc.Health);

        [Test]
        public void HydrationLoadCorrectly() => CheckCharacterMetric(AppData.Profile.Characters.Pmc.Health.Hydration);

        [Test]
        public void EnergyLoadCorrectly() => CheckCharacterMetric(AppData.Profile.Characters.Pmc.Health.Energy);

        [Test]
        public void BodyPartsNotNull() => Assert.IsNotNull(AppData.Profile.Characters.Pmc.Health.BodyParts);

        [Test]
        public void BodyPartsHeadLoadCorrectly() => CheckCharacterMetric(AppData.Profile.Characters.Pmc.Health.BodyParts.Head?.Health);

        [Test]
        public void BodyPartsChestLoadCorrectly() => CheckCharacterMetric(AppData.Profile.Characters.Pmc.Health.BodyParts.Chest?.Health);

        [Test]
        public void BodyPartsStomachLoadCorrectly() => CheckCharacterMetric(AppData.Profile.Characters.Pmc.Health.BodyParts.Stomach?.Health);

        [Test]
        public void BodyPartsLeftArmLoadCorrectly() => CheckCharacterMetric(AppData.Profile.Characters.Pmc.Health.BodyParts.LeftArm?.Health);

        [Test]
        public void BodyPartsRightArmLoadCorrectly() => CheckCharacterMetric(AppData.Profile.Characters.Pmc.Health.BodyParts.RightArm?.Health);

        [Test]
        public void BodyPartsLeftLegLoadCorrectly() => CheckCharacterMetric(AppData.Profile.Characters.Pmc.Health.BodyParts.LeftLeg?.Health);

        [Test]
        public void BodyPartsRightLegLoadCorrectly() => CheckCharacterMetric(AppData.Profile.Characters.Pmc.Health.BodyParts.RightLeg?.Health);

        [Test]
        public void HeadNotEmpty() => Assert.IsNotNull(AppData.Profile.Characters.Pmc.Customization.Head, "Head is empty");

        [Test]
        public void TraderStandingsNotEmpty() => Assert.IsFalse(AppData.Profile.Characters.Pmc.TraderStandings.Count == 0);

        [Test]
        public void TraderStandingsNotNull() => Assert.IsNotNull(AppData.Profile.Characters.Pmc.TraderStandings, "TraderStandings is null");

        [Test]
        public void RagfairInfoNotNull() => Assert.IsNotNull(AppData.Profile.Characters.Pmc.RagfairInfo, "RagfairInfo is null");

        [Test]
        public void RagfairStandingLoadCorrectly() =>
            Assert.AreEqual(AppData.Profile.Characters.Pmc.RagfairInfo.Rating,
                            AppData.Profile.Characters.Pmc.TraderStandingsExt.First(x => x.Id == AppData.AppSettings.RagfairTraderId).Standing,
                            "Ragfair standing not load correctly");

        [Test]
        public void QuestsLoadCorrectly()
        {
            Assert.IsNotNull(AppData.Profile.Characters.Pmc.Quests, "Quests is null");
            Assert.IsFalse(AppData.Profile.Characters.Pmc.IsQuestsEmpty, "Quests is empty");
            Assert.IsFalse(AppData.Profile.Characters.Pmc.Quests.Any(x => x.LocalizedTraderName == x.QuestTrader), "Quests Localized TraderName's not loaded");
            var temp = AppData.Profile.Characters.Pmc.Quests.Where(x => x.LocalizedQuestName == x.QuestQid);
            Assert.IsFalse(AppData.Profile.Characters.Pmc.Quests.Any(x => x.LocalizedQuestName == x.Qid), "Quests Localized QuestName's not loaded");
            Assert.IsFalse(AppData.Profile.Characters.Pmc.Quests.Any(x => x.LocalizedQuestType == AppData.AppLocalization.GetLocalizedString("tab_quests_unknown_group")), "Quests Localized QuestType's not loaded");
        }

        [Test]
        public void RepeatableQuestsNotNull() => Assert.IsNotNull(AppData.Profile.Characters.Pmc.RepeatableQuests, "RepeatableQuests is null");

        [Test]
        public void RepeatableQuestsNotEmpty() => Assert.IsFalse(AppData.Profile.Characters.Pmc.RepeatableQuests.Length == 0, "RepeatableQuests is empty");

        [Test]
        public void RepeatableQuestsActiveQuestsNotEmpty() => Assert.IsTrue(AppData.Profile.Characters.Pmc.RepeatableQuests.Any(x => x.ActiveQuests.Length > 0), "RepeatableQuests ActiveQuests is empty");

        [Test]
        public void RepeatableQuestsUnknownTypeIsEmpty() => Assert.IsFalse(AppData.Profile.Characters.Pmc.RepeatableQuests.Any(x => x.Type == QuestType.Unknown), "RepeatableQuests Unknown type is not empty");

        [Test]
        public void RepeatableQuestsActiveQuestsWithActiveQuestTypeUnknownIsEmpty() => Assert.IsFalse(AppData.Profile.Characters.Pmc.RepeatableQuests.Any(x => x.ActiveQuests.Any(q => q.Type == ActiveQuestType.Unknown)), "RepeatableQuests ActiveQuests with ActiveQuestType Unknown is not empty");

        [Test]
        public void EncyclopediaNotNull() => Assert.IsNotNull(AppData.Profile.Characters.Pmc.Encyclopedia, "Encyclopedia is null");

        [Test]
        public void EncyclopediaNotEmpty() => Assert.IsFalse(AppData.Profile.Characters.Pmc.Encyclopedia.Count == 0, "Encyclopedia is empty");

        [Test]
        public void ExaminedItemsNotNull() => Assert.IsNotNull(AppData.Profile.Characters.Pmc.ExaminedItems, "ExaminedItems is null");

        [Test]
        public void ExaminedItemsNotEmpty() => Assert.IsTrue(AppData.Profile.Characters.Pmc.ExaminedItems.Any(), "ExaminedItems is empty");

        [Test]
        public void ExaminedItemsLoadedCorrectly() => Assert.IsTrue(AppData.Profile.Characters.Pmc.ExaminedItems
            .Any(x => !string.IsNullOrEmpty(x.Id) && x.Id != x.Name && x.Name != null), "ExaminedItems is not loaded correctly");

        [Test]
        public void HideoutNotNull() => Assert.IsNotNull(AppData.Profile.Characters.Pmc.Hideout, "Hideout is null");

        [Test]
        public void HideoutAreasNotEmpty() => Assert.IsFalse(AppData.Profile.Characters.Pmc.Hideout.Areas.Length == 0, "HideoutAreas is empty");

        [Test]
        public void HideoutAreasHasCorrectLocalizedName() => Assert.False(AppData.Profile.Characters.Pmc.Hideout.Areas.Any(x => x.LocalizedName == $"hideout_area_{x.Type}_name"));

        [Test]
        public void PmcSkillsNotNull() => Assert.IsNotNull(AppData.Profile.Characters.Pmc.Skills, "Pmc skills is null");

        [Test]
        public void PmcCommonSkillsNotEmpty() => Assert.IsFalse(AppData.Profile.Characters.Pmc.IsCommonSkillsEmpty, "Pmc CommonSkills is empty");

        [Test]
        public void PmcCommonSkillsHaveLocalizedNames() => Assert.IsFalse(AppData.Profile.Characters.Pmc.Skills.Common.Any(x => string.IsNullOrEmpty(x.LocalizedName)), "Not all Pmc CommonSkills have localized name");

        [Test]
        public void PmcMasteringSkillsNotEmpty() => Assert.IsFalse(AppData.Profile.Characters.Pmc.IsMasteringsEmpty, "Pmc MasteringSkills is empty");

        [Test]
        public void PmcMasteringSkillsHaveLocalizedNames() => Assert.IsFalse(AppData.Profile.Characters.Pmc.Skills.Mastering.Any(x => string.IsNullOrEmpty(x.LocalizedName)), "Not all Pmc Masterings have localized name");

        [Test]
        public void ScavSkillsNotNull() => Assert.IsNotNull(AppData.Profile.Characters.Scav.Skills, "Scav skills is null");

        [Test]
        public void ScavCommonSkillsNotEmpty() => Assert.IsFalse(AppData.Profile.Characters.Scav.IsCommonSkillsEmpty, "Scav CommonSkills is empty");

        [Test]
        public void ScavCommonSkillsHaveLocalizedNames() => Assert.IsFalse(AppData.Profile.Characters.Scav.Skills.Common.Any(x => string.IsNullOrEmpty(x.LocalizedName)), "Not all Scav CommonSkills have localized name");

        [Test]
        public void ScavMasteringSkillsNotEmpty() => Assert.IsFalse(AppData.Profile.Characters.Scav.IsMasteringsEmpty, "Scav MasteringSkills is empty");

        [Test]
        public void ScavMasteringSkillsHaveLocalizedNames() => Assert.IsFalse(AppData.Profile.Characters.Scav.Skills.Mastering.Any(x => string.IsNullOrEmpty(x.LocalizedName)), "Not all Scav Masterings have localized name");

        [Test]
        public void InventoryNotNull() => Assert.IsNotNull(AppData.Profile.Characters.Pmc.Inventory);

        [Test]
        public void InventoryStashNotEmpty() => Assert.IsNotEmpty(AppData.Profile.Characters.Pmc.Inventory.Stash);

        [Test]
        public void PmcInventoryHasItems() => Assert.True(AppData.Profile.Characters.Pmc.Inventory.HasItems);

        [Test]
        public void PmcInventoryHasPockets() => Assert.True(AppData.Profile.Characters.Pmc.Inventory.Items.Any(x => x.IsPockets));

        [Test]
        public void ScavInventoryNotHasItems() => Assert.False(AppData.Profile.Characters.Scav.Inventory.HasItems);

        [Test]
        public void ScavInventoryHasPockets() => Assert.True(AppData.Profile.Characters.Scav.Inventory.Items.Any(x => x.IsPockets));

        [Test]
        public void PmcInventoryNotContainsModdedItems() => Assert.False(AppData.Profile.Characters.Pmc.Inventory.ContainsModdedItems);

        [Test]
        public void PmcInventoryContainsItemsWithIcons() => Assert.True(AppData.Profile.Characters.Pmc.Inventory.InventoryItems.Any(x => x.CategoryIcon != null));

        [Test]
        public void PmcInventoryContainsItemsWithTag()
        {
            Assert.True(AppData.Profile.Characters.Pmc.Inventory.InventoryItems.Any(x => !string.IsNullOrEmpty(x.Upd?.Tag?.Name)));
            Assert.True(AppData.Profile.Characters.Pmc.Inventory.InventoryItems.Any(x => !string.IsNullOrEmpty(x.Tag)));
        }

        [Test]
        public void PmcInventoryContainsItemsWithCountString() => Assert.True(AppData.Profile.Characters.Pmc.Inventory.InventoryItems.Any(x => !string.IsNullOrEmpty(x.CountString)));

        [Test]
        public void PmcInventoryItemsHaveCorrectLocalizedNames() => Assert.False(AppData.Profile.Characters.Pmc.Inventory.InventoryItems.Any(x => x.LocalizedName == x.Tpl));

        [Test]
        public void ScavInventoryNotContainsModdedItems() => Assert.False(AppData.Profile.Characters.Scav.Inventory.ContainsModdedItems);

        [Test]
        public void PmcPocketsHasItems() => Assert.True(AppData.Profile.Characters.Pmc.Inventory.PocketsHasItems);

        [Test]
        public void ScavPocketsHasItems() => Assert.True(AppData.Profile.Characters.Scav.Inventory.PocketsHasItems);

        [Test]
        public void PmcInventoryHasEquipment() => Assert.True(AppData.Profile.Characters.Pmc.Inventory.HasEquipment);

        [Test]
        public void ScavInventoryHasEquipment() => Assert.True(AppData.Profile.Characters.Scav.Inventory.HasEquipment);

        [Test]
        public void PmcInventoryHaveDollarsCountString() => Assert.False(string.IsNullOrEmpty(AppData.Profile.Characters.Pmc.Inventory.DollarsCount));

        [Test]
        public void PmcInventoryHaveRoublesCountString() => Assert.False(string.IsNullOrEmpty(AppData.Profile.Characters.Pmc.Inventory.RublesCount));

        [Test]
        public void PmcInventoryHaveEurosCountString() => Assert.False(string.IsNullOrEmpty(AppData.Profile.Characters.Pmc.Inventory.EurosCount));

        [Test]
        public void InventoryEquipmentNotEmpty() => Assert.IsNotEmpty(AppData.Profile.Characters.Pmc.Inventory.Equipment);

        [Test]
        public void PmcInventoryFirstPrimaryWeaponNotNull() => Assert.NotNull(AppData.Profile.Characters.Pmc.Inventory.FirstPrimaryWeapon);

        [Test]
        public void ScavInventoryFirstPrimaryWeaponNotNull() => Assert.NotNull(AppData.Profile.Characters.Scav.Inventory.FirstPrimaryWeapon);

        [Test]
        public void PmcInventoryHeadwearNotNull() => Assert.NotNull(AppData.Profile.Characters.Pmc.Inventory.Headwear);

        [Test]
        public void ScavInventoryHeadwearNotNull() => Assert.NotNull(AppData.Profile.Characters.Scav.Inventory.Headwear);

        [Test]
        public void PmcInventoryTacticalVestNotNull() => Assert.NotNull(AppData.Profile.Characters.Pmc.Inventory.TacticalVest);

        [Test]
        public void ScavInventoryTacticalVestNotNull() => Assert.NotNull(AppData.Profile.Characters.Scav.Inventory.TacticalVest);

        [Test]
        public void PmcInventorySecuredContainerNotNull() => Assert.NotNull(AppData.Profile.Characters.Pmc.Inventory.SecuredContainer);

        [Test]
        public void ScavInventorySecuredContainerIsNull() => Assert.Null(AppData.Profile.Characters.Scav.Inventory.SecuredContainer);

        [Test]
        public void PmcInventoryBackpackNotNull() => Assert.NotNull(AppData.Profile.Characters.Pmc.Inventory.Backpack);

        [Test]
        public void ScavInventoryBackpackNotNull() => Assert.NotNull(AppData.Profile.Characters.Scav.Inventory.Backpack);

        [Test]
        public void PmcInventoryEarpieceNotNull() => Assert.NotNull(AppData.Profile.Characters.Pmc.Inventory.Earpiece);

        [Test]
        public void ScavInventoryEarpieceNotNull() => Assert.NotNull(AppData.Profile.Characters.Scav.Inventory.Earpiece);

        [Test]
        public void InventoryItemsNotEmpty() => Assert.IsFalse(AppData.Profile.Characters.Pmc.Inventory.Items.Length == 0);

        [Test]
        public void PmcPocketsNotNull() => Assert.IsNotEmpty(AppData.Profile.Characters.Pmc.Inventory.Pockets);

        [Test]
        public void WeaponBuildsNotNull() => Assert.IsNotNull(AppData.Profile.WeaponBuilds);

        [Test]
        public void WeaponBuildsNotEmpty()
        {
            AppData.Profile.Load(TestHelpers.profileFile);
            Assert.IsFalse(AppData.Profile.WeaponBuilds.Count == 0);
            Assert.IsFalse(AppData.Profile.WBuilds.Count == 0);
        }

        [Test]
        public void PMCStashContainsVerticalItems() => Assert.True(AppData.Profile.Characters.Pmc.Inventory.Items.Any(x => x.Location?.R == ItemRotation.Vertical));

        [Test]
        public void PMCStashContainsContainers() => Assert.True(AppData.Profile.Characters.Pmc.Inventory.Items.Any(x => x.IsContainer));

        [Test]
        public void PMCStashContainsWeapons() => Assert.True(AppData.Profile.Characters.Pmc.Inventory.Items.Any(x => x.IsWeapon));

        [Test]
        public void ScavStashContainsContainers() => Assert.True(AppData.Profile.Characters.Scav.Inventory.Items.Any(x => x.IsContainer));

        [Test]
        public void ScavStashContainsWeapons() => Assert.True(AppData.Profile.Characters.Scav.Inventory.Items.Any(x => x.IsWeapon));

        [Test]
        public void ProfileSavesCorrectly()
        {
            AppData.AppSettings.AutoAddMissingQuests = false;
            AppData.AppSettings.AutoAddMissingMasterings = false;
            AppData.AppSettings.AutoAddMissingScavSkills = false;
            AppData.Profile.Load(TestHelpers.profileFile);
            var expected = JsonConvert.DeserializeObject(File.ReadAllText(TestHelpers.profileFile));
            string testFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "test.json");
            AppData.Profile.Save(TestHelpers.profileFile, testFile);
            var result = JsonConvert.DeserializeObject(File.ReadAllText(testFile));
            Assert.AreEqual(expected.ToString(), result.ToString());
        }

        [Test]
        public void TraderSalesSumAndStandingCanIncreaseLevel()
        {
            AppData.Profile.Load(TestHelpers.profileFile);
            var firstTrader = AppData.Profile.Characters.Pmc.TraderStandingsExt.First(x => x.Id != AppData.AppSettings.RagfairTraderId && x.LoyaltyLevel < 2 && x.TraderBase.LoyaltyLevels.Count > x.LoyaltyLevel);
            Assert.IsNotNull(firstTrader, "Trader for test not found");
            var currentLevel = firstTrader.LoyaltyLevel;
            firstTrader.SalesSum = firstTrader.TraderBase.LoyaltyLevels[currentLevel].MinSalesSum;
            firstTrader.Standing = firstTrader.TraderBase.LoyaltyLevels[currentLevel].MinStanding;
            Assert.IsTrue(firstTrader.LoyaltyLevel == currentLevel + 1);
        }

        [Test]
        public void TradersLoadedCorrectly()
        {
            AppData.Profile.Load(TestHelpers.profileFile);
            Assert.IsFalse(AppData.Profile.Characters.Pmc.TraderStandingsExt.Any(x => string.IsNullOrEmpty(x.Id)), "Traders id's not loaded");
            Assert.IsFalse(AppData.Profile.Characters.Pmc.TraderStandingsExt.Any(x => x.TraderStanding == null), "Traders TraderStanding's not loaded");
            Assert.IsFalse(AppData.Profile.Characters.Pmc.TraderStandingsExt.Any(x => x.TraderBase == null), "Traders TraderBase's not loaded");
            Assert.IsFalse(AppData.Profile.Characters.Pmc.TraderStandingsExt.Any(x => string.IsNullOrEmpty(x.TraderBase.Id)), "Traders TraderBase id's not loaded");
            Assert.IsFalse(AppData.Profile.Characters.Pmc.TraderStandingsExt.Any(x => !x.TraderBase.LoyaltyLevels.Any()), "Traders TraderBase LoyaltyLevels's not loaded");
            Assert.IsFalse(AppData.Profile.Characters.Pmc.TraderStandingsExt.Any(x => x.BitmapImage == null), "Traders BitmapImage's not loaded");
            Assert.IsFalse(AppData.Profile.Characters.Pmc.TraderStandingsExt.Any(x => x.Id != AppData.AppSettings.RagfairTraderId && x.LocalizedName == x.Id), "Traders LocalizedName's not loaded");
            Assert.IsFalse(AppData.Profile.Characters.Pmc.TraderStandingsExt.Any(x => x.SalesSum != x.TraderStanding.SalesSum), "Traders SalesSum's not loaded correctly");
        }

        [Test]
        public void TradersSavesCorrectly()
        {
            AppData.Profile.Load(TestHelpers.profileFile);
            AppData.Profile.Characters.Pmc.SetAllTradersMax();
            SaveAndLoadProfile("testTraders.json");
            Assert.IsTrue(AppData.Profile.Characters.Pmc.TraderStandingsExt
                .All(x => x.LoyaltyLevel == x.MaxLevel), "TraderStandingsExt not in max levels");
        }

        [Test]
        public void QuestsStatusesSavesCorrectly()
        {
            AppData.AppSettings.AutoAddMissingQuests = true;
            AppData.Profile.Load(TestHelpers.profileFile);
            AppData.Profile.Characters.Pmc.SetAllQuests(QuestStatus.Fail);
            SaveAndLoadProfile("testQuests.json");
            Assert.IsTrue(AppData.Profile.Characters.Pmc.Quests.All(x => x.Status == QuestStatus.Fail)
                && AppData.Profile.Characters.Pmc.Quests.All(x => x.StatusTimers.ContainsKey(QuestStatus.Fail))
                && AppData.Profile.Characters.Pmc.Quests.Where(x => x.Type == QuestType.Standart).Count() >= AppData.ServerDatabase.QuestsData.Count);
        }

        [Test]
        public void QuestsFirstLockedStatusSavesCorrectly()
        {
            AppData.AppSettings.AutoAddMissingQuests = true;
            AppData.Profile.Load(TestHelpers.profileFile);
            var locked = AppData.Profile.Characters.Pmc.Quests?
                .Where(x => x.Type == QuestType.Standart && x.Status == QuestStatus.Locked)?
                .FirstOrDefault();
            Assert.IsNotNull(locked, "Cant find locked quest");
            locked.Status = QuestStatus.AvailableForFinish;
            SaveAndLoadProfile("testQuests.json");
            Assert.IsTrue(AppData.Profile.Characters.Pmc.Quests?
                .Where(x => x.Qid == locked.Qid)?
                .First().Status == QuestStatus.AvailableForFinish);
        }

        [Test]
        public void HideoutAreaLevelsSavesCorrectly()
        {
            AppData.Profile.Load(TestHelpers.profileFile);
            AppData.Profile.Characters.Pmc.SetAllHideoutAreasMax();
            SaveAndLoadProfile("testHideouts.json");
            Assert.IsTrue(AppData.Profile.Characters.Pmc.Hideout.Areas
                .All(x => x.Level == x.MaxLevel));
        }

        [Test]
        public void HideoutAreaLevelCantBeSettedGreatherThanMax()
        {
            AppData.Profile.Load(TestHelpers.profileFile);
            var firstArea = AppData.Profile.Characters.Pmc.Hideout.Areas.First();
            var max = firstArea.MaxLevel;
            firstArea.Level = max + 3;
            Assert.IsTrue(firstArea.Level == max);
        }

        [Test]
        public void PmcCommonSkillsSavesCorrectly() => CommonSkillsSavesCorrectly(AppData.Profile.Characters.Pmc, "testPmcCommonSkills.json");

        [Test]
        public void ScavCommonSkillsSavesCorrectly()
        {
            AppData.AppSettings.AutoAddMissingScavSkills = true;
            CommonSkillsSavesCorrectly(AppData.Profile.Characters.Scav, "testScavCommonSkills.json");
            Assert.IsTrue(AppData.Profile.Characters.Scav.Skills.Common.Length >= AppData.Profile.Characters.Pmc.Skills.Common.Length);
        }

        [Test]
        public void PmcMasteringSkillsSavesCorrectly()
        {
            AppData.AppSettings.AutoAddMissingMasterings = true;
            AppData.Profile.Load(TestHelpers.profileFile);
            AppData.Profile.Characters.Pmc.SetAllMasteringsSkills(AppData.ServerDatabase.ServerGlobals.Config.Mastering.Max(x => x.Level2 + x.Level3));
            SaveAndLoadProfile("testPmcMasteringSkills.json");
            var isAllSkillsProgressMax = AppData.Profile.Characters.Pmc.Skills.Mastering.All(x => x.Progress == x.MaxValue);
            var profileSkillsCount = AppData.Profile.Characters.Pmc.Skills.Mastering.Length;
            var dbSkillsCount = AppData.ServerDatabase.ServerGlobals.Config.Mastering.Where(x => !AppData.AppSettings.BannedMasterings.Contains(x.Name)).Count();
            Assert.IsTrue(isAllSkillsProgressMax && profileSkillsCount == dbSkillsCount);
        }

        [Test]
        public void ScavMasteringSkillsSavesCorrectly()
        {
            AppData.AppSettings.AutoAddMissingMasterings = true;
            AppData.Profile.Load(TestHelpers.profileFile);
            AppData.Profile.Characters.Scav.SetAllMasteringsSkills(AppData.ServerDatabase.ServerGlobals.Config.Mastering.Max(x => x.Level2 + x.Level3));
            SaveAndLoadProfile("testScavMasteringSkills.json");
            var isAllSkillsProgressMax = AppData.Profile.Characters.Scav.Skills.Mastering.All(x => x.Progress == x.MaxValue);
            var profileSkillsCount = AppData.Profile.Characters.Scav.Skills.Mastering.Length;
            var dbSkillsCount = AppData.ServerDatabase.ServerGlobals.Config.Mastering.Where(x => !AppData.AppSettings.BannedMasterings.Contains(x.Name)).Count();
            Assert.IsTrue(isAllSkillsProgressMax && profileSkillsCount == dbSkillsCount);
        }

        [Test]
        public void ExaminedItemsSavesCorrectly()
        {
            AppData.Profile.Load(TestHelpers.profileFile);
            AppData.Profile.Characters.Pmc.Encyclopedia = new();
            var expected = AppData.Profile.Characters.Pmc.ExaminedItems.Count();
            AppData.Profile.Characters.Pmc.ExamineAll();
            SaveAndLoadProfile("testExaminedItems.json");
            Assert.AreNotEqual(expected, AppData.Profile.Characters.Pmc.ExaminedItems.Count());
        }

        [Test]
        public void SuitsSavesCorrectly()
        {
            AppData.Profile.Load(TestHelpers.profileFile);
            AppData.ServerDatabase.AcquireAllClothing();
            SaveAndLoadProfile("testSuits.json");
            Assert.IsTrue(AppData.Profile.Suits.Length == AppData.ServerDatabase.TraderSuits.Count);
        }

        [Test]
        public void PmcPocketsSavesCorrectly() => PocketsSavesCorrectly(AppData.Profile.Characters.Pmc.Inventory, "testPmcPockets.json");

        [Test]
        public void ScavPocketsSavesCorrectly() => PocketsSavesCorrectly(AppData.Profile.Characters.Scav.Inventory, "testScavPockets.json");

        [Test]
        public void PmsStashCanGetInnerItems()
        {
            AppData.Profile.Load(TestHelpers.profileFile);
            var weaponId = AppData.Profile.Characters.Pmc.Inventory.InventoryItems.First(x => x.IsWeapon)?.Id;
            Assert.IsFalse(string.IsNullOrEmpty(weaponId), "Weapon not found in pmc stash");
            var innerItems = AppData.Profile.Characters.Pmc.Inventory.GetInnerItems(weaponId);
            Assert.IsNotEmpty(innerItems, "Inner items of weapon empty");
        }

        [Test]
        public void PmcStashRemovingItemsSavesCorrectly()
        {
            AppData.Profile.Load(TestHelpers.profileFile);
            string expectedId1 = AppData.Profile.Characters.Pmc.Inventory.InventoryItems.First().Id;
            string expectedId2 = AppData.Profile.Characters.Pmc.Inventory.InventoryItems.Where(x => x.Id != expectedId1 && x.ParentId != expectedId1).First().Id;
            RemovingItemsSavesCorrectly(new() { expectedId1, expectedId2 }, AppData.Profile.Characters.Pmc.Inventory, "testStashRemovingItems.json");
        }

        [Test]
        public void ScavStashRemovingItemsSavesCorrectly()
        {
            AppData.Profile.Load(TestHelpers.profileFile);
            string expectedId1 = AppData.Profile.Characters.Scav.Inventory.TacticalVest?.Id;
            string expectedId2 = AppData.Profile.Characters.Scav.Inventory.FirstPrimaryWeapon?.Id ?? AppData.Profile.Characters.Scav.Inventory.Holster?.Id;
            Assert.IsNotNull(expectedId1, "TacticalVest is null");
            Assert.IsNotNull(expectedId2, "FirstPrimaryWeapon is null");
            RemovingItemsSavesCorrectly(new() { expectedId1, expectedId2 }, AppData.Profile.Characters.Scav.Inventory, "testScavStashRemovingItems.json");
        }

        [Test]
        public void PmcStashRemovingAllItemsSavesCorrectly()
        {
            AppData.Profile.Load(TestHelpers.profileFile);
            AppData.Profile.Characters.Pmc.Inventory.RemoveAllItems();
            SaveAndLoadProfile("testStashRemovingAllItems.json");
            Assert.AreEqual(0, AppData.Profile.Characters.Pmc.Inventory.InventoryItems.Count());
        }

        [Test]
        public void PmcStashRemovingAllItemsRunsCorrectly()
        {
            AppData.Profile.Load(TestHelpers.profileFile);
            var ids = AppData.Profile.Characters.Pmc.Inventory.Items.Select(x => x.Id);
            AppData.Profile.Characters.Pmc.Inventory.RemoveAllItems();
            var missedItems = AppData.Profile.Characters.Pmc.Inventory.Items.Where(x => x.ParentId != null && !ids.Contains(x.ParentId));
            Assert.IsEmpty(missedItems);
        }

        [Test]
        public void PmcStashRemovingAllEquipmentSavesCorrectly()
        {
            RemovingAllEquipmentSavesCorrectly(AppData.Profile.Characters.Pmc.Inventory, "testStashRemovingAllEquipment.json");
            Assert.True(AppData.Profile.Characters.Pmc.Inventory.InventoryItems.Any());
        }

        [Test]
        public void ScavStashRemovingAllEquipmentSavesCorrectly()
            => RemovingAllEquipmentSavesCorrectly(AppData.Profile.Characters.Scav.Inventory, "testScavStashRemovingAllEquipment.json");

        [Test]
        public void Stash2DMapCalculatingCorrectly()
        {
            AppData.Profile.Load(TestHelpers.profileFile);
            InventoryItem ProfileStash = AppData.Profile.Characters.Pmc.Inventory.Items.Where(x => x.Id == AppData.Profile.Characters.Pmc.Inventory.Stash).FirstOrDefault();
            var stash2d = AppData.Profile.Characters.Pmc.Inventory.GetSlotsMap(ProfileStash);
            Assert.AreNotEqual(new int[0, 0], stash2d);
            Assert.IsFalse(stash2d.Cast<int>().All(x => x == 0));
        }

        [Test]
        public void StashAddingItemsSavesCorrectly()
        {
            AppData.Profile.Load(TestHelpers.profileFile);
            AppData.Profile.Characters.Pmc.Inventory.RemoveAllItems();
            var largestItems = AppData.ServerDatabase.ItemsDB
                .Where(x => !AppData.Profile.Characters.Pmc.Inventory.InventoryItems
                .Any(y => y.Tpl == x.Key) && x.Value.CanBeAddedToStash)
                .OrderByDescending(x => x.Value.Properties?.Width + x.Value.Properties?.Height)
                .ToArray();
            var item1 = largestItems[0].Value;
            var item2 = largestItems[1].Value;
            item1.AddingQuantity = 1;
            item1.AddingFir = false;
            item2.AddingQuantity = 2;
            item2.AddingFir = true;
            AppData.Profile.Characters.Pmc.Inventory.AddNewItemsToStash(item2);
            AppData.Profile.Characters.Pmc.Inventory.AddNewItemsToStash(item1);
            SaveAndLoadProfile("testStashAddingItems.json");
            var savedItems = AppData.Profile.Characters.Pmc.Inventory.InventoryItems
                .Where(x => x.Tpl == item1.Id || x.Tpl == item2.Id).ToArray();
            Assert.AreEqual(3, savedItems.Length);
            Assert.AreEqual(true, savedItems[0].Upd.SpawnedInSession);
            Assert.AreEqual(true, savedItems[1].Upd.SpawnedInSession);
            Assert.AreEqual(false, savedItems[2].Upd.SpawnedInSession);
            Assert.AreNotEqual(savedItems[0].Id, savedItems[1].Id);
            Assert.AreNotEqual(savedItems[2].Id, savedItems[1].Id);
            Assert.IsFalse(AppData.Profile.Characters.Pmc.Inventory.InventoryItems
                .Where(x => AppData.Profile.Characters.Pmc.Inventory.InventoryItems
                .Any(y => y.Id != x.Id && y.Location.X == x.Location.X && y.Location.Y == x.Location.Y))
                .Any());
        }

        [Test]
        public void AddingItemsToContainerSavesCorrectly()
        {
            AppData.Profile.Load(TestHelpers.profileFile);
            var sick = AppData.ServerDatabase.ItemsDB["5c0a840b86f7742ffa4f2482"];
            var sickCases = AppData.Profile.Characters.Pmc.Inventory.Items.Where(x => x.Tpl == sick.Id).Select(x => x.Id);
            sick.AddingQuantity = 1;
            sick.AddingFir = true;
            AppData.Profile.Characters.Pmc.Inventory.AddNewItemsToStash(sick);
            var newItems = AppData.ServerDatabase.ItemsDB.Where(x => x.Value?.Properties?.Width > 2).Take(3).Select(x => x.Value);
            var tempSick = AppData.Profile.Characters.Pmc.Inventory.Items.Where(x => x.Tpl == sick.Id).LastOrDefault();
            foreach (var newItem in newItems)
            {
                newItem.AddingQuantity = 1;
                newItem.AddingFir = true;
                AppData.Profile.Characters.Pmc.Inventory.AddNewItemsToContainer(tempSick, newItem, "main");
            }
            SaveAndLoadProfile("testStashAddingItems.json");
            var addedSick = AppData.Profile.Characters.Pmc.Inventory.Items.Where(x => x.Tpl == sick.Id).LastOrDefault();
            Assert.NotNull(addedSick);
            Assert.False(sickCases.Contains(addedSick.Id));
            var addedItemsToSick = AppData.Profile.Characters.Pmc.Inventory.Items.Where(x => x.ParentId == addedSick.Id);
            Assert.AreEqual(3, addedItemsToSick.Count());
        }

        [Test]
        public void StashAddingMoneysSavesCorrectly()
        {
            AppData.Profile.Load(TestHelpers.profileFile);
            var startValue = AppData.Profile.Characters.Pmc.Inventory.Items
                .Where(x => x.Tpl == AppData.AppSettings.MoneysRublesTpl)
                .Sum(x => x.Upd.StackObjectsCount ?? 0);
            var roubles = AppData.ServerDatabase.ItemsDB[AppData.AppSettings.MoneysRublesTpl];
            roubles.AddingQuantity = 2000000;
            AppData.Profile.Characters.Pmc.Inventory.AddNewItemsToStash(roubles);
            SaveAndLoadProfile("testStashAddingMoneys.json");
            var endValue = AppData.Profile.Characters.Pmc.Inventory.Items
                .Where(x => x.Tpl == AppData.AppSettings.MoneysRublesTpl)
                .Sum(x => x.Upd.StackObjectsCount ?? 0);
            Assert.AreEqual(startValue + 2000000, endValue);
            Assert.IsFalse(AppData.Profile.Characters.Pmc.Inventory.InventoryItems
                .Where(x => AppData.Profile.Characters.Pmc.Inventory.InventoryItems
                .Any(y => y.Id != x.Id && y.Location.X == x.Location.X && y.Location.Y == x.Location.Y))
                .Any());
        }

        [Test]
        public void StashAddingDogtagWithPropertiesSavesCorrectly()
        {
            AppData.Profile.Load(TestHelpers.profileFile);
            var dogtag = AppData.ServerDatabase.ItemsDB.Values.Where(x => x.Properties?.DogTagQualities == true).FirstOrDefault();
            var newDogtag = TarkovItem.CopyFrom(dogtag);
            newDogtag.DogtagProperties.Nickname = "Test";
            newDogtag.DogtagProperties.Level = 69;
            newDogtag.DogtagProperties.UpdateProperties();
            AppData.Profile.Characters.Pmc.Inventory.AddNewItemsToStash(newDogtag);
            SaveAndLoadProfile("testStashAddingDogtag.json");
            var addedDogtag = AppData.Profile.Characters.Pmc.Inventory.InventoryItems.Where(x => x.Tpl == newDogtag.Id).LastOrDefault();
            Assert.NotNull(addedDogtag, "Added dogtag not founded");
            Assert.AreNotEqual(addedDogtag.Upd.Dogtag.AccountId, AppData.Profile.Characters.Pmc.Aid);
            Assert.AreNotEqual(addedDogtag.Upd.Dogtag.ProfileId, AppData.Profile.Characters.Pmc.Aid);
            Assert.AreEqual(addedDogtag.Upd.Dogtag.KillerAccountId, AppData.Profile.Characters.Pmc.Aid);
            Assert.AreEqual(addedDogtag.Upd.Dogtag.KillerProfileId, AppData.Profile.Characters.Pmc.PmcId);
            Assert.IsFalse(string.IsNullOrEmpty(addedDogtag.Upd.Dogtag.Time));
            Assert.AreEqual(addedDogtag.Upd.Dogtag.KillerName, AppData.Profile.Characters.Pmc.Info.Nickname);
            Assert.IsTrue(addedDogtag.Upd.Dogtag.WeaponName.EndsWith(" Name"));
            Assert.IsTrue(addedDogtag.Upd.Dogtag.WeaponName.Length > " Name".Length);
            Assert.AreEqual(addedDogtag.Upd.Dogtag.Nickname, "Test");
            Assert.AreEqual(addedDogtag.Upd.Dogtag.Level, 69);
            Assert.IsFalse(string.IsNullOrEmpty(addedDogtag.Upd.Dogtag.Status));
            Assert.IsFalse(string.IsNullOrEmpty(addedDogtag.Upd.Dogtag.Side));
        }

        [Test]
        public void WeaponBuildRemoveSavesCorrectly()
        {
            LoadProfileAndPrepareWeaponBuilds();
            var expected = AppData.Profile.WeaponBuilds.FirstOrDefault().Key;
            AppData.Profile.RemoveBuild(expected);
            SaveAndLoadProfile("testWeaponBuildRemove.json");
            Assert.IsFalse(AppData.Profile.WeaponBuilds.ContainsKey(expected));
        }

        [Test]
        public void WeaponBuildsRemoveSavesCorrectly()
        {
            LoadProfileAndPrepareWeaponBuilds();
            AppData.Profile.RemoveBuilds();
            SaveAndLoadProfile("testWeaponBuildsRemove.json");
            Assert.IsFalse(AppData.Profile.WeaponBuilds.Any());
        }

        [Test]
        public void WeaponBuildExportCorrectly()
        {
            LoadProfileAndPrepareWeaponBuilds();
            var expected = AppData.Profile.WeaponBuilds.FirstOrDefault();
            string testFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "testWeaponBuildExport.json");
            Profile.ExportBuild(expected.Value, testFile);
            WeaponBuild weaponBuild = JsonConvert.DeserializeObject<WeaponBuild>(File.ReadAllText(testFile));
            Assert.AreEqual(expected.Value.Name, weaponBuild.Name);
            Assert.AreEqual(expected.Value.Root, weaponBuild.Root);
            Assert.AreEqual(expected.Value.RecoilForceBack, weaponBuild.RecoilForceBack);
            Assert.AreEqual(expected.Value.RecoilForceUp, weaponBuild.RecoilForceUp);
            Assert.AreEqual(expected.Value.Ergonomics, weaponBuild.Ergonomics);
            Assert.AreEqual(expected.Value.Items.Length, weaponBuild.Items.Length);
        }

        [Test]
        public void WeaponBuildImportSavesCorrectly()
        {
            AppData.Profile.Load(TestHelpers.profileFile);
            AppData.Profile.ImportBuildFromFile(TestHelpers.weaponBuild);
            AppData.Profile.ImportBuildFromFile(TestHelpers.weaponBuild);
            SaveAndLoadProfile("testWeaponBuildsImport.json");
            Assert.IsTrue(AppData.Profile.WeaponBuilds.ContainsKey("TestBuild"));
            Assert.AreEqual(2, AppData.Profile.WeaponBuilds.Where(x => x.Value.Name.StartsWith("Test")).Count());
        }

        [Test]
        public void WeaponBuildCalculatingCorrectly()
        {
            AppData.Profile.Load(TestHelpers.profileFile);
            AppData.Profile.ImportBuildFromFile(TestHelpers.weaponBuild);
            var build = AppData.Profile.WeaponBuilds.Where(x => x.Key == "TestBuild").FirstOrDefault();
            Assert.NotNull(build);
            Assert.AreEqual(48.5, build.Value.Ergonomics);
            Assert.AreEqual(71, build.Value.RecoilForceUp);
            Assert.AreEqual(179, build.Value.RecoilForceBack);
        }

        [Test]
        public void WeaponBuildAddedToContainerSavesCorrectly()
        {
            AppData.Profile.Load(TestHelpers.profileFile);
            WeaponBuild weaponBuild = JsonConvert.DeserializeObject<WeaponBuild>(File.ReadAllText(TestHelpers.weaponBuild));
            List<string> iDs = new() { weaponBuild.Root };
            var weaponsCount = AppData.Profile.Characters.Pmc.Inventory.Items.Where(x => x.Tpl == weaponBuild.RootTpl).Count();
            iDs.AddRange(weaponBuild.BuildItems.Select(x => x.Id));
            weaponBuild.AddingQuantity = 2;
            weaponBuild.AddingFir = true;
            AppData.Profile.Characters.Pmc.Inventory.AddNewItemsToStash(weaponBuild);
            SaveAndLoadProfile("testStashAddingWeapons.json");
            Assert.AreEqual(weaponsCount + 2, AppData.Profile.Characters.Pmc.Inventory.Items.Where(x => x.Tpl == weaponBuild.RootTpl).Count());
            Assert.False(AppData.Profile.Characters.Pmc.Inventory.Items.Select(x => x.Id).Any(y => iDs.Contains(y)));
        }

        [Test]
        public void ProfileNotChangedAfterLoading()
        {
            DisableAutoAddDataInSettings();
            AppData.Profile.Load(TestHelpers.profileFile);
            Assert.IsFalse(AppData.Profile.IsProfileChanged());
        }

        [Test]
        public void ProfileChangedAfterEditings()
        {
            DisableAutoAddDataInSettings();
            AppData.Profile.Load(TestHelpers.profileFile);
            AppData.Profile.Characters.Pmc.SetAllTradersMax();
            AppData.Profile.Characters.Pmc.SetAllHideoutAreasMax();
            AppData.Profile.Characters.Pmc.SetAllQuests(QuestStatus.Fail);
            Assert.IsTrue(AppData.Profile.IsProfileChanged());
        }

        [Test]
        public void ProfileCanRemoveDuplicatedItems()
        {
            AppData.Profile.Load(TestHelpers.profileWithDuplicatedItems);
            Assert.IsTrue(AppData.Profile.Characters.Pmc.Inventory.InventoryHaveDuplicatedItems);
            AppData.Profile.Characters.Pmc.Inventory.RemoveDuplicatedItems();
            Assert.IsFalse(AppData.Profile.Characters.Pmc.Inventory.InventoryHaveDuplicatedItems);
            SaveAndLoadProfile("testRemoveDuplicatedItems.json");
            Assert.IsFalse(AppData.Profile.Characters.Pmc.Inventory.InventoryHaveDuplicatedItems);
        }

        [Test]
        public void ProfileHealthSavesCorrectly()
        {
            AppData.Profile.Load(TestHelpers.profileFile);
            AppData.Profile.Characters.Pmc.Health.Energy.Current = 300;
            AppData.Profile.Characters.Pmc.Health.Hydration.Current = 350;
            AppData.Profile.Characters.Pmc.Health.BodyParts.Head.Health.Current = 400;
            AppData.Profile.Characters.Pmc.Health.BodyParts.Chest.Health.Current = 450;
            AppData.Profile.Characters.Pmc.Health.BodyParts.Stomach.Health.Current = 500;
            AppData.Profile.Characters.Pmc.Health.BodyParts.LeftArm.Health.Current = 550;
            AppData.Profile.Characters.Pmc.Health.BodyParts.RightArm.Health.Current = 600;
            AppData.Profile.Characters.Pmc.Health.BodyParts.LeftLeg.Health.Current = 650;
            AppData.Profile.Characters.Pmc.Health.BodyParts.RightLeg.Health.Current = 700;
            SaveAndLoadProfile("testHealth.json");
            Assert.AreEqual(300, AppData.Profile.Characters.Pmc.Health.Energy.Current, "Health.Energy.Current is not 300");
            Assert.AreEqual(300, AppData.Profile.Characters.Pmc.Health.Energy.Maximum, "Health.Energy.Maximum is not 300");
            Assert.AreEqual(350, AppData.Profile.Characters.Pmc.Health.Hydration.Current, "Health.Hydration.Current is not 350");
            Assert.AreEqual(350, AppData.Profile.Characters.Pmc.Health.Hydration.Maximum, "Health.Hydration.Maximum is not 350");
            Assert.AreEqual(400, AppData.Profile.Characters.Pmc.Health.BodyParts.Head.Health.Current, "Health.BodyParts.Head.Health.Current is not 400");
            Assert.AreEqual(400, AppData.Profile.Characters.Pmc.Health.BodyParts.Head.Health.Maximum, "Health.BodyParts.Head.Health.Maximum is not 400");
            Assert.AreEqual(450, AppData.Profile.Characters.Pmc.Health.BodyParts.Chest.Health.Current, "Health.BodyParts.Chest.Health.Current is not 450");
            Assert.AreEqual(450, AppData.Profile.Characters.Pmc.Health.BodyParts.Chest.Health.Maximum, "Health.BodyParts.Chest.Health.Maximum is not 450");
            Assert.AreEqual(500, AppData.Profile.Characters.Pmc.Health.BodyParts.Stomach.Health.Current, "Health.BodyParts.Stomach.Health.Current is not 500");
            Assert.AreEqual(500, AppData.Profile.Characters.Pmc.Health.BodyParts.Stomach.Health.Maximum, "Health.BodyParts.Stomach.Health.Maximum is not 500");
            Assert.AreEqual(550, AppData.Profile.Characters.Pmc.Health.BodyParts.LeftArm.Health.Current, "Health.BodyParts.LeftArm.Health.Current is not 550");
            Assert.AreEqual(550, AppData.Profile.Characters.Pmc.Health.BodyParts.LeftArm.Health.Maximum, "Health.BodyParts.LeftArm.Health.Maximum is not 550");
            Assert.AreEqual(600, AppData.Profile.Characters.Pmc.Health.BodyParts.RightArm.Health.Current, "Health.BodyParts.RightArm.Health.Current is not 600");
            Assert.AreEqual(600, AppData.Profile.Characters.Pmc.Health.BodyParts.RightArm.Health.Maximum, "Health.BodyParts.RightArm.Health.Maximum is not 600");
            Assert.AreEqual(650, AppData.Profile.Characters.Pmc.Health.BodyParts.LeftLeg.Health.Current, "Health.BodyParts.LeftLeg.Health.Current is not 650");
            Assert.AreEqual(650, AppData.Profile.Characters.Pmc.Health.BodyParts.LeftLeg.Health.Maximum, "Health.BodyParts.LeftLeg.Health.Maximum is not 650");
            Assert.AreEqual(700, AppData.Profile.Characters.Pmc.Health.BodyParts.RightLeg.Health.Current, "Health.BodyParts.RightLeg.Health.Current is not 700");
            Assert.AreEqual(700, AppData.Profile.Characters.Pmc.Health.BodyParts.RightLeg.Health.Maximum, "Health.BodyParts.RightLeg.Health.Maximum is not 700");
        }

        private static void CheckCharacterMetric(CharacterMetric metric)
        {
            Assert.IsNotNull(metric);
            Assert.IsFalse(metric.Current == 0);
            Assert.IsFalse(metric.Maximum == 0);
        }

        private static void LoadProfileAndPrepareWeaponBuilds()
        {
            AppData.Profile.Load(TestHelpers.profileFile);
            if (AppData.Profile.WeaponBuilds.Count == 0)
                AppData.Profile.ImportBuildFromFile(TestHelpers.weaponBuild);
        }

        private static void CommonSkillsSavesCorrectly(Character character, string filename)
        {
            AppData.Profile.Load(TestHelpers.profileFile);
            character.SetAllCommonSkills(AppData.AppSettings.CommonSkillMaxValue);
            SaveAndLoadProfile(filename);
            Assert.IsFalse(character.Skills.Common
                .Where(x => x.Id.ToLower().StartsWith("bot"))
                .Any(x => x.Progress > 0));
            Assert.IsTrue(character.Skills.Common
                .All(x => x.Id.ToLower().StartsWith("bot") || x.Progress == AppData.AppSettings.CommonSkillMaxValue));
        }

        private static void PocketsSavesCorrectly(CharacterInventory inventory, string filename)
        {
            AppData.Profile.Load(TestHelpers.profileFile);
            string expected = AppData.ServerDatabase.Pockets.Last().Key;
            inventory.Pockets = expected;
            SaveAndLoadProfile(filename);
            Assert.IsTrue(inventory.Pockets == expected);
        }

        private static void RemovingItemsSavesCorrectly(List<string> itemIds, CharacterInventory inventory, string filename)
        {
            inventory.RemoveItems(itemIds);
            SaveAndLoadProfile(filename);
            foreach (var id in itemIds)
            {
                Assert.IsFalse(inventory.Items.Any(x => x.Id == id), "id not removed");
                Assert.IsFalse(inventory.Items.Any(x => x.ParentId == id), "id child items not removed");
            }
        }

        private static void RemovingAllEquipmentSavesCorrectly(CharacterInventory inventory, string filename)
        {
            AppData.Profile.Load(TestHelpers.profileFile);
            inventory.RemoveAllEquipment();
            SaveAndLoadProfile(filename);
            Assert.Null(inventory.FirstPrimaryWeapon);
            Assert.Null(inventory.Headwear);
            Assert.Null(inventory.TacticalVest);
            Assert.Null(inventory.SecuredContainer);
            Assert.Null(inventory.Backpack);
            Assert.Null(inventory.Earpiece);
            Assert.Null(inventory.FaceCover);
            Assert.Null(inventory.Eyewear);
            Assert.Null(inventory.ArmorVest);
            Assert.Null(inventory.SecondPrimaryWeapon);
            Assert.Null(inventory.Holster);
            Assert.Null(inventory.Scabbard);
            Assert.Null(inventory.ArmBand);
            Assert.AreEqual(0, inventory.PocketsItems.Count());
        }

        private static void DisableAutoAddDataInSettings()
        {
            AppData.AppSettings.AutoAddMissingQuests = false;
            AppData.AppSettings.AutoAddMissingMasterings = false;
            AppData.AppSettings.AutoAddMissingScavSkills = false;
        }

        private static void SaveAndLoadProfile(string filename)
        {
            string testFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, filename);
            AppData.Profile.Save(TestHelpers.profileFile, testFile);
            AppData.Profile.Load(testFile);
        }
    }
}