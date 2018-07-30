using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Lagan.Analyzer.Tests.Helpers;
using Lagan.Analyzer.Tests.Verifiers;
using System.Linq;

namespace Lagan.Analyzer.Tests
{
    public abstract class LaganCodeFixVerifier : CodeFixVerifier
    {
        protected override CodeFixProvider GetCSharpCodeFixProvider()
        {
            return new LaganCodeFixProvider();
        }

        protected override DiagnosticAnalyzer GetCSharpDiagnosticAnalyzer()
        {
            return new LaganAnalyzer();
        }
    }

    [TestClass]
    public class GivenAnUnAdornedIDisposableMemberVariable : LaganCodeFixVerifier
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

                        public static void Main() { }
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
    }

    [TestClass]
    public class GivenAnOwnedAttributeAdornedMemberVariable : LaganCodeFixVerifier
    {
        [TestMethod]
        public void ItDoesNotGiveAnAnalyzerWarning()
        {
            var test = @"
                using System.IO;
                using Lagan.Core;

                namespace ConsoleApplication1
                {
                    class TypeName
                    {
                        [Owned]
                        private Stream _stream;

                        public static void Main() { }
                    }
                }";

            VerifyCSharpDiagnostic(test);
        }
    }
}
