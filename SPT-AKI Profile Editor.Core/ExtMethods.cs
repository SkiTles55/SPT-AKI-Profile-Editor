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

        public static string GenerateNewId(string[] ids)
        {
            string id;
            do
            {
                var getTime = DateTime.Now;
                Random rnd = new();
                var random = rnd.Next(100000000, 999999999).ToString();
                var retVal = $"I{getTime:MM}{getTime:dd}{getTime:HH}{getTime:mm}{getTime:ss}{random}";
                var sign = MakeSign(24 - retVal.Length, rnd);
                id = retVal + sign;
            } while (ids.Contains(id));
            return id;
        }

        private static Func<JToken, bool> IsNullOrEmptyProperty() =>
            jt => jt.Type == JTokenType.Property && IsNullOrEmpty(jt);

        private static bool IsNullOrEmpty(JToken jt) =>
            jt.Values()
            .All(a => a.Type == JTokenType.Null) || !jt.Values().Any();

        private static string MakeSign(int length, Random random)
        {
            var result = "";
            var characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            for (int i = 0; i < length; i++)
                result += characters.ElementAt((int)Math.Floor(random.NextDouble() * characters.Length));
            return result;
        }
    }
}