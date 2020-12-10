using PropertiesComparer.ChangesReport;
using PropertiesComparer.Exceptions;
using PropertiesComparer.Extensions;
using PropertiesComparer.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace PropertiesComparer
{
    internal sealed class Comparator
    {
        private readonly PropertyInfo[] _propertyInfos;
        private readonly (string[] keys, string[] compares) _keysCompares;
        private readonly Dictionary<Type, LambdaExpression> _comparatorForType;
        private readonly Dictionary<string, LambdaExpression> _comparatorForProperty;

        private Comparator(Type type, string[] keys, string[] compares, Dictionary<Type, LambdaExpression> comparatorForType, Dictionary<string, LambdaExpression> comparatorForProperty)
        {
            static IEnumerable<string> UnknownProperties(PropertyInfo[] propertyInfos, string[] data) => data.Where(x => !propertyInfos.Select(prop => prop.Name).Contains(x));

            if (!keys.ArrayHasProperData()) throw new EmptyPropertyNameException("Keys");
            if (!compares.ArrayHasProperData()) throw new EmptyPropertyNameException("Compares");

            var duplicates = keys.Where(key => compares.Contains(key));
            if (duplicates.Any()) throw new DuplicatesKeyCompareException(duplicates);

            _propertyInfos = type.ExtractProperties();

            if (!_propertyInfos.Any()) throw new NoPublicPropertiesException();

            var unknownPropertiesKey = UnknownProperties(_propertyInfos, keys);
            if (unknownPropertiesKey.Any()) throw new PropertyNameNotExistException("Keys", unknownPropertiesKey);

            var unknownPropertiesCompare = UnknownProperties(_propertyInfos, compares);
            if (unknownPropertiesCompare.Any()) throw new PropertyNameNotExistException("Compares", unknownPropertiesCompare);

            _comparatorForType = comparatorForType ?? new Dictionary<Type, LambdaExpression>();
            _comparatorForProperty = comparatorForProperty ?? new Dictionary<string, LambdaExpression>();
            _keysCompares = (keys, compares);
        }

        internal static IEnumerable<ComparisonResult> Run<T>
            ( IEnumerable<T> before
            , IEnumerable<T> after
            , IEnumerable<LambdaExpression> keys
            , IEnumerable<LambdaExpression> compares
            , Dictionary<Type, LambdaExpression> comparatorForType
            , Dictionary<string, LambdaExpression> comparatorForProperty
            , bool addDeleteOnly = false)
            where T : class
        {
            if (!before.DataSetIsNotEmpty()) throw new NoDataToCompareException("Before collection");
            if (!after.DataSetIsNotEmpty()) throw new NoDataToCompareException("After collection");

            var keysData = keys.ParamsResolver();
            var comparesData = compares.ParamsResolver();
            var comparator = new Comparator(typeof(T), keysData, comparesData, comparatorForType, comparatorForProperty);
            var keyList = comparator.CompleteKeyList(before, after);
            return addDeleteOnly 
                ? keyList.Where(x => x.CompareState == CompareState.Add || x.CompareState == CompareState.Delete)
                : comparator.CollecDifferences(before.ToList(), after.ToList(), keyList);
        }
       
        private IEnumerable<ComparisonResult> CompleteKeyList<T>(IEnumerable<T> before, IEnumerable<T> after)
        {
            var keyList = new List<ComparisonResult>();

            CollectData(keyList
                , before
                , (key, idx) => new ComparisonResult { Key = key, BeforeIndexes = new List<int> { idx }, CompareState = CompareState.Delete }
                , (item, idx) => item.BeforeIndexes.Add(idx));

            CollectData(keyList
                , after
                , (key, idx) => new ComparisonResult { Key = key, AfterIndexes = new List<int> { idx } , CompareState = CompareState.Add }
                , (item, idx) => { item.AfterIndexes.Add(idx); item.CompareState = CompareState.KeyExist; });

            return keyList;
        }

        private void CollectData<T>(List<ComparisonResult> keyList, IEnumerable<T> collection, Func<Dictionary<string, ItemValue>, int, ComparisonResult> newKeyList, Action<ComparisonResult, int> addIdx)
        {
            foreach (var collectionItem in collection.Select((value, idx) => new { idx, value }))
            {
                var key = collectionItem.value.GetKeys(_keysCompares.keys);
                var item = keyList.Where(keyItem => KeyMatched(keyItem.Key, key)).FirstOrDefault();
                if (item is null) { keyList.Add(newKeyList(key, collectionItem.idx)); }
                else { addIdx(item, collectionItem.idx); }
            }
        }

        private IEnumerable<ComparisonResult> CollecDifferences<T>(List<T> before, List<T> after, IEnumerable<ComparisonResult> addDeleteDiffResult)
        {
            foreach(var item in addDeleteDiffResult.Where(x=>x.CompareState == CompareState.KeyExist))
            {
                foreach (var beforeData in item.BeforeIndexes.Select( idx => new { value = before[idx].GetCompares(_keysCompares.compares), idx=idx }))
                {
                    foreach (var afterData in item.AfterIndexes.Select(idx => new { value = after[idx].GetCompares(_keysCompares.compares), idx = idx }))
                    {
                        var beforeAfterChanges = beforeData.value
                            .Where(beforeItem => !afterData.value[beforeItem.Key].IsEqual(beforeItem.Value, _comparatorForProperty.ComparatorForPropoperty(beforeItem.Key), _comparatorForType.ComparatorForType(beforeItem.Value)))
                            .ToDictionary(beforeItem => beforeItem.Key, beforeItem => new BeforeAfterChange { Before = beforeItem.Value, After = afterData.value[beforeItem.Key]});

                        if (beforeAfterChanges.Count > 0)
                        {
                            item.Changes.Add(new Change { IndexBefore = beforeData.idx, IndexAfter = afterData.idx, BeforeAfterChanges = beforeAfterChanges });
                        }
                    }
                }
                item.CompareState = item.Changes.Any() ? CompareState.Changed : CompareState.Equal;
            }

            return addDeleteDiffResult;
        }

        private bool KeyMatched(Dictionary<string, ItemValue> key1, Dictionary<string, ItemValue> key2)
            => key1.All(x => key2[x.Key].IsEqual(x.Value, _comparatorForProperty.ComparatorForPropoperty(x.Key), _comparatorForType.ComparatorForType(x.Value)));
    }
}
