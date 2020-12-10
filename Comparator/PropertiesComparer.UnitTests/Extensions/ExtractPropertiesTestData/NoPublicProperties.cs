namespace PropertiesComparer.UnitTests.Extensions.ExtractPropertiesTestData
{
    public class NoPublicProperties
    {
        private int Prop1 { get; set; }
        protected int Prop2 { get; set; }

        int Prop3 { get; set; }
    }
}
