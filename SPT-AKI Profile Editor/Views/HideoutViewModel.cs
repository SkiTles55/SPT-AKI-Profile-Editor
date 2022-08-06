using SPT_AKI_Profile_Editor.Core.ProfileClasses;
using SPT_AKI_Profile_Editor.Helpers;

namespace SPT_AKI_Profile_Editor.Views
{
    public class HideoutTabViewModel : BindableViewModel
    {
        public static RelayCommand SetAllMaxCommand => new(obj => Profile.Characters?.Pmc?.SetAllHideoutAreasMax());
    }
}