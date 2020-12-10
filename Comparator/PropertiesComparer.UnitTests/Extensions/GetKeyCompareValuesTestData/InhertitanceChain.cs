using System;

namespace PropertiesComparer.UnitTests.Extensions.GetKeyCompareValuesTestData
{
    public class Fist
    {
        public string Prop11 { get; set; }
        public int Prop12 { get; set; }
        public ulong Prop13 { get; set; }
    }
    public class Second : Fist
    {
        public Guid Prop21 { get; set; }
        public bool Prop22 { get; set; }
        public uint Prop23 { get; set; }
    }
    class InhertitanceChain : Second
    {
        public int Prop31 { get; set; }
        public long Prop32 { get; set; }
        public short Prop33 { get; set; }
    }
}
