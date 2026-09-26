using NUnit.Framework;
using SPT_AKI_Profile_Editor.Core;
using SPT_AKI_Profile_Editor.Core.Enums;
using SPT_AKI_Profile_Editor.Core.HelperClasses;
using SPT_AKI_Profile_Editor.Core.ProfileClasses;
using SPT_AKI_Profile_Editor.Core.ServerClasses;
using System.Collections.Generic;
using System.Linq;

namespace SPT_AKI_Profile_Editor.Tests
{
    internal class CharacterInventoryTests
    {
        private static readonly string stashTpl = "stash_tpl";
        private static readonly string stashId = "stash_id";
        private static readonly string organizerId = "organizer_id";
        private static readonly string[] keyCandidates = ["k1", "k2", "k3", "k4", "k5", "k6"];

        private static TarkovItem ContainerTpl(string id, int cellsH, int cellsV, string[] filter) => new(id, new TarkovItemProperties
        {
            Width = 2,
            Height = 2,
            StackMaxSize = 1,
            Grids = new[] { new Grid { Props = new GridProps
            {
                CellsH = cellsH,
                CellsV = cellsV,
                Filters = filter == null ? null : new[] { new Filters { Filter = filter, ExcludedFilter = [] } }
            } } }
        }, "container_parent", "Item");

        private static TarkovItem ItemTpl(string id, string parent) => new(id, new TarkovItemProperties
        {
            Width = 1,
            Height = 1,
            StackMaxSize = 1
        }, parent, "Item");

        [SetUp]
        public void Setup()
        {
            AppData.ServerDatabase.LocalesGlobal = [];
            AppData.Profile.Characters = new ProfileCharacters { Pmc = new Character { Bonuses = [], StashRowsBonusCount = 0 } };
            LoadMinimalItemsDb();
        }

        private void LoadMinimalItemsDb()
        {
            var db = new Dictionary<string, TarkovItem>
            {
                [stashTpl] = ContainerTpl(stashTpl, 5, 5, null),
                [OrganizerCollections.KeyOrganizerTpl] = ContainerTpl(OrganizerCollections.KeyOrganizerTpl, 2, 2, new[] { "keycat" })
            };
            foreach (var id in keyCandidates)
                db[id] = ItemTpl(id, "keycat");
            AppData.ServerDatabase.ItemsDB = db;
        }

        private static CharacterInventory InventoryWithOrganizer()
            => new()
            {
                Stash = stashId,
                Items = new[]
                {
                    new InventoryItem { Id = stashId, Tpl = stashTpl },
                    new InventoryItem
                    {
                        Id = organizerId,
                        Tpl = OrganizerCollections.KeyOrganizerTpl,
                        ParentId = stashId,
                        Location = new ItemLocation { X = 0, Y = 0, R = ItemRotation.Horizontal }
                    }
                }
            };

        private static InventoryItem GetOrganizer(CharacterInventory inventory) => inventory.Items.First(x => x.Id == organizerId);

        [Test]
        public void ReturnsZeroForUnsupportedContainer()
        {
            var inventory = InventoryWithOrganizer();
            var unsupported = new InventoryItem { Id = "whatever", Tpl = "unsupported_tpl" };
            var result = inventory.AddCollectionItemsToOrganizer(unsupported);
            Assert.That(result, Is.EqualTo(new OrganizerFillResult(0, 0)));
            Assert.That(inventory.Items.Count(), Is.EqualTo(2));
        }

        [Test]
        public void FillsOrganizerAndCreatesNewContainersWhenFull()
        {
            var inventory = InventoryWithOrganizer();
            var result = inventory.AddCollectionItemsToOrganizer(GetOrganizer(inventory));
            Assert.That(result, Is.EqualTo(new OrganizerFillResult(6, 1)));
            var organizers = inventory.Items.Where(x => x.Tpl == OrganizerCollections.KeyOrganizerTpl).ToList();
            Assert.That(organizers.Count, Is.EqualTo(2));
            Assert.That(inventory.Items.Count(x => x.Tpl == OrganizerCollections.KeyOrganizerTpl && x.ParentId == stashId), Is.EqualTo(2));
            var innerTpls = inventory.Items.Where(x => x.ParentId == organizers[0].Id || x.ParentId == organizers[1].Id).Select(x => x.Tpl).ToList();
            Assert.That(innerTpls, Is.EquivalentTo(keyCandidates));
            Assert.That(innerTpls.Distinct().Count(), Is.EqualTo(6));
        }

        [Test]
        public void DoesNotDuplicateExistingItemsInOtherOrganizers()
        {
            var inventory = InventoryWithOrganizer();
            inventory.Items = inventory.Items.Append(new InventoryItem
            {
                Id = "pre_key_id",
                ParentId = organizerId,
                Tpl = "k1",
                SlotId = "main",
                Location = new ItemLocation { X = 0, Y = 0, R = ItemRotation.Horizontal }
            }).ToArray();
            var result = inventory.AddCollectionItemsToOrganizer(GetOrganizer(inventory));
            Assert.That(result.AddedItems, Is.EqualTo(5));
            Assert.That(innerTpls(inventory).Where(x => x == "k1").Count(), Is.EqualTo(1));
        }

        [Test]
        public void IsIdempotent()
        {
            var inventory = InventoryWithOrganizer();
            inventory.AddCollectionItemsToOrganizer(GetOrganizer(inventory));
            var second = inventory.AddCollectionItemsToOrganizer(GetOrganizer(inventory));
            Assert.That(second, Is.EqualTo(new OrganizerFillResult(0, 0)));
        }

        [Test]
        public void NeverAddsBlockedKeys()
        {
            AppData.ServerDatabase.ItemsDB["57518f7724597720a31c09ab"] = ItemTpl("57518f7724597720a31c09ab", "keycat");
            var inventory = InventoryWithOrganizer();
            var result = inventory.AddCollectionItemsToOrganizer(GetOrganizer(inventory));
            Assert.That(result, Is.EqualTo(new OrganizerFillResult(6, 1)));
            Assert.That(innerTpls(inventory), Does.Not.Contain("57518f7724597720a31c09ab"));
        }

        private static List<string> innerTpls(CharacterInventory inventory)
            => inventory.Items.Where(x => x.ParentId == organizerId).Select(x => x.Tpl).ToList();
    }
}