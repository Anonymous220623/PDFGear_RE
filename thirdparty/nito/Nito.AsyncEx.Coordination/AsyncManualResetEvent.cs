// Decompiled with JetBrains decompiler
// Type: Nito.AsyncEx.AsyncManualResetEvent
// Assembly: Nito.AsyncEx.Coordination, Version=5.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5843C369-F4A6-4604-9695-EBAC5AF4AB11
// Assembly location: D:\PDFGear\bin\Nito.AsyncEx.Coordination.dll

using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

#nullable enable
namespace Nito.AsyncEx;

[DebuggerDisplay("Id = {Id}, IsSet = {GetStateForDebugger}")]
[DebuggerTypeProxy(typeof (AsyncManualResetEvent.DebugView))]
public sealed class AsyncManualResetEvent
{
  private readonly object _mutex;
  private TaskCompletionSource<object?> _tcs;
  private int _id;

  [DebuggerNonUserCode]
  private bool GetStateForDebugger => this._tcs.Task.IsCompleted;

  public AsyncManualResetEvent(bool set)
  {
    this._mutex = new object();
    this._tcs = TaskCompletionSourceExtensions.CreateAsyncTaskSource<object>();
    if (!set)
      return;
    this._tcs.TrySetResult((object) null);
  }

  public AsyncManualResetEvent()
    : this(false)
  {
  }

  public int Id => IdManager<AsyncManualResetEvent>.GetId(ref this._id);

  public bool IsSet
  {
    get
    {
      lock (this._mutex)
        return this._tcs.Task.IsCompleted;
    }
  }

  public Task WaitAsync()
  {
    lock (this._mutex)
      return (Task) this._tcs.Task;
  }

  public Task WaitAsync(CancellationToken cancellationToken)
  {
    Task @this = this.WaitAsync();
    return @this.IsCompleted ? @this : @this.WaitAsync(cancellationToken);
  }

  public void Wait() => this.WaitAsync().WaitAndUnwrapException();

  public void Wait(CancellationToken cancellationToken)
  {
    Task task = this.WaitAsync(CancellationToken.None);
    if (task.IsCompleted)
      return;
    task.WaitAndUnwrapException(cancellationToken);
  }

  public void Set()
  {
    lock (this._mutex)
      this._tcs.TrySetResult((object) null);
  }

  public void Reset()
  {
    lock (this._mutex)
    {
      if (!this._tcs.Task.IsCompleted)
        return;
      this._tcs = TaskCompletionSourceExtensions.CreateAsyncTaskSource<object>();
    }
  }

  [DebuggerNonUserCode]
  private sealed class DebugView
  {
    private readonly AsyncManualResetEvent _mre;

    public DebugView(AsyncManualResetEvent mre) => this._mre = mre;

    public int Id => this._mre.Id;

    public bool IsSet => this._mre.GetStateForDebugger;

    public Task CurrentTask => (Task) this._mre._tcs.Task;
  }
}
