using Newtonsoft.Json;
using SPT_AKI_Profile_Editor.Core.HelperClasses;
using System.Linq;

namespace SPT_AKI_Profile_Editor.Core.ServerClasses
{
    public class ServerGlobalsConfig : BindableEntity
    {
        private Mastering[] mastering;

        private ConfigExp exp;

        [JsonProperty("Mastering")]
        public Mastering[] Mastering
        {
            get => mastering;
            set
            {
                mastering = value;
                OnPropertyChanged("Mastering");
            }
        }

        [JsonProperty("exp")]
        public ConfigExp Exp
        {
            get => exp;
            set
            {
                exp = value;
                OnPropertyChanged("Exp");
            }
        }

        [JsonIgnore]
        public float MaxProgressValue => Mastering.Max(x => x.Level2 + x.Level3);
    }
}