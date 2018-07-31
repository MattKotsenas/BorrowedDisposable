using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Lagan.Analyzer.Tests.Helpers;
using Lagan.Analyzer.Tests.Verifiers;

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
    public class GivenADisposableMemberVariable : LaganCodeFixVerifier
    {
        [DataTestMethod]
        [DataRow("private Stream", "_stream", true, 40, DisplayName = "With no annotation")]
        [DataRow("[Owned] private Stream", "_stream", false, -1, DisplayName = "With [Owned]")]
        [DataRow("[Borrowed] private Stream", "_stream", false, -1, DisplayName = "With [Borrowed]")]
        [DataRow("private Owned<Stream>", "_stream", false, -1, DisplayName = "With Owned<T>")]
        [DataRow("private Borrowed<Stream>", "_stream", false, -1, DisplayName = "With Borrowed<T>")]
        public void ItAnalyzesAndGivesAWarningIfNeeded(string declatation, string name, bool shouldHaveDiagnostic, int column)
        {
            var test = @"
                using System.IO;
                using Lagan.Core;

                namespace ConsoleApplication1
                {
                    class TypeName
                    {
                        $declaration$ $name$;

                        public static void Main() { }
                    }
                }".Replace("$declaration$", declatation).Replace("$name$", name);

            if (shouldHaveDiagnostic)
            {
                var expected = new DiagnosticResult
                {
                    Id = LaganAnalyzer.DiagnosticId,
                    Message = string.Format(LaganAnalyzer.MissingLifetimeDiagnostic.MessageFormat.ToString(), name),
                    Severity = DiagnosticSeverity.Warning,
                    Locations = new[] { new DiagnosticResultLocation("Test0.cs", 9, column) }
                };
                VerifyCSharpDiagnostic(test, expected);
            }
            else
            {
                VerifyCSharpDiagnostic(test);
            }
        }
    }

    [TestClass]
    public class GivenADisposableParameter : LaganCodeFixVerifier
    {
        [DataTestMethod]
        [DataRow("Stream", "stream", true, 59, DisplayName = "With no annotation")]
        [DataRow("[Owned] Stream", "stream", false, -1, DisplayName = "With [Owned]")]
        [DataRow("[Borrowed] Stream", "stream", false, -1, DisplayName = "With [Borrowed]")]
        [DataRow("Owned<Stream>", "stream", false, -1, DisplayName = "With Owned<T>")]
        [DataRow("Borrowed<Stream>", "stream", false, -1, DisplayName = "With Borrowed<T>")]
        public void ItAnalyzesAndGivesAWarningIfNeeded(string declatation, string name, bool shouldHaveDiagnostic, int column)
        {
            var test = @"
                using System.IO;
                using Lagan.Core;

                namespace ConsoleApplication1
                {
                    class TypeName
                    {
                        private void UseDisposable($declaration$ $name$) { }

                        public static void Main() { }
                    }
                }".Replace("$declaration$", declatation).Replace("$name$", name);

            if (shouldHaveDiagnostic)
            {
                var expected = new DiagnosticResult
                {
                    Id = LaganAnalyzer.DiagnosticId,
                    Message = string.Format(LaganAnalyzer.MissingLifetimeDiagnostic.MessageFormat.ToString(), name),
                    Severity = DiagnosticSeverity.Warning,
                    Locations = new[] { new DiagnosticResultLocation("Test0.cs", 9, column) }
                };
                VerifyCSharpDiagnostic(test, expected);
            }
            else
            {
                VerifyCSharpDiagnostic(test);
            }
        }
    }

    [TestClass]
    public class GivenANonDisposableParameter : LaganCodeFixVerifier
    {
        // Note that we don't need to test the case of a non-disposable type in the type parameter
        // because our generic constraint on Owned<T> and Borrowed<T> prevents that from compiling

        [DataTestMethod]
        [DataRow("string", "myString", false, -1, DisplayName = "With no annotation")]
        [DataRow("[Owned] string", "myString", true, 57, DisplayName = "With [Owned]")]
        [DataRow("[Borrowed] string", "myString", true, 60, DisplayName = "With [Borrowed]")]
        public void ItAnalyzesAndGivesAWarningIfNeeded(string declatation, string name, bool shouldHaveDiagnostic, int column)
        {
            var test = @"
                using System.IO;
                using Lagan.Core;

                namespace ConsoleApplication1
                {
                    class TypeName
                    {
                        private void Use($declaration$ $name$) { }

                        public static void Main() { }
                    }
                }".Replace("$declaration$", declatation).Replace("$name$", name);

            if (shouldHaveDiagnostic)
            {
                var expected = new DiagnosticResult
                {
                    Id = LaganAnalyzer.DiagnosticId,
                    Message = string.Format(LaganAnalyzer.UnnecessaryLifetimeDiagnostic.MessageFormat.ToString(), name),
                    Severity = DiagnosticSeverity.Warning,
                    Locations = new[] { new DiagnosticResultLocation("Test0.cs", 9, column) }
                };
                VerifyCSharpDiagnostic(test, expected);
            }
            else
            {
                VerifyCSharpDiagnostic(test);
            }
        }
    }

    [TestClass]
    public class GivenANonDisposableMemberVariable : LaganCodeFixVerifier
    {
        // Note that we don't need to test the case of a non-disposable type in the type parameter
        // because our generic constraint on Owned<T> and Borrowed<T> prevents that from compiling

        [DataTestMethod]
        [DataRow("private string", "_myString", false, -1, DisplayName = "With no annotation")]
        [DataRow("[Owned] private string", "_myString", true, 48, DisplayName = "With [Owned]")]
        [DataRow("[Borrowed] private string", "_myString", true, 51, DisplayName = "With [Borrowed]")]
        public void ItAnalyzesAndGivesAWarningIfNeeded(string declatation, string name, bool shouldHaveDiagnostic, int column)
        {
            var test = @"
                using System.IO;
                using Lagan.Core;

                namespace ConsoleApplication1
                {
                    class TypeName
                    {
                        $declaration$ $name$;

                        public static void Main() { }
                    }
                }".Replace("$declaration$", declatation).Replace("$name$", name);

            if (shouldHaveDiagnostic)
            {
                var expected = new DiagnosticResult
                {
                    Id = LaganAnalyzer.DiagnosticId,
                    Message = string.Format(LaganAnalyzer.UnnecessaryLifetimeDiagnostic.MessageFormat.ToString(), name),
                    Severity = DiagnosticSeverity.Warning,
                    Locations = new[] { new DiagnosticResultLocation("Test0.cs", 9, column) }
                };
                VerifyCSharpDiagnostic(test, expected);
            }
            else
            {
                VerifyCSharpDiagnostic(test);
            }
        }
    }
}
