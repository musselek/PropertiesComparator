using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace PropertiesComparer.Builder
{
    internal sealed class BuildParts<TCmp>
    {
        internal IEnumerable<TCmp> Before { get; init; }
        internal IEnumerable<TCmp> After { get; init; }
        internal IList<LambdaExpression> Keys { get; init; }
        internal IList<LambdaExpression> Compares { get; init; }
        internal Dictionary<Type, LambdaExpression> ComparatorForType { get; init; }
        internal Dictionary<string, LambdaExpression> ComparatorForProperty { get; init; }
        internal (LambdaExpression Before, LambdaExpression After) ProcessData { get; set; }
        internal BuildParts()
            => (Before, After, Keys, Compares, ComparatorForType, ComparatorForProperty) = (new List<TCmp>(), new List<TCmp>(), new List<LambdaExpression>(), new List<LambdaExpression>(), new Dictionary<Type, LambdaExpression>(), new Dictionary<string, LambdaExpression>());
    }
}
