using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Lagan.Core.Tests
{
    public abstract class ConversionTestsBase
    {
        protected readonly MockDisposableSubclass Disposable = new MockDisposableSubclass();
        protected readonly MockDisposableConsumer Consumer = new MockDisposableConsumer();
    }

    [TestClass]
    public class Given_an_instance_of_an_Owned_disposable : ConversionTestsBase
    {
        private readonly Owned<MockDisposableSubclass> _ownedDisposable;

        public Given_an_instance_of_an_Owned_disposable()
        {
            _ownedDisposable = Disposable.AsOwned();
        }

        [TestMethod]
        public void It_can_be_used_as_an_Owned_disposable()
        {
            Consumer.UseOwned(_ownedDisposable);
        }

        [TestMethod]
        public void It_can_be_used_as_a_Borrowed_disposable()
        {
            Consumer.UseBorrowed(_ownedDisposable.AsBorrowed());
        }

        [TestMethod]
        public void It_can_be_used_as_an_IDisposable()
        {
            Consumer.Use(_ownedDisposable.UnsafeValue);
        }

        [TestMethod]
        public void The_Dipose_call_count_is_1()
        {
            _ownedDisposable.Dispose();
            Assert.AreEqual(1, Disposable.DisposeCallCount);
        }
    }

    [TestClass]
    public class Given_an_instance_of_a_BorrowedDisposable : ConversionTestsBase
    {
        private readonly Borrowed<MockDisposableSubclass> _borrowedDisposable;

        public Given_an_instance_of_a_BorrowedDisposable()
        {
            _borrowedDisposable = Disposable.AsBorrowed();
        }

        [TestMethod]
        public void It_can_be_used_as_a_BorrowedDisposable()
        {
            Consumer.UseBorrowed(_borrowedDisposable);
        }

        [TestMethod]
        public void It_can_be_used_as_an_IDisposable()
        {
            Consumer.Use(_borrowedDisposable.UnsafeValue);
        }

        [TestMethod]
        public void The_Dipose_call_count_is_0()
        {
            _borrowedDisposable.Dispose();
            Assert.AreEqual(0, Disposable.DisposeCallCount);
        }
    }

    [TestClass]
    public class Given_an_instance_of_a_BorrowedDisposable_with_strict_mode_set : ConversionTestsBase
    {
        private readonly Borrowed<MockDisposableSubclass> _borrowedDisposable;

        public Given_an_instance_of_a_BorrowedDisposable_with_strict_mode_set()
        {
            _borrowedDisposable = Disposable.AsBorrowed(isStrict: true);
        }

        [TestMethod]
        public void It_can_be_used_as_a_BorrowedDisposable()
        {
            Consumer.UseBorrowed(_borrowedDisposable);
        }

        [TestMethod]
        public void It_can_be_used_as_an_IDisposable()
        {
            Consumer.Use(_borrowedDisposable.UnsafeValue);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void The_Dipose_call_count_is_0()
        {
            _borrowedDisposable.Dispose();
        }
    }
}
