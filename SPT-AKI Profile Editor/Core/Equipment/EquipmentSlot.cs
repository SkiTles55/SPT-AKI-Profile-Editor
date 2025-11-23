using SPT_AKI_Profile_Editor.Core.ProfileClasses;
using System.Collections.Generic;

namespace SPT_AKI_Profile_Editor.Core.Equipment
{
    public class EquipmentSlot(string slotName)
    {
        public string SlotName { get; } = slotName;

        public virtual List<InventoryItem> ItemsList { get; }
    }
}