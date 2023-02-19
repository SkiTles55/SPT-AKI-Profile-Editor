using NUnit.Framework;
using SPT_AKI_Profile_Editor.Core;
using SPT_AKI_Profile_Editor.Core.Enums;
using SPT_AKI_Profile_Editor.Tests.Hepers;

namespace SPT_AKI_Profile_Editor.Tests.EnumsTests
{
    internal class ActiveQuestTypeTests : EnumTests<ActiveQuestType>
    {
        private readonly SafeEnumConverter<ActiveQuestType> converter = new();

        [OneTimeSetUp]
        public void Setup()
        {
            AppData.AppSettings.Language = "ru";
            TestHelpers.LoadDatabase();
        }

        [Test]
        public void ActiveQuestTypeHaveLocalizedNames()
        {
            foreach (ActiveQuestType questType in allEnumValues)
                Assert.IsFalse(questType != ActiveQuestType.Unknown && questType.LocalizedName() == questType.ToString());
        }

        [Test]
        public override void ConverterCanConvert() => Assert.IsTrue(converter.CanConvert(typeof(ActiveQuestType)));

        [Test]
        public override void ConverterCanReadAllValues()
        {
            foreach (ActiveQuestType questType in allEnumValues)
                ConverterCanRead(questType.ToString(), converter, questType);
        }

        [Test]
        public override void ConverterCanReadStringValue() => ConverterCanRead("Elimination", converter, ActiveQuestType.Elimination);

        [Test]
        public override void ConverterCanReadNotExistingStringValue() => ConverterCanRead("NotExistingValue", converter, ActiveQuestType.Unknown);

        [Test]
        public override void ConverterCanReadIntegerValue() => ConverterCanRead(0, converter, ActiveQuestType.Completion);

        [Test]
        public override void ConverterCanReadNotSupportedValue() => ConverterCanRead(76f, converter, ActiveQuestType.Unknown);

        [Test]
        public override void ConverterCanReadNullValue() => ConverterCanRead(null, converter, ActiveQuestType.Unknown);
    }
}