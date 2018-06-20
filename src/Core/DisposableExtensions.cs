using System;

namespace BorrowedDisposable.Core
{
    public static class DisposableExtensions
    {
        // TODO: This is unsafe because it allows a (possibly shared) `IDisposable` to be treated as if it were owned
        public static OwnedDisposable<T> AsOwned<T>(this T disposable) where T : IDisposable
        {
            return new OwnedDisposable<T>(disposable);
        }

        public static BorrowedDisposable<T> AsBorrowed<T>(this T disposable, bool isStrict = false) where T : IDisposable
        {
            return new BorrowedDisposable<T>(disposable, isStrict);
        }

        public static BorrowedDisposable<T> AsBorrowed<T>(this OwnedDisposable<T> owned, bool isStrict = false) where T : IDisposable
        {
            return owned.Inner.AsBorrowed(isStrict);
        }


        public static T AsDisposableUnsafe<T>(this OwnedDisposable<T> owned) where T : IDisposable
        {
            return owned.Inner;
        }

        public static T AsDisposableUnsafe<T>(this BorrowedDisposable<T> borrowed) where T : IDisposable
        {
            return borrowed.Inner;
        }
    }
}