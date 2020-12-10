using System;
using System.Linq.Expressions;

namespace PropertiesComparer.Builder
{
    public interface IBuilderComparesStep<TCmp> where TCmp : class
    {
        IBuilder<TCmp> ToCompare(params Expression<Func<TCmp, object>>[] compares);
    }
}
