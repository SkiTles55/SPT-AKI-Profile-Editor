using SPT_AKI_Profile_Editor.Core.ProfileClasses;
using System.Collections.Generic;

namespace SPT_AKI_Profile_Editor.Core.Equipment
{
    public class EquipmentSlot
    {
        public EquipmentSlot(string slotName) => SlotName = slotName;

        public string SlotName { get; }

        public virtual List<InventoryItem> ItemsList { get; }
    }
}