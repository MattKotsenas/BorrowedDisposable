using System;

namespace Lagan.Core
{
    public static class DisposableExtensions
    {
        // TODO: This is unsafe because it allows a (possibly shared) `IDisposable` to be treated as if it were owned
        public static Owned<T> AsOwned<T>(this T disposable) where T : IDisposable
        {
            return new OwnedContainer<T>(disposable);
        }

        public static Borrowed<T> AsBorrowed<T>(this T disposable, bool isStrict = false) where T : IDisposable
        {
            return new BorrowedContainer<T>(disposable, isStrict);
        }

        public static Borrowed<T> AsBorrowed<T>(this Owned<T> owned, bool isStrict = false) where T : IDisposable
        {
            return owned.UnsafeValue.AsBorrowed(isStrict);
        }
    }
}