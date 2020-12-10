using PropertiesComparer.Items;
using System.Collections.Generic;

namespace PropertiesComparer.ChangesReport
{
    public sealed record ComparisonResult
    {
        public Dictionary<string, ItemValue> Key { get; init; }
        public List<int> BeforeIndexes { get; init; }
        public List<int> AfterIndexes { get; init; }
        public List<Change> Changes { get; }
        public CompareState CompareState { get; set; }

        internal ComparisonResult() => (Changes, Key, BeforeIndexes, AfterIndexes) = (new List<Change>(), new Dictionary<string, ItemValue>(), new List<int>(), new List<int>());
    };
}
