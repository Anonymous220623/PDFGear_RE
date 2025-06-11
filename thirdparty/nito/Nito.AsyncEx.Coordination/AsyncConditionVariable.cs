// Decompiled with JetBrains decompiler
// Type: Nito.AsyncEx.AsyncConditionVariable
// Assembly: Nito.AsyncEx.Coordination, Version=5.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5843C369-F4A6-4604-9695-EBAC5AF4AB11
// Assembly location: D:\PDFGear\bin\Nito.AsyncEx.Coordination.dll

using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

#nullable enable
namespace Nito.AsyncEx;

[DebuggerDisplay("Id = {Id}, AsyncLockId = {_asyncLock.Id}")]
[DebuggerTypeProxy(typeof (AsyncConditionVariable.DebugView))]
public sealed class AsyncConditionVariable
{
  private readonly AsyncLock _asyncLock;
  private readonly IAsyncWaitQueue<object> _queue;
  private int _id;
  private readonly object _mutex;

  internal AsyncConditionVariable(AsyncLock asyncLock, IAsyncWaitQueue<object>? queue)
  {
    this._asyncLock = asyncLock;
    this._queue = queue ?? (IAsyncWaitQueue<object>) new DefaultAsyncWaitQueue<object>();
    this._mutex = new object();
  }

  public AsyncConditionVariable(AsyncLock asyncLock)
    : this(asyncLock, (IAsyncWaitQueue<object>) null)
  {
  }

  public int Id => IdManager<AsyncConditionVariable>.GetId(ref this._id);

  public void Notify()
  {
    lock (this._mutex)
    {
      if (this._queue.IsEmpty)
        return;
      this._queue.Dequeue();
    }
  }

  public void NotifyAll()
  {
    lock (this._mutex)
      this._queue.DequeueAll();
  }

  public Task WaitAsync(CancellationToken cancellationToken)
  {
    lock (this._mutex)
    {
      Task task = AsyncConditionVariable.WaitAndRetakeLockAsync((Task) this._queue.Enqueue<object>(this._mutex, cancellationToken), this._asyncLock);
      this._asyncLock.ReleaseLock();
      return task;
    }
  }

  private static async Task WaitAndRetakeLockAsync(Task task, AsyncLock asyncLock)
  {
    try
    {
      await task.ConfigureAwait(false);
    }
    finally
    {
      IDisposable disposable = await asyncLock.LockAsync().ConfigureAwait(false);
    }
  }

  public Task WaitAsync() => this.WaitAsync(CancellationToken.None);

  public void Wait(CancellationToken cancellationToken)
  {
    this.WaitAsync(cancellationToken).WaitAndUnwrapException(CancellationToken.None);
  }

  public void Wait() => this.Wait(CancellationToken.None);

  [DebuggerNonUserCode]
  private sealed class DebugView
  {
    private readonly AsyncConditionVariable _cv;

    public DebugView(AsyncConditionVariable cv) => this._cv = cv;

    public int Id => this._cv.Id;

    public AsyncLock AsyncLock => this._cv._asyncLock;

    public IAsyncWaitQueue<object> WaitQueue => this._cv._queue;
  }
}
