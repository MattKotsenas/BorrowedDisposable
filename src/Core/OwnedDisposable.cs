using System;

namespace BorrowedDisposable.Core
{
    public struct OwnedDisposable<T> : IDisposable where T : IDisposable
    {
        private readonly T _inner;

        public OwnedDisposable(T inner)
        {
            _inner = inner;
        }

        // TODO: Should this be an implicit or explicit operator (ease-of-use vs. safety)
        public static implicit operator T(OwnedDisposable<T> owned)
        {
            return owned._inner;
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                _inner.Dispose();
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
