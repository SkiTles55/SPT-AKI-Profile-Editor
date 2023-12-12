using Newtonsoft.Json;
using NUnit.Framework;
using SPT_AKI_Profile_Editor.Core;
using SPT_AKI_Profile_Editor.Core.ProfileClasses;
using SPT_AKI_Profile_Editor.Core.ProgressTransfer;
using SPT_AKI_Profile_Editor.Tests.Hepers;
using System;
using System.Collections.Generic;
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
                trader.LoyaltyLevel = Math.Max(3, trader.MaxLevel);
                merchantList.Add(new(trader.Id,
                                     trader.TraderStanding.Unlocked,
                                     trader.LoyaltyLevel,
                                     trader.Standing,
                                     trader.SalesSum));
            }
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