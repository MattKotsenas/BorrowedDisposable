using System;

namespace Lagan.Core
{
    public static class DisposableExtensions
    {
        // TODO: This is unsafe because it allows a (possibly shared) `IDisposable` to be treated as if it were owned
        public static Owned<T> AsOwned<T>(this T disposable) where T : IDisposable
        {
            return new Owned<T>(disposable);
        }

        public static Borrowed<T> AsBorrowed<T>(this T disposable, bool isStrict = false) where T : IDisposable
        {
            return new Borrowed<T>(disposable, isStrict);
        }

        public static Borrowed<T> AsBorrowed<T>(this Owned<T> owned, bool isStrict = false) where T : IDisposable
        {
            return owned.Inner.AsBorrowed(isStrict);
        }


        public static T AsDisposableUnsafe<T>(this Owned<T> owned) where T : IDisposable
        {
            return owned.Inner;
        }

        public static T AsDisposableUnsafe<T>(this Borrowed<T> borrowed) where T : IDisposable
        {
            return borrowed.Inner;
        }
    }
}