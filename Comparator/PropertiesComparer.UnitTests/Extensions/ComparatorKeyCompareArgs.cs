using System.Collections.Generic;
using System.Linq.Expressions;

namespace PropertiesComparer.UnitTests.Extensions
{
    public sealed record ComparatorKeyCompareArgs(IEnumerable<LambdaExpression> Keys, IEnumerable<LambdaExpression> Compares);
}
