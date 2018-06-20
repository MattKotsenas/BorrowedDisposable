using System;
using BorrowedDisposable.Core;

public class MockDisposableConsumer
{
    public void UseOwnedDisposable(OwnedDisposable<MockDisposable> ownedDisposable)
    {
    }

    public void UseBorrowedDisposable(BorrowedDisposable<MockDisposable> borrowedDisposable)
    {
    }

    public void UseDisposable(MockDisposable disposable)
    {
    }
}