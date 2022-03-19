using SPT_AKI_Profile_Editor.Core.ServerClasses;
using SPT_AKI_Profile_Editor.Helpers;

namespace SPT_AKI_Profile_Editor.Views
{
    internal class ClothingTabViewModel : BindableViewModel
    {
        public static RelayCommand AcquireAllCommand => new(obj => { ServerDatabase.AcquireAllClothing(); });
    }
}