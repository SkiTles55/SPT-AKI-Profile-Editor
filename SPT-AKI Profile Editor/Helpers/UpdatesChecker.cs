using System;
using System.Linq;
using System.Net;
using System.Reflection;

namespace SPT_AKI_Profile_Editor.Core
{
    public class UpdatesChecker
    {
        public static bool CheckUpdate(string link = null, Version version = null)
        {
            try
            {
                if (string.IsNullOrEmpty(link))
                    link = AppSettings.RepositoryLink;
                if (version == null)
                    version = GetVersion();
                WebRequest request = WebRequest.Create(link);
                WebResponse response = request.GetResponse();
                float currentVersion = float.Parse(string.Format(" {0},{1}", version.Major, version.Minor));
                float latestVersion = currentVersion;
                if (response.ResponseUri != null)
                    latestVersion = float.Parse(response.ResponseUri.ToString().Split('/').Last().Replace(".", ","));
                return latestVersion > currentVersion;
            }
            catch (Exception ex)
            {
                Logger.Log($"UpdatesChecker error : {ex.Message}");
                return false;
            }
        }

        public static Version GetVersion() => Assembly.GetExecutingAssembly().GetName().Version;

        public static string GetAppTitleWithVersion()
        {
            Version version = GetVersion();
            return $"SPT-AKI Profile Editor {$" {version.Major}.{version.Minor}"}";
        }
    }
}