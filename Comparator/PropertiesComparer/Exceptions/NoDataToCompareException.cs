namespace PropertiesComparer.Exceptions
{
    public class NoDataToCompareException : ComparerException
    {
        public override string Code { get; } = "no_data_to_compare";
        public NoDataToCompareException(string name) : base($"{name} has no data to compare") { }
    }
}
