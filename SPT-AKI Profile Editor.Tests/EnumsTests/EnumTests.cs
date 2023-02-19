using Newtonsoft.Json;
using NUnit.Framework;
using System;

namespace SPT_AKI_Profile_Editor.Tests.EnumsTests
{
    internal class TestEnumReader : JsonReader
    {
        private readonly object _receivedValue;
        private readonly JsonToken _tokenType;

        public TestEnumReader(object receivedValue)
        {
            _receivedValue = receivedValue;
            _tokenType = receivedValue switch
            {
                string => JsonToken.String,
                int => JsonToken.Integer,
                _ => JsonToken.Null,
            };
        }

        public override object Value => _receivedValue;

        public override JsonToken TokenType => _tokenType;

        public override bool Read() => true;
    }

    internal abstract class EnumTests<T> where T : Enum
    {
        internal readonly Array allEnumValues = Enum.GetValues(typeof(T));

        public static void ConverterCanRead(object value, JsonConverter converter, T expectedValue)
        {
            JsonReader reader = new TestEnumReader(value);
            var obj = converter.ReadJson(reader, typeof(string), null, JsonSerializer.CreateDefault());

            Assert.That((T)obj, Is.Not.Null, "Readed value is null");
            Assert.That((T)obj, Is.InstanceOf<T>(), $"Readed value is not {nameof(T)}");
            Assert.That((T)obj, Is.EqualTo(expectedValue), $"Readed value is not {expectedValue}");
        }

        public abstract void ConverterCanConvert();

        public abstract void ConverterCanReadAllValues();

        public abstract void ConverterCanReadStringValue();

        public abstract void ConverterCanReadNotExistingStringValue();

        public abstract void ConverterCanReadIntegerValue();

        public abstract void ConverterCanReadNotSupportedValue();

        public abstract void ConverterCanReadNullValue();
    }
}