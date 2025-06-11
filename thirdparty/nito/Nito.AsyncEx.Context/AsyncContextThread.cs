// Decompiled with JetBrains decompiler
// Type: Nito.AsyncEx.AsyncContextThread
// Assembly: Nito.AsyncEx.Context, Version=5.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AE3F77A0-CC84-476F-AEB8-35E8B2BFA4C2
// Assembly location: D:\PDFGear\bin\Nito.AsyncEx.Context.dll

using Nito.Disposables;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

#nullable enable
namespace Nito.AsyncEx;

[DebuggerTypeProxy(typeof (AsyncContextThread.DebugView))]
public sealed class AsyncContextThread : SingleDisposable<AsyncContext>
{
  private readonly Task _thread;

  private static AsyncContext CreateAsyncContext()
  {
    AsyncContext asyncContext = new AsyncContext();
    asyncContext.SynchronizationContext.OperationStarted();
    return asyncContext;
  }

  private AsyncContextThread(AsyncContext context)
    : base(context)
  {
    this.Context = context;
    this._thread = Task.Factory.StartNew(new Action(this.Execute), CancellationToken.None, TaskCreationOptions.LongRunning | TaskCreationOptions.DenyChildAttach, TaskScheduler.Default);
  }

  public AsyncContextThread()
    : this(AsyncContextThread.CreateAsyncContext())
  {
  }

  public AsyncContext Context { get; }

  private void Execute()
  {
    using (this.Context)
      this.Context.Execute();
  }

  private void AllowThreadToExit() => this.Context.SynchronizationContext.OperationCompleted();

  public Task JoinAsync()
  {
    this.Dispose();
    return this._thread;
  }

  public void Join() => this.JoinAsync().WaitAndUnwrapException();

  protected override void Dispose(AsyncContext context) => this.AllowThreadToExit();

  public TaskFactory Factory => this.Context.Factory;

  [DebuggerNonUserCode]
  internal sealed class DebugView
  {
    private readonly AsyncContextThread _thread;

    public DebugView(AsyncContextThread thread) => this._thread = thread;

    public AsyncContext Context => this._thread.Context;

    public object Thread => (object) this._thread._thread;
  }
}
