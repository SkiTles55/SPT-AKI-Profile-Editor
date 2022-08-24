using NUnit.Framework;
using SPT_AKI_Profile_Editor.Core;
using SPT_AKI_Profile_Editor.Tests.Hepers;
using SPT_AKI_Profile_Editor.Views;

namespace SPT_AKI_Profile_Editor.Tests.ViewModelsTests
{
    internal class IssuesDialogViewModelTests
    {
        [Test]
        public void CanInitialize()
        {
            IssuesDialogViewModel viewModel = new(null, new TestsIssuesService(), null);
            Assert.That(viewModel, Is.Not.Null);
            Assert.That(viewModel.SaveCommand, Is.Null);
            Assert.That(viewModel.IssuesService, Is.Not.Null);
            Assert.That(viewModel.FixCommand, Is.Not.Null);
            Assert.That(viewModel.IgnoreCommand, Is.Not.Null);
            Assert.That(viewModel.FixAllCommand, Is.Not.Null);
            Assert.That(viewModel.RemeberAction, Is.False);
        }

        [Test]
        public void CanExecuteSaveCommand()
        {
            var saveCommandExecuted = false;
            IssuesDialogViewModel viewModel = new(new(obj => saveCommandExecuted = true), new TestsIssuesService(), null);
            viewModel.SaveCommand.Execute(null);
            Assert.That(saveCommandExecuted, Is.True, "SaveCommand not executed");
        }

        [Test]
        public void CanExecuteFixCommand()
        {
            var fixActionExecuted = false;
            var issuesService = new TestsIssuesService();
            var testIssue = new TestsProfileIssue(new(() => fixActionExecuted = true), "testIssue");
            issuesService.ProfileIssues.Add(testIssue);
            IssuesDialogViewModel viewModel = new(null, issuesService, null);
            viewModel.FixCommand.Execute(testIssue.FixAction);
            Assert.That(fixActionExecuted, Is.True, "FixCommand not executed");
        }

        [Test]
        public void CanSkipSaveCommandAfterFixCommand()
        {
            var saveCommandExecuted = false;
            var fixActionExecuted = false;
            var issuesService = new TestsIssuesService();
            var testIssue = new TestsProfileIssue(new(() => fixActionExecuted = true), "testIssue");
            issuesService.ProfileIssues.Add(testIssue);
            IssuesDialogViewModel viewModel = new(new(obj => saveCommandExecuted = true), issuesService, null);
            viewModel.FixCommand.Execute(testIssue.FixAction);
            Assert.That(fixActionExecuted, Is.True, "FixCommand not executed");
            Assert.That(saveCommandExecuted, Is.False, "SaveCommand executed");
        }

        [Test]
        public void CanExecuteSaveCommandAfterFixCommand()
        {
            var saveCommandExecuted = false;
            var fixActionExecuted = false;
            var issuesService = new TestsIssuesService();
            var testIssue = new TestsProfileIssue(new(() =>
            {
                fixActionExecuted = true;
                issuesService.ProfileIssues.Clear();
            }), "testIssue");
            issuesService.ProfileIssues.Add(testIssue);
            IssuesDialogViewModel viewModel = new(new(obj => saveCommandExecuted = true), issuesService, null);
            viewModel.FixCommand.Execute(testIssue.FixAction);
            Assert.That(fixActionExecuted, Is.True, "FixCommand not executed");
            Assert.That(saveCommandExecuted, Is.True, "SaveCommand not executed");
        }

        [Test]
        public void CanExecuteIgnoreCommand()
        {
            var saveCommandExecuted = false;
            var fixActionExecuted = false;
            var issuesService = new TestsIssuesService();
            var testIssue = new TestsProfileIssue(new(() => fixActionExecuted = true), "testIssue");
            issuesService.ProfileIssues.Add(testIssue);
            IssuesDialogViewModel viewModel = new(new(obj => saveCommandExecuted = true), issuesService, null);
            viewModel.IgnoreCommand.Execute(null);
            Assert.That(fixActionExecuted, Is.False, "FixCommand executed");
            Assert.That(saveCommandExecuted, Is.True, "SaveCommand not executed");
        }

        [Test]
        public void CanExecuteFixAllCommand()
        {
            var saveCommandExecuted = false;
            var fixActionExecuted = false;
            var issuesService = new TestsIssuesService();
            var testIssue = new TestsProfileIssue(new(() =>
            {
                fixActionExecuted = true;
                issuesService.ProfileIssues.Clear();
            }), "testIssue");
            issuesService.ProfileIssues.Add(testIssue);
            IssuesDialogViewModel viewModel = new(new(obj => saveCommandExecuted = true), issuesService, null);
            viewModel.FixAllCommand.Execute(null);
            Assert.That(fixActionExecuted, Is.True, "FixCommand not executed");
            Assert.That(saveCommandExecuted, Is.True, "SaveCommand not executed");
        }

        [Test]
        public void CanRemeberAction()
        {
            var saveCommandExecuted = false;
            AppData.AppSettings.IssuesAction = Core.Enums.IssuesAction.AlwaysShow;
            var issuesService = new TestsIssuesService();
            IssuesDialogViewModel viewModel = new(new(obj => saveCommandExecuted = true), issuesService, null)
            {
                RemeberAction = true
            };
            viewModel.FixAllCommand.Execute(null);
            Assert.That(saveCommandExecuted, Is.True, "SaveCommand not executed");
            Assert.That(AppData.AppSettings.IssuesAction, Is.EqualTo(Core.Enums.IssuesAction.AlwaysFix), "RemeberAction skipped");
        }

        [Test]
        public void CanSkipRemeberAction()
        {
            var saveCommandExecuted = false;
            AppData.AppSettings.IssuesAction = Core.Enums.IssuesAction.AlwaysShow;
            var issuesService = new TestsIssuesService();
            IssuesDialogViewModel viewModel = new(new(obj => saveCommandExecuted = true), issuesService, null)
            {
                RemeberAction = false
            };
            viewModel.IgnoreCommand.Execute(null);
            Assert.That(saveCommandExecuted, Is.True, "SaveCommand not executed");
            Assert.That(AppData.AppSettings.IssuesAction, Is.EqualTo(Core.Enums.IssuesAction.AlwaysShow), "RemeberAction not skipped");
        }
    }
}