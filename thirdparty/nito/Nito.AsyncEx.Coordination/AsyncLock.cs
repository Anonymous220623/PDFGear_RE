// Decompiled with JetBrains decompiler
// Type: Nito.AsyncEx.AsyncLock
// Assembly: Nito.AsyncEx.Coordination, Version=5.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5843C369-F4A6-4604-9695-EBAC5AF4AB11
// Assembly location: D:\PDFGear\bin\Nito.AsyncEx.Coordination.dll

using Nito.Disposables;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

#nullable enable
namespace Nito.AsyncEx;

[DebuggerDisplay("Id = {Id}, Taken = {_taken}")]
[DebuggerTypeProxy(typeof (AsyncLock.DebugView))]
public sealed class AsyncLock
{
  private bool _taken;
  private readonly IAsyncWaitQueue<IDisposable> _queue;
  private int _id;
  private readonly object _mutex;

  public AsyncLock()
    : this((IAsyncWaitQueue<IDisposable>) null)
  {
  }

  internal AsyncLock(IAsyncWaitQueue<IDisposable>? queue)
  {
    this._queue = queue ?? (IAsyncWaitQueue<IDisposable>) new DefaultAsyncWaitQueue<IDisposable>();
    this._mutex = new object();
  }

  public int Id => IdManager<AsyncLock>.GetId(ref this._id);

  private Task<IDisposable> RequestLockAsync(CancellationToken cancellationToken)
  {
    lock (this._mutex)
    {
      if (this._taken)
        return this._queue.Enqueue<IDisposable>(this._mutex, cancellationToken);
      this._taken = true;
      return Task.FromResult<IDisposable>((IDisposable) new AsyncLock.Key(this));
    }
  }

  public AwaitableDisposable<IDisposable> LockAsync(CancellationToken cancellationToken)
  {
    return new AwaitableDisposable<IDisposable>(this.RequestLockAsync(cancellationToken));
  }

  public AwaitableDisposable<IDisposable> LockAsync() => this.LockAsync(CancellationToken.None);

  public IDisposable Lock(CancellationToken cancellationToken)
  {
    return this.RequestLockAsync(cancellationToken).WaitAndUnwrapException<IDisposable>();
  }

  public IDisposable Lock() => this.Lock(CancellationToken.None);

  internal void ReleaseLock()
  {
    lock (this._mutex)
    {
      if (this._queue.IsEmpty)
        this._taken = false;
      else
        this._queue.Dequeue((IDisposable) new AsyncLock.Key(this));
    }
  }

  private sealed class Key(AsyncLock asyncLock) : SingleDisposable<AsyncLock>(asyncLock)
  {
    protected override void Dispose(AsyncLock context) => context.ReleaseLock();
  }

  [DebuggerNonUserCode]
  private sealed class DebugView
  {
    private readonly AsyncLock _mutex;

    public DebugView(AsyncLock mutex) => this._mutex = mutex;

    public int Id => this._mutex.Id;

    public bool Taken => this._mutex._taken;

    public IAsyncWaitQueue<IDisposable> WaitQueue => this._mutex._queue;
  }
}
