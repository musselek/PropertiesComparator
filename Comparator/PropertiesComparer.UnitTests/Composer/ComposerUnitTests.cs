using FluentAssertions;
using NUnit.Framework;
using PropertiesComparer.Exceptions;
using PropertiesComparer.Items;
using PropertiesComparer.UnitTests.Composer.ComposerTestData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace PropertiesComparer.UnitTests.Composer
{
    public class ComposerUnitTests
    {
        [Test]
        public void When_ProcessData_Is_Set_And_Null_Data_Are_Compared_Then_Exception_Is_Thrown()
        {
            var before = new Complex { };
            var after = new Complex { };

            var complexComparer = new ComplexComparerWithProcessData();
            Action act = () => complexComparer.ComparisonResults(before, after).ToList();
            act.Should().Throw<ArgumentNullException>();
        }

        [Test]
        public void When_ProcessData_Is_Set_And_Empty_Data_Are_Compared_Then_Exception_Is_Thrown()
        {
            var before = new Complex
            {
                Prop1 = new List<SomeClass>()
            };
            var after = new Complex
            {
                Prop1 = new List<SomeClass>()
            };

            var complexComparer = new ComplexComparerWithProcessData();
            Action act = () => complexComparer.ComparisonResults(before, after).ToList();
            act.Should().Throw<ArgumentNullException>();
        }

        [Test]
        public void When_Wrong_Property_Is_Set_Then_Exception_Is_Thrown()
        {
            var before = new Complex
            {
                Prop2 = new List<Example> { new Example { Prop1 = "A", Prop2 = 1 } }
            };
            var after = new Complex
            {
                Prop2 = new List<Example> { new Example { Prop1 = "A", Prop2 = 1 } }
            };

            var complexComparer = new ComplexComparerWithWrongType();
            Action act = () => complexComparer.ComparisonResults(before, after).ToList();
            act.Should().Throw<PropertyNameNotExistException>();
        }


        [Test]
        public void When_ProcessData_Is_Set_And_Data_With_Null_Values_Then_Exception_Is_Thrown()
        {
            var before = new Complex
            {
                Prop1 = new List<SomeClass> { null, null }
            };
            var after = new Complex
            {
                Prop1 = new List<SomeClass> { null, null }
            };

            var complexComparer = new ComplexComparerWithProcessData();
            Action act = () => complexComparer.ComparisonResults(before, after).ToList();
            act.Should().Throw<ArgumentNullException>();
        }

        [Test]
        public void When_ProcessData_Is_Not_Set_And_Data_With_Null_Values_Then_Exception_Is_Thrown()
        {
            var before = new Complex
            {
                Prop1 = new List<SomeClass> ()
            };
            var after = new Complex
            {
                Prop1 = new List<SomeClass> ()
            };

            var complexComparer = new ComplexComparerWithCaseSensitiveOptions();
            Action act = () => complexComparer.ComparisonResults(before, after).ToList();
            act.Should().Throw<ArgumentNullException>();
        }


        [Test]
        public void When_ProcessData_Is_Not_Set_And_Data_With_Null_Values_Then_Exception_is_Thrown()
        {
            var before = new Complex
            {
                Prop1 = new List<SomeClass> { null, null }
            };
            var after = new Complex
            {
                Prop1 = new List<SomeClass> { null, null }
            };

            var complexComparer = new ComplexComparerWithCaseSensitiveOptions();
            //var res = complexComparer.ComparisonResults(before, after).ToList();
            //res.Count().Should().Be(1);
            //res[0].Count().Should().Be(1);
            //res[0].ToList()[0].CompareState.Should().Be(CompareState.Equal);

            Action act = () => complexComparer.ComparisonResults(before, after).ToList();
            act.Should().Throw<ArgumentNullException>();
        }

        [Test]
        public void When_CaseSensitive_Is_Enambled_For_Type_And_Is_Disabled_For_Property_Then_Data_Are_Not_Equal()
        {
            var before = new Complex
            {
                Prop1 = new List<SomeClass> { new SomeClass { P1 = 1, P2 = "A", P3 = 1.1, P4 = "b" } },
                Prop2 = new List<Example> { new Example { Prop1 = "A", Prop2 = 1} }
            };
            var after = new Complex
            {
                Prop1 = new List<SomeClass> { new SomeClass { P1 = 1, P2 = "a", P3 = 1.1, P4 = "B" } },
                Prop2 = new List<Example> { new Example { Prop1 = "A", Prop2 = 1 } }
            };

            var complexComparer = new ComplexComparerWithCaseSensitiveOptions();
            var res = complexComparer.ComparisonResults(before, after).ToList();
            res.Count().Should().Be(1);
            res[0].Count().Should().Be(1);
            res[0].ToList()[0].CompareState.Should().Be(CompareState.Changed);
        }

        [Test]
        public void When_Composer_Consists_Two_Collection_Equal_And_Not_Equal_Then_Are_Two_Reports()
        {
            var before = new Complex
            {
                Prop1 = new List<SomeClass> { new SomeClass { P1 = 1, P2 = "A", P3 = 1.1, P4 = "b" } },
                Prop2 = new List<Example> { new Example { Prop1 = "A", Prop2 = 1 } }
            };
            var after = new Complex
            {
                Prop1 = new List<SomeClass> { new SomeClass { P1 = 1, P2 = "a", P3 = 1.1, P4 = "B" } },
                Prop2 = new List<Example> { new Example { Prop1 = "A", Prop2 = 1 } }
            };

            var complexComparer = new ComplexComparerWithTwoComparators();
            var res = complexComparer.ComparisonResults(before, after).ToList();
            res.Count().Should().Be(2);
            res[0].Count().Should().Be(1);
            res[0].ToList()[0].CompareState.Should().Be(CompareState.Changed);
            res[1].Count().Should().Be(1);
            res[1].ToList()[0].CompareState.Should().Be(CompareState.Equal);
        }
    }
}
