using Newtonsoft.Json;
using SPT_AKI_Profile_Editor.Core.Enums;
using SPT_AKI_Profile_Editor.Core.ProfileClasses;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Character = SPT_AKI_Profile_Editor.Core.ProfileClasses.Character;

namespace SPT_AKI_Profile_Editor.Core.ProgressTransfer
{
    public class ProgressTransferService
    {
        public static void ImportProgress(SettingsModel settings, string filePath)
        {
        }

        public static void ExportProgress(SettingsModel settings, string filePath)
        {
            var profileProgress = new ProfileProgress();
            if (settings.Info.GroupState != false)
            {
                profileProgress.Info = new ProfileProgress.InfoProgress();
                ExportInfo(settings.Info.Pmc, AppData.Profile.Characters.Pmc, () =>
                {
                    profileProgress.Info.Pmc = new();
                    return profileProgress.Info.Pmc;
                });
                ExportInfo(settings.Info.Scav, AppData.Profile.Characters.Scav, () =>
                {
                    profileProgress.Info.Scav = new();
                    return profileProgress.Info.Scav;
                });
            }
            if (settings.Merchants)
                profileProgress.Merchants = AppData.Profile.Characters.Pmc.TraderStandings
                    .Select(x => ExportMerchant(x))
                    .ToArray();
            if (settings.Quests)
                profileProgress.Quests = AppData.Profile.Characters.Pmc.Quests
                    .ToDictionary(x => x.Qid, x => x.Status);
            string json = JsonConvert.SerializeObject(profileProgress);
            File.WriteAllText(filePath, json);
        }

        private static ProfileProgress.Merchant ExportMerchant(KeyValuePair<string, CharacterTraderStanding> x)
            => new(x.Key,
                    x.Value.Unlocked,
                    x.Value.LoyaltyLevel,
                    x.Value.Standing,
                    x.Value.SalesSum);

        private static void ExportInfo(InfoGroup info,
                                       Character character,
                                       Func<ProfileProgress.InfoProgress.Character> createFunc)
        {
            if (info.GroupState != false)
            {
                var characterInfo = createFunc();
                if (info.Nickname)
                    characterInfo.Nickname = character.Info.Nickname;
                if (info.SideEnabled && info.Side)
                    characterInfo.Side = character.Info.Side;
                if (info.Voice)
                    characterInfo.Voice = character.Info.Voice;
                if (info.Experience)
                    characterInfo.Experience = character.Info.Experience;
                if (info.Head)
                    characterInfo.Head = character.Customization.Head;
                if (info.Pockets)
                    characterInfo.Pockets = character.Inventory.Pockets;
                if (info.Health)
                    characterInfo.Health = character.Health;
            }
        }
    }

    public class ProfileProgress
    {
        public InfoProgress Info;
        public Merchant[] Merchants;
        public Dictionary<string, QuestStatus> Quests;

        public class InfoProgress
        {
            public Character Pmc;
            public Character Scav;

            public class Character
            {
                public string Nickname;
                public string Side;
                public string Voice;
                public long Experience;
                public string Head;
                public string Pockets;
                public CharacterHealth Health;
            }
        }

        public class Merchant
        {
            public string Id;
            public bool Enabled;
            public int Level;
            public float Standing;
            public long SalesSum;

            public Merchant(string id,
                            bool enabled,
                            int level,
                            float standing,
                            long salesSum)
            {
                Id = id;
                Enabled = enabled;
                Level = level;
                Standing = standing;
                SalesSum = salesSum;
            }
        }
    }
}