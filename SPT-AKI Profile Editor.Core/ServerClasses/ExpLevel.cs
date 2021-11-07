using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;

namespace SPT_AKI_Profile_Editor.Core.ServerClasses
{
    public class ExpLevel : INotifyPropertyChanged
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

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }
}