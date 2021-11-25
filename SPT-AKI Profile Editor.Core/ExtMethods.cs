using Newtonsoft.Json.Linq;
using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace SPT_AKI_Profile_Editor.Core
{
    public static class ExtMethods
    {
        private static readonly JsonSerializerOptions _serializerOptions = new()
        {
            WriteIndented = true,
            Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        };

        public static string WindowsCulture => CultureInfo.CurrentCulture.Parent.ToString();

        public static void SaveJson(string path, object data) => File.WriteAllText(path, JsonSerializer.Serialize(data, _serializerOptions));

        public static bool PathIsServerFolder(AppSettings appSettings, string path = null)
        {
            if (string.IsNullOrEmpty(path)) path = appSettings.ServerPath;
            if (string.IsNullOrEmpty(path)) return false;
            if (!Directory.Exists(path)) return false;
            if (appSettings.FilesList.Any(x => !File.Exists(Path.Combine(path, x.Value)))) return false;
            if (appSettings.DirsList.Any(x => !Directory.Exists(Path.Combine(path, x.Value)))) return false;

            return true;
        }
        public static bool ServerHaveProfiles(AppSettings appSettings) => appSettings.ServerProfiles != null && appSettings.ServerProfiles.Count > 0;

        public static JObject RemoveNullAndEmptyProperties(JObject jObject)
        {
            while (jObject.Descendants().Any(IsNullOrEmptyProperty()))
                foreach (var jt in jObject.Descendants().Where(IsNullOrEmptyProperty()).ToArray())
                    jt.Remove();
            return jObject;
        }

        private static Func<JToken, bool> IsNullOrEmptyProperty() =>
            jt => jt.Type == JTokenType.Property && IsNullOrEmpty(jt);

        private static bool IsNullOrEmpty(JToken jt) =>
            jt.Values()
            .All(a => a.Type == JTokenType.Null) || !jt.Values().Any();
    }
}