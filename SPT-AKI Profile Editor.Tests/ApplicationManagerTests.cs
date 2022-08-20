using NUnit.Framework;
using SPT_AKI_Profile_Editor.Helpers;

namespace SPT_AKI_Profile_Editor.Tests
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
        public void CanCheckProcessServer() => Assert.IsFalse(_applicationManager.CheckProcess());
    }
}