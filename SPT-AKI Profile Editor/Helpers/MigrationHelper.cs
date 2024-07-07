using SPT_AKI_Profile_Editor.Core;
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
                return new MigrationIntent(localization.GetLocalizedString("migration_to_3.9.0_title"),
                                           localization.GetLocalizedString("migration_to_3.9.0_message"),
                                           "pe3.0, spt3.9.0, relative paths migration");
            return null;
        }

        private static bool MigrationRequered(AppSettings settings)
        {
            var dirs = settings.DirsList.Select(x => x.Value.StartsWith("Aki"));
            var files = settings.FilesList.Select(x => x.Value.StartsWith("Aki"));
            return dirs.Any(x => x) || files.Any(x => x);
        }
    }
}