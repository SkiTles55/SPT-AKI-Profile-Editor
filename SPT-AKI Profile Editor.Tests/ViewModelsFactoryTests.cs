using NUnit.Framework;
using SPT_AKI_Profile_Editor.Helpers;
using SPT_AKI_Profile_Editor.Tests.Hepers;

namespace SPT_AKI_Profile_Editor.Tests
{
    internal class ViewModelsFactoryTests
    {
        [Test]
        public void InitializeCorrectly()
        {
            ViewModelsFactory factory = new(new TestsDialogManager(),
                                            new TestsWorker(),
                                            new TestsApplicationManager(),
                                            new TestsWindowsDialogs(),
                                            null);
            Assert.That(factory, Is.Not.Null, "ViewModelsFactory is null");
            Assert.That(factory.FastMode, Is.Not.Null, "FastMode is null");
            Assert.That(factory.InfoTab, Is.Not.Null, "InfoTab is null");
            Assert.That(factory.MerchantsTab, Is.Not.Null, "MerchantsTab is null");
            Assert.That(factory.QuestsTab, Is.Not.Null, "QuestsTab is null");
            Assert.That(factory.HideoutTab, Is.Not.Null, "HideoutTab is null");
            Assert.That(factory.SkillsTab, Is.Not.Null, "SkillsTab is null");
            Assert.That(factory.MasteringTab, Is.Not.Null, "MasteringTab is null");
            Assert.That(factory.ExaminedItemsTab, Is.Not.Null, "ExaminedItemsTab is null");
            Assert.That(factory.StashTab, Is.Not.Null, "StashTab is null");
            Assert.That(factory.ClothingTab, Is.Not.Null, "ClothingTab is null");
            Assert.That(factory.WeaponBuildsTab, Is.Not.Null, "WeaponBuildsTab is null");
            Assert.That(factory.BackupsTab, Is.Not.Null, "BackupsTab is null");
            Assert.That(factory.AboutTab, Is.Not.Null, "AboutTab is null");
        }
    }
}