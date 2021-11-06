using SPT_AKI_Profile_Editor.Core.ProfileClasses;
using SPT_AKI_Profile_Editor.Core.ServerClasses;
using System.Collections.Generic;

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

        public static void LoadBotTypes()
        {
            ServerDatabase.Heads = new Dictionary<string, string>();
            ServerDatabase.Voices = new Dictionary<string, string>();
            //foreach (var btype in Directory.GetFiles(Path.Combine(Lang.options.EftServerPath, Lang.options.DirsList["dir_bots"])))
            //{
            //    BotType bot = JsonConvert.DeserializeObject<BotType>(File.ReadAllText(btype));
            //    if (bot.appearance.head != null)
            //        foreach (var head in bot.appearance.head)
            //            if (!Heads.ContainsKey(head))
            //                Heads.Add(head, globalLang.Customization.ContainsKey(head) ? globalLang.Customization[head].Name : head);
            //    if (bot.appearance.voice != null)
            //        foreach (var voice in bot.appearance.voice)
            //            if (!Voices.ContainsKey(voice))
            //                Voices.Add(voice, voice);
            //}
        }
    }
}