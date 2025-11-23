using SPT_AKI_Profile_Editor.Core.ProfileClasses;
using System.Collections.Generic;

namespace SPT_AKI_Profile_Editor.Core.Equipment
{
    public class EquipmentSlotItem : EquipmentSlot
    {
        public EquipmentSlotItem(string slotName,
                                 InventoryItem item,
                                 string emptySlotText) : base(slotName)
        {
            Item = item;
            EmptySlotText = emptySlotText;
            ItemsList = [item];
        }

        public InventoryItem Item { get; }
        public string EmptySlotText { get; }

        public override List<InventoryItem> ItemsList { get; }
    }
}