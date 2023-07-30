using Newtonsoft.Json;
using SPT_AKI_Profile_Editor.Core.HelperClasses;
using SPT_AKI_Profile_Editor.Core.ProfileClasses;
using SPT_AKI_Profile_Editor.Core.ServerClasses;
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
        public static readonly ServerDatabase ServerDatabase;
        public static readonly GridFilters GridFilters;
        public static readonly BackupService BackupService;
        public static readonly IssuesService IssuesService;

        private static readonly bool IsRunningFromNUnit = AppDomain.CurrentDomain.GetAssemblies().Any(a => a.FullName.ToLowerInvariant().StartsWith("nunit.framework"));
        private static readonly string AppDataPath = IsRunningFromNUnit ? Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TestAppData") : DefaultValues.AppDataFolder;
        private static readonly string HelperDbPath = "user\\mods\\ProfileEditorHelper\\exportedDB";

        static AppData()
        {
            GridFilters = new();
            AppSettings = new(Path.Combine(AppDataPath, "AppSettings.json"));
            AppSettings.Load();
            BackupService = new(Path.Combine(AppDataPath, "Backups"));
            AppLocalization = new(AppSettings.Language, Path.Combine(AppDataPath, "Localizations"));
            IssuesService = new();
            Profile = new();
            ServerDatabase = new();
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
                LoadQuestsData();
                LoadHideoutAreaInfos();
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
            string path = Path.Combine(AppSettings.ServerPath, AppSettings.DirsList[SPTServerDir.globals], AppSettings.Language + ".json");
            try
            {
                Dictionary<string, string> global = JsonConvert.DeserializeObject<Dictionary<string, string>>(File.ReadAllText(path));
                ServerDatabase.LocalesGlobal = global;
            }
            catch (Exception ex) { Logger.Log($"ServerDatabase LocalesGlobal ({path}) loading error: {ex.Message}"); }
        }

        private static void LoadBotTypes()
        {
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
                        foreach (var head in bot.Appearance.Heads)
                            if (!Heads.ContainsKey(head))
                                Heads.Add(head, ServerDatabase.LocalesGlobal.ContainsKey(head.Name()) ? ServerDatabase.LocalesGlobal[head.Name()] : head);
                    if (bot.Appearance.Voices != null)
                        foreach (var voice in bot.Appearance.Voices)
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
            ServerDatabase.ServerGlobals = new();
            string path = Path.Combine(AppSettings.ServerPath, AppSettings.FilesList[SPTServerFile.globals]);
            try
            {
                ServerGlobals global = JsonConvert.DeserializeObject<ServerGlobals>(File.ReadAllText(path));
                ServerDatabase.ServerGlobals = global;
            }
            catch (Exception ex) { Logger.Log($"ServerDatabase ServerGlobals ({path}) loading error: {ex.Message}"); }
        }

        private static void LoadTradersInfos()
        {
            ServerDatabase.TraderInfos = new();
            var traderInfos = new Dictionary<string, TraderBase>();
            foreach (var tbase in Directory.GetDirectories(Path.Combine(AppSettings.ServerPath, AppSettings.DirsList[SPTServerDir.traders])))
            {
                if (!File.Exists(Path.Combine(tbase, "base.json")))
                    continue;
                try
                {
                    traderInfos.Add(Path.GetFileNameWithoutExtension(tbase), JsonConvert.DeserializeObject<TraderBase>(File.ReadAllText(Path.Combine(tbase, "base.json"))));
                }
                catch (Exception ex) { Logger.Log($"ServerDatabase TraderInfo ({tbase}) loading error: {ex.Message}"); }
            }
            ServerDatabase.TraderInfos = traderInfos;
        }

        private static void LoadQuestsData()
        {
            ServerDatabase.QuestsData = new();
            string path = Path.Combine(AppSettings.ServerPath, AppSettings.FilesList[SPTServerFile.quests]);
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

        private static void LoadItemsDB()
        {
            ServerDatabase.ItemsDB = new();
            string path = AppSettings.UsingModHelper
                ? GetHelperDBFilePath("items.json")
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
            string path = Path.Combine(AppSettings.ServerPath, AppSettings.FilesList[SPTServerFile.handbook]);
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
                                                    new ObservableCollection<KeyValuePair<string, WeaponBuild>>());
                Logger.Log($"ServerDatabase HandbookHelper loading error: {ex.Message}");
            }
        }

        private static string GetHelperDBFilePath(string filename)
        {
            var path = Path.Combine(AppSettings.ServerPath, HelperDbPath, filename);
            if (!File.Exists(path))
                throw new Exception("HelperDBFile not found");
            return path;
        }
    }
}