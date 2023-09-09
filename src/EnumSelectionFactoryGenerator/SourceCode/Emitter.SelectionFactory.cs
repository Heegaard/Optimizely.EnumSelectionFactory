using EnumSelectionFactoryGenerator.CodeBuilders;
using EnumSelectionFactoryGenerator.Entities;

namespace EnumSelectionFactoryGenerator.SourceCode;

internal sealed partial class Emitter
{
    internal IEnumerable<CodeSource> GetSelectionFactory(List<EnumTemplate> enums, string sharedNamespace)
    {
        foreach (var enumTemplate in enums)
        {
            yield return new($"{enumTemplate.Name}SelectionFactory.g.cs", CreateSelectionFactory());

            string CreateSelectionFactory() =>
                CSharpCodeBuilder.Create()
                .Using("EPiServer.Shell.ObjectEditing")
                .Using($"{sharedNamespace}.Enums")
                .Namespace($"{sharedNamespace}.SelectionFactories")
                .Class($"public class {enumTemplate.Name}SelectionFactory : ISelectionFactory")
                .Tab()
                .NewLine()
                .Method($"public IEnumerable<ISelectItem> GetSelections(ExtendedMetadata metadata)", methodBuilder => methodBuilder
                    .Tab()
                    .Line($"var enumValues = {enumTemplate.Name}Extension.GetValues();")
                    .Line("foreach (var item in enumValues)")
                    .Line("{")
                    .Tab()
                    .Line("yield return new SelectItem { Value = item, Text = item.ToStringFast() };")
                    .Tab(-1)
                    .Line("}")
                    )
                .Build();
        }
    }
}


//using EPiServer.Shell.ObjectEditing;
//using Olympus.Shared.Content.Enums;

//namespace Olympus.Shared.Content.SelectionFactories;

//public class SiteSelectionFactory : ISelectionFactory
//{
//    public IEnumerable<ISelectItem> GetSelections(ExtendedMetadata metadata)
//    {
//        var sites = SiteEnumExtensions.GetValues();

//        foreach (var site in sites)
//        {
//            yield return new SelectItem { Value = site, Text = site.ToStringFast() };
//        }
//    }
//}