using SPT_AKI_Profile_Editor.Core.Enums;
using SPT_AKI_Profile_Editor.Core.ProfileClasses;
using SPT_AKI_Profile_Editor.Helpers;

namespace SPT_AKI_Profile_Editor
{
    /// <summary>
    /// Логика взаимодействия для WeaponBuildWindow.xaml
    /// </summary>
    public partial class WeaponBuildWindow : ItemViewWindow
    {
        public WeaponBuildWindow(InventoryItem item, StashEditMode editMode) : base(item.Id)
        {
            InitializeComponent();
            DataContext = new WeaponBuildWindowViewModel(item, editMode, DialogCoordinator, App.WindowsDialogs);
            this.AllowDragging();
        }
    }
}