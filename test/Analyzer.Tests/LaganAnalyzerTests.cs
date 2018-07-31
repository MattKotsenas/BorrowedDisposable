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

    public class GivenADisposableMemberVariable
    {
        [TestClass]
        public class WithNoAnnotation : LaganCodeFixVerifier
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
                    Message = string.Format(LaganAnalyzer.MissingLifetimeDiagnostic.MessageFormat.ToString(), "_stream"),
                    Severity = DiagnosticSeverity.Warning,
                    Locations = new[] { new DiagnosticResultLocation("Test0.cs", 8, 40) }
                };

                VerifyCSharpDiagnostic(test, expected);
            }
        }

        [TestClass]
        public class WithAnOwnedAnnotation : LaganCodeFixVerifier
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

        [TestClass]
        public class WithABorrowedAnnotation : LaganCodeFixVerifier
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
                        [Borrowed]
                        private Stream _stream;

                        public static void Main() { }
                    }
                }";

                VerifyCSharpDiagnostic(test);
            }
        }
    }

    public class GivenADisposableParameter
    {
        [TestClass]
        public class WithNoAnnotation : LaganCodeFixVerifier
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
                        private void UseDisposable(Stream stream) { }

                        public static void Main() { }
                    }
                }";
                var expected = new DiagnosticResult
                {
                    Id = LaganAnalyzer.DiagnosticId,
                    Message = string.Format(LaganAnalyzer.MissingLifetimeDiagnostic.MessageFormat.ToString(), "stream"),
                    Severity = DiagnosticSeverity.Warning,
                    Locations = new[] { new DiagnosticResultLocation("Test0.cs", 8, 59) }
                };

                VerifyCSharpDiagnostic(test, expected);
            }
        }

        [TestClass]
        public class WithAnOwnedAnnotation : LaganCodeFixVerifier
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
                        private void UseDisposable([Owned] Stream stream) { }

                        public static void Main() { }
                    }
                }";

                VerifyCSharpDiagnostic(test);
            }
        }

        [TestClass]
        public class WithABorrowedAnnotation : LaganCodeFixVerifier
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
                        private void UseDisposable([Borrowed] Stream stream) { }

                        public static void Main() { }
                    }
                }";

                VerifyCSharpDiagnostic(test);
            }
        }
    }
}
