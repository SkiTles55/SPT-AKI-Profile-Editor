using SPT_AKI_Profile_Editor.Core.ProfileClasses;
using System;

namespace SPT_AKI_Profile_Editor.Core.Issues
{
    public class DuplicateItemsIDIssue : ProfileIssue
    {
        private CharacterInventory _inventory;

        public DuplicateItemsIDIssue(CharacterInventory inventory) : base(TargetName(inventory))
        {
            _inventory = inventory;
            Description = $"{TargetId} contains items with duplicated ID's";
        }

        public override Action FixAction => () => { _inventory.RemoveDuplicatedItems(); };

        public override string Description { get; }

        private static string TargetName(CharacterInventory inventory) => inventory == AppData.Profile.Characters.Pmc.Inventory ? "PMC" : "Scav";
    }
}