using EnumSelectionFactoryGenerator.Entities;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using System.Collections.Immutable;
using System.Text;

namespace EnumSelectionFactoryGenerator.SourceCode;

[Generator]
internal sealed partial class EnumSelectionFactoryGenerator : IIncrementalGenerator
{
    static string sharedNamespace = "EnumSelectionFactory";
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {

        // Do a simple filter for enums
        IncrementalValuesProvider<EnumDeclarationSyntax> enumDeclarations = context.SyntaxProvider
            .CreateSyntaxProvider(
                predicate: static (s, _) => IsSyntaxTargetForGeneration(s), // select enums with attributes
                transform: static (ctx, _) => GetSemanticTargetForGeneration(ctx, sharedNamespace)) // sect the enum with the [EnumExtensions] attribute
            .Where(static m => m is not null)!; // filter out attributed enums that we don't care about

        // Combine the selected enums with the `Compilation`
        IncrementalValueProvider<(Compilation, ImmutableArray<EnumDeclarationSyntax>)> compilationAndEnums
            = context.CompilationProvider.Combine(enumDeclarations.Collect());

        context.RegisterPostInitializationOutput(static callback => PostInitializationExecute(callback, sharedNamespace));
        context.RegisterSourceOutput(compilationAndEnums, static (spc, settings) => Execute(settings.Item1, settings.Item2, spc));
    }

    static bool IsSyntaxTargetForGeneration(SyntaxNode node)
    => node is EnumDeclarationSyntax m && m.AttributeLists.Count > 0;

    static EnumDeclarationSyntax GetSemanticTargetForGeneration(GeneratorSyntaxContext context, string sharedNamespace)
    {
        // we know the node is a EnumDeclarationSyntax thanks to IsSyntaxTargetForGeneration
        var enumDeclarationSyntax = (EnumDeclarationSyntax)context.Node;

        // loop through all the attributes on the method
        foreach (AttributeListSyntax attributeListSyntax in enumDeclarationSyntax.AttributeLists)
        {
            foreach (AttributeSyntax attributeSyntax in attributeListSyntax.Attributes)
            {
                if (context.SemanticModel.GetSymbolInfo(attributeSyntax).Symbol is not IMethodSymbol attributeSymbol)
                {
                    // weird, we couldn't get the symbol, ignore it
                    continue;
                }

                INamedTypeSymbol attributeContainingTypeSymbol = attributeSymbol.ContainingType;
                string fullName = attributeContainingTypeSymbol.ToDisplayString();

                // Is the attribute the [EnumExtensions] attribute?
                if (fullName == $"{sharedNamespace}.Attributes.EnumExtensionAttribute")
                {
                    // return the enum
                    return enumDeclarationSyntax;
                }
            }
        }

        // we didn't find the attribute we were looking for
        return null;
    }

    /// <summary>
    /// This is where the heavy work should be
    /// </summary>
    /// <param name="compilation"></param>
    /// <param name="classes"></param>
    /// <param name="sourceProductionContext"></param>
    private static void Execute(Compilation compilation, ImmutableArray<EnumDeclarationSyntax> enums, SourceProductionContext sourceProductionContext)
    {
        Emitter emitter = new()
        {
            SharedNamespace = sharedNamespace,
            CancellationToken = sourceProductionContext.CancellationToken,
        };

        if (enums.IsDefaultOrEmpty)
        {
            // nothing to do yet

            // If there were errors in the EnumDeclarationSyntax, we won't create an
            // EnumToGenerate for it, so make sure we have something to generate
            var codeTest = emitter.GetTest("Et");


            foreach (var codeSource in codeTest)
            {
                sourceProductionContext.AddSource(codeSource.Name, SourceText.From(codeSource.Source, Encoding.UTF8));
            }

            return;
        }

        // I'm not sure if this is actually necessary, but `[LoggerMessage]` does it, so seems like a good idea!
        IEnumerable<EnumDeclarationSyntax> distinctEnums = enums.Distinct();

        // Convert each EnumDeclarationSyntax to an EnumToGenerate
        List<EnumTemplate> enumsToGenerate = GetTypesToGenerate(compilation, distinctEnums, sourceProductionContext.CancellationToken);

        // If there were errors in the EnumDeclarationSyntax, we won't create an
        // EnumToGenerate for it, so make sure we have something to generate
        var codeSources = emitter
            .GetEnumExtensions(enumsToGenerate, sharedNamespace)
            .Concat(emitter.GetSelectionFactory(enumsToGenerate, sharedNamespace));


        foreach (var codeSource in codeSources)
        {
            sourceProductionContext.AddSource(codeSource.Name, SourceText.From(codeSource.Source, Encoding.UTF8));
        }


    }

    static List<EnumTemplate> GetTypesToGenerate(Compilation compilation, IEnumerable<EnumDeclarationSyntax> enums, CancellationToken ct)
    {
        // Create a list to hold our output
        var enumsToGenerate = new List<EnumTemplate>();
        // Get the semantic representation of our marker attribute 
        INamedTypeSymbol? enumAttribute = compilation.GetTypeByMetadataName($"{sharedNamespace}.Attributes.EnumExtensionAttribute");

        if (enumAttribute == null)
        {
            // If this is null, the compilation couldn't find the marker attribute type
            // which suggests there's something very wrong! Bail out..
            return enumsToGenerate;
        }

        foreach (EnumDeclarationSyntax enumDeclarationSyntax in enums)
        {
            // stop if we're asked to
            ct.ThrowIfCancellationRequested();

            // Get the semantic representation of the enum syntax
            SemanticModel semanticModel = compilation.GetSemanticModel(enumDeclarationSyntax.SyntaxTree);
            if (semanticModel.GetDeclaredSymbol(enumDeclarationSyntax) is not INamedTypeSymbol enumSymbol)
            {
                // something went wrong, bail out
                continue;
            }

            // Get the full type name of the enum e.g. Colour, 
            // or OuterClass<T>.Colour if it was nested in a generic type (for example)
            string enumName = enumSymbol.ToString();

            var test = enumSymbol.Name;

            // Get all the members in the enum
            ImmutableArray<ISymbol> enumMembers = enumSymbol.GetMembers();
            var members = new List<string>(enumMembers.Length);

            // Get all the fields from the enum, and add their name to the list
            foreach (ISymbol member in enumMembers)
            {
                if (member is IFieldSymbol field && field.ConstantValue is not null)
                {
                    members.Add(member.Name);
                }
            }

            // Create an EnumToGenerate for use in the generation phase
            enumsToGenerate.Add(new EnumTemplate(enumName, test, members));
        }

        return enumsToGenerate;
    }

}
