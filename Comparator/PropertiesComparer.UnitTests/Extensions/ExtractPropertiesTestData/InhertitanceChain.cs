using System;

namespace PropertiesComparer.UnitTests.Extensions.ExtractPropertiesTestData
{
    public class Fist
    {
        public string Prop1 { get; set; }
    }
    public class Second : Fist
    {
        public Guid Prop2 { get; set; }
    }
    class InhertitanceChain : Second
    {
        public int Prop3 { get; set; }
    }
}
