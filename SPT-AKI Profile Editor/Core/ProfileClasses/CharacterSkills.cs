using Newtonsoft.Json;
using SPT_AKI_Profile_Editor.Core.HelperClasses;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SPT_AKI_Profile_Editor.Core.ProfileClasses
{
    public class CharacterSkills : BindableEntity
    {
        private CharacterSkill[] common;

        private CharacterSkill[] mastering;

        public CharacterSkill[] Common
        {
            get => common;
            set
            {
                common = value;
                OnPropertyChanged(nameof(Common));
                OnPropertyChanged(nameof(IsCommonSkillsEmpty));
            }
        }

        public CharacterSkill[] Mastering
        {
            get => mastering;
            set
            {
                mastering = value;
                OnPropertyChanged(nameof(Mastering));
                OnPropertyChanged(nameof(IsMasteringsEmpty));
            }
        }

        [JsonIgnore]
        public bool IsCommonSkillsEmpty => Common == null || Common.Length == 0;

        [JsonIgnore]
        public bool IsMasteringsEmpty => Mastering == null || Mastering.Length == 0;

        public void RemoveCommonSkills(IEnumerable<string> skillIds)
            => Common = [.. Common.Where(x => !skillIds.Contains(x.Id))];

        public void AddCommonSkill(CharacterSkill skill) => Common = [.. Common, skill];

        public void RemoveMasteringSkills(IEnumerable<string> skillIds)
            => Mastering = [.. Mastering.Where(x => !skillIds.Contains(x.Id))];

        public void AddMasteringSkill(CharacterSkill skill) => Mastering = [.. Mastering, skill];
    }
}