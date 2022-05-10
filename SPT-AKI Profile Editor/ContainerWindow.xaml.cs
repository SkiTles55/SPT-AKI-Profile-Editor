using MahApps.Metro.Controls;
using SPT_AKI_Profile_Editor.Core.ProfileClasses;

namespace SPT_AKI_Profile_Editor
{
    /// <summary>
    /// Логика взаимодействия для ContainerView.xaml
    /// </summary>
    public partial class ContainerWindow : MetroWindow
    {
        public string ItemId { get; }
        public ContainerWindow(InventoryItem item)
        {
            InitializeComponent();
            DataContext = new ContainerWindowViewModel(item);
            ItemId = item.Id;
        }
    }
}