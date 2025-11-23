using Newtonsoft.Json;
using SPT_AKI_Profile_Editor.Core.HelperClasses;

namespace SPT_AKI_Profile_Editor.Core.ServerClasses
{
    public class LevelExpTable : BindableEntity
    {
        private long exp;

        [JsonProperty("exp")]
        public long Exp
        {
            get => exp;
            set
            {
                exp = value;
                OnPropertyChanged(nameof(Exp));
            }
        }
    }
}