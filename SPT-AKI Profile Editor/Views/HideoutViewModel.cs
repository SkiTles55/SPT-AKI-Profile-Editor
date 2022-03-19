using SPT_AKI_Profile_Editor.Core.ProfileClasses;
using SPT_AKI_Profile_Editor.Helpers;

namespace SPT_AKI_Profile_Editor.Views
{
    internal class HideoutTabViewModel : BindableViewModel
    {
        public static RelayCommand SetAllMaxCommand => new(obj =>
        {
            if (Profile.Characters?.Pmc?.Hideout?.Areas == null)
                return;
            Profile.Characters.Pmc.SetAllHideoutAreasMax();
        });
    }
}