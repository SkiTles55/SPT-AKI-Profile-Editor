using SPT_AKI_Profile_Editor.Core.ProfileClasses;
using System.Collections.Generic;

namespace SPT_AKI_Profile_Editor.Core.Equipment
{
    public class EquipmentSlotPockets(string slotName,
        IEnumerable<InventoryItem> items) : EquipmentSlot(slotName)
    {
        public override List<InventoryItem> ItemsList { get; } = [.. items];

        public bool PocketsHasItems => ItemsList.Count > 0;
    }
}