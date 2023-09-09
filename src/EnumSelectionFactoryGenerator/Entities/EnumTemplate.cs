namespace EnumSelectionFactoryGenerator.Entities;

public readonly struct EnumTemplate
{
    public readonly string FullName;
    public readonly string Name;
    public readonly List<string> Values;

    public EnumTemplate(string fullName, string name, List<string> values)
    {
        FullName = fullName;
        Name = name;
        Values = values;
    }
}
