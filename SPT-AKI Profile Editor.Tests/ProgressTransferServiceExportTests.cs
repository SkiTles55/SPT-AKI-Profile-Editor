using Newtonsoft.Json;
using NUnit.Framework;
using SPT_AKI_Profile_Editor.Core;
using SPT_AKI_Profile_Editor.Core.ProgressTransfer;
using SPT_AKI_Profile_Editor.Tests.Hepers;
using System;
using System.IO;

namespace SPT_AKI_Profile_Editor.Tests
{
    internal class ProgressTransferServiceExportTests
    {
        private readonly string pmcNickname = "ProgressTransferServiceTestPMCNickname";
        private readonly long pmcExperience = 19198744;

        private readonly string exportPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "testProgressExport.json");
        private readonly SettingsModel exportSettings = new();

        [OneTimeSetUp]
        public void Setup()
        {
            TestHelpers.LoadDatabaseAndProfile();
            AppData.Profile.Characters.Pmc.Info.Nickname = pmcNickname;
            AppData.Profile.Characters.Pmc.Info.Experience = pmcExperience;
        }

        [Test]
        public void CanExportAllSettings()
        {
            exportSettings.ChangeAll(true);
            ProgressTransferService.ExportProgress(exportSettings, AppData.Profile, exportPath);

            string fileText = File.ReadAllText(exportPath);
            ProfileProgress exportedProgress = JsonConvert.DeserializeObject<ProfileProgress>(fileText);

            Assert.That(exportedProgress, Is.Not.Null);
            Assert.That(exportedProgress.Info.Pmc.Nickname, Is.EqualTo(pmcNickname));
            Assert.That(exportedProgress.Info.Pmc.Experience, Is.EqualTo(pmcExperience));
        }
    }
}