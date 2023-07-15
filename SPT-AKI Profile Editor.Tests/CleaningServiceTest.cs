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
        private readonly string pmcModdedInventoryItemTpl = "pmcModdedInventoryItemObject";
        private readonly string pmcModdedInventoryItemId = "pmcModdedInventoryItemObjectId";
        private readonly string scavModdedInventoryItemTpl = "scavModdedInventoryItemObject";
        private readonly string scavModdedInventoryItemId = "scavModdedInventoryItemObjectId";
        private readonly string pmcModdedExaminedItemId = "pmcModdedExaminedItemId";
        private readonly string pmcModdedQuestQid = "pmcModdedQuestQid";
        private readonly string pmcModdedMerchantId = "pmcModdedMerchantId";

        [OneTimeSetUp]
        public void Setup()
        {
            PrepareTestProfile();
            TestHelpers.LoadDatabase();
        }

        [Test]
        public void CleaningServiceCanLoadEntitiesList()
        {
            AppData.Profile.Load(testProfilePath);
            var cleaningService = new CleaningService();
            cleaningService.LoadEntitiesList();
            CheckModdedEntityType(cleaningService, ModdedEntityType.PmcInventoryItem, pmcModdedInventoryItemId);
            CheckModdedEntityType(cleaningService, ModdedEntityType.ScavInventoryItem, scavModdedInventoryItemId);
            CheckModdedEntityType(cleaningService, ModdedEntityType.ExaminedItem, pmcModdedExaminedItemId);
            CheckModdedEntityType(cleaningService, ModdedEntityType.Quest, pmcModdedQuestQid);
            CheckModdedEntityType(cleaningService, ModdedEntityType.Merchant, pmcModdedMerchantId);
        }

        [Test]
        public void CleaningServiceCanRemovePmcInventoryItems()
            => CheckModdedEntityRemoveWithoutSave(ModdedEntityType.PmcInventoryItem, pmcModdedInventoryItemId);

        [Test]
        public void CleaningServiceCanRemoveScavInventoryItems()
            => CheckModdedEntityRemoveWithoutSave(ModdedEntityType.ScavInventoryItem, scavModdedInventoryItemId);

        [Test]
        public void CleaningServiceCanRemoveExaminedItems()
            => CheckModdedEntityRemoveWithoutSave(ModdedEntityType.ExaminedItem, pmcModdedExaminedItemId);

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
            var questObject = JObject.FromObject(questsObject[0]);
            questObject["qid"] = questQid;
            questsObject.LastOrDefault().AddAfterSelf(JObject.FromObject(questObject));
        }

        private static void AddModdedMerchant(JObject profileJObject, string character, string merchantId)
        {
            var tradersInfoObject = profileJObject.SelectToken("characters")[character].SelectToken("TradersInfo");
            var traderInfoObject = tradersInfoObject["54cb50c76803fa8b248b4571"].ToObject<CharacterTraderStanding>();
            tradersInfoObject[merchantId] = JObject.FromObject(traderInfoObject);
        }

        private static void CheckModdedEntityType(CleaningService cleaningService, ModdedEntityType type, string expectedItemId)
        {
            Assert.That(cleaningService.ModdedEntities.Any(),
                        Is.True,
                        "ModdedEntities list is empty");
            var moddedInventoryItems = cleaningService.ModdedEntities.Where(x => x.Type == type);
            Assert.That(moddedInventoryItems.Count(),
                        Is.EqualTo(1),
                        $"ModdedEntities count with type {type} not 1");
            Assert.That(moddedInventoryItems.All(x => !string.IsNullOrEmpty(x.Type.LocalizedName())),
                        Is.True,
                        $"LocalizedName for {type} type not loaded");
            Assert.That(moddedInventoryItems.FirstOrDefault().Id,
                        Is.EqualTo(expectedItemId),
                        $"Id for {type} type not correct");
        }

        private void CheckModdedEntityRemoveWithoutSave(ModdedEntityType type, string expectedItemId)
        {
            AppData.Profile.Load(testProfilePath);
            var cleaningService = new CleaningService();
            cleaningService.LoadEntitiesList();
            cleaningService.MarkAll(true, type);
            cleaningService.RemoveSelected(null, null);
            Assert.That(cleaningService.ModdedEntities.Where(x => x.Type == type), Is.Empty, $"{type} not removed from cleaningService.ModdedEntities");
            switch (type)
            {
                case ModdedEntityType.PmcInventoryItem:
                    Assert.That(AppData.Profile.Characters.Pmc.Inventory.Items.FirstOrDefault(x => x.Id == expectedItemId), Is.Null, $"{type} not removed from Pmc.Inventory.Items");
                    break;

                case ModdedEntityType.ScavInventoryItem:
                    Assert.That(AppData.Profile.Characters.Scav.Inventory.Items.FirstOrDefault(x => x.Id == expectedItemId), Is.Null, $"{type} not removed from Scav.Inventory.Items");
                    break;

                case ModdedEntityType.ExaminedItem:
                    Assert.That(AppData.Profile.Characters.Pmc.Encyclopedia.ContainsKey(expectedItemId), Is.False, $"{type} not removed from Pmc.Encyclopedia");
                    break;

                default:
                    Assert.Fail($"unsupported type: {type}");
                    break;
            }
        }

        private void PrepareTestProfile()
        {
            JsonSerializerSettings seriSettings = new() { Formatting = Formatting.Indented, Converters = new List<JsonConverter>() { new StringEnumConverterExt() } };
            JObject profileJObject = JObject.Parse(File.ReadAllText(TestHelpers.profileFile));
            AddModdedInventoryItem(profileJObject, "pmc", pmcModdedInventoryItemId, pmcModdedInventoryItemTpl);
            AddModdedInventoryItem(profileJObject, "scav", scavModdedInventoryItemId, scavModdedInventoryItemTpl);
            AddModdedExaminedItem(profileJObject, "pmc", pmcModdedExaminedItemId);
            AddModdedQuest(profileJObject, "pmc", pmcModdedQuestQid);
            AddModdedMerchant(profileJObject, "pmc", pmcModdedMerchantId);
            string json = JsonConvert.SerializeObject(profileJObject, seriSettings);
            File.WriteAllText(testProfilePath, json);
        }
    }
}