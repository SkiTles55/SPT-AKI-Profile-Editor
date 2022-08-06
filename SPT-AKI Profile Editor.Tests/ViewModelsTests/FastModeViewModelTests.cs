using NUnit.Framework;
using SPT_AKI_Profile_Editor.Core;
using SPT_AKI_Profile_Editor.Core.Enums;
using SPT_AKI_Profile_Editor.Tests.Hepers;
using SPT_AKI_Profile_Editor.Views;

namespace SPT_AKI_Profile_Editor.Tests.ViewModelsTests
{
    internal class FastModeViewModelTests
    {
        [Test]
        public void CanInitialize()
        {
            FastModeViewModel viewModel = new();
            Assert.That(viewModel, Is.Not.Null);
            Assert.That(viewModel.Pmc, Is.Not.Null);
            Assert.That(viewModel.Scav, Is.Not.Null);
            Assert.That(viewModel.SetMerchantsMax, Is.True);
            Assert.That(viewModel.SetAllQuestsValue, Is.EqualTo(QuestStatus.Success));
            Assert.That(viewModel.SetHideoutMax, Is.True);
            Assert.That(viewModel.SetAllPmcSkillsValue, Is.EqualTo(0f));
            Assert.That(viewModel.SetAllScavSkillsValue, Is.EqualTo(0f));
            Assert.That(viewModel.SetAllPmcMasteringsValue, Is.EqualTo(0f));
            Assert.That(viewModel.SetAllScavMasteringsValue, Is.EqualTo(0f));
            Assert.That(viewModel.ExamineAll, Is.True);
            Assert.That(viewModel.AcquireAll, Is.True);
        }

        [Test]
        public void HasNeededData()
        {
            Assert.That(FastModeViewModel.AppSettings, Is.Not.Null);
            Assert.That(FastModeViewModel.QuestStatuses, Is.Not.Empty);
        }

        [Test]
        public void CanDidOpenningRefresh()
        {
            AppData.LoadDatabase();
            FastModeViewModel viewModel = new();
            viewModel.OpenningRefresh.Execute(null);
            Assert.That(viewModel, Is.Not.Null);
            Assert.That(viewModel.Pmc.Experience, Is.GreaterThan(0));
            Assert.That(viewModel.Scav.Experience, Is.GreaterThan(0));
            Assert.That(viewModel.SetAllPmcSkillsValue, Is.GreaterThan(0f));
            Assert.That(viewModel.SetAllScavSkillsValue, Is.GreaterThan(0f));
            Assert.That(viewModel.SetAllPmcMasteringsValue, Is.GreaterThan(0f));
            Assert.That(viewModel.SetAllScavMasteringsValue, Is.GreaterThan(0f));
        }

        [Test]
        public void CanSaveProfile()
        {
            AppData.AppSettings.ServerPath = TestConstants.serverPath;
            AppData.LoadDatabase();
            AppData.Profile.Load(TestConstants.profileFile);
            FastModeViewModel viewModel = new();
            viewModel.OpenningRefresh.Execute(null);
            var expectedExp = viewModel.Pmc.Experience;
            Assert.That(AppData.Profile.Characters.Pmc.Info.Experience, Is.Not.EqualTo(expectedExp));
            viewModel.SaveProfile.Execute(null);
            Assert.That(AppData.Profile.Characters.Pmc.Info.Experience, Is.EqualTo(expectedExp));
        }
    }
}