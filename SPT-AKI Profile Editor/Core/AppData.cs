using Newtonsoft.Json;
using SPT_AKI_Profile_Editor.Core.HelperClasses;
using SPT_AKI_Profile_Editor.Core.ProfileClasses;
using SPT_AKI_Profile_Editor.Core.ServerClasses;
using SPT_AKI_Profile_Editor.Core.ServerClasses.Configs;
using SPT_AKI_Profile_Editor.Core.ServerClasses.Hideout;
using SPT_AKI_Profile_Editor.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;

namespace SPT_AKI_Profile_Editor.Core
{
    public static class AppData
    {
        public static readonly AppSettings AppSettings;
        public static readonly AppLocalization AppLocalization;
        public static readonly Profile Profile;
        public static readonly ServerConfigs ServerConfigs;
        public static readonly ServerDatabase ServerDatabase;
        public static readonly GridFilters GridFilters;
        public static readonly BackupService BackupService;
        public static readonly IssuesService IssuesService;
        public static readonly IHelperModManager HelperModManager;

        private static readonly bool IsRunningFromNUnit = AppDomain.CurrentDomain.GetAssemblies().Any(a => a.FullName.ToLowerInvariant().StartsWith("nunit.framework"));
        private static readonly string AppDataPath = IsRunningFromNUnit ? Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TestAppData") : DefaultValues.AppDataFolder;

        static AppData()
        {
            GridFilters = new();
            AppSettings = new(Path.Combine(AppDataPath, "AppSettings.json"));
            AppSettings.Load();
            BackupService = new(Path.Combine(AppDataPath, "Backups"));
            AppLocalization = new(AppSettings.Language, Path.Combine(AppDataPath, "Localizations"));
            IssuesService = new();
            Profile = new();
            ServerConfigs = new();
            ServerDatabase = new();
            HelperModManager = new HelperModManager(AppSettings.modHelperUpdateUrl, Path.Combine(AppDataPath, "ModHelperUpdate"));
        }

        public static void LoadDatabase()
        {
            if (AppSettings.PathIsServerFolder())
            {
                LoadLocalesGlobal();
                LoadBotTypes();
                LoadItemsDB();
                LoadServerGlobals();
                LoadTradersInfos();
                LoadQuestConfig();
                LoadQuestsData();
                LoadHideoutAreaInfos();
                LoadHideoutProduction();
                FindPockets();
                LoadTraderSuits();
                LoadHandbook();
                LoadHandbookHelper();
            }
        }

        public static void StartupEvents(ICleaningService cleaningService)
        {
            LoadDatabase();
            Profile.Load(Path.Combine(AppSettings.ServerPath, AppSettings.DirsList[SPTServerDir.profiles], AppSettings.DefaultProfile));
            BackupService.LoadBackupsList();
            GridFilters.Clear();
            cleaningService?.MarkAll(false);
            cleaningService?.LoadEntitiesList();
        }

        public static Dictionary<string, string> GetAvailableKeys()
        {
            Dictionary<string, string> availableKeys = new();
            try
            {
                string path = Path.Combine(AppSettings.ServerPath, AppSettings.FilesList[SPTServerFile.languages]);
                Dictionary<string, string> languages = JsonConvert.DeserializeObject<Dictionary<string, string>>(File.ReadAllText(path));
                availableKeys = languages
                    .Where(x => x.Key.Length == 2 && ShouldAddToAvailableKeys(x.Key))
                    .ToDictionary(x => x.Key, x => x.Value);
            }
            catch (Exception ex) { Logger.Log($"LoadAvailableKeys loading error: {ex.Message}"); }
            return availableKeys;
        }

        public static bool ShouldAddToAvailableKeys(string key) =>
            !AppLocalization.Localizations.ContainsKey(key)
            && File.Exists(Path.Combine(AppSettings.ServerPath, AppSettings.DirsList[SPTServerDir.globals], key + ".json"));

        private static void LoadLocalesGlobal()
        {
            ServerDatabase.LocalesGlobal = new();
            string path = AppSettings.UsingModHelper
                ? GetHelperDBFilePath($"Locales\\{AppSettings.Language}.json")
                : Path.Combine(AppSettings.ServerPath,
                               AppSettings.DirsList[SPTServerDir.globals],
                               AppSettings.Language + ".json");
            try
            {
                Dictionary<string, string> global = JsonConvert.DeserializeObject<Dictionary<string, string>>(File.ReadAllText(path));
                ServerDatabase.LocalesGlobal = global;
            }
            catch (Exception ex) { Logger.Log($"ServerDatabase LocalesGlobal ({path}) loading error: {ex.Message}"); }
        }

        private static void LoadBotTypes()
        {
            static string GetHeadName(string headKey, string botName)
            {
                if (ServerDatabase.LocalesGlobal.ContainsKey(headKey.Name()))
                {
                    var localizedName = ServerDatabase.LocalesGlobal[headKey.Name()];
                    if (!string.IsNullOrWhiteSpace(localizedName))
                        return localizedName;
                }

                return $"{Path.GetFileNameWithoutExtension(botName)} [{headKey}]";
            }

            Dictionary<string, string> Heads = new();
            Dictionary<string, string> Voices = new();
            foreach (var btype in Directory.GetFiles(Path.Combine(AppSettings.ServerPath, AppSettings.DirsList[SPTServerDir.bots])))
            {
                try
                {
                    if (!File.Exists(btype))
                        continue;
                    BotType bot = JsonConvert.DeserializeObject<BotType>(File.ReadAllText(btype));
                    if (bot.Appearance.Heads != null)
                        foreach (var head in bot.Appearance.Heads.Keys)
                            if (!Heads.ContainsKey(head))
                                Heads.Add(head, GetHeadName(head, btype));
                    if (bot.Appearance.Voices != null)
                        foreach (var voice in bot.Appearance.Voices.Keys)
                            if (!Voices.ContainsKey(voice))
                                Voices.Add(voice, voice);
                }
                catch (Exception ex) { Logger.Log($"ServerDatabase BotType ({btype}) loading error: {ex.Message}"); }
            }
            ServerDatabase.Heads = Heads;
            ServerDatabase.Voices = Voices;
        }

        private static void LoadServerGlobals()
        {
            ServerDatabase.ServerGlobals = AppSettings.UsingModHelper
                ? GetHelperModDBServerGlobals()
                : GetServerDBServerGlobals();
        }

        private static ServerGlobals GetServerDBServerGlobals()
        {
            string path = Path.Combine(AppSettings.ServerPath, AppSettings.FilesList[SPTServerFile.globals]);
            try
            {
                return JsonConvert.DeserializeObject<ServerGlobals>(File.ReadAllText(path));
            }
            catch (Exception ex)
            {
                Logger.Log($"ServerDatabase ServerGlobals ({path}) loading error: {ex.Message}");
                return new();
            }
        }

        private static ServerGlobals GetHelperModDBServerGlobals()
        {
            string itemPresetsPath = GetHelperDBFilePath("ItemPresets.json");
            string expTablePath = GetHelperDBFilePath("ExpTable.json");
            string masteringPath = GetHelperDBFilePath("Mastering.json");
            try
            {
                Dictionary<string, ItemPreset> itemPresets = JsonConvert.DeserializeObject<Dictionary<string, ItemPreset>>(File.ReadAllText(itemPresetsPath));
                Mastering[] mastering = JsonConvert.DeserializeObject<Mastering[]>(File.ReadAllText(masteringPath));
                ConfigExp configExp = JsonConvert.DeserializeObject<ConfigExp>(File.ReadAllText(expTablePath));

                ServerGlobalsConfig globalsConfig = new() { Mastering = mastering, Exp = configExp };
                return new(globalsConfig, itemPresets);
            }
            catch (Exception ex)
            {
                Logger.Log($"ServerDatabase ServerGlobals build from HelperMod DB error: {ex.Message}");
                return new();
            }
        }

        private static void LoadTradersInfos()
        {
            ServerDatabase.TraderInfos = new();
            var traderInfos = new Dictionary<string, TraderBase>();
            if (AppSettings.UsingModHelper)
            {
                foreach (var baseFile in Directory.GetFiles(GetHelperDBFilePath("Traders")))
                    AddTraderInfo(traderInfos, Path.GetFileNameWithoutExtension(baseFile), baseFile);
            }
            else
            {
                foreach (var tbase in Directory.GetDirectories(Path.Combine(AppSettings.ServerPath, AppSettings.DirsList[SPTServerDir.traders])))
                {
                    if (!File.Exists(Path.Combine(tbase, "base.json")))
                        continue;
                    AddTraderInfo(traderInfos, Path.GetFileNameWithoutExtension(tbase), Path.Combine(tbase, "base.json"));
                }
            }
            ServerDatabase.TraderInfos = traderInfos;
        }

        private static void AddTraderInfo(Dictionary<string, TraderBase> traderInfos, string traderId, string filepath)
        {
            try { traderInfos.Add(traderId, JsonConvert.DeserializeObject<TraderBase>(File.ReadAllText(filepath))); }
            catch (Exception ex) { Logger.Log($"ServerDatabase TraderInfo ({traderId}) loading error: {ex.Message}"); }
        }

        private static void LoadQuestConfig()
        {
            ServerConfigs.Quest = new();
            string path = AppSettings.UsingModHelper
                ? GetHelperDBFilePath("QuestConfig.json")
                : Path.Combine(AppSettings.ServerPath, AppSettings.FilesList[SPTServerFile.questConfig]);
            try
            {
                Quest questConfig = JsonConvert.DeserializeObject<Quest>(File.ReadAllText(path));
                ServerConfigs.Quest = questConfig;
            }
            catch (Exception ex) { Logger.Log($"ServerConfigs Quest ({path}) loading error: {ex.Message}"); }
        }

        private static void LoadQuestsData()
        {
            ServerDatabase.QuestsData = new();
            string path = AppSettings.UsingModHelper
                ? GetHelperDBFilePath("Quests.json")
                : Path.Combine(AppSettings.ServerPath, AppSettings.FilesList[SPTServerFile.quests]);
            try
            {
                Dictionary<string, QuestData> questsData = JsonConvert.DeserializeObject<Dictionary<string, QuestData>>(File.ReadAllText(path));
                ServerDatabase.QuestsData = questsData;
            }
            catch (Exception ex) { Logger.Log($"ServerDatabase QuestsData ({path}) loading error: {ex.Message}"); }
        }

        private static void LoadHideoutAreaInfos()
        {
            ServerDatabase.HideoutAreaInfos = new();
            string path = Path.Combine(AppSettings.ServerPath, AppSettings.FilesList[SPTServerFile.areas]);
            try
            {
                List<HideoutAreaInfo> HideoutAreaInfos = JsonConvert.DeserializeObject<List<HideoutAreaInfo>>(File.ReadAllText(path));
                ServerDatabase.HideoutAreaInfos = HideoutAreaInfos;
            }
            catch (Exception ex) { Logger.Log($"ServerDatabase HideoutAreaInfos ({path}) loading error: {ex.Message}"); }
        }

        private static void LoadHideoutProduction()
        {
            ServerDatabase.HideoutProduction = Array.Empty<HideoutProduction>();
            string path = AppSettings.UsingModHelper
                ? GetHelperDBFilePath("Production.json")
                : Path.Combine(AppSettings.ServerPath, AppSettings.FilesList[SPTServerFile.production]);
            try
            {
                HideoutProduction[] HideoutProduction = JsonConvert.DeserializeObject<HideoutProduction[]>(File.ReadAllText(path));
                ServerDatabase.HideoutProduction = HideoutProduction;
            }
            catch (Exception ex) { Logger.Log($"ServerDatabase HideoutProduction ({path}) loading error: {ex.Message}"); }
        }

        private static void LoadItemsDB()
        {
            ServerDatabase.ItemsDB = new();
            string path = AppSettings.UsingModHelper
                ? GetHelperDBFilePath("Items.json")
                : Path.Combine(AppSettings.ServerPath, AppSettings.FilesList[SPTServerFile.items]);
            try
            {
                Dictionary<string, TarkovItem> itemsDB = JsonConvert.DeserializeObject<Dictionary<string, TarkovItem>>(File.ReadAllText(path));
                ServerDatabase.ItemsDB = itemsDB;
            }
            catch (Exception ex) { Logger.Log($"ServerDatabase ItemsDB ({path}) loading error: {ex.Message}"); }
        }

        private static void FindPockets() => ServerDatabase.Pockets = ServerDatabase.ItemsDB
            .Where(x => x.Value.Parent == AppSettings.PocketsContainerTpl)
            .OrderBy(x => x.Value.SlotsCount)
            .ToDictionary(x => x.Key, x => GetPocketsName(x.Value));

        private static string GetPocketsName(TarkovItem x) =>
            $"{x.LocalizedName} ({x.SlotsCount})";

        private static void LoadTraderSuits()
        {
            ServerDatabase.TraderSuits = new();
            var traderSuits = new List<TraderSuit>();
            foreach (var tbase in Directory.GetDirectories(Path.Combine(AppSettings.ServerPath, AppSettings.DirsList[SPTServerDir.traders])))
            {
                try
                {
                    if (!File.Exists(Path.Combine(tbase, "suits.json")))
                        continue;
                    var traderSuitsList = JsonConvert.DeserializeObject<List<TraderSuit>>(File.ReadAllText(Path.Combine(tbase, "suits.json")));
                    foreach (var suit in traderSuitsList.Where(x => !traderSuits.Any(y => y.SuiteId == x.SuiteId)))
                        traderSuits.Add(suit);
                }
                catch (Exception ex) { Logger.Log($"ServerDatabase TraderSuits ({tbase}) loading error: {ex.Message}"); }
            }
            ServerDatabase.TraderSuits = traderSuits;
        }

        private static void LoadHandbook()
        {
            string path = AppSettings.UsingModHelper
                ? GetHelperDBFilePath("Handbook.json")
                : Path.Combine(AppSettings.ServerPath, AppSettings.FilesList[SPTServerFile.handbook]);
            try
            {
                Handbook handbook = JsonConvert.DeserializeObject<Handbook>(File.ReadAllText(path));
                ServerDatabase.Handbook = handbook;
            }
            catch (Exception ex)
            {
                ServerDatabase.Handbook = new();
                Logger.Log($"ServerDatabase Handbook ({path}) loading error: {ex.Message}");
            }
        }

        private static void LoadHandbookHelper()
        {
            try
            {
                ServerDatabase.HandbookHelper = new(ServerDatabase.Handbook.Categories,
                                                    ServerDatabase.ItemsDB,
                                                    ServerDatabase.ServerGlobals.GlobalBuilds);
            }
            catch (Exception ex)
            {
                ServerDatabase.HandbookHelper = new(new List<HandbookCategory>(),
                                                    new Dictionary<string, TarkovItem>(),
                                                    new ObservableCollection<WeaponBuild>());
                Logger.Log($"ServerDatabase HandbookHelper loading error: {ex.Message}");
            }
        }

        private static string GetHelperDBFilePath(string filename)
        {
            var path = Path.Combine(AppSettings.ServerPath, HelperModManager.DbPath, filename);
            return File.Exists(path) || (Directory.Exists(path) && Directory.GetFiles(path).Any())
                ? path
                : throw new Exception(AppLocalization.GetLocalizedString("db_load_helper_file_not_found"));
        }
    }
}