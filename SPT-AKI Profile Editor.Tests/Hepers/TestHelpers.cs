using Newtonsoft.Json;
using SPT_AKI_Profile_Editor.Core;
using SPT_AKI_Profile_Editor.Core.Enums;
using SPT_AKI_Profile_Editor.Core.HelperClasses;
using SPT_AKI_Profile_Editor.Core.ProfileClasses;
using SPT_AKI_Profile_Editor.Core.ServerClasses;
using SPT_AKI_Profile_Editor.Helpers;
using System;
using System.Collections.Generic;
using System.IO;

namespace SPT_AKI_Profile_Editor.Tests.Hepers
{
    internal class TestHelpers
    {
        public static readonly JsonSerializerSettings seriSettings = new() { Formatting = Formatting.Indented, Converters = [new StringEnumConverterExt()] };
        public static readonly string profileFile = @"E:\SPT\SPT_Runtime\user\profiles\692ab300864c24352c711a34.json";
        public static readonly string serverPath = @"E:\SPT";
        public static readonly string wrongServerPath = @"D:\WinSetupFromUSB";
        public static readonly string profileWithDuplicatedItems = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "testFiles", "profileWithDuplicatedItems.json");
        public static readonly string weaponBuild = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "testFiles", "testBuild.json");
        public static readonly string moddedWeaponBuild = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "testFiles", "testModdedBuild.json");
        public static readonly string moddedEquipmentBuild = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "testFiles", "testModdedEquipmentBuild.json");
        public static readonly string appDataPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TestAppData");
        public static readonly string fileDownloaderTestUrl = "https://raw.githubusercontent.com/SkiTles55/SPT-AKI-Profile-Editor/master/FAQ.md";
        public static readonly string fileDownloaderTestSavePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "fileDownloaderTest.md");
        public static readonly string equipmentBuild = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "testFiles", "testEquipmentBuild.json");
        public static readonly string profileProgress = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "testFiles", "testProfileProgress.json");
        private static App _app;

        static TestHelpers()
        {
            if (!Directory.Exists(appDataPath))
            {
                DirectoryInfo dir = new(appDataPath);
                dir.Create();
            }
            else
            {
                DirectoryInfo di = new(appDataPath);
                foreach (FileInfo file in di.GetFiles())
                    file.Delete();
                foreach (DirectoryInfo dir in di.GetDirectories())
                    dir.Delete(true);
            }
        }

        public static void SetupApp()
        {
            if (_app == null)
            {
                _app = new App();
                _app.InitializeComponent();
            }
        }

        public static InventoryItem[] GenerateTestItems(int count, string parentId)
        {
            var items = new InventoryItem[count];
            for (int i = 0; i < count; i++)
            {
                items[i] = new InventoryItem()
                {
                    Id = $"TestItem{i}",
                    ParentId = parentId,
                    Tpl = $"{parentId}_{i}"
                };
            }
            return items;
        }

        public static string GetTestName(string prefix, bool isPmcItem)
            => $"{prefix}_Test_{(isPmcItem ? "PMC" : "Scav")}";

        public static void SetupTestCharacters(string prefix)
        {
            CharacterInventory pmcInventory = new()
            {
                Items = GenerateTestItems(3, GetTestName(prefix, true))
            };
            CharacterInventory scavInventory = new()
            {
                Items = GenerateTestItems(5, GetTestName(prefix, false))
            };
            Character pmc = new()
            {
                Inventory = pmcInventory,
            };
            Character scav = new()
            {
                Inventory = scavInventory,
            };
            ProfileCharacters characters = new()
            {
                Pmc = pmc,
                Scav = scav
            };
            AppData.Profile.Characters = characters;
        }

        public static void LoadDatabaseAndProfile()
        {
            LoadDatabase();
            AppData.Profile.Load(profileFile);
        }

        public static void LoadDatabase()
        {
            AppData.AppSettings.ServerPath = serverPath;
            AppData.LoadDatabase();
        }

        public static void SaveAndLoadProfile(string filename)
        {
            string testFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, filename);
            AppData.Profile.Save(profileFile, testFile);
            AppData.Profile.Load(testFile);
        }

        public static CharacterInventory SetupOrganizerInventory()
        {
            AppData.ServerDatabase.LocalesGlobal = [];
            AppData.Profile.Characters = new ProfileCharacters { Pmc = new Character { Bonuses = [], StashRowsBonusCount = 0 } };
            AppData.ServerDatabase.ItemsDB = new Dictionary<string, TarkovItem>
            {
                ["stash_tpl"] = new("stash_tpl", new TarkovItemProperties { Width = 2, Height = 2, Grids = new[] { new Grid { Props = new GridProps { CellsH = 5, CellsV = 5 } } } }, "container_parent", "Item"),
                [OrganizerCollections.KeyOrganizerTpl] = new(OrganizerCollections.KeyOrganizerTpl, new TarkovItemProperties { Width = 2, Height = 2, StackMaxSize = 1, Grids = new[] { new Grid { Props = new GridProps { CellsH = 2, CellsV = 2, Filters = new[] { new Filters { Filter = new[] { "keycat" }, ExcludedFilter = [] } } } } } }, "container_parent", "Item"),
                ["k1"] = new("k1", new TarkovItemProperties { Width = 1, Height = 1, StackMaxSize = 1 }, "keycat", "Item"),
                ["k2"] = new("k2", new TarkovItemProperties { Width = 1, Height = 1, StackMaxSize = 1 }, "keycat", "Item"),
                ["k3"] = new("k3", new TarkovItemProperties { Width = 1, Height = 1, StackMaxSize = 1 }, "keycat", "Item"),
                ["k4"] = new("k4", new TarkovItemProperties { Width = 1, Height = 1, StackMaxSize = 1 }, "keycat", "Item"),
                ["k5"] = new("k5", new TarkovItemProperties { Width = 1, Height = 1, StackMaxSize = 1 }, "keycat", "Item")
            };
            return new CharacterInventory
            {
                Stash = "stash_id",
                Items = new[]
                {
                    new InventoryItem { Id = "stash_id", Tpl = "stash_tpl" },
                    new InventoryItem { Id = "organizer_vm_id", Tpl = OrganizerCollections.KeyOrganizerTpl, ParentId = "stash_id", Location = new ItemLocation { X = 0, Y = 0, R = ItemRotation.Horizontal } }
                }
            };
        }
    }
}