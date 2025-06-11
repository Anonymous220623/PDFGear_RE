// Decompiled with JetBrains decompiler
// Type: Nito.AsyncEx.AsyncAutoResetEvent
// Assembly: Nito.AsyncEx.Coordination, Version=5.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5843C369-F4A6-4604-9695-EBAC5AF4AB11
// Assembly location: D:\PDFGear\bin\Nito.AsyncEx.Coordination.dll

using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

#nullable enable
namespace Nito.AsyncEx;

[DebuggerDisplay("Id = {Id}, IsSet = {_set}")]
[DebuggerTypeProxy(typeof (AsyncAutoResetEvent.DebugView))]
public sealed class AsyncAutoResetEvent
{
  private readonly IAsyncWaitQueue<object> _queue;
  private bool _set;
  private int _id;
  private readonly object _mutex;

  internal AsyncAutoResetEvent(bool set, IAsyncWaitQueue<object>? queue)
  {
    this._queue = queue ?? (IAsyncWaitQueue<object>) new DefaultAsyncWaitQueue<object>();
    this._set = set;
    this._mutex = new object();
  }

  public AsyncAutoResetEvent(bool set)
    : this(set, (IAsyncWaitQueue<object>) null)
  {
  }

  public AsyncAutoResetEvent()
    : this(false, (IAsyncWaitQueue<object>) null)
  {
  }

  public int Id => IdManager<AsyncAutoResetEvent>.GetId(ref this._id);

  public bool IsSet
  {
    get
    {
      lock (this._mutex)
        return this._set;
    }
  }

  public Task WaitAsync(CancellationToken cancellationToken)
  {
    lock (this._mutex)
    {
      if (!this._set)
        return (Task) this._queue.Enqueue<object>(this._mutex, cancellationToken);
      this._set = false;
      return TaskConstants.Completed;
    }
  }

  public Task WaitAsync() => this.WaitAsync(CancellationToken.None);

  public void Wait(CancellationToken cancellationToken)
  {
    this.WaitAsync(cancellationToken).WaitAndUnwrapException(CancellationToken.None);
  }

  public void Wait() => this.Wait(CancellationToken.None);

  public void Set()
  {
    lock (this._mutex)
    {
      if (this._queue.IsEmpty)
        this._set = true;
      else
        this._queue.Dequeue();
    }
  }

  [DebuggerNonUserCode]
  private sealed class DebugView
  {
    private readonly AsyncAutoResetEvent _are;

    public DebugView(AsyncAutoResetEvent are) => this._are = are;

    public int Id => this._are.Id;

    public bool IsSet => this._are._set;

    public IAsyncWaitQueue<object> WaitQueue => this._are._queue;
  }
}
