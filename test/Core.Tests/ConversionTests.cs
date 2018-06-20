using Microsoft.VisualStudio.TestTools.UnitTesting;
using BorrowedDisposable.Core;
using System;

namespace Core.Tests
{
    public abstract class ConversionTestsBase
    {
        protected readonly MockDisposable Disposable = new MockDisposable();
        protected readonly MockDisposableConsumer Consumer = new MockDisposableConsumer();
    }

    [TestClass]
    public class Given_an_instance_of_an_OwnedDisposable : ConversionTestsBase
    {
        private readonly OwnedDisposable<MockDisposable> _ownedDisposable;

        public Given_an_instance_of_an_OwnedDisposable()
        {
            _ownedDisposable = new OwnedDisposable<MockDisposable>(Disposable);
        }

        [TestMethod]
        public void It_can_be_used_as_an_OwnedDisposable()
        {
            Consumer.UseOwnedDisposable(_ownedDisposable);
        }

        [TestMethod]
        public void It_can_be_used_as_an_IDisposable()
        {
            Consumer.UseDisposable(_ownedDisposable);
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
        private readonly BorrowedDisposable<MockDisposable> _borrowedDisposable;

        public Given_an_instance_of_a_BorrowedDisposable()
        {
            _borrowedDisposable = new BorrowedDisposable<MockDisposable>(Disposable);
        }

        [TestMethod]
        public void It_can_be_used_as_a_BorrowedDisposable()
        {
            Consumer.UseBorrowedDisposable(_borrowedDisposable);
        }

        [TestMethod]
        public void It_can_be_used_as_an_IDisposable()
        {
            Consumer.UseDisposable(_borrowedDisposable);
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
        private readonly BorrowedDisposable<MockDisposable> _borrowedDisposable;

        public Given_an_instance_of_a_BorrowedDisposable_with_strict_mode_set()
        {
            _borrowedDisposable = new BorrowedDisposable<MockDisposable>(Disposable, isStrict: true);
        }

        [TestMethod]
        public void It_can_be_used_as_a_BorrowedDisposable()
        {
            Consumer.UseBorrowedDisposable(_borrowedDisposable);
        }

        [TestMethod]
        public void It_can_be_used_as_an_IDisposable()
        {
            Consumer.UseDisposable(_borrowedDisposable);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void The_Dipose_call_count_is_0()
        {
            _borrowedDisposable.Dispose();
        }
    }
}
