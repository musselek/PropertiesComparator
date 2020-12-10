namespace PropertiesComparer.UnitTests.Extensions.ExtractPropertiesTestData
{
    public class Type1
    {
        public int PropA { get; set; }
    }

    public class Type2
    {
        public int PropB { get; set; }
    }

    public class ComplexPropertiesNonEqutables
    {
        public Type1 Type1 { get; set; }
        public Type2 Type2 { get; set; }
    }
}
