using PropertiesComparer.Builder;
using PropertiesComparer.Composer;

namespace PropertiesComparer.UnitTests.Composer.ComposerTestData
{
    class ComplexComparerWithWrongType : ComparatorComposer<Complex>
    {
        public ComplexComparerWithWrongType()
        {
            Compare(x => x.Prop2, (x, y) =>
            {
                return x.DifferenceWith(y)
                    .WithKeys(x => x.Prop1)
                    .ToCompare(x => x.Prop2, x => x.Prop3)
                    .Run();
            });
        }
    }
}
