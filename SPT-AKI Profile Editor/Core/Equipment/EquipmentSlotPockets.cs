using SPT_AKI_Profile_Editor.Core.ProfileClasses;
using System.Collections.Generic;
using System.Linq;

namespace SPT_AKI_Profile_Editor.Core.Equipment
{
    public class EquipmentSlotPockets : EquipmentSlot
    {
        public EquipmentSlotPockets(string slotName,
                                    IEnumerable<InventoryItem> items) : base(slotName)
            => ItemsList = items.ToList();

        public override List<InventoryItem> ItemsList { get; }

        public bool PocketsHasItems => ItemsList.Count > 0;
    }
}