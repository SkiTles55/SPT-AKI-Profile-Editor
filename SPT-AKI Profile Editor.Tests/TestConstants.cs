﻿using System;
using System.IO;

namespace SPT_AKI_Profile_Editor.Tests
{
    internal class TestConstants
    {
        public static readonly string profileFile = @"C:\SPT\user\profiles\99ccaab4eee294d7f3b432af.json";

        public static readonly string serverPath = @"C:\SPT";

        public static readonly string profileWithDuplicatedItems = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "testFiles", "profileWithDuplicatedItems.json");
    }
}