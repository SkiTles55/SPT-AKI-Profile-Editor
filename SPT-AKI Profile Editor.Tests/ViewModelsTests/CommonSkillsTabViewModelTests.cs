using NUnit.Framework;
using SPT_AKI_Profile_Editor.Core;
using SPT_AKI_Profile_Editor.Tests.Hepers;
using SPT_AKI_Profile_Editor.Views;
using System.Linq;

namespace SPT_AKI_Profile_Editor.Tests.ViewModelsTests
{
    internal class CommonSkillsTabViewModelTests
    {
        [OneTimeSetUp]
        public void Setup() => TestHelpers.LoadDatabase();

        [Test]
        public void CanInitialize()
        {
            CommonSkillsTabViewModel viewModel = new(null, null, null, null, null);
            Assert.That(viewModel, Is.Not.Null);
            Assert.That(viewModel.MaxSkillsValue, Is.EqualTo(AppData.AppSettings.CommonSkillMaxValue));
            Assert.That(viewModel.SetAllPmcSkillsValue, Is.EqualTo(0f));
            Assert.That(viewModel.SetAllScavSkillsValue, Is.EqualTo(0f));
            Assert.That(viewModel.SetAllPmsSkillsCommand, Is.Not.Null);
            Assert.That(viewModel.SetAllScavSkillsCommand, Is.Not.Null);
            Assert.That(viewModel.OpenSettingsCommand, Is.Not.Null);
        }

        [Test]
        public void CantSetAllPmcSkillsValueGreatherThanInServerDatabase()
        {
            CommonSkillsTabViewModel viewModel = new(null, null, null, null, null)
            {
                SetAllPmcSkillsValue = float.MaxValue
            };
            Assert.That(viewModel.SetAllPmcSkillsValue, Is.EqualTo(AppData.AppSettings.CommonSkillMaxValue));
        }

        [Test]
        public void CantSetAllScavSkillsValueGreatherThanInServerDatabase()
        {
            CommonSkillsTabViewModel viewModel = new(null, null, null, null, null)
            {
                SetAllScavSkillsValue = float.MaxValue
            };
            Assert.That(viewModel.SetAllScavSkillsValue, Is.EqualTo(AppData.AppSettings.CommonSkillMaxValue));
        }

        [Test]
        public void CanExecuteSetAllPmsSkillsCommand()
        {
            AppData.AppSettings.AutoAddMissingMasterings = true;
            AppData.Profile.Load(TestHelpers.profileFile);
            CommonSkillsTabViewModel viewModel = new(null, null, null, null, null)
            {
                SetAllPmcSkillsValue = 200f
            };
            viewModel.SetAllPmsSkillsCommand.Execute(null);
            Assert.That(AppData.Profile.Characters.Pmc.Skills.Common.Where(x => !x.Id.ToLower().StartsWith("bot")).All(x => x.Progress == 200f), Is.True);
            Assert.That(AppData.Profile.Characters.Pmc.Skills.Common.Where(x => x.Id.ToLower().StartsWith("bot")).All(x => x.Progress == 0f), Is.True, "Bot skills progress not zero");
        }

        [Test]
        public void CanExecuteSetAllScavSkillsCommand()
        {
            AppData.AppSettings.AutoAddMissingMasterings = true;
            AppData.Profile.Load(TestHelpers.profileFile);
            CommonSkillsTabViewModel viewModel = new(null, null, null, null, null)
            {
                SetAllScavSkillsValue = 200f
            };
            viewModel.SetAllScavSkillsCommand.Execute(null);
            Assert.That(AppData.Profile.Characters.Scav.Skills.Common.Where(x => !x.Id.ToLower().StartsWith("bot")).All(x => x.Progress == 200f), Is.True);
            Assert.That(AppData.Profile.Characters.Scav.Skills.Common.Where(x => x.Id.ToLower().StartsWith("bot")).All(x => x.Progress == 0f), Is.True, "Bot skills progress not zero");
        }

        [Test]
        public void CanExecuteOpenSettingsCommand()
        {
            TestsDialogManager dialogManager = new();
            CommonSkillsTabViewModel viewModel = new(dialogManager, null, null, null, null);
            viewModel.OpenSettingsCommand.Execute(null);
            Assert.That(dialogManager.SettingsDialogOpened, Is.True);
        }
    }
}