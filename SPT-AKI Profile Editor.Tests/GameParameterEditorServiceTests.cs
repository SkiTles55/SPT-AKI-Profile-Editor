using NUnit.Framework;
using SPT_AKI_Profile_Editor.Helpers;
using System;
using System.IO;

namespace SPT_AKI_Profile_Editor.Tests
{
    [TestFixture]
    public class GameParameterEditorServiceTests
    {
        [Test]
        public void UpdateBossChance_UpdatesAllLocationBaseJsonFiles()
        {
            var tempRoot = Path.Combine(Path.GetTempPath(), "GameParameterEditorServiceTests", Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(Path.Combine(tempRoot, "SPT", "SPT_Data", "database", "locations", "map1"));
            Directory.CreateDirectory(Path.Combine(tempRoot, "SPT", "SPT_Data", "database", "locations", "map2"));

            var fileA = Path.Combine(tempRoot, "SPT", "SPT_Data", "database", "locations", "map1", "base.json");
            var fileB = Path.Combine(tempRoot, "SPT", "SPT_Data", "database", "locations", "map2", "base.json");

            File.WriteAllText(fileA, "{\"BossChance\": 0.35}");
            File.WriteAllText(fileB, "{\"BossChance\": 0.5}");

            int updated = GameParameterEditorService.UpdateBossChance(tempRoot, 0.0f);

            Assert.That(updated, Is.EqualTo(2));
            Assert.That(File.ReadAllText(fileA), Does.Contain("\"BossChance\": 0"));
            Assert.That(File.ReadAllText(fileB), Does.Contain("\"BossChance\": 0"));

            Directory.Delete(tempRoot, true);
        }
    }
}
