namespace Lagan.Core.Tests
{
    public class MockDisposableConsumer
    {
        public void UseOwned(Owned<MockDisposable> owned)
        {
        }

        public void UseBorrowed(Borrowed<MockDisposable> borrowed)
        {
        }

        public void Use(MockDisposable disposable)
        {
        }
    }
}