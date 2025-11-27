using NUnit.Framework;
using SPT_AKI_Profile_Editor.Core;
using SPT_AKI_Profile_Editor.Core.Enums;
using SPT_AKI_Profile_Editor.Helpers;
using SPT_AKI_Profile_Editor.Tests.Hepers;
using SPT_AKI_Profile_Editor.Views;

namespace SPT_AKI_Profile_Editor.Tests.ViewModelsTests
{
    internal class FastModeViewModelTests
    {
        [OneTimeSetUp]
        public void Setup() => TestHelpers.LoadDatabase();

        [Test]
        public void CanInitialize()
        {
            AppData.Profile.Load(TestHelpers.profileFile);
            FastModeViewModel viewModel = new(null);
            Assert.That(viewModel, Is.Not.Null);
            Assert.That(viewModel.Pmc, Is.Not.Null);
            Assert.That(viewModel.Scav, Is.Not.Null);
            Assert.That(viewModel.SetMerchantsMax, Is.True);
            Assert.That(viewModel.SetAllQuestsValue, Is.EqualTo(QuestStatus.Success));
            Assert.That(viewModel.SetHideoutMax, Is.True);
            Assert.That(viewModel.UnlockAllCrafts, Is.True);
            Assert.That(viewModel.ReceiveAllAchievements, Is.True);
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
            AppData.Profile.Load(TestHelpers.profileFile);
            FastModeViewModel viewModel = new(null);
            viewModel.OpenningRefresh.Execute(null);
            Assert.That(viewModel, Is.Not.Null);
            Assert.That(viewModel.Pmc.Experience, Is.GreaterThan(0));
            Assert.That(viewModel.Scav.Experience, Is.GreaterThan(0));
            Assert.That(viewModel.SetAllPmcSkillsValue, Is.Not.GreaterThan(AppData.AppSettings.CommonSkillMaxValue));
            Assert.That(viewModel.SetAllScavSkillsValue, Is.Not.GreaterThan(AppData.AppSettings.CommonSkillMaxValue));
            Assert.That(viewModel.SetAllPmcMasteringsValue, Is.Not.GreaterThan(AppData.ServerDatabase.ServerGlobals.Config.MaxProgressValue));
            Assert.That(viewModel.SetAllScavMasteringsValue, Is.Not.GreaterThan(AppData.ServerDatabase.ServerGlobals.Config.MaxProgressValue));
        }

        [Test]
        public void CanApplyAndSaveProfile() => CheckApplyExecution(true);

        [Test]
        public void CanApplyWithoutSave() => CheckApplyExecution(false);

        private static void CheckApplyExecution(bool withSave)
        {
            AppData.Profile.Load(TestHelpers.profileFile);
            bool saveExecuted = false;
            RelayCommand command = new((obj) => saveExecuted = true);
            FastModeViewModel viewModel = new(command);
            viewModel.OpenningRefresh.Execute(null);
            var expectedExp = viewModel.Pmc.Experience;
            Assert.That(AppData.Profile.Characters.Pmc.Info.Experience, Is.Not.EqualTo(expectedExp));
            viewModel.SaveProfile.Execute(withSave);
            Assert.That(AppData.Profile.Characters.Pmc.Info.Experience, Is.EqualTo(expectedExp));
            Assert.That(saveExecuted, Is.EqualTo(withSave));
        }
    }
}