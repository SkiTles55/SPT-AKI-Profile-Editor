using NUnit.Framework;
using SPT_AKI_Profile_Editor.Core;
using SPT_AKI_Profile_Editor.Helpers;

namespace SPT_AKI_Profile_Editor.Tests.Hepers
{
    internal class ApplicationManagerTests
    {
        private IApplicationManager _applicationManager;

        [OneTimeSetUp]
        public void Setup()
        {
            _applicationManager = new ApplicationManager();
        }

        [Test]
        public void CanCheckProcessExplorer() => Assert.IsTrue(_applicationManager.CheckProcess("explorer", @"C:\Windows\explorer.exe"));

        [Test]
        public void CanCheckProcessServer()
        {
            AppData.AppSettings.ServerPath = TestHelpers.serverPath;
            Assert.IsFalse(_applicationManager.CheckProcess());
        }

        [Test]
        public void CanGetAppTitleWithVersion() => Assert.That(_applicationManager.GetAppTitleWithVersion(), Is.Not.Null);
    }
}