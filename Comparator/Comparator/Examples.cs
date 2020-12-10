using PropertiesComparer.Builder;
using PropertiesComparer.Composer;
using System;
using System.Collections.Generic;

namespace Comparator
{
    class Examples
    {
        class Insurance : IEquatable<Insurance>
        {
            public string Name { get; set; }
            public string Code { get; set; }
            public bool Equals(Insurance other)
            {
                return Name.Equals(other.Name) && Code.Equals(other.Code);
            }
        }

        class Example
        {
            public string Prop1 { get; set; }
            public int Prop2 { get; set; }
            public List<SomeClass> Prop3 { get; set; }
            public Insurance Prop4 { get; set; }
        }

        class SomeClass
        {
            public int P1 { get; set; }
            public string P2 { get; set; }
            public double P3 { get; set; }
            public string P4 { get; set; }
        }

        class Complex
        {
            public List<SomeClass> Prop1 { get; init; }
            public List<Example> Prop2 { get; init; }
            public List<Example> Prop3 { get; init; }
            public Dictionary<int, int> Prop4 { get; init; }
            public int Prop5 { get; init; }
        }

        class ComplexComparer : ComparatorComposer<Complex>
        {
            public ComplexComparer()
            {
                Compare(x => x.Prop1, (x,y) => 
                    x.DifferenceWith(y)
                     .ProcessData(before: x => DataChanger(x, 111))
                     .WithKeys(x => x.P1)
                     .ToCompare(x => x.P2, x => x.P3)
                     .TypeComparator<string>((s1, s2) => s1.Equals(s2, StringComparison.InvariantCultureIgnoreCase))
                     .PropertyComparator(x => x.P4, (s1, s2) => s1.Equals(s2, StringComparison.InvariantCulture))
                     .Run()
                     );

                Compare(x => x.Prop2, (x,y) => 
                    x.DifferenceWith(y)
                    .WithKeys(x => x.Prop1)
                    .ToCompare(x => x.Prop2, x=>x.Prop4)
                    .Run()
                    );
            }

            private IEnumerable<SomeClass> DataChanger(IEnumerable<SomeClass> input, int someValue)
            {
                foreach (var x in input)
                {
                    x.P1 += someValue;
                }

                return input;
            }
        }

        static void Main(string[] args)
        {
            var before = new List<Example> { new Example { Prop1 = "A", Prop2 = 1 }, new Example { Prop1 = "B", Prop2 = 2 } };
            var after = new List<Example> { new Example { Prop1 = "A", Prop2 = 2 }, new Example { Prop1 = "B", Prop2 = 1 } };

            var diff2 = before
                .DifferenceWith(after)
               .WithKeys(x => x.Prop1)
               .ToCompare(x => x.Prop2)
               .Run();

            var c1 = new Complex {
                Prop1 = new List<SomeClass> { new SomeClass { P1 = 1, P2 = "A", P3 = 1.1, P4 = "b" } } ,
                Prop2 = new List<Example> { new Example { Prop1= "a1", Prop2 = 11, Prop4 = new Insurance { Code = "c1" , Name = "n1" } } }
                };
            var c2 = new Complex { 
                Prop1 = new List<SomeClass> { new SomeClass { P1 = 1, P2 = "a", P3 = 1.1, P4 = "B" } },
                Prop2 = new List<Example> { new Example { Prop1 = "a1", Prop2 = 12, Prop4 = new Insurance { Code = "c2", Name = "n2" } } }
            };

            var rep = c1.Prop1.DifferenceWith(c2.Prop1).WithKeys(x => x.P1).ToCompare(x => x.P2)
                .TypeComparator<string>((a, b) => a.Equals(b, StringComparison.InvariantCultureIgnoreCase))
                .PropertyComparator(x => x.P2, (x, y) => x.Equals(y, StringComparison.InvariantCulture))
                .Run();

            var complexComparer = new ComplexComparer();


            foreach (var cr in complexComparer.ComparisonResults(c1, c2))
            {
                //do sth with report
            }
        }
    }
}

