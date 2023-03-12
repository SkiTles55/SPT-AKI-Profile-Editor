using NUnit.Framework;
using SPT_AKI_Profile_Editor.Core;
using SPT_AKI_Profile_Editor.Core.HelperClasses;
using SPT_AKI_Profile_Editor.Tests.Hepers;
using SPT_AKI_Profile_Editor.Views;
using System;
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
            Assert.That(File.Exists(testsWindowsDialogs.templateExportPath), Is.True, "Changes not saved to file");
            TemplateEntity templateEntity = TemplateEntity.Load(testsWindowsDialogs.templateExportPath);
            Assert.That(templateEntity, Is.Not.Null, "Load saving template failed");
        }

        [Test]
        public void CanLoadTemplate()
        {
            AppData.AppSettings.AutoAddMissingScavSkills = true;
            AppData.Profile.Load(TestHelpers.profileFile);
            var expectedChanges = TemplateEntity.Load(TestHelpers.template);
            Assert.That(expectedChanges, Is.Not.Null, "Test template is null");
            ChangesTabViewModel viewModel = new(new TestsWindowsDialogs(), new TestsWorker());
            viewModel.LoadTemplate.Execute(null);
            Assert.That(viewModel.ProfileChanges, Is.Not.Null, "ProfileChanges is null");
            Assert.That(TemplatesEqual(viewModel.ProfileChanges, expectedChanges), Is.True, "ProfileChanges not equal to test template");
        }

        private bool TemplatesEqual(TemplateEntity first, TemplateEntity second)
        {
            if (first.Id != second.Id)
                return false;
            if ((first.Values?.Count ?? 0) != (second.Values?.Count ?? 0))
                return false;

            for (int i = 0; i < (first.Values?.Count ?? 0); i++)
            {
                var firstValue = first.Values?.ElementAt(i);
                var secondValue = second.Values?.Where(x => x.Key == firstValue?.Key).FirstOrDefault();
                if (firstValue == null && secondValue == null)
                    continue;
                var firstType = firstValue!.Value.Value.GetType();
                if (firstValue?.Key != secondValue?.Key || firstValue!.Value.Value.CompareTo(Convert.ChangeType(secondValue!.Value.Value, firstType)) != 0)
                    return false;
            }

            if ((first.TemplateEntities?.Count ?? 0) != (second.TemplateEntities?.Count ?? 0))
                return false;

            for (int i = 0; i < (first.TemplateEntities?.Count ?? 0); i++)
            {
                if (!TemplatesEqual(first.TemplateEntities[i], second.TemplateEntities[i]))
                    return false;
            }

            return true;
        }
    }
}