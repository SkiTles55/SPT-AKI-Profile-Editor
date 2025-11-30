using Newtonsoft.Json;
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
        public List<ModdedEntity> ModdedEntitiesForRemoving = [];

        private ProfileCharacters characters;
        private UserBuilds userBuilds;
        private int profileHash = 0;
        private CustomisationUnlock[] customisationUnlocks;

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

        [JsonProperty("customisationUnlocks")]
        public CustomisationUnlock[] CustomisationUnlocks
        {
            get => customisationUnlocks;
            set
            {
                customisationUnlocks = value;
                OnPropertyChanged(nameof(CustomisationUnlocks));
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
                profile.Characters.Pmc.UpdateQuestsData();
            if (NeedToAddMissingMasteringsSkills())
            {
                AddMissingMasteringSkills(profile.Characters.Pmc.Skills);
                AddMissingMasteringSkills(profile.Characters.Scav.Skills);
            }
            AddMisingHeadToServerDatabase(profile.Characters?.Pmc);
            AddMisingHeadToServerDatabase(profile.Characters?.Scav);
            Characters = profile.Characters;
            CustomisationUnlocks = profile.customisationUnlocks;
            UserBuilds = profile.UserBuilds;

            static void AddMisingHeadToServerDatabase(Character character)
            {
                if (!string.IsNullOrEmpty(character?.Customization?.Head)
                && !AppData.ServerDatabase.Heads.Any(x => x.Key == character.Customization.Head))
                {
                    var existHeads = new Dictionary<string, string>(AppData.ServerDatabase.Heads)
                    {
                        { character.Customization.Head, character.Customization.Head }
                    };
                    AppData.ServerDatabase.Heads = existHeads;
                }
            }

            static void AddMissingMasteringSkills(CharacterSkills characterSkills)
            {
                if (characterSkills.Mastering.Length != AppData.ServerDatabase.ServerGlobals.Config.Mastering.Length)
                {
                    characterSkills.Mastering =
                    [
                        .. characterSkills.Mastering,
                        .. AppData.ServerDatabase.ServerGlobals.Config.Mastering
                            .Where(x => !characterSkills.Mastering.Any(y => y.Id == x.Name))
                            .Select(x => new CharacterSkill { Id = x.Name, Progress = 0 }),
                    ];
                }
            }

            bool NeedToAddMissingMasteringsSkills() => AppData.AppSettings.AutoAddMissingMasterings
                && profile.Characters?.Pmc?.Skills?.Mastering != null
                && profile.Characters?.Scav?.Skills?.Mastering != null;
        }

        public List<SaveException> Save(string targetPath, string savePath = null)
            => new ProfileSaver(this, AppData.AppSettings, AppData.ServerDatabase).Save(targetPath, savePath);

        public bool IsProfileChanged()
            => ProfileHash != 0 && ProfileHash != JsonConvert.SerializeObject(this).ToString().GetHashCode();
    }
}