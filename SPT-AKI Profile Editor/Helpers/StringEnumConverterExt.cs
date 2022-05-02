using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using SPT_AKI_Profile_Editor.Core.Enums;

namespace SPT_AKI_Profile_Editor.Helpers
{
    public class StringEnumConverterExt : StringEnumConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value is ItemRotation)
            {
                writer.WriteValue(value.ToString());
                return;
            }

            base.WriteJson(writer, value, serializer);
        }
    }
}