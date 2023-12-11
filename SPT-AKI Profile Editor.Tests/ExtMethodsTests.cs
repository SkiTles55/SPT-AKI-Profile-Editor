using NUnit.Framework;
using SPT_AKI_Profile_Editor.Core;
using SPT_AKI_Profile_Editor.Tests.Hepers;
using System.Linq;

namespace SPT_AKI_Profile_Editor.Tests
{
    internal class ExtMethodsTests
    {
        [Test]
        public void IdNotEmpty()
            => Assert.That(ExtMethods.GenerateNewId(new string[] { "testid" }), Is.Not.Empty);

        [Test]
        public void CanGenerateUniqueId()
        {
            TestHelpers.LoadDatabaseAndProfile();
            var idsArray = AppData.Profile.Characters.Pmc.Inventory.Items.Select(x => x.Id).ToList();
            var startCount = idsArray.Count;

            for (int i = 0; i < 150; i++)
                idsArray.Add(ExtMethods.GenerateNewId(idsArray));

            Assert.That(idsArray.Count == startCount + 150,
                        Is.True,
                        "List od id's does't have new id's");
            Assert.That(idsArray.Count,
                        Is.EqualTo(idsArray.Distinct().Count()),
                        "List od id's have duplicates");
        }
    }
}