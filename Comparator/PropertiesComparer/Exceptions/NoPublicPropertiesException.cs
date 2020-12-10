
namespace PropertiesComparer.Exceptions
{
    public class NoPublicPropertiesException : ComparerException
    {
        public override string Code { get; } = "no_public_properties";
        public NoPublicPropertiesException() : base("No public properties found") { }
    }
}
