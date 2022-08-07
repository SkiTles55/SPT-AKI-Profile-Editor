using NUnit.Framework;
using SPT_AKI_Profile_Editor.Core;
using SPT_AKI_Profile_Editor.Core.Enums;
using SPT_AKI_Profile_Editor.Tests.Hepers;
using SPT_AKI_Profile_Editor.Views;
using System.Linq;

namespace SPT_AKI_Profile_Editor.Tests.ViewModelsTests
{
    internal class QuestsTabViewModelTests
    {
        [OneTimeSetUp]
        public void Setup() => TestConstants.LoadDatabaseAndProfile();

        [Test]
        public void CanInitialize()
        {
            QuestsTabViewModel viewModel = new(null);
            Assert.That(viewModel, Is.Not.Null);
            Assert.That(viewModel.OpenSettingsCommand, Is.Not.Null);
        }

        [Test]
        public void HasNeededData() => Assert.That(QuestsTabViewModel.SetAllCommand, Is.Not.Null);

        [Test]
        public void CanExecuteSetAllCommand()
        {
            QuestsTabViewModel.SetAllValue = QuestStatus.Fail;
            QuestsTabViewModel.SetAllCommand.Execute(null);
            Assert.That(AppData.Profile.Characters.Pmc.Quests.All(x => x.Status == QuestStatus.Fail), Is.True);
        }

        [Test]
        public void CanExecuteOpenSettingsCommand()
        {
            TestsDialogManager dialogManager = new();
            QuestsTabViewModel viewModel = new(dialogManager);
            viewModel.OpenSettingsCommand.Execute(null);
            Assert.That(dialogManager.SettingsDialogOpened, Is.True);
        }
    }
}