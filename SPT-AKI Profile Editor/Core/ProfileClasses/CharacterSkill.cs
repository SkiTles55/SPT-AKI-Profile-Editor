using Newtonsoft.Json;
using SPT_AKI_Profile_Editor.Core.HelperClasses;
using SPT_AKI_Profile_Editor.Helpers;
using System;
using System.Linq;

namespace SPT_AKI_Profile_Editor.Core.ProfileClasses
{
    public class CharacterSkill : TemplateableEntity
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
            set => SetProperty(nameof(Progress), ref progress, value > MaxValue ? MaxValue : value);
        }

        [JsonIgnore]
        public float MaxValue => GetMaxProgress();

        [JsonIgnore]
        public string LocalizedName => AppData.ServerDatabase.LocalesGlobal.ContainsKey(Id)
            ? AppData.ServerDatabase.LocalesGlobal[Id] : MasteringLocalizedName();

        [JsonIgnore]
        public override string TemplateEntityId => Id;

        private float GetMaxProgress()
        {
            var mastering = AppData.ServerDatabase?.ServerGlobals?.Config?.Mastering.Where(x => x.Name == Id).FirstOrDefault();
            return mastering != null ? mastering.Level2 + mastering.Level3 : AppData.AppSettings.CommonSkillMaxValue;
        }

        private string MasteringLocalizedName()
        {
            var mastering = AppData.ServerDatabase.ServerGlobals.Config.Mastering
                .Where(x => x.Name == Id).FirstOrDefault();
            if (mastering != null)
                return string.Join(Environment.NewLine, mastering.Templates
                    .Where(x => AppData.ServerDatabase.LocalesGlobal.ContainsKey(x.Name()))
                    .Select(y => AppData.ServerDatabase.LocalesGlobal[y.Name()]));
            return Id;
        }
    }
}