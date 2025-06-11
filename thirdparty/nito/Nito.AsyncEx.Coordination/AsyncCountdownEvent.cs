// Decompiled with JetBrains decompiler
// Type: Nito.AsyncEx.AsyncCountdownEvent
// Assembly: Nito.AsyncEx.Coordination, Version=5.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5843C369-F4A6-4604-9695-EBAC5AF4AB11
// Assembly location: D:\PDFGear\bin\Nito.AsyncEx.Coordination.dll

using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

#nullable enable
namespace Nito.AsyncEx;

[DebuggerDisplay("Id = {Id}, CurrentCount = {_count}")]
[DebuggerTypeProxy(typeof (AsyncCountdownEvent.DebugView))]
public sealed class AsyncCountdownEvent
{
  private readonly AsyncManualResetEvent _mre;
  private long _count;

  public AsyncCountdownEvent(long count)
  {
    this._mre = new AsyncManualResetEvent(count == 0L);
    this._count = count;
  }

  public int Id => this._mre.Id;

  public long CurrentCount
  {
    get
    {
      lock (this._mre)
        return this._count;
    }
  }

  public Task WaitAsync() => this._mre.WaitAsync();

  public Task WaitAsync(CancellationToken cancellationToken)
  {
    return this._mre.WaitAsync(cancellationToken);
  }

  public void Wait() => this._mre.Wait();

  public void Wait(CancellationToken cancellationToken) => this._mre.Wait(cancellationToken);

  private void ModifyCount(long difference, bool add)
  {
    if (difference == 0L)
      return;
    lock (this._mre)
    {
      long count = this._count;
      if (add)
        checked { this._count += difference; }
      else
        checked { this._count -= difference; }
      if (count == 0L)
        this._mre.Reset();
      else if (this._count == 0L)
      {
        this._mre.Set();
      }
      else
      {
        if ((count >= 0L || this._count <= 0L) && (count <= 0L || this._count >= 0L))
          return;
        this._mre.Set();
        this._mre.Reset();
      }
    }
  }

  public void AddCount(long addCount) => this.ModifyCount(addCount, true);

  public void AddCount() => this.AddCount(1L);

  public void Signal(long signalCount) => this.ModifyCount(signalCount, false);

  public void Signal() => this.Signal(1L);

  [DebuggerNonUserCode]
  private sealed class DebugView
  {
    private readonly AsyncCountdownEvent _ce;

    public DebugView(AsyncCountdownEvent ce) => this._ce = ce;

    public int Id => this._ce.Id;

    public long CurrentCount => this._ce.CurrentCount;

    public AsyncManualResetEvent AsyncManualResetEvent => this._ce._mre;
  }
}
