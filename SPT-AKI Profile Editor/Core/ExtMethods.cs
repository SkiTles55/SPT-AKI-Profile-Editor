using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;

namespace SPT_AKI_Profile_Editor.Core
{
    public static class ExtMethods
    {
        public static string WindowsCulture => CultureInfo.CurrentCulture.Parent.ToString();

        public static void OpenUrl(string url)
        {
            ProcessStartInfo link = new(url)
            {
                UseShellExecute = true
            };
            Process.Start(link);
        }

        public static JObject RemoveNullAndEmptyProperties(JObject jObject)
        {
            while (jObject.Descendants().Any(IsNullOrEmptyProperty()))
                foreach (var jt in jObject.Descendants().Where(IsNullOrEmptyProperty()).ToArray())
                    jt.Remove();
            return jObject;
        }

        public static string GenerateNewId(IEnumerable<string> ids)
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