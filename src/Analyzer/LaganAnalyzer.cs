using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;
using Lagan.Core;
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
        public static readonly string Category = "Design";

        public static readonly IDiagnostic MissingLifetimeDiagnostic = new MissingLifetimeDiagnostic();

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get { return ImmutableArray.Create(MissingLifetimeDiagnostic.Rule); } }

        public override void Initialize(AnalysisContext context)
        {
            context.RegisterSyntaxNodeAction(AnalyzeSyntax, SyntaxKind.Parameter, SyntaxKind.FieldDeclaration, SyntaxKind.VariableDeclaration);
        }

        private static bool ImplementsIDisposable(ITypeSymbol typeSymbol)
        {
            return typeSymbol.AllInterfaces.Any(i => i.ContainingNamespace.Name == "System" && i.Name == "IDisposable");
        }

        private static bool IsOwnedAttribute(ITypeSymbol attribute)
        {
            var ns = typeof(OwnedAttribute).Namespace;
            var name = nameof(OwnedAttribute);

            return attribute.ContainingNamespace.ToDisplayString() == ns && attribute.Name == name;
        }

        private static bool IsBorrowedAttribute(ITypeSymbol attribute)
        {
            var ns = typeof(BorrowedAttribute).Namespace;
            var name = nameof(BorrowedAttribute);

            return attribute.ContainingNamespace.ToDisplayString() == ns && attribute.Name == name;
        }

        private static IReadOnlyCollection<ITypeSymbol> GetAttributeSymbols(SyntaxNode node, SemanticModel model)
        {
            IReadOnlyCollection<AttributeListSyntax> lists;

            // There's no base type / interface to get attributes from, so try casting to the concrete type.
            if (node is FieldDeclarationSyntax field) { lists = field.AttributeLists; }
            else if (node is ParameterSyntax parameter) { lists = parameter.AttributeLists; }
            else { lists = Array.Empty<AttributeListSyntax>(); }

            return lists.SelectMany(attributeLists => attributeLists.Attributes).Select(attribute => model.GetTypeInfo(attribute).Type).ToList();
        }

        private static void AnalyzeSyntax(SyntaxNodeAnalysisContext context)
        {
            if (context.Node is FieldDeclarationSyntax field)
            {
                var attributes = GetAttributeSymbols(field, context.SemanticModel);
                var typeInfo = context.SemanticModel.GetTypeInfo(field.Declaration.Type).Type;

                if (ImplementsIDisposable(typeInfo) && !attributes.Any(IsOwnedAttribute) && !attributes.Any(IsBorrowedAttribute))
                {
                    foreach (var identifier in field.Declaration.Variables.Select(variable => variable.Identifier))
                    {
                        var diagnostic = Diagnostic.Create(MissingLifetimeDiagnostic.Rule, identifier.GetLocation(), identifier.ValueText);
                        context.ReportDiagnostic(diagnostic);
                    }
                }
            }
            else if (context.Node is ParameterSyntax parameter)
            {
                var attributes = GetAttributeSymbols(parameter, context.SemanticModel);
                var typeInfo = context.SemanticModel.GetTypeInfo(parameter.Type).Type;

                if (ImplementsIDisposable(typeInfo) && !attributes.Any(IsOwnedAttribute) && !attributes.Any(IsBorrowedAttribute))
                {
                    var identifier = parameter.Identifier;
                    var diagnostic = Diagnostic.Create(MissingLifetimeDiagnostic.Rule, identifier.GetLocation(), identifier.ValueText);
                    context.ReportDiagnostic(diagnostic);
                }
            }
        }
    }
}
