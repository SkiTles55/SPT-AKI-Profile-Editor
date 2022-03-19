using SPT_AKI_Profile_Editor.Core.ProfileClasses;
using SPT_AKI_Profile_Editor.Core.ServerClasses;
using SPT_AKI_Profile_Editor.Helpers;

namespace SPT_AKI_Profile_Editor.Views
{
    internal class MerchantsTabViewModel : BindableViewModel
    {
        public static RelayCommand SetAllMaxCommand => new(obj =>
          {
              if (Profile.Characters?.Pmc?.TraderStandings == null)
                  return;
              ServerDatabase.SetAllTradersMax();
          });
    }
}