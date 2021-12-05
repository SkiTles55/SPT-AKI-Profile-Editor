using NUnit.Framework;
using SPT_AKI_Profile_Editor.Core;

namespace SPT_AKI_Profile_Editor.Tests
{
    class ServerCheckerTests
    {
        [Test]
        public void ServerCheckerCanCheckExplorer() => Assert.IsTrue(ServerChecker.CheckProcess("explorer", @"C:\Windows\explorer.exe"));
        [Test]
        public void ServerCheckerCanCheckServer() => Assert.IsFalse(ServerChecker.CheckProcess());
    }
}
