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
        public class WithAnOwnedType : LaganCodeFixVerifier
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
                        private Owned<Stream> _stream;

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

        [TestClass]
        public class WithABorrowedType : LaganCodeFixVerifier
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
                        private Borrowed<Stream> _stream;

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
        public class WithAnOwnedType : LaganCodeFixVerifier
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
                        private void UseDisposable(Owned<Stream> stream) { }

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

        [TestClass]
        public class WithABorrowedType : LaganCodeFixVerifier
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
                        private void UseDisposable(Borrowed<Stream> stream) { }

                        public static void Main() { }
                    }
                }";

                VerifyCSharpDiagnostic(test);
            }
        }
    }

    public class GivenANonDisposableParameter
    {
        // Note that we don't need to test the case of a non-disposable type in the type parameter
        // because our generic constraint on Owned<T> and Borrowed<T> prevents that from compiling

        [TestClass]
        public class WithNoAnnotation : LaganCodeFixVerifier
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
                        private void Use(string myString) { }

                        public static void Main() { }
                    }
                }";

                VerifyCSharpDiagnostic(test);
            }
        }

        [TestClass]
        public class WithAnOwnedAnnotation : LaganCodeFixVerifier
        {
            [TestMethod]
            public void ItGivesAnAnalyzerWarning()
            {
                var test = @"
                using System.IO;
                using Lagan.Core;

                namespace ConsoleApplication1
                {
                    class TypeName
                    {
                        private void Use([Owned] string myString) { }

                        public static void Main() { }
                    }
                }";

                var expected = new DiagnosticResult
                {
                    Id = LaganAnalyzer.DiagnosticId,
                    Message = string.Format(LaganAnalyzer.UnnecessaryLifetimeDiagnostic.MessageFormat.ToString(), "myString"),
                    Severity = DiagnosticSeverity.Warning,
                    Locations = new[] { new DiagnosticResultLocation("Test0.cs", 9, 57) }
                };

                VerifyCSharpDiagnostic(test, expected);
            }
        }

        [TestClass]
        public class WithABorrowedAnnotation : LaganCodeFixVerifier
        {
            [TestMethod]
            public void ItGivesAnAnalyzerWarning()
            {
                var test = @"
                using System.IO;
                using Lagan.Core;

                namespace ConsoleApplication1
                {
                    class TypeName
                    {
                        private void Use([Borrowed] string myString) { }

                        public static void Main() { }
                    }
                }";

                var expected = new DiagnosticResult
                {
                    Id = LaganAnalyzer.DiagnosticId,
                    Message = string.Format(LaganAnalyzer.UnnecessaryLifetimeDiagnostic.MessageFormat.ToString(), "myString"),
                    Severity = DiagnosticSeverity.Warning,
                    Locations = new[] { new DiagnosticResultLocation("Test0.cs", 9, 60) }
                };

                VerifyCSharpDiagnostic(test, expected);
            }
        }
    }

    public class GivenANonDisposableMemberVariable
    {
        // Note that we don't need to test the case of a non-disposable type in the type parameter
        // because our generic constraint on Owned<T> and Borrowed<T> prevents that from compiling

        [TestClass]
        public class WithNoAnnotation : LaganCodeFixVerifier
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
                        private string _myString;

                        public static void Main() { }
                    }
                }";

                VerifyCSharpDiagnostic(test);
            }
        }

        [TestClass]
        public class WithAnOwnedAnnotation : LaganCodeFixVerifier
        {
            [TestMethod]
            public void ItGivesAnAnalyzerWarning()
            {
                var test = @"
                using System.IO;
                using Lagan.Core;

                namespace ConsoleApplication1
                {
                    class TypeName
                    {
                        [Owned]
                        private string _myString;

                        public static void Main() { }
                    }
                }";

                var expected = new DiagnosticResult
                {
                    Id = LaganAnalyzer.DiagnosticId,
                    Message = string.Format(LaganAnalyzer.UnnecessaryLifetimeDiagnostic.MessageFormat.ToString(), "_myString"),
                    Severity = DiagnosticSeverity.Warning,
                    Locations = new[] { new DiagnosticResultLocation("Test0.cs", 10, 40) }
                };

                VerifyCSharpDiagnostic(test, expected);
            }
        }

        [TestClass]
        public class WithABorrowedAnnotation : LaganCodeFixVerifier
        {
            [TestMethod]
            public void ItGivesAnAnalyzerWarning()
            {
                var test = @"
                using System.IO;
                using Lagan.Core;

                namespace ConsoleApplication1
                {
                    class TypeName
                    {
                        [Borrowed]
                        private string _myString;

                        public static void Main() { }
                    }
                }";

                var expected = new DiagnosticResult
                {
                    Id = LaganAnalyzer.DiagnosticId,
                    Message = string.Format(LaganAnalyzer.UnnecessaryLifetimeDiagnostic.MessageFormat.ToString(), "_myString"),
                    Severity = DiagnosticSeverity.Warning,
                    Locations = new[] { new DiagnosticResultLocation("Test0.cs", 10, 40) }
                };

                VerifyCSharpDiagnostic(test, expected);
            }
        }
    }
}
