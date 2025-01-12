using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace SPT_AKI_Profile_Editor.Core.ServerClasses
{
    [JsonConverter(typeof(BotTypeAppearanceConverter))]
    internal class BotTypeAppearance
    {
        [JsonProperty("head")]
        public Dictionary<string, int> Heads { get; set; }

        [JsonProperty("voice")]
        public Dictionary<string, int> Voices { get; set; }
    }

    internal class BotTypeAppearanceConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType) => objectType == typeof(BotTypeAppearance);

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JObject jo = JObject.Load(reader);
            var botTypeAppearance = new BotTypeAppearance
            {
                Heads = jo["head"].Type == JTokenType.Array ? new Dictionary<string, int>() : jo["head"].ToObject<Dictionary<string, int>>(),
                Voices = jo["voice"].Type == JTokenType.Array ? new Dictionary<string, int>() : jo["voice"].ToObject<Dictionary<string, int>>()
            };
            return botTypeAppearance;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var botTypeAppearance = (BotTypeAppearance)value;
            JObject jo = new()
            {
                { "head", JToken.FromObject(botTypeAppearance.Heads) },
                { "voice", JToken.FromObject(botTypeAppearance.Voices) }
            };
            jo.WriteTo(writer);
        }
    }
}