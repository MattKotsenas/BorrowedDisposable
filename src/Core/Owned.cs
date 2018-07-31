using System;

namespace Lagan.Core
{
    public interface Owned<out T> : IDisposable where T : IDisposable
    {
        T UnsafeValue { get; } // TODO: Is there a way to remove this?
    }

    internal struct OwnedContainer<T> : Owned<T> where T : IDisposable
    {
        public T UnsafeValue { get; }

        internal OwnedContainer(T value)
        {
            UnsafeValue = value;
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                UnsafeValue.Dispose();
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
