using NUnit.Framework;
using SPT_AKI_Profile_Editor.Core.Enums;
using SPT_AKI_Profile_Editor.Tests.Hepers;

namespace SPT_AKI_Profile_Editor.Tests.EnumsTests
{
    internal class QuestTypeTests : EnumTests<QuestType>
    {
        private readonly SafeEnumConverter<QuestType> converter = new();

        [Test]
        public void ActiveQuestTypeHaveLocalizedNames()
        {
            TestHelpers.LoadDatabase();
            foreach (QuestType questType in allEnumValues)
                Assert.That(questType.LocalizedName().StartsWith("tab_quests_"), Is.False);
        }

        [Test]
        public override void ConverterCanConvert()
            => Assert.That(converter.CanConvert(typeof(QuestType)), Is.True);

        [Test]
        public override void ConverterCanReadAllValues()
        {
            foreach (QuestType questType in allEnumValues)
                ConverterCanRead(questType.ToString(), converter, questType);
        }

        [Test]
        public override void ConverterCanReadIntegerValue()
            => ConverterCanRead(0, converter, QuestType.Standart);

        [Test]
        public override void ConverterCanReadNotExistingStringValue()
            => ConverterCanRead("NotExistingValue", converter, QuestType.Unknown);

        [Test]
        public override void ConverterCanReadNotSupportedValue()
            => ConverterCanRead(76f, converter, QuestType.Unknown);

        [Test]
        public override void ConverterCanReadNullValue()
            => ConverterCanRead(null, converter, QuestType.Unknown);

        [Test]
        public override void ConverterCanReadStringValue()
            => ConverterCanRead("Daily_Savage", converter, QuestType.Daily_Savage);
    }
}