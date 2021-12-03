using NUnit.Framework;
using SPT_AKI_Profile_Editor.Core;
using System;

namespace SPT_AKI_Profile_Editor.Tests
{
    class UpdatesCheckerTests
    {
        const string oldRepository = "https://github.com/SkiTles55/SP-EFT-ProfileEditor/releases/latest";

        [Test]
        public void AppHaveUpdatesWithOldRepository() => Assert.IsTrue(UpdatesChecker.CheckUpdate(oldRepository, new Version(1, 8)));

        [Test]
        public void AppDoesntHaveUpdatesWithOldRepository() => Assert.IsFalse(UpdatesChecker.CheckUpdate(oldRepository, new Version(2, 2)));

        [Test]
        public void AppDoesntHaveUpdates() => Assert.IsFalse(UpdatesChecker.CheckUpdate());
    }
}