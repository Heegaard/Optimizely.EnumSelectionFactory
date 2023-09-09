namespace EnumSelectionFactoryGenerator.SourceCode;

internal sealed partial class Emitter
{
    internal IEnumerable<CodeSource> GetAttributes()
    {
        yield return new($"EnumExtensionAttribute.g.cs", CreateEnumExtensionAttribute());

        string CreateEnumExtensionAttribute() =>
            $$"""
                namespace {{SharedNamespace}}.Attributes;

                [AttributeUsage(AttributeTargets.Enum, Inherited = true, AllowMultiple = false)]
                public class EnumExtensionAttribute : Attribute
                {

                }
                """;
    }

}
