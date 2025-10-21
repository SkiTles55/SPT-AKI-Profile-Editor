using SPT_AKI_Profile_Editor.Core;
using SPT_AKI_Profile_Editor.Core.HelperClasses;
using System.Collections.Generic;
using System.Linq;

namespace SPT_AKI_Profile_Editor.Helpers
{
    public interface IMigrationHelper
    {
        MigrationIntent GetMigrationIntent(AppSettings settings, AppLocalization localization);

        void PerformMigration(AppSettings settings, IApplicationManager applicationManager);
    }

    public class MigrationIntent
    {
        public MigrationIntent(string title, string message, string tag)
        {
            Title = title;
            Message = message;
            Tag = tag;
        }

        public string Title { get; }
        public string Message { get; }
        public string Tag { get; }
    }

    public class MigrationHelper : IMigrationHelper
    {
        public void PerformMigration(AppSettings settings, IApplicationManager applicationManager)
        {
            settings.DirsList = DefaultValues.DefaultDirsList;
            settings.FilesList = DefaultValues.DefaultFilesList;
            settings.Save();
            applicationManager.DeleteLocalizations();
            applicationManager.RestartApplication();
        }

        public MigrationIntent GetMigrationIntent(AppSettings settings, AppLocalization localization)
        {
            if (MigrationRequered(settings))
                return new MigrationIntent(localization.GetLocalizedString("migration_to_4.0.1_title"),
                                           localization.GetLocalizedString("migration_to_4.0.1_message"),
                                           "pe4.0, spt4.0.1, relative paths migration");
            return null;
        }

        private static bool MigrationRequered(AppSettings settings)
        {
            var dirs = settings.DirsList.Select(x => !x.Value.StartsWith("SPT"));
            var dirs2 = settings.DirsList.Select(x => x.Key != SPTServerDir.profiles && x.Value.Contains("Server"));
            var files = settings.FilesList.Select(x => !x.Value.StartsWith("SPT"));
            KeyValuePair<string, string>? tradersImagesPath = settings.DirsList.Where(x => x.Key == SPTServerDir.traderImages).FirstOrDefault();
            var oldTraderIcons = tradersImagesPath?.Value != DefaultValues.DefaultDirsList.FirstOrDefault(x => x.Key == SPTServerDir.traderImages).Value;
            return dirs.Any(x => x) || dirs.Any(x => x) || files.Any(x => x) || oldTraderIcons;
        }
    }
}