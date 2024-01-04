using EnumSelectionFactoryGenerator.CodeBuilders;
using EnumSelectionFactoryGenerator.Entities;

namespace EnumSelectionFactoryGenerator.SourceCode;

internal sealed partial class Emitter
{
    internal IEnumerable<CodeSource> GetEnumExtensions(List<EnumTemplate> enums, string sharedNamespace)
    {
        foreach (var enumTemplate in enums)
        {
            yield return new($"{enumTemplate.Name}Extension.g.cs", CreateEnumExtension());

            string CreateEnumExtension() =>
                CSharpCodeBuilder.Create()
                .Namespace($"{SharedNamespace}.Enums")
                .Class($"public static partial class {enumTemplate.Name}Extension")
                .Tab()
                .NewLine()
                .Method($"public static string ToStringFast(this {enumTemplate.FullName} value)", methodBuilder => methodBuilder
                    .Tab()
                    .Line("return value switch")
                    .Line("{")
                    .Tab()
                    .Foreach(enumTemplate.Values, (b, c) => b.Line($"{enumTemplate.FullName}.{c.Value} => nameof({enumTemplate.FullName}.{c.Value}),"))
                    .Line("_ => value.ToString()")
                    .Tab(-1)
                    .Line("};")
                    )
                .NewLine()
                .Method($"public static {enumTemplate.FullName}[] GetValues()", methodBuilder => methodBuilder
                    .Tab()
                    .Line("return new[]")
                    .Line("{")
                    .Tab()
                    .Foreach(enumTemplate.Values, (b, c) => b.Line($"{enumTemplate.FullName}.{c.Value},"))
                    .Tab(-1)
                    .Line("};")
                    )
                .NewLine()
                .Method($"public static string ToDisplayName(this {enumTemplate.FullName} value)", methodBuilder => methodBuilder
                    .Tab()
                    .Line("return value switch")
                    .Line("{")
                    .Tab()
                    .Foreach(enumTemplate.Values, (b, c) => b.Line($"{enumTemplate.FullName}.{c.Value} => \"{c.Name}\","))
                    .Line("_ => value.ToString()")
                    .Tab(-1)
                    .Line("};")
                    )
                .Build();
        }

        yield return new($"SharedEnumExtension.g.cs", CreateSharedEnumExtension());

        string CreateSharedEnumExtension() =>
                CSharpCodeBuilder.Create()
                .Namespace($"{SharedNamespace}.Enums")
                .Class($"public static partial class SharedEnumExtension")
                .Tab()
                .NewLine()
                .Method($"public static string ToStringFast(this Enum value)", methodBuilder => methodBuilder
                    .Tab()
                    .Line("return value switch")
                    .Line("{")
                    .Tab()
                    .Foreach(enums, (b, c) => b.Line($"{c.FullName} propertyEnum => propertyEnum.ToStringFast(),"))
                    .Line("_ => value.ToString()")
                    .Tab(-1)
                    .Line("};")
                    )
                .NewLine()
                .Method($"public static Enum[] GetValues(this Enum value)", methodBuilder => methodBuilder
                    .Tab()
                    .Line("return value switch")
                    .Line("{")
                    .Tab()
                    .Foreach(enums, (b, c) => b.Line($"{c.FullName} propertyEnum => propertyEnum.GetValues(),"))
                    .Line("_ => new Enum[0]")
                    .Tab(-1)
                    .Line("};")
                    )
                .NewLine()
                .Method($"public static string ToDisplayName(this Enum value)", methodBuilder => methodBuilder
                    .Tab()
                    .Line("return value switch")
                    .Line("{")
                    .Tab()
                    .Foreach(enums, (b, c) => b.Line($"{c.FullName} propertyEnum => propertyEnum.ToDisplayName(),"))
                    .Line("_ => value.ToString()")
                    .Tab(-1)
                    .Line("};")
                    )
                .Build();
    }

    internal IEnumerable<CodeSource> GetTest(string sharedNamespace)
    {
        yield return new($"Test.g.cs", Test());

        string Test() =>
        $$"""
                namespace {{SharedNamespace}}.Test;

                public class TestExtension
                {

                }
                """;


    }
}