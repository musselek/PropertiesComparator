using System.Collections.Generic;

namespace PropertiesComparer.UnitTests.Composer.ComposerTestData
{
    public class Complex
    {
        public List<SomeClass> Prop1 { get; init; }
        public List<Example> Prop2 { get; init; }
        public Dictionary<int, int> Prop4 { get; init; }
        public int Prop5 { get; init; }
    }
}
