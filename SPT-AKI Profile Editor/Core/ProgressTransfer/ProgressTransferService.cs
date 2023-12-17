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
            string fileText = File.ReadAllText(filePath);
            ProfileProgress importedProgress = JsonConvert.DeserializeObject<ProfileProgress>(fileText);
            var pmc = profile?.Characters?.Pmc;
            if (settings.Info.GroupState != false)
                ImportCharactersInfo(settings.Info, importedProgress.Info, pmc, profile?.Characters?.Scav);
            if (settings.Merchants && importedProgress.Merchants != null)
                ImportMerchants(importedProgress, pmc);
            if (settings.Quests && importedProgress.Quests != null)
                ImportQuests(importedProgress, pmc);
            if (settings.Hideout && importedProgress.Hideout != null)
                ImportHideout(importedProgress, pmc);
            if (settings.Crafts && importedProgress.Crafts != null)
                pmc.UnlockedInfo = new() { UnlockedProductionRecipe = importedProgress.Crafts.UnlockedProductionRecipe };
            if (settings.ExaminedItems &&  importedProgress.ExaminedItems != null)
                pmc.Encyclopedia = importedProgress.ExaminedItems;
            if (settings.Clothing &&  importedProgress.Clothing != null)
                profile.Suits = importedProgress.Clothing;
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
                    .DistinctBy(x => x.Qid)
                    .Where(x => x.Type == Enums.QuestType.Standart)
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
            ExportBuilds(settings, profile, profileProgress);

            string json = JsonConvert.SerializeObject(profileProgress, SeriSettings);
            File.WriteAllText(filePath, json);
        }

        #region Import functions

        private static void ImportCharactersInfo(Info infoSettings,
                                                 ProfileProgress.InfoProgress importedInfoProgress,
                                                 Character pmc,
                                                 Character scav)
        {
            ImportInfo(infoSettings.Pmc, pmc, importedInfoProgress?.Pmc);
            ImportInfo(infoSettings.Scav, scav, importedInfoProgress?.Scav);
        }

        private static void ImportInfo(InfoGroup info,
                                       Character character,
                                       ProfileProgress.InfoProgress.Character imported)
        {
            if (imported == null || info.GroupState == false)
                return;
            if (info.Nickname && imported.Nickname != null)
                character.Info.Nickname = imported.Nickname;
            if (info.SideEnabled && info.Side && imported.Side != null)
                character.Info.Side = imported.Side;
            if (info.Voice && imported.Voice != null)
                character.Info.Voice = imported.Voice;
            if (info.Experience && imported.Experience != null)
                character.Info.Experience = imported.Experience ?? 0;
            if (info.Head && imported.Head != null)
                character.Customization.Head = imported.Head;
            if (info.Pockets && imported.Pockets != null)
                character.Inventory.Pockets = imported.Pockets;
            if (info.Health && imported.HealthMetrics != null)
            {
                character.Health.BodyParts.Head.Health.Maximum = imported.HealthMetrics.Head.Maximum;
                character.Health.BodyParts.Head.Health.Current = imported.HealthMetrics.Head.Current;

                character.Health.BodyParts.Chest.Health.Maximum = imported.HealthMetrics.Chest.Maximum;
                character.Health.BodyParts.Chest.Health.Current = imported.HealthMetrics.Chest.Current;

                character.Health.BodyParts.Stomach.Health.Maximum = imported.HealthMetrics.Stomach.Maximum;
                character.Health.BodyParts.Stomach.Health.Current = imported.HealthMetrics.Stomach.Current;

                character.Health.BodyParts.LeftArm.Health.Maximum = imported.HealthMetrics.LeftArm.Maximum;
                character.Health.BodyParts.LeftArm.Health.Current = imported.HealthMetrics.LeftArm.Current;

                character.Health.BodyParts.RightArm.Health.Maximum = imported.HealthMetrics.RightArm.Maximum;
                character.Health.BodyParts.RightArm.Health.Current = imported.HealthMetrics.RightArm.Current;

                character.Health.BodyParts.LeftLeg.Health.Maximum = imported.HealthMetrics.LeftLeg.Maximum;
                character.Health.BodyParts.LeftLeg.Health.Current = imported.HealthMetrics.LeftLeg.Current;

                character.Health.BodyParts.RightLeg.Health.Maximum = imported.HealthMetrics.RightLeg.Maximum;
                character.Health.BodyParts.RightLeg.Health.Current = imported.HealthMetrics.RightLeg.Current;

                character.Health.Hydration.Maximum = imported.HealthMetrics.Hydration.Maximum;
                character.Health.Hydration.Current = imported.HealthMetrics.Hydration.Current;

                character.Health.Energy.Maximum = imported.HealthMetrics.Energy.Maximum;
                character.Health.Energy.Current = imported.HealthMetrics.Energy.Current;
            }
        }

        private static void ImportMerchants(ProfileProgress importedProgress, Character pmc)
        {
            foreach (var importedMerchant in importedProgress.Merchants)
            {
                var profileMerchant = pmc.TraderStandings.FirstOrDefault(x => x.Key == importedMerchant.Id);
                if (profileMerchant.Value == null)
                    continue;
                profileMerchant.Value.LoyaltyLevel = importedMerchant.Level;
                profileMerchant.Value.Standing = importedMerchant.Standing;
                profileMerchant.Value.SalesSum = importedMerchant.SalesSum;
                profileMerchant.Value.Unlocked = importedMerchant.Enabled;
            }
            pmc.NotifyTradersUpdated();
        }

        private static void ImportQuests(ProfileProgress importedProgress, Character pmc)
        {
            pmc.RemoveQuests(pmc.Quests.Select(x => x.Qid).Where(x => !importedProgress.Quests.ContainsKey(x)));
            foreach (var importedQuest in importedProgress.Quests)
            {
                var existQuest = pmc.Quests.FirstOrDefault(x => x.Qid == importedQuest.Key);
                if (existQuest != null)
                    existQuest.Status = importedQuest.Value;
                else
                    pmc.AddQuest(new() { Qid = importedQuest.Key, Status = importedQuest.Value });
            }
        }

        private static void ImportHideout(ProfileProgress importedProgress, Character pmc)
        {
            foreach (var importedHideout in importedProgress.Hideout)
            {
                var hideoutArea = pmc.Hideout.Areas.FirstOrDefault(x => x.Type == importedHideout.Key);
                if (hideoutArea == null)
                    continue;
                hideoutArea.Level = importedHideout.Value;
            }
        }

        #endregion Import functions

        #region Export functions

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

        private static ProfileProgress.Merchant ExportMerchant(KeyValuePair<string, CharacterTraderStanding> x)
            => new(x.Key,
                    x.Value.Unlocked,
                    x.Value.LoyaltyLevel,
                    x.Value.Standing,
                    x.Value.SalesSum);

        private static void ExportBuilds(SettingsModel settings,
                                         Profile profile,
                                         ProfileProgress profileProgress)
        {
            if (settings.Builds.GroupState != false)
            {
                profileProgress.Builds = new();
                if (settings.Builds.WeaponBuilds)
                    profileProgress.Builds.WeaponsBuilds = profile.UserBuilds.WeaponBuilds;
                if (settings.Builds.EquipmentBuilds)
                    profileProgress.Builds.EquipmentBuilds = profile.UserBuilds.EquipmentBuilds;
            }
        }

        #endregion Export functions
    }
}