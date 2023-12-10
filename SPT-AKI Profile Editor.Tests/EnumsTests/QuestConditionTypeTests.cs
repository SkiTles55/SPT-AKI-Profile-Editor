using NUnit.Framework;
using SPT_AKI_Profile_Editor.Core.Enums;

namespace SPT_AKI_Profile_Editor.Tests.EnumsTests
{
    internal class QuestConditionTypeTests : EnumTests<QuestConditionType>
    {
        private readonly SafeEnumConverter<QuestConditionType> converter = new();

        [Test]
        public override void ConverterCanConvert() => Assert.That(converter.CanConvert(typeof(QuestConditionType)), Is.True);

        [Test]
        public override void ConverterCanReadAllValues()
        {
            foreach (QuestConditionType questType in allEnumValues)
                ConverterCanRead(questType.ToString(), converter, questType);
        }

        [Test]
        public override void ConverterCanReadIntegerValue() => ConverterCanRead(0, converter, QuestConditionType.Level);

        [Test]
        public override void ConverterCanReadNotExistingStringValue() => ConverterCanRead("NotExistingValue", converter, QuestConditionType.Unknown);

        [Test]
        public override void ConverterCanReadNotSupportedValue() => ConverterCanRead(76f, converter, QuestConditionType.Unknown);

        [Test]
        public override void ConverterCanReadNullValue() => ConverterCanRead(null, converter, QuestConditionType.Unknown);

        [Test]
        public override void ConverterCanReadStringValue() => ConverterCanRead("TraderLoyalty", converter, QuestConditionType.TraderLoyalty);
    }
}