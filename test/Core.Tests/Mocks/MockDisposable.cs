using System;

namespace Lagan.Core.Tests
{
    public class MockDisposable : IDisposable
    {
        public int DisposeCallCount { get; private set; }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                DisposeCallCount++;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }

    public class MockDisposableSubclass : MockDisposable
    {
    }
}
