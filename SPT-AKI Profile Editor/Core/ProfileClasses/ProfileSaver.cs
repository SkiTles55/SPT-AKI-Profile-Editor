using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SPT_AKI_Profile_Editor.Core.Enums;
using SPT_AKI_Profile_Editor.Core.ServerClasses;
using SPT_AKI_Profile_Editor.Helpers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
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
                SaveEntry.Bonuses => AppData.AppLocalization.GetLocalizedString("tab_stash_additional_lines"),
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
            HideoutCrafts,
            Bonuses
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
            WriteTraders(pmc, profile.Characters.Pmc);
            WriteTraders(scav, profile.Characters.Scav);
            WriteQuests(pmc);
            WriteHideout(pmc, profile.Characters.Pmc.Inventory, out string newStash);
            WriteHideoutCrafts(pmc);
            WriteSkills(profile.Characters.Pmc.Skills.Common, pmc, "Common", SaveEntry.CommonSkillsPmc);
            WriteSkills(profile.Characters.Scav.Skills.Common, scav, "Common", SaveEntry.CommonSkillsScav);
            WriteSkills(profile.Characters.Pmc.Skills.Mastering, pmc, "Mastering", SaveEntry.MasteringSkillsPmc);
            WriteSkills(profile.Characters.Scav.Skills.Mastering, scav, "Mastering", SaveEntry.MasteringSkillsScav);
            WriteSuits(jobject);
            WriteStashBonus(pmc);
            // Stash writing always must be after after writing hideout due to update stashes by hideout area stages
            WriteStash(pmc, profile.Characters.Pmc.Inventory, newStash, SaveEntry.StashPmc);
            WriteStash(scav, profile.Characters.Scav.Inventory, null, SaveEntry.StashScav);
            WriteUserBuilds(jobject);
            if (!exceptions.HaveAllErrors())
            {
                string json = SerializeProfile(jobject);
                File.WriteAllText(savePath, json);
            }
            return exceptions;
        }

        private static string SerializeProfile(JObject profileObject)
        {
            JsonSerializer jsonSerializer = JsonSerializer.CreateDefault(SeriSettings);
            jsonSerializer.Formatting = Formatting.Indented;

            StringBuilder sb = new(256);
            StringWriter sw = new(sb, CultureInfo.InvariantCulture);
            using (JsonTextWriter jsonWriter = new(sw))
            {
                jsonWriter.Formatting = jsonSerializer.Formatting;
                jsonWriter.IndentChar = '\t';
                jsonWriter.Indentation = 1;

                jsonSerializer.Serialize(jsonWriter, profileObject);
            }

            return sw.ToString();
        }

        private void WriteSuits(JObject jobject)
        {
            try { jobject.SelectToken("suits").Replace(JToken.FromObject(profile.Suits.ToArray())); }
            catch (Exception ex) { exceptions.Add(new(SaveEntry.Suits, ex)); }
        }

        private void WriteTraders(JToken token, Character character)
        {
            try
            {
                JToken TradersInfo = token.SelectToken("TradersInfo");
                var tradersIdsForRemove = profile.ModdedEntitiesForRemoving
                    .Where(x => x.Type == ModdedEntityType.Merchant)
                    .Select(x => x.Id);
                if (tradersIdsForRemove.Any())
                    TradersInfo.RemoveFields(tradersIdsForRemove);
                foreach (var trader in character.TraderStandings.Where(x => !tradersIdsForRemove.Contains(x.Key)))
                {
                    JToken traderToken = TradersInfo.SelectToken($"['{trader.Key}']");
                    var traderInfo = character.TraderStandings[trader.Key];
                    traderToken["loyaltyLevel"] = traderInfo.LoyaltyLevel;
                    traderToken["salesSum"] = traderInfo.SalesSum;
                    traderToken["standing"] = Math.Round(traderInfo.Standing, 2);
                    traderToken["unlocked"] = traderInfo.Unlocked;
                }
                token.SelectToken("RagfairInfo")["rating"] = Math.Round(character.RagfairInfo.Rating, 2);
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
                List<JToken> questsForRemove = new();

                var questsObject = questsToken.ToObject<CharacterQuest[]>();
                if (questsObject.Any())
                {
                    for (int index = 0; index < questsObject.Length; ++index)
                    {
                        JToken questToken = questsToken[index];
                        var quest = questToken.ToObject<CharacterQuest>();

                        var edited = profile.Characters.Pmc.Quests.Where(x => x.Qid == quest.Qid).FirstOrDefault();
                        if (edited != null)
                        {
                            if (quest != null && quest.Status != edited.Status)
                            {
                                questToken["status"] = edited.Status.ToString();
                                questToken["statusTimers"] = JObject.FromObject(edited.StatusTimers);
                                if (edited.Status <= QuestStatus.AvailableForStart && questToken["completedConditions"] != null)
                                    questToken["completedConditions"]?.Replace(JToken.FromObject(Array.Empty<string>()));
                            }
                        }
                        else
                            questsForRemove.Add(questToken);
                    }

                    foreach (var token in questsForRemove)
                        token.Remove();

                    foreach (var quest in profile.Characters.Pmc.Quests.Where(x => !questsObject.Any(y => y.Qid == x.Qid)))
                    {
                        var lastQuestToken = questsToken.LastOrDefault();
                        if (lastQuestToken != null)
                            lastQuestToken.AddAfterSelf(JObject.FromObject(quest));
                        else
                        {
                            questsToken.Replace(JToken.FromObject(new CharacterQuest[] { quest }));
                            questsToken = pmc.SelectToken("Quests");
                        }
                    }
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

        private void WriteStashBonus(JToken pmc)
        {
            try
            {
                JToken bonusesToken = pmc.SelectToken("Bonuses");
                var bonusesObject = bonusesToken.ToObject<CharacterBonus[]>();
                var isStashRowsBonusUpdate = profile.Characters.Pmc.StashRowsBonusCount > 0;
                if (bonusesObject.Length > 0)
                {
                    var bonusEdited = false;
                    JToken forRemove = null;
                    for (int index = 0; index < bonusesObject.Length; ++index)
                    {
                        JToken bonusToken = bonusesToken[index];
                        var probe = bonusToken?.ToObject<CharacterBonus>();
                        if (probe != null && probe.Type == CharacterBonus.StashRowsType)
                        {
                            if (isStashRowsBonusUpdate)
                            {
                                bonusToken["value"] = profile.Characters.Pmc.StashRowsBonusCount;
                                bonusEdited = true;
                            }
                            else
                                forRemove = bonusToken;
                            break;
                        }
                    }
                    forRemove?.Remove();
                    if (!bonusEdited && isStashRowsBonusUpdate)
                    {
                        var bonusToken = CharacterBonus.CreateStashRowsBonus(profile.Characters.Pmc.StashRowsBonusCount);
                        pmc.SelectToken("Bonuses").LastOrDefault().AddAfterSelf(JObject.FromObject(bonusToken).RemoveNullAndEmptyProperties());
                    }
                }
                else if (isStashRowsBonusUpdate)
                    bonusesToken.Replace(JObject.FromObject(profile.Characters.Pmc.Bonuses).RemoveNullAndEmptyProperties());
            }
            catch (Exception ex) { exceptions.Add(new(SaveEntry.Bonuses, ex)); }
        }

        private void WriteStash(JToken characterToken, CharacterInventory inventory, string newStash, SaveEntry entry)
        {
            try
            {
                List<JToken> ForRemove = new();
                var inventoryToken = characterToken.SelectToken("Inventory");
                JToken itemsToken = inventoryToken?.SelectToken("items");
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
                inventoryToken?.SelectToken("hideoutAreaStashes").Replace(JToken.FromObject(inventory.HideoutAreaStashes));
            }
            catch (Exception ex) { exceptions.Add(new(entry, ex)); }
        }

        private void WriteHideout(JToken pmc, CharacterInventory inventory, out string newStash)
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
                    var areaDBInfo = AppData.ServerDatabase.HideoutAreaInfos
                        .Where(x => x.Type == probe.Type)
                        .FirstOrDefault();
                    if (areaInfo.Level > 0 && areaInfo.Level > probe.Level)
                    {
                        for (int l = probe.Level; l <= areaInfo.Level; l++)
                        {
                            var areaBonuses = areaDBInfo.Stages[l.ToString()];
                            if (areaBonuses == null)
                                continue;
                            var areaInfoObject = JObject.Parse(areaBonuses.ToString());
                            var BonusesList = areaInfoObject.SelectToken("bonuses").ToObject<List<JToken>>();
                            if (BonusesList == null || BonusesList.Count == 0)
                                continue;
                            foreach (var listItem in BonusesList)
                            {
                                var bonus = listItem.ToObject<CharacterBonus>();
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
                    UpdateStashForHideoutArea(areaDBInfo, areaInfo.Type.ToString(), areaInfo.Level, inventory);
                }
            }
            catch (Exception ex) { exceptions.Add(new(SaveEntry.Hideout, ex)); }
        }

        private void UpdateStashForHideoutArea(HideoutAreaInfo hideoutAreaInfo, string type, int level, CharacterInventory inventory)
        {
            var areaInfoObject = JObject.Parse(hideoutAreaInfo.Stages[level.ToString()].ToString());
            var areaStageContainer = areaInfoObject.SelectToken("container").ToObject<string>();
            if (!string.IsNullOrEmpty(areaStageContainer))
            {
                inventory.HideoutAreaStashes[type] = hideoutAreaInfo.Id;
                var inventoryItem = inventory.Items.FirstOrDefault(x => x.Id == hideoutAreaInfo.Id);
                if (inventoryItem != null)
                    inventoryItem.Tpl = areaStageContainer;
                else
                    inventory.Items = inventory.Items.Append(new InventoryItem() { Id = hideoutAreaInfo.Id, Tpl = areaStageContainer }).ToArray();
            }
            else
            {
                inventory.HideoutAreaStashes.Remove(type);
                inventory.Items = inventory.Items.Where(x => x.Id != hideoutAreaInfo.Id).ToArray();
            }

            /****
             * from Hideout controller
             * // Edge case, add/update `stand1/stand2/stand3` children
        if (dbHideoutArea.type === HideoutAreas.EQUIPMENT_PRESETS_STAND) {
            // Can have multiple 'standx' children depending on upgrade level
            this.addMissingPresetStandItemsToProfile(sessionID, hideoutStage, pmcData, dbHideoutArea, output);
        }

        // Dont inform client when upgraded area is hall of fame or equipment stand, BSG doesn't inform client this specifc upgrade has occurred
        // will break client if sent
        if (![HideoutAreas.PLACE_OF_FAME].includes(dbHideoutArea.type)) {
            this.addContainerUpgradeToClientOutput(sessionID, dbHideoutArea.type, dbHideoutArea, hideoutStage, output);
        }

        // Some hideout areas (Gun stand) have child areas linked to it
        const childDbArea = this.databaseService
            .getHideout()
            .areas.find((area) => area.parentArea === dbHideoutArea._id);
        if (childDbArea) {
            // Add key/value to `hideoutAreaStashes` dictionary - used to link hideout area to inventory stash by its id
            if (!pmcData.Inventory.hideoutAreaStashes[childDbArea.type]) {
                pmcData.Inventory.hideoutAreaStashes[childDbArea.type] = childDbArea._id;
            }

            // Set child area level to same as parent area
            pmcData.Hideout.Areas.find((hideoutArea) => hideoutArea.type === childDbArea.type).level =
                pmcData.Hideout.Areas.find((x) => x.type === profileParentHideoutArea.type).level;

            // Add/upgrade stash item in player inventory
            const childDbAreaStage = childDbArea.stages[profileParentHideoutArea.level];
            this.addUpdateInventoryItemToProfile(sessionID, pmcData, childDbArea, childDbAreaStage);

            // Inform client of the changes
            this.addContainerUpgradeToClientOutput(sessionID, childDbArea.type, childDbArea, childDbAreaStage, output);
        }
            ***/
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
                // Saving magazineBuilds for returning them back later
                var magazineBulds = jobject.SelectToken("userbuilds")["magazineBuilds"];

                profile.UserBuilds.RemoveParentsFromBuilds();
                jobject.SelectToken("userbuilds").Replace(JObject.FromObject(profile.UserBuilds).RemoveNullAndEmptyProperties());
                if (profile.UserBuilds.EquipmentBuilds?.Any() != true)
                    jobject.SelectToken("userbuilds")["equipmentBuilds"] = JToken.FromObject(Array.Empty<EquipmentBuild>());
                if (profile.UserBuilds.WeaponBuilds?.Any() != true)
                    jobject.SelectToken("userbuilds")["weaponBuilds"] = JToken.FromObject(Array.Empty<WeaponBuild>());
                // Returning previous magazineBuilds
                jobject.SelectToken("userbuilds")["magazineBuilds"] = magazineBulds;
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