using NUnit.Framework;
using SPT_AKI_Profile_Editor.Core.Enums;
using SPT_AKI_Profile_Editor.Core.ServerClasses.Hideout;

namespace SPT_AKI_Profile_Editor.Tests.EnumsTests
{
    internal class RequirementTypeTests : EnumTests<RequirementType>
    {
        private readonly SafeEnumConverter<RequirementType> converter = new();

        [Test]
        public override void ConverterCanConvert()
            => Assert.That(converter.CanConvert(typeof(RequirementType)), Is.True);

        [Test]
        public override void ConverterCanReadAllValues()
        {
            foreach (RequirementType questType in allEnumValues)
                ConverterCanRead(questType.ToString(), converter, questType);
        }

        [Test]
        public override void ConverterCanReadIntegerValue()
            => ConverterCanRead(0, converter, RequirementType.QuestComplete);

        [Test]
        public override void ConverterCanReadNotExistingStringValue()
            => ConverterCanRead("NotExistingValue", converter, RequirementType.Unknown);

        [Test]
        public override void ConverterCanReadNotSupportedValue()
            => ConverterCanRead(76f, converter, RequirementType.Unknown);

        [Test]
        public override void ConverterCanReadNullValue()
            => ConverterCanRead(null, converter, RequirementType.Unknown);

        [Test]
        public override void ConverterCanReadStringValue()
            => ConverterCanRead("QuestComplete", converter, RequirementType.QuestComplete);
    }
}