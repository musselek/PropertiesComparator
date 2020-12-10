using System.Collections.Concurrent;
using System.Collections.Generic;

namespace PropertiesComparer.UnitTests.Extensions.ExtractPropertiesTestData
{
#nullable enable
    public class ImproperNullableTypes
    {
        public Type1? Type1 { get; set; }
        public Type2? Type2 { get; set; }
        public int[]? ArrayType { get; set; }
        public List<int>? ListType { get; set; }
        public HashSet<int>? HashSetType { get; set; }
        public Dictionary<int, int>? DictionaryType { get; set; }
        public ConcurrentDictionary<int, int>? ConcurrentDictionaryType { get; set; }
        public ConcurrentQueue<int>? ConcurrentQueueType { get; set; }
        public ConcurrentBag<int>? ConcurrentBagType { get; set; }
    }
#nullable disable
}
