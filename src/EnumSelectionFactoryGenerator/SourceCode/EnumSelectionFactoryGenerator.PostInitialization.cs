using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using System.Text;

namespace EnumSelectionFactoryGenerator.SourceCode;

internal sealed partial class EnumSelectionFactoryGenerator
{
    private static void PostInitializationExecute(IncrementalGeneratorPostInitializationContext context, string sharedNamespace)
    {
        Emitter emitter = new()
        {
            SharedNamespace = sharedNamespace,
            CancellationToken = context.CancellationToken,
        };

        var codeSources = emitter.GetAttributes();

        foreach (var codeSource in codeSources)
        {
            context.AddSource(codeSource.Name, SourceText.From(codeSource.Source, Encoding.UTF8));
        }
    }
}
