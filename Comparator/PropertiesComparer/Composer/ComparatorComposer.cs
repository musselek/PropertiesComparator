using PropertiesComparer.Builder;
using PropertiesComparer.ChangesReport;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace PropertiesComparer.Composer
{
    public abstract class ComparatorComposer<T> where T : class
    {
        private readonly IList<ComposerWorker> _composerWorkers = new List<ComposerWorker>();

        public void Compare<TElement>(Expression<Func<T, IEnumerable<TElement>>> collectionToCompareResolver, Func<IEnumerable<TElement>, IEnumerable<TElement>, IEnumerable<ComparisonResult>> comparerBuilder) where TElement : class
            => _composerWorkers.Add(new ComposerWorker { CollectionResolver = collectionToCompareResolver, Comparer = comparerBuilder.ForceNonGeneric() });

        public IEnumerable<IEnumerable<ComparisonResult>> ComparisonResults(T before, T after)
        {
            before.RequireNotNull();
            after.RequireNotNull();

            foreach (var worker in _composerWorkers)
            {
                worker.CollectionResolver.RequireNotNull();
                worker.Comparer.RequireNotNull();

                var beforeCollection = worker.CollectionResolver.Compile().DynamicInvoke(before) as IEnumerable<object>;
                var afterCollection = worker.CollectionResolver.Compile().DynamicInvoke(after) as IEnumerable<object>;

                yield return worker.Comparer(beforeCollection, afterCollection);
            }
        }
    }
}
