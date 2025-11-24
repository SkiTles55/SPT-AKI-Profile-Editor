using NUnit.Framework;
using NUnit.Framework.Internal;
using SPT_AKI_Profile_Editor.Core;
using SPT_AKI_Profile_Editor.Core.Enums;
using SPT_AKI_Profile_Editor.Core.ProfileClasses;
using SPT_AKI_Profile_Editor.Core.ServerClasses;
using SPT_AKI_Profile_Editor.Core.ServerClasses.Hideout;
using SPT_AKI_Profile_Editor.Tests.Hepers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SPT_AKI_Profile_Editor.Tests
{
    internal class ServerDatabaseTests
    {
        [OneTimeSetUp]
        public void Setup() => TestHelpers.LoadDatabase();

        [Test]
        public void BotTypesHeadsNotNull()
            => Assert.That(AppData.ServerDatabase.Heads, Is.Not.Null);

        [Test]
        public void BotTypesHeadsNotEmpty()
            => Assert.That(AppData.ServerDatabase.Heads, Is.Not.Empty);

        [Test]
        public void BotTypesHeadsHasCorrectNames()
        {
            foreach (var head in AppData.ServerDatabase.Heads)
                Assert.That(string.IsNullOrWhiteSpace(head.Value),
                            Is.False,
                            $"head {head.Key} has incorrect name: {head.Value}");
        }

        [Test]
        public void BotTypesVoicesNotEmpty()
            => Assert.That(AppData.ServerDatabase.Voices, Is.Not.Empty);

        [Test]
        public void BotTypesVoicesNotNull()
            => Assert.That(AppData.ServerDatabase.Voices, Is.Not.Null);

        [Test]
        public void LocalesGlobalNotNull()
            => Assert.That(AppData.ServerDatabase.LocalesGlobal, Is.Not.Null);

        [Test]
        public void LocalesGlobalNotEmpty()
            => Assert.That(AppData.ServerDatabase.LocalesGlobal, Is.Not.Empty);

        [Test]
        public void ServerGlobalsNotNull()
            => Assert.That(AppData.ServerDatabase.ServerGlobals, Is.Not.Null);

        [Test]
        public void ServerGlobalsConfigNotNull()
            => Assert.That(AppData.ServerDatabase.ServerGlobals.Config, Is.Not.Null);

        [Test]
        public void ServerGlobalsItemPresetsNotNull()
            => Assert.That(AppData.ServerDatabase.ServerGlobals.ItemPresets.Count != 0, Is.True);

        [Test]
        public void ServerGlobalsItemPresetsCanbeConvertedToWeaponBuilds()
        {
            var builds = AppData.ServerDatabase.ServerGlobals.ItemPresets.Values.Select(x => new WeaponBuild(x));
            Assert.That(builds.Any(x => x == null || !x.BuildItems.Any()), Is.False, "BuildItems is empty");
            Assert.That(builds.Any(x => string.IsNullOrEmpty(x.LocalizedName)), Is.False, "LocalizedName wrong");
            Assert.That(builds.Any(x => x.Weapon == null), Is.False, "Weapon is null");
            Assert.That(builds.Any(x => x.HasModdedItems), Is.False, "WeaponBuild has modded items");
        }

        [Test]
        public void ServerGlobalsConfigExpLevelExpTableNotEmpty()
            => Assert.That(AppData.ServerDatabase.ServerGlobals.Config.Exp.Level.ExpTable.Length > 0, Is.True);

        [Test]
        public void ServerGlobalsConfigExpLevelMaxExpNotZero()
            => Assert.That(AppData.ServerDatabase.ServerGlobals.Config.Exp.Level.MaxExp > 0, Is.True);

        [Test]
        public void ServerGlobalsConfigExpLevelMaxLevelGreaterThanOne()
            => Assert.That(AppData.ServerDatabase.ServerGlobals.Config.Exp.Level.MaxLevel > 1, Is.True);

        [Test]
        public void ServerGlobalsMasteringNotEmpty()
            => Assert.That(AppData.ServerDatabase.ServerGlobals.Config.Mastering.Length > 0, Is.True);

        [Test]
        public void ServerGlobalsHasMaxProgressValue()
            => Assert.That(AppData.ServerDatabase.ServerGlobals.Config.MaxProgressValue > 0, Is.True);

        [Test]
        public void TraderInfosNotEmpty()
            => Assert.That(AppData.ServerDatabase.TraderInfos, Is.Not.Empty);

        [Test]
        public void TraderInfosNotNul()
            => Assert.That(AppData.ServerDatabase.TraderInfos, Is.Not.Null);

        [Test]
        public void TradersInfosHaveIds()
            => Assert.That(AppData.ServerDatabase.TraderInfos.Any(x => string.IsNullOrEmpty(x.Value.Id)), Is.False);

        [Test]
        public void TradersInfosHaveImageUrl()
            => Assert.That(AppData.ServerDatabase.TraderInfos.Any(x => string.IsNullOrEmpty(x.Value.ImageUrl)), Is.False);

        [Test]
        public void TradersInfosHaveLoyaltyLevels()
            => Assert.That(AppData.ServerDatabase.TraderInfos.Any(x => x.Value.LoyaltyLevels.Count == 0), Is.False);

        [Test]
        public void TradersInfosLoyaltyLevelsHaveMinLevel()
            => Assert.That(AppData.ServerDatabase.TraderInfos.Any(x => x.Value.LoyaltyLevels.Any(l => l.MinLevel > 0)),
                           Is.True);

        [Test]
        public void TradersInfosLoyaltyLevelsHaveMinSalesSum()
            => Assert.That(AppData.ServerDatabase.TraderInfos.Any(x => x.Value.LoyaltyLevels.Any(l => l.MinSalesSum > 0)),
                           Is.True);

        [Test]
        public void TradersInfosLoyaltyLevelsHaveMinStanding()
            => Assert.That(AppData.ServerDatabase.TraderInfos.Any(x => x.Value.LoyaltyLevels.Any(l => l.MinStanding > 0)),
                           Is.True);

        [Test]
        public void QuestsDataNotNul()
            => Assert.That(AppData.ServerDatabase.QuestsData, Is.Not.Null);

        [Test]
        public void QuestsDataNotEmpty()
            => Assert.That(AppData.ServerDatabase.QuestsData, Is.Not.Empty);

        [Test]
        public void QuestsDataConditionsNotEmpty()
            => Assert.That(AppData.ServerDatabase.QuestsData.Any(x => x.Value.Conditions.AvailableForStart.Count != 0),
                           Is.True);

        [Test]
        public void QuestsDataConditionsLevelNotEmpty()
            => Assert.That(AppData.ServerDatabase.QuestsData.Any(ContainsCondition(QuestConditionType.Level)),
                           Is.True);

        [Test]
        public void QuestsDataConditionsQuestNotEmpty()
            => Assert.That(AppData.ServerDatabase.QuestsData.Any(ContainsCondition(QuestConditionType.Quest)),
                           Is.True);

        [Test]
        public void QuestsDataConditionsTraderLoyaltyNotEmpty()
            => Assert.That(AppData.ServerDatabase.QuestsData.Any(ContainsCondition(QuestConditionType.TraderLoyalty)),
                           Is.True);

        [Test]
        public void QuestsDataConditionsTraderStandingNotEmpty()
            => Assert.That(AppData.ServerDatabase.QuestsData.Any(ContainsCondition(QuestConditionType.TraderStanding)),
                           Is.True);

        [Test]
        public void QuestsDataConditionsUnknownIsEmpty()
            => Assert.That(AppData.ServerDatabase.QuestsData.Any(ContainsCondition(QuestConditionType.Unknown)),
                           Is.False);

        [Test]
        public void QuestsDataConditionsCompareMethodGreaterOrEqualNotEmpty()
            => Assert.That(AppData.ServerDatabase.QuestsData.Any(ContainsCompareMethod(">=")),
                           Is.True);

        [Test]
        public void QuestsDataConditionsCompareMethodNullNotEmpty()
            => Assert.That(AppData.ServerDatabase.QuestsData.Any(ContainsCompareMethod(null)),
                           Is.True);

        [Test]
        public void HideoutAreaInfosNotNul() => Assert.That(AppData.ServerDatabase.HideoutAreaInfos, Is.Not.Null);

        [Test]
        public void HideoutAreaInfosDataNotEmpty() => Assert.That(AppData.ServerDatabase.HideoutAreaInfos, Is.Not.Empty);

        [Test]
        public void HideoutProductionLoadsCorrectly()
        {
            Assert.That(AppData.ServerDatabase.HideoutProduction, Is.Not.Null, "HideoutProduction is null");
            Assert.That(AppData.ServerDatabase.HideoutProduction, Is.Not.Empty, "HideoutProduction is empty");
            Assert.That(AppData.ServerDatabase.HideoutProduction.Any(x => string.IsNullOrWhiteSpace(x.Id)),
                        Is.False,
                        "HideoutProductions has item with null id");
            Assert.That(AppData.ServerDatabase.HideoutProduction.Any(x => x.Requirements == null),
                        Is.False,
                        "HideoutProductions has item with null Requirements");
            Assert.That(AppData.ServerDatabase.HideoutProduction.Any(x => x.Requirements.Any(r => r.Type == RequirementType.QuestComplete)),
                        Is.True,
                        "HideoutProductions doesn't have items with QuestComplete type requirement");
            Assert.That(AppData.ServerDatabase.HideoutProduction.Any(x => x.Requirements.Any(r => r.Type == RequirementType.Unknown)),
                        Is.True,
                        "HideoutProductions doesn't have items with Unknown type requirement");
            Assert.That(AppData.ServerDatabase.HideoutProduction.Any(x => x.Locked),
                        Is.True,
                        "HideoutProductions doesn't have locked productions");
            Assert.That(AppData.ServerDatabase.HideoutProduction.Any(x => x.UnlocksByQuest),
                        Is.True,
                        "HideoutProductions doesn't have locked by quest productions");
            Assert.That(AppData.ServerDatabase.HideoutProduction.Any(x => x.AreaType != 0),
                        Is.True,
                        "HideoutProductions doesn't productions for area type not 0");
            Assert.That(AppData.ServerDatabase.HideoutProduction.Any(x => x.Requirements.Any(r => !string.IsNullOrEmpty(r.QuestId))),
                        Is.True,
                        "HideoutProductions doesn't have productions with quest id in Requirements");
        }

        [Test]
        public void ItemsDBNotNul() => Assert.That(AppData.ServerDatabase.ItemsDB, Is.Not.Null);

        [Test]
        public void ItemsDBNotEmpty() => Assert.That(AppData.ServerDatabase.ItemsDB, Is.Not.Empty);

        [Test]
        public void ItemsDBHaveItemsWithCategoryIcon()
            => Assert.That(AppData.ServerDatabase.ItemsDB.Any(x => x.Value.CategoryIcon != null), Is.True);

        [Test]
        public void ItemsDBHaveItemsWithDescription()
            => Assert.That(AppData.ServerDatabase.ItemsDB.Any(x => x.Value.LocalizedDescription != x.Value.Id),
                           Is.True);

        [Test]
        public void ItemsDBHaveItemsWithSlots()
            => Assert.That(AppData.ServerDatabase.ItemsDB.Any(x => x.Value.Properties?.Slots?.Any() ?? false),
                           Is.True);

        [Test]
        public void PocketsNotNul() => Assert.That(AppData.ServerDatabase.Pockets, Is.Not.Null);

        [Test]
        public void PocketsNotEmpty() => Assert.That(AppData.ServerDatabase.Pockets, Is.Not.Empty);

        [Test]
        public void TraderSuitsNotNul() => Assert.That(AppData.ServerDatabase.TraderSuits, Is.Not.Null);

        [Test]
        public void TraderSuitsNotEmpty() => Assert.That(AppData.ServerDatabase.TraderSuits, Is.Not.Empty);

        [Test]
        public void SlotsCountCalculatesCorrectly()
            => Assert.That(AppData.ServerDatabase.ItemsDB["557ffd194bdc2d28148b457f"].SlotsCount, Is.EqualTo(4));

        [Test]
        public void ItemsDBFilterForPistolCaseLoadCorrectly()
            => Assert.That(AppData.ServerDatabase.ItemsDB["567143bf4bdc2d1a0f8b4567"].Properties.Grids[0].Props.Filters[0].Filter,
                           Is.Not.Empty);

        [Test]
        public void ItemsDBFilterLoadCorrectly()
            => Assert.That(AppData.ServerDatabase.ItemsDB.Values.Any(x => x.Properties?.Grids?.Any(y => (y.Props?.Filters?[0].Filter?.Length ?? 0) > 0) ?? false),
                           Is.True);

        [Test]
        public void HandbookLoadsCorrectly()
        {
            Assert.That(AppData.ServerDatabase.Handbook, Is.Not.Null, "Handbook is null");
            Assert.That(AppData.ServerDatabase.Handbook.Categories, Is.Not.Empty, "Handbook Categories empty");
            Assert.That(AppData.ServerDatabase.Handbook.Categories.Any(x => string.IsNullOrEmpty(x.Id)),
                        Is.False,
                        "Handbook Categories doesnt have id's");
            Assert.That(AppData.ServerDatabase.Handbook.Items, Is.Not.Empty, "Handbook Items empty");
        }

        [Test]
        public void HandbookHelperCanInitialize()
            => Assert.That(new HandbookHelper(AppData.ServerDatabase.Handbook.Categories,
                                              AppData.ServerDatabase.ItemsDB,
                                              AppData.ServerDatabase.ServerGlobals.GlobalBuilds),
                            Is.Not.Null);

        [Test]
        public void HandbookHelperCategoriesForItemsAddingNotEmpty()
            => Assert.That(AppData.ServerDatabase.HandbookHelper.CategoriesForItemsAdding.Any(), Is.True);

        [Test]
        public void HandbookHelperCategoriesForItemsAddingWithFilterNotEmpty()
            => Assert.That(AppData.ServerDatabase.HandbookHelper.CategoriesForItemsAddingWithFilter("5c093ca986f7740a1867ab12").Any(),
                           Is.True);

        [Test]
        public void HandbookHelperCategoriesForItemsAddingHaveCategories()
            => Assert.That(AppData.ServerDatabase.HandbookHelper.CategoriesForItemsAdding.Any(x => x.Categories.Any()),
                           Is.True);

        [Test]
        public void HandbookHelperCategoriesForItemsAddingHaveItems()
            => Assert.That(AppData.ServerDatabase.HandbookHelper.CategoriesForItemsAdding.Any(x => x.Items.Any()),
                           Is.True);

        [Test]
        public void HandbookHelperCategoriesForItemsAddingHaveBitmapImages()
            => Assert.That(AppData.ServerDatabase.HandbookHelper.CategoriesForItemsAdding.Any(x => x.BitmapIcon != null),
                           Is.True);

        [Test]
        public void HandbookHelperCategoriesForItemsAddingHaveLocalizedNames()
            => Assert.That(AppData.ServerDatabase.HandbookHelper.CategoriesForItemsAdding.All(x => !string.IsNullOrEmpty(x.LocalizedName)),
                           Is.True);

        [Test]
        public void HandbookHelperCategoriesForItemsAddingHaveIconPath()
            => Assert.That(AppData.ServerDatabase.HandbookHelper.CategoriesForItemsAdding.Any(x => !string.IsNullOrEmpty(x.Icon)),
                           Is.True);

        [Test]
        public void HandbookHelperCategoriesForItemsAddingPrimaryNotHaveParentId()
            => Assert.That(AppData.ServerDatabase.HandbookHelper.CategoriesForItemsAdding.All(x => string.IsNullOrEmpty(x.ParentId)),
                           Is.True);

        [Test]
        public void HandbookHelperCategoriesForItemsAddingHaveNotHidden()
            => Assert.That(AppData.ServerDatabase.HandbookHelper.CategoriesForItemsAdding.All(x => x.IsNotHidden),
                           Is.True);

        [Test]
        public void HandbookHelperCategoriesForItemsAddingNotExpanded()
            => Assert.That(AppData.ServerDatabase.HandbookHelper.CategoriesForItemsAdding.All(x => !x.IsExpanded),
                           Is.True);

        private static Func<KeyValuePair<string, QuestData>, bool> ContainsCondition(QuestConditionType condition)
                                                                                                                                                                                                                                                                            => x => x.Value.Conditions.AvailableForStart.Any(y => y.Type == condition);

        private static Func<KeyValuePair<string, QuestData>, bool> ContainsCompareMethod(string method)
            => x => x.Value.Conditions.AvailableForStart.Any(y => y.CompareMethod == method);
    }
}