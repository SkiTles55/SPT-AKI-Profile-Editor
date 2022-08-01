using NUnit.Framework;
using SPT_AKI_Profile_Editor.Core;
using SPT_AKI_Profile_Editor.Helpers;
using SPT_AKI_Profile_Editor.Views;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SPT_AKI_Profile_Editor.Tests.ViewModelsTests
{
    internal class ServerPathEditorViewModelTests
    {
        private AppSettings settings;

        [OneTimeSetUp]
        public void Setup()
        {
            settings = new(Path.Combine(TestConstants.AppDataPath, "AppSettings.json"));
            settings.Load();
        }

        [Test]
        public void ServerPathEditorViewModelWithRightPathInitializeCorrectly()
        {
            ServerPathEditorViewModel pathEditorViewModel = TestViewModel(settings.CheckServerPath(TestConstants.serverPath));
            Assert.That(pathEditorViewModel, Is.Not.Null, "ServerPathEditorViewModel is null");
            Assert.That(pathEditorViewModel.Paths.Any(), Is.True, "ServerPathEditorViewModel Paths is empty");
            Assert.That(pathEditorViewModel.Paths.All(x => x.IsFounded), Is.True, "ServerPathEditorViewModel Paths wrong values");
            var expectedKeys = settings.FilesList.Select(x => x.Key).Concat(settings.DirsList.Select(x => x.Key));
            Assert.That(pathEditorViewModel.Paths.Select(x => x.Key), Is.EqualTo(expectedKeys), "ServerPathEditorViewModel Paths did not contains all keys");
            var expectedPaths = settings.FilesList.Select(x => x.Value).Concat(settings.DirsList.Select(x => x.Value));
            Assert.That(pathEditorViewModel.Paths.Select(x => x.Path), Is.EqualTo(expectedPaths), "ServerPathEditorViewModel Paths did not contains all paths");
        }

        [Test]
        public void ServerPathEditorViewModelWithWrongPathInitializeCorrectly()
        {
            ServerPathEditorViewModel pathEditorViewModel = TestViewModel(settings.CheckServerPath(TestConstants.wrongServerPath));
            Assert.That(pathEditorViewModel, Is.Not.Null, "ServerPathEditorViewModel is null");
            Assert.That(pathEditorViewModel.Paths.Any(), Is.True, "ServerPathEditorViewModel Paths is empty");
            Assert.That(pathEditorViewModel.Paths.All(x => !x.IsFounded), Is.True, "ServerPathEditorViewModel Paths wrong values");
            var expectedKeys = settings.FilesList.Select(x => x.Key).Concat(settings.DirsList.Select(x => x.Key));
            Assert.That(pathEditorViewModel.Paths.Select(x => x.Key), Is.EqualTo(expectedKeys), "ServerPathEditorViewModel Paths did not contains all keys");
            var expectedPaths = settings.FilesList.Select(x => x.Value).Concat(settings.DirsList.Select(x => x.Value));
            Assert.That(pathEditorViewModel.Paths.Select(x => x.Path), Is.EqualTo(expectedPaths), "ServerPathEditorViewModel Paths did not contains all paths");
        }

        [Test]
        public void ServerPathEditorViewModelCanCallRetryCommand()
        {
            var retryCommandCalled = false;
            RelayCommand retryCommand = new(obj => retryCommandCalled = (bool)obj);
            ServerPathEditorViewModel pathEditorViewModel = TestViewModel(new List<ServerPathEntry>(), retryCommand);
            pathEditorViewModel.RetryCommand.Execute(true);
            Assert.That(retryCommandCalled, Is.True, "ServerPathEditorViewModel RetryCommand not called");
        }

        private static ServerPathEditorViewModel TestViewModel(IEnumerable<ServerPathEntry> paths,
                                         RelayCommand retryCommand = null) => new(paths, retryCommand);
    }
}