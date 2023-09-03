using SPT_AKI_Profile_Editor.Core.ProfileClasses;
using SPT_AKI_Profile_Editor.Helpers;

namespace SPT_AKI_Profile_Editor
{
    /// <summary>
    /// Логика взаимодействия для ContainerView.xaml
    /// </summary>
    public partial class ContainerWindow : ItemViewWindow
    {
        public ContainerWindow(InventoryItem item, CharacterInventory inventory) : base(item.Id)
            => Setup(new ContainerWindowViewModel(item,
                                                  inventory,
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