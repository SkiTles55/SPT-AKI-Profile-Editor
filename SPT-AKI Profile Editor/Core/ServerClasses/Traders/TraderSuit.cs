using Newtonsoft.Json;
using SPT_AKI_Profile_Editor.Core.HelperClasses;
using SPT_AKI_Profile_Editor.Core.ProfileClasses;
using SPT_AKI_Profile_Editor.Helpers;
using System.Linq;

namespace SPT_AKI_Profile_Editor.Core.ServerClasses
{
    public class TraderSuit : BindableEntity
    {
        [JsonProperty("_id")]
        public string Id { get; set; }

        [JsonProperty("suiteId")]
        public string SuiteId { get; set; }

        [JsonIgnore]
        public string LocalizedName =>
            AppData.ServerDatabase.LocalesGlobal.ContainsKey(SuiteId.Name()) ? AppData.ServerDatabase.LocalesGlobal[SuiteId.Name()] : SuiteId;

        [JsonIgnore]
        public bool Boughted
        {
            get => AppData.Profile?.CustomisationUnlocks?.Any(x => x.IsSuitUnlock && x.Id == SuiteId) ?? false;
            set
            {
                if (AppData.Profile?.CustomisationUnlocks == null)
                    return;
                if (value)
                {
                    if (!AppData.Profile.CustomisationUnlocks.Any(x => x.IsSuitUnlock && x.Id == SuiteId))
                        AppData.Profile.CustomisationUnlocks = AppData.Profile.CustomisationUnlocks.Append(new CustomisationUnlock(SuiteId)).ToArray();
                }
                else
                {
                    if (AppData.Profile.CustomisationUnlocks.Any(x => x.IsSuitUnlock && x.Id == SuiteId))
                        AppData.Profile.CustomisationUnlocks = AppData.Profile.CustomisationUnlocks.Where(x => !x.IsSuitUnlock || x.Id != SuiteId).ToArray();
                }
                OnPropertyChanged("Boughted");
            }
        }
    }
}