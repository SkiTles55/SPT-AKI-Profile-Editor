using Newtonsoft.Json;
using SPT_AKI_Profile_Editor.Core.Enums;
using SPT_AKI_Profile_Editor.Core.HelperClasses;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace SPT_AKI_Profile_Editor.Core
{
    public static class DefaultValues
    {
        public const string ColorScheme = "Light.Emerald";
        public const string PocketsContainerTpl = "557596e64bdc2dc2118b4571";
        public const string PocketsSlotId = "Pockets";
        public const string FirstPrimaryWeaponSlotId = "FirstPrimaryWeapon";
        public const string HeadwearSlotId = "Headwear";
        public const string TacticalVestSlotId = "TacticalVest";
        public const string BackpackSlotId = "Backpack";
        public const string EarpieceSlotId = "Earpiece";
        public const string SecuredContainerSlotId = "SecuredContainer";
        public const string FaceCoverSlotId = "FaceCover";
        public const string EyewearSlotId = "Eyewear";
        public const string ArmorVestSlotId = "ArmorVest";
        public const string SecondPrimaryWeaponSlotId = "SecondPrimaryWeapon";
        public const string HolsterSlotId = "Holster";
        public const string ScabbardSlotId = "Scabbard";
        public const string ArmBandSlotId = "ArmBand";
        public const bool CheckUpdates = true;
        public const string MoneysDollarsTpl = "5696686a4bdc2da3298b456a";
        public const string MoneysRublesTpl = "5449016a4bdc2d6f028b456f";
        public const string MoneysEurosTpl = "569668774bdc2da2298b4568";
        public const string RagfairTraderId = "ragfair";
        public const string EndlessDevBackpackId = "56e294cdd2720b603a8b4575";
        public const string BearDogtagTpl = "59f32bb586f774757e1e8442";
        public const float CommonSkillMaxValue = 5100;
        public const IssuesAction DefaultIssuesAction = IssuesAction.AlwaysShow;
        public const string FenceTraderId = "579dc571d53a0658a154fbec";

        public static readonly string AppDataFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "SPT-AKI Profile Editor");

        public static List<string> BannedItems => new()
        {
            "543be5dd4bdc2deb348b4569",
            "55d720f24bdc2d88028b456d",
            "557596e64bdc2dc2118b4571",
            "566965d44bdc2d814c8b4571",
            "566abbb64bdc2d144c8b457d",
            "5448f39d4bdc2d0a728b4568",
            "5943d9c186f7745a13413ac9",
            "5996f6cb86f774678763a6ca",
            "5996f6d686f77467977ba6cc",
            "5996f6fc86f7745e585b4de3",
            "5cdeb229d7f00c000e7ce174",
            "5d52cc5ba4b9367408500062"
        };

        public static List<string> BannedMasterings => new()
        {
            "MR43"
        };

        public static Dictionary<string, string> DefaultDirsList => new()
        {
            [SPTServerDir.globals] = "Aki_Data\\Server\\database\\locales\\global",
            [SPTServerDir.traders] = "Aki_Data\\Server\\database\\traders",
            [SPTServerDir.bots] = "Aki_Data\\Server\\database\\bots\\types",
            [SPTServerDir.profiles] = "user\\profiles",
            [SPTServerDir.handbookIcons] = "Aki_Data\\Server\\images\\handbook",
            [SPTServerDir.traderImages] = "Aki_Data\\Server\\images\\traders"
        };

        public static Dictionary<string, string> DefaultFilesList => new()
        {
            [SPTServerFile.globals] = "Aki_Data\\Server\\database\\globals.json",
            [SPTServerFile.items] = "Aki_Data\\Server\\database\\templates\\items.json",
            [SPTServerFile.quests] = "Aki_Data\\Server\\database\\templates\\quests.json",
            [SPTServerFile.questConfig] = "Aki_Data\\Server\\configs\\quest.json",
            [SPTServerFile.areas] = "Aki_Data\\Server\\database\\hideout\\areas.json",
            [SPTServerFile.production] = "Aki_Data\\Server\\database\\hideout\\production.json",
            [SPTServerFile.handbook] = "Aki_Data\\Server\\database\\templates\\handbook.json",
            [SPTServerFile.languages] = "Aki_Data\\Server\\database\\locales\\languages.json",
            [SPTServerFile.serverexe] = "Aki.Server.exe"
        };

        public static List<AppLocalization> DefaultLocalizations()
        {
            List<AppLocalization> loaded = new();
            foreach (var file in new string[] { "en.json", "ru.json", "ch.json" })
            {
                try
                {
                    Assembly assembly = Assembly.GetExecutingAssembly();
                    Stream embeddedFile = assembly.GetManifestResourceStream("SPT_AKI_Profile_Editor.Resources.Localizations." + file);
                    StreamReader reader = new(embeddedFile);
                    AppLocalization appLocalization = JsonConvert.DeserializeObject<AppLocalization>(reader.ReadToEnd());
                    loaded?.Add(appLocalization);
                }
                catch (Exception ex) { Logger.Log($"Loading embedded localization file ({file}) error: {ex.Message}"); }
            }
            return loaded;
        }
    }
}