# PropertiesComparator
This is a simple example of step builder, functional approach and generics in c#.
This projects is for comparison two sets of objects which can be understood as BEFORE - AFTER 

This projects allows
* compare two lists of types
* build a comparator composer to compare two sets of data which contains a lists to compare

## How it works
In general comparator allows to compare two list of objects based od their public properties which are:
* Primitive Type
* Enum
* Object which implement the IEquatable interface

User has to to define a Key (one on more properties) and Properties to compare (also one on more)
Rules for Key and ToCompare:
* sets cannot contain the same elements
* sets cannot contain null or empty values

###### Comparator Composer
After parsing the XML(EXCEL, CSV) file the result usually looks as below
 ```csharp
public class XMLParsingResult
{
    public List<Typ1> Prop1 { get; set; }
    public List<Typ2> Prop2 { get; set; }
    public string[] Prop3 { get; set; }
    /*
    * ...
    /*
    public List<int> PropN { get; set; }
}
 ```
In case when user wants to compare a some data from the file is able to compose a comparator and use it. 
It can be understood like a recipe for a comparison for particular user or group. The comparator is not run immediately after creation, but only when user runs *ComparisonResults*
 
## Return value
The return report can be prepared in two modes, depends on the switch in Run method
* Only Add/Delete 
* All changes - default one
If user wants only to see which elements have changed - run Run method with true (addDeleteOnly)
If user wants to see all changes - If user wants to see all changes - run Run method without any param

As a result is a Comparison Result.
Comparison results, can be read as result for a particular key data
- Key - Dictionary, name of param and value of the single key element
- BeforeIndexes - list of items in "before" set where the key combination exists
- AfterIndexes - list of items in "after" set where the key combination exists
- Change - list of changes among all before and after items. Contains indexes (before, after) and dictionary property name/ value before, value after
- CompareState - result of the comparision
    - Add - the item only exists in After set
    - Delete - the item only exists in Before set
    - KeyExist - the key exists in both sets (Before, After)
    - Changed - something is changed 
    - Equal -the data for the key are the same in both sets

## Exceptions
* **EmptyPropertyNameException** - in case when in keys/compares array contains null or empty value
* **DuplicatesKeyCompareException** - in case when the same property existsi in both keys and compares array
* **NoPublicPropertiesException** - in case when the object to check does not have any public properties
* **PropertyNameNotExistException** - in case when the proerty name from keys/compares array not extist on the object properties list
* **NoDataToCompareException** - in case when the collection to compare is null or empty 

## Available methods
* **ProcessData** - if user needs do something with data before compare process, here cas set the methods to prcess before and after data
* **WithKeys** - here user defines the key for comparator
* **ToCompare** - here user defines the properties which should be taken to the comparison
* **TypeComparator** - here user is able to define the comparator for the type 
* **PropertyComparator** - here user is able to define the comparator for the property
* **Run** - to run the process, *Run(true)* means, user wants to see only add, delete data

## Comparison hierarchy
1. Comparer for property
2. Comparer for type
3. IsEqual method

in case if item is null then comparison looks *item1 == item2*

## Example
###### Comparator Composer
They are a list of data from xml like in example below
```csharp
class Example
{
    public string Prop1 { get; set; }
    public int Prop2 { get; set; }
    public List<int> Prop3 { get; set; }
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
        //here user is able to define its own comparator or can build the defaut one 
        
        Compare(x => x.Prop1, (x,y) => 
            x.DifferenceWith(y)
             .ProcessData(before: x => DataChanger(x, 111))
             .WithKeys(x => x.P1)
             .ToCompare(x => x.P2, x => x.P4)
             .TypeComparator<string>((s1, s2) => s1.Equals(s2, StringComparison.InvariantCultureIgnoreCase))
             .PropertyComparator(x => x.P4, (s1, s2) => s1.Equals(s2, StringComparison.InvariantCulture))
             .Run()
             );

        Compare(x => x.Prop2, (x,y) =>
            x.DifferenceWith(y)
            .WithKeys(x => x.Prop1)
            .ToCompare(x => x.Prop2)
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

public void Test()
{
    var c1 = new Complex {
                Prop1 = new List<SomeClass> { new SomeClass { P1 = 1, P2 = "A", P3 = 1.1, P4 = "b" } } ,
                Prop2 = new List<Example> { new Example { Prop1= "a1", Prop2 = 11, Prop4 = new Insurance { Code = "c1" , Name = "n1" } } }
                };
    var c2 = new Complex { 
                Prop1 = new List<SomeClass> { new SomeClass { P1 = 1, P2 = "a", P3 = 1.1, P4 = "B" } },
                Prop2 = new List<Example> { new Example { Prop1 = "a1", Prop2 = 12, Prop4 = new Insurance { Code = "c2", Name = "n2" } } }
            };

    var complexComparer = new ComplexComparer();

    foreach (var cr in complexComparer.ComparisonResults(before, after))
    {
        //do sth with report
    }
}

```
###### Comparator 
In the object below is not defined any ID or GUID which could be used as a key. In this case user has to define a key and data to compare to check what has changed.
If user wants check if someone changed the insurer. Define key - for exaple Name and LastName, and as compares the Insurance.

```csharp
public class Insurance : IEquatable<Insurance>
{
    public string Name { get; set; }
    public string Code { get; set; }
    public bool Equals(Insurance other)
    {
        return Name.Equals(other.Name) && Code.Equals(other.Code);
    }
}

public class Person
{
    public string Name {get; set;}
    public string LastName {get; set;}
    public int Age {get; set;}
    public string City {get; set;}
    public Insurance Insurance {get; set;}
}
public class Data
{
    public void Test()
    {
        var before = new List<Person>
        {
            new Person() { Name = "Jan", LastName = "Kowalski", Age = 32, City = "Kluczbork" , Insurance = new Insurance {Name = "Ins1", Code ="Code 1"}},
        };

        var after = new List<Person>
        {
            new Person() { Name = "Jan", LastName = "Kowalski", Age = 32, City = "Kluczbork" , Insurance = new Insurance {Name = "Ins2", Code ="Code 2"}},
        };
        
        var keys = new[] { "Name", "LastName" };
        var compares = new[] { "Age", "Insurance" };
        
        //only add delete data
        var addDeleteReport = before.DifferenceWith(after).WithKeys(x=>x.Name, x=>x.LastName).ToCompare(x=>x.Age, x=>x.City).Run(true);
        
        //all changes
        var allDataReport = before.DifferenceWith(after).WithKeys(x=>x.Name, x=>x.LastName).ToCompare(x=>x.Age, x=>x.City).Run();
    }
}

```      
