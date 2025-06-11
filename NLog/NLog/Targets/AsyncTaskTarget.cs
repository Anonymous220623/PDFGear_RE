// Decompiled with JetBrains decompiler
// Type: NLog.Targets.AsyncTaskTarget
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using NLog.Common;
using NLog.Internal;
using NLog.Targets.Wrappers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;

#nullable disable
namespace NLog.Targets;

public abstract class AsyncTaskTarget : TargetWithContext
{
  private readonly Timer _taskTimeoutTimer;
  private CancellationTokenSource _cancelTokenSource;
  private AsyncRequestQueueBase _requestQueue;
  private readonly Action _taskCancelledToken;
  private readonly Action<Task, object> _taskCompletion;
  private Task _previousTask;
  private readonly Timer _lazyWriterTimer;
  private readonly ReusableAsyncLogEventList _reusableAsyncLogEventList = new ReusableAsyncLogEventList(200);
  private Tuple<List<LogEventInfo>, List<AsyncContinuation>> _reusableLogEvents;
  private AsyncHelpersTask? _flushEventsInQueueDelegate;
  private bool? _forceLockingQueue;

  [DefaultValue(1)]
  public int TaskDelayMilliseconds { get; set; }

  [DefaultValue(150)]
  public int TaskTimeoutSeconds { get; set; }

  [DefaultValue(0)]
  public int RetryCount { get; set; }

  [DefaultValue(500)]
  public int RetryDelayMilliseconds { get; set; }

  [DefaultValue(false)]
  public bool ForceLockingQueue
  {
    get => this._forceLockingQueue ?? false;
    set => this._forceLockingQueue = new bool?(value);
  }

  [DefaultValue("Discard")]
  public AsyncTargetWrapperOverflowAction OverflowAction
  {
    get => this._requestQueue.OnOverflow;
    set => this._requestQueue.OnOverflow = value;
  }

  [DefaultValue(10000)]
  public int QueueLimit
  {
    get => this._requestQueue.RequestLimit;
    set => this._requestQueue.RequestLimit = value;
  }

  [DefaultValue(1)]
  public int BatchSize { get; set; }

  protected virtual TaskScheduler TaskScheduler => TaskScheduler.Default;

  protected AsyncTaskTarget()
  {
    this.OptimizeBufferReuse = true;
    this.TaskTimeoutSeconds = 150;
    this.TaskDelayMilliseconds = 1;
    this.BatchSize = 1;
    this.RetryDelayMilliseconds = 500;
    this._taskCompletion = new Action<Task, object>(this.TaskCompletion);
    this._taskCancelledToken = new Action(this.TaskCancelledToken);
    this._taskTimeoutTimer = new Timer(new TimerCallback(this.TaskTimeout), (object) null, -1, -1);
    this._requestQueue = (AsyncRequestQueueBase) new AsyncRequestQueue(10000, AsyncTargetWrapperOverflowAction.Discard);
    this._lazyWriterTimer = new Timer((TimerCallback) (s => this.TaskStartNext((object) null, false)), (object) null, -1, -1);
  }

  protected override void InitializeTarget()
  {
    this._cancelTokenSource = new CancellationTokenSource();
    this._cancelTokenSource.Token.Register(this._taskCancelledToken);
    base.InitializeTarget();
    if (this.BatchSize <= 0)
      this.BatchSize = 1;
    if (!this.ForceLockingQueue && this.OverflowAction == AsyncTargetWrapperOverflowAction.Block && (Decimal) this.BatchSize * 1.5M > (Decimal) this.QueueLimit)
      this.ForceLockingQueue = true;
    if (this._forceLockingQueue.HasValue && this._forceLockingQueue.Value != this._requestQueue is AsyncRequestQueue)
      this._requestQueue = this.ForceLockingQueue ? (AsyncRequestQueueBase) new AsyncRequestQueue(this.QueueLimit, this.OverflowAction) : (AsyncRequestQueueBase) new ConcurrentRequestQueue(this.QueueLimit, this.OverflowAction);
    if (this.BatchSize <= this.QueueLimit)
      return;
    this.BatchSize = this.QueueLimit;
  }

  protected abstract Task WriteAsyncTask(LogEventInfo logEvent, CancellationToken cancellationToken);

  protected virtual Task WriteAsyncTask(
    IList<LogEventInfo> logEvents,
    CancellationToken cancellationToken)
  {
    if (logEvents.Count == 1)
      return this.WriteAsyncTask(logEvents[0], cancellationToken);
    Task task = (Task) null;
    for (int index = 0; index < logEvents.Count; ++index)
    {
      LogEventInfo logEvent = logEvents[index];
      task = task != null ? task.ContinueWith<Task>((Func<Task, Task>) (t => this.WriteAsyncTask(logEvent, cancellationToken)), cancellationToken, TaskContinuationOptions.ExecuteSynchronously, this.TaskScheduler).Unwrap() : this.WriteAsyncTask(logEvent, cancellationToken);
    }
    return task;
  }

  protected virtual bool RetryFailedAsyncTask(
    Exception exception,
    CancellationToken cancellationToken,
    int retryCountRemaining,
    out TimeSpan retryDelay)
  {
    if (cancellationToken.IsCancellationRequested || retryCountRemaining < 0)
    {
      retryDelay = TimeSpan.Zero;
      return false;
    }
    retryDelay = TimeSpan.FromMilliseconds((double) (this.RetryDelayMilliseconds * (this.RetryCount - retryCountRemaining) * 2 + this.RetryDelayMilliseconds));
    return true;
  }

  protected override void Write(AsyncLogEventInfo logEvent)
  {
    if (this._cancelTokenSource.IsCancellationRequested)
    {
      logEvent.Continuation((Exception) null);
    }
    else
    {
      this.PrecalculateVolatileLayouts(logEvent.LogEvent);
      if (!this._requestQueue.Enqueue(logEvent))
        return;
      bool lockTaken = false;
      try
      {
        if (this._previousTask == null)
          Monitor.Enter(this.SyncRoot, ref lockTaken);
        else
          Monitor.TryEnter(this.SyncRoot, 50, ref lockTaken);
        if (this._previousTask != null)
          return;
        this._lazyWriterTimer.Change(this.TaskDelayMilliseconds, -1);
      }
      finally
      {
        if (lockTaken)
          Monitor.Exit(this.SyncRoot);
      }
    }
  }

  protected override void WriteAsyncThreadSafe(AsyncLogEventInfo logEvent)
  {
    try
    {
      this.Write(logEvent);
    }
    catch (Exception ex)
    {
      if (this.ExceptionMustBeRethrown(ex))
        throw;
      logEvent.Continuation(ex);
    }
  }

  protected override void FlushAsync(AsyncContinuation asyncContinuation)
  {
    Task previousTask = this._previousTask;
    // ISSUE: explicit non-virtual call
    if ((previousTask != null ? (!__nonvirtual (previousTask.IsCompleted) ? 1 : 0) : 0) != 0 || !this._requestQueue.IsEmpty)
    {
      InternalLogger.Debug<string, string>("{0} Flushing {1}", this.Name, this._requestQueue.IsEmpty ? "empty queue" : "pending queue items");
      if (this._requestQueue.OnOverflow != AsyncTargetWrapperOverflowAction.Block)
      {
        this._requestQueue.Enqueue(new AsyncLogEventInfo((LogEventInfo) null, asyncContinuation));
        this._lazyWriterTimer.Change(0, -1);
      }
      else
      {
        if (!this._flushEventsInQueueDelegate.HasValue)
          this._flushEventsInQueueDelegate = new AsyncHelpersTask?(new AsyncHelpersTask((WaitCallback) (cont =>
          {
            this._requestQueue.Enqueue(new AsyncLogEventInfo((LogEventInfo) null, (AsyncContinuation) cont));
            lock (this.SyncRoot)
              this._lazyWriterTimer.Change(0, -1);
          })));
        AsyncHelpers.StartAsyncTask(this._flushEventsInQueueDelegate.Value, (object) asyncContinuation);
        this._lazyWriterTimer.Change(0, -1);
      }
    }
    else
    {
      InternalLogger.Debug<string>("{0} Flushing Nothing", this.Name);
      asyncContinuation((Exception) null);
    }
  }

  protected override void CloseTarget()
  {
    this._taskTimeoutTimer.Change(-1, -1);
    this._cancelTokenSource.Cancel();
    this._requestQueue.Clear();
    this._previousTask = (Task) null;
    base.CloseTarget();
  }

  protected override void Dispose(bool disposing)
  {
    base.Dispose(disposing);
    if (!disposing)
      return;
    this._cancelTokenSource.Dispose();
    this._taskTimeoutTimer.WaitForDispose(TimeSpan.Zero);
    this._lazyWriterTimer.WaitForDispose(TimeSpan.Zero);
  }

  private void TaskStartNext(object previousTask, bool fullBatchCompleted)
  {
    do
    {
      lock (this.SyncRoot)
      {
        if (this.CheckOtherTask(previousTask))
          break;
        if (!this.IsInitialized)
        {
          this._previousTask = (Task) null;
          break;
        }
        if (previousTask != null && !fullBatchCompleted && this.TaskDelayMilliseconds >= 50 && !this._requestQueue.IsEmpty)
        {
          this._previousTask = (Task) null;
          this._lazyWriterTimer.Change(this.TaskDelayMilliseconds, -1);
          break;
        }
        using (ReusableObjectCreator<IList<AsyncLogEventInfo>>.LockOject lockOject = this._reusableAsyncLogEventList.Allocate())
        {
          IList<AsyncLogEventInfo> result = lockOject.Result;
          this._requestQueue.DequeueBatch(this.BatchSize, result);
          if (result.Count > 0)
          {
            if (this.TaskCreation(result))
              break;
          }
          else
          {
            this._previousTask = (Task) null;
            break;
          }
        }
      }
    }
    while (!this._requestQueue.IsEmpty || previousTask != null);
  }

  private bool CheckOtherTask(object previousTask)
  {
    if (previousTask != null)
    {
      if (this._previousTask != null && previousTask != this._previousTask)
        return true;
    }
    else
    {
      Task previousTask1 = this._previousTask;
      // ISSUE: explicit non-virtual call
      if ((previousTask1 != null ? (!__nonvirtual (previousTask1.IsCompleted) ? 1 : 0) : 0) != 0)
        return true;
    }
    return false;
  }

  internal Task WriteAsyncTaskWithRetry(
    Task firstTask,
    IList<LogEventInfo> logEvents,
    CancellationToken cancellationToken,
    int retryCount)
  {
    TaskCompletionSource<object> tcs = new TaskCompletionSource<object>();
    try
    {
      return firstTask.ContinueWith<Task>((Func<Task, Task>) (t =>
      {
        if (t.IsFaulted || t.IsCanceled)
        {
          if (t.Exception != null)
            tcs.TrySetException((Exception) t.Exception);
          Exception actualException = AsyncTaskTarget.ExtractActualException(t.Exception);
          TimeSpan retryDelay;
          if (this.RetryFailedAsyncTask(actualException, cancellationToken, retryCount - 1, out retryDelay))
          {
            InternalLogger.Warn(actualException, "{0}: Write operation failed. {1} attempts left. Sleep {2} ms", (object) this.Name, (object) retryCount, (object) retryDelay.TotalMilliseconds);
            AsyncHelpers.WaitForDelay(retryDelay);
            if (!cancellationToken.IsCancellationRequested)
            {
              Task firstTask1;
              lock (this.SyncRoot)
                firstTask1 = this.StartWriteAsyncTask(logEvents, cancellationToken);
              if (firstTask1 != null)
                return this.WriteAsyncTaskWithRetry(firstTask1, logEvents, cancellationToken, retryCount - 1);
            }
          }
          InternalLogger.Warn(actualException, "{0}: Write operation failed after {1} retries", (object) this.Name, (object) (this.RetryCount - retryCount));
        }
        else
          tcs.SetResult((object) null);
        return (Task) tcs.Task;
      }), cancellationToken, TaskContinuationOptions.ExecuteSynchronously, this.TaskScheduler).Unwrap();
    }
    catch (Exception ex)
    {
      tcs.SetException(ex);
    }
    return (Task) tcs.Task;
  }

  private bool TaskCreation(IList<AsyncLogEventInfo> logEvents)
  {
    Tuple<List<LogEventInfo>, List<AsyncContinuation>> state = (Tuple<List<LogEventInfo>, List<AsyncContinuation>>) null;
    try
    {
      if (this._cancelTokenSource.IsCancellationRequested)
      {
        for (int index = 0; index < logEvents.Count; ++index)
          logEvents[index].Continuation((Exception) null);
        return false;
      }
      state = Interlocked.CompareExchange<Tuple<List<LogEventInfo>, List<AsyncContinuation>>>(ref this._reusableLogEvents, (Tuple<List<LogEventInfo>, List<AsyncContinuation>>) null, this._reusableLogEvents) ?? Tuple.Create<List<LogEventInfo>, List<AsyncContinuation>>(new List<LogEventInfo>(), new List<AsyncContinuation>());
      for (int index = 0; index < logEvents.Count; ++index)
      {
        if (logEvents[index].LogEvent == null)
        {
          state.Item2.Add(logEvents[index].Continuation);
        }
        else
        {
          state.Item1.Add(logEvents[index].LogEvent);
          state.Item2.Add(logEvents[index].Continuation);
        }
      }
      if (state.Item1.Count == 0)
      {
        this.NotifyTaskCompletion((IList<AsyncContinuation>) state.Item2, (Exception) null);
        state.Item2.Clear();
        Interlocked.CompareExchange<Tuple<List<LogEventInfo>, List<AsyncContinuation>>>(ref this._reusableLogEvents, state, (Tuple<List<LogEventInfo>, List<AsyncContinuation>>) null);
        InternalLogger.Debug<string>("{0} Flush Completed", this.Name);
        return false;
      }
      Task firstTask = this.StartWriteAsyncTask((IList<LogEventInfo>) state.Item1, this._cancelTokenSource.Token);
      if (firstTask == null)
      {
        InternalLogger.Debug<string>("{0} WriteAsyncTask returned null", this.Name);
        this.NotifyTaskCompletion((IList<AsyncContinuation>) state.Item2, (Exception) null);
        return false;
      }
      if (this.RetryCount > 0)
        firstTask = this.WriteAsyncTaskWithRetry(firstTask, (IList<LogEventInfo>) state.Item1, this._cancelTokenSource.Token, this.RetryCount);
      this._previousTask = firstTask;
      if (this.TaskTimeoutSeconds > 0)
        this._taskTimeoutTimer.Change(this.TaskTimeoutSeconds * 1000, -1);
      firstTask.ContinueWith(this._taskCompletion, (object) state, this.TaskScheduler);
      return true;
    }
    catch (Exception ex)
    {
      this._previousTask = (Task) null;
      InternalLogger.Error(ex, "{0} WriteAsyncTask failed on creation", (object) this.Name);
      this.NotifyTaskCompletion((IList<AsyncContinuation>) state?.Item2, ex);
    }
    return false;
  }

  private Task StartWriteAsyncTask(
    IList<LogEventInfo> logEvents,
    CancellationToken cancellationToken)
  {
    try
    {
      Task task = this.WriteAsyncTask(logEvents, cancellationToken);
      if (task != null && task.Status == TaskStatus.Created)
        task.Start(this.TaskScheduler);
      return task;
    }
    catch (Exception ex)
    {
      if (ex.MustBeRethrownImmediately())
        throw;
      InternalLogger.Error(ex, "{0} WriteAsyncTask failed on creation", (object) this.Name);
      return Task.Factory.StartNew((Action<object>) (e =>
      {
        throw (Exception) e;
      }), (object) new AggregateException(new Exception[1]
      {
        ex
      }), this._cancelTokenSource.Token, TaskCreationOptions.None, this.TaskScheduler);
    }
  }

  private void NotifyTaskCompletion(IList<AsyncContinuation> reusableContinuations, Exception ex)
  {
    try
    {
      int index = 0;
      while (true)
      {
        int num = index;
        int? count = reusableContinuations?.Count;
        int valueOrDefault = count.GetValueOrDefault();
        if (num < valueOrDefault & count.HasValue)
        {
          reusableContinuations[index](ex);
          ++index;
        }
        else
          break;
      }
    }
    catch
    {
    }
  }

  private void TaskCompletion(Task completedTask, object continuation)
  {
    bool flag = true;
    bool fullBatchCompleted = true;
    try
    {
      if (completedTask == this._previousTask)
      {
        if (this.TaskTimeoutSeconds > 0)
          this._taskTimeoutTimer.Change(-1, -1);
      }
      else
      {
        flag = false;
        if (!this.IsInitialized)
          return;
      }
      if (continuation is Tuple<List<LogEventInfo>, List<AsyncContinuation>> tuple)
        this.NotifyTaskCompletion((IList<AsyncContinuation>) tuple.Item2, (Exception) null);
      else
        flag = false;
      if (completedTask.IsCanceled)
      {
        flag = false;
        if (completedTask.Exception != null)
          InternalLogger.Warn((Exception) completedTask.Exception, "{0} WriteAsyncTask was cancelled", (object) this.Name);
        else
          InternalLogger.Info<string>("{0} WriteAsyncTask was cancelled", this.Name);
      }
      else if (completedTask.Exception != null)
      {
        Exception actualException = AsyncTaskTarget.ExtractActualException(completedTask.Exception);
        flag = false;
        if (this.RetryCount <= 0)
        {
          TimeSpan retryDelay;
          if (this.RetryFailedAsyncTask(actualException, CancellationToken.None, 0, out retryDelay))
          {
            InternalLogger.Warn(actualException, "{0}: WriteAsyncTask failed on completion. Sleep {1} ms", (object) this.Name, (object) retryDelay.TotalMilliseconds);
            AsyncHelpers.WaitForDelay(retryDelay);
          }
        }
        else
          InternalLogger.Warn(actualException, "{0} WriteAsyncTask failed on completion", (object) this.Name);
      }
      if (!flag || !this.OptimizeBufferReuse)
        return;
      fullBatchCompleted = tuple.Item2.Count * 2 > this.BatchSize;
      tuple.Item1.Clear();
      tuple.Item2.Clear();
      Interlocked.CompareExchange<Tuple<List<LogEventInfo>, List<AsyncContinuation>>>(ref this._reusableLogEvents, tuple, (Tuple<List<LogEventInfo>, List<AsyncContinuation>>) null);
    }
    finally
    {
      this.TaskStartNext((object) completedTask, fullBatchCompleted);
    }
  }

  private void TaskTimeout(object state)
  {
    try
    {
      if (!this.IsInitialized)
        return;
      InternalLogger.Warn<string>("{0} WriteAsyncTask had timeout. Task will be cancelled.", this.Name);
      Task task = this._previousTask;
      try
      {
        lock (this.SyncRoot)
        {
          if (task != null && task == this._previousTask)
          {
            this._previousTask = (Task) null;
            this._cancelTokenSource.Cancel();
          }
          else
            task = (Task) null;
        }
        if (task != null)
        {
          if (task.Status != TaskStatus.Canceled && task.Status != TaskStatus.Faulted && task.Status != TaskStatus.RanToCompletion && !task.Wait(100))
            InternalLogger.Debug<string, TaskStatus>("{0} WriteAsyncTask had timeout. Task did not cancel properly: {1}.", this.Name, task.Status);
          this.RetryFailedAsyncTask(AsyncTaskTarget.ExtractActualException(task.Exception) ?? (Exception) new TimeoutException("WriteAsyncTask had timeout"), CancellationToken.None, 0, out TimeSpan _);
        }
      }
      catch (Exception ex)
      {
        object[] objArray = new object[1]
        {
          (object) this.Name
        };
        InternalLogger.Debug(ex, "{0} WriteAsyncTask had timeout. Task failed to cancel properly.", objArray);
      }
      this.TaskStartNext((object) null, false);
    }
    catch (Exception ex)
    {
      object[] objArray = new object[1]
      {
        (object) this.Name
      };
      InternalLogger.Error(ex, "{0} WriteAsyncTask failed on timeout", objArray);
    }
  }

  private static Exception ExtractActualException(AggregateException taskException)
  {
    ReadOnlyCollection<Exception> innerExceptions = taskException?.Flatten()?.InnerExceptions;
    // ISSUE: explicit non-virtual call
    return innerExceptions == null || __nonvirtual (innerExceptions.Count) != 1 ? (Exception) taskException : innerExceptions[0];
  }

  private void TaskCancelledToken()
  {
    this._cancelTokenSource = new CancellationTokenSource();
    this._cancelTokenSource.Token.Register(this._taskCancelledToken);
  }
}
