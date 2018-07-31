using System;

namespace Lagan.Core
{
    public interface Borrowed<out T> : IDisposable where T : IDisposable
    {
        T UnsafeValue { get; } // TODO: Is there a way to remove this?
    }

    internal struct BorrowedContainer<T> : Borrowed<T> where T : IDisposable
    {
        private readonly bool _isStrict; // TODO: Should this be part of the final API design?

        internal BorrowedContainer(T value, bool isStrict = false)
        {
            UnsafeValue = value;
            _isStrict = isStrict;
        }

        public T UnsafeValue { get; }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_isStrict)
                {
                    throw new InvalidOperationException($"Attempt to dispose of a {nameof(Borrowed<T>)}");
                }
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}