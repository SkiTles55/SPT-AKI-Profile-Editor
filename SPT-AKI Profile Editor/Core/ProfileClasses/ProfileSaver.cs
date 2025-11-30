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
        {
            var locale = AppData.AppLocalization;
            return entry switch
            {
                SaveEntry.UserBuilds => locale.GetLocalizedString(LocalizationKeys.Tabs.Presets),
                SaveEntry.Suits => locale.GetLocalizedString(LocalizationKeys.Tabs.Clothing),
                SaveEntry.Traders => locale.GetLocalizedString(LocalizationKeys.Tabs.Merchants),
                SaveEntry.Encyclopedia => locale.GetLocalizedString(LocalizationKeys.Tabs.ExaminedItems),
                SaveEntry.CharacterInfoPmc => $"{locale.GetLocalizedString(LocalizationKeys.Tabs.Info)} ({locale.GetLocalizedString(LocalizationKeys.Tabs.Pmc)})",
                SaveEntry.CharacterInfoScav => $"{locale.GetLocalizedString(LocalizationKeys.Tabs.Info)} ({locale.GetLocalizedString(LocalizationKeys.Tabs.Scav)})",
                SaveEntry.CharacterHealthPmc => $"{locale.GetLocalizedString(LocalizationKeys.Tabs.Health)} ({locale.GetLocalizedString(LocalizationKeys.Tabs.Pmc)})",
                SaveEntry.CharacterHealthScav => $"{locale.GetLocalizedString(LocalizationKeys.Tabs.Health)} ({locale.GetLocalizedString(LocalizationKeys.Tabs.Scav)})",
                SaveEntry.Quests => locale.GetLocalizedString(LocalizationKeys.Tabs.Quests),
                SaveEntry.CommonSkillsPmc => $"{locale.GetLocalizedString(LocalizationKeys.Tabs.Skills)} ({locale.GetLocalizedString(LocalizationKeys.Tabs.Pmc)})",
                SaveEntry.CommonSkillsScav => $"{locale.GetLocalizedString(LocalizationKeys.Tabs.Skills)} ({locale.GetLocalizedString(LocalizationKeys.Tabs.Scav)})",
                SaveEntry.MasteringSkillsPmc => $"{locale.GetLocalizedString(LocalizationKeys.Tabs.Mastering)} ({locale.GetLocalizedString(LocalizationKeys.Tabs.Pmc)})",
                SaveEntry.MasteringSkillsScav => $"{locale.GetLocalizedString(LocalizationKeys.Tabs.Mastering)} ({locale.GetLocalizedString(LocalizationKeys.Tabs.Scav)})",
                SaveEntry.StashPmc => $"{locale.GetLocalizedString(LocalizationKeys.Tabs.Stash)} ({locale.GetLocalizedString(LocalizationKeys.Tabs.Pmc)})",
                SaveEntry.StashScav => $"{locale.GetLocalizedString(LocalizationKeys.Tabs.Stash)} ({locale.GetLocalizedString(LocalizationKeys.Tabs.Scav)})",
                SaveEntry.Hideout => $"{locale.GetLocalizedString(LocalizationKeys.Tabs.Hideout)} ({locale.GetLocalizedString(LocalizationKeys.Tabs.HideoutZones)})",
                SaveEntry.HideoutCrafts => $"{locale.GetLocalizedString(LocalizationKeys.Tabs.Hideout)} ({locale.GetLocalizedString(LocalizationKeys.Tabs.HideoutCraftsUnlock)})",
                SaveEntry.HideoutStartedCrafts => $"{locale.GetLocalizedString(LocalizationKeys.Tabs.Hideout)} ({locale.GetLocalizedString(LocalizationKeys.Tabs.HideoutCrafts)})",
                SaveEntry.Bonuses => locale.GetLocalizedString(LocalizationKeys.Tabs.StashAdditionalLines),
                _ => entry.ToString(),
            };
        }
    }

    public static class IEnumerableExtension
    {
        public static bool HaveAllErrors(this IEnumerable<SaveException> entries)
            => Enum.GetNames<SaveEntry>().Length == entries.Count();

        public static string GetLocalizedDescription(this IEnumerable<SaveException> exceptions)
            => string.Join("\n", exceptions.Select(x => $"{x.Entry.LocalizedName()}: {x.Exception.Message}"));
    }

    public class ProfileSaver(Profile profile, AppSettings appSettings, ServerDatabase serverDatabase)
    {
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
            HideoutStartedCrafts,
            Bonuses
        }

        private static JsonSerializerSettings SeriSettings => new()
        {
            Formatting = Formatting.Indented,
            Converters = [new StringEnumConverterExt()]
        };

        public List<SaveException> Save(string targetPath, string savePath = null)
        {
            savePath ??= targetPath;
            List<SaveException> exceptions = [];
            var jobject = JObject.Parse(File.ReadAllText(targetPath));
            var pmc = jobject.SelectToken(JsonPaths.Characters)[JsonPaths.Pmc];
            var scav = jobject.SelectToken(JsonPaths.Characters)[JsonPaths.Scav];
            string newStash = string.Empty;
            var writeOperations = new (Action, SaveEntry)[]
            {
                (() => WriteCharacterInfo(pmc, profile.Characters.Pmc), SaveEntry.CharacterInfoPmc),
                (() => WriteCharacterInfo(scav, profile.Characters.Scav), SaveEntry.CharacterInfoScav),
                (() => WriteCharacterHealth(pmc, profile.Characters.Pmc.Health), SaveEntry.CharacterHealthPmc),
                (() => WriteCharacterHealth(scav, profile.Characters.Scav.Health), SaveEntry.CharacterHealthScav),
                (() => WriteEncyclopedia(pmc), SaveEntry.Encyclopedia),
                (() => WriteTraders(pmc, profile.Characters.Pmc), SaveEntry.Traders),
                (() => WriteTraders(scav, profile.Characters.Scav), SaveEntry.Traders),
                (() => WriteQuests(pmc), SaveEntry.Quests),
                (() => WriteHideout(pmc, profile.Characters.Pmc.Inventory, out newStash), SaveEntry.Hideout),
                (() => WriteHideoutCrafts(pmc), SaveEntry.HideoutCrafts),
                (() => WriteHideoutStartedCrafts(pmc), SaveEntry.HideoutStartedCrafts),
                (() => WriteSkills(profile.Characters.Pmc.Skills.Common, pmc, SkillTypes.Common), SaveEntry.CommonSkillsPmc),
                (() => WriteSkills(profile.Characters.Scav.Skills.Common, scav, SkillTypes.Common), SaveEntry.CommonSkillsScav),
                (() => WriteSkills(profile.Characters.Pmc.Skills.Mastering, pmc, SkillTypes.Mastering), SaveEntry.MasteringSkillsPmc),
                (() => WriteSkills(profile.Characters.Scav.Skills.Mastering, scav, SkillTypes.Mastering), SaveEntry.MasteringSkillsScav),
                (() => WriteSuits(jobject), SaveEntry.Suits),
                (() => WriteStashBonus(pmc), SaveEntry.Bonuses),
                // Stash writing always must be after after writing hideout due to update stashes by hideout area stages
                (() => WriteStash(pmc, profile.Characters.Pmc.Inventory, newStash), SaveEntry.StashPmc),
                (() => WriteStash(scav, profile.Characters.Scav.Inventory, null), SaveEntry.StashScav),
                (() => WriteUserBuilds(jobject), SaveEntry.UserBuilds)
            };
            foreach (var (operation, entry) in writeOperations)
            {
                try { operation(); }
                catch (Exception ex) { exceptions.Add(new(entry, ex)); }
            }
            if (!exceptions.HaveAllErrors())
                File.WriteAllText(savePath, SerializeProfile(jobject));
            return exceptions;
        }

        private static string SerializeProfile(JObject profileObject)
        {
            var jsonSerializer = JsonSerializer.CreateDefault(SeriSettings);
            jsonSerializer.Formatting = Formatting.Indented;
            var sb = new StringBuilder(256);
            var sw = new StringWriter(sb, CultureInfo.InvariantCulture);
            using (var jsonWriter = new JsonTextWriter(sw))
            {
                jsonWriter.Formatting = jsonSerializer.Formatting;
                jsonWriter.IndentChar = '\t';
                jsonWriter.Indentation = 1;
                jsonSerializer.Serialize(jsonWriter, profileObject);
            }
            return sw.ToString();
        }

        private static void WriteCharacterInfo(JToken character, Character profileCharacter)
        {
            var infoToken = character.SelectToken(JsonPaths.Info);
            infoToken[CharacterProperties.Nickname] = profileCharacter.Info.Nickname;
            infoToken[CharacterProperties.Level] = profileCharacter.Info.Level;
            infoToken[CharacterProperties.Experience] = profileCharacter.Info.Experience;
            character.SelectToken(JsonPaths.Customization)[CharacterProperties.Head] = profileCharacter.Customization.Head;
            character.SelectToken(JsonPaths.Customization)[CharacterProperties.Voice] = profileCharacter.Customization.Voice;
            if (profileCharacter.IsScav)
                return;
            infoToken[CharacterProperties.LowerNickname] = profileCharacter.Info.Nickname.ToLower();
            infoToken[CharacterProperties.Side] = profileCharacter.Info.Side;
        }

        private static void WriteCharacterHealth(JToken character, CharacterHealth characterHealth)
        {
            var healthToken = character.SelectToken(JsonPaths.Health);
            var bodyParts = new[]
            {
                (BodyParts.Head, characterHealth.BodyParts.Head),
                (BodyParts.Chest, characterHealth.BodyParts.Chest),
                (BodyParts.Stomach, characterHealth.BodyParts.Stomach),
                (BodyParts.LeftArm, characterHealth.BodyParts.LeftArm),
                (BodyParts.RightArm, characterHealth.BodyParts.RightArm),
                (BodyParts.LeftLeg, characterHealth.BodyParts.LeftLeg),
                (BodyParts.RightLeg, characterHealth.BodyParts.RightLeg)
            };
            var healthParts = new[]
            {
                (HealthProperties.Energy, characterHealth.Energy),
                (HealthProperties.Hydration, characterHealth.Hydration)
            };
            foreach (var (partName, bodyPart) in bodyParts)
            {
                healthToken[JsonPaths.BodyParts][partName][JsonPaths.Health][HealthProperties.Current] = (int)bodyPart.Health.Current;
                healthToken[JsonPaths.BodyParts][partName][JsonPaths.Health][HealthProperties.Maximum] = (int)bodyPart.Health.Maximum;
            }
            foreach (var (partName, healthPart) in healthParts)
            {
                healthToken[partName][HealthProperties.Current] = (int)healthPart.Current;
                healthToken[partName][HealthProperties.Maximum] = (int)healthPart.Maximum;
            }
        }

        private static void WriteSkills(CharacterSkill[] skills, JToken character, string skillType)
        {
            var skillsToken = character.SelectToken(JsonPaths.Skills).SelectToken(skillType);
            var existingSkills = skillsToken.ToObject<CharacterSkill[]>();
            if (existingSkills.Length == 0)
            {
                skillsToken.Replace(JToken.FromObject(skills));
                return;
            }
            for (int i = 0; i < existingSkills.Length; i++)
            {
                var skillToken = skillsToken[i];
                var existingSkill = existingSkills[i];
                var editedSkill = skills.FirstOrDefault(x => x.Id == existingSkill.Id);

                if (editedSkill != null && editedSkill.Progress != existingSkill.Progress)
                    skillToken[SkillProperties.Progress] = editedSkill.Progress;
            }
            foreach (var newSkill in skills.Where(x => !existingSkills.Any(y => y.Id == x.Id)))
                skillsToken.Last().AddAfterSelf(JObject.FromObject(newSkill));
        }

        private static void WriteStash(JToken characterToken, CharacterInventory inventory, string newStash)
        {
            var inventoryToken = characterToken.SelectToken(JsonPaths.Inventory);
            var itemsToken = inventoryToken?.SelectToken(JsonPaths.Items);
            var existingItems = itemsToken.ToObject<InventoryItem[]>().ToList();
            existingItems.RemoveAll(existingItem =>
            {
                var currentItem = inventory.Items.FirstOrDefault(x => x.Id == existingItem.Id);
                if (currentItem == null)
                    return true;
                if (!string.IsNullOrEmpty(newStash) && existingItem.Id == inventory.Stash)
                    existingItem.Tpl = newStash;
                if (existingItem.IsPockets)
                    existingItem.Tpl = inventory.Pockets;
                return false;
            });
            existingItems.AddRange(inventory.Items.Where(x => !existingItems.Any(y => y.Id == x.Id)));
            itemsToken.Replace(JToken.FromObject(existingItems));
            inventoryToken.SelectToken(JsonPaths.HideoutAreaStashes).Replace(JToken.FromObject(inventory.HideoutAreaStashes));
        }

        private void UpdateStashForHideoutArea(HideoutAreaInfo hideoutAreaInfo, string type, int level, CharacterInventory inventory)
        {
            if (hideoutAreaInfo == null)
                return;
            var areaInfoObject = JObject.Parse(hideoutAreaInfo.Stages[level.ToString()].ToString());
            var areaStageContainer = areaInfoObject.SelectToken(JsonPaths.Container).ToObject<string>();
            if (!string.IsNullOrEmpty(areaStageContainer))
            {
                var inventoryItemsList = inventory.Items.ToList();
                inventory.HideoutAreaStashes[type] = hideoutAreaInfo.Id;
                var inventoryItem = inventory.Items.FirstOrDefault(x => x.Id == hideoutAreaInfo.Id);

                if (inventoryItem != null)
                    inventoryItem.Tpl = areaStageContainer;
                else
                    inventoryItemsList.Add(new InventoryItem() { Id = hideoutAreaInfo.Id, Tpl = areaStageContainer });

                if (type == appSettings.HideoutAreaEquipmentPresetsType.ToString() && serverDatabase.ItemsDB.ContainsKey(areaStageContainer))
                    AddMissingPresetStandItems(areaStageContainer, inventoryItemsList, hideoutAreaInfo.Id, inventory.Pockets);

                inventory.Items = [.. inventoryItemsList];
            }
            else
            {
                inventory.HideoutAreaStashes.Remove(type);
                inventory.Items = [.. inventory.Items.Where(x => x.Id != hideoutAreaInfo.Id)];
            }
        }

        private void AddMissingPresetStandItems(string container, List<InventoryItem> inventoryItemsList, string areaId, string pocketsTpl)
        {
            var slots = serverDatabase.ItemsDB[container].Properties?.Slots;
            if (slots == null)
                return;

            void AddItem(string id, string tpl, string parentId, string slotId)
            {
                inventoryItemsList.Add(new InventoryItem
                {
                    Id = id,
                    Tpl = tpl,
                    ParentId = parentId,
                    SlotId = slotId
                });
            }

            foreach (var mannequinSlot in slots)
            {
                if (inventoryItemsList.Any(x => x.ParentId == areaId && x.SlotId == mannequinSlot.Name))
                    continue;

                var iDs = inventoryItemsList.Select(x => x.Id).ToList();
                var newId = ExtMethods.GenerateNewId(iDs);
                AddItem(newId, appSettings.MannequinInventoryTpl, areaId, mannequinSlot.Name);
                iDs.Add(newId);

                // Add pocket child item
                AddItem(ExtMethods.GenerateNewId(iDs), pocketsTpl, newId, appSettings.PocketsSlotId);
            }
        }

        private void WriteSuits(JObject jobject)
        {
            jobject.SelectToken(JsonPaths.CustomisationUnlocks).Replace(JToken.FromObject(profile.CustomisationUnlocks.ToArray()));
        }

        private void WriteTraders(JToken token, Character character)
        {
            var tradersToken = token.SelectToken(JsonPaths.TradersInfo);
            var tradersIdsForRemove = profile.ModdedEntitiesForRemoving
                .Where(x => x.Type == ModdedEntityType.Merchant)
                .Select(x => x.Id);
            if (tradersIdsForRemove.Any())
                tradersToken.RemoveFields(tradersIdsForRemove);
            foreach (var trader in character.TraderStandings.Where(x => !tradersIdsForRemove.Contains(x.Key)))
            {
                var traderToken = tradersToken.SelectToken($"['{trader.Key}']");
                var traderInfo = trader.Value;
                traderToken[TraderProperties.LoyaltyLevel] = traderInfo.LoyaltyLevel;
                traderToken[TraderProperties.SalesSum] = traderInfo.SalesSum;
                traderToken[TraderProperties.Standing] = Math.Round(traderInfo.Standing, 2);
                traderToken[TraderProperties.Unlocked] = traderInfo.Unlocked;
            }
            token.SelectToken(JsonPaths.RagfairInfo)[TraderProperties.Rating] = Math.Round(character.RagfairInfo.Rating, 2);
        }

        private void WriteEncyclopedia(JToken pmc)
        {
            pmc.SelectToken(JsonPaths.Encyclopedia).Replace(JToken.FromObject(profile.Characters.Pmc.Encyclopedia));
        }

        private void WriteQuests(JToken pmc)
        {
            JToken questsToken = pmc.SelectToken(JsonPaths.Quests);
            List<JToken> questsForRemove = [];
            var questsObject = questsToken.ToObject<CharacterQuest[]>();
            if (questsObject.Length != 0)
            {
                for (int index = 0; index < questsObject.Length; ++index)
                {
                    JToken questToken = questsToken[index];
                    var quest = questToken.ToObject<CharacterQuest>();

                    var edited = profile.Characters.Pmc.Quests.FirstOrDefault(x => x.Qid == quest.Qid);
                    if (edited != null)
                    {
                        if (quest != null && quest.Status != edited.Status)
                        {
                            questToken[QuestProperties.Status] = edited.Status.ToString();
                            questToken[QuestProperties.StartTime] = edited.StartTime;
                            questToken[QuestProperties.StatusTimers] = JObject.FromObject(edited.StatusTimers);
                            if (edited.Status <= QuestStatus.AvailableForStart && questToken[QuestProperties.CompletedConditions] != null)
                                questToken[QuestProperties.CompletedConditions]?.Replace(JToken.FromObject(Array.Empty<string>()));
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
                        questsToken = pmc.SelectToken(JsonPaths.Quests);
                    }
                }
            }
            else
                questsToken.Replace(JToken.FromObject(profile.Characters.Pmc.Quests));
        }

        private void WriteStashBonus(JToken pmc)
        {
            var bonusesToken = pmc.SelectToken(JsonPaths.Bonuses);
            var bonusesObject = bonusesToken.ToObject<CharacterBonus[]>();
            var isStashRowsBonusUpdate = profile.Characters.Pmc.StashRowsBonusCount > 0;

            if (bonusesObject.Length > 0)
            {
                var bonusEdited = false;
                JToken forRemove = null;
                for (int index = 0; index < bonusesObject.Length; index++)
                {
                    var bonusToken = bonusesToken[index];
                    var probe = bonusToken?.ToObject<CharacterBonus>();
                    if (probe != null && probe.Type == BonusTypes.StashRows)
                    {
                        if (isStashRowsBonusUpdate)
                        {
                            bonusToken[BonusProperties.Value] = profile.Characters.Pmc.StashRowsBonusCount;
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
                    pmc.SelectToken(JsonPaths.Bonuses).LastOrDefault().AddAfterSelf(JObject.FromObject(bonusToken).RemoveNullAndEmptyProperties());
                }
            }
            else if (isStashRowsBonusUpdate)
                bonusesToken.Replace(JObject.FromObject(profile.Characters.Pmc.Bonuses).RemoveNullAndEmptyProperties());
        }

        private void WriteHideout(JToken pmc, CharacterInventory inventory, out string newStash)
        {
            newStash = string.Empty;
            var areasToken = pmc.SelectToken(JsonPaths.Hideout).SelectToken(JsonPaths.Areas);
            var hideoutAreasObject = areasToken.ToObject<HideoutArea[]>();
            for (int i = 0; i < hideoutAreasObject.Length; i++)
            {
                var areaToken = areasToken[i];
                var probe = areaToken.ToObject<HideoutArea>();
                var areaInfo = profile.Characters.Pmc.Hideout.Areas.FirstOrDefault(x => x.Type == probe.Type);
                if (areaInfo == null)
                    continue;
                var areaDBInfo = serverDatabase.HideoutAreaInfos.FirstOrDefault(x => x.Type == probe.Type);
                if (areaInfo.Level > 0 && areaInfo.Level > probe.Level)
                {
                    for (int l = probe.Level; l <= areaInfo.Level; l++)
                    {
                        var areaBonuses = areaDBInfo.Stages[l.ToString()];
                        if (areaBonuses == null)
                            continue;
                        var areaInfoObject = JObject.Parse(areaBonuses.ToString());
                        var bonusesList = areaInfoObject.SelectToken(AreInfoProperties.Bonuses).ToObject<List<JToken>>();
                        if (bonusesList == null || bonusesList.Count == 0)
                            continue;
                        foreach (var listItem in bonusesList)
                        {
                            var bonus = listItem.ToObject<CharacterBonus>();
                            if (bonus == null)
                                continue;
                            switch (bonus.Type)
                            {
                                case BonusTypes.StashSize:
                                    newStash = bonus.TemplateId;
                                    break;

                                case BonusTypes.MaximumEnergyReserve:
                                    pmc.SelectToken(JsonPaths.Health).SelectToken(HealthProperties.Energy)[HealthProperties.Maximum] =
                                        Math.Max(110, profile.Characters.Pmc.Health.Energy.Maximum);
                                    break;
                            }
                            pmc.SelectToken(JsonPaths.Bonuses).LastOrDefault().AddAfterSelf(JObject.FromObject(listItem));
                        }
                    }
                }
                areaToken[HideoutProperties.Level] = areaInfo.Level;
                UpdateStashForHideoutArea(areaDBInfo, areaInfo.Type.ToString(), areaInfo.Level, inventory);
            }
        }

        private void WriteHideoutCrafts(JToken pmc)
        {
            var crafts = profile.Characters.Pmc.HideoutProductions.Where(x => x.Added).Select(x => x.Production.Id).ToArray();
            pmc.SelectToken(JsonPaths.UnlockedInfo)[HideoutProperties.UnlockedProductionRecipe].Replace(JToken.FromObject(crafts));
        }

        private void WriteHideoutStartedCrafts(JToken pmc)
        {
            var forSave = profile.Characters.Pmc.Hideout?.Production;
            if (forSave != null && forSave.Count != 0)
            {
                var productionToken = pmc.SelectToken(JsonPaths.Hideout).SelectToken(JsonPaths.Production);
                if (productionToken == null)
                    return;
                foreach (var startedCraft in forSave.Values)
                {
                    var production = productionToken[startedCraft.RecipeId];
                    if (production == null)
                        continue;
                    production[HideoutProperties.Progress] = startedCraft.Progress;
                    production[HideoutProperties.StartTimestamp] = startedCraft.StartTimestamp;
                }
                var existCrafts = productionToken.ToObject<Dictionary<string, object>>()?.Keys;
                var forRemove = existCrafts.Except(forSave.Keys);
                productionToken.RemoveFields(forRemove);
            }
            else
            {
                var emptyDict = new Dictionary<string, object>();
                pmc.SelectToken(JsonPaths.Hideout).SelectToken(JsonPaths.Production).Replace(JToken.FromObject(emptyDict));
            }
        }

        private void WriteUserBuilds(JObject jobject)
        {
            var magazineBuilds = jobject.SelectToken(JsonPaths.Userbuilds)[JsonPaths.MagazineBuilds];
            profile.UserBuilds.RemoveParentsFromBuilds();
            jobject.SelectToken(JsonPaths.Userbuilds).Replace(JObject.FromObject(profile.UserBuilds).RemoveNullAndEmptyProperties());
            if ((profile.UserBuilds.WeaponBuilds?.Count > 0) != true)
                jobject.SelectToken(JsonPaths.Userbuilds)[JsonPaths.WeaponBuilds] = JToken.FromObject(Array.Empty<WeaponBuild>());
            if ((profile.UserBuilds.EquipmentBuilds?.Count > 0) != true)
                jobject.SelectToken(JsonPaths.Userbuilds)[JsonPaths.EquipmentBuilds] = JToken.FromObject(Array.Empty<EquipmentBuild>());
            jobject.SelectToken(JsonPaths.Userbuilds)[JsonPaths.MagazineBuilds] = magazineBuilds;
        }

        internal static class LocalizationKeys
        {
            public static class Tabs
            {
                public const string Presets = "tab_presets_title";
                public const string Clothing = "tab_clothing_title";
                public const string Merchants = "tab_merchants_title";
                public const string ExaminedItems = "tab_examined_items_title";
                public const string Info = "tab_info_title";
                public const string Pmc = "tab_info_pmc";
                public const string Scav = "tab_info_scav";
                public const string Health = "tab_info_health";
                public const string Quests = "tab_quests_title";
                public const string Skills = "tab_skills_title";
                public const string Mastering = "tab_mastering_title";
                public const string Stash = "tab_stash_title";
                public const string Hideout = "tab_hideout_title";
                public const string HideoutZones = "tab_hideout_zones";
                public const string HideoutCraftsUnlock = "tab_hideout_crafts_unlock";
                public const string HideoutCrafts = "tab_hideout_crafts";
                public const string StashAdditionalLines = "tab_stash_additional_lines";
            }
        }

        private static class JsonPaths
        {
            public const string Characters = "characters";
            public const string Pmc = "pmc";
            public const string Scav = "scav";
            public const string CustomisationUnlocks = "customisationUnlocks";
            public const string TradersInfo = "TradersInfo";
            public const string RagfairInfo = "RagfairInfo";
            public const string Encyclopedia = "Encyclopedia";
            public const string Info = "Info";
            public const string Customization = "Customization";
            public const string Health = "Health";
            public const string BodyParts = "BodyParts";
            public const string Quests = "Quests";
            public const string Skills = "Skills";
            public const string Inventory = "Inventory";
            public const string Items = "items";
            public const string HideoutAreaStashes = "hideoutAreaStashes";
            public const string Bonuses = "Bonuses";
            public const string Hideout = "Hideout";
            public const string Areas = "Areas";
            public const string Production = "Production";
            public const string UnlockedInfo = "UnlockedInfo";
            public const string Userbuilds = "userbuilds";
            public const string MagazineBuilds = "magazineBuilds";
            public const string WeaponBuilds = "weaponBuilds";
            public const string EquipmentBuilds = "equipmentBuilds";
            public const string Container = "container";
        }

        private static class CharacterProperties
        {
            public const string Nickname = "Nickname";
            public const string LowerNickname = "LowerNickname";
            public const string Level = "Level";
            public const string Experience = "Experience";
            public const string Side = "Side";
            public const string Head = "Head";
            public const string Voice = "Voice";
        }

        private static class HealthProperties
        {
            public const string Energy = "Energy";
            public const string Hydration = "Hydration";
            public const string Current = "Current";
            public const string Maximum = "Maximum";
        }

        private static class BodyParts
        {
            public const string Head = "Head";
            public const string Chest = "Chest";
            public const string Stomach = "Stomach";
            public const string LeftArm = "LeftArm";
            public const string RightArm = "RightArm";
            public const string LeftLeg = "LeftLeg";
            public const string RightLeg = "RightLeg";
        }

        private static class TraderProperties
        {
            public const string LoyaltyLevel = "loyaltyLevel";
            public const string SalesSum = "salesSum";
            public const string Standing = "standing";
            public const string Unlocked = "unlocked";
            public const string Rating = "rating";
        }

        private static class SkillProperties
        {
            public const string Progress = "Progress";
        }

        private static class SkillTypes
        {
            public const string Common = "Common";
            public const string Mastering = "Mastering";
        }

        private static class BonusTypes
        {
            public const string StashRows = "StashRows";
            public const string StashSize = "StashSize";
            public const string MaximumEnergyReserve = "MaximumEnergyReserve";
        }

        private static class BonusProperties
        {
            public const string Value = "value";
        }

        private static class AreInfoProperties
        {
            public const string Bonuses = "bonuses";
        }

        private static class HideoutProperties
        {
            public const string Level = "level";
            public const string UnlockedProductionRecipe = "unlockedProductionRecipe";
            public const string Progress = "Progress";
            public const string StartTimestamp = "StartTimestamp";
        }

        private static class QuestProperties
        {
            public const string Status = "status";
            public const string StartTime = "startTime";
            public const string StatusTimers = "statusTimers";
            public const string CompletedConditions = "completedConditions";
        }
    }

    public class SaveException(SaveEntry entry, Exception exception)
    {
        public SaveEntry Entry { get; } = entry;
        public Exception Exception { get; } = exception;
    }
}