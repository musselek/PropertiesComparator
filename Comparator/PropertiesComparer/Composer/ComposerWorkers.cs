using PropertiesComparer.ChangesReport;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace PropertiesComparer.Composer
{
    internal sealed record ComposerWorker
    {
        internal LambdaExpression CollectionResolver { get; init; }
        internal Func<object, object, IEnumerable<ComparisonResult>> Comparer { get; init; }
    }
}
