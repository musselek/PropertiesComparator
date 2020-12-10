using FluentAssertions;
using NUnit.Framework;
using PropertiesComparer.Extensions;
using System.Collections.Generic;
using System;
using System.Linq.Expressions;

namespace PropertiesComparer.UnitTests.Extensions
{
    public class IsEqualUnitTests
    {
        private const decimal V = 1.0M;

        [Test]
        public void When_Comparator_Options_Are_Null_Or_Not_Set_Then_Use_Default_One()
        {
            "a".ToItemValue().IsEqual("a".ToItemValue(), null, null).Should().BeTrue();
            "A".ToItemValue().IsEqual("a".ToItemValue(), null, null).Should().BeFalse();
        }

        [TestCase("", "")]
        [TestCase(" ", " ")]
        [TestCase(null, null)]
        public void When_Strings_Are_Null_or_Empty_Then_Are_Equal(string string1, string string2)
        {
            string1.ToItemValue().IsEqual(string2.ToItemValue(), null, null).Should().BeTrue();
        }

        [TestCase("Jan", "jan")]
        [TestCase("JAN", "jAN")]
        public void When_Strings_Are_The_Same_Ignoring_Case_Then_Are_Equal(string string1, string string2)
        {
            Expression<Func<string, string, bool>> func = (a, b) => a.Equals(b, StringComparison.InvariantCultureIgnoreCase);

            string1.ToItemValue().IsEqual(string2.ToItemValue(), null, func).Should().BeTrue();
        }

        [TestCase("Jan", "jan")]
        [TestCase("JAN", "jAN")]
        public void When_Strings_Are_The_Same_Ignoring_Case_Then_Are__Not_Equal(string string1, string string2)
        {
            string1.ToItemValue().IsEqual(string2.ToItemValue(), null, null).Should().BeFalse();
        }


        [TestCase("Jan", "jan")]
        [TestCase("Jan", "Jan")]
        [TestCase(1, 1)]
        [TestCase(1.1, 1.1)]
        [TestCase('c', 'c')]
        [TestCase(Int64.MaxValue, Int64.MaxValue)]
        [TestCase(Int32.MaxValue, Int32.MaxValue)]
        public void When_Data_Are_The_Same_Then_Are_Equal<T>(T value1, T value2)
        {
            Expression<Func<string, string, bool>> func = (a, b) => a.Equals(b, StringComparison.InvariantCultureIgnoreCase);

            value1.ToItemValue().IsEqual(value2.ToItemValue(), null, value2.GetType() == typeof(string) ? func : null).Should().BeTrue();
        }

        [Test]
        public void When_Decimal_Data_Are_The_Same_Then_Are_Equal()
        {
            decimal value1 = 1.0M;
            decimal value2 = 1.0M;

            value1.ToItemValue().IsEqual(value2.ToItemValue(), null, null).Should().BeTrue();
        }

        [Test]
        public void When_Comparator_For_Type_Is_Set_Then_Use_This_Comparator()
        {
            Expression<Func<int, int, bool>> func = (a, b) => false;

            1.ToItemValue().IsEqual(1.ToItemValue(), null, func).Should().BeFalse();
        }

        [Test]
        public void When_Comparator_For_Property_And_For_Type_Are_Set_Then_Use_Property_Comparator()
        {
            Expression<Func<string, string, bool>> funcProperty = (s1, s2) => s1.Equals(s1, StringComparison.InvariantCultureIgnoreCase);
            Expression<Func<string, string, bool>> funcType = (s1, s2) => s1.Equals(s1, StringComparison.InvariantCulture);

            "A".ToItemValue().IsEqual("a".ToItemValue(), funcProperty, funcType).Should().BeTrue();
        }

    }
}
