using System;

namespace BorrowedDisposable.Core
{
    public struct BorrowedDisposable<T> : IDisposable where T : IDisposable
    {
        private readonly T _inner;
        private readonly bool _isStrict; // TODO: Should this be part of the final API design?

        public BorrowedDisposable(T inner, bool isStrict = false)
        {
            _inner = inner;
            _isStrict = isStrict;
        }

        // TODO: Should this be an implicit or explicit operator (ease-of-use vs. safety)
        public static implicit operator T(BorrowedDisposable<T> owned)
        {
            return owned._inner;
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_isStrict)
                {
                    throw new InvalidOperationException($"Attempt to dispose of a {nameof(BorrowedDisposable)}");
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