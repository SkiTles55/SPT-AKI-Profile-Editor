using SPT_AKI_Profile_Editor.Core.Enums;
using SPT_AKI_Profile_Editor.Core.ProfileClasses;
using SPT_AKI_Profile_Editor.Helpers;

namespace SPT_AKI_Profile_Editor
{
    /// <summary>
    /// Логика взаимодействия для ContainerView.xaml
    /// </summary>
    public partial class ContainerWindow : ItemViewWindow
    {
        public ContainerWindow(InventoryItem item, StashEditMode editMode) : base(item.Id)
            => Setup(new ContainerWindowViewModel(item,
                                                  editMode,
                                                  DialogCoordinator,
                                                  App.ApplicationManager));

        public ContainerWindow(InventoryItem item, EquipmentBuild build) : base(item.Id)
            => Setup(new ContainerWindowViewModel(item,
                                                  editMode,
                                                  DialogCoordinator,
                                                  App.ApplicationManager));

        private void Setup(ContainerWindowViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
            this.AllowDragging();
        }
    }
}