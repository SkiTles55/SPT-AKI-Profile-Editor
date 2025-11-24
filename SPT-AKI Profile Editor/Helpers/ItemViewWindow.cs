using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;

namespace SPT_AKI_Profile_Editor.Helpers
{
    public class ItemViewWindow(string itemId) : MetroWindow
    {
        public DialogCoordinator DialogCoordinator = new();

        public string ItemId { get; } = itemId;
    }
}