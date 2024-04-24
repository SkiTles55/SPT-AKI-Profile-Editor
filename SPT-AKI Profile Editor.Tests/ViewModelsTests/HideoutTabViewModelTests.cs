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
        public void CanSetAllAreasMax()
        {
            TestHelpers.LoadDatabase();
            HideoutTabViewModel.SetAllMaxCommand.Execute(null);
            Assert.That(AppData.Profile.Characters.Pmc.Hideout.Areas.All(x => x.Level == x.MaxLevel || !x.CanSetMaxLevel),
                        Is.True);
        }

        [Test]
        public void CanAddAllCrafts()
        {
            TestHelpers.LoadDatabase();
            HideoutTabViewModel.AddAllCrafts.Execute(null);
            Assert.That(AppData.Profile.Characters.Pmc.HideoutProductions.All(x => x.Added), Is.True);
        }
    }
}