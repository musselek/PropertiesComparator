using System.Collections.Generic;

namespace PropertiesComparer.UnitTests.ComparatorTestData
{

    public class ComplexType
    {
        public NoPropertiesData noPropertiesData { get; set; }
    }

    public class NoPublicProperties
    {
        private int Data{ get; set; }
    }

    public class CollectionData
    {
        public List<string> Data { get; set; }
    }

    public class NoPropertiesData
    {
    }
}
