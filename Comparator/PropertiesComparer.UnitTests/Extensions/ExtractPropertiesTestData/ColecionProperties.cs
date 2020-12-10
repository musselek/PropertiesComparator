using System.Collections.Concurrent;
using System.Collections.Generic;

namespace PropertiesComparer.UnitTests.Extensions.ExtractPropertiesTestData
{
    public class ColecionProperties
    {
        public int[] ArrayType { get; set; }
        public List<int> ListType { get; set; }
        public HashSet<int> HashSetType { get; set; }
        public Dictionary<int, int> DictionaryType { get; set; }
        public ConcurrentDictionary<int, int> ConcurrentDictionaryType { get; set; }
        public ConcurrentQueue<int> ConcurrentQueueType { get; set; }
        public ConcurrentBag<int> ConcurrentBagType { get; set; }
    }
}
