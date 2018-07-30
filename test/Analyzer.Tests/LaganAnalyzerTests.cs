using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Lagan.Analyzer.Tests.Helpers;
using Lagan.Analyzer.Tests.Verifiers;

namespace Lagan.Analyzer.Tests
{
    [TestClass]
    public class GivenAnUnAdornedIDisposableMemberVariable : CodeFixVerifier
    {
        [TestMethod]
        public void ItGivesAnAnalyzerWarning()
        {
            var test = @"
                using System.IO;

                namespace ConsoleApplication1
                {
                    class TypeName
                    {
                        private Stream _stream;
                    }
                }";
            var expected = new DiagnosticResult
            {
                Id = LaganAnalyzer.DiagnosticId,
                Message = string.Format(LaganAnalyzer.MessageFormat.ToString(), "_stream"),
                Severity = DiagnosticSeverity.Warning,
                Locations = new[] { new DiagnosticResultLocation("Test0.cs", 8, 40) }
            };

            VerifyCSharpDiagnostic(test, expected);
        }

        protected override CodeFixProvider GetCSharpCodeFixProvider()
        {
            return new LaganCodeFixProvider();
        }

        protected override DiagnosticAnalyzer GetCSharpDiagnosticAnalyzer()
        {
            return new LaganAnalyzer();
        }
    }
}
