using System;

namespace Lagan.Core
{
    public struct Borrowed<T> : IDisposable where T : IDisposable
    {
        internal readonly T Inner;
        private readonly bool _isStrict; // TODO: Should this be part of the final API design?

        internal Borrowed(T inner, bool isStrict = false)
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