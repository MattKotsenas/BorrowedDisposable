using System;

namespace Lagan.Core
{
    public struct Owned<T> : IDisposable where T : IDisposable
    {
        internal readonly T Inner;

        internal Owned(T inner)
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
