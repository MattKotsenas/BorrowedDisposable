using System;

namespace BorrowedDisposable.Core
{
    public struct OwnedDisposable<T> : IDisposable where T : IDisposable
    {
        internal readonly T Inner;

        internal OwnedDisposable(T inner)
        {
            Inner = inner;
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                Inner.Dispose();
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
