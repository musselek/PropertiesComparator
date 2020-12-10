using PropertiesComparer.ChangesReport;
using System.Collections.Generic;

namespace PropertiesComparer.Builder
{
    public interface IRunner
    {
        IEnumerable<ComparisonResult> Run(bool addDeleteOnly = false);
    }
}
