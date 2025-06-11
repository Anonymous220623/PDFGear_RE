// Decompiled with JetBrains decompiler
// Type: Nito.AsyncEx.AsyncReaderWriterLock
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

[DebuggerDisplay("Id = {Id}, State = {GetStateForDebugger}, ReaderCount = {GetReaderCountForDebugger}")]
[DebuggerTypeProxy(typeof (AsyncReaderWriterLock.DebugView))]
public sealed class AsyncReaderWriterLock
{
  private readonly IAsyncWaitQueue<IDisposable> _writerQueue;
  private readonly IAsyncWaitQueue<IDisposable> _readerQueue;
  private readonly object _mutex;
  private int _id;
  private int _locksHeld;

  [DebuggerNonUserCode]
  internal AsyncReaderWriterLock.State GetStateForDebugger
  {
    get
    {
      if (this._locksHeld == 0)
        return AsyncReaderWriterLock.State.Unlocked;
      return this._locksHeld == -1 ? AsyncReaderWriterLock.State.WriteLocked : AsyncReaderWriterLock.State.ReadLocked;
    }
  }

  [DebuggerNonUserCode]
  internal int GetReaderCountForDebugger => this._locksHeld <= 0 ? 0 : this._locksHeld;

  internal AsyncReaderWriterLock(
    IAsyncWaitQueue<IDisposable>? writerQueue,
    IAsyncWaitQueue<IDisposable>? readerQueue)
  {
    this._writerQueue = writerQueue ?? (IAsyncWaitQueue<IDisposable>) new DefaultAsyncWaitQueue<IDisposable>();
    this._readerQueue = readerQueue ?? (IAsyncWaitQueue<IDisposable>) new DefaultAsyncWaitQueue<IDisposable>();
    this._mutex = new object();
  }

  public AsyncReaderWriterLock()
    : this((IAsyncWaitQueue<IDisposable>) null, (IAsyncWaitQueue<IDisposable>) null)
  {
  }

  public int Id => IdManager<AsyncReaderWriterLock>.GetId(ref this._id);

  private void ReleaseWaitersWhenCanceled(Task task)
  {
    task.ContinueWith((Action<Task>) (t =>
    {
      lock (this._mutex)
        this.ReleaseWaiters();
    }), CancellationToken.None, TaskContinuationOptions.OnlyOnCanceled | TaskContinuationOptions.ExecuteSynchronously, TaskScheduler.Default);
  }

  private Task<IDisposable> RequestReaderLockAsync(CancellationToken cancellationToken)
  {
    lock (this._mutex)
    {
      if (this._locksHeld < 0 || !this._writerQueue.IsEmpty)
        return this._readerQueue.Enqueue<IDisposable>(this._mutex, cancellationToken);
      ++this._locksHeld;
      return Task.FromResult<IDisposable>((IDisposable) new AsyncReaderWriterLock.ReaderKey(this));
    }
  }

  public AwaitableDisposable<IDisposable> ReaderLockAsync(CancellationToken cancellationToken)
  {
    return new AwaitableDisposable<IDisposable>(this.RequestReaderLockAsync(cancellationToken));
  }

  public AwaitableDisposable<IDisposable> ReaderLockAsync()
  {
    return this.ReaderLockAsync(CancellationToken.None);
  }

  public IDisposable ReaderLock(CancellationToken cancellationToken)
  {
    return this.RequestReaderLockAsync(cancellationToken).WaitAndUnwrapException<IDisposable>();
  }

  public IDisposable ReaderLock() => this.ReaderLock(CancellationToken.None);

  private Task<IDisposable> RequestWriterLockAsync(CancellationToken cancellationToken)
  {
    Task<IDisposable> task;
    lock (this._mutex)
    {
      if (this._locksHeld == 0)
      {
        this._locksHeld = -1;
        task = Task.FromResult<IDisposable>((IDisposable) new AsyncReaderWriterLock.WriterKey(this));
      }
      else
        task = this._writerQueue.Enqueue<IDisposable>(this._mutex, cancellationToken);
    }
    this.ReleaseWaitersWhenCanceled((Task) task);
    return task;
  }

  public AwaitableDisposable<IDisposable> WriterLockAsync(CancellationToken cancellationToken)
  {
    return new AwaitableDisposable<IDisposable>(this.RequestWriterLockAsync(cancellationToken));
  }

  public AwaitableDisposable<IDisposable> WriterLockAsync()
  {
    return this.WriterLockAsync(CancellationToken.None);
  }

  public IDisposable WriterLock(CancellationToken cancellationToken)
  {
    return this.RequestWriterLockAsync(cancellationToken).WaitAndUnwrapException<IDisposable>();
  }

  public IDisposable WriterLock() => this.WriterLock(CancellationToken.None);

  private void ReleaseWaiters()
  {
    if (this._locksHeld == -1)
      return;
    if (!this._writerQueue.IsEmpty)
    {
      if (this._locksHeld != 0)
        return;
      this._locksHeld = -1;
      this._writerQueue.Dequeue((IDisposable) new AsyncReaderWriterLock.WriterKey(this));
    }
    else
    {
      while (!this._readerQueue.IsEmpty)
      {
        this._readerQueue.Dequeue((IDisposable) new AsyncReaderWriterLock.ReaderKey(this));
        ++this._locksHeld;
      }
    }
  }

  internal void ReleaseReaderLock()
  {
    lock (this._mutex)
    {
      --this._locksHeld;
      this.ReleaseWaiters();
    }
  }

  internal void ReleaseWriterLock()
  {
    lock (this._mutex)
    {
      this._locksHeld = 0;
      this.ReleaseWaiters();
    }
  }

  internal enum State
  {
    Unlocked,
    ReadLocked,
    WriteLocked,
  }

  private sealed class ReaderKey(AsyncReaderWriterLock asyncReaderWriterLock) : 
    SingleDisposable<AsyncReaderWriterLock>(asyncReaderWriterLock)
  {
    protected override void Dispose(AsyncReaderWriterLock context) => context.ReleaseReaderLock();
  }

  private sealed class WriterKey(AsyncReaderWriterLock asyncReaderWriterLock) : 
    SingleDisposable<AsyncReaderWriterLock>(asyncReaderWriterLock)
  {
    protected override void Dispose(AsyncReaderWriterLock context) => context.ReleaseWriterLock();
  }

  [DebuggerNonUserCode]
  private sealed class DebugView
  {
    private readonly AsyncReaderWriterLock _rwl;

    public DebugView(AsyncReaderWriterLock rwl) => this._rwl = rwl;

    public int Id => this._rwl.Id;

    public AsyncReaderWriterLock.State State => this._rwl.GetStateForDebugger;

    public int ReaderCount => this._rwl.GetReaderCountForDebugger;

    public IAsyncWaitQueue<IDisposable> ReaderWaitQueue => this._rwl._readerQueue;

    public IAsyncWaitQueue<IDisposable> WriterWaitQueue => this._rwl._writerQueue;
  }
}
