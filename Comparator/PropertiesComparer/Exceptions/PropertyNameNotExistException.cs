using System.Collections.Generic;

namespace PropertiesComparer.Exceptions
{
    public class PropertyNameNotExistException : ComparerException
    {
        public override string Code { get; } = "property_not_exist";
        public PropertyNameNotExistException(string name, IEnumerable<string> unknowProperties) : base($"{name} has unknows properties [:[{string.Join(", ", unknowProperties)}]") { }
    }
}
