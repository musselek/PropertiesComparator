using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace PropertiesComparer.Builder
{
    public interface IBuilderProcessDataStep<TCmp> where TCmp : class
    {
        IBuilderKeysStep<TCmp> ProcessData(Expression<Func<IEnumerable<TCmp>, IEnumerable<TCmp>>> before = null, Expression<Func<IEnumerable<TCmp>, IEnumerable<TCmp>>> after = null);
        IBuilderComparesStep<TCmp> WithKeys(params Expression<Func<TCmp, object>>[] keys);
    }
}
