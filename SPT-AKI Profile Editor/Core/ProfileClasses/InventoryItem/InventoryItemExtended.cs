using System.Collections.Generic;
using System.Linq;

namespace SPT_AKI_Profile_Editor.Core.ProfileClasses
{
    public class InventoryItemExtended : InventoryItem
    {
        public InventoryItemExtended(InventoryItem item, InventoryItem[] items)
        {
            Id = item.Id;
            Tpl = item.Tpl;
            SlotId = item.SlotId;
            Location = item.Location;
            ParentId = item.ParentId;
            Upd = item.Upd;
            InnerItems = items.Where(x => x.ParentId == item.Id).Select(x => new InventoryItemExtended(x, items)).ToList();
        }

        public List<InventoryItemExtended> InnerItems { get; }
    }
}