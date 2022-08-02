using Newtonsoft.Json;
using SPT_AKI_Profile_Editor.Core.HelperClasses;
using System;
using System.Linq;

namespace SPT_AKI_Profile_Editor.Core.ProfileClasses
{
    public class CharacterSkill : BindableEntity
    {
        private string id;

        private float progress;

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

        private float GetMaxProgress()
        {
            var mastering = AppData.ServerDatabase.ServerGlobals.Config.Mastering.Where(x => x.Name == Id).FirstOrDefault();
            if (mastering != null)
                return mastering.Level2 + mastering.Level3;
            else
                return AppData.AppSettings.CommonSkillMaxValue;
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
    }
}