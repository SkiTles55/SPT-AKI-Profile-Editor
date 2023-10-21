using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using SPT_AKI_Profile_Editor.Core;
using SPT_AKI_Profile_Editor.Core.Enums;
using SPT_AKI_Profile_Editor.Core.ProfileClasses;
using SPT_AKI_Profile_Editor.Helpers;
using SPT_AKI_Profile_Editor.Tests.Hepers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SPT_AKI_Profile_Editor.Tests
{
    internal class CleaningServiceTest
    {
        private readonly string testProfilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "testProfileWithModdedItems.json");
        private readonly string testSaveProfilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "testSaveProfileWithModdedItems.json");
        private readonly string pmcModdedInventoryItemTpl = "pmcModdedInventoryItemObject";
        private readonly string pmcModdedInventoryItemId = "pmcModdedInventoryItemObjectId";
        private readonly string scavModdedInventoryItemTpl = "scavModdedInventoryItemObject";
        private readonly string scavModdedInventoryItemId = "scavModdedInventoryItemObjectId";
        private readonly string pmcModdedExaminedItemId = "pmcModdedExaminedItemId";
        private readonly string pmcModdedQuestQid = "pmcModdedQuestQid";
        private readonly string pmcModdedMerchantId = "pmcModdedMerchantId";
        private readonly string pmcModdedWeaponBuildId = "pmcModdedWeaponBuildId";
        private readonly string pmcModdedEquipmentBuildId = "pmcModdedEquipmentBuildId";

        [OneTimeSetUp]
        public void Setup()
        {
            PrepareTestProfile();
            TestHelpers.LoadDatabase();
        }

        [Test]
        public void CleaningServiceCanLoadEntitiesList()
        {
            var cleaningService = GetPreparedCleaningService();
            CheckModdedEntityType(cleaningService, ModdedEntityType.PmcInventoryItem, pmcModdedInventoryItemId);
            CheckModdedEntityType(cleaningService, ModdedEntityType.ScavInventoryItem, scavModdedInventoryItemId);
            CheckModdedEntityType(cleaningService, ModdedEntityType.ExaminedItem, pmcModdedExaminedItemId);
            CheckModdedEntityType(cleaningService, ModdedEntityType.Quest, pmcModdedQuestQid);
            CheckModdedEntityType(cleaningService, ModdedEntityType.Merchant, pmcModdedMerchantId);
            CheckModdedEntityType(cleaningService, ModdedEntityType.WeaponBuild, pmcModdedWeaponBuildId);
            CheckModdedEntityType(cleaningService, ModdedEntityType.EquipmentBuild, pmcModdedEquipmentBuildId);
        }

        [Test]
        public void CleaningServiceCanRemovePmcInventoryItems()
            => CheckModdedEntityRemove(ModdedEntityType.PmcInventoryItem, pmcModdedInventoryItemId);

        [Test]
        public void CleaningServiceCanRemoveScavInventoryItems()
            => CheckModdedEntityRemove(ModdedEntityType.ScavInventoryItem, scavModdedInventoryItemId);

        [Test]
        public void CleaningServiceCanRemoveExaminedItems()
            => CheckModdedEntityRemove(ModdedEntityType.ExaminedItem, pmcModdedExaminedItemId);

        [Test]
        public void CleaningServiceCanRemoveQuest()
            => CheckModdedEntityRemove(ModdedEntityType.Quest, pmcModdedQuestQid);

        [Test]
        public void CleaningServiceCanRemoveMerchant()
            => CheckModdedEntityRemove(ModdedEntityType.Merchant, pmcModdedMerchantId);

        [Test]
        public void CleaningServiceCanRemoveWeaponBuild()
            => CheckModdedEntityRemove(ModdedEntityType.WeaponBuild, pmcModdedWeaponBuildId);

        [Test]
        public void CleaningServiceCanRemoveEquipmentBuild()
            => CheckModdedEntityRemove(ModdedEntityType.EquipmentBuild, pmcModdedEquipmentBuildId);

        [Test]
        public void CleaningServiceCanOperateWithSelection()
        {
            AppData.Profile.Load(testProfilePath);
            var cleaningService = new CleaningService();
            Assert.That(cleaningService.CanDeselectAny, Is.False, "Deselect enabled while entities list not loaded");
            Assert.That(cleaningService.CanSelectAll, Is.False, "SelectAll enabled while entities list not loaded");
            cleaningService.LoadEntitiesList();
            Assert.That(cleaningService.CanDeselectAny, Is.False, "Deselect enabled while no elements selected");
            Assert.That(cleaningService.CanSelectAll, Is.True, "SelectAll not enabled while all elements not selected");
            cleaningService.MarkAll(true, ModdedEntityType.Merchant);
            Assert.That(cleaningService.CanDeselectAny, Is.True, "Deselect not enabled after selecting 1 group");
            Assert.That(cleaningService.CanSelectAll, Is.True, "SelectAll not enabled while selected only 1 group");
            cleaningService.MarkAll(true);
            Assert.That(cleaningService.CanDeselectAny, Is.True, "Deselect not enabled after selecting all elements");
            Assert.That(cleaningService.CanSelectAll, Is.False, "SelectAll enabled while all elements selected");
            cleaningService.MarkAll(false);
            Assert.That(cleaningService.CanDeselectAny, Is.False, "Deselect enabled after unmark all objects");
            Assert.That(cleaningService.CanSelectAll, Is.True, "SelectAll not enabled after unmark all objects");
        }

        [Test]
        public void CleaningServiceRemovingCanBeCanceledByDialog()
        {
            bool saveCalled = false;
            var cleaningService = GetPreparedCleaningService();
            cleaningService.MarkAll(true);
            var expectedCount = cleaningService.ModdedEntities.Count;
            var dialogManager = new TestsDialogManager
            {
                YesNoDialogResult = false
            };
            RelayCommand saveCommand = new(obj =>
            {
                saveCalled = true;
            });
            cleaningService.RemoveSelected(saveCommand, dialogManager);
            Assert.That(saveCalled, Is.False, "Save profile called");
            Assert.That(cleaningService.ModdedEntities.Count, Is.EqualTo(expectedCount), "Some modded entities removed");
        }

        private static void AddModdedInventoryItem(JObject profileJObject, string character, string itemId, string itemTpl)
        {
            var inventoryJObject = profileJObject.SelectToken("characters")[character].SelectToken("Inventory");
            var inventoryItemObject = JObject.FromObject(inventoryJObject.SelectToken("items")[0]);
            inventoryItemObject["_id"] = itemId;
            inventoryItemObject["_tpl"] = itemTpl;
            inventoryJObject.SelectToken("items").LastOrDefault().AddAfterSelf(JObject.FromObject(inventoryItemObject));
        }

        private static void AddModdedExaminedItem(JObject profileJObject, string character, string itemId)
        {
            var encyclopedia = profileJObject.SelectToken("characters")[character].SelectToken("Encyclopedia").ToObject<Dictionary<string, bool>>();
            encyclopedia.Add(itemId, true);
            profileJObject.SelectToken("characters")[character].SelectToken("Encyclopedia").Replace(JToken.FromObject(encyclopedia));
        }

        private static void AddModdedQuest(JObject profileJObject, string character, string questQid)
        {
            var questsObject = profileJObject.SelectToken("characters")[character].SelectToken("Quests");
            if (!questsObject.HasValues)
            {
                var newQuests = new CharacterQuest[] { new CharacterQuest { Qid = questQid } };
                questsObject.Replace(JToken.FromObject(newQuests));
            }
            else
            {
                var questObject = JObject.FromObject(questsObject[0]);
                questObject["qid"] = questQid;
                questsObject.LastOrDefault().AddAfterSelf(JObject.FromObject(questObject));
            }
        }

        private static void AddModdedMerchant(JObject profileJObject, string character, string merchantId)
        {
            var tradersInfoObject = profileJObject.SelectToken("characters")[character].SelectToken("TradersInfo");
            var traderInfoObject = tradersInfoObject["54cb50c76803fa8b248b4571"].ToObject<CharacterTraderStanding>();
            tradersInfoObject[merchantId] = JObject.FromObject(traderInfoObject);
        }

        private static void AddModdedWeaponBuild(JObject profileJObject, string weaponBuildId)
        {
            WeaponBuild weaponBuild = JsonConvert.DeserializeObject<WeaponBuild>(File.ReadAllText(TestHelpers.moddedWeaponBuild));
            weaponBuild.Id = weaponBuildId;
            List<WeaponBuild> moddedBuilds = new() { weaponBuild };
            UserBuilds builds = profileJObject.SelectToken("userbuilds")?.ToObject<UserBuilds>() ?? new();
            builds.WeaponBuilds = moddedBuilds;
            profileJObject.SelectToken("userbuilds").Replace(JObject.FromObject(builds).RemoveNullAndEmptyProperties());
        }

        private static void AddModdedEquipmentBuild(JObject profileJObject, string equipmentBuildId)
        {
            EquipmentBuild equipmentBuild = JsonConvert.DeserializeObject<EquipmentBuild>(File.ReadAllText(TestHelpers.moddedWeaponBuild));
            equipmentBuild.Id = equipmentBuildId;
            List<EquipmentBuild> moddedBuilds = new() { equipmentBuild };
            UserBuilds builds = profileJObject.SelectToken("userbuilds")?.ToObject<UserBuilds>() ?? new();
            builds.EquipmentBuilds = moddedBuilds;
            profileJObject.SelectToken("userbuilds").Replace(JObject.FromObject(builds).RemoveNullAndEmptyProperties());
        }

        private static void CheckModdedEntityType(CleaningService cleaningService, ModdedEntityType type, string expectedItemId)
        {
            Assert.That(cleaningService.ModdedEntities.Any(),
                        Is.True,
                        "ModdedEntities list is empty");
            var moddedInventoryItems = cleaningService.ModdedEntities.Where(x => x.Type == type);
            Assert.That(moddedInventoryItems.Count(),
                        Is.GreaterThanOrEqualTo(1),
                        $"ModdedEntities count with type {type} not 1");
            Assert.That(moddedInventoryItems.All(x => !string.IsNullOrEmpty(x.Type.LocalizedName())),
                        Is.True,
                        $"LocalizedName for {type} type not loaded");
            Assert.That(moddedInventoryItems.Any(x => x.Id == expectedItemId),
                        Is.True,
                        $"Id for {type} type not correct");
        }

        private static void CheckRemovingResult(CleaningService cleaningService, ModdedEntityType type, string expectedItemId)
        {
            Assert.That(cleaningService.ModdedEntities.Where(x => x.Type == type),
                        Is.Empty,
                        $"{type} not removed from cleaningService.ModdedEntities");
            switch (type)
            {
                case ModdedEntityType.PmcInventoryItem:
                    Assert.That(AppData.Profile.Characters.Pmc.Inventory.Items.FirstOrDefault(x => x.Id == expectedItemId),
                                Is.Null,
                                $"{type} not removed from Pmc.Inventory.Items");
                    break;

                case ModdedEntityType.ScavInventoryItem:
                    Assert.That(AppData.Profile.Characters.Scav.Inventory.Items.FirstOrDefault(x => x.Id == expectedItemId),
                                Is.Null,
                                $"{type} not removed from Scav.Inventory.Items");
                    break;

                case ModdedEntityType.ExaminedItem:
                    Assert.That(AppData.Profile.Characters.Pmc.Encyclopedia.ContainsKey(expectedItemId),
                                Is.False,
                                $"{type} not removed from Pmc.Encyclopedia");
                    break;

                case ModdedEntityType.Quest:
                    Assert.That(AppData.Profile.Characters.Pmc.Quests.FirstOrDefault(x => x.Qid == expectedItemId),
                                Is.Null,
                                $"{type} not removed from Pmc.Quests");
                    break;

                case ModdedEntityType.Merchant:
                    Assert.That(AppData.Profile.Characters.Pmc.TraderStandings.ContainsKey(expectedItemId),
                                Is.False,
                                $"{type} not removed from Pmc.TraderStandings");
                    break;

                case ModdedEntityType.WeaponBuild:
                    Assert.That(AppData.Profile.UserBuilds.WeaponBuilds.FirstOrDefault(x => x.Id == expectedItemId),
                                Is.Null,
                                $"{type} not removed from Profile.UserBuilds.WeaponBuilds");
                    break;

                case ModdedEntityType.EquipmentBuild:
                    Assert.That(AppData.Profile.UserBuilds.EquipmentBuilds.FirstOrDefault(x => x.Id == expectedItemId),
                                Is.Null,
                                $"{type} not removed from Profile.UserBuilds.EquipmentBuilds");
                    break;

                default:
                    Assert.Fail($"unsupported type: {type}");
                    break;
            }
        }

        private CleaningService GetPreparedCleaningService()
        {
            AppData.Profile.Load(testProfilePath);
            var cleaningService = new CleaningService();
            cleaningService.LoadEntitiesList();
            return cleaningService;
        }

        private void CheckModdedEntityRemove(ModdedEntityType type, string expectedItemId)
        {
            bool profileResaved = false;
            var cleaningService = GetPreparedCleaningService();
            cleaningService.MarkAll(true, type);
            RelayCommand saveCommand = new(obj =>
            {
                profileResaved = true;
                AppData.Profile.Save(testProfilePath, testSaveProfilePath);
                AppData.Profile.Load(testSaveProfilePath);
                cleaningService.LoadEntitiesList();
            });
            cleaningService.RemoveSelected(saveCommand, new TestsDialogManager());
            CheckRemovingResult(cleaningService, type, expectedItemId);
            Assert.That(profileResaved, Is.EqualTo(!type.CanBeRemovedWithoutSave()), $"Profile resaved for {type}. That not expected");
        }

        private void PrepareTestProfile()
        {
            JObject profileJObject = JObject.Parse(File.ReadAllText(TestHelpers.profileFile));
            AddModdedInventoryItem(profileJObject, "pmc", pmcModdedInventoryItemId, pmcModdedInventoryItemTpl);
            AddModdedInventoryItem(profileJObject, "scav", scavModdedInventoryItemId, scavModdedInventoryItemTpl);
            AddModdedExaminedItem(profileJObject, "pmc", pmcModdedExaminedItemId);
            AddModdedQuest(profileJObject, "pmc", pmcModdedQuestQid);
            AddModdedMerchant(profileJObject, "pmc", pmcModdedMerchantId);
            AddModdedWeaponBuild(profileJObject, pmcModdedWeaponBuildId);
            AddModdedEquipmentBuild(profileJObject, pmcModdedEquipmentBuildId);
            string json = JsonConvert.SerializeObject(profileJObject, TestHelpers.seriSettings);
            File.WriteAllText(testProfilePath, json);
        }
    }
}