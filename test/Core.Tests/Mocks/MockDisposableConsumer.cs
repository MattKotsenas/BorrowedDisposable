using System;
using BorrowedDisposable.Core;

public class MockDisposableConsumer
{
    public void UseOwnedDisposable(IOwnedDisposable ownedDisposable)
    {

    }

    public void UseBorrowedDisposable(IBorrowedDisposable borrowedDisposable)
    {

    }

    public void UseDisposable(IDisposable disposable)
    {

    }
}