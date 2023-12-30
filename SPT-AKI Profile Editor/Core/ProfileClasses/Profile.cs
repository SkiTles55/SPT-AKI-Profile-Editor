using Newtonsoft.Json;
using SPT_AKI_Profile_Editor.Core.Enums;
using SPT_AKI_Profile_Editor.Core.HelperClasses;
using SPT_AKI_Profile_Editor.Helpers;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SPT_AKI_Profile_Editor.Core.ProfileClasses
{
    public class Profile : BindableEntity
    {
        [JsonIgnore]
        public List<ModdedEntity> ModdedEntitiesForRemoving = new();

        private ProfileCharacters characters;
        private string[] suits;
        private UserBuilds userBuilds;
        private int profileHash = 0;

        [JsonProperty("characters")]
        public ProfileCharacters Characters
        {
            get => characters;
            set
            {
                characters = value;
                OnPropertyChanged(nameof(Characters));
                OnPropertyChanged(nameof(IsProfileEmpty));
            }
        }

        [JsonProperty("suits")]
        public string[] Suits
        {
            get => suits;
            set
            {
                suits = value;
                OnPropertyChanged(nameof(Suits));
            }
        }

        [JsonProperty("userbuilds")]
        public UserBuilds UserBuilds
        {
            get => userBuilds;
            set
            {
                userBuilds = value;
                OnPropertyChanged(nameof(UserBuilds));
            }
        }

        [JsonIgnore]
        public bool IsProfileEmpty => Characters?.Pmc?.Info == null;

        [JsonIgnore]
        public int ProfileHash => profileHash;

        public void Load(string path)
        {
            string fileText = File.ReadAllText(path);
            Profile profile = JsonConvert.DeserializeObject<Profile>(fileText);
            profile.Characters.Pmc.SetupHideoutProductions();
            profileHash = JsonConvert.SerializeObject(profile).ToString().GetHashCode();
            if (profile.Characters?.Pmc?.Quests != null)
            {
                var addMissing = NeedToAddMissingQuests();
                var addMissingEvent = NeedToAddMissingEventQuests();

                bool ShouldAddQuest(string questQid)
                {
                    if (profile.Characters.Pmc.Quests.Any(y => y.Qid == questQid))
                        return false;

                    if (AppData.ServerConfigs.Quest.EventQuests.ContainsKey(questQid))
                        return addMissingEvent;
                    else
                        return addMissing;
                }

                if (addMissing || addMissingEvent)
                {
                    profile.Characters.Pmc.Quests = profile.Characters.Pmc.Quests
                    .Concat(AppData.ServerDatabase.QuestsData
                    .Where(x => ShouldAddQuest(x.Key))
                    .Select(x => new CharacterQuest { Qid = x.Key, Status = QuestStatus.Locked })
                    .ToArray())
                    .ToArray();
                }

                profile.Characters.Pmc.UpdateQuestsData();
            }
            if (NeedToAddMissingScavCommonSkills())
            {
                profile.Characters.Scav.Skills.Common = profile.Characters.Pmc.Skills.Common
                    .Select(x => new CharacterSkill { Id = x.Id, Progress = 0 })
                    .ToArray();
            }
            if (NeedToAddMissingMasteringsSkills())
            {
                AddMissingMasteringSkills(profile.Characters.Pmc.Skills);
                AddMissingMasteringSkills(profile.Characters.Scav.Skills);
            }
            AddMisingHeadToServerDatabase(profile.Characters?.Pmc);
            AddMisingHeadToServerDatabase(profile.Characters?.Scav);
            Characters = profile.Characters;
            Suits = profile.Suits;
            UserBuilds = profile.UserBuilds;

            static void AddMisingHeadToServerDatabase(Character character)
            {
                if (!string.IsNullOrEmpty(character?.Customization?.Head)
                && !AppData.ServerDatabase.Heads.Any(x => x.Key == character.Customization.Head))
                    AppData.ServerDatabase.Heads.Add(character.Customization.Head, character.Customization.Head);
            }

            static void AddMissingMasteringSkills(CharacterSkills characterSkills)
            {
                if (characterSkills.Mastering.Length != AppData.ServerDatabase.ServerGlobals.Config.Mastering.Length)
                {
                    characterSkills.Mastering = characterSkills.Mastering
                        .Concat(AppData.ServerDatabase.ServerGlobals.Config.Mastering
                        .Where(x => !AppData.AppSettings.BannedMasterings.Contains(x.Name) && !characterSkills.Mastering.Any(y => y.Id == x.Name))
                        .Select(x => new CharacterSkill { Id = x.Name, Progress = 0 })
                        .ToArray())
                        .ToArray();
                }
            }

            bool NeedToAddMissingQuests() => AppData.AppSettings.AutoAddMissingQuests
                && profile.Characters.Pmc.Quests.Length != AppData.ServerDatabase.QuestsData.Count;

            bool NeedToAddMissingEventQuests() => AppData.AppSettings.AutoAddMissingEventQuests
                && profile.Characters.Pmc.Quests.Length != AppData.ServerDatabase.QuestsData.Count;

            bool NeedToAddMissingScavCommonSkills() => AppData.AppSettings.AutoAddMissingScavSkills
                && profile.Characters?.Pmc?.Skills?.Common != null
                && profile.Characters?.Scav?.Skills?.Common != null
                && profile.Characters.Scav.Skills.Common.Length == 0;

            bool NeedToAddMissingMasteringsSkills() => AppData.AppSettings.AutoAddMissingMasterings
                && profile.Characters?.Pmc?.Skills?.Mastering != null
                && profile.Characters?.Scav?.Skills?.Mastering != null;
        }

        public List<SaveException> Save(string targetPath, string savePath = null)
            => new ProfileSaver(this).Save(targetPath, savePath);

        public bool IsProfileChanged()
            => ProfileHash != 0 && ProfileHash != JsonConvert.SerializeObject(this).ToString().GetHashCode();
    }
}