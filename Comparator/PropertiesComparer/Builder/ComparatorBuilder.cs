using PropertiesComparer.ChangesReport;
using PropertiesComparer.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace PropertiesComparer.Builder
{
    public sealed class ComparatorBuilder<TCmp>
                                    : IBuilderProcessDataStep<TCmp>
                                    , IBuilderKeysStep<TCmp>
                                    , IBuilderComparesStep<TCmp>
                                    , IBuilderTypeComparator<TCmp>
                                    , IBuilder<TCmp>
                                    , IBuilderPropertyComparator<TCmp>
                                    where TCmp : class
    {
        private readonly BuildParts<TCmp> _buildParts;

        public IBuilderTypeComparator<TCmp> TypeComparator<T>(Expression<Func<T, T, bool>> expression) where T : IEquatable<T>
        {
            _buildParts.ComparatorForType[typeof(T)] = expression;
            return this;
        }
        public IBuilder<TCmp> ToCompare(params Expression<Func<TCmp, object>>[] compares)
        {
            _buildParts.Keys.RequireNotNull();

            FillCollection(compares, _buildParts.Compares);
            return this;
        }
        public IBuilderComparesStep<TCmp> WithKeys(params Expression<Func<TCmp, object>>[] keys)
        {
            _buildParts.Before.RequireNotNull();
            _buildParts.After.RequireNotNull();

            FillCollection(keys, _buildParts.Keys);
            return this;
        }

        public IBuilderKeysStep<TCmp> ProcessData(Expression<Func<IEnumerable<TCmp>, IEnumerable<TCmp>>> before = null, Expression<Func<IEnumerable<TCmp>, IEnumerable<TCmp>>> after = null)
        {
            _buildParts.ProcessData = (before, after);
            return this;
        }

        public IBuilderPropertyComparator<TCmp> PropertyComparator<TElem>(Expression<Func<TCmp, TElem>> func, Expression<Func<TElem, TElem, bool>> expression) where TElem : IEquatable<TElem>
        {
            var propertyName = func.GetMemberInfo().Member.Name;

            _buildParts.ComparatorForProperty[propertyName] = expression;
            return this;
        }

        internal ComparatorBuilder(IEnumerable<TCmp> dataBefore, IEnumerable<TCmp> dataAfter) 
            => _buildParts = new BuildParts<TCmp> { Before = dataBefore, After = dataAfter };

        public IEnumerable<ComparisonResult> Run(bool addDeleteOnly = false)
        {
            _buildParts.Compares.RequireNotNull();
             return Comparator.Run(_buildParts.Before.PrepareData(_buildParts.ProcessData.Before)
                                 , _buildParts.After.PrepareData(_buildParts.ProcessData.After)
                                 , _buildParts.Keys
                                 , _buildParts.Compares
                                 , _buildParts.ComparatorForType
                                 , _buildParts.ComparatorForProperty
                                 , addDeleteOnly);
        }

        private void FillCollection(Expression<Func<TCmp, object>>[] data, IList<LambdaExpression> collection)
        {
            if (data is null) return;
            foreach (var item in data.Where(x => x is not null))
            {
                collection.Add(item);
            }
        }
    }
}
