using NUnit.Framework;
using SPT_AKI_Profile_Editor.Core;
using SPT_AKI_Profile_Editor.Tests.Hepers;
using System.Linq;

namespace SPT_AKI_Profile_Editor.Tests
{
    internal class ExtMethodsTests
    {
        [Test]
        public void IdNotEmpty() => Assert.IsNotEmpty(ExtMethods.GenerateNewId(new string[] { "testid" }));

        [Test]
        public void CanGenerateUniqueId()
        {
            TestHelpers.LoadDatabaseAndProfile();
            var idsArray = AppData.Profile.Characters.Pmc.Inventory.Items.Select(x => x.Id).ToList();
            var startCount = idsArray.Count;

            for (int i = 0; i < 150; i++)
                idsArray.Add(ExtMethods.GenerateNewId(idsArray));

            Assert.IsTrue(idsArray.Count == startCount + 150, "List od id's does't have new id's");
            Assert.IsTrue(idsArray.Count == idsArray.Distinct().Count(), "List od id's have duplicates");
        }
    }
}