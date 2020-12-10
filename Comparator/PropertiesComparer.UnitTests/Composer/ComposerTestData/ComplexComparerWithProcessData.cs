using PropertiesComparer.Builder;
using PropertiesComparer.Composer;
using System;
using System.Collections.Generic;

namespace PropertiesComparer.UnitTests.Composer.ComposerTestData
{
    class ComplexComparerWithProcessData : ComparatorComposer<Complex>
    {
        public ComplexComparerWithProcessData()
        {
            Compare(x => x.Prop1, (x, y) =>
            {
                return x.DifferenceWith(y)
                    .ProcessData(after: x => Change(x))
                    .WithKeys(x => x.P1)
                    .ToCompare(x => x.P2, x => x.P4)
                    .TypeComparator<string>((s1, s2) => s1.Equals(s2, StringComparison.InvariantCultureIgnoreCase))
                    .PropertyComparator(x => x.P4, (s1, s2) => s1.Equals(s2, StringComparison.InvariantCulture))
                    .Run();
            });
        }

        public IEnumerable<SomeClass> Change(IEnumerable<SomeClass> someClasses)
        {
            foreach (var someClass in someClasses)
            {
                someClass.P1 += 100;
            }

            return someClasses;
        }
    }
}
