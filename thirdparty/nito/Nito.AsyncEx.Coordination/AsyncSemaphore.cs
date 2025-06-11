// Decompiled with JetBrains decompiler
// Type: Nito.AsyncEx.AsyncSemaphore
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

[DebuggerDisplay("Id = {Id}, CurrentCount = {_count}")]
[DebuggerTypeProxy(typeof (AsyncSemaphore.DebugView))]
public sealed class AsyncSemaphore
{
  private readonly IAsyncWaitQueue<object> _queue;
  private long _count;
  private int _id;
  private readonly object _mutex;

  internal AsyncSemaphore(long initialCount, IAsyncWaitQueue<object>? queue)
  {
    this._queue = queue ?? (IAsyncWaitQueue<object>) new DefaultAsyncWaitQueue<object>();
    this._count = initialCount;
    this._mutex = new object();
  }

  public AsyncSemaphore(long initialCount)
    : this(initialCount, (IAsyncWaitQueue<object>) null)
  {
  }

  public int Id => IdManager<AsyncSemaphore>.GetId(ref this._id);

  public long CurrentCount
  {
    get
    {
      lock (this._mutex)
        return this._count;
    }
  }

  public Task WaitAsync(CancellationToken cancellationToken)
  {
    lock (this._mutex)
    {
      if (this._count == 0L)
        return (Task) this._queue.Enqueue<object>(this._mutex, cancellationToken);
      --this._count;
      return TaskConstants.Completed;
    }
  }

  public Task WaitAsync() => this.WaitAsync(CancellationToken.None);

  public void Wait(CancellationToken cancellationToken)
  {
    this.WaitAsync(cancellationToken).WaitAndUnwrapException(CancellationToken.None);
  }

  public void Wait() => this.Wait(CancellationToken.None);

  public void Release(long releaseCount)
  {
    if (releaseCount == 0L)
      return;
    lock (this._mutex)
    {
      long num = checked (this._count + releaseCount);
      for (; releaseCount != 0L && !this._queue.IsEmpty; --releaseCount)
        this._queue.Dequeue();
      this._count += releaseCount;
    }
  }

  public void Release() => this.Release(1L);

  private async Task<IDisposable> DoLockAsync(CancellationToken cancellationToken)
  {
    AsyncSemaphore asyncSemaphore = this;
    await asyncSemaphore.WaitAsync(cancellationToken).ConfigureAwait(false);
    return (IDisposable) Disposable.Create(new Action(asyncSemaphore.Release));
  }

  public AwaitableDisposable<IDisposable> LockAsync(CancellationToken cancellationToken)
  {
    return new AwaitableDisposable<IDisposable>(this.DoLockAsync(cancellationToken));
  }

  public AwaitableDisposable<IDisposable> LockAsync() => this.LockAsync(CancellationToken.None);

  public IDisposable Lock(CancellationToken cancellationToken)
  {
    this.Wait(cancellationToken);
    return (IDisposable) Disposable.Create(new Action(this.Release));
  }

  public IDisposable Lock() => this.Lock(CancellationToken.None);

  [DebuggerNonUserCode]
  private sealed class DebugView
  {
    private readonly AsyncSemaphore _semaphore;

    public DebugView(AsyncSemaphore semaphore) => this._semaphore = semaphore;

    public int Id => this._semaphore.Id;

    public long CurrentCount => this._semaphore._count;

    public IAsyncWaitQueue<object> WaitQueue => this._semaphore._queue;
  }
}
