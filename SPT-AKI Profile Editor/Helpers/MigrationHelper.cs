using SPT_AKI_Profile_Editor.Core;
using SPT_AKI_Profile_Editor.Core.HelperClasses;
using System.Collections.Generic;
using System.Linq;

namespace SPT_AKI_Profile_Editor.Helpers
{
    /// <summary>
    /// 迁移助手接口。
    /// 提供检查是否需要迁移和执行配置迁移的能力。
    /// </summary>
    public interface IMigrationHelper
    {
        MigrationIntent GetMigrationIntent(AppSettings settings, AppLocalization localization);

        void PerformMigration(AppSettings settings, IApplicationManager applicationManager);
    }

    /// <summary>
    /// 迁移意图描述。
    /// 包含迁移标题、提示信息和唯一标签，用于确认用户是否执行迁移。
    /// </summary>
    public class MigrationIntent(string title, string message, string tag)
    {
        public string Title { get; } = title;
        public string Message { get; } = message;
        public string Tag { get; } = tag;
    }

    /// <summary>
    /// 迁移实现类。
    /// 检测旧版 path 配置并更新为当前默认目录结构，必要时重启应用。
    /// </summary>
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