using FluentAssertions;
using NUnit.Framework;
using PropertiesComparer.Builder;
using PropertiesComparer.Exceptions;
using PropertiesComparer.Items;
using PropertiesComparer.UnitTests.ComparatorTestData;
using PropertiesComparer.UnitTests.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PropertiesComparer.UnitTests
{
    public class ComparatorUnitTest
    {
        #region NullBuilderData

        [Test]
        public void When_Builder_Has_Null_As_Data_Then_Exception_Is_Thrown()
        {
            List<Person> nullList = null;

            Assert.Throws<ArgumentNullException>(() => nullList.DifferenceWith(new List<Person>()).WithKeys().ToCompare().Run());
            Assert.Throws<ArgumentNullException>(() => (new List<Person>()).DifferenceWith(null).WithKeys().ToCompare().Run());
            Assert.Throws<ArgumentNullException>(() => (new List<Person>()).DifferenceWith((new List<Person>())).WithKeys(null).ToCompare().Run());
            Assert.Throws<ArgumentNullException>(() => (new List<Person>()).DifferenceWith(new List<Person>()).WithKeys().ToCompare(null).Run());
        }

        #endregion

        #region Wrong Input Data

        [TestCaseSource(typeof(ComparatorUnitTestCases), nameof(ComparatorUnitTestCases.WrongConstructorArgs))]
        public void When_Key_Compare_Data_Are_Null_Or_Empty_Then_Exception_Is_Thrown(ComparatorKeyCompareArgs comparatorConstructorArgs)
        {
            Action act = () => Comparator.Run(new List<Person>(), new List<Person>(), comparatorConstructorArgs.Keys, comparatorConstructorArgs.Compares, null, null);
            act.Should().Throw<NoDataToCompareException>();
        }

        [Test]
        public void When_Keys_And_Compares_Have_The_Same_Values_Then_Exception_Is_Thrown()
        {
            Action act = () => (new List<Person>() { new Person() }).DifferenceWith(new List<Person>() { new Person() }).WithKeys(x=>x.Age).ToCompare(x => x.Age).Run();
            act.Should().Throw<DuplicatesKeyCompareException>();

            Action act2 = () => (new List<Person>() { new Person() }).DifferenceWith(new List<Person>() { new Person() }).WithKeys(x => x.Age, x=>x.City).ToCompare(x => x.Age).Run();
            act2.Should().Throw<DuplicatesKeyCompareException>();

            Action act3 = () => (new List<Person>() { new Person() }).DifferenceWith(new List<Person>() { new Person() }).WithKeys(x => x.Age).ToCompare(x => x.Age, x => x.City).Run();
            act3.Should().Throw<DuplicatesKeyCompareException>();

        }

        #endregion

        #region Key Is Found

        [Test]
        public void When_Items_Beftore_And_After_Are_The_Same_No_Differences_Exist()
        {
            //Arrange
            var before = new List<Person>
                {
                    new Person() { Name = "Jan", LastName = "Kowalski", Age = 32, City = "Bochnia" },
                    new Person() { Name = "Anna", LastName = "Nowak", Age = 23, City = "Kraków" },
                };

            var after = new List<Person>
                {
                    new Person() { Name = "Anna", LastName = "Nowak", Age = 23, City = "Kraków" },
                    new Person() { Name = "Jan", LastName = "Kowalski", Age = 32, City = "Bochnia" }
                };

            var keys = new[] { "Name", "LastName" };
            var compares = new[] { "Age", "City" };

            //Act
            var report = before.DifferenceWith(after).WithKeys(x => x.Name, x => x.LastName).ToCompare(x => x.Age, x => x.City).Run().ToList();
            
            //Assert
            report.Count.Should().Be(2);
            report[0].CompareState.Should().Be(CompareState.Equal);
            report[0].BeforeIndexes.Count.Should().Be(1);
            report[0].AfterIndexes.Count.Should().Be(1);

            report[1].CompareState.Should().Be(CompareState.Equal);
            report[1].BeforeIndexes.Count.Should().Be(1);
            report[1].AfterIndexes.Count.Should().Be(1);
        }

        [Test]
        public void When_Key_Exists_Mark_Data_As_Absent()
        {
            //Arrange
            var before = new List<Person>
                {
                    new Person() { Name = "Jan", LastName = "Kowalski", Age = 32, City = "Bochnia" },
                };

            var after = new List<Person>
                {
                    new Person() { Name = "Jan", LastName = "Kowalski", Age = 52, City = "Bochnia" },
                    new Person() { Name = "Jan", LastName = "Kowalski", Age = 32, City = "Kraków" }
                };

            var keys = new[] { "Name", "LastName" };
            var compares = new[] { "Age", "City" };

            //Act
            var report = before.DifferenceWith(after).WithKeys(x => x.Name, x => x.LastName).ToCompare(x => x.Age, x => x.City).Run().ToList();

            //Assert
            report.Count.Should().Be(1);
            report[0].CompareState.Should().Be(CompareState.Changed);
            report[0].BeforeIndexes.Count.Should().Be(1);
            report[0].AfterIndexes.Count.Should().Be(2);

            report[0].Changes.Count.Should().Be(2);
            report[0].Changes[0].IndexBefore.Should().Be(0);
            report[0].Changes[0].IndexAfter.Should().Be(0);
            report[0].Changes[0].BeforeAfterChanges.Count.Should().Be(1);
            Assert.IsTrue(report[0].Changes[0].BeforeAfterChanges[compares[0]].Before.Item.Equals(32));
            Assert.IsTrue(report[0].Changes[0].BeforeAfterChanges[compares[0]].After.Item.Equals(52));

            report[0].Changes[1].IndexBefore.Should().Be(0);
            report[0].Changes[1].IndexAfter.Should().Be(1);
            report[0].Changes[1].BeforeAfterChanges.Count.Should().Be(1);
            Assert.IsTrue(report[0].Changes[1].BeforeAfterChanges[compares[1]].Before.Item.Equals("Bochnia"));
            Assert.IsTrue(report[0].Changes[1].BeforeAfterChanges[compares[1]].After.Item.Equals("Kraków"));
        }

        [Test]
        public void When_Key_Exists_Not_In_All_Cases_Mark_Proper_Key_Occurence()
        {
            //Arrange
            var before = new List<Person>
                {
                    new Person() { Name = "Jan", LastName = "Kowalski", Age = 32, City = "Bochnia" },
                };

            var after = new List<Person>
                {
                    new Person() { Name = "Jan", LastName = "Kowalski", Age = 52, City = "Bochnia" },
                    new Person() { Name = "Janusz", LastName = "Kowalski", Age = 32, City = "Kraków" }
                };

            var keys = new[] { "Name", "LastName" };
            var compares = new[] { "Age", "City" };

            //Act
            var report = before.DifferenceWith(after).WithKeys(x => x.Name, x => x.LastName).ToCompare(x => x.Age, x => x.City).Run().ToList();

            //Assert
            report.Count.Should().Be(2);
            report[0].CompareState.Should().Be(CompareState.Changed);
            report[0].BeforeIndexes.Count.Should().Be(1);
            report[0].AfterIndexes.Count.Should().Be(1);
            report[0].Changes.Count.Should().Be(1);
            report[0].Changes[0].BeforeAfterChanges.Count.Should().Be(1);
            Assert.IsTrue(report[0].Changes[0].BeforeAfterChanges[compares[0]].Before.Item.Equals(32));
            Assert.IsTrue(report[0].Changes[0].BeforeAfterChanges[compares[0]].After.Item.Equals(52));

            report[1].CompareState.Should().Be(CompareState.Add);
            report[1].BeforeIndexes.Count.Should().Be(0);
            report[1].AfterIndexes.Count.Should().Be(1);
            report[1].Changes.Count.Should().Be(0);
        }

        #endregion

        #region Different Data

        [Test]
        public void When_Items_Beftore_And_After_Are_Not_The_Same_Keys_Are_Not_Present()
        {
            //Arrange
            var before = new List<Person>
                {
                    new Person() { Name = "Jan", LastName = "Kowalski", Age = 32, City = "Kraków" },
                    new Person() { Name = "Anna", LastName = "Nowak", Age = 23, City = "Bochnia" }
                };

            var after = new List<Person>
                {
                    new Person() { Name = "Anna", LastName = "Kowalski", Age = 23, City = "Bochnia" },
                    new Person() { Name = "Jan", LastName = "Nowak", Age = 32, City = "Kraków" }
                };

            var keys = new[] { "Name", "LastName" };
            var compares = new[] { "Age", "City" };

            //Act
            var report = before.DifferenceWith(after).WithKeys(x => x.Name, x => x.LastName).ToCompare(x => x.Age, x => x.City).Run().ToList();

            //Assert
            report.Count.Should().Be(4);
            report[0].CompareState.Should().Be(CompareState.Delete);
            report[0].BeforeIndexes.Count.Should().Be(1);
            report[0].AfterIndexes.Count.Should().Be(0);

            report[1].CompareState.Should().Be(CompareState.Delete);
            report[1].BeforeIndexes.Count.Should().Be(1);
            report[1].AfterIndexes.Count.Should().Be(0);

            report[2].CompareState.Should().Be(CompareState.Add);
            report[2].BeforeIndexes.Count.Should().Be(0);
            report[2].AfterIndexes.Count.Should().Be(1);

            report[3].CompareState.Should().Be(CompareState.Add);
            report[3].BeforeIndexes.Count.Should().Be(0);
            report[3].AfterIndexes.Count.Should().Be(1);
        }

        #endregion

        #region CaseSensitive Tests

        [Test]
        public void When_Setting_Contains_Ignore_Case_Option_Then_Case_Is_Ignored_On_Comparison()
        {
            //Arrange
            var before = new List<Person>
                {
                    new Person() { Name = "jAn", LastName = "kOwalsKi", Age = 32, City = "Bochnia" },
                };

            var after = new List<Person>
                {
                    new Person() { Name = "Jan", LastName = "Kowalski", Age = 32, City =  "bochnia"},
                };

            var keys = new[] { "Name", "LastName" };
            var compares = new[] { "Age", "City" };

            //Act
            var report = before
                .DifferenceWith(after).WithKeys(x => x.Name, x => x.LastName).ToCompare(x => x.Age, x => x.City)
                .TypeComparator<string>((s1, s2)=>s1.Equals(s2, StringComparison.InvariantCultureIgnoreCase))
                .Run()
                .ToList();

            //Assert
            report.Count.Should().Be(1);
            report[0].CompareState.Should().Be(CompareState.Equal);
        }

        [Test]
        public void When_Setting_Contains_Default_Data_Then_Case_Is_Not_Ignored_On_Comparison()
        {
            //Arrange
            var before = new List<Person>
                {
                    new Person() { Name = "jAn", LastName = "kOwalsKi", Age = 32, City = "Bochnia" },
                };

            var after = new List<Person>
                {
                    new Person() { Name = "Jan", LastName = "Kowalski", Age = 32, City =  "bochnia"},
                };

            var keys = new[] { "Name", "LastName" };
            var compares = new[] { "Age", "City" };

            //Act
            var report = before
                 .DifferenceWith(after).WithKeys(x => x.Name, x => x.LastName).ToCompare(x => x.Age, x => x.City)
                 .TypeComparator<string>((s1, s2) => s1.Equals(s2, StringComparison.InvariantCulture))
                 .Run()
                 .ToList();

            //Assert
            report.Count.Should().Be(2);
            report[0].CompareState.Should().Be(CompareState.Delete);
            report[1].CompareState.Should().Be(CompareState.Add);
        }

        #endregion

        #region AddDelete report

        [Test]
        public void When_Items_Changed_And_User_Wants_AddDelete_Report_Then_Report_Is_Empty()
        {
            //Arrange
            var before = new List<Person>
                {
                    new Person() { Name = "Jan", LastName = "Kowalski", Age = 32, City = "Kraków" },
                    new Person() { Name = "Anna", LastName = "Nowak", Age = 23, City = "Bochnia" }
                };

            var after = new List<Person>
                {
                    new Person() { Name = "Anna", LastName = "Nowak", Age = 23, City = "Bochnia" },
                    new Person() { Name = "Jan", LastName = "Kowalski", Age = 32, City = "Kraków" },
                };

            var keys = new[] { "Name", "LastName" };
            var compares = new[] { "Age", "City" };

            //Act
            var report = before
                  .DifferenceWith(after).WithKeys(x => x.Name, x => x.LastName).ToCompare(x => x.Age, x => x.City)
                  .Run(true)
                  .ToList();

            //Assert
            report.Count.Should().Be(0);
        }

        [Test]
        public void When_Items_Changed_Add_Delete_And_User_Wants_AddDelete_Then_Report_Contains_AddDelete_Data()
        {
            //Arrange
            var before = new List<Person>
                {
                    new Person() { Name = "Jan", LastName = "Kran", Age = 32, City = "Kraków" },
                    new Person() { Name = "Jan", LastName = "Kowalski", Age = 32, City = "Kraków" },
                    new Person() { Name = "Anna", LastName = "Nowak", Age = 23, City = "Bochnia" }
                };

            var after = new List<Person>
                {
                    new Person() { Name = "Anna", LastName = "Nowak", Age = 23, City = "Bochnia" },
                    new Person() { Name = "Jan", LastName = "Kowalski", Age = 32, City = "Kraków" },
                    new Person() { Name = "Andrzej", LastName = "Kran", Age = 32, City = "Kraków" },
                };

            var keys = new[] { "Name", "LastName" };
            var compares = new[] { "Age", "City" };

            //Act
            var report = before
                 .DifferenceWith(after).WithKeys(x => x.Name, x => x.LastName).ToCompare(x => x.Age, x => x.City)
                 .Run(true)
                 .ToList();

            //Assert
            report.Count.Should().Be(2);
            report[0].CompareState.Should().Be(CompareState.Delete);
            report[1].CompareState.Should().Be(CompareState.Add);
        }

        #endregion
    }
 }
