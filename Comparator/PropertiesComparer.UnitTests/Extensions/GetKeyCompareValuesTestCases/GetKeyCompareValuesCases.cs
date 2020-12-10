using PropertiesComparer.UnitTests.Extensions.GetKeyCompareValuesTestData;

namespace PropertiesComparer.UnitTests.Extensions.GetKeyCompareValuesTestCases
{
    internal sealed class GetKeyCompareValuesCases
    {
        public static string[][] NullEmptyArrayOrEmptyValues =
        {
            (string[])null,
            new string[] { },
            new string[] { null },
            new string[] { string.Empty },
            new string[] {" "},
            new string[] { string.Empty, " ", null }
        };
    }
}
