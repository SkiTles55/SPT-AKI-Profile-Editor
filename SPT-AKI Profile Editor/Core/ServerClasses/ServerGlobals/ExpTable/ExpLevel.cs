using Newtonsoft.Json;
using SPT_AKI_Profile_Editor.Core.HelperClasses;
using System;
using System.Linq;

namespace SPT_AKI_Profile_Editor.Core.ServerClasses
{
    public class ExpLevel : BindableEntity
    {
        private LevelExpTable[] expTable;

        [JsonConstructor]
        public ExpLevel(LevelExpTable[] expTable)
        {
            ExpTable = expTable;
            MaxLevel = Math.Max(1, expTable.Length - 1);
            MaxExp = expTable.Select(x => x.Exp).Sum();
        }

        [JsonProperty("exp_table")]
        public LevelExpTable[] ExpTable
        {
            get => expTable;
            set
            {
                expTable = value;
                OnPropertyChanged("ExpTable");
            }
        }

        [JsonIgnore]
        public long MaxExp { get; }

        [JsonIgnore]
        public int MaxLevel { get; }
    }
}