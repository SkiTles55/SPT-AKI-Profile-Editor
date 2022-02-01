using Newtonsoft.Json;
using NUnit.Framework;
using SPT_AKI_Profile_Editor.Core;
using SPT_AKI_Profile_Editor.Core.ProfileClasses;
using System;
using System.IO;
using System.Linq;

namespace SPT_AKI_Profile_Editor.Tests
{
    class ProfileTests
    {
        const string profileFile = @"C:\SPT\user\profiles\5d2e0e1f320b903988c78d91.json";

        [OneTimeSetUp]
        public void Setup()
        {
            AppData.AppSettings.ServerPath = @"C:\SPT";
            AppData.LoadDatabase();
            AppData.Profile.Load(profileFile);
        }

        [Test]
        public void CharactersNotNull() => Assert.IsNotNull(AppData.Profile.Characters);

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
        public void CustomizationNotNully() => Assert.IsNotNull(AppData.Profile.Characters.Pmc.Customization);

        [Test]
        public void HeadNotEmpty() => Assert.IsNotNull(AppData.Profile.Characters.Pmc.Customization.Head, "Head is empty");

        [Test]
        public void TraderStandingsNotEmpty() => Assert.IsFalse(AppData.Profile.Characters.Pmc.TraderStandings.Count == 0);

        [Test]
        public void TraderStandingsNotNull() => Assert.IsNotNull(AppData.Profile.Characters.Pmc.TraderStandings, "TraderStandings is null");

        [Test]
        public void QuestsNotNull() => Assert.IsNotNull(AppData.Profile.Characters.Pmc.Quests, "Quests is null");

        [Test]
        public void QuestsNotEmpty() => Assert.IsFalse(AppData.Profile.Characters.Pmc.Quests.Length == 0, "Quests is empty");

        [Test]
        public void EncyclopediaNotNull() => Assert.IsNotNull(AppData.Profile.Characters.Pmc.Encyclopedia, "Encyclopedia is null");

        [Test]
        public void EncyclopediaNotEmpty() => Assert.IsFalse(AppData.Profile.Characters.Pmc.Encyclopedia.Count == 0, "Encyclopedia is empty");

        [Test]
        public void HideoutNotNull() => Assert.IsNotNull(AppData.Profile.Characters.Pmc.Hideout, "Hideout is null");

        [Test]
        public void HideoutAreasNotEmpty() => Assert.IsFalse(AppData.Profile.Characters.Pmc.Hideout.Areas.Length == 0, "HideoutAreas is empty");

        [Test]
        public void PmcSkillsNotNull() => Assert.IsNotNull(AppData.Profile.Characters.Pmc.Skills, "Pmc skills is null");

        [Test]
        public void PmcCommonSkillsNotEmpty() => Assert.IsFalse(AppData.Profile.Characters.Pmc.Skills.Common.Length == 0, "Pmc CommonSkills is empty");

        [Test]
        public void PmcMasteringSkillsNotEmpty() => Assert.IsFalse(AppData.Profile.Characters.Pmc.Skills.Mastering.Length == 0, "Pmc MasteringSkills is empty");

        [Test]
        public void ScavSkillsNotNull() => Assert.IsNotNull(AppData.Profile.Characters.Scav.Skills, "Scav skills is null");

        [Test]
        public void ScavCommonSkillsNotEmpty() => Assert.IsFalse(AppData.Profile.Characters.Scav.Skills.Common.Length == 0, "Scav CommonSkills is empty");

        [Test]
        public void ScavMasteringSkillsNotEmpty() => Assert.IsFalse(AppData.Profile.Characters.Scav.Skills.Mastering.Length == 0, "Scav MasteringSkills is empty");

        [Test]
        public void InventoryNotNull() => Assert.IsNotNull(AppData.Profile.Characters.Pmc.Inventory);

        [Test]
        public void InventoryStashNotEmpty() => Assert.IsNotEmpty(AppData.Profile.Characters.Pmc.Inventory.Stash);

        [Test]
        public void InventoryItemsNotEmpty() => Assert.IsFalse(AppData.Profile.Characters.Pmc.Inventory.Items.Length == 0);

        [Test]
        public void PmcPocketsNotNull() => Assert.IsNotEmpty(AppData.Profile.Characters.Pmc.Inventory.Pockets);

        [Test]
        public void WeaponBuildsNotNull() => Assert.IsNotNull(AppData.Profile.WeaponBuilds);

        [Test]
        public void WeaponBuildsNotEmpty() => Assert.IsFalse(AppData.Profile.WeaponBuilds.Count == 0);

        [Test]
        public void ProfileSavesCorrectly()
        {
            AppData.AppSettings.AutoAddMissingQuests = false;
            AppData.AppSettings.AutoAddMissingMasterings = false;
            AppData.AppSettings.AutoAddMissingScavSkills = false;
            AppData.Profile.Load(profileFile);
            var expected = JsonConvert.DeserializeObject(File.ReadAllText(profileFile));
            string testFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "test.json");
            AppData.Profile.Save(profileFile, testFile);
            var result = JsonConvert.DeserializeObject(File.ReadAllText(testFile));
            Assert.AreEqual(expected.ToString(), result.ToString());
        }

        [Test]
        public void TradersSavesCorrectly()
        {
            AppData.Profile.Load(profileFile);
            AppData.ServerDatabase.SetAllTradersMax();
            string testFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "testTraders.json");
            AppData.Profile.Save(profileFile, testFile);
            AppData.Profile.Load(testFile);
            Assert.IsTrue(AppData.Profile.Characters.Pmc.TraderStandings
                .Where(x => x.Key != "ragfair")
                .All(x => x.Value.LoyaltyLevel == AppData.ServerDatabase.TraderInfos[x.Key].MaxLevel));

        }

        [Test]
        public void QuestsStatusesSavesCorrectly()
        {
            AppData.AppSettings.AutoAddMissingQuests = true;
            AppData.Profile.Load(profileFile);
            AppData.Profile.Characters.Pmc.SetAllQuests("Fail");
            string testFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "testQuests.json");
            AppData.Profile.Save(profileFile, testFile);
            AppData.Profile.Load(testFile);
            var test1 = AppData.Profile.Characters.Pmc.Quests.All(x => x.Status == "Fail");
            var test2 = AppData.Profile.Characters.Pmc.Quests.Length == AppData.ServerDatabase.QuestsData.Count;
            Assert.IsTrue(AppData.Profile.Characters.Pmc.Quests.All(x => x.Status == "Fail")
                && AppData.Profile.Characters.Pmc.Quests.Length == AppData.ServerDatabase.QuestsData.Count);
        }

        [Test]
        public void HideoutAreaLevelsSavesCorrectly()
        {
            AppData.Profile.Load(profileFile);
            AppData.Profile.Characters.Pmc.SetAllHideoutAreasMax();
            string testFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "testHideouts.json");
            AppData.Profile.Save(profileFile, testFile);
            AppData.Profile.Load(testFile);
            Assert.IsTrue(AppData.Profile.Characters.Pmc.Hideout.Areas
                .All(x => x.Level == x.MaxLevel));
        }

        [Test]
        public void PmcCommonSkillsSavesCorrectly()
        {
            AppData.Profile.Load(profileFile);
            AppData.Profile.Characters.Pmc.SetAllCommonSkills(AppData.AppSettings.CommonSkillMaxValue);
            string testFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "testPmcCommonSkills.json");
            AppData.Profile.Save(profileFile, testFile);
            AppData.Profile.Load(testFile);
            Assert.IsTrue(AppData.Profile.Characters.Pmc.Skills.Common
                .All(x => x.Id.StartsWith("Bot") || x.Progress == AppData.AppSettings.CommonSkillMaxValue));
        }

        [Test]
        public void ScavCommonSkillsSavesCorrectly()
        {
            AppData.AppSettings.AutoAddMissingScavSkills = true;
            AppData.Profile.Load(profileFile);
            AppData.Profile.Characters.Scav.SetAllCommonSkills(AppData.AppSettings.CommonSkillMaxValue);
            string testFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "testScavCommonSkills.json");
            AppData.Profile.Save(profileFile, testFile);
            AppData.Profile.Load(testFile);
            Assert.IsTrue(AppData.Profile.Characters.Scav.Skills.Common
                .All(x => x.Id.StartsWith("Bot") || x.Progress == x.MaxValue)
                && AppData.Profile.Characters.Scav.Skills.Common.Length == AppData.Profile.Characters.Pmc.Skills.Common.Length);
        }

        [Test]
        public void PmcMasteringSkillsSavesCorrectly()
        {
            AppData.AppSettings.AutoAddMissingMasterings = true;
            AppData.Profile.Load(profileFile);
            AppData.Profile.Characters.Pmc.SetAllMasteringsSkills(AppData.ServerDatabase.ServerGlobals.Config.Mastering.Max(x => x.Level2 + x.Level3));
            string testFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "testPmcMasteringSkills.json");
            AppData.Profile.Save(profileFile, testFile);
            AppData.Profile.Load(testFile);
            Assert.IsTrue(AppData.Profile.Characters.Pmc.Skills.Mastering
                .All(x => x.Progress == x.MaxValue)
                && AppData.Profile.Characters.Pmc.Skills.Mastering.Length == AppData.ServerDatabase.ServerGlobals.Config.Mastering.Length);
        }

        [Test]
        public void ScavMasteringSkillsSavesCorrectly()
        {
            AppData.AppSettings.AutoAddMissingMasterings = true;
            AppData.Profile.Load(profileFile);
            AppData.Profile.Characters.Scav.SetAllMasteringsSkills(AppData.ServerDatabase.ServerGlobals.Config.Mastering.Max(x => x.Level2 + x.Level3));
            string testFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "testScavMasteringSkills.json");
            AppData.Profile.Save(profileFile, testFile);
            AppData.Profile.Load(testFile);
            Assert.IsTrue(AppData.Profile.Characters.Scav.Skills.Mastering
                .All(x => x.Progress == x.MaxValue)
                && AppData.Profile.Characters.Scav.Skills.Mastering.Length == AppData.ServerDatabase.ServerGlobals.Config.Mastering.Length);
        }

        [Test]
        public void ExaminedItemsSavesCorrectly()
        {
            AppData.Profile.Load(profileFile);
            var expected = AppData.Profile.Characters.Pmc.ExaminedItems.Count;
            AppData.Profile.Characters.Pmc.ExamineAll();
            string testFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "testExaminedItems.json");
            AppData.Profile.Save(profileFile, testFile);
            AppData.Profile.Load(testFile);
            Assert.AreNotEqual(expected, AppData.Profile.Characters.Pmc.ExaminedItems.Count);
        }

        [Test]
        public void SuitsSavesCorrectly()
        {
            AppData.Profile.Load(profileFile);
            AppData.ServerDatabase.AcquireAllClothing();
            string testFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "testSuits.json");
            AppData.Profile.Save(profileFile, testFile);
            AppData.Profile.Load(testFile);
            Assert.IsTrue(AppData.Profile.Suits.Length == AppData.ServerDatabase.TraderSuits.Count);
        }

        [Test]
        public void PmcPocketsSavesCorrectly()
        {
            AppData.Profile.Load(profileFile);
            string expected = AppData.ServerDatabase.Pockets.Last().Key;
            AppData.Profile.Characters.Pmc.Inventory.Pockets = expected;
            string testFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "testPmcPockets.json");
            AppData.Profile.Save(profileFile, testFile);
            AppData.Profile.Load(testFile);
            Assert.IsTrue(AppData.Profile.Characters.Pmc.Inventory.Pockets == expected);
        }

        [Test]
        public void ScavPocketsSavesCorrectly()
        {
            AppData.Profile.Load(profileFile);
            string expected = AppData.ServerDatabase.Pockets.Last().Key;
            AppData.Profile.Characters.Scav.Inventory.Pockets = expected;
            string testFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "testScavPockets.json");
            AppData.Profile.Save(profileFile, testFile);
            AppData.Profile.Load(testFile);
            Assert.IsTrue(AppData.Profile.Characters.Scav.Inventory.Pockets == expected);
        }

        [Test]
        public void StashRemovingItemsSavesCorrectly()
        {
            AppData.Profile.Load(profileFile);
            string expectedId1 = AppData.Profile.Characters.Pmc.Inventory.InventoryItems[0].Id;
            string expectedId2 = AppData.Profile.Characters.Pmc.Inventory.InventoryItems[1].Id;
            AppData.Profile.Characters.Pmc.Inventory.RemoveItems(new() { expectedId1, expectedId2 });
            string testFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "testStashRemovingItems.json");
            AppData.Profile.Save(profileFile, testFile);
            AppData.Profile.Load(testFile);
            Assert.IsFalse(AppData.Profile.Characters.Pmc.Inventory.Items.Any(x => x.Id == expectedId1));
            Assert.IsFalse(AppData.Profile.Characters.Pmc.Inventory.Items.Any(x => x.Id == expectedId2));
        }

        [Test]
        public void StashRemovingAllItemsSavesCorrectly()
        {
            AppData.Profile.Load(profileFile);
            AppData.Profile.Characters.Pmc.Inventory.RemoveAllItems();
            string testFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "testStashRemovingAllItems.json");
            AppData.Profile.Save(profileFile, testFile);
            AppData.Profile.Load(testFile);
            Assert.AreEqual(0, AppData.Profile.Characters.Pmc.Inventory.InventoryItems.Length);
        }

        [Test]
        public void Stash2DMapCalculatingCorrectly()
        {
            AppData.Profile.Load(profileFile);
            var stash2d = AppData.Profile.Characters.Pmc.Inventory.GetPlayerStashSlotMap();
            Assert.AreNotEqual(new int[0, 0], stash2d);
            Assert.IsFalse(stash2d.Cast<int>().All(x => x == 0));
        }

        [Test]
        public void StashAddingItemsSavesCorrectly()
        {
            AppData.Profile.Load(profileFile);
            var largestItems = AppData.ServerDatabase.ItemsDB
                .Where(x => !AppData.Profile.Characters.Pmc.Inventory.InventoryItems
                .Any(y => y.Tpl == x.Key))
                .OrderByDescending(x => x.Value.Properties?.Width + x.Value.Properties?.Height)
                .ToArray();
            var item1 = largestItems[0];
            var item2 = largestItems[1];
            Assert.IsNotNull(item1);
            Assert.IsNotNull(item2);
            AppData.Profile.Characters.Pmc.Inventory.AddNewItems(item2.Key, 2, true);
            AppData.Profile.Characters.Pmc.Inventory.AddNewItems(item1.Key, 1, false);
            string testFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "testStashAddingItems.json");
            AppData.Profile.Save(profileFile, testFile);
            AppData.Profile.Load(testFile);
            var savedItems = AppData.Profile.Characters.Pmc.Inventory.InventoryItems
                .Where(x => x.Tpl == item1.Key || x.Tpl == item2.Key).ToArray();
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
        public void StashAddingMoneysSavesCorrectly()
        {
            AppData.Profile.Load(profileFile);
            var startValue = AppData.Profile.Characters.Pmc.Inventory.Items
                .Where(x => x.Tpl == AppData.AppSettings.MoneysRublesTpl)
                .Sum(x => x.Upd.StackObjectsCount ?? 0);
            AppData.Profile.Characters.Pmc.Inventory.AddNewItems(AppData.AppSettings.MoneysRublesTpl, 2000000, false);
            string testFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "testStashAddingMoneys.json");
            AppData.Profile.Save(profileFile, testFile);
            AppData.Profile.Load(testFile);
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
        public void WeaponBuildRemoveSavesCorrectly()
        {
            AppData.Profile.Load(profileFile);
            var expected = AppData.Profile.WeaponBuilds.FirstOrDefault().Key;
            AppData.Profile.RemoveBuild(expected);
            string testFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "testWeaponBuildsRemove.json");
            AppData.Profile.Save(profileFile, testFile);
            AppData.Profile.Load(testFile);
            Assert.IsFalse(AppData.Profile.WeaponBuilds.ContainsKey(expected));
        }

        [Test]
        public void WeaponBuildExportCorrectly()
        {
            AppData.Profile.Load(profileFile);
            var expected = AppData.Profile.WeaponBuilds.FirstOrDefault();
            string testFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "testWeaponBuildExport.json");
            AppData.Profile.ExportBuild(expected.Key, testFile);
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
            AppData.Profile.Load(profileFile);
            AppData.Profile.ImportBuild(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "testFiles", "testBuild.json"));
            AppData.Profile.ImportBuild(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "testFiles", "testBuild.json"));
            string testFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "testWeaponBuildsImport.json");
            AppData.Profile.Save(profileFile, testFile);
            AppData.Profile.Load(testFile);
            Assert.IsTrue(AppData.Profile.WeaponBuilds.ContainsKey("TestBuild"));
            Assert.AreEqual(2, AppData.Profile.WeaponBuilds.Where(x => x.Value.Name.StartsWith("Test")).Count());
        }

        [Test]
        public void ProfileNotChangedAfterLoading()
        {
            AppData.AppSettings.AutoAddMissingQuests = false;
            AppData.AppSettings.AutoAddMissingMasterings = false;
            AppData.AppSettings.AutoAddMissingScavSkills = false;
            AppData.Profile.Load(profileFile);
            Assert.IsFalse(ExtMethods.IsProfileChanged(AppData.Profile));
        }

        [Test]
        public void ProfileChangedAfterEditings()
        {
            AppData.AppSettings.AutoAddMissingQuests = false;
            AppData.AppSettings.AutoAddMissingMasterings = false;
            AppData.AppSettings.AutoAddMissingScavSkills = false;
            AppData.Profile.Load(profileFile);
            AppData.ServerDatabase.SetAllTradersMax();
            Assert.IsTrue(ExtMethods.IsProfileChanged(AppData.Profile));
        }

        [Test]
        public void ProfileCanRemoveDuplicatedItems()
        {
            AppData.Profile.Load(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "testFiles", "profileWithDuplicatedItems.json"));
            Assert.IsTrue(AppData.Profile.Characters.Pmc.Inventory.InventoryHaveDuplicatedItems);
            AppData.Profile.Characters.Pmc.Inventory.RemoveDuplicatedItems();
            Assert.IsFalse(AppData.Profile.Characters.Pmc.Inventory.InventoryHaveDuplicatedItems);
            string testFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "testRemoveDuplicatedItems.json");
            AppData.Profile.Save(profileFile, testFile);
            AppData.Profile.Load(testFile);
            Assert.IsFalse(AppData.Profile.Characters.Pmc.Inventory.InventoryHaveDuplicatedItems);
        }
    }
}