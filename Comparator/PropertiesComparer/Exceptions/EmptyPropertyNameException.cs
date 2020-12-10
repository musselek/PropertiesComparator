namespace PropertiesComparer.Exceptions
{
    class EmptyPropertyNameException : ComparerException
    {
        public override string Code { get; } = "empty_property_name";
        public EmptyPropertyNameException(string name) : base($"{name} has empty property name(s)") { }
    }
}
