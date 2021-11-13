using SPT_AKI_Profile_Editor.Core.ProfileClasses;
using SPT_AKI_Profile_Editor.Core.ServerClasses;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace SPT_AKI_Profile_Editor.Core
{
    public static class AppData
    {
        public readonly static AppSettings AppSettings;
        public readonly static AppLocalization AppLocalization;
        public readonly static Profile Profile;
        public readonly static ServerDatabase ServerDatabase;

        static AppData()
        {
            AppSettings = new();
            AppSettings.Load();
            AppLocalization = new(AppSettings.Language);
            Profile = new();
            ServerDatabase = new();
        }

        public static void LoadDatabase()
        {
            if (ExtMethods.PathIsServerFolder(AppSettings))
            {
                LoadLocalesGlobal();
                LoadBotTypes();
                LoadServerGlobals();
                LoadTradersInfos();
            }
        }

        private static void LoadLocalesGlobal()
        {
            ServerDatabase.LocalesGlobal = new();
            string path = Path.Combine(AppSettings.ServerPath, AppSettings.DirsList["dir_globals"], AppSettings.Language + ".json");
            try
            {
                LocalesGlobal global = JsonSerializer.Deserialize<LocalesGlobal>(File.ReadAllText(path));
                ServerDatabase.LocalesGlobal = global;
            }
            catch (Exception ex) { Logger.Log($"ServerDatabase LocalesGlobal ({path}) loading error: {ex.Message}"); }
        }

        private static void LoadBotTypes()
        {
            Dictionary<string, string> Heads = new();
            Dictionary<string, string> Voices = new();
            foreach (var btype in Directory.GetFiles(Path.Combine(AppSettings.ServerPath, AppSettings.DirsList["dir_bots"])))
            {
                try
                {
                    BotType bot = JsonSerializer.Deserialize<BotType>(File.ReadAllText(btype));
                    if (bot.Appearance.Heads != null)
                        foreach (var head in bot.Appearance.Heads)
                            if (!Heads.ContainsKey(head))
                                Heads.Add(head, ServerDatabase.LocalesGlobal.Customization.ContainsKey(head) ? ServerDatabase.LocalesGlobal.Customization[head].Name : head);
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
            string path = Path.Combine(AppSettings.ServerPath, AppSettings.FilesList["file_globals"]);
            try
            {
                ServerGlobals global = JsonSerializer.Deserialize<ServerGlobals>(File.ReadAllText(path));
                ServerDatabase.ServerGlobals = global;
            }
            catch (Exception ex) { Logger.Log($"ServerDatabase ServerGlobals ({path}) loading error: {ex.Message}"); }
        }

        private static void LoadTradersInfos()
        {
            ServerDatabase.TraderInfos = new();
            var traderInfos = new Dictionary<string, TraderBase>();
            foreach (var tbase in Directory.GetDirectories(Path.Combine(AppSettings.ServerPath, AppSettings.DirsList["dir_traders"])))
            {
                if (Path.GetFileNameWithoutExtension(tbase) == "ragfair")
                    continue;
                traderInfos.Add(Path.GetFileNameWithoutExtension(tbase), JsonSerializer.Deserialize<TraderBase>(File.ReadAllText(Path.Combine(tbase, "base.json"))));
            }
            ServerDatabase.TraderInfos = traderInfos;
        }
    }
}