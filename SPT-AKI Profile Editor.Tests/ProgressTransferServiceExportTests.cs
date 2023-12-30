using Newtonsoft.Json;
using NUnit.Framework;
using SPT_AKI_Profile_Editor.Core;
using SPT_AKI_Profile_Editor.Core.Enums;
using SPT_AKI_Profile_Editor.Core.ProfileClasses;
using SPT_AKI_Profile_Editor.Core.ProgressTransfer;
using SPT_AKI_Profile_Editor.Helpers;
using SPT_AKI_Profile_Editor.Tests.Hepers;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using static SPT_AKI_Profile_Editor.Core.ProgressTransfer.ProfileProgress;
using static SPT_AKI_Profile_Editor.Core.ProgressTransfer.ProfileProgress.InfoProgress.Character;
using static SPT_AKI_Profile_Editor.Core.ProgressTransfer.ProfileProgress.InfoProgress.Character.Health;

namespace SPT_AKI_Profile_Editor.Tests
{
    internal class ProgressTransferServiceExportTests
    {
        private readonly string pmcNickname = "ProgressTransferServiceTestPMCNickname";
        private readonly string scavNickname = "ProgressTransferServiceTestScavNickname";
        private readonly long pmcExperience = 19198744;
        private readonly long scavExperience = 1918744;
        private readonly string exportPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "testProgressExport.json");
        private readonly SettingsModel exportSettings = new();
        private string pmcSide;
        private string pmcVoice;
        private string pmcHead;
        private string pmcPockets;
        private Health pmcHealth;

        private string scavVoice;
        private string scavHead;
        private string scavPockets;
        private Health scavHealth;

        private List<Merchant> merchantList;
        private Dictionary<string, QuestStatus> questsList;
        private Dictionary<int, int> hideoutlist;
        private string[] productionsList;
        private Dictionary<string, bool> encyclopediaList;
        private string[] suitsList;

        [OneTimeSetUp]
        public void Setup()
        {
            AppData.AppSettings.AutoAddMissingQuests = false;
            TestHelpers.LoadDatabaseAndProfile();
            var pmc = AppData.Profile.Characters.Pmc;
            pmcSide = pmc.Info.Side == "Bear" ? "Usec" : "Bear";
            pmcVoice = GetVoice(pmc);
            pmcHead = GetHead(pmc);
            pmcPockets = GetPockets(pmc);
            pmcHealth = GetHealth(pmc);

            var scav = AppData.Profile.Characters.Scav;
            scavVoice = GetVoice(scav);
            scavHead = GetHead(scav);
            scavPockets = GetPockets(scav);
            scavHealth = GetHealth(scav);

            ChangeCharacter(pmc, pmcNickname, pmcVoice, pmcSide, pmcExperience, pmcHead, pmcPockets);
            ChangeCharacter(scav, scavNickname, scavVoice, "Bear", scavExperience, scavHead, scavPockets);

            merchantList = new();
            foreach (var trader in pmc.TraderStandingsExt)
            {
                trader.LoyaltyLevel = Math.Min(2, trader.MaxLevel);
                merchantList.Add(new(trader.Id,
                                     trader.TraderStanding.Unlocked,
                                     trader.LoyaltyLevel,
                                     trader.Standing,
                                     trader.SalesSum));
            }

            pmc.RemoveAllQuests();
            pmc.Quests = AppData.ServerDatabase.QuestsData.Take(10).Select(x => new CharacterQuest { Qid = x.Key, Status = QuestStatus.Locked }).ToArray();
            pmc.SetAllQuests(QuestStatus.Success);
            questsList = pmc.Quests.ToDictionary(x => x.Qid, x => x.Status);

            foreach (var area in pmc.Hideout.Areas)
                area.Level = Math.Min(2, area.MaxLevel);
            hideoutlist = pmc.Hideout.Areas.ToDictionary(x => x.Type, x => x.Level);

            foreach (var production in pmc.HideoutProductions.Take(4))
                production.Added = true;
            productionsList = pmc.HideoutProductions.Where(x => x.Added).Select(x => x.Production.Id).ToArray();

            foreach (var item in AppData.ServerDatabase.ItemsDB
                .Where(x => x.Value.Parent != null
                && x.Value.Type == "Item"
                && !x.Value.Properties.ExaminedByDefault
                && !pmc.Encyclopedia.Any(c => c.Key == x.Key)
                && AppData.ServerDatabase.LocalesGlobal.ContainsKey(x.Key.Name()))
                .Take(10))
                pmc.Encyclopedia.Add(item.Key, true);
            encyclopediaList = pmc.Encyclopedia;

            foreach (var suit in AppData.ServerDatabase.TraderSuits.Where(x => !x.Boughted).Take(4))
                suit.Boughted = true;
            suitsList = AppData.ServerDatabase.TraderSuits.Where(x => x.Boughted).Select(x => x.SuiteId).ToArray();
        }

        [Test]
        public void CanExportAllSettings()
        {
            exportSettings.ChangeAll(true);
            ProgressTransferService.ExportProgress(exportSettings, AppData.Profile, exportPath);

            string fileText = File.ReadAllText(exportPath);
            ProfileProgress exportedProgress = JsonConvert.DeserializeObject<ProfileProgress>(fileText);

            Assert.That(exportedProgress, Is.Not.Null);
            CheckExportedInfoProgress(exportedProgress.Info.Pmc,
                                      pmcNickname,
                                      pmcVoice,
                                      pmcSide,
                                      pmcExperience,
                                      pmcHead,
                                      pmcPockets,
                                      pmcHealth);
            CheckExportedInfoProgress(exportedProgress.Info.Scav,
                                      scavNickname,
                                      scavVoice,
                                      null,
                                      scavExperience,
                                      scavHead,
                                      scavPockets,
                                      scavHealth);

            Assert.That(exportedProgress.Merchants.All(x => Compare(x, merchantList.First(y => y.Id == x.Id))),
                        Is.True);

            Assert.That(exportedProgress.Quests, Is.EqualTo(questsList));

            Assert.That(exportedProgress.Hideout, Is.EqualTo(hideoutlist));

            Assert.That(exportedProgress.Crafts.UnlockedProductionRecipe,
                        Is.EqualTo(productionsList));

            Assert.That(exportedProgress.ExaminedItems, Is.EqualTo(encyclopediaList));

            Assert.That(exportedProgress.Clothing.OrderBy(x => x), Is.EqualTo(suitsList.OrderBy(x => x)));
        }

        private static bool Compare(Merchant merchant, Merchant other)
            => merchant.Enabled == other.Enabled
            && merchant.SalesSum == other.SalesSum
            && merchant.Level == other.Level
            && merchant.Standing == other.Standing;

        private static string GetVoice(Character character)
                    => AppData.ServerDatabase.Voices.FirstOrDefault(x => x.Key != character.Info.Voice).Key;

        private static string GetHead(Character character)
            => AppData.ServerDatabase.Heads.FirstOrDefault(x => x.Key != character.Customization.Head).Key;

        private static string GetPockets(Character character)
            => AppData.ServerDatabase.Pockets.FirstOrDefault(x => x.Key != character.Inventory.Pockets).Key;

        private static Health GetHealth(Character character)
            => new(character.Health);

        private static void ChangeCharacter(Character pmc,
                                            string nickname,
                                            string voice,
                                            string side,
                                            long exp,
                                            string head,
                                            string pockets)
        {
            pmc.Info.Nickname = nickname;
            pmc.Info.Voice = voice;
            pmc.Info.Side = side;
            pmc.Info.Experience = exp;
            pmc.Customization.Head = head;
            pmc.Inventory.Pockets = pockets;
            // health change skipped for now
        }

        private static void CheckExportedInfoProgress(ProfileProgress.InfoProgress.Character character,
                                            string expectedNickname,
                                            string expectedVoice,
                                            string expectedSide,
                                            long expectedExp,
                                            string expectedHead,
                                            string expectedPockets,
                                            Health expectedHealth)
        {
            Assert.That(character.Nickname, Is.EqualTo(expectedNickname));
            Assert.That(character.Voice, Is.EqualTo(expectedVoice));
            Assert.That(character.Side, Is.EqualTo(expectedSide));
            Assert.That(character.Experience, Is.EqualTo(expectedExp));
            Assert.That(character.Head, Is.EqualTo(expectedHead));
            Assert.That(character.Pockets, Is.EqualTo(expectedPockets));
            Assert.That(Compare(character.HealthMetrics, expectedHealth), Is.True);
        }

        private static bool Compare(Health health1, Health health2)
            => Compare(health1.Head, health2.Head)
                && Compare(health1.Chest, health2.Chest)
                && Compare(health1.LeftArm, health2.LeftArm)
                && Compare(health1.RightArm, health2.RightArm)
                && Compare(health1.LeftLeg, health2.LeftLeg)
                && Compare(health1.RightLeg, health2.RightLeg)
                && Compare(health1.Hydration, health2.Hydration)
                && Compare(health1.Energy, health2.Energy);

        private static bool Compare(Metric metric1, Metric metric2)
            => metric1.Current == metric2.Current && metric1.Maximum == metric2.Maximum;
    }
}