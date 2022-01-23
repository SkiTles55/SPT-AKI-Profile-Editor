using System.Text.Json.Serialization;

namespace SPT_AKI_Profile_Editor.Core.ServerClasses
{
    public class LevelExpTable : BindableEntity
    {
        [JsonPropertyName("exp")]
        public long Exp
        {
            get => exp;
            set
            {
                exp = value;
                OnPropertyChanged("Exp");
            }
        }

        private long exp;
    }
}
