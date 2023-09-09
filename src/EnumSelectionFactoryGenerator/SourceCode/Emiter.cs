namespace EnumSelectionFactoryGenerator.SourceCode;

internal sealed partial class Emitter
{
    internal required string SharedNamespace { get; init; }

    internal required CancellationToken CancellationToken { get; init; }

}

internal record ContentClass(string Name, string Guid, string Group, string FullyQualifiedName, IReadOnlyList<ContentProperty> ContentProperties);
internal record ContentProperty(string Name, string TypeName, string ConverterType);
internal record CodeSource(string Name, string Source);