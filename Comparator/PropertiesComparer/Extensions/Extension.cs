using PropertiesComparer.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace PropertiesComparer.Extensions
{
    internal static class Extension
    {
        internal static ItemValue ToItemValue<T>(this T item)
            => new ItemValue(item);

        internal static bool ArrayHasProperData(this string[] data)
            => data is not null && data.Any() && !data.Where(x => string.IsNullOrWhiteSpace(x)).Any();

        internal static bool DataSetIsNotEmpty<T>(this IEnumerable<T> data)
            => data is not null && data.Any();

        internal static PropertyInfo[] ExtractProperties(this Type type)
            =>type
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(x=> Nullable.GetUnderlyingType(x.PropertyType).IsProperType() || x.PropertyType.IsProperType())
                .ToArray();

        internal static Dictionary<string, ItemValue> GetKeys<T>(this T item, string[] keys)
           => item.GetValues(keys);

        internal static Dictionary<string, ItemValue> GetCompares<T>(this T item, string[] compares)
            => item.GetValues(compares);

        internal static LambdaExpression ComparatorForType(this Dictionary<Type, LambdaExpression> comparatorForTypes, ItemValue value)
            => comparatorForTypes.TryGetValue(value.Item.GetType(), out LambdaExpression lambda) ? lambda : null;

        internal static LambdaExpression ComparatorForPropoperty(this Dictionary<string, LambdaExpression> comparatorForTypes, string property)
            => !string.IsNullOrWhiteSpace(property) && comparatorForTypes.TryGetValue(property, out LambdaExpression lambda) ? lambda : null;

        internal static bool IsEqual(this ItemValue value, ItemValue toCompare, LambdaExpression comparatorForProperty, LambdaExpression comparatorForType)
            => value.Item is null
                ? value.Item == toCompare.Item
                : comparatorForProperty is not null
                    ? comparatorForProperty.Compile().DynamicInvoke(value.Item, toCompare.Item)
                    : comparatorForType is not null
                        ? comparatorForType.Compile().DynamicInvoke(value.Item, toCompare.Item)
                        : value.Item.Equals(toCompare.Item);

        internal static MemberExpression GetMemberInfo(this LambdaExpression exp)
            => exp is null
                ? null
                : exp.Body.NodeType switch
                {
                    ExpressionType.Convert => ((UnaryExpression)exp.Body).Operand as MemberExpression,
                    ExpressionType.MemberAccess => exp.Body as MemberExpression,
                    _ => null
                };

        internal static string[] ParamsResolver(this IEnumerable<LambdaExpression> expressions)
           => expressions is null
                ? Array.Empty<string>()
                : expressions.Select(x => x.GetMemberInfo()).Where(x => x is not null).Select(x => x.Member.Name).ToArray();

        private static Dictionary<string, ItemValue> GetValues<T>(this T item, string[] data)
            => (item is null || data is null)
                    ? new Dictionary<string, ItemValue>()
                    : item.GetType().GetProperties()
                        .Where(property => property is not null && data.Contains(property.Name))
                        .ToDictionary(property => property.Name, property => property.GetValue(item).ToItemValue());
            
        private static bool IsProperType(this Type type) =>
            type switch
            {
                null => false,
                _ => type.IsPrimitive
                        || type.IsEnum
                        || type.GetInterfaces().Any(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IEquatable<>))
            };
    }
}
