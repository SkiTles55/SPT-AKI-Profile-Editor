using System.Linq;
using System.Text.Json.Serialization;

namespace SPT_AKI_Profile_Editor.Core.ServerClasses
{
    public class TraderSuit : BindableEntity
    {
        [JsonPropertyName("_id")]
        public string Id { get; set; }

        [JsonPropertyName("suiteId")]
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