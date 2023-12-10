using Newtonsoft.Json;
using SPT_AKI_Profile_Editor.Core.ProfileClasses;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SPT_AKI_Profile_Editor.Core.ProgressTransfer
{
    public class ProgressTransferService
    {
        private static JsonSerializerSettings SeriSettings => new()
        {
            Formatting = Formatting.Indented,
            NullValueHandling = NullValueHandling.Ignore
        };

        public static void ImportProgress(SettingsModel settings, Profile profile, string filePath)
        {
        }

        public static void ExportProgress(SettingsModel settings, Profile profile, string filePath)
        {
            var pmc = profile?.Characters?.Pmc;
            var profileProgress = new ProfileProgress();
            if (settings.Info.GroupState != false)
                ExportCharactersInfo(settings, profile, pmc, profileProgress);
            if (settings.Merchants)
                profileProgress.Merchants = pmc?.TraderStandings
                    .Select(x => ExportMerchant(x))
                    .ToArray();
            if (settings.Quests)
                profileProgress.Quests = pmc?.Quests
                    .ToDictionary(x => x.Qid, x => x.Status);
            if (settings.Hideout)
                profileProgress.Hideout = pmc?.Hideout?.Areas
                    .ToDictionary(x => x.Type, x => x.Level);
            if (settings.Crafts)
                profileProgress.Crafts = pmc?.UnlockedInfo;
            if (settings.ExaminedItems)
                profileProgress.ExaminedItems = pmc?.Encyclopedia;
            if (settings.Clothing)
                profileProgress.Clothing = profile?.Suits;
            if (settings.Skills.GroupState != false)
                ExportCommonSkills(settings, profile, profileProgress);
            if (settings.Masterings.GroupState != false)
                ExportMasteringSkills(settings, profile, profileProgress);

            string json = JsonConvert.SerializeObject(profileProgress, SeriSettings);
            File.WriteAllText(filePath, json);
        }

        private static void ExportCharactersInfo(SettingsModel settings,
                                                 Profile profile,
                                                 Character pmc,
                                                 ProfileProgress profileProgress)
        {
            profileProgress.Info = new();
            ExportInfo(settings.Info.Pmc,
                       pmc,
                       CreatePmcInfoProgress(profileProgress));
            ExportInfo(settings.Info.Scav,
                       profile?.Characters?.Scav,
                       CreateScavInfoProgress(profileProgress));
        }

        private static void ExportMasteringSkills(SettingsModel settings,
                                                  Profile profile,
                                                  ProfileProgress profileProgress)
            => ExportSkills(settings.Masterings,
                            profile?.Characters?.Pmc?.Skills?.Mastering,
                            profile?.Characters?.Scav?.Skills?.Mastering,
                            () =>
                            {
                                profileProgress.MasteringSkills = new();
                                return profileProgress.MasteringSkills;
                            });

        private static void ExportCommonSkills(SettingsModel settings,
                                               Profile profile,
                                               ProfileProgress profileProgress)
            => ExportSkills(settings.Skills,
                            profile?.Characters?.Pmc?.Skills?.Common,
                            profile?.Characters?.Scav?.Skills?.Common,
                            () =>
                            {
                                profileProgress.CommonSkills = new();
                                return profileProgress.CommonSkills;
                            });

        private static Func<ProfileProgress.InfoProgress.Character> CreateScavInfoProgress(ProfileProgress profileProgress)
            => () =>
            {
                profileProgress.Info.Scav = new();
                return profileProgress.Info.Scav;
            };

        private static Func<ProfileProgress.InfoProgress.Character> CreatePmcInfoProgress(ProfileProgress profileProgress)
            => () =>
            {
                profileProgress.Info.Pmc = new();
                return profileProgress.Info.Pmc;
            };

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
                    characterInfo.Nickname = character?.Info?.Nickname;
                if (info.SideEnabled && info.Side)
                    characterInfo.Side = character?.Info?.Side;
                if (info.Voice)
                    characterInfo.Voice = character?.Info?.Voice;
                if (info.Experience && character?.Info?.Experience != null)
                    characterInfo.Experience = character.Info.Experience;
                if (info.Head)
                    characterInfo.Head = character?.Customization?.Head;
                if (info.Pockets)
                    characterInfo.Pockets = character?.Inventory?.Pockets;
                if (info.Health)
                    characterInfo.HealthMetrics = new(character?.Health);
            }
        }

        private static void ExportSkills(SkillGroup group,
                                         CharacterSkill[] pmcSkils,
                                         CharacterSkill[] scavSkils,
                                         Func<ProfileProgress.SkillsProgress> createFunc)
        {
            var progress = createFunc();
            if (group.Pmc)
                progress.Pmc = pmcSkils?.ToDictionary(x => x.Id, x => x.Progress);
            if (group.Scav)
                progress.Scav = scavSkils?.ToDictionary(x => x.Id, x => x.Progress);
        }
    }
}