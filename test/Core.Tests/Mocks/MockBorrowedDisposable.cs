using System;

public class MockBorrowedDisposable : IBorrowedDisposable
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