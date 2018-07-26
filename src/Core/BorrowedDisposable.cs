using System;

namespace Lagan.Core
{
    public struct BorrowedDisposable<T> : IDisposable where T : IDisposable
    {
        internal readonly T Inner;
        private readonly bool _isStrict; // TODO: Should this be part of the final API design?

        internal BorrowedDisposable(T inner, bool isStrict = false)
        {
            Inner = inner;
            _isStrict = isStrict;
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_isStrict)
                {
                    throw new InvalidOperationException($"Attempt to dispose of a {nameof(BorrowedDisposable<T>)}");
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