using NUnit.Framework;
using SPT_AKI_Profile_Editor.Core.Enums;

namespace SPT_AKI_Profile_Editor.Tests.EnumsTests
{
    internal class QuestStatusTests : EnumTests<QuestStatus>
    {
        private readonly SafeEnumConverter<QuestStatus> converter = new();

        [Test]
        public override void ConverterCanConvert() => Assert.IsTrue(converter.CanConvert(typeof(QuestStatus)));

        [Test]
        public override void ConverterCanReadAllValues()
        {
            foreach (QuestStatus questType in allEnumValues)
                ConverterCanRead(questType.ToString(), converter, questType);
        }

        [Test]
        public override void ConverterCanReadIntegerValue() => ConverterCanRead(1, converter, QuestStatus.AvailableForStart);

        [Test]
        public override void ConverterCanReadNotExistingStringValue() => ConverterCanRead("NotExistingValue", converter, QuestStatus.Fail);

        [Test]
        public override void ConverterCanReadNotSupportedValue() => ConverterCanRead(76f, converter, QuestStatus.Fail);

        [Test]
        public override void ConverterCanReadNullValue() => ConverterCanRead(null, converter, QuestStatus.Fail);

        [Test]
        public override void ConverterCanReadStringValue() => ConverterCanRead("FailRestartable", converter, QuestStatus.FailRestartable);
    }
}