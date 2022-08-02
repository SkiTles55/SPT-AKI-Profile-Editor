using SPT_AKI_Profile_Editor.Core;
using SPT_AKI_Profile_Editor.Core.HelperClasses;
using SPT_AKI_Profile_Editor.Core.ProfileClasses;
using SPT_AKI_Profile_Editor.Core.ServerClasses;

namespace SPT_AKI_Profile_Editor.Helpers
{
    public class BindableViewModel : BindableEntity
    {
        public static AppLocalization AppLocalization => AppData.AppLocalization;
        public static Profile Profile => AppData.Profile;
        public static GridFilters GridFilters => AppData.GridFilters;
        public static ServerDatabase ServerDatabase => AppData.ServerDatabase;
    }
}