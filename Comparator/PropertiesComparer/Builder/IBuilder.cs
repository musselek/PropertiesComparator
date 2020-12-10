using System;
using System.Linq.Expressions;

namespace PropertiesComparer.Builder
{
    public interface IBuilder<TCmp> : IRunner where TCmp : class
    {
        IBuilderTypeComparator<TCmp> TypeComparator<T>(Expression<Func<T, T, bool>> expression) where T : IEquatable<T>;
        IBuilderPropertyComparator<TCmp> PropertyComparator<TElem>(Expression<Func<TCmp, TElem>> func, Expression<Func<TElem, TElem, bool>> expression) where TElem : IEquatable<TElem>;
    }
}
