
using System.Collections.Generic;

namespace PropertiesComparer.Exceptions
{
    public class DuplicatesKeyCompareException : ComparerException
    {
        public override string Code { get; } = "duplicates_found";
        public DuplicatesKeyCompareException(IEnumerable<string> duplicates) : base($"Duplicates found :[{string.Join(", ", duplicates)}]") { }
    }
}
