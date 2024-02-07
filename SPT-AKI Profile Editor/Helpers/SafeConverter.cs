using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace SPT_AKI_Profile_Editor.Helpers
{
    public class ValidListConverter<T> : JsonConverter<List<T>>
    {
        public override List<T> ReadJson(JsonReader reader, Type objectType, List<T> existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            List<T> list = new();
            var token = JToken.Load(reader);

            if (token.Type == JTokenType.Array)
                foreach (JToken item in token.Children())
                {
                    try
                    {
                        T value = item.ToObject<T>(serializer);
                        if (value != null)
                            list.Add(value);
                    }
                    catch { }
                }

            return list;
        }

        public override void WriteJson(JsonWriter writer, List<T> value, JsonSerializer serializer)
        {
            writer.WriteStartArray();
            foreach (var item in value)
            {
                JToken token = JToken.FromObject(item, serializer);
                token.WriteTo(writer);
            }
            writer.WriteEndArray();
        }
    }
}