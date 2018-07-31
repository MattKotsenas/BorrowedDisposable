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
        public static readonly IDiagnostic UnnecessaryLifetimeDiagnostic = new UnnecessaryLifetimeDiagnostic();

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(MissingLifetimeDiagnostic.Rule, UnnecessaryLifetimeDiagnostic.Rule);

        public override void Initialize(AnalysisContext context)
        {
            context.RegisterSyntaxNodeAction(AnalyzeSyntax, SyntaxKind.Parameter, SyntaxKind.FieldDeclaration, SyntaxKind.VariableDeclaration);
        }

        private static bool ImplementsIDisposable(ITypeSymbol symbol)
        {
            return symbol.AllInterfaces.Any(i => i.ContainingNamespace.Name == "System" && i.Name == "IDisposable");
        }

        private static bool IsOwnedAttribute(ITypeSymbol symbol)
        {
            var ns = typeof(OwnedAttribute).Namespace;
            var name = nameof(OwnedAttribute);

            return symbol.ContainingNamespace.ToDisplayString() == ns && symbol.Name == name;
        }

        private static bool IsBorrowedAttribute(ITypeSymbol symbol)
        {
            var ns = typeof(BorrowedAttribute).Namespace;
            var name = nameof(BorrowedAttribute);

            return symbol.ContainingNamespace.ToDisplayString() == ns && symbol.Name == name;
        }

        private static bool IsOwnedType(ITypeSymbol symbol)
        {
            var ns = typeof(Owned<>).Namespace;
            var name = nameof(Owned<IDisposable>);

            return symbol.ContainingNamespace.ToDisplayString() == ns && symbol.Name == name;
        }

        private static bool IsBorrowedType(ITypeSymbol symbol)
        {
            var ns = typeof(Borrowed<>).Namespace;
            var name = nameof(Borrowed<IDisposable>);

            return symbol.ContainingNamespace.ToDisplayString() == ns && symbol.Name == name;
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

        private static bool HasLifetimeAnnotation(IReadOnlyCollection<ITypeSymbol> attributes, ITypeSymbol symbol)
        {
            return (attributes.Any(IsOwnedAttribute) || attributes.Any(IsBorrowedAttribute) || IsOwnedType(symbol) || IsBorrowedType(symbol));
        }

        private static void AnalyzeSyntax(SyntaxNodeAnalysisContext context)
        {
            if (context.Node is FieldDeclarationSyntax field)
            {
                var attributes = GetAttributeSymbols(field, context.SemanticModel);
                var typeInfo = context.SemanticModel.GetTypeInfo(field.Declaration.Type).Type;

                var hasLifetimeAnnotation = HasLifetimeAnnotation(attributes, typeInfo);

                if (ImplementsIDisposable(typeInfo))
                {
                    if (!hasLifetimeAnnotation)
                    {
                        foreach (var identifier in field.Declaration.Variables.Select(variable => variable.Identifier))
                        {
                            var diagnostic = Diagnostic.Create(MissingLifetimeDiagnostic.Rule, identifier.GetLocation(), identifier.ValueText);
                            context.ReportDiagnostic(diagnostic);
                        }
                    }
                }
                else
                {
                    if (hasLifetimeAnnotation)
                    {
                        foreach (var identifier in field.Declaration.Variables.Select(variable => variable.Identifier))
                        {
                            var diagnostic = Diagnostic.Create(UnnecessaryLifetimeDiagnostic.Rule, identifier.GetLocation(), identifier.ValueText);
                            context.ReportDiagnostic(diagnostic);
                        }
                    }
                }
            }
            else if (context.Node is ParameterSyntax parameter)
            {
                var attributes = GetAttributeSymbols(parameter, context.SemanticModel);
                var typeInfo = context.SemanticModel.GetTypeInfo(parameter.Type).Type;

                var hasLifetimeAnnotation = HasLifetimeAnnotation(attributes, typeInfo);

                if (ImplementsIDisposable(typeInfo))
                {
                    if (!hasLifetimeAnnotation)
                    {
                        var identifier = parameter.Identifier;
                        var diagnostic = Diagnostic.Create(MissingLifetimeDiagnostic.Rule, identifier.GetLocation(), identifier.ValueText);
                        context.ReportDiagnostic(diagnostic);
                    }
                }
                else
                {
                    if (hasLifetimeAnnotation)
                    {
                        var identifier = parameter.Identifier;
                        var diagnostic = Diagnostic.Create(UnnecessaryLifetimeDiagnostic.Rule, identifier.GetLocation(), identifier.ValueText);
                        context.ReportDiagnostic(diagnostic);
                    }
                }
            }
        }
    }
}
