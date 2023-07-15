using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NUnit.Framework;
using SPT_AKI_Profile_Editor.Core;
using SPT_AKI_Profile_Editor.Core.Enums;
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

        [OneTimeSetUp]
        public void Setup()
        {
            PrepareTestProfile();
            TestHelpers.LoadDatabase();
        }

        [Test]
        public void CleaningServiceCanLoadEntitiesListWithPmcInventoryItemType()
            => CheckInventoryItemType(ModdedEntityType.PmcInventoryItem, pmcModdedInventoryItemId);

        [Test]
        public void CleaningServiceCanLoadEntitiesListWithScavInventoryItemType()
            => CheckInventoryItemType(ModdedEntityType.ScavInventoryItem, scavModdedInventoryItemId);

        private static void AddModdedInventoryItem(JObject profileJObject, string character, string itemId, string itemTpl)
        {
            var inventoryJObject = profileJObject.SelectToken("characters")[character].SelectToken("Inventory");
            var inventoryItemObject = JObject.FromObject(inventoryJObject.SelectToken("items")[0]);
            inventoryItemObject["_id"] = itemId;
            inventoryItemObject["_tpl"] = itemTpl;
            inventoryJObject.SelectToken("items").LastOrDefault().AddAfterSelf(JObject.FromObject(inventoryItemObject));
        }

        private void CheckInventoryItemType(ModdedEntityType type, string expectedItemId)
        {
            AppData.Profile.Load(testProfilePath);
            var cleaningService = new CleaningService();
            cleaningService.LoadEntitiesList();
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

        private void PrepareTestProfile()
        {
            JsonSerializerSettings seriSettings = new() { Formatting = Formatting.Indented, Converters = new List<JsonConverter>() { new StringEnumConverterExt() } };
            JObject profileJObject = JObject.Parse(File.ReadAllText(TestHelpers.profileFile));
            AddModdedInventoryItem(profileJObject, "pmc", pmcModdedInventoryItemId, pmcModdedInventoryItemTpl);
            AddModdedInventoryItem(profileJObject, "scav", scavModdedInventoryItemId, scavModdedInventoryItemTpl);
            string json = JsonConvert.SerializeObject(profileJObject, seriSettings);
            File.WriteAllText(testProfilePath, json);
        }
    }
}