using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;

namespace SPT_AKI_Profile_Editor.Helpers
{
    public class ItemViewWindow : MetroWindow
    {
        public DialogCoordinator DialogCoordinator = new();

        public ItemViewWindow(string itemId) => ItemId = itemId;

        public string ItemId { get; }
    }
}