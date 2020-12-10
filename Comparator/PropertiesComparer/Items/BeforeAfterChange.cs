namespace PropertiesComparer.Items
{
    public sealed record BeforeAfterChange
    {
        public ItemValue Before { get; init; }
        public ItemValue After { get; init; }
    }
}
