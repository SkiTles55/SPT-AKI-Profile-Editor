using Newtonsoft.Json.Linq;
using System;
using System.Linq;

namespace SPT_AKI_Profile_Editor.Helpers
{
    public static class JObjectExtension
    {
        public static JObject RemoveNullAndEmptyProperties(this JObject jObject)
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