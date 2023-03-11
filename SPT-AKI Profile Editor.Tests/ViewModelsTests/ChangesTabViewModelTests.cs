using NUnit.Framework;
using SPT_AKI_Profile_Editor.Core;
using SPT_AKI_Profile_Editor.Tests.Hepers;
using SPT_AKI_Profile_Editor.Views;
using System.IO;
using System.Linq;

namespace SPT_AKI_Profile_Editor.Tests.ViewModelsTests
{
    internal class ChangesTabViewModelTests
    {
        [OneTimeSetUp]
        public void Setup() => TestHelpers.LoadDatabase();

        [Test]
        public void CanInitialize()
        {
            ChangesTabViewModel viewModel = new(new TestsWindowsDialogs(), new TestsWorker());
            Assert.That(viewModel, Is.Not.Null, "ViewModel is null");
            Assert.That(viewModel.ProfileChanges, Is.Null, "ProfileChanges not null");
            Assert.That(viewModel.GetAllChanges, Is.Not.Null, "GetAllChanges is null");
            Assert.That(viewModel.LoadTemplate, Is.Not.Null, "LoadTemplate is null");
            Assert.That(viewModel.SaveAsTemplate, Is.Not.Null, "SaveAsTemplate is null");
            Assert.That(viewModel.HasChanges, Is.False, "Profile has changes");
        }

        [Test]
        public void CanGetAllChanges()
        {
            AppData.Profile.Load(TestHelpers.profileFile);
            ChangesTabViewModel viewModel = new(new TestsWindowsDialogs(), new TestsWorker());
            AppData.Profile.Characters.Pmc.Info.Level += 1;
            viewModel.GetAllChanges.Execute(null);
            Assert.That(viewModel.ProfileChanges, Is.Not.Null, "ProfileChanges null");
            Assert.That(viewModel.HasChanges, Is.True, "Profile has not changed");
        }

        [Test]
        public void CanSaveAsTemplate()
        {
            TestsWindowsDialogs testsWindowsDialogs = new();
            AppData.Profile.Load(TestHelpers.profileFile);
            ChangesTabViewModel viewModel = new(testsWindowsDialogs, new TestsWorker());
            AppData.Profile.Characters.Pmc.Info.Level += 1;
            AppData.Profile.Characters.Pmc.Info.Nickname = "testNickname";
            foreach (var skill in AppData.Profile.Characters.Pmc.Skills.Common.Take(5))
                skill.Progress += 300;
            viewModel.GetAllChanges.Execute(null);
            Assert.That(viewModel.ProfileChanges, Is.Not.Null, "ProfileChanges null");
            Assert.That(viewModel.HasChanges, Is.True, "Profile has not changed");
            viewModel.SaveAsTemplate.Execute(null);
            Assert.That(string.IsNullOrEmpty(testsWindowsDialogs.SavedFilePath), Is.False, "SavedFilePath is null");
            Assert.That(File.Exists(testsWindowsDialogs.SavedFilePath), Is.True, "Changes not saved to file");
        }
    }
}