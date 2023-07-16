using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SPT_AKI_Profile_Editor.Helpers
{
    public static class JTokenExtension
    {
        public static JToken RemoveFields(this JToken token, IEnumerable<string> fields)
        {
            if (token is not JContainer container)
                return token;

            List<JToken> removeList = new();
            foreach (JToken el in container.Children())
            {
                if (el is JProperty p && fields.Contains(p.Name))
                    removeList.Add(el);
                el.RemoveFields(fields);
            }

            foreach (JToken el in removeList)
                el.Remove();

            return token;
        }
    }
}