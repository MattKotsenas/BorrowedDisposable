using Microsoft.CodeAnalysis;

namespace Lagan.Analyzer
{
    public interface IDiagnostic
    {
        // You can change these strings in the Resources.resx file. If you do not want your analyzer to be localize-able, you can use regular strings for Title and MessageFormat.
        // See https://github.com/dotnet/roslyn/blob/master/docs/analyzers/Localizing%20Analyzers.md for more on localization
        LocalizableString Title { get; }
        LocalizableString MessageFormat { get; }
        LocalizableString Description { get; }
        DiagnosticDescriptor Rule { get; }
    }

    /// <summary>
    /// This diagnostic is for instances of <see cref="IDisposable"/> without annotation
    /// </summary>
    public class MissingAnnotationDiagnostic : IDiagnostic
    {
        public MissingAnnotationDiagnostic()
        {
            Title = new LocalizableResourceString(nameof(Resources.MissingAnnotationTitle), Resources.ResourceManager, typeof(Resources));
            MessageFormat = new LocalizableResourceString(nameof(Resources.MissingAnnotationMessageFormat), Resources.ResourceManager, typeof(Resources));
            Description = new LocalizableResourceString(nameof(Resources.MissingAnnotationDescription), Resources.ResourceManager, typeof(Resources));
            Rule = new DiagnosticDescriptor(LaganAnalyzer.DiagnosticId, Title, MessageFormat, LaganAnalyzer.Category, DiagnosticSeverity.Warning, isEnabledByDefault: true, description: Description);
        }

        public LocalizableString Title { get; }

        public LocalizableString MessageFormat { get; }

        public LocalizableString Description { get; }

        public DiagnosticDescriptor Rule { get; }
    }

    /// <summary>
    /// This diagnostics is for when a user adds an annotation to something that does not implement <see cref="IDisposable"/>
    /// </summary>
    public class UnnecessaryAnnotationDiagnostic : IDiagnostic
    {
        public UnnecessaryAnnotationDiagnostic()
        {
            Title = new LocalizableResourceString(nameof(Resources.UnnecessaryAnnotationTitle), Resources.ResourceManager, typeof(Resources));
            MessageFormat = new LocalizableResourceString(nameof(Resources.UnnecessaryAnnotationMessageFormat), Resources.ResourceManager, typeof(Resources));
            Description = new LocalizableResourceString(nameof(Resources.UnnecessaryAnnotationDescription), Resources.ResourceManager, typeof(Resources));
            Rule = new DiagnosticDescriptor(LaganAnalyzer.DiagnosticId, Title, MessageFormat, LaganAnalyzer.Category, DiagnosticSeverity.Warning, isEnabledByDefault: true, description: Description);
        }

        public LocalizableString Title { get; }

        public LocalizableString MessageFormat { get; }

        public LocalizableString Description { get; }

        public DiagnosticDescriptor Rule { get; }
    }
}
