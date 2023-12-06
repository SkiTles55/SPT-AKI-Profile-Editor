using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SPT_AKI_Profile_Editor.Core.Enums;
using SPT_AKI_Profile_Editor.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using static SPT_AKI_Profile_Editor.Core.ProfileClasses.ProfileSaver;

namespace SPT_AKI_Profile_Editor.Core.ProfileClasses
{
    public static class SaveEntryExtension
    {
        public static string LocalizedName(this SaveEntry entry)
            => entry switch
            {
                SaveEntry.UserBuilds => AppData.AppLocalization.GetLocalizedString("tab_presets_title"),
                SaveEntry.Suits => AppData.AppLocalization.GetLocalizedString("tab_clothing_title"),
                SaveEntry.Traders => AppData.AppLocalization.GetLocalizedString("tab_merchants_title"),
                SaveEntry.Encyclopedia => AppData.AppLocalization.GetLocalizedString("tab_examined_items_title"),
                SaveEntry.CharacterInfoPmc => $"{AppData.AppLocalization.GetLocalizedString("tab_info_title")} ({AppData.AppLocalization.GetLocalizedString("tab_info_pmc")})",
                SaveEntry.CharacterInfoScav => $"{AppData.AppLocalization.GetLocalizedString("tab_info_title")} ({AppData.AppLocalization.GetLocalizedString("tab_info_scav")})",
                SaveEntry.CharacterHealthPmc => $"{AppData.AppLocalization.GetLocalizedString("tab_info_health")} ({AppData.AppLocalization.GetLocalizedString("tab_info_pmc")})",
                SaveEntry.CharacterHealthScav => $"{AppData.AppLocalization.GetLocalizedString("tab_info_health")} ({AppData.AppLocalization.GetLocalizedString("tab_info_scav")})",
                SaveEntry.Quests => AppData.AppLocalization.GetLocalizedString("tab_quests_title"),
                SaveEntry.CommonSkillsPmc => $"{AppData.AppLocalization.GetLocalizedString("tab_skills_title")} ({AppData.AppLocalization.GetLocalizedString("tab_info_pmc")})",
                SaveEntry.CommonSkillsScav => $"{AppData.AppLocalization.GetLocalizedString("tab_skills_title")} ({AppData.AppLocalization.GetLocalizedString("tab_info_scav")})",
                SaveEntry.MasteringSkillsPmc => $"{AppData.AppLocalization.GetLocalizedString("tab_mastering_title")} ({AppData.AppLocalization.GetLocalizedString("tab_info_pmc")})",
                SaveEntry.MasteringSkillsScav => $"{AppData.AppLocalization.GetLocalizedString("tab_mastering_title")} ({AppData.AppLocalization.GetLocalizedString("tab_info_scav")})",
                SaveEntry.StashPmc => $"{AppData.AppLocalization.GetLocalizedString("tab_stash_title")} ({AppData.AppLocalization.GetLocalizedString("tab_info_pmc")})",
                SaveEntry.StashScav => $"{AppData.AppLocalization.GetLocalizedString("tab_stash_title")} ({AppData.AppLocalization.GetLocalizedString("tab_info_scav")})",
                SaveEntry.Hideout => $"{AppData.AppLocalization.GetLocalizedString("tab_hideout_title")} ({AppData.AppLocalization.GetLocalizedString("tab_hideout_zones")})",
                SaveEntry.HideoutCrafts => $"{AppData.AppLocalization.GetLocalizedString("tab_hideout_title")} ({AppData.AppLocalization.GetLocalizedString("tab_hideout_crafts")})",
                _ => entry.ToString(),
            };
    }

    public static class IEnumerableExtension
    {
        public static bool HaveAllErrors(this IEnumerable<SaveException> entries)
            => Enum.GetNames(typeof(SaveEntry)).Length == entries.Count();

        public static string GetLocalizedDescription(this IEnumerable<SaveException> exceptions)
            => string.Join("\n", exceptions.Select(x => $"{x.Entry.LocalizedName()}: {x.Exception.Message}"));
    }

    public class ProfileSaver
    {
        private readonly Profile profile;
        private readonly List<SaveException> exceptions = new();

        public ProfileSaver(Profile profile) => this.profile = profile;

        public enum SaveEntry
        {
            Suits,
            Traders,
            Encyclopedia,
            CharacterInfoPmc,
            CharacterInfoScav,
            CharacterHealthPmc,
            CharacterHealthScav,
            Quests,
            CommonSkillsPmc,
            CommonSkillsScav,
            MasteringSkillsPmc,
            MasteringSkillsScav,
            StashPmc,
            StashScav,
            Hideout,
            UserBuilds,
            HideoutCrafts
        }

        private static JsonSerializerSettings SeriSettings => new()
        {
            Formatting = Formatting.Indented,
            Converters = new List<JsonConverter>() { new StringEnumConverterExt() }
        };

        public List<SaveException> Save(string targetPath, string savePath = null)
        {
            if (string.IsNullOrEmpty(savePath))
                savePath = targetPath;
            JObject jobject = JObject.Parse(File.ReadAllText(targetPath));
            JToken pmc = jobject.SelectToken("characters")["pmc"];
            JToken scav = jobject.SelectToken("characters")["scav"];
            WriteCharacterInfo(pmc, profile.Characters.Pmc, SaveEntry.CharacterInfoPmc);
            WriteCharacterInfo(scav, profile.Characters.Scav, SaveEntry.CharacterInfoScav);
            WriteCharacterHealth(pmc, profile.Characters.Pmc.Health, SaveEntry.CharacterHealthPmc);
            WriteCharacterHealth(scav, profile.Characters.Scav.Health, SaveEntry.CharacterHealthScav);
            WriteEncyclopedia(pmc);
            WriteTraders(pmc);
            WriteQuests(pmc);
            WriteHideout(pmc, out string newStash);
            WriteHideoutCrafts(pmc);
            WriteSkills(profile.Characters.Pmc.Skills.Common, pmc, "Common", SaveEntry.CommonSkillsPmc);
            WriteSkills(profile.Characters.Scav.Skills.Common, scav, "Common", SaveEntry.CommonSkillsScav);
            WriteSkills(profile.Characters.Pmc.Skills.Mastering, pmc, "Mastering", SaveEntry.MasteringSkillsPmc);
            WriteSkills(profile.Characters.Scav.Skills.Mastering, scav, "Mastering", SaveEntry.MasteringSkillsScav);
            WriteSuits(jobject);
            WriteStash(pmc, profile.Characters.Pmc.Inventory, newStash, SaveEntry.StashPmc);
            WriteStash(scav, profile.Characters.Scav.Inventory, null, SaveEntry.StashScav);
            WriteUserBuilds(jobject);
            if (!exceptions.HaveAllErrors())
            {
                string json = JsonConvert.SerializeObject(jobject, SeriSettings);
                File.WriteAllText(savePath, json);
            }
            return exceptions;
        }

        private void WriteSuits(JObject jobject)
        {
            try { jobject.SelectToken("suits").Replace(JToken.FromObject(profile.Suits.ToArray())); }
            catch (Exception ex) { exceptions.Add(new(SaveEntry.Suits, ex)); }
        }

        private void WriteTraders(JToken pmc)
        {
            try
            {
                JToken TradersInfo = pmc.SelectToken("TradersInfo");
                var tradersIdsForRemove = profile.ModdedEntitiesForRemoving
                    .Where(x => x.Type == ModdedEntityType.Merchant)
                    .Select(x => x.Id);
                if (tradersIdsForRemove.Any())
                    TradersInfo.RemoveFields(tradersIdsForRemove);
                foreach (var trader in profile.Characters.Pmc.TraderStandings.Where(x => !tradersIdsForRemove.Contains(x.Key)))
                {
                    JToken traderToken = TradersInfo.SelectToken($"['{trader.Key}']");
                    var traderInfo = profile.Characters.Pmc.TraderStandings[trader.Key];
                    traderToken["loyaltyLevel"] = traderInfo.LoyaltyLevel;
                    traderToken["salesSum"] = traderInfo.SalesSum;
                    traderToken["standing"] = Math.Round(traderInfo.Standing, 2);
                    traderToken["unlocked"] = traderInfo.Unlocked;
                }
                pmc.SelectToken("RagfairInfo")["rating"] = Math.Round(profile.Characters.Pmc.RagfairInfo.Rating, 2);
            }
            catch (Exception ex) { exceptions.Add(new(SaveEntry.Traders, ex)); }
        }

        private void WriteEncyclopedia(JToken pmc)
        {
            try { pmc.SelectToken("Encyclopedia").Replace(JToken.FromObject(profile.Characters.Pmc.Encyclopedia)); }
            catch (Exception ex) { exceptions.Add(new(SaveEntry.Encyclopedia, ex)); }
        }

        private void WriteCharacterInfo(JToken character, Character profileCharacter, SaveEntry entry)
        {
            try
            {
                JToken infoToken = character.SelectToken("Info");
                infoToken["Nickname"] = profileCharacter.Info.Nickname;
                infoToken["Voice"] = profileCharacter.Info.Voice;
                infoToken["Level"] = profileCharacter.Info.Level;
                infoToken["Experience"] = profileCharacter.Info.Experience;
                character.SelectToken("Customization")["Head"] = profileCharacter.Customization.Head;

                if (!profileCharacter.IsScav)
                {
                    infoToken["LowerNickname"] = profileCharacter.Info.Nickname.ToLower();
                    infoToken["Side"] = profileCharacter.Info.Side;
                }
            }
            catch (Exception ex) { exceptions.Add(new(entry, ex)); }
        }

        private void WriteCharacterHealth(JToken character, CharacterHealth characterHealth, SaveEntry entry)
        {
            try
            {
                JToken healthToken = character.SelectToken("Health");
                healthToken["Energy"]["Current"] = (int)characterHealth.Energy.Current;
                healthToken["Energy"]["Maximum"] = (int)characterHealth.Energy.Maximum;
                healthToken["Hydration"]["Current"] = (int)characterHealth.Hydration.Current;
                healthToken["Hydration"]["Maximum"] = (int)characterHealth.Hydration.Maximum;
                healthToken["BodyParts"]["Head"]["Health"]["Current"] = (int)characterHealth.BodyParts.Head.Health.Current;
                healthToken["BodyParts"]["Head"]["Health"]["Maximum"] = (int)characterHealth.BodyParts.Head.Health.Maximum;
                healthToken["BodyParts"]["Chest"]["Health"]["Current"] = (int)characterHealth.BodyParts.Chest.Health.Current;
                healthToken["BodyParts"]["Chest"]["Health"]["Maximum"] = (int)characterHealth.BodyParts.Chest.Health.Maximum;
                healthToken["BodyParts"]["Stomach"]["Health"]["Current"] = (int)characterHealth.BodyParts.Stomach.Health.Current;
                healthToken["BodyParts"]["Stomach"]["Health"]["Maximum"] = (int)characterHealth.BodyParts.Stomach.Health.Maximum;
                healthToken["BodyParts"]["LeftArm"]["Health"]["Current"] = (int)characterHealth.BodyParts.LeftArm.Health.Current;
                healthToken["BodyParts"]["LeftArm"]["Health"]["Maximum"] = (int)characterHealth.BodyParts.LeftArm.Health.Maximum;
                healthToken["BodyParts"]["RightArm"]["Health"]["Current"] = (int)characterHealth.BodyParts.RightArm.Health.Current;
                healthToken["BodyParts"]["RightArm"]["Health"]["Maximum"] = (int)characterHealth.BodyParts.RightArm.Health.Maximum;
                healthToken["BodyParts"]["LeftLeg"]["Health"]["Current"] = (int)characterHealth.BodyParts.LeftLeg.Health.Current;
                healthToken["BodyParts"]["LeftLeg"]["Health"]["Maximum"] = (int)characterHealth.BodyParts.LeftLeg.Health.Maximum;
                healthToken["BodyParts"]["RightLeg"]["Health"]["Current"] = (int)characterHealth.BodyParts.RightLeg.Health.Current;
                healthToken["BodyParts"]["RightLeg"]["Health"]["Maximum"] = (int)characterHealth.BodyParts.RightLeg.Health.Maximum;
            }
            catch (Exception ex) { exceptions.Add(new(entry, ex)); }
        }

        private void WriteQuests(JToken pmc)
        {
            try
            {
                JToken questsToken = pmc.SelectToken("Quests");
                var questQidsForRemove = profile.ModdedEntitiesForRemoving
                    .Where(x => x.Type == ModdedEntityType.Quest)
                    .Select(x => x.Id);
                List<JToken> questsForRemove = new();

                var questsObject = questsToken.ToObject<CharacterQuest[]>();
                if (questsObject.Any())
                {
                    for (int index = 0; index < questsObject.Length; ++index)
                    {
                        JToken questToken = questsToken[index];
                        var quest = questToken.ToObject<CharacterQuest>();

                        if (questQidsForRemove.Contains(quest.Qid))
                        {
                            questsForRemove.Add(questToken);
                            continue;
                        }

                        var edited = profile.Characters.Pmc.Quests.Where(x => x.Qid == quest.Qid).FirstOrDefault();
                        if (edited != null && quest != null && quest.Status != edited.Status)
                        {
                            questToken["status"] = edited.Status.ToString();
                            questToken["statusTimers"] = JObject.FromObject(edited.StatusTimers);
                            if (edited.Status <= QuestStatus.AvailableForStart && questToken["completedConditions"] != null)
                                questToken["completedConditions"]?.Replace(JToken.FromObject(Array.Empty<string>()));
                        }
                    }

                    foreach (var token in questsForRemove)
                        token.Remove();

                    foreach (var quest in profile.Characters.Pmc.Quests.Where(x => !questsObject.Any(y => y.Qid == x.Qid)))
                        questsToken.LastOrDefault().AddAfterSelf(JObject.FromObject(quest));
                }
                else
                    questsToken.Replace(JToken.FromObject(profile.Characters.Pmc.Quests));
            }
            catch (Exception ex) { exceptions.Add(new(SaveEntry.Quests, ex)); }
        }

        private void WriteSkills(CharacterSkill[] skills, JToken character, string type, SaveEntry entry)
        {
            try
            {
                JToken skillsToken = character.SelectToken("Skills").SelectToken(type);
                var skillsObject = skillsToken.ToObject<CharacterSkill[]>();
                if (skillsObject.Length > 0)
                {
                    for (int index = 0; index < skillsObject.Length; ++index)
                    {
                        JToken skillToken = skillsToken[index];
                        var probe = skillToken?.ToObject<CharacterSkill>();
                        var edited = skills.Where(x => x.Id == probe.Id).FirstOrDefault();
                        if (edited != null && probe != null && edited.Progress != probe.Progress)
                            skillToken["Progress"] = edited.Progress;
                    }
                    foreach (var skill in skills.Where(x => !skillsObject.Any(y => y.Id == x.Id)))
                        skillsToken.LastOrDefault().AddAfterSelf(JObject.FromObject(skill));
                }
                else
                    skillsToken.Replace(JToken.FromObject(skills));
            }
            catch (Exception ex) { exceptions.Add(new(entry, ex)); }
        }

        private void WriteStash(JToken characterToken, CharacterInventory inventory, string newStash, SaveEntry entry)
        {
            try
            {
                List<JToken> ForRemove = new();
                JToken itemsToken = characterToken.SelectToken("Inventory")?.SelectToken("items");
                var itemsObject = itemsToken.ToObject<InventoryItem[]>();
                if (itemsObject.Length > 0)
                {
                    for (int index = 0; index < itemsObject.Length; ++index)
                    {
                        JToken itemToken = itemsToken[index];
                        var probe = itemToken?.ToObject<InventoryItem>();
                        if (probe == null)
                            continue;
                        if (!string.IsNullOrEmpty(newStash) && probe.Id == inventory.Stash && probe.Tpl != newStash)
                            itemToken["_tpl"] = newStash;
                        if (!inventory.Items.Any(x => x.Id == probe.Id))
                            ForRemove.Add(itemToken);
                        if (probe.IsPockets)
                            itemToken["_tpl"] = inventory.Pockets;
                    }
                    foreach (var removedItem in ForRemove)
                        removedItem.Remove();
                    JsonSerializer serializer = JsonSerializer.Create(SeriSettings);
                    foreach (var item in inventory.Items.Where(x => !itemsObject.Any(y => y.Id == x.Id)))
                        itemsToken?.LastOrDefault()?.AddAfterSelf(JObject.FromObject(item, serializer).RemoveNullAndEmptyProperties());
                }
            }
            catch (Exception ex) { exceptions.Add(new(entry, ex)); }
        }

        private void WriteHideout(JToken pmc, out string newStash)
        {
            newStash = string.Empty;
            try
            {
                JToken areasToken = pmc.SelectToken("Hideout").SelectToken("Areas");
                var hideoutAreasObject = areasToken.ToObject<HideoutArea[]>();
                for (int i = 0; i < hideoutAreasObject.Length; i++)
                {
                    JToken areaToken = areasToken[i];
                    var probe = areaToken.ToObject<HideoutArea>();
                    var areaInfo = profile.Characters.Pmc.Hideout.Areas.Where(x => x.Type == probe.Type).FirstOrDefault();
                    if (areaInfo == null)
                        continue;
                    if (areaInfo.Level > 0 && areaInfo.Level > probe.Level)
                    {
                        for (int l = probe.Level; l <= areaInfo.Level; l++)
                        {
                            var areaBonuses = AppData.ServerDatabase.HideoutAreaInfos
                                .Where(x => x.Type == probe.Type)
                                .FirstOrDefault()
                                .Stages[l.ToString()];
                            if (areaBonuses == null)
                                continue;
                            var BonusesList = JObject.Parse(areaBonuses.ToString()).SelectToken("bonuses").ToObject<List<JToken>>();
                            if (BonusesList == null || BonusesList.Count == 0)
                                continue;
                            foreach (var listItem in BonusesList)
                            {
                                var bonus = listItem.ToObject<CharacterBonuses>();
                                if (bonus == null)
                                    continue;
                                switch (bonus.Type)
                                {
                                    case "StashSize":
                                        newStash = bonus.TemplateId;
                                        break;

                                    case "MaximumEnergyReserve":
                                        pmc.SelectToken("Health").SelectToken("Energy")["Maximum"] = Math.Max(110, profile.Characters.Pmc.Health.Energy.Maximum);
                                        break;
                                }
                                pmc.SelectToken("Bonuses").LastOrDefault().AddAfterSelf(JObject.FromObject(listItem));
                            }
                        }
                    }
                    areaToken["level"] = areaInfo.Level;
                }
            }
            catch (Exception ex) { exceptions.Add(new(SaveEntry.Hideout, ex)); }
        }

        private void WriteHideoutCrafts(JToken pmc)
        {
            try
            {
                var crafts = profile.Characters.Pmc.HideoutProductions.Where(x => x.Added).Select(x => x.Production.Id).ToArray();
                pmc.SelectToken("UnlockedInfo")["unlockedProductionRecipe"].Replace(JToken.FromObject(crafts));
            }
            catch (Exception ex) { exceptions.Add(new(SaveEntry.HideoutCrafts, ex)); }
        }

        private void WriteUserBuilds(JObject jobject)
        {
            try
            {
                jobject.SelectToken("userbuilds").Replace(JObject.FromObject(profile.UserBuilds).RemoveNullAndEmptyProperties());
                if (profile.UserBuilds.EquipmentBuilds?.Any() != true)
                    jobject.SelectToken("userbuilds")["equipmentBuilds"] = JToken.FromObject(Array.Empty<EquipmentBuild>());
                if (profile.UserBuilds.WeaponBuilds?.Any() != true)
                    jobject.SelectToken("userbuilds")["weaponBuilds"] = JToken.FromObject(Array.Empty<WeaponBuild>());
            }
            catch (Exception ex) { exceptions.Add(new(SaveEntry.UserBuilds, ex)); }
        }
    }

    public class SaveException
    {
        public SaveException(SaveEntry entry, Exception exception)
        {
            Entry = entry;
            Exception = exception;
        }

        public SaveEntry Entry { get; }
        public Exception Exception { get; }
    }
}