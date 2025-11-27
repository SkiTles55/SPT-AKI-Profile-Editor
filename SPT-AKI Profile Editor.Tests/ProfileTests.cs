using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using SPT_AKI_Profile_Editor.Core;
using SPT_AKI_Profile_Editor.Core.Enums;
using SPT_AKI_Profile_Editor.Core.Equipment;
using SPT_AKI_Profile_Editor.Core.ProfileClasses;
using SPT_AKI_Profile_Editor.Core.ServerClasses;
using SPT_AKI_Profile_Editor.Tests.Hepers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;

namespace SPT_AKI_Profile_Editor.Tests
{
    internal class ProfileTests
    {
        [OneTimeSetUp]
        public void Setup() => TestHelpers.LoadDatabaseAndProfile();

        [Test]
        public void ProfileHashNotZero() => Assert.That(AppData.Profile.ProfileHash, Is.Not.Zero);

        [Test]
        public void CharactersNotNull() => Assert.That(AppData.Profile.Characters, Is.Not.Null);

        [Test]
        public void ProfileNotEmpty() => Assert.That(AppData.Profile.IsProfileEmpty, Is.False, "Profile is empty");

        [Test]
        public void CustomisationUnlocksNotEmpty() => Assert.That(AppData.Profile.CustomisationUnlocks, Is.Not.Empty);

        [Test]
        public void PmcNotNull() => Assert.That(AppData.Profile.Characters.Pmc, Is.Not.Null);

        [Test]
        public void ScavNotNull() => Assert.That(AppData.Profile.Characters.Scav, Is.Not.Null);

        [Test]
        public void IdNotEmpty() => Assert.That(AppData.Profile.Characters.Pmc.Aid, Is.Not.Null, "Id is empty");

        [Test]
        public void InfoNotNull() => Assert.That(AppData.Profile.Characters.Pmc.Info, Is.Not.Null);

        [Test]
        public void NicknameNotEmpty()
            => Assert.That(AppData.Profile.Characters.Pmc.Info.Nickname, Is.Not.Null, "Nickname is empty");

        [Test]
        public void SideNotEmpty()
            => Assert.That(AppData.Profile.Characters.Pmc.Info.Side, Is.Not.Null, "Side is empty");

        [Test]
        public void VoiceNotEmpty()
            => Assert.That(AppData.Profile.Characters.Pmc.Customization.Voice, Is.Not.Null, "Voice is empty");

        [Test]
        public void PmcExperienceNotZero()
            => Assert.That(AppData.Profile.Characters.Pmc.Info.Experience, Is.Not.Zero, "Experience is zero");

        [Test]
        public void PmcLevelNotZero()
            => Assert.That(AppData.Profile.Characters.Pmc.Info.Level, Is.Not.Zero, "Level is zero");

        [Test]
        public void ScavExperienceNotZero()
            => Assert.That(AppData.Profile.Characters.Scav.Info.Experience, Is.Not.Zero, "Experience is zero");

        [Test]
        public void ScavLevelNotZero()
            => Assert.That(AppData.Profile.Characters.Scav.Info.Level, Is.Not.Zero, "Level is zero");

        [Test]
        public void GameVersionNotEmpty()
            => Assert.That(AppData.Profile.Characters.Pmc.Info.GameVersion, Is.Not.Null.Or.Empty, "GameVersion is empty");

        [Test]
        public void CustomizationNotNull() => Assert.That(AppData.Profile.Characters.Pmc.Customization, Is.Not.Null);

        [Test]
        public void HealthNotNull() => Assert.That(AppData.Profile.Characters.Pmc.Health, Is.Not.Null);

        [Test]
        public void HydrationLoadCorrectly() => CheckCharacterMetric(AppData.Profile.Characters.Pmc.Health.Hydration);

        [Test]
        public void EnergyLoadCorrectly() => CheckCharacterMetric(AppData.Profile.Characters.Pmc.Health.Energy);

        [Test]
        public void BodyPartsNotNull() => Assert.That(AppData.Profile.Characters.Pmc.Health.BodyParts, Is.Not.Null);

        [Test]
        public void BodyPartsHeadLoadCorrectly()
            => CheckCharacterMetric(AppData.Profile.Characters.Pmc.Health.BodyParts.Head?.Health);

        [Test]
        public void BodyPartsChestLoadCorrectly()
            => CheckCharacterMetric(AppData.Profile.Characters.Pmc.Health.BodyParts.Chest?.Health);

        [Test]
        public void BodyPartsStomachLoadCorrectly()
            => CheckCharacterMetric(AppData.Profile.Characters.Pmc.Health.BodyParts.Stomach?.Health);

        [Test]
        public void BodyPartsLeftArmLoadCorrectly()
            => CheckCharacterMetric(AppData.Profile.Characters.Pmc.Health.BodyParts.LeftArm?.Health);

        [Test]
        public void BodyPartsRightArmLoadCorrectly()
            => CheckCharacterMetric(AppData.Profile.Characters.Pmc.Health.BodyParts.RightArm?.Health);

        [Test]
        public void BodyPartsLeftLegLoadCorrectly()
            => CheckCharacterMetric(AppData.Profile.Characters.Pmc.Health.BodyParts.LeftLeg?.Health);

        [Test]
        public void BodyPartsRightLegLoadCorrectly()
            => CheckCharacterMetric(AppData.Profile.Characters.Pmc.Health.BodyParts.RightLeg?.Health);

        [Test]
        public void HeadNotEmpty()
            => Assert.That(AppData.Profile.Characters.Pmc.Customization.Head, Is.Not.Null, "Head is empty");

        [Test]
        public void TraderStandingsNotEmpty()
            => Assert.That(AppData.Profile.Characters.Pmc.TraderStandings, Is.Not.Empty);

        [Test]
        public void TraderStandingsNotNull()
            => Assert.That(AppData.Profile.Characters.Pmc.TraderStandings, Is.Not.Null, "TraderStandings is null");

        [Test]
        public void UnlockedInfoLoadsCorrectly()
        {
            Assert.That(AppData.Profile.Characters.Pmc.UnlockedInfo, Is.Not.Null, "UnlockedInfo is null");
            Assert.That(AppData.Profile.Characters.Pmc.UnlockedInfo.UnlockedProductionRecipe,
                        Is.Not.Null,
                        "UnlockedProductionRecipe is null");
            Assert.That(AppData.Profile.Characters.Pmc.UnlockedInfo.UnlockedProductionRecipe,
                        Is.Not.Empty,
                        "UnlockedProductionRecipe is empty");
        }

        [Test]
        public void RagfairInfoNotNull()
            => Assert.That(AppData.Profile.Characters.Pmc.RagfairInfo, Is.Not.Null, "RagfairInfo is null");

        [Test]
        public void QuestsLoadCorrectly()
        {
            Assert.That(AppData.Profile.Characters.Pmc.Quests, Is.Not.Null, "Quests is null");
            Assert.That(AppData.Profile.Characters.Pmc.Quests.Length != 0, Is.True, "Quests is empty");
            Assert.That(AppData.Profile.Characters.Pmc.Quests.Any(x => x.LocalizedTraderName == x.QuestTrader),
                        Is.False,
                        "Quests Localized TraderName's not loaded");
            var temp = AppData.Profile.Characters.Pmc.Quests.Where(x => x.LocalizedQuestName == x.QuestQid);
            Assert.That(AppData.Profile.Characters.Pmc.Quests.Any(x => x.LocalizedQuestName == x.Qid),
                        Is.False,
                        "Quests Localized QuestName's not loaded");
            Assert.That(AppData.Profile.Characters.Pmc.Quests.Any(x => x.LocalizedQuestType == AppData.AppLocalization.GetLocalizedString("tab_quests_unknown_group")),
                        Is.False,
                        "Quests Localized QuestType's not loaded");
        }

        [Test]
        public void RepeatableQuestsNotNull()
            => Assert.That(AppData.Profile.Characters.Pmc.RepeatableQuests,
                           Is.Not.Null,
                           "RepeatableQuests is null");

        [Test]
        public void RepeatableQuestsNotEmpty()
            => Assert.That(AppData.Profile.Characters.Pmc.RepeatableQuests,
                           Is.Not.Empty,
                           "RepeatableQuests is empty");

        [Test]
        public void RepeatableQuestsActiveQuestsNotEmpty()
            => Assert.That(AppData.Profile.Characters.Pmc.RepeatableQuests.Any(x => x.ActiveQuests.Length > 0),
                           Is.True,
                           "RepeatableQuests ActiveQuests is empty");

        [Test]
        public void RepeatableQuestsUnknownTypeIsEmpty()
            => Assert.That(AppData.Profile.Characters.Pmc.RepeatableQuests.Any(x => x.Type == QuestType.Unknown),
                           Is.False,
                           "RepeatableQuests Unknown type is not empty");

        [Test]
        public void RepeatableQuestsActiveQuestsWithActiveQuestTypeUnknownIsEmpty()
            => Assert.That(AppData.Profile.Characters.Pmc.RepeatableQuests.Any(x => x.ActiveQuests.Any(q => q.Type == ActiveQuestType.Unknown)),
                           Is.False,
                           "RepeatableQuests ActiveQuests with ActiveQuestType Unknown is not empty");

        [Test]
        public void EncyclopediaNotNull()
            => Assert.That(AppData.Profile.Characters.Pmc.Encyclopedia,
                           Is.Not.Null,
                           "Encyclopedia is null");

        [Test]
        public void EncyclopediaNotEmpty()
            => Assert.That(AppData.Profile.Characters.Pmc.Encyclopedia.Count == 0,
                           Is.False,
                           "Encyclopedia is empty");

        [Test]
        public void ExaminedItemsNotNull()
            => Assert.That(AppData.Profile.Characters.Pmc.ExaminedItems,
                           Is.Not.Null,
                           "ExaminedItems is null");

        [Test]
        public void ExaminedItemsNotEmpty()
            => Assert.That(AppData.Profile.Characters.Pmc.ExaminedItems.Any(),
                           Is.True,
                           "ExaminedItems is empty");

        [Test]
        public void ExaminedItemsLoadedCorrectly()
            => Assert.That(AppData.Profile.Characters.Pmc.ExaminedItems.Any(x => !string.IsNullOrEmpty(x.Id) && x.Id != x.Name && x.Name != null),
                           Is.True,
                           "ExaminedItems is not loaded correctly");

        [Test]
        public void HideoutNotNull()
            => Assert.That(AppData.Profile.Characters.Pmc.Hideout, Is.Not.Null, "Hideout is null");

        [Test]
        public void HideoutAreasNotEmpty()
            => Assert.That(AppData.Profile.Characters.Pmc.Hideout.Areas.Length == 0,
                           Is.False,
                           "HideoutAreas is empty");

        [Test]
        public void HideoutAreasHasCorrectLocalizedName()
            => Assert.That(AppData.Profile.Characters.Pmc.Hideout.Areas.Any(x => x.LocalizedName == $"hideout_area_{x.Type}_name"),
                           Is.False);

        [Test]
        public void HideoutProductionsLoadsCorrectly()
        {
            Assert.That(AppData.Profile.Characters.Pmc.HideoutProductions,
                        Is.Not.Null,
                        "HideoutProductions is null");
            Assert.That(AppData.Profile.Characters.Pmc.HideoutProductions,
                        Is.Not.Empty,
                        "HideoutProductions is empty");
            Assert.That(AppData.Profile.Characters.Pmc.HideoutProductions.Any(x => x.ProductItem.Name == x.Production.EndProduct),
                        Is.False,
                        "HideoutProductions has item with bad ProductItem.Name");
            Assert.That(AppData.Profile.Characters.Pmc.HideoutProductions.Any(x => x.AreaLocalizedName == $"hideout_area_{x.Production.AreaType}_name"),
                        Is.False,
                        "HideoutProductions has item with bad AreaLocalizedName");
        }

        [Test]
        public void HideoutStartedProductionsNotEmpty()
            => Assert.That(AppData.Profile.Characters.Pmc.Hideout.Production.Any,
                           Is.True,
                           "HideoutProductions is empty");

        [Test]
        public void PmcSkillsNotNull()
            => Assert.That(AppData.Profile.Characters.Pmc.Skills, Is.Not.Null, "Pmc skills is null");

        [Test]
        public void PmcCommonSkillsNotEmpty()
            => Assert.That(AppData.Profile.Characters.Pmc.Skills.IsCommonSkillsEmpty,
                           Is.False,
                           "Pmc CommonSkills is empty");

        [Test]
        public void PmcCommonSkillsHaveLocalizedNames()
            => Assert.That(AppData.Profile.Characters.Pmc.Skills.Common.Any(x => string.IsNullOrEmpty(x.LocalizedName)),
                           Is.False,
                           "Not all Pmc CommonSkills have localized name");

        [Test]
        public void PmcMasteringSkillsNotEmpty()
            => Assert.That(AppData.Profile.Characters.Pmc.Skills.IsMasteringsEmpty,
                           Is.False,
                           "Pmc MasteringSkills is empty");

        [Test]
        public void PmcMasteringSkillsHaveLocalizedNames()
            => Assert.That(AppData.Profile.Characters.Pmc.Skills.Mastering.Any(x => string.IsNullOrEmpty(x.LocalizedName)),
                           Is.False,
                           "Not all Pmc Masterings have localized name");

        [Test]
        public void ScavSkillsNotNull()
            => Assert.That(AppData.Profile.Characters.Scav.Skills,
                           Is.Not.Null,
                           "Scav skills is null");

        [Test]
        public void ScavCommonSkillsNotEmpty()
            => Assert.That(AppData.Profile.Characters.Scav.Skills.IsCommonSkillsEmpty,
                           Is.False,
                           "Scav CommonSkills is empty");

        [Test]
        public void ScavCommonSkillsHaveLocalizedNames()
            => Assert.That(AppData.Profile.Characters.Scav.Skills.Common.Any(x => string.IsNullOrEmpty(x.LocalizedName)),
                           Is.False,
                           "Not all Scav CommonSkills have localized name");

        [Test]
        public void ScavMasteringSkillsNotEmpty()
            => Assert.That(AppData.Profile.Characters.Scav.Skills.IsMasteringsEmpty,
                           Is.False,
                           "Scav MasteringSkills is empty");

        [Test]
        public void ScavMasteringSkillsHaveLocalizedNames()
            => Assert.That(AppData.Profile.Characters.Scav.Skills.Mastering.Any(x => string.IsNullOrEmpty(x.LocalizedName)),
                           Is.False,
                           "Not all Scav Masterings have localized name");

        [Test]
        public void InventoryNotNull()
            => Assert.That(AppData.Profile.Characters.Pmc.Inventory, Is.Not.Null);

        [Test]
        public void InventoryStashNotEmpty()
            => Assert.That(AppData.Profile.Characters.Pmc.Inventory.Stash, Is.Not.Empty);

        [Test]
        public void PmcInventoryHasItems() =>
            Assert.That(AppData.Profile.Characters.Pmc.Inventory.HasItems, Is.True);

        [Test]
        public void PmcInventoryHasPockets()
            => Assert.That(AppData.Profile.Characters.Pmc.Inventory.Items.Any(x => x.IsPockets),
                           Is.True);

        [Test]
        public void ScavInventoryNotHasItems()
            => Assert.That(AppData.Profile.Characters.Scav.Inventory.HasItems, Is.False);

        [Test]
        public void ScavInventoryHasPockets()
            => Assert.That(AppData.Profile.Characters.Scav.Inventory.Items.Any(x => x.IsPockets),
                           Is.True);

        [Test]
        public void PmcInventoryNotContainsModdedItems()
            => Assert.That(AppData.Profile.Characters.Pmc.Inventory.ContainsModdedItems, Is.False);

        [Test]
        public void PmcInventoryContainsItemsWithIcons()
            => Assert.That(AppData.Profile.Characters.Pmc.Inventory.InventoryItems.Any(x => x.CategoryIcon != null),
                           Is.True);

        [Test]
        public void PmcInventoryContainsItemsWithTag()
        {
            Assert.That(AppData.Profile.Characters.Pmc.Inventory.InventoryItems.Any(x => !string.IsNullOrEmpty(x.Upd?.Tag?.Name)),
                        Is.True);
            Assert.That(AppData.Profile.Characters.Pmc.Inventory.InventoryItems.Any(x => !string.IsNullOrEmpty(x.Tag)),
                        Is.True);
        }

        [Test]
        public void PmcInventoryContainsItemsWithCountString()
            => Assert.That(AppData.Profile.Characters.Pmc.Inventory.InventoryItems.Any(x => !string.IsNullOrEmpty(x.CountString)),
                           Is.True);

        [Test]
        public void PmcInventoryItemsHaveCorrectLocalizedNames()
            => Assert.That(AppData.Profile.Characters.Pmc.Inventory.InventoryItems.Any(x => x.LocalizedName == x.Tpl),
                           Is.False);

        [Test]
        public void ScavInventoryNotContainsModdedItems()
            => Assert.That(AppData.Profile.Characters.Scav.Inventory.ContainsModdedItems, Is.False);

        [Test]
        public void PmcPocketsHasItems()
            => Assert.That(AppData.Profile.Characters.Pmc.Inventory.PocketsHasItems, Is.True);

        [Test]
        public void ScavPocketsHasItems()
            => Assert.That(AppData.Profile.Characters.Scav.Inventory.PocketsHasItems, Is.True);

        [Test]
        public void PmcInventoryHasEquipment()
            => Assert.That(AppData.Profile.Characters.Pmc.Inventory.HasEquipment, Is.True);

        [Test]
        public void ScavInventoryHasEquipment()
            => Assert.That(AppData.Profile.Characters.Scav.Inventory.HasEquipment, Is.True);

        [Test]
        public void PmcInventoryHaveDollarsCountString()
            => Assert.That(string.IsNullOrEmpty(AppData.Profile.Characters.Pmc.Inventory.DollarsCount), Is.False);

        [Test]
        public void PmcInventoryHaveRoublesCountString()
            => Assert.That(string.IsNullOrEmpty(AppData.Profile.Characters.Pmc.Inventory.RublesCount), Is.False);

        [Test]
        public void PmcInventoryHaveEurosCountString()
            => Assert.That(string.IsNullOrEmpty(AppData.Profile.Characters.Pmc.Inventory.EurosCount), Is.False);

        [Test]
        public void InventoryEquipmentNotEmpty()
            => Assert.That(AppData.Profile.Characters.Pmc.Inventory.Equipment, Is.Not.Empty);

        [Test]
        public void PmcEquipmentNotNullAndEmpty()
        {
            Assert.That(AppData.Profile.Characters.Pmc.Inventory.EquipmentSlots,
                        Is.Not.Null,
                        "Pmc equipment is null");
            Assert.That(AppData.Profile.Characters.Pmc.Inventory.EquipmentSlots.SelectMany(x => x.ItemsList).Any(),
                        Is.True,
                        "Pmc equipment is true");
        }

        [Test]
        public void InventoryItemsNotEmpty()
            => Assert.That(AppData.Profile.Characters.Pmc.Inventory.Items, Is.Not.Empty);

        [Test]
        public void PmcPocketsNotNull()
            => Assert.That(AppData.Profile.Characters.Pmc.Inventory.Pockets, Is.Not.Empty);

        [Test]
        public void WeaponBuildsNotNull()
            => Assert.That(AppData.Profile.UserBuilds.WeaponBuilds, Is.Not.Null);

        [Test]
        public void WeaponBuildsNotEmpty()
        {
            AppData.Profile.Load(TestHelpers.profileFile);
            CheckBuilds(AppData.Profile.UserBuilds.WeaponBuilds, AppData.Profile.UserBuilds.WBuilds);
        }

        [Test]
        public void EquipmentBuildsNotNull()
            => Assert.That(AppData.Profile.UserBuilds.EquipmentBuilds, Is.Not.Null);

        [Test]
        public void EquipmentBuildsNotEmpty()
        {
            AppData.Profile.Load(TestHelpers.profileFile);
            CheckBuilds(AppData.Profile.UserBuilds.EquipmentBuilds, AppData.Profile.UserBuilds.EBuilds);
        }

        [Test]
        public void PMCStashContainsVerticalItems()
            => Assert.That(AppData.Profile.Characters.Pmc.Inventory.Items.Any(x => x.Location?.R == ItemRotation.Vertical),
                           Is.True);

        [Test]
        public void PMCStashContainsContainers()
            => Assert.That(AppData.Profile.Characters.Pmc.Inventory.Items.Any(x => x.IsContainer), Is.True);

        [Test]
        public void PMCStashContainsWeapons()
            => Assert.That(AppData.Profile.Characters.Pmc.Inventory.Items.Any(x => x.IsWeapon), Is.True);

        [Test]
        public void ScavStashContainsContainers()
            => Assert.That(AppData.Profile.Characters.Scav.Inventory.Items.Any(x => x.IsContainer), Is.True);

        [Test]
        public void ScavStashContainsWeapons()
            => Assert.That(AppData.Profile.Characters.Scav.Inventory.Items.Any(x => x.IsWeapon), Is.True);

        [Test]
        public void AllAchievementsLoadsCorrectly()
        {
            Assert.That(AppData.Profile.Characters.Pmc.AllAchievements,
                        Is.Not.Null,
                        "AllAchievements is null");
            Assert.That(AppData.Profile.Characters.Pmc.AllAchievements,
                        Is.Not.Empty,
                        "AllAchievements is empty");
            Assert.That(AppData.Profile.Characters.Pmc.AllAchievements.Any(x => x.BitmapImage != null),
                        Is.True,
                        "AllAchievements doesnt have images");
            Assert.That(AppData.Profile.Characters.Pmc.AllAchievements.Any(x => x.Timestamp == 0),
                        Is.False,
                        "AllAchievements has item with bad Timestamp");
            Assert.That(AppData.Profile.Characters.Pmc.AllAchievements.Any(x => x.LocalizedName == x.Id),
                        Is.False,
                        "AllAchievements has item with bad LocalizedName");
            Assert.That(AppData.Profile.Characters.Pmc.AllAchievements.Any(x => x.LocalizedDescription == ""),
                        Is.False,
                        "AllAchievements has item with bad LocalizedDescription");
            Assert.That(AppData.Profile.Characters.Pmc.AllAchievements.Any(x => string.IsNullOrEmpty(x.Rarity)),
                        Is.False,
                        "AllAchievements has item with bad Rarity");
        }

        [Test]
        public void ProfileSavesCorrectly()
        {
            AppData.AppSettings.AutoAddMissingMasterings = false;
            AppData.AppSettings.AutoAddMissingScavSkills = false;
            AppData.Profile.Load(TestHelpers.profileFile);
            var expected = JsonConvert.DeserializeObject(File.ReadAllText(TestHelpers.profileFile));
            string testFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "test.json");
            AppData.Profile.Save(TestHelpers.profileFile, testFile);
            var result = JsonConvert.DeserializeObject(File.ReadAllText(testFile));
            Assert.That(result.ToString(), Is.EqualTo(expected.ToString()));
        }

        [Test]
        public void TraderSalesSumAndStandingCanIncreaseLevel()
        {
            static bool CanBeUsedForTest(CharacterTraderStandingExtended x)
                => x.LoyaltyLevel < 2
                && x.TraderBase.LoyaltyLevels.Count > x.LoyaltyLevel;

            AppData.Profile.Load(TestHelpers.profileFile);
            var firstTrader = AppData.Profile.Characters.Pmc.TraderStandingsExt.FirstOrDefault(x => CanBeUsedForTest(x));
            Assert.That(firstTrader, Is.Not.Null, "Trader for test not found");
            var currentLevel = firstTrader.LoyaltyLevel;
            firstTrader.SalesSum = firstTrader.TraderBase.LoyaltyLevels[currentLevel].MinSalesSum;
            firstTrader.Standing = firstTrader.TraderBase.LoyaltyLevels[currentLevel].MinStanding;
            Assert.That(firstTrader.LoyaltyLevel, Is.EqualTo(currentLevel + 1));
        }

        [Test]
        public void TradersLoadedCorrectly()
        {
            AppData.Profile.Load(TestHelpers.profileFile);
            Assert.That(AppData.Profile.Characters.Pmc.TraderStandingsExt.Any(x => string.IsNullOrEmpty(x.Id)),
                        Is.False,
                        "Traders id's not loaded");
            Assert.That(AppData.Profile.Characters.Pmc.TraderStandingsExt.Any(x => x.TraderStanding == null),
                        Is.False,
                        "Traders TraderStanding's not loaded");
            Assert.That(AppData.Profile.Characters.Pmc.TraderStandingsExt.Any(x => x.TraderBase == null),
                        Is.False,
                        "Traders TraderBase's not loaded");
            Assert.That(AppData.Profile.Characters.Pmc.TraderStandingsExt.Any(x => string.IsNullOrEmpty(x.TraderBase.Id)),
                        Is.False,
                        "Traders TraderBase id's not loaded");
            Assert.That(AppData.Profile.Characters.Pmc.TraderStandingsExt.Any(x => x.TraderBase.LoyaltyLevels.Count == 0),
                        Is.False,
                        "Traders TraderBase LoyaltyLevels's not loaded");
            Assert.That(AppData.Profile.Characters.Pmc.TraderStandingsExt.Any(x => x.BitmapImage == null),
                        Is.False,
                        "Traders BitmapImage's not loaded");
            Assert.That(AppData.Profile.Characters.Pmc.TraderStandingsExt.Any(x => x.LocalizedName == x.Id),
                        Is.False,
                        "Traders LocalizedName's not loaded");
            Assert.That(AppData.Profile.Characters.Pmc.TraderStandingsExt.Any(x => x.SalesSum != x.TraderStanding.SalesSum),
                        Is.False,
                        "Traders SalesSum's not loaded correctly");
        }

        [Test]
        public void TradersSavesCorrectly()
        {
            AppData.Profile.Load(TestHelpers.profileFile);
            AppData.Profile.Characters.Pmc.SetAllTradersMax();
            TestHelpers.SaveAndLoadProfile("testTraders.json");
            Assert.That(AppData.Profile.Characters.Pmc.TraderStandingsExt.All(x => x.LoyaltyLevel == x.MaxLevel),
                        Is.True,
                        "TraderStandingsExt not in max levels");
        }

        [Test]
        public void RagfairStandingSavesCorrectly()
        {
            AppData.Profile.Load(TestHelpers.profileFile);
            var startingRating = AppData.Profile.Characters.Pmc.RagfairInfo.Rating;
            AppData.Profile.Characters.Pmc.RagfairInfo.Rating += 3;
            TestHelpers.SaveAndLoadProfile("testRagfairStanding.json");
            Assert.That(AppData.Profile.Characters.Pmc.RagfairInfo.Rating,
                        Is.EqualTo(startingRating + 3),
                        "Ragfair standing not saves correctly");
        }

        [Test]
        public void ChangeFenceStandingAffectScavCharacter()
        {
            AppData.Profile.Load(TestHelpers.profileFile);
            var fencePmcTrader = AppData.Profile.Characters.Pmc.TraderStandingsExt.FirstOrDefault(x => x.Id == AppData.AppSettings.FenceTraderId);
            Assert.That(fencePmcTrader, Is.Not.Null, "Fence trader for PMC is null");
            var newValue = 3.2f;
            fencePmcTrader.Standing = newValue;
            TestHelpers.SaveAndLoadProfile("testTradersFenceStanding.json");
            var fenceScavTrader = AppData.Profile.Characters.Scav.TraderStandingsExt.FirstOrDefault(x => x.Id == AppData.AppSettings.FenceTraderId);
            Assert.That(fenceScavTrader, Is.Not.Null, "Fence trader for Scav is null");
            Assert.That(fenceScavTrader.Standing, Is.EqualTo(newValue), $"Fence trader scanding for Scav not equal to {newValue}");
        }

        [Test]
        public void QuestsAddingWorksCorrectly()
        {
            JObject profileJObject = JObject.Parse(File.ReadAllText(TestHelpers.profileFile));
            var testProfilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "testProfileWithEmptyQuests.json");
            profileJObject.SelectToken("characters")["pmc"].SelectToken("Quests").Replace(JToken.FromObject(Array.Empty<CharacterQuest>()));
            string json = JsonConvert.SerializeObject(profileJObject, TestHelpers.seriSettings);
            File.WriteAllText(testProfilePath, json);

            LoadProfileAndCheckQuestsCount(testProfilePath, true);
            LoadProfileAndCheckQuestsCount(testProfilePath, false);
        }

        [Test]
        public void QuestsStatusesSavesCorrectly()
        {
            AppData.Profile.Load(TestHelpers.profileFile);
            AppData.Profile.Characters.Pmc.AddAllMisingQuests(false);
            AppData.Profile.Characters.Pmc.AddAllMisingQuests(true);
            AppData.Profile.Characters.Pmc.SetAllQuests(QuestStatus.Fail);
            TestHelpers.SaveAndLoadProfile("testQuests.json");
            bool allQuestsLoadedAndInFail = AppData.Profile.Characters.Pmc.Quests.All(x => x.Status == QuestStatus.Fail)
                && AppData.Profile.Characters.Pmc.Quests.All(x => x.StatusTimers.ContainsKey(QuestStatus.Fail))
                && AppData.Profile.Characters.Pmc.Quests.Where(x => x.Type == QuestType.Standart).Count() >= AppData.ServerDatabase.QuestsData.Count;
            Assert.That(allQuestsLoadedAndInFail, Is.True);
        }

        [Test]
        public void QuestStatusCanAddAndRemoveCraft()
        {
            AppData.Profile.Load(TestHelpers.profileFile);
            AppData.Profile.Characters.Pmc.AddAllMisingQuests(false);
            var production = AppData.Profile.Characters.Pmc.HideoutProductions.FirstOrDefault(x => !x.Added);
            Assert.That(production, Is.Not.Null, "Unable to find production for adding");
            var quest = AppData.Profile.Characters.Pmc.Quests.FirstOrDefault(x => production.Production.Requirements.FirstOrDefault(r => r.QuestId == x.QuestQid) != null);
            Assert.That(quest, Is.Not.Null, "Unable to find quest for production");
            quest.Status = QuestStatus.Success;
            TestHelpers.SaveAndLoadProfile("testQuestAddCraft.json");
            Assert.That(AppData.Profile.Characters.Pmc.HideoutProductions.FirstOrDefault(x => x.Production.Id == production.Production.Id).Added,
                        Is.True,
                        "Craft not added");
            AppData.Profile.Characters.Pmc.Quests.FirstOrDefault(x => x.QuestQid == quest.Qid).Status = QuestStatus.AvailableForFinish;
            TestHelpers.SaveAndLoadProfile("testQuestRemoveCraft.json");
            Assert.That(AppData.Profile.Characters.Pmc.HideoutProductions.FirstOrDefault(x => x.Production.Id == production.Production.Id).Added,
                        Is.False,
                        "Craft not removed");
        }

        [Test]
        public void QuestsFirstLockedStatusSavesCorrectly()
        {
            AppData.Profile.Load(TestHelpers.profileFile);
            AppData.Profile.Characters.Pmc.AddAllMisingQuests(false);
            var locked = AppData.Profile.Characters.Pmc.Quests?
                .Where(x => x.Type == QuestType.Standart && x.Status == QuestStatus.Locked)?
                .FirstOrDefault();
            Assert.That(locked, Is.Not.Null, "Cant find locked quest");
            locked.Status = QuestStatus.AvailableForFinish;
            TestHelpers.SaveAndLoadProfile("testQuests.json");
            Assert.That(AppData.Profile.Characters.Pmc.Quests?
                .Where(x => x.Qid == locked.Qid)?
                .First().Status == QuestStatus.AvailableForFinish, Is.True);
        }

        [Test]
        public void HideoutAreaLevelsSavesCorrectly()
        {
            AppData.Profile.Load(TestHelpers.profileFile);
            AppData.Profile.Characters.Pmc.SetAllHideoutAreasMax();
            TestHelpers.SaveAndLoadProfile("testHideouts.json");
            Assert.That(AppData.Profile.Characters.Pmc.Hideout.Areas
                .All(x => x.Level == x.MaxLevel), Is.True);
            var inventory = AppData.Profile.Characters.Pmc.Inventory;
            Assert.That(inventory.HideoutAreaStashes.Count,
                        Is.GreaterThan(1),
                        "Stashes for hideout areas not writed");
            foreach (var hideoutAreaStash in inventory.HideoutAreaStashes)
            {
                var item = inventory.Items.FirstOrDefault(x => x.Id == hideoutAreaStash.Value);
                Assert.That(item, Is.Not.Null, "Inventory not contains stash for hideout area");
            }

            void CheckItemsAmount(string tpl, string failMessage)
            {
                var items = inventory.Items.Where(x => x.Tpl == tpl);
                Assert.That(items.Count(), Is.GreaterThan(1), failMessage);
            }

            CheckItemsAmount(AppData.AppSettings.MannequinInventoryTpl, "Mannequins not added");
            CheckItemsAmount(inventory.Pockets, "Pockets for mannequins not added");
        }

        [Test]
        public void HideoutCraftsSavesCorrectly()
        {
            AppData.Profile.Load(TestHelpers.profileFile);
            AppData.Profile.Characters.Pmc.AddAllCrafts();
            TestHelpers.SaveAndLoadProfile("testCrafts.json");
            Assert.That(AppData.Profile.Characters.Pmc.HideoutProductions.All(x => x.Added), Is.True);
        }

        [Test]
        public void HideoutAreaLevelCantBeSettedGreatherThanMax()
        {
            AppData.Profile.Load(TestHelpers.profileFile);
            var firstArea = AppData.Profile.Characters.Pmc.Hideout.Areas.First();
            var max = firstArea.MaxLevel;
            firstArea.Level = max + 3;
            Assert.That(firstArea.Level, Is.EqualTo(max));
        }

        [Test]
        public void HideoutFinishedCraftSavesCorrectly()
        {
            AppData.Profile.Load(TestHelpers.profileFile);
            var craft = AppData.Profile.Characters.Pmc.Hideout.Production?.Values.FirstOrDefault(x => !x.IsFinished);
            Assert.That(craft, Is.Not.Null, "Started craft not founded");
            craft.SetFinished();
            TestHelpers.SaveAndLoadProfile("testSaveFinishedCraft.json");
            Assert.That(AppData.Profile.Characters.Pmc.Hideout.Production?.Values.FirstOrDefault(x => x.RecipeId == craft.RecipeId)?.IsFinished, Is.True, "Craft not finished");
        }

        [Test]
        public void HideoutFinishedAllCraftsSavesCorrectly()
        {
            AppData.Profile.Load(TestHelpers.profileFile);
            AppData.Profile.Characters.Pmc.Hideout?.SetAllCraftsFinished();
            TestHelpers.SaveAndLoadProfile("testSaveFinishedAllCrafts.json");
            Assert.That(AppData.Profile.Characters.Pmc.Hideout.Production?.Values.Where(x => !x.IsFinished).Any(), Is.False, "Crafts not finished");
        }

        [Test]
        public void HideoutRemovedCraftSavesCorrectly()
        {
            AppData.Profile.Load(TestHelpers.profileFile);
            var craft = AppData.Profile.Characters.Pmc.Hideout.Production?.Values.FirstOrDefault();
            Assert.That(craft, Is.Not.Null, "Started craft not founded");
            AppData.Profile.Characters.Pmc.Hideout.RemoveCraft(craft.RecipeId);
            TestHelpers.SaveAndLoadProfile("testSaveRemovedCraft.json");
            Assert.That(AppData.Profile.Characters.Pmc.Hideout.Production?.Values.FirstOrDefault(x => x.RecipeId == craft.RecipeId), Is.Null, "Craft not removed");
        }

        [Test]
        public void HideoutRemoveAllCraftsSavesCorrectly()
        {
            AppData.Profile.Load(TestHelpers.profileFile);
            AppData.Profile.Characters.Pmc.Hideout.RemoveAllCrafts();
            TestHelpers.SaveAndLoadProfile("testSaveRemovedAllCrafts.json");
            Assert.That(AppData.Profile.Characters.Pmc.Hideout.Production?.Count, Is.Not.Null.And.Zero, "Crafts not removed");
        }

        [Test]
        public void PmcCommonSkillsSavesCorrectly()
            => CommonSkillsSavesCorrectly(AppData.Profile.Characters.Pmc, "testPmcCommonSkills.json");

        [Test]
        public void ScavCommonSkillsSavesCorrectly()
        {
            AppData.AppSettings.AutoAddMissingScavSkills = true;
            CommonSkillsSavesCorrectly(AppData.Profile.Characters.Scav, "testScavCommonSkills.json");
            Assert.That(AppData.Profile.Characters.Scav.Skills.Common.Length >= AppData.Profile.Characters.Pmc.Skills.Common.Length,
                        Is.True);
        }

        [Test]
        public void PmcMasteringSkillsSavesCorrectly()
        {
            LoadProfileWithAllMasterings();
            MasteringSkillsSavesCorrectly(AppData.Profile.Characters.Pmc);
        }

        [Test]
        public void ScavMasteringSkillsSavesCorrectly()
        {
            LoadProfileWithAllMasterings();
            MasteringSkillsSavesCorrectly(AppData.Profile.Characters.Scav);
        }

        [Test]
        public void ExaminedItemsSavesCorrectly()
        {
            AppData.Profile.Load(TestHelpers.profileFile);
            AppData.Profile.Characters.Pmc.Encyclopedia = [];
            var expected = AppData.Profile.Characters.Pmc.ExaminedItems.Count();
            AppData.Profile.Characters.Pmc.ExamineAll();
            TestHelpers.SaveAndLoadProfile("testExaminedItems.json");
            Assert.That(AppData.Profile.Characters.Pmc.ExaminedItems.Count(), Is.Not.EqualTo(expected));
        }

        [Test]
        public void SuitsSavesCorrectly()
        {
            AppData.Profile.Load(TestHelpers.profileFile);
            AppData.ServerDatabase.AcquireAllClothing();
            string savePath = "testSuits.json";
            TestHelpers.SaveAndLoadProfile(savePath);
            Assert.That(AppData.Profile.CustomisationUnlocks.Where(x => x.IsSuitUnlock).Count, Is.GreaterThanOrEqualTo(AppData.ServerDatabase.TraderSuits.Count));
        }

        [Test]
        public void PmcPocketsSavesCorrectly()
            => PocketsSavesCorrectly(AppData.Profile.Characters.Pmc.Inventory, "testPmcPockets.json");

        [Test]
        public void ScavPocketsSavesCorrectly()
            => PocketsSavesCorrectly(AppData.Profile.Characters.Scav.Inventory, "testScavPockets.json");

        [Test]
        public void PmsStashCanGetInnerItems()
        {
            AppData.Profile.Load(TestHelpers.profileFile);
            var weaponId = AppData.Profile.Characters.Pmc.Inventory.InventoryItems.First(x => x.IsWeapon)?.Id;
            Assert.That(string.IsNullOrEmpty(weaponId), Is.False, "Weapon not found in pmc stash");
            var innerItems = AppData.Profile.Characters.Pmc.Inventory.GetInnerItems(weaponId);
            Assert.That(innerItems, Is.Not.Empty, "Inner items of weapon empty");
        }

        [Test]
        public void PmcStashRemovingItemsSavesCorrectly()
        {
            AppData.Profile.Load(TestHelpers.profileFile);
            string expectedId1 = AppData.Profile.Characters.Pmc.Inventory.InventoryItems.First().Id;
            string expectedId2 = AppData.Profile.Characters.Pmc.Inventory.InventoryItems
                .Where(x => x.Id != expectedId1 && x.ParentId != expectedId1)
                .First()
                .Id;
            RemovingItemsSavesCorrectly([expectedId1, expectedId2],
                                        AppData.Profile.Characters.Pmc.Inventory,
                                        "testStashRemovingItems.json");
        }

        [Test]
        public void PmcStashBonusesCountAddingSavesCorrectly()
        {
            AppData.Profile.Load(TestHelpers.profileFile);
            var expectedValue = AppData.Profile.Characters.Pmc.StashRowsBonusCount + 6;
            SaveAndCheckStashBonuses(expectedValue, "testStashBonusesCountAdding.json");
        }

        [Test]
        public void PmcStashBonusesCountRemovingSavesCorrectly()
        {
            AppData.Profile.Load(TestHelpers.profileFile);
            var expectedValue = 0;
            SaveAndCheckStashBonuses(expectedValue, "testStashBonusesCountRemoving.json");
        }

        [Test]
        public void ScavStashRemovingItemsSavesCorrectly()
        {
            AppData.Profile.Load(TestHelpers.profileFile);
            var expectedIds = AppData.Profile.Characters.Scav.Inventory.EquipmentSlots
                .Where(x => x is EquipmentSlotItem && x.ItemsList.Count != 0)
                .Take(2)
                .SelectMany(x => x.ItemsList)
                .Where(x => x != null)
                .Select(x => x.Id)
                .ToList();
            RemovingItemsSavesCorrectly(expectedIds,
                                        AppData.Profile.Characters.Scav.Inventory,
                                        "testScavStashRemovingItems.json");
        }

        [Test]
        public void PmcStashRemovingAllItemsSavesCorrectly()
        {
            AppData.Profile.Load(TestHelpers.profileFile);
            AppData.Profile.Characters.Pmc.Inventory.RemoveAllItems();
            TestHelpers.SaveAndLoadProfile("testStashRemovingAllItems.json");
            Assert.That(AppData.Profile.Characters.Pmc.Inventory.InventoryItems, Is.Empty);
        }

        [Test]
        public void PmcStashRemovingAllItemsRunsCorrectly()
        {
            AppData.Profile.Load(TestHelpers.profileFile);
            var ids = AppData.Profile.Characters.Pmc.Inventory.Items.Select(x => x.Id);
            AppData.Profile.Characters.Pmc.Inventory.RemoveAllItems();
            var missedItems = AppData.Profile.Characters.Pmc.Inventory.Items.Where(x => x.ParentId != null && !ids.Contains(x.ParentId));
            Assert.That(missedItems, Is.Empty);
        }

        [Test]
        public void PmcStashRemovingAllEquipmentSavesCorrectly()
        {
            RemovingAllEquipmentSavesCorrectly(AppData.Profile.Characters.Pmc.Inventory, "testStashRemovingAllEquipment.json");
            Assert.That(AppData.Profile.Characters.Pmc.Inventory.InventoryItems.Any(), Is.True);
        }

        [Test]
        public void ScavStashRemovingAllEquipmentSavesCorrectly()
            => RemovingAllEquipmentSavesCorrectly(AppData.Profile.Characters.Scav.Inventory,
                                                  "testScavStashRemovingAllEquipment.json");

        [Test]
        public void Stash2DMapCalculatingCorrectly()
        {
            AppData.Profile.Load(TestHelpers.profileFile);
            InventoryItem ProfileStash = AppData.Profile.Characters.Pmc.Inventory.Items
                .Where(x => x.Id == AppData.Profile.Characters.Pmc.Inventory.Stash)
                .FirstOrDefault();
            var stash2d = AppData.Profile.Characters.Pmc.Inventory.GetSlotsMap(ProfileStash);
            Assert.That(stash2d, Is.Not.EqualTo(new int[0, 0]));
            Assert.That(stash2d.Cast<int>().All(x => x == 0), Is.False);
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
            var savedItems = AddAndGetItemsToProfileStash([item2, item1],
                                                          "testStashAddingItems.json",
                                                          AppData.Profile.Characters.Pmc.Inventory.Stash);
            Assert.That(savedItems[0].Upd.SpawnedInSession, Is.True);
            Assert.That(savedItems[1].Upd.SpawnedInSession, Is.True);
            Assert.That(savedItems[2].Upd.SpawnedInSession, Is.False);
            Assert.That(AppData.Profile.Characters.Pmc.Inventory.InventoryItems
                .Where(x => AppData.Profile.Characters.Pmc.Inventory.InventoryItems
                .Any(y => y.Id != x.Id && y.Location.X == x.Location.X && y.Location.Y == x.Location.Y))
                .Any(), Is.False);
        }

        [Test]
        public void StashAddingItemsToQuestRaidItemsSavesCorrectly()
            => TestAddingItemsToQuestStash(StashType.QuestRaidItems,
                                           "testQuestRaidItemsAddingItems.json",
                                           AppData.Profile.Characters.Pmc.Inventory.QuestRaidItems);

        [Test]
        public void StashAddingItemsToQuestStashItemsSavesCorrectly()
            => TestAddingItemsToQuestStash(StashType.QuestStashItems,
                                           "testQuestStashItemsAddingItems.json",
                                           AppData.Profile.Characters.Pmc.Inventory.QuestStashItems);

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
            TestHelpers.SaveAndLoadProfile("testStashAddingItems.json");
            var addedSick = AppData.Profile.Characters.Pmc.Inventory.Items.Where(x => x.Tpl == sick.Id).LastOrDefault();
            Assert.That(addedSick, Is.Not.Null);
            Assert.That(sickCases.Contains(addedSick.Id), Is.False);
            var addedItemsToSick = AppData.Profile.Characters.Pmc.Inventory.Items.Where(x => x.ParentId == addedSick.Id);
            Assert.That(addedItemsToSick.Count(), Is.EqualTo(3));
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
            TestHelpers.SaveAndLoadProfile("testStashAddingMoneys.json");
            var endValue = AppData.Profile.Characters.Pmc.Inventory.Items
                .Where(x => x.Tpl == AppData.AppSettings.MoneysRublesTpl)
                .Sum(x => x.Upd.StackObjectsCount ?? 0);
            Assert.That(endValue, Is.EqualTo(startValue + 2000000));
            var duplicatedItems = AppData.Profile.Characters.Pmc.Inventory.InventoryItems
                .Where(x => AppData.Profile.Characters.Pmc.Inventory.InventoryItems.Any(HaveSamePosition(x)));
            Assert.That(duplicatedItems, Is.Empty);
        }

        [Test]
        public void StashAddingDogtagWithPropertiesSavesCorrectly()
        {
            AppData.Profile.Load(TestHelpers.profileFile);
            var dogtag = AppData.ServerDatabase.ItemsDB.Values
                .Where(x => x.Properties?.DogTagQualities == true)
                .FirstOrDefault();
            var newDogtag = TarkovItem.CopyFrom(dogtag);
            newDogtag.DogtagProperties.Nickname = "Test";
            newDogtag.DogtagProperties.Level = 69;
            newDogtag.DogtagProperties.UpdateProperties();
            AppData.Profile.Characters.Pmc.Inventory.AddNewItemsToStash(newDogtag);
            TestHelpers.SaveAndLoadProfile("testStashAddingDogtag.json");
            var addedDogtag = AppData.Profile.Characters.Pmc.Inventory.InventoryItems
                .Where(x => x.Tpl == newDogtag.Id)
                .LastOrDefault();
            Assert.That(addedDogtag, Is.Not.Null, "Added dogtag not founded");
            Assert.That(addedDogtag.Upd.Dogtag.AccountId, Is.Not.EqualTo(AppData.Profile.Characters.Pmc.Aid));
            Assert.That(addedDogtag.Upd.Dogtag.ProfileId, Is.Not.EqualTo(AppData.Profile.Characters.Pmc.Aid));
            Assert.That(addedDogtag.Upd.Dogtag.KillerAccountId, Is.EqualTo(AppData.Profile.Characters.Pmc.Aid));
            Assert.That(addedDogtag.Upd.Dogtag.KillerProfileId, Is.EqualTo(AppData.Profile.Characters.Pmc.PmcId));
            Assert.That(string.IsNullOrEmpty(addedDogtag.Upd.Dogtag.Time), Is.False);
            Assert.That(addedDogtag.Upd.Dogtag.KillerName, Is.EqualTo(AppData.Profile.Characters.Pmc.Info.Nickname));
            Assert.That(addedDogtag.Upd.Dogtag.WeaponName.EndsWith(" Name"), Is.True);
            Assert.That(addedDogtag.Upd.Dogtag.WeaponName.Length > " Name".Length, Is.True);
            Assert.That(addedDogtag.Upd.Dogtag.Nickname, Is.EqualTo("Test"));
            Assert.That(addedDogtag.Upd.Dogtag.Level, Is.EqualTo(69));
            Assert.That(string.IsNullOrEmpty(addedDogtag.Upd.Dogtag.Status), Is.False);
            Assert.That(string.IsNullOrEmpty(addedDogtag.Upd.Dogtag.Side), Is.False);
        }

        [Test]
        public void WeaponBuildRemoveSavesCorrectly()
        {
            LoadProfileAndPrepareWeaponBuilds();
            var expectedId = AppData.Profile.UserBuilds.WeaponBuilds.FirstOrDefault().Id;
            AppData.Profile.UserBuilds.RemoveWeaponBuild(expectedId);
            TestHelpers.SaveAndLoadProfile("testWeaponBuildRemove.json");
            Assert.That(AppData.Profile.UserBuilds?.WeaponBuilds?.FirstOrDefault(x => x.Id == expectedId), Is.Null);
        }

        [Test]
        public void WeaponBuildsRemoveSavesCorrectly()
        {
            LoadProfileAndPrepareWeaponBuilds();
            AppData.Profile.UserBuilds.RemoveWeaponBuilds();
            TestHelpers.SaveAndLoadProfile("testWeaponBuildsRemove.json");
            Assert.That(AppData.Profile.UserBuilds?.WeaponBuilds?.Count, Is.Not.Null.And.Zero);
        }

        [Test]
        public void EquipmentBuildRemoveSavesCorrectly()
        {
            LoadProfileAndPrepareEquipmentBuilds();
            var expectedId = AppData.Profile.UserBuilds.EquipmentBuilds.FirstOrDefault().Id;
            AppData.Profile.UserBuilds.RemoveEquipmentBuild(expectedId);
            TestHelpers.SaveAndLoadProfile("testEquipmentBuildRemove.json");
            Assert.That(AppData.Profile.UserBuilds.EquipmentBuilds?.FirstOrDefault(x => x.Id == expectedId),
                        Is.Null);
        }

        [Test]
        public void EquipmentBuildsRemoveSavesCorrectly()
        {
            LoadProfileAndPrepareEquipmentBuilds();
            AppData.Profile.UserBuilds.RemoveEquipmentBuilds();
            TestHelpers.SaveAndLoadProfile("testEquipmentBuildsRemove.json");
            Assert.That(AppData.Profile.UserBuilds?.EquipmentBuilds?.Count, Is.Not.Null.And.Zero);
        }

        [Test]
        public void EmptyBuildsSavesCorrectly()
        {
            AppData.Profile.Load(TestHelpers.profileFile);
            AppData.Profile.UserBuilds.RemoveEquipmentBuilds();
            AppData.Profile.UserBuilds.RemoveWeaponBuilds();
            TestHelpers.SaveAndLoadProfile("testEmptyBuildsSave.json");
            Assert.That(AppData.Profile.UserBuilds?.WeaponBuilds, Is.Not.Null, "WeaponBuilds is null");
            Assert.That(AppData.Profile.UserBuilds?.EquipmentBuilds, Is.Not.Null, "EquipmentBuilds is null");
        }

        [Test]
        public void WeaponBuildExportCorrectly()
        {
            LoadProfileAndPrepareWeaponBuilds();
            var expected = AppData.Profile.UserBuilds.WeaponBuilds.FirstOrDefault();
            string testFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "testWeaponBuildExport.json");
            UserBuilds.ExportBuild(expected, testFile);
            WeaponBuild weaponBuild = JsonConvert.DeserializeObject<WeaponBuild>(File.ReadAllText(testFile));
            Assert.That(weaponBuild.Name, Is.EqualTo(expected.Name));
            Assert.That(weaponBuild.Root, Is.EqualTo(expected.Root));
            Assert.That(weaponBuild.RecoilForceBack, Is.EqualTo(expected.RecoilForceBack));
            Assert.That(weaponBuild.RecoilForceUp, Is.EqualTo(expected.RecoilForceUp));
            Assert.That(weaponBuild.Ergonomics, Is.EqualTo(expected.Ergonomics));
            Assert.That(weaponBuild.Items.Length, Is.EqualTo(expected.Items.Length));
        }

        [Test]
        public void EquipmentBuildExportCorrectly()
        {
            LoadProfileAndPrepareEquipmentBuilds();
            var expected = AppData.Profile.UserBuilds.EquipmentBuilds.FirstOrDefault();
            string testFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "testEquipmentBuildExport.json");
            UserBuilds.ExportBuild(expected, testFile);
            EquipmentBuild equipmentBuild = JsonConvert.DeserializeObject<EquipmentBuild>(File.ReadAllText(testFile));
            Assert.That(equipmentBuild.Name, Is.EqualTo(expected.Name));
            Assert.That(equipmentBuild.Root, Is.EqualTo(expected.Root));
            Assert.That(equipmentBuild.Items.Length, Is.EqualTo(expected.Items.Length));
            Assert.That(equipmentBuild.FastPanel?.Length ?? 0, Is.EqualTo(expected.FastPanel?.Length ?? 0));
        }

        [Test]
        public void WeaponBuildImportSavesCorrectly()
        {
            AppData.Profile.Load(TestHelpers.profileFile);
            var startCount = AppData.Profile.UserBuilds.WeaponBuilds.Where(x => x.Name.StartsWith("Test")).Count();
            AppData.Profile.UserBuilds.ImportWeaponBuildFromFile(TestHelpers.weaponBuild);
            AppData.Profile.UserBuilds.ImportWeaponBuildFromFile(TestHelpers.weaponBuild);
            TestHelpers.SaveAndLoadProfile("testWeaponBuildsImport.json");
            Assert.That(AppData.Profile.UserBuilds.WeaponBuilds.Any(x => x.Name == "TestBuild"), Is.True);
            Assert.That(AppData.Profile.UserBuilds.WeaponBuilds.Where(x => x.Name.StartsWith("Test")).Count(),
                        Is.EqualTo(startCount + 2));
        }

        [Test]
        public void EquipmentBuildImportSavesCorrectly()
        {
            AppData.Profile.Load(TestHelpers.profileFile);
            var startCount = AppData.Profile.UserBuilds.EquipmentBuilds.Where(x => x.Name.StartsWith("Test")).Count();
            AppData.Profile.UserBuilds.ImportEquipmentBuildFromFile(TestHelpers.equipmentBuild);
            AppData.Profile.UserBuilds.ImportEquipmentBuildFromFile(TestHelpers.equipmentBuild);
            TestHelpers.SaveAndLoadProfile("testEquipmentBuildsImport.json");
            Assert.That(AppData.Profile.UserBuilds.EquipmentBuilds.Any(x => x.Name == "TestBuild"), Is.True);
            Assert.That(AppData.Profile.UserBuilds.EquipmentBuilds.Where(x => x.Name.StartsWith("Test")).Count(),
                        Is.EqualTo(startCount + 2));
        }

        [Test]
        public void WeaponBuildCalculatingCorrectly()
        {
            AppData.Profile.Load(TestHelpers.profileFile);
            AppData.Profile.UserBuilds.ImportWeaponBuildFromFile(TestHelpers.weaponBuild);
            var build = AppData.Profile.UserBuilds.WeaponBuilds.Where(x => x.Name == "TestBuild").FirstOrDefault();
            Assert.That(build, Is.Not.Null);
            Assert.That(build.Ergonomics, Is.EqualTo(36));
            Assert.That(build.RecoilForceUp, Is.EqualTo(68));
            Assert.That(build.RecoilForceBack, Is.EqualTo(193));
        }

        [Test]
        public void WeaponBuildAddedToContainerSavesCorrectly()
        {
            AppData.Profile.Load(TestHelpers.profileFile);
            WeaponBuild weaponBuild = JsonConvert.DeserializeObject<WeaponBuild>(File.ReadAllText(TestHelpers.weaponBuild));
            List<string> iDs = [weaponBuild.Root];
            var weaponsCount = AppData.Profile.Characters.Pmc.Inventory.Items.Where(x => x.Tpl == weaponBuild.RootTpl).Count();
            iDs.AddRange(weaponBuild.BuildItems.Select(x => x.Id));
            weaponBuild.AddingQuantity = 2;
            weaponBuild.AddingFir = true;
            AppData.Profile.Characters.Pmc.Inventory.AddNewItemsToStash(weaponBuild);
            TestHelpers.SaveAndLoadProfile("testStashAddingWeapons.json");
            Assert.That(AppData.Profile.Characters.Pmc.Inventory.Items.Where(x => x.Tpl == weaponBuild.RootTpl).Count(),
                        Is.EqualTo(weaponsCount + 2));
            Assert.That(AppData.Profile.Characters.Pmc.Inventory.Items.Select(x => x.Id).Any(y => iDs.Contains(y)),
                        Is.False);
        }

        [Test]
        public void ProfileNotChangedAfterLoading()
        {
            DisableAutoAddDataInSettings();
            AppData.Profile.Load(TestHelpers.profileFile);
            Assert.That(AppData.Profile.IsProfileChanged(), Is.False);
        }

        [Test]
        public void ProfileChangedAfterAddingCrafts()
        {
            DisableAutoAddDataInSettings();
            AppData.Profile.Load(TestHelpers.profileFile);
            Assert.That(AppData.Profile.IsProfileChanged(), Is.False);
            AppData.Profile.Characters.Pmc.AddAllCrafts();
            Assert.That(AppData.Profile.IsProfileChanged(), Is.True);
        }

        [Test]
        public void ProfileChangedAfterEditings()
        {
            DisableAutoAddDataInSettings();
            AppData.Profile.Load(TestHelpers.profileFile);
            AppData.Profile.Characters.Pmc.SetAllTradersMax();
            AppData.Profile.Characters.Pmc.SetAllHideoutAreasMax();
            AppData.Profile.Characters.Pmc.SetAllQuests(QuestStatus.Fail);
            Assert.That(AppData.Profile.IsProfileChanged(), Is.True);
        }

        [Test]
        public void ProfileCanRemoveDuplicatedItems()
        {
            AppData.Profile.Load(TestHelpers.profileWithDuplicatedItems);
            Assert.That(AppData.Profile.Characters.Pmc.Inventory.InventoryHaveDuplicatedItems, Is.True);
            AppData.Profile.Characters.Pmc.Inventory.RemoveDuplicatedItems();
            Assert.That(AppData.Profile.Characters.Pmc.Inventory.InventoryHaveDuplicatedItems, Is.False);
            TestHelpers.SaveAndLoadProfile("testRemoveDuplicatedItems.json");
            Assert.That(AppData.Profile.Characters.Pmc.Inventory.InventoryHaveDuplicatedItems, Is.False);
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
            TestHelpers.SaveAndLoadProfile("testHealth.json");
            Assert.That(AppData.Profile.Characters.Pmc.Health.Energy.Current,
                        Is.EqualTo(300),
                        "Health.Energy.Current is not 300");
            Assert.That(AppData.Profile.Characters.Pmc.Health.Energy.Maximum,
                        Is.EqualTo(300),
                        "Health.Energy.Maximum is not 300");
            Assert.That(AppData.Profile.Characters.Pmc.Health.Hydration.Current,
                        Is.EqualTo(350),
                        "Health.Hydration.Current is not 350");
            Assert.That(AppData.Profile.Characters.Pmc.Health.Hydration.Maximum,
                        Is.EqualTo(350),
                        "Health.Hydration.Maximum is not 350");
            Assert.That(AppData.Profile.Characters.Pmc.Health.BodyParts.Head.Health.Current,
                        Is.EqualTo(400),
                        "Health.BodyParts.Head.Health.Current is not 400");
            Assert.That(AppData.Profile.Characters.Pmc.Health.BodyParts.Head.Health.Maximum,
                        Is.EqualTo(400),
                        "Health.BodyParts.Head.Health.Maximum is not 400");
            Assert.That(AppData.Profile.Characters.Pmc.Health.BodyParts.Chest.Health.Current,
                        Is.EqualTo(450),
                        "Health.BodyParts.Chest.Health.Current is not 450");
            Assert.That(AppData.Profile.Characters.Pmc.Health.BodyParts.Chest.Health.Maximum,
                        Is.EqualTo(450),
                        "Health.BodyParts.Chest.Health.Maximum is not 450");
            Assert.That(AppData.Profile.Characters.Pmc.Health.BodyParts.Stomach.Health.Current,
                        Is.EqualTo(500),
                        "Health.BodyParts.Stomach.Health.Current is not 500");
            Assert.That(AppData.Profile.Characters.Pmc.Health.BodyParts.Stomach.Health.Maximum,
                        Is.EqualTo(500),
                        "Health.BodyParts.Stomach.Health.Maximum is not 500");
            Assert.That(AppData.Profile.Characters.Pmc.Health.BodyParts.LeftArm.Health.Current,
                        Is.EqualTo(550),
                        "Health.BodyParts.LeftArm.Health.Current is not 550");
            Assert.That(AppData.Profile.Characters.Pmc.Health.BodyParts.LeftArm.Health.Maximum,
                        Is.EqualTo(550),
                        "Health.BodyParts.LeftArm.Health.Maximum is not 550");
            Assert.That(AppData.Profile.Characters.Pmc.Health.BodyParts.RightArm.Health.Current,
                        Is.EqualTo(600),
                        "Health.BodyParts.RightArm.Health.Current is not 600");
            Assert.That(AppData.Profile.Characters.Pmc.Health.BodyParts.RightArm.Health.Maximum,
                        Is.EqualTo(600),
                        "Health.BodyParts.RightArm.Health.Maximum is not 600");
            Assert.That(AppData.Profile.Characters.Pmc.Health.BodyParts.LeftLeg.Health.Current,
                        Is.EqualTo(650),
                        "Health.BodyParts.LeftLeg.Health.Current is not 650");
            Assert.That(AppData.Profile.Characters.Pmc.Health.BodyParts.LeftLeg.Health.Maximum,
                        Is.EqualTo(650),
                        "Health.BodyParts.LeftLeg.Health.Maximum is not 650");
            Assert.That(AppData.Profile.Characters.Pmc.Health.BodyParts.RightLeg.Health.Current,
                        Is.EqualTo(700),
                        "Health.BodyParts.RightLeg.Health.Current is not 700");
            Assert.That(AppData.Profile.Characters.Pmc.Health.BodyParts.RightLeg.Health.Maximum,
                        Is.EqualTo(700),
                        "Health.BodyParts.RightLeg.Health.Maximum is not 700");
        }

        [Test]
        public void AchievementsSavesCorrectly()
        {
            AppData.Profile.Load(TestHelpers.profileFile);
            var startCount = AppData.Profile.Characters.Pmc.Achievements.Count;
            AppData.Profile.Characters.Pmc.ReceiveAllAchievements();
            TestHelpers.SaveAndLoadProfile("testAchievementst.json");
            Assert.That(AppData.Profile.Characters.Pmc.Achievements.Count, Is.GreaterThan(startCount));
        }

        private static void SaveAndCheckStashBonuses(int expectedValue, string filename)
        {
            AppData.Profile.Characters.Pmc.StashRowsBonusCount = expectedValue;
            TestHelpers.SaveAndLoadProfile(filename);
            Assert.That(AppData.Profile.Characters.Pmc.StashRowsBonusCount,
                        Is.EqualTo(expectedValue),
                        "Stash bonuses count wrong");
        }

        private static void LoadProfileWithAllMasterings()
        {
            AppData.AppSettings.AutoAddMissingMasterings = true;
            AppData.Profile.Load(TestHelpers.profileFile);
        }

        private static void MasteringSkillsSavesCorrectly(Character character)
        {
            character.SetAllMasteringsSkills(AppData.ServerDatabase.ServerGlobals.Config.Mastering.Max(x => x.Level2 + x.Level3));
            TestHelpers.SaveAndLoadProfile("testPmcMasteringSkills.json");
            var isAllSkillsProgressMax = character.Skills.Mastering.All(x => x.Progress == x.MaxValue);
            var profileSkillsCount = character.Skills.Mastering.Length;
            var dbSkillsCount = AppData.ServerDatabase.ServerGlobals.Config.Mastering.Length;
            Assert.That(isAllSkillsProgressMax && profileSkillsCount == dbSkillsCount, Is.True);
        }

        private static Func<InventoryItem, bool> HaveSamePosition(InventoryItem x)
            => y => y.Id != x.Id && y.Location.X == x.Location.X && y.Location.Y == x.Location.Y;

        private static void LoadProfileAndCheckQuestsCount(string profilePath, bool isStandartQuests)
        {
            AppData.Profile.Load(profilePath);
            AppData.Profile.Characters.Pmc.AddAllMisingQuests(!isStandartQuests);

            var questsCount = AppData.ServerDatabase.QuestsData
                .Where(x => AppData.ServerConfigs.Quest.EventQuests.ContainsKey(x.Key) != isStandartQuests)
                .Count();
            Assert.That(AppData.Profile.Characters.Pmc.Quests.Length,
                        Is.EqualTo(questsCount),
                        $"{(isStandartQuests ? "Standart" : "Event")} quests count wrong");
        }

        private static void CheckBuilds<T>(List<T> buildsList, ObservableCollection<T> buildsCollection) where T : Build
        {
            Assert.That(buildsList.Count == 0, Is.False);
            Assert.That(buildsCollection.Count == 0, Is.False);
        }

        private static void TestAddingItemsToQuestStash(StashType stashType,
                                                        string filename,
                                                        string expectedStashId)
        {
            AppData.Profile.Load(TestHelpers.profileFile);
            var items = AppData.ServerDatabase.ItemsDB
                .Values
                .OrderBy(x => x.Properties.Width * x.Properties.Height)
                .Where(x => x.CanBeAddedToStash && x.IsQuestItem)
                .Take(2);
            foreach (var item in items)
                item.StashType = stashType;
            _ = AddAndGetItemsToProfileStash(items, filename, expectedStashId);
        }

        private static InventoryItem[] AddAndGetItemsToProfileStash(IEnumerable<TarkovItem> items,
                                                                    string filename,
                                                                    string expectedStashId)
        {
            foreach (var item in items)
                AppData.Profile.Characters.Pmc.Inventory.AddNewItemsToStash(item);
            TestHelpers.SaveAndLoadProfile(filename);
            var savedItems = AppData.Profile.Characters.Pmc.Inventory.Items
                .Where(x => items.Any(y => y.Id == x.Tpl)).ToArray();
            Assert.That(savedItems.Length,
                        Is.EqualTo(items.Sum(x => x.AddingQuantity)),
                        "Added items count wrong");
            Assert.That(savedItems.All(x => x.ParentId == expectedStashId),
                        Is.True,
                        "Wrong parentId in added items");
            Assert.That(savedItems.Select(x => x.Id).Distinct().Count(),
                        Is.EqualTo(savedItems.Length),
                        "Added items have not unique id's");
            return savedItems;
        }

        private static void CheckCharacterMetric(CharacterMetric metric)
        {
            Assert.That(metric, Is.Not.Null);
            Assert.That(metric.Current == 0, Is.False);
            Assert.That(metric.Maximum == 0, Is.False);
        }

        private static void LoadProfileAndPrepareWeaponBuilds()
        {
            AppData.Profile.Load(TestHelpers.profileFile);
            if (AppData.Profile.UserBuilds.WeaponBuilds.Count == 0)
                AppData.Profile.UserBuilds.ImportWeaponBuildFromFile(TestHelpers.weaponBuild);
        }

        private static void LoadProfileAndPrepareEquipmentBuilds()
        {
            AppData.Profile.Load(TestHelpers.profileFile);
            if (AppData.Profile.UserBuilds.EquipmentBuilds.Count == 0)
                AppData.Profile.UserBuilds.ImportEquipmentBuildFromFile(TestHelpers.equipmentBuild);
        }

        private static void CommonSkillsSavesCorrectly(Character character, string filename)
        {
            AppData.Profile.Load(TestHelpers.profileFile);
            character.SetAllCommonSkills(AppData.AppSettings.CommonSkillMaxValue);
            TestHelpers.SaveAndLoadProfile(filename);
            Assert.That(character.Skills.Common
                .Where(x => x.Id.StartsWith("bot", StringComparison.CurrentCultureIgnoreCase))
                .Any(x => x.Progress > 0), Is.False);
            Assert.That(character.Skills.Common.All(x => x.Id.StartsWith("bot", StringComparison.CurrentCultureIgnoreCase) || x.Progress == AppData.AppSettings.CommonSkillMaxValue),
                        Is.True);
        }

        private static void PocketsSavesCorrectly(CharacterInventory inventory, string filename)
        {
            AppData.Profile.Load(TestHelpers.profileFile);
            string expected = AppData.ServerDatabase.Pockets.Last().Key;
            inventory.Pockets = expected;
            TestHelpers.SaveAndLoadProfile(filename);
            Assert.That(inventory.Pockets == expected, Is.True);
        }

        private static void RemovingItemsSavesCorrectly(List<string> itemIds, CharacterInventory inventory, string filename)
        {
            inventory.RemoveItems(itemIds);
            TestHelpers.SaveAndLoadProfile(filename);
            foreach (var id in itemIds)
            {
                Assert.That(inventory.Items.Any(x => x.Id == id), Is.False, "id not removed");
                Assert.That(inventory.Items.Any(x => x.ParentId == id), Is.False, "id child items not removed");
            }
        }

        private static void RemovingAllEquipmentSavesCorrectly(CharacterInventory inventory, string filename)
        {
            AppData.Profile.Load(TestHelpers.profileFile);
            inventory.RemoveAllEquipment();
            TestHelpers.SaveAndLoadProfile(filename);
            Assert.That(inventory.EquipmentSlots.SelectMany(x => x.ItemsList).Any(x => x != null), Is.False);
        }

        private static void DisableAutoAddDataInSettings()
        {
            AppData.AppSettings.AutoAddMissingMasterings = false;
            AppData.AppSettings.AutoAddMissingScavSkills = false;
        }
    }
}