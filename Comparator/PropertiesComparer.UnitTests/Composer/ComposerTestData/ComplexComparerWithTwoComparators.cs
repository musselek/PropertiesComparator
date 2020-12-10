using PropertiesComparer.Builder;
using PropertiesComparer.Composer;

namespace PropertiesComparer.UnitTests.Composer.ComposerTestData
{
    public class ComplexComparerWithTwoComparators : ComparatorComposer<Complex>
    {
        public ComplexComparerWithTwoComparators()
        {
            Compare(x => x.Prop1, (x, y) =>
            {
                return x.DifferenceWith(y)
                    .WithKeys(x => x.P1)
                    .ToCompare(x => x.P2, x => x.P4)
                    .Run();
            });

            Compare(x => x.Prop2, (x, y) =>
            {
                return x.DifferenceWith(y)
                    .WithKeys(x => x.Prop1)
                    .ToCompare(x => x.Prop2)
                    .Run();
            });
        }
    }
}
