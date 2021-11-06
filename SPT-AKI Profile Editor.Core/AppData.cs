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
        public static AppSettings AppSettings;
        public static AppLocalization AppLocalization;
        public static Profile Profile;
        public static ServerDatabase ServerDatabase;

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
                LoadGlobal();
                LoadBotTypes();
            }
        }

        private static void LoadGlobal()
        {
            ServerDatabase.LocalesGlobal = new();
            string path = Path.Combine(AppSettings.ServerPath, AppSettings.DirsList["dir_globals"], AppSettings.Language + ".json");
            try
            {
                LocalesGlobal global = JsonSerializer.Deserialize<LocalesGlobal>(File.ReadAllText(path));
                ServerDatabase.LocalesGlobal = global;
            }
            catch (Exception ex) { Logger.Log($"ServerDatabase Global ({path}) loading error: {ex.Message}"); }
        }

        private static void LoadBotTypes()
        {
            ServerDatabase.Heads = new Dictionary<string, string>();
            ServerDatabase.Voices = new Dictionary<string, string>();
            foreach (var btype in Directory.GetFiles(Path.Combine(AppSettings.ServerPath, AppSettings.DirsList["dir_bots"])))
            {
                try
                {
                    BotType bot = JsonSerializer.Deserialize<BotType>(File.ReadAllText(btype));
                    if (bot.Appearance.Heads != null)
                        foreach (var head in bot.Appearance.Heads)
                            if (!ServerDatabase.Heads.ContainsKey(head))
                                ServerDatabase.Heads.Add(head, ServerDatabase.LocalesGlobal.Customization.ContainsKey(head) ? ServerDatabase.LocalesGlobal.Customization[head].Name : head);
                    if (bot.Appearance.Voices != null)
                        foreach (var voice in bot.Appearance.Voices)
                            if (!ServerDatabase.Voices.ContainsKey(voice))
                                ServerDatabase.Voices.Add(voice, voice);
                }
                catch (Exception ex) { Logger.Log($"ServerDatabase BotType ({btype}) loading error: {ex.Message}"); }
            }
        }
    }
}