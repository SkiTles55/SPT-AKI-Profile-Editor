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
    }
}