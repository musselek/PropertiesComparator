using PropertiesComparer.ChangesReport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace PropertiesComparer.Builder
{
    public static class Extensions
    {
        public static IBuilderProcessDataStep<T> DifferenceWith<T>(this IEnumerable<T> before, IEnumerable<T> after) where T : class
            => new ComparatorBuilder<T>(before, after);

        internal static IEnumerable<T> RequireNotNull<T>(this IEnumerable<T> args)
          => (args is null || !args.Any() || args.Any(x => x is null)) switch
          {
              true => throw new ArgumentNullException(nameof(args)),
              _ => args
          };

        internal static IList<T> RequireNotNull<T>(this IList<T> args)
          => (args is null || !args.Any() || args.Any(x => x is null)) switch
          {
              true => throw new ArgumentNullException(nameof(args)),
              _ => args
          };

        internal static void RequireNotNull<T>(this T item)
        {
            if (item is null) { throw new ArgumentNullException(nameof(item)); }
        }

        internal static IEnumerable<T> PrepareData<T>(this IEnumerable<T> data, LambdaExpression expression)
            => expression is null ? data : expression.Compile().DynamicInvoke(data) as IEnumerable<T>;

        internal static Func<object, object, IEnumerable<ComparisonResult>> ForceNonGeneric<TElement>(this Func<IEnumerable<TElement>, IEnumerable<TElement>, IEnumerable<ComparisonResult>> func)
            => (x, y) => func((IEnumerable<TElement>)x, (IEnumerable<TElement>)y);
    }
}
