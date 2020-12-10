using System;
using System.Linq.Expressions;

namespace PropertiesComparer.Builder
{
    public interface IBuilderKeysStep<TCmp> where TCmp : class
    {
        IBuilderComparesStep<TCmp> WithKeys(params Expression<Func<TCmp, object>>[] keys);
    }
}
