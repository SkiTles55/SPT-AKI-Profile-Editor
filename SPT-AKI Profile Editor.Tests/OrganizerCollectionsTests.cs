using NUnit.Framework;
using SPT_AKI_Profile_Editor.Core;
using SPT_AKI_Profile_Editor.Core.HelperClasses;
using SPT_AKI_Profile_Editor.Core.ServerClasses;
using System.Collections.Generic;
using System.Linq;

namespace SPT_AKI_Profile_Editor.Tests
{
    internal class OrganizerCollectionsTests
    {
        private static TarkovItem Container(string id, string[] filter) => new(id, new TarkovItemProperties
        {
            Grids = new[] { new Grid { Props = new GridProps
            {
                CellsH = 4,
                CellsV = 4,
                Filters = filter == null ? null : new[] { new Filters { Filter = filter, ExcludedFilter = [] } }
            } } }
        }, "container_parent", "Item");

        private static TarkovItem Item(string id, string parent) => new(id, new TarkovItemProperties
        {
            Width = 1,
            Height = 1,
            StackMaxSize = 1
        }, parent, "Item");

        [SetUp]
        public void Setup()
        {
            AppData.ServerDatabase.LocalesGlobal = [];
            AppData.ServerDatabase.ItemsDB = [];
        }

        [Test]
        public void GetByOrganizerTplReturnsNullForUnknownTpl()
            => Assert.That(OrganizerCollections.GetByOrganizerTpl("unknown"), Is.Null);

        [Test]
        public void GetByOrganizerTplReturnsInfoForKnownTpls()
        {
            var keys = OrganizerCollections.GetByOrganizerTpl(OrganizerCollections.KeyOrganizerTpl);
            var keycards = OrganizerCollections.GetByOrganizerTpl(OrganizerCollections.KeycardOrganizerTpl);
            Assert.That(keys, Is.Not.Null);
            Assert.That(keys.ButtonTextKey, Is.EqualTo("container_add_all_keys"));
            Assert.That(keycards, Is.Not.Null);
            Assert.That(keycards.ButtonTextKey, Is.EqualTo("container_add_all_keycards"));
        }

        [Test]
        public void GetCandidatesOnlyReturnsFilterAllowedItemsSortedById()
        {
            var organizerTpl = Container(OrganizerCollections.KeyOrganizerTpl, new[] { "keycat" });
            AppData.ServerDatabase.ItemsDB = new Dictionary<string, TarkovItem>
            {
                [OrganizerCollections.KeyOrganizerTpl] = organizerTpl,
                ["k2"] = Item("k2", "keycat"),
                ["k1"] = Item("k1", "keycat"),
                ["c1"] = Item("c1", "cardcat"),
                ["o1"] = Item("o1", "othercat")
            };
            var candidates = OrganizerCollections.GetByOrganizerTpl(OrganizerCollections.KeyOrganizerTpl).GetCandidates().Select(x => x.Id).ToList();
            Assert.That(candidates, Is.EqualTo(new[] { "k1", "k2" }));
        }

        [Test]
        public void GetCandidatesExcludesBlockedKeys()
        {
            var organizerTpl = Container(OrganizerCollections.KeyOrganizerTpl, new[] { "keycat" });
            const string blockedId = "57518f7724597720a31c09ab";
            AppData.ServerDatabase.ItemsDB = new Dictionary<string, TarkovItem>
            {
                [OrganizerCollections.KeyOrganizerTpl] = organizerTpl,
                ["k1"] = Item("k1", "keycat"),
                [blockedId] = Item(blockedId, "keycat")
            };
            var candidates = OrganizerCollections.GetByOrganizerTpl(OrganizerCollections.KeyOrganizerTpl).GetCandidates().Select(x => x.Id).ToList();
            Assert.That(candidates, Does.Not.Contain(blockedId));
            Assert.That(candidates, Is.EqualTo(new[] { "k1" }));
        }

        [Test]
        public void GetCandidatesExcludesSelfWhitelistedItem()
        {
            var organizerTpl = Container(OrganizerCollections.KeyOrganizerTpl, new[] { "keycat", "specialkeyid" });
            AppData.ServerDatabase.ItemsDB = new Dictionary<string, TarkovItem>
            {
                [OrganizerCollections.KeyOrganizerTpl] = organizerTpl,
                ["k1"] = Item("k1", "keycat"),
                ["specialkeyid"] = Item("specialkeyid", "othercat")
            };
            var candidates = OrganizerCollections.GetByOrganizerTpl(OrganizerCollections.KeyOrganizerTpl).GetCandidates().Select(x => x.Id).ToList();
            Assert.That(candidates, Is.EqualTo(new[] { "k1" }));
        }

        [Test]
        public void GetCandidatesReturnsEmptyWhenOrganizerTemplateMissing()
        {
            Assert.That(OrganizerCollections.GetByOrganizerTpl(OrganizerCollections.KeyOrganizerTpl).GetCandidates(), Is.Empty);
        }
    }
}
