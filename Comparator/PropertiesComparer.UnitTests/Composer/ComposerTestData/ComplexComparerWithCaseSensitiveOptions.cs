using PropertiesComparer.Composer;
using System;
using PropertiesComparer.Builder;


namespace PropertiesComparer.UnitTests.Composer.ComposerTestData
{
    class ComplexComparerWithCaseSensitiveOptions : ComparatorComposer<Complex>
    {
        public ComplexComparerWithCaseSensitiveOptions()
        {
            Compare(x => x.Prop1, (x, y) =>
            {
                return x.DifferenceWith(y).WithKeys(x => x.P1)
                                .ToCompare(x => x.P2, x => x.P4)
                                .TypeComparator<string>((s1, s2) => s1.Equals(s2, StringComparison.InvariantCultureIgnoreCase))
                                .PropertyComparator(x => x.P4, (s1, s2) => s1.Equals(s2, StringComparison.InvariantCulture))
                                .Run();
            });
        }
    }
}
