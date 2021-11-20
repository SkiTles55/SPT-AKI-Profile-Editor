using Newtonsoft.Json;
using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace SPT_AKI_Profile_Editor.Core.ProfileClasses
{
    public class CharacterSkill : INotifyPropertyChanged
    {
        [JsonProperty("Id")]
        public string Id
        {
            get => id;
            set
            {
                id = value;
                OnPropertyChanged("Id");
            }
        }
        [JsonProperty("Progress")]
        public float Progress
        {
            get => progress;
            set
            {
                progress = value > MaxValue ? MaxValue : value;
                OnPropertyChanged("Progress");
            }
        }
        [JsonIgnore]
        public float MaxValue => GetMaxProgress();
        [JsonIgnore]
        public string LocalizedName => AppData.ServerDatabase.LocalesGlobal.Interface.ContainsKey(Id)
            ? AppData.ServerDatabase.LocalesGlobal.Interface[Id] : MasteringLocalizedName();

        private string id;
        private float progress;

        private float GetMaxProgress()
        {
            var mastering = AppData.ServerDatabase.ServerGlobals.Config.Mastering.Where(x => x.Name == Id).FirstOrDefault();
            if (mastering != null)
                return mastering.Level2 + mastering.Level3;
            else
                return AppData.ServerDatabase.CommonSkillMaxValue;
        }

        private string MasteringLocalizedName()
        {
            var mastering = AppData.ServerDatabase.ServerGlobals.Config.Mastering
                .Where(x => x.Name == Id).FirstOrDefault();
            if (mastering != null)
                return string.Join(Environment.NewLine, mastering.Templates
                    .Where(x => AppData.ServerDatabase.LocalesGlobal.Templates.ContainsKey(x))
                    .Select(y => AppData.ServerDatabase.LocalesGlobal.Templates[y].Name));
            else
                return Id;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }
}