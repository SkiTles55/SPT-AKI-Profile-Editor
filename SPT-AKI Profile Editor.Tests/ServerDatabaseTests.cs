using NUnit.Framework;
using SPT_AKI_Profile_Editor.Core;
using SPT_AKI_Profile_Editor.Core.ProfileClasses;
using SPT_AKI_Profile_Editor.Core.ServerClasses;
using SPT_AKI_Profile_Editor.Tests.Hepers;
using System.Linq;
using static SPT_AKI_Profile_Editor.Core.ServerClasses.QuestData.QuestConditions.QuestCondition;

namespace SPT_AKI_Profile_Editor.Tests
{
    internal class ServerDatabaseTests
    {
        [OneTimeSetUp]
        public void Setup()
        {
            AppData.AppSettings.ServerPath = TestHelpers.serverPath;
            AppData.LoadDatabase();
        }

        [Test]
        public void BotTypesHeadsNotEmpty() => Assert.IsFalse(AppData.ServerDatabase.Heads.Count == 0);

        [Test]
        public void BotTypesHeadsNotNull() => Assert.IsNotNull(AppData.ServerDatabase.Heads);

        [Test]
        public void BotTypesVoicesNotEmpty() => Assert.IsFalse(AppData.ServerDatabase.Voices.Count == 0);

        [Test]
        public void BotTypesVoicesNotNull() => Assert.IsNotNull(AppData.ServerDatabase.Voices);

        [Test]
        public void LocalesGlobalNotNull() => Assert.IsNotNull(AppData.ServerDatabase.LocalesGlobal);

        [Test]
        public void LocalesGlobalNotEmpty() => Assert.IsFalse(AppData.ServerDatabase.LocalesGlobal.Count == 0);

        [Test]
        public void ServerGlobalsNotNull() => Assert.IsNotNull(AppData.ServerDatabase.ServerGlobals);

        [Test]
        public void ServerGlobalsConfigNotNull() => Assert.IsNotNull(AppData.ServerDatabase.ServerGlobals.Config);

        [Test]
        public void ServerGlobalsItemPresetsNotNull() => Assert.True(AppData.ServerDatabase.ServerGlobals.ItemPresets.Any());

        [Test]
        public void ServerGlobalsItemPresetsCanbeConvertedToWeaponBuilds()
        {
            var builds = AppData.ServerDatabase.ServerGlobals.ItemPresets.Values.Select(x => new WeaponBuild(x));
            Assert.False(builds.Any(x => x == null || !x.BuildItems.Any()));
            Assert.False(builds.Any(x => string.IsNullOrEmpty(x.LocalizedName)));
        }

        [Test]
        public void ServerGlobalsConfigExpLevelExpTableNotEmpty() => Assert.IsTrue(AppData.ServerDatabase.ServerGlobals.Config.Exp.Level.ExpTable.Length > 0);

        [Test]
        public void ServerGlobalsConfigExpLevelMaxExpNotZero() => Assert.True(AppData.ServerDatabase.ServerGlobals.Config.Exp.Level.MaxExp > 0);

        [Test]
        public void ServerGlobalsMasteringNotEmpty() => Assert.IsTrue(AppData.ServerDatabase.ServerGlobals.Config.Mastering.Length > 0);

        [Test]
        public void ServerGlobalsHasMaxProgressValue() => Assert.IsTrue(AppData.ServerDatabase.ServerGlobals.Config.MaxProgressValue > 0);

        [Test]
        public void TraderInfosNotEmpty() => Assert.IsFalse(AppData.ServerDatabase.TraderInfos.Count == 0);

        [Test]
        public void TraderInfosNotNul() => Assert.IsNotNull(AppData.ServerDatabase.TraderInfos);

        [Test]
        public void QuestsDataNotNul() => Assert.IsNotNull(AppData.ServerDatabase.QuestsData);

        [Test]
        public void QuestsDataNotEmpty() => Assert.IsFalse(AppData.ServerDatabase.QuestsData.Count == 0);

        [Test]
        public void QuestsDataConditionsNotEmpty() => Assert.True(AppData.ServerDatabase.QuestsData.Any(x => x.Value.Conditions.AvailableForStart.Count != 0));

        [Test]
        public void QuestsDataConditionsLevelNotEmpty() => Assert.True(AppData.ServerDatabase.QuestsData.Any(x => x.Value.Conditions.AvailableForStart.Any(y => y.Type == QuestConditionType.Level)));

        [Test]
        public void QuestsDataConditionsQuestNotEmpty() => Assert.True(AppData.ServerDatabase.QuestsData.Any(x => x.Value.Conditions.AvailableForStart.Any(y => y.Type == QuestConditionType.Quest)));

        [Test]
        public void QuestsDataConditionsTraderLoyaltyNotEmpty() => Assert.True(AppData.ServerDatabase.QuestsData.Any(x => x.Value.Conditions.AvailableForStart.Any(y => y.Type == QuestConditionType.TraderLoyalty)));

        [Test]
        public void QuestsDataConditionsCompareMethodGreaterOrEqualNotEmpty() => Assert.True(AppData.ServerDatabase.QuestsData.Any(x => x.Value.Conditions.AvailableForStart.Any(y => y.Props.CompareMethod == ">=")));

        [Test]
        public void QuestsDataConditionsCompareMethodNullNotEmpty() => Assert.True(AppData.ServerDatabase.QuestsData.Any(x => x.Value.Conditions.AvailableForStart.Any(y => y.Props.CompareMethod == null)));

        [Test]
        public void HideoutAreaInfosNotNul() => Assert.IsNotNull(AppData.ServerDatabase.HideoutAreaInfos);

        [Test]
        public void HideoutAreaInfosDataNotEmpty() => Assert.IsFalse(AppData.ServerDatabase.HideoutAreaInfos.Count == 0);

        [Test]
        public void ItemsDBNotNul() => Assert.IsNotNull(AppData.ServerDatabase.ItemsDB);

        [Test]
        public void ItemsDBNotEmpty() => Assert.IsFalse(AppData.ServerDatabase.ItemsDB.Count == 0);

        [Test]
        public void PocketsNotNul() => Assert.IsNotNull(AppData.ServerDatabase.Pockets);

        [Test]
        public void PocketsNotEmpty() => Assert.IsFalse(AppData.ServerDatabase.Pockets.Count == 0);

        [Test]
        public void TraderSuitsNotNul() => Assert.IsNotNull(AppData.ServerDatabase.TraderSuits);

        [Test]
        public void TraderSuitsNotEmpty() => Assert.IsFalse(AppData.ServerDatabase.TraderSuits.Count == 0);

        [Test]
        public void SlotsCountCalculatesCorrectly() => Assert.AreEqual(4, AppData.ServerDatabase.ItemsDB["557ffd194bdc2d28148b457f"].SlotsCount);

        [Test]
        public void ItemsDBFilterForPistolCaseLoadCorrectly() => Assert.True(AppData.ServerDatabase.ItemsDB["567143bf4bdc2d1a0f8b4567"].Properties.Grids[0].Props.Filters[0].Filter.Length > 0);

        [Test]
        public void ItemsDBFilterLoadCorrectly() => Assert.True(AppData.ServerDatabase.ItemsDB.Values.Any(x => x.Properties?.Grids?.Any(y => (y.Props?.Filters?[0].Filter?.Length ?? 0) > 0) ?? false));

        [Test]
        public void HandbookNotNull() => Assert.IsNotNull(AppData.ServerDatabase.Handbook);

        [Test]
        public void HandbookCategoriesNotEmpty() => Assert.IsFalse(AppData.ServerDatabase.Handbook.Categories.Count == 0);

        [Test]
        public void HandbookItemsNotEmpty() => Assert.IsFalse(AppData.ServerDatabase.Handbook.Items.Count == 0);

        [Test]
        public void HandbookHelperCanInitialize() => Assert.NotNull(new HandbookHelper(AppData.ServerDatabase.Handbook.Categories,
                                                    AppData.ServerDatabase.ItemsDB,
                                                    AppData.ServerDatabase.ServerGlobals.GlobalBuilds));

        [Test]
        public void HandbookHelperCategoriesForItemsAddingNotEmpty() => Assert.IsTrue(AppData.ServerDatabase.HandbookHelper.CategoriesForItemsAdding.Any());

        [Test]
        public void HandbookHelperCategoriesForItemsAddingWithFilterNotEmpty() => Assert.IsTrue(AppData.ServerDatabase.HandbookHelper.CategoriesForItemsAddingWithFilter("5c093ca986f7740a1867ab12").Any());

        [Test]
        public void HandbookHelperCategoriesForItemsAddingHaveCategories() => Assert.IsTrue(AppData.ServerDatabase.HandbookHelper.CategoriesForItemsAdding.Any(x => x.Categories.Any()));

        [Test]
        public void HandbookHelperCategoriesForItemsAddingHaveItems() => Assert.IsTrue(AppData.ServerDatabase.HandbookHelper.CategoriesForItemsAdding.Any(x => x.Items.Any()));

        [Test]
        public void HandbookHelperCategoriesForItemsAddingHaveBitmapImages() => Assert.IsTrue(AppData.ServerDatabase.HandbookHelper.CategoriesForItemsAdding.Any(x => x.BitmapIcon != null));

        [Test]
        public void HandbookHelperCategoriesForItemsAddingHaveLocalizedNames() => Assert.IsTrue(AppData.ServerDatabase.HandbookHelper.CategoriesForItemsAdding.All(x => !string.IsNullOrEmpty(x.LocalizedName)));

        [Test]
        public void HandbookHelperCategoriesForItemsAddingHaveIconPath() => Assert.IsTrue(AppData.ServerDatabase.HandbookHelper.CategoriesForItemsAdding.Any(x => !string.IsNullOrEmpty(x.Icon)));

        [Test]
        public void HandbookHelperCategoriesForItemsAddingPrimaryNotHaveParentId() => Assert.IsTrue(AppData.ServerDatabase.HandbookHelper.CategoriesForItemsAdding.All(x => string.IsNullOrEmpty(x.ParentId)));

        [Test]
        public void HandbookHelperCategoriesForItemsAddingHaveNotHidden() => Assert.IsTrue(AppData.ServerDatabase.HandbookHelper.CategoriesForItemsAdding.All(x => x.IsNotHidden));

        [Test]
        public void HandbookHelperCategoriesForItemsAddingNotExpanded() => Assert.IsTrue(AppData.ServerDatabase.HandbookHelper.CategoriesForItemsAdding.All(x => !x.IsExpanded));
    }
}