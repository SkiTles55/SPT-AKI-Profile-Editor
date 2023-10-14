using Newtonsoft.Json;
using SPT_AKI_Profile_Editor.Core.Enums;
using SPT_AKI_Profile_Editor.Core.HelperClasses;
using SPT_AKI_Profile_Editor.Helpers;
using System;
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
            profileHash = JsonConvert.SerializeObject(profile).ToString().GetHashCode();
            if (profile.Characters?.Pmc?.Quests != null)
            {
                if (NeedToAddMissingQuests())
                {
                    profile.Characters.Pmc.Quests = profile.Characters.Pmc.Quests
                    .Concat(AppData.ServerDatabase.QuestsData
                    .Where(x => !profile.Characters.Pmc.Quests.Any(y => y.Qid == x.Key))
                    .Select(x => new CharacterQuest { Qid = x.Key, Status = QuestStatus.Locked })
                    .ToArray())
                    .ToArray();
                }
                if (profile.Characters.Pmc.Quests.Length > 0)
                    foreach (var quest in profile.Characters.Pmc.Quests)
                        SetupQuest(quest);
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

            void SetupQuest(CharacterQuest quest)
            {
                quest.QuestQid = quest.Qid;
                quest.Type = QuestType.Standart;
                if (AppData.ServerDatabase.LocalesGlobal.ContainsKey(quest.Qid.QuestName()) || profile.Characters.Pmc.RepeatableQuests == null || profile.Characters.Pmc.RepeatableQuests.Length == 0)
                {
                    quest.QuestTrader = AppData.ServerDatabase.QuestsData.ContainsKey(quest.Qid) ? AppData.ServerDatabase.QuestsData[quest.Qid].TraderId : "unknown";
                    quest.QuestData = AppData.ServerDatabase.QuestsData.ContainsKey(quest.Qid) ? AppData.ServerDatabase.QuestsData[quest.Qid] : null;
                    return;
                }

                foreach (QuestType type in Enum.GetValues(typeof(QuestType)))
                {
                    var typeQuests = profile.Characters.Pmc.RepeatableQuests.Where(x => x.Type == type).FirstOrDefault();
                    if (typeQuests == null)
                        continue;
                    if (SetupQuestFromArray(typeQuests.ActiveQuests, type))
                        return;
                    if (SetupQuestFromArray(typeQuests.InactiveQuests, type))
                        return;
                }

                bool SetupQuestFromArray(ActiveQuest[] array, QuestType type)
                {
                    if (array.Any())
                    {
                        var repeatableQuest = array.Where(x => x.Id == quest.Qid);
                        if (repeatableQuest.Any())
                        {
                            quest.Type = type;
                            quest.QuestTrader = repeatableQuest.First().TraderId;
                            quest.QuestQid = repeatableQuest.First().Type.LocalizedName();
                            return true;
                        }
                    }
                    return false;
                }
            }

            bool NeedToAddMissingQuests() => AppData.AppSettings.AutoAddMissingQuests
                && profile.Characters.Pmc.Quests.Length != AppData.ServerDatabase.QuestsData.Count;

            bool NeedToAddMissingScavCommonSkills() => AppData.AppSettings.AutoAddMissingScavSkills
                && profile.Characters?.Pmc?.Skills?.Common != null
                && profile.Characters?.Scav?.Skills?.Common != null
                && profile.Characters.Scav.Skills.Common.Length == 0;

            bool NeedToAddMissingMasteringsSkills() => AppData.AppSettings.AutoAddMissingMasterings
                && profile.Characters?.Pmc?.Skills?.Mastering != null
                && profile.Characters?.Scav?.Skills?.Mastering != null;
        }

        public void Save(string targetPath, string savePath = null)
        {
            new ProfileSaver(this).Save(targetPath, savePath);
        }

        public bool IsProfileChanged() => ProfileHash != 0 && ProfileHash != JsonConvert.SerializeObject(this).ToString().GetHashCode();
    }
}