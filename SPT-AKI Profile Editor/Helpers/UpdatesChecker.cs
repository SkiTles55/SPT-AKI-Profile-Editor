using ReleaseChecker.GitHub;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace SPT_AKI_Profile_Editor.Core
{
    public class UpdatesChecker
    {
        private static readonly GitHubChecker gitHubChecker;

        static UpdatesChecker()
        {
            gitHubChecker = new GitHubChecker(AppData.AppSettings.repoAuthor, AppData.AppSettings.repoName);
        }

        public static async Task<GitHubRelease> CheckUpdate()
        {
            try
            {
                var currentVersion = GetVersion();
                var latestRelease = await gitHubChecker.GetLatestReleaseAsync(true);
                if (latestRelease != null && new Version(latestRelease.Tag) > currentVersion)
                    return latestRelease;
                Logger.Log($"No updates available");
                return null;
            }
            catch (Exception ex)
            {
                Logger.Log($"UpdatesChecker error : {ex.Message}");
                return null;
            }
        }

        public static Version GetVersion() => Assembly.GetExecutingAssembly().GetName().Version;

        public static string GetAppTitleWithVersion()
        {
            Version version = GetVersion();
            return $"SPT-AKI Profile Editor {$" {version.Major}.{version.Minor}"}" + (version.Build != 0 ? "." + version.Build.ToString() : "");
        }
    }
}