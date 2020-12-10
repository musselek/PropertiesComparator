using PropertiesComparer.Items;
using System.Collections.Generic;

namespace PropertiesComparer.ChangesReport
{
    public record Change
    {
        public int IndexBefore { get; init; }
        public int IndexAfter { get; init; }
        public Dictionary<string, BeforeAfterChange> BeforeAfterChanges { get; init; }
    }
}
