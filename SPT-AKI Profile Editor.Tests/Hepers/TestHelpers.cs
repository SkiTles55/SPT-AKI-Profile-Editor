using SPT_AKI_Profile_Editor.Core;
using SPT_AKI_Profile_Editor.Core.Enums;
using SPT_AKI_Profile_Editor.Core.ProfileClasses;
using System;
using System.IO;

namespace SPT_AKI_Profile_Editor.Tests.Hepers
{
    internal class TestHelpers
    {
        public static readonly string profileFile = @"D:\AKI_3.3.0_AIO\user\profiles\f5598c54e1b36b8e743b4698.json";
        public static readonly string serverPath = @"D:\AKI_3.3.0_AIO";
        public static readonly string wrongServerPath = @"D:\WinSetupFromUSB";
        public static readonly string profileWithDuplicatedItems = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "testFiles", "profileWithDuplicatedItems.json");
        public static readonly string weaponBuild = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "testFiles", "testBuild.json");
        public static readonly string appDataPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TestAppData");
        public static readonly string fileDownloaderTestUrl = "https://raw.githubusercontent.com/SkiTles55/SPT-AKI-Profile-Editor/master/FAQ.md";
        public static readonly string fileDownloaderTestSavePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "fileDownloaderTest.md");
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

        public static string GetTestName(string prefix, StashEditMode editMode)
        {
            return editMode switch
            {
                StashEditMode.PMC => $"{prefix}_Test_PMC",
                StashEditMode.Scav => $"{prefix}_Test_Scav",
                _ => $"{prefix}_Test_Unknown",
            };
        }

        public static void SetupTestCharacters(string prefix, StashEditMode editMode)
        {
            CharacterInventory pmcInventory = new()
            {
                Items = GenerateTestItems(3, GetTestName(prefix, editMode))
            };
            CharacterInventory scavInventory = new()
            {
                Items = GenerateTestItems(5, GetTestName(prefix, editMode))
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
    }
}