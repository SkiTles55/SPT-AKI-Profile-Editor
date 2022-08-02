using Newtonsoft.Json;
using SPT_AKI_Profile_Editor.Core.HelperClasses;
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
            AppData.ServerDatabase.LocalesGlobal.Templates.ContainsKey(SuiteId) ? AppData.ServerDatabase.LocalesGlobal.Templates[SuiteId].Name : SuiteId;

        [JsonIgnore]
        public bool Boughted
        {
            get => AppData.Profile?.Suits?.Any(x => x == SuiteId) ?? false;
            set
            {
                if (AppData.Profile?.Suits == null)
                    return;
                if (value)
                {
                    if (!AppData.Profile.Suits.Any(x => x == SuiteId))
                        AppData.Profile.Suits = AppData.Profile.Suits.Append(SuiteId).ToArray();
                }
                else
                {
                    if (AppData.Profile.Suits.Any(x => x == SuiteId))
                        AppData.Profile.Suits = AppData.Profile.Suits.Except(new string[] { SuiteId }).ToArray();
                }
                OnPropertyChanged("Boughted");
            }
        }
    }
}