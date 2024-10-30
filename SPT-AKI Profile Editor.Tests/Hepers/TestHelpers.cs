using Newtonsoft.Json;
using SPT_AKI_Profile_Editor.Core;
using SPT_AKI_Profile_Editor.Core.ProfileClasses;
using SPT_AKI_Profile_Editor.Helpers;
using System;
using System.Collections.Generic;
using System.IO;

namespace SPT_AKI_Profile_Editor.Tests.Hepers
{
    internal class TestHelpers
    {
        public static readonly JsonSerializerSettings seriSettings = new() { Formatting = Formatting.Indented, Converters = new List<JsonConverter>() { new StringEnumConverterExt() } };
        public static readonly string profileFile = @"D:\SPT\user\profiles\670243b60004ce099760c812.json";
        public static readonly string serverPath = @"D:\SPT";
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
    }
}