using SPT_AKI_Profile_Editor.Core.ProfileClasses;
using SPT_AKI_Profile_Editor.Helpers;

namespace SPT_AKI_Profile_Editor.Views
{
    class ExaminedItemsTabViewModel : BindableViewModel
    {
        public static RelayCommand ExamineAllCommand => new(obj => { Profile.Characters.Pmc.ExamineAll(); });
    }
}
