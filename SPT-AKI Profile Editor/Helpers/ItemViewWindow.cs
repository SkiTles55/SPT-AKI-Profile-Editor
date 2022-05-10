using MahApps.Metro.Controls;

namespace SPT_AKI_Profile_Editor.Helpers
{
    public class ItemViewWindow : MetroWindow
    {
        public ItemViewWindow(string itemId) => ItemId = itemId;

        public string ItemId { get; }
    }
}