using System;
using BorrowedDisposable.Core;

public class MockOwnedDisposable : IOwnedDisposable
{
    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {

        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}