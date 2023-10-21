using NUnit.Framework;
using SPT_AKI_Profile_Editor.Core;
using SPT_AKI_Profile_Editor.Tests.Hepers;
using System.Linq;

namespace SPT_AKI_Profile_Editor.Tests
{
    internal class ServerConfigsTests
    {
        [OneTimeSetUp]
        public void Setup() => TestHelpers.LoadDatabase();

        [Test]
        public void QuestConfigLoadsCorrectly()
        {
            Assert.NotNull(AppData.ServerConfigs.Quest, "Quest config is null");
            Assert.True(AppData.ServerConfigs.Quest.EventQuests.Any(), "EventQuests is empty");
        }
    }
}