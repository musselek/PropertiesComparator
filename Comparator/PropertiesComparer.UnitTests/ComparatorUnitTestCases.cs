using PropertiesComparer.Builder;
using PropertiesComparer.UnitTests.ComparatorTestData;
using PropertiesComparer.UnitTests.Extensions;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace PropertiesComparer.UnitTests
{
    internal sealed class ComparatorUnitTestCases
    {
        public static ComparatorKeyCompareArgs[] WrongConstructorArgs =
       {
            new ComparatorKeyCompareArgs(null, null),
            new ComparatorKeyCompareArgs(new List<LambdaExpression>() , null),
            new ComparatorKeyCompareArgs(new List<LambdaExpression>(){ null }, null),

            new ComparatorKeyCompareArgs(new List<LambdaExpression>(), null),
            new ComparatorKeyCompareArgs(new List<LambdaExpression>(){ null }, null)
           
        };
    }
}
