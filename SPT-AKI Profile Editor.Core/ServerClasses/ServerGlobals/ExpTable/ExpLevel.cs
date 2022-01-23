using System.Text.Json.Serialization;

namespace SPT_AKI_Profile_Editor.Core.ServerClasses
{
    public class ExpLevel : BindableEntity
    {
        [JsonPropertyName("exp_table")]
        public LevelExpTable[] ExpTable
        {
            get => expTable;
            set
            {
                expTable = value;
                OnPropertyChanged("ExpTable");
                MaxExp = CalculateMaxExp();
            }
        }
        [JsonIgnore]
        public long MaxExp
        {
            get => maxExp;
            set
            {
                maxExp = value;
                OnPropertyChanged("MaxExp");
            }
        }

        private LevelExpTable[] expTable;
        private long maxExp;

        private long CalculateMaxExp()
        {
            long experience = 0;
            foreach (var exp in expTable)
                experience += exp.Exp;
            return experience;
        }
    }
}