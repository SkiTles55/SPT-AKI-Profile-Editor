using SPT_AKI_Profile_Editor.Core;
using SPT_AKI_Profile_Editor.Core.HelperClasses;
using System.Linq;

namespace SPT_AKI_Profile_Editor.Helpers
{
    public interface IMigrationHelper
    {
        MigrationIntent GetMigrationIntent(AppSettings settings, AppLocalization localization);

        void PerformMigration(AppSettings settings, IApplicationManager applicationManager);
    }

    public class MigrationIntent(string title, string message, string tag)
    {
        public string Title { get; } = title;
        public string Message { get; } = message;
        public string Tag { get; } = tag;
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
                return new MigrationIntent(localization.GetLocalizedString("migration_to_4.1.1_title"),
                                           localization.GetLocalizedString("migration_to_4.1.1_message"),
                                           "pe4.1, spt4.1.1, SPT_Runtime paths migration");
            return null;
        }

        private static bool MigrationRequered(AppSettings settings)
        {
            var defaultDirs = DefaultValues.DefaultDirsList;
            var defaultFiles = DefaultValues.DefaultFilesList;
            return settings.DirsList.Any(x => !defaultDirs.TryGetValue(x.Key, out var path) || x.Value != path)
                || settings.FilesList.Any(x => !defaultFiles.TryGetValue(x.Key, out var path) || x.Value != path);
        }
    }
}
