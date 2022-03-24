using System;
using System.Linq;
using System.Net.Http;
using System.Reflection;

namespace SPT_AKI_Profile_Editor.Core
{
    public class UpdatesChecker
    {
        private static readonly HttpClient HttpClient;

        static UpdatesChecker()
        {
            HttpClient = new HttpClient();
        }

        public static bool CheckUpdate(string link = null, Version version = null)
        {
            try
            {
                if (string.IsNullOrEmpty(link))
                    link = AppSettings.RepositoryLink;
                if (version == null)
                    version = GetVersion();

                using var responce = HttpClient.GetAsync(new Uri(link)).Result;
                var responseUrl = responce.RequestMessage.RequestUri.ToString();
                if (responseUrl == link)
                    return false;

                Version latest = new(responseUrl.Split('/').Last());

                return latest > version;
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
            return $"SPT-AKI Profile Editor {$" {version.Major}.{version.Minor}"}" + (version.Build != 0 ? "." + version.Build.ToString() : "");
        }
    }
}