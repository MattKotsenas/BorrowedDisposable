using Microsoft.VisualStudio.TestTools.UnitTesting;
using BorrowedDisposable.Core;

namespace Core.Tests
{
    [TestClass]
    public class Given_an_instance_of_IOwnedDisposable
    {
        private readonly IOwnedDisposable _ownedDisposable = new MockOwnedDisposable();
        private readonly MockDisposableConsumer _consumer = new MockDisposableConsumer();

        [TestMethod]
        public void It_can_be_used_as_an_OwnedDisposable()
        {
            _consumer.UseOwnedDisposable(_ownedDisposable);
        }

        [TestMethod]
        public void It_can_be_used_as_an_IDisposable()
        {
            _consumer.UseDisposable(_ownedDisposable);
        }
    }

    [TestClass]
    public class Given_an_instance_of_IBorrowedDisposable
    {
        private readonly IBorrowedDisposable _borrowedDisposable = new MockBorrowedDisposable();
        private readonly MockDisposableConsumer _consumer = new MockDisposableConsumer();

        [TestMethod]
        public void It_can_be_used_as_a_BorrowedDisposable()
        {
            _consumer.UseBorrowedDisposable(_borrowedDisposable);
        }

        [TestMethod]
        public void It_can_be_used_as_an_IDisposable()
        {
            _consumer.UseDisposable(_borrowedDisposable);
        }
    }
}
