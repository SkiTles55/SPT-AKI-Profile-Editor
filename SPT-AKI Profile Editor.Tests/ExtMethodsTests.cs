using NUnit.Framework;
using SPT_AKI_Profile_Editor.Core;

namespace SPT_AKI_Profile_Editor.Tests
{
    internal class ExtMethodsTests
    {
        [Test]
        public void IdNotEmpty() => Assert.IsNotEmpty(ExtMethods.GenerateNewId(new string[] { "testid" }));
    }
}