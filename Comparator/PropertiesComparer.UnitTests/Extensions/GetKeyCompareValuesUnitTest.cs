using FluentAssertions;
using NUnit.Framework;
using PropertiesComparer.Extensions;
using PropertiesComparer.UnitTests.Extensions.GetKeyCompareValuesTestCases;
using PropertiesComparer.UnitTests.Extensions.GetKeyCompareValuesTestData;
using System.Linq;

namespace PropertiesComparer.UnitTests.Extensions
{
    public class GetKeyCompareValuesUnitTest
    {
        [Test]
        public void When_Type_Is_Null_Then_Data_Is_Empty()
        {
            var keys = ((CustomType)null).GetKeys(new string[] { });
            var compares = ((CustomType)null).GetCompares(new string[] { });

            keys.Should().NotBeNull();
            keys.Count.Should().Be(0);

            compares.Should().NotBeNull();
            compares.Count.Should().Be(0);
        }

        [TestCaseSource(typeof(GetKeyCompareValuesCases), nameof(GetKeyCompareValuesCases.NullEmptyArrayOrEmptyValues))]
        public void When_Array_Is_Null_Or_Empty_Then_Data_Is_Empty(string [] data)
        {
            var customType = new CustomType();

            var keys = customType.GetKeys(data);
            var compares = customType.GetCompares(data);

            keys.Should().NotBeNull();
            keys.Count.Should().Be(0);

            compares.Should().NotBeNull();
            compares.Count.Should().Be(0);
        }

        [Test]
        public void When_Property_Name_Is_NotPresent_Then_Data_Is_Empty()
        {
            var customType = new CustomType();

            var keys = customType.GetKeys(new[] { "unknown"});
            var compares = customType.GetCompares(new[] { "unknown" });

            keys.Should().NotBeNull();
            keys.Count.Should().Be(0);

            compares.Should().NotBeNull();
            compares.Count.Should().Be(0);
        }

        [Test]
        public void When_Property_Case_Not_Match_Then_Data_Is_Empty()
        {
            var customType = new CustomType();

            var keys = customType.GetKeys(new[] { "t1", "t2", "t3" });
            var compares = customType.GetCompares(new[] { "t5", "t7", "t8" });

            keys.Should().NotBeNull();
            keys.Count.Should().Be(0);

            compares.Should().NotBeNull();
            compares.Count.Should().Be(0);
        }



        [Test]
        public void When_Key_Compare_Are_Set_Then_Keys_And_Compares_Data_Are_Set()
        {
            var customType = new CustomType();

            var keyCompareData = new KeyCompareData(new CustomType(), new[] { "T1", "T2", "T3" }, new[] { "T5", "T7", "T8" });

            var keys = customType.GetKeys(keyCompareData.Keys);
            var compares = customType.GetCompares(keyCompareData.Compares);

            keys.Should().NotBeNull();
            keys.Count.Should().Be(keyCompareData.Keys.Length);
            keys.Keys.All(x => keyCompareData.Keys.Any(y => y == x)).Should().BeTrue();

            compares.Should().NotBeNull();
            compares.Count.Should().Be(keyCompareData.Compares.Length);
            compares.Keys.All(x => keyCompareData.Compares.Any(y => y == x)).Should().BeTrue();
        }

        [Test]
        public void When_Base_Key_Compare_Are_Set_Then_Keys_And_Compares_Data_Are_Set()
        {
            var inhertitanceChain = new InhertitanceChain();

            var keyCompareData = new KeyCompareData(new CustomType(), new[] { "Prop11", "Prop22", "Prop33" }, new[] { "Prop12", "Prop21", "Prop33" });

            var keys = inhertitanceChain.GetKeys(keyCompareData.Keys);
            var compares = inhertitanceChain.GetCompares(keyCompareData.Compares);

            keys.Should().NotBeNull();
            keys.Count.Should().Be(keyCompareData.Keys.Length);
            keys.Keys.All(x => keyCompareData.Keys.Any(y => y == x)).Should().BeTrue();

            compares.Should().NotBeNull();
            compares.Count.Should().Be(keyCompareData.Compares.Length);
            compares.Keys.All(x => keyCompareData.Compares.Any(y => y == x)).Should().BeTrue();
        }
    }
}
