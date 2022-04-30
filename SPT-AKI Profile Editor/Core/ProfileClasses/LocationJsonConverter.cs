using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace SPT_AKI_Profile_Editor.Core.ProfileClasses
{
    public class LocationJsonConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            throw new NotImplementedException();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.Value == null)
            {
                JObject jo = JObject.Load(reader);

                ItemLocation loc = new();
                serializer.Populate(jo.CreateReader(), loc);

                return loc;
            }
            else if (reader.TokenType == JsonToken.Integer)
                return new ItemLocation { SimpleNumber = Convert.ToInt32(reader.Value) };
            else
                return null;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value is ItemLocation ciil)
            {
                if (ciil.SimpleNumber.HasValue)
                    writer.WriteValue(ciil.SimpleNumber.Value);
                else
                    serializer.Serialize(writer, new { x = ciil.X, y = ciil.Y, r = ciil.R, isSearched = ciil.IsSearched });
            }
            else
                throw new NotImplementedException();
        }
    }
}