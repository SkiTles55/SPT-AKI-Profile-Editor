using SPT_AKI_Profile_Editor.Core;
using SPT_AKI_Profile_Editor.Core.Enums;
using SPT_AKI_Profile_Editor.Core.ProfileClasses;
using System;
using System.IO;

namespace SPT_AKI_Profile_Editor.Tests
{
    internal class TestConstants
    {
        public static readonly string profileFile = @"C:\SPT-AKI\user\profiles\51d161ff9b7af0eabbe0f320.json";

        public static readonly string serverPath = @"C:\SPT-AKI";

        public static readonly string profileWithDuplicatedItems = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "testFiles", "profileWithDuplicatedItems.json");

        public static readonly string weaponBuild = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "testFiles", "testBuild.json");

        public static InventoryItem[] GenerateTestItems(int count, string parentId)
        {
            var items = new InventoryItem[count];
            for (int i = 0; i < count; i++)
            {
                items[i] = new InventoryItem()
                {
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
    }
}