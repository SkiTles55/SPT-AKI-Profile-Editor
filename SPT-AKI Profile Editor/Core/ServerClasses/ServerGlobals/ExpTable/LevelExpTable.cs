using Newtonsoft.Json;

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
                OnPropertyChanged("Exp");
            }
        }
    }
}