using NUnit.Framework;
using SPT_AKI_Profile_Editor.Core;
using SPT_AKI_Profile_Editor.Tests.Hepers;
using SPT_AKI_Profile_Editor.Views;
using System.Linq;

namespace SPT_AKI_Profile_Editor.Tests.ViewModelsTests
{
    internal class HideoutTabViewModelTests
    {
        [Test]
        public void CanInitialize()
        {
            HideoutTabViewModel viewModel = new();
            Assert.That(viewModel, Is.Not.Null);
        }

        [Test]
        public void CanSetAllMax()
        {
            TestConstants.LoadDatabase();
            HideoutTabViewModel.SetAllMaxCommand.Execute(null);
            Assert.That(AppData.Profile.Characters.Pmc.Hideout.Areas.All(x => x.Level == x.MaxLevel), Is.True);
        }
    }
}