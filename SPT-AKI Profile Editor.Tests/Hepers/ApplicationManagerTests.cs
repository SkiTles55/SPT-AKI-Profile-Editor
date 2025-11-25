using NUnit.Framework;
using SPT_AKI_Profile_Editor.Core;
using SPT_AKI_Profile_Editor.Helpers;

namespace SPT_AKI_Profile_Editor.Tests.Hepers
{
    internal class ApplicationManagerTests
    {
        private ApplicationManager _applicationManager;

        [OneTimeSetUp]
        public void Setup()
        {
            _applicationManager = new ApplicationManager();
        }

        [Test]
        public void CanCheckProcessExplorer()
            => Assert.That(_applicationManager.CheckProcess("explorer", @"C:\Windows\explorer.exe"),
                           Is.True);

        [Test]
        public void CanCheckProcessServer()
        {
            AppData.AppSettings.ServerPath = TestHelpers.serverPath;
            Assert.That(_applicationManager.CheckProcess(), Is.False);
        }

        [Test]
        public void CanGetAppTitleWithVersion() => Assert.That(_applicationManager.GetAppTitleWithVersion(), Is.Not.Null);
    }
}