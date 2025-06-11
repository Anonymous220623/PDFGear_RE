// Decompiled with JetBrains decompiler
// Type: Nito.AsyncEx.AsyncContext
// Assembly: Nito.AsyncEx.Context, Version=5.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: AE3F77A0-CC84-476F-AEB8-35E8B2BFA4C2
// Assembly location: D:\PDFGear\bin\Nito.AsyncEx.Context.dll

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

#nullable enable
namespace Nito.AsyncEx;

[DebuggerDisplay("Id = {Id}, OperationCount = {_outstandingOperations}")]
[DebuggerTypeProxy(typeof (AsyncContext.DebugView))]
public sealed class AsyncContext : IDisposable
{
  private readonly AsyncContext.TaskQueue _queue;
  private readonly AsyncContext.AsyncContextSynchronizationContext _synchronizationContext;
  private readonly AsyncContext.AsyncContextTaskScheduler _taskScheduler;
  private readonly TaskFactory _taskFactory;
  private int _outstandingOperations;

  [EditorBrowsable(EditorBrowsableState.Never)]
  public AsyncContext()
  {
    this._queue = new AsyncContext.TaskQueue();
    this._synchronizationContext = new AsyncContext.AsyncContextSynchronizationContext(this);
    this._taskScheduler = new AsyncContext.AsyncContextTaskScheduler(this);
    this._taskFactory = new TaskFactory(CancellationToken.None, TaskCreationOptions.HideScheduler, TaskContinuationOptions.HideScheduler, (TaskScheduler) this._taskScheduler);
  }

  public int Id => this._taskScheduler.Id;

  private void OperationStarted() => Interlocked.Increment(ref this._outstandingOperations);

  private void OperationCompleted()
  {
    if (Interlocked.Decrement(ref this._outstandingOperations) != 0)
      return;
    this._queue.CompleteAdding();
  }

  private void Enqueue(Task task, bool propagateExceptions)
  {
    this.OperationStarted();
    task.ContinueWith((Action<Task>) (_ => this.OperationCompleted()), CancellationToken.None, TaskContinuationOptions.ExecuteSynchronously, (TaskScheduler) this._taskScheduler);
    this._queue.TryAdd(task, propagateExceptions);
  }

  public void Dispose() => this._queue.Dispose();

  [EditorBrowsable(EditorBrowsableState.Never)]
  public void Execute()
  {
    SynchronizationContextSwitcher.ApplyContext((SynchronizationContext) this._synchronizationContext, (Action) (() =>
    {
      foreach (Tuple<Task, bool> consuming in this._queue.GetConsumingEnumerable())
      {
        this._taskScheduler.DoTryExecuteTask(consuming.Item1);
        if (consuming.Item2)
          consuming.Item1.WaitAndUnwrapException();
      }
    }));
  }

  public static void Run(Action action)
  {
    if (action == null)
      throw new ArgumentNullException(nameof (action));
    using (AsyncContext asyncContext = new AsyncContext())
    {
      Task task = asyncContext._taskFactory.Run(action);
      asyncContext.Execute();
      task.WaitAndUnwrapException();
    }
  }

  public static TResult Run<TResult>(Func<TResult> action)
  {
    if (action == null)
      throw new ArgumentNullException(nameof (action));
    using (AsyncContext asyncContext = new AsyncContext())
    {
      Task<TResult> task = asyncContext._taskFactory.Run<TResult>(action);
      asyncContext.Execute();
      return task.WaitAndUnwrapException<TResult>();
    }
  }

  public static void Run(Func<Task> action)
  {
    if (action == null)
      throw new ArgumentNullException(nameof (action));
    using (AsyncContext context = new AsyncContext())
    {
      context.OperationStarted();
      Task task = context._taskFactory.Run(action).ContinueWith((Action<Task>) (t =>
      {
        context.OperationCompleted();
        t.WaitAndUnwrapException();
      }), CancellationToken.None, TaskContinuationOptions.ExecuteSynchronously, (TaskScheduler) context._taskScheduler);
      context.Execute();
      task.WaitAndUnwrapException();
    }
  }

  public static TResult Run<TResult>(Func<Task<TResult>> action)
  {
    if (action == null)
      throw new ArgumentNullException(nameof (action));
    using (AsyncContext context = new AsyncContext())
    {
      context.OperationStarted();
      Task<TResult> task = context._taskFactory.Run<TResult>(action).ContinueWith<TResult>((Func<Task<TResult>, TResult>) (t =>
      {
        context.OperationCompleted();
        return t.WaitAndUnwrapException<TResult>();
      }), CancellationToken.None, TaskContinuationOptions.ExecuteSynchronously, (TaskScheduler) context._taskScheduler);
      context.Execute();
      return task.WaitAndUnwrapException<TResult>();
    }
  }

  public static AsyncContext? Current
  {
    get
    {
      return !(SynchronizationContext.Current is AsyncContext.AsyncContextSynchronizationContext current) ? (AsyncContext) null : current.Context;
    }
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  public SynchronizationContext SynchronizationContext
  {
    get => (SynchronizationContext) this._synchronizationContext;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  public TaskScheduler Scheduler => (TaskScheduler) this._taskScheduler;

  [EditorBrowsable(EditorBrowsableState.Never)]
  public TaskFactory Factory => this._taskFactory;

  [DebuggerNonUserCode]
  internal sealed class DebugView
  {
    private readonly AsyncContext _context;

    public DebugView(AsyncContext context) => this._context = context;

    public TaskScheduler TaskScheduler => (TaskScheduler) this._context._taskScheduler;
  }

  private sealed class AsyncContextSynchronizationContext : SynchronizationContext
  {
    private readonly AsyncContext _context;

    public AsyncContextSynchronizationContext(AsyncContext context) => this._context = context;

    public AsyncContext Context => this._context;

    public override void Post(SendOrPostCallback d, object state)
    {
      this._context.Enqueue(this._context._taskFactory.Run((Action) (() => d(state))), true);
    }

    public override void Send(SendOrPostCallback d, object state)
    {
      if (AsyncContext.Current == this._context)
        d(state);
      else
        this._context._taskFactory.Run((Action) (() => d(state))).WaitAndUnwrapException();
    }

    public override void OperationStarted() => this._context.OperationStarted();

    public override void OperationCompleted() => this._context.OperationCompleted();

    public override SynchronizationContext CreateCopy()
    {
      return (SynchronizationContext) new AsyncContext.AsyncContextSynchronizationContext(this._context);
    }

    public override int GetHashCode() => this._context.GetHashCode();

    public override bool Equals(object obj)
    {
      return obj is AsyncContext.AsyncContextSynchronizationContext synchronizationContext && this._context == synchronizationContext._context;
    }
  }

  private sealed class TaskQueue : IDisposable
  {
    private readonly BlockingCollection<Tuple<Task, bool>> _queue;

    public TaskQueue() => this._queue = new BlockingCollection<Tuple<Task, bool>>();

    public IEnumerable<Tuple<Task, bool>> GetConsumingEnumerable()
    {
      return this._queue.GetConsumingEnumerable();
    }

    [DebuggerNonUserCode]
    internal IEnumerable<Task> GetScheduledTasks()
    {
      foreach (Tuple<Task, bool> tuple in (IEnumerable<Tuple<Task, bool>>) this._queue)
        yield return tuple.Item1;
    }

    public bool TryAdd(Task item, bool propagateExceptions)
    {
      try
      {
        return this._queue.TryAdd(Tuple.Create<Task, bool>(item, propagateExceptions));
      }
      catch (InvalidOperationException ex)
      {
        return false;
      }
    }

    public void CompleteAdding() => this._queue.CompleteAdding();

    public void Dispose() => this._queue.Dispose();
  }

  private sealed class AsyncContextTaskScheduler : TaskScheduler
  {
    private readonly AsyncContext _context;

    public AsyncContextTaskScheduler(AsyncContext context) => this._context = context;

    [DebuggerNonUserCode]
    protected override IEnumerable<Task> GetScheduledTasks()
    {
      return this._context._queue.GetScheduledTasks();
    }

    protected override void QueueTask(Task task) => this._context.Enqueue(task, false);

    protected override bool TryExecuteTaskInline(Task task, bool taskWasPreviouslyQueued)
    {
      return AsyncContext.Current == this._context && this.TryExecuteTask(task);
    }

    public override int MaximumConcurrencyLevel => 1;

    public void DoTryExecuteTask(Task task) => this.TryExecuteTask(task);
  }
}
