using NUnit.Framework;
using SPT_AKI_Profile_Editor.Core.ProfileClasses;
using System;
using System.Linq;
using static SPT_AKI_Profile_Editor.Core.ProfileClasses.ProfileSaver;

namespace SPT_AKI_Profile_Editor.Tests.EnumsTests
{
    internal class SaveEntryTests
    {
        [Test]
        public void SaveEntryHasLocalizedNames()
        {
            foreach (var entry in Enum.GetValues(typeof(SaveEntry)).Cast<SaveEntry>())
            {
                var name = entry.LocalizedName();
                Assert.False(string.IsNullOrWhiteSpace(name), $"entry has incorrect name: {name}");
            }
        }
    }
}