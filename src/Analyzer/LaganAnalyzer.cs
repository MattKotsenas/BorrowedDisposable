using System.Collections.Immutable;
using System.Linq;
using System.Reflection;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Lagan.Analyzer
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class LaganAnalyzer : DiagnosticAnalyzer
    {
        public static readonly string DiagnosticId = "LaganAnalyzer";

        // You can change these strings in the Resources.resx file. If you do not want your analyzer to be localize-able, you can use regular strings for Title and MessageFormat.
        // See https://github.com/dotnet/roslyn/blob/master/docs/analyzers/Localizing%20Analyzers.md for more on localization
        public static readonly LocalizableString Title = new LocalizableResourceString(nameof(Resources.AnalyzerTitle), Resources.ResourceManager, typeof(Resources));
        public static readonly LocalizableString MessageFormat = new LocalizableResourceString(nameof(Resources.AnalyzerMessageFormat), Resources.ResourceManager, typeof(Resources));
        public static readonly LocalizableString Description = new LocalizableResourceString(nameof(Resources.AnalyzerDescription), Resources.ResourceManager, typeof(Resources));
        public static readonly string Category = "Design";

        private static readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor(DiagnosticId, Title, MessageFormat, Category, DiagnosticSeverity.Warning, isEnabledByDefault: true, description: Description);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get { return ImmutableArray.Create(Rule); } }

        public override void Initialize(AnalysisContext context)
        {
            context.RegisterSyntaxNodeAction(AnalyzeSyntax, SyntaxKind.Parameter, SyntaxKind.FieldDeclaration, SyntaxKind.VariableDeclaration);
        }

        private static bool ImplementsIDisposable(ITypeSymbol typeSymbol)
        {
            return typeSymbol.AllInterfaces.Any(i => i.ContainingNamespace.Name == "System" && i.Name == "IDisposable");
        }

        private static void AnalyzeSyntax(SyntaxNodeAnalysisContext context)
        {
            if (context.Node is VariableDeclarationSyntax node)
            {
                var typeInfo = context.SemanticModel.GetTypeInfo(node.Type).Type;

                if (ImplementsIDisposable(typeInfo))
                {
                    foreach (var identifier in node.Variables.Select(variable => variable.Identifier))
                    {
                        var diagnostic = Diagnostic.Create(Rule, identifier.GetLocation(), identifier.ValueText);
                        context.ReportDiagnostic(diagnostic);
                    }
                }
            }
        }
    }
}
