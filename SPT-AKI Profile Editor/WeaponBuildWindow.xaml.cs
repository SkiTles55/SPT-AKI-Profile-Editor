using SPT_AKI_Profile_Editor.Core.ProfileClasses;
using SPT_AKI_Profile_Editor.Helpers;

namespace SPT_AKI_Profile_Editor
{
    /// <summary>
    /// Логика взаимодействия для WeaponBuildWindow.xaml
    /// </summary>
    public partial class WeaponBuildWindow : ItemViewWindow
    {
        public WeaponBuildWindow(InventoryItem item, CharacterInventory inventory) : base(item.Id)
            => Setup(new WeaponBuildWindowViewModel(item,
                                                    inventory,
                                                    DialogCoordinator,
                                                    App.WindowsDialogs));

        private void Setup(WeaponBuildWindowViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
            this.AllowDragging();
        }
    }
}