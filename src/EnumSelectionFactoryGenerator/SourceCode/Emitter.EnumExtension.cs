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
                    .Foreach(enumTemplate.Values, (b, c) => b.Line($"{enumTemplate.FullName}.{c} => nameof({enumTemplate.FullName}.{c}),"))
                    .Line("_ => value.ToString()")
                    .Line("};"))
                    .NewLine()
                .Method($"public static {enumTemplate.FullName}[] GetValues()", methodBuilder => methodBuilder
                .Tab()
                .Line("return new[]")
                .Line("{")
                .Tab()
                .Foreach(enumTemplate.Values, (b, c) => b.Line($"{enumTemplate.FullName}.{c},"))
                .Tab(-1)
                .Line("};")
                )
                .Build();
        }
    }

    //public static Olympus.Shared.Content.Enums.SiteEnum[] GetValues()
    //{
    //    return new[]
    //    {
    //            Olympus.Shared.Content.Enums.SiteEnum.Unknown,
    //            Olympus.Shared.Content.Enums.SiteEnum.Poseidon,
    //        };
    //}

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