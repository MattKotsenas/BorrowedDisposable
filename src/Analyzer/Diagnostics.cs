using System;
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
    /// This diagnostic is for instances of <see cref="IDisposable"/> with annotation
    /// </summary>
    public class MissingLifetimeDiagnostic : IDiagnostic
    {
        public MissingLifetimeDiagnostic()
        {
            Title = new LocalizableResourceString(nameof(Resources.MissingLifetimeTitle), Resources.ResourceManager, typeof(Resources));
            MessageFormat = new LocalizableResourceString(nameof(Resources.MissingLifetimeMessageFormat), Resources.ResourceManager, typeof(Resources));
            Description = new LocalizableResourceString(nameof(Resources.MissingLifetimeDescription), Resources.ResourceManager, typeof(Resources));
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
    public class UnnecessaryLifetimeDiagnostic : IDiagnostic
    {
        public UnnecessaryLifetimeDiagnostic()
        {
            Title = new LocalizableResourceString(nameof(Resources.UnnecessaryLifetimeTitle), Resources.ResourceManager, typeof(Resources));
            MessageFormat = new LocalizableResourceString(nameof(Resources.UnnecessaryLifetimeMessageFormat), Resources.ResourceManager, typeof(Resources));
            Description = new LocalizableResourceString(nameof(Resources.UnnecessaryLifetimeDescription), Resources.ResourceManager, typeof(Resources));
            Rule = new DiagnosticDescriptor(LaganAnalyzer.DiagnosticId, Title, MessageFormat, LaganAnalyzer.Category, DiagnosticSeverity.Warning, isEnabledByDefault: true, description: Description);
        }

        public LocalizableString Title { get; }

        public LocalizableString MessageFormat { get; }

        public LocalizableString Description { get; }

        public DiagnosticDescriptor Rule { get; }
    }
}
