using NUnit.Framework;
using SPT_AKI_Profile_Editor.Core;
using SPT_AKI_Profile_Editor.Tests.Hepers;

namespace SPT_AKI_Profile_Editor.Tests
{
    internal class ServerConfigsTests
    {
        [OneTimeSetUp]
        public void Setup() => TestHelpers.LoadDatabase();

        [Test]
        public void QuestConfigLoadsCorrectly()
        {
            Assert.That(AppData.ServerConfigs.Quest, Is.Not.Null, "Quest config is null");
            Assert.That(AppData.ServerConfigs.Quest.EventQuests, Is.Not.Empty, "EventQuests is empty");
        }
    }
}