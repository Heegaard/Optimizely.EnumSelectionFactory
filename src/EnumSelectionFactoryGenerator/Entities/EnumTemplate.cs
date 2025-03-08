namespace EnumSelectionFactoryGenerator.Entities;

public readonly struct EnumTemplate
{
    public readonly string FullName;
    public readonly string Name;
    public readonly List<EnumMember> Values;

    public EnumTemplate(string fullName, string name, List<EnumMember> values)
    {
        FullName = fullName;
        Name = name;
        Values = values;
    }
}

public readonly struct EnumMember
{
    public readonly string? Name;
    public readonly string? ShortName;
    public readonly string Value;

    public EnumMember(string? name, string? shortName, string value)
    {
        Name = name;
        ShortName = shortName;
        Value = value;
    }
}
