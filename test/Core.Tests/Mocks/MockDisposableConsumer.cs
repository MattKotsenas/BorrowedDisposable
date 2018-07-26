namespace Lagan.Core.Tests
{
    public class MockDisposableConsumer
    {
        public void UseOwnedDisposable(OwnedDisposable<MockDisposable> ownedDisposable)
        {
        }

        public void UseBorrowedDisposable(BorrowedDisposable<MockDisposable> borrowedDisposable)
        {
        }

        public void UseDisposable(MockDisposable disposable)
        {
        }
    }
}