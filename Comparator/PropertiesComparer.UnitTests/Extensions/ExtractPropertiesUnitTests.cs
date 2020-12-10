using FluentAssertions;
using NUnit.Framework;
using PropertiesComparer.Extensions;
using PropertiesComparer.UnitTests.Extensions.ExtractPropertiesTestData;
using System;

namespace PropertiesComparer.UnitTests.Extensions
{
    public class ExtractPropertiesUnitTests
    {
        [TestCase(typeof(NoProperties))]
        [TestCase(typeof(NoPublicProperties))]
        [TestCase(typeof(ComplexPropertiesNonEqutables))]
        [TestCase(typeof(ColecionProperties))]
        [TestCase(typeof(OnlyMembers))]
        [TestCase(typeof(ImproperNullableTypes))]
        public void When_Property_Array_Should_Be_Empty(Type type)
        {
            var data = type.ExtractProperties();
            const int expectedSize = 0;

            data.Length.Should().Be(expectedSize);
        }

        [TestCase(typeof(Enums), 3)]
        [TestCase(typeof(PrimitiveTypes), 13)]
        [TestCase(typeof(NullableTypes), 13)]
        [TestCase(typeof(ExpectedTypes), 17)]
        [TestCase(typeof(InhertitanceExpectedTypes), 17)]
        [TestCase(typeof(InhertitanceChain), 3)]
        [TestCase(typeof(RecordWithList), 3)]
        [TestCase(typeof(RecordWithProperties), 3)]
        public void When_Property_Array_Should_Have_Data(Type type, int expectedSize)
        {
            var data = type.ExtractProperties();

            data.Length.Should().Be(expectedSize);
        }
    }
}
