// Decompiled with JetBrains decompiler
// Type: Nito.AsyncEx.AsyncMonitor
// Assembly: Nito.AsyncEx.Coordination, Version=5.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 5843C369-F4A6-4604-9695-EBAC5AF4AB11
// Assembly location: D:\PDFGear\bin\Nito.AsyncEx.Coordination.dll

using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

#nullable enable
namespace Nito.AsyncEx;

[DebuggerDisplay("Id = {Id}, ConditionVariableId = {_conditionVariable.Id}")]
public sealed class AsyncMonitor
{
  private readonly AsyncLock _asyncLock;
  private readonly AsyncConditionVariable _conditionVariable;

  internal AsyncMonitor(
    IAsyncWaitQueue<IDisposable>? lockQueue,
    IAsyncWaitQueue<object>? conditionVariableQueue)
  {
    this._asyncLock = new AsyncLock(lockQueue);
    this._conditionVariable = new AsyncConditionVariable(this._asyncLock, conditionVariableQueue);
  }

  public AsyncMonitor()
    : this((IAsyncWaitQueue<IDisposable>) null, (IAsyncWaitQueue<object>) null)
  {
  }

  public int Id => this._asyncLock.Id;

  public AwaitableDisposable<IDisposable> EnterAsync(CancellationToken cancellationToken)
  {
    return this._asyncLock.LockAsync(cancellationToken);
  }

  public AwaitableDisposable<IDisposable> EnterAsync() => this.EnterAsync(CancellationToken.None);

  public IDisposable Enter(CancellationToken cancellationToken)
  {
    return this._asyncLock.Lock(cancellationToken);
  }

  public IDisposable Enter() => this.Enter(CancellationToken.None);

  public Task WaitAsync(CancellationToken cancellationToken)
  {
    return this._conditionVariable.WaitAsync(cancellationToken);
  }

  public Task WaitAsync() => this.WaitAsync(CancellationToken.None);

  public void Wait(CancellationToken cancellationToken)
  {
    this._conditionVariable.Wait(cancellationToken);
  }

  public void Wait() => this.Wait(CancellationToken.None);

  public void Pulse() => this._conditionVariable.Notify();

  public void PulseAll() => this._conditionVariable.NotifyAll();
}
