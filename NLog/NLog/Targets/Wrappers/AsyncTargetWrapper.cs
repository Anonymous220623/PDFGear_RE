// Decompiled with JetBrains decompiler
// Type: NLog.Targets.Wrappers.AsyncTargetWrapper
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using NLog.Common;
using NLog.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;

#nullable disable
namespace NLog.Targets.Wrappers;

[Target("AsyncWrapper", IsWrapper = true)]
public class AsyncTargetWrapper : WrapperTargetBase
{
  private readonly object _writeLockObject = new object();
  private readonly object _timerLockObject = new object();
  private Timer _lazyWriterTimer;
  private readonly ReusableAsyncLogEventList _reusableAsyncLogEventList = new ReusableAsyncLogEventList(200);
  private bool? _forceLockingQueue;
  private AsyncRequestQueueBase _requestQueue;
  private AsyncHelpersTask? _flushEventsInQueueDelegate;

  private event EventHandler<LogEventDroppedEventArgs> _logEventDroppedEvent;

  private event EventHandler<LogEventQueueGrowEventArgs> _eventQueueGrowEvent;

  public AsyncTargetWrapper()
    : this((Target) null)
  {
  }

  public AsyncTargetWrapper(string name, Target wrappedTarget)
    : this(wrappedTarget)
  {
    this.Name = name;
  }

  public AsyncTargetWrapper(Target wrappedTarget)
    : this(wrappedTarget, 10000, AsyncTargetWrapperOverflowAction.Discard)
  {
  }

  public AsyncTargetWrapper(
    Target wrappedTarget,
    int queueLimit,
    AsyncTargetWrapperOverflowAction overflowAction)
  {
    this._requestQueue = (AsyncRequestQueueBase) new AsyncRequestQueue(10000, AsyncTargetWrapperOverflowAction.Discard);
    this.TimeToSleepBetweenBatches = 1;
    this.BatchSize = 200;
    this.FullBatchSizeWriteLimit = 5;
    this.WrappedTarget = wrappedTarget;
    this.QueueLimit = queueLimit;
    this.OverflowAction = overflowAction;
  }

  [DefaultValue(200)]
  public int BatchSize { get; set; }

  [DefaultValue(1)]
  public int TimeToSleepBetweenBatches { get; set; }

  public event EventHandler<LogEventDroppedEventArgs> LogEventDropped
  {
    add
    {
      if (this._logEventDroppedEvent == null && this._requestQueue != null)
        this._requestQueue.LogEventDropped += new EventHandler<LogEventDroppedEventArgs>(this.OnRequestQueueDropItem);
      this._logEventDroppedEvent += value;
    }
    remove
    {
      this._logEventDroppedEvent -= value;
      if (this._logEventDroppedEvent != null || this._requestQueue == null)
        return;
      this._requestQueue.LogEventDropped -= new EventHandler<LogEventDroppedEventArgs>(this.OnRequestQueueDropItem);
    }
  }

  public event EventHandler<LogEventQueueGrowEventArgs> EventQueueGrow
  {
    add
    {
      if (this._eventQueueGrowEvent == null && this._requestQueue != null)
        this._requestQueue.LogEventQueueGrow += new EventHandler<LogEventQueueGrowEventArgs>(this.OnRequestQueueGrow);
      this._eventQueueGrowEvent += value;
    }
    remove
    {
      this._eventQueueGrowEvent -= value;
      if (this._eventQueueGrowEvent != null || this._requestQueue == null)
        return;
      this._requestQueue.LogEventQueueGrow -= new EventHandler<LogEventQueueGrowEventArgs>(this.OnRequestQueueGrow);
    }
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

  [DefaultValue(5)]
  public int FullBatchSizeWriteLimit { get; set; }

  [DefaultValue(false)]
  public bool ForceLockingQueue
  {
    get => this._forceLockingQueue ?? false;
    set => this._forceLockingQueue = new bool?(value);
  }

  protected override void FlushAsync(AsyncContinuation asyncContinuation)
  {
    if (!this._flushEventsInQueueDelegate.HasValue)
      this._flushEventsInQueueDelegate = new AsyncHelpersTask?(new AsyncHelpersTask(new WaitCallback(this.FlushEventsInQueue)));
    AsyncHelpers.StartAsyncTask(this._flushEventsInQueueDelegate.Value, (object) asyncContinuation);
  }

  protected override void InitializeTarget()
  {
    base.InitializeTarget();
    if (!this.OptimizeBufferReuse && this.WrappedTarget != null && this.WrappedTarget.OptimizeBufferReuse)
    {
      this.OptimizeBufferReuse = this.GetType() == typeof (AsyncTargetWrapper);
      if (!this.OptimizeBufferReuse && !this.ForceLockingQueue)
        this.ForceLockingQueue = true;
    }
    if (!this.ForceLockingQueue && this.OverflowAction == AsyncTargetWrapperOverflowAction.Block && (Decimal) this.BatchSize * 1.5M > (Decimal) this.QueueLimit)
      this.ForceLockingQueue = true;
    if (this._forceLockingQueue.HasValue && this._forceLockingQueue.Value != this._requestQueue is AsyncRequestQueue)
      this._requestQueue = this.ForceLockingQueue ? (AsyncRequestQueueBase) new AsyncRequestQueue(this.QueueLimit, this.OverflowAction) : (AsyncRequestQueueBase) new ConcurrentRequestQueue(this.QueueLimit, this.OverflowAction);
    if (this.BatchSize > this.QueueLimit && this.TimeToSleepBetweenBatches <= 1)
      this.BatchSize = this.QueueLimit;
    this._requestQueue.Clear();
    InternalLogger.Trace<string>("AsyncWrapper(Name={0}): Start Timer", this.Name);
    this._lazyWriterTimer = new Timer(new TimerCallback(this.ProcessPendingEvents), (object) null, -1, -1);
    this.StartLazyWriterTimer();
  }

  protected override void CloseTarget()
  {
    this.StopLazyWriterThread();
    if (Monitor.TryEnter(this._writeLockObject, 500))
    {
      try
      {
        this.WriteEventsInQueue(int.MaxValue, "Closing Target");
      }
      finally
      {
        Monitor.Exit(this._writeLockObject);
      }
    }
    if (this.OverflowAction == AsyncTargetWrapperOverflowAction.Block)
      this._requestQueue.Clear();
    base.CloseTarget();
  }

  protected virtual void StartLazyWriterTimer()
  {
    lock (this._timerLockObject)
    {
      if (this._lazyWriterTimer == null)
        return;
      if (this.TimeToSleepBetweenBatches <= 1)
      {
        InternalLogger.Trace<string>("AsyncWrapper(Name={0}): Throttled timer scheduled", this.Name);
        this._lazyWriterTimer.Change(1, -1);
      }
      else
        this._lazyWriterTimer.Change(this.TimeToSleepBetweenBatches, -1);
    }
  }

  protected virtual bool StartInstantWriterTimer() => this.StartTimerUnlessWriterActive(true);

  private bool StartTimerUnlessWriterActive(bool instant)
  {
    bool flag = false;
    try
    {
      flag = Monitor.TryEnter(this._writeLockObject);
      if (flag)
      {
        if (instant)
        {
          lock (this._timerLockObject)
          {
            if (this._lazyWriterTimer != null)
            {
              this._lazyWriterTimer.Change(0, -1);
              return true;
            }
          }
        }
        else
        {
          this.StartLazyWriterTimer();
          return true;
        }
      }
    }
    finally
    {
      if (flag)
        Monitor.Exit(this._writeLockObject);
    }
    return false;
  }

  protected virtual void StopLazyWriterThread()
  {
    lock (this._timerLockObject)
    {
      Timer lazyWriterTimer = this._lazyWriterTimer;
      if (lazyWriterTimer == null)
        return;
      this._lazyWriterTimer = (Timer) null;
      lazyWriterTimer.WaitForDispose(TimeSpan.FromSeconds(1.0));
    }
  }

  protected override void Write(AsyncLogEventInfo logEvent)
  {
    this.PrecalculateVolatileLayouts(logEvent.LogEvent);
    if (!this._requestQueue.Enqueue(logEvent))
      return;
    if (this.TimeToSleepBetweenBatches == 0)
    {
      this.StartInstantWriterTimer();
    }
    else
    {
      if (this.TimeToSleepBetweenBatches > 1)
        return;
      this.StartLazyWriterTimer();
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

  private void ProcessPendingEvents(object state)
  {
    if (this._lazyWriterTimer == null)
      return;
    bool flag = false;
    try
    {
      lock (this._writeLockObject)
      {
        if (this.WriteEventsInQueue(this.BatchSize, "Timer") == this.BatchSize)
          flag = true;
        if (!flag || this.TimeToSleepBetweenBatches > 1)
          return;
        this.StartInstantWriterTimer();
      }
    }
    catch (Exception ex)
    {
      flag = false;
      InternalLogger.Error(ex, "AsyncWrapper(Name={0}): Error in lazy writer timer procedure.", (object) this.Name);
      if (!ex.MustBeRethrownImmediately())
        return;
      throw;
    }
    finally
    {
      if (this.TimeToSleepBetweenBatches <= 1)
      {
        if (!flag && !this._requestQueue.IsEmpty)
          this.StartTimerUnlessWriterActive(false);
      }
      else
        this.StartLazyWriterTimer();
    }
  }

  private void FlushEventsInQueue(object state)
  {
    try
    {
      AsyncContinuation asyncContinuation = state as AsyncContinuation;
      lock (this._writeLockObject)
      {
        this.WriteEventsInQueue(int.MaxValue, "Flush Async");
        if (asyncContinuation != null)
          base.FlushAsync(asyncContinuation);
      }
      if (this.TimeToSleepBetweenBatches > 1 || this._requestQueue.IsEmpty)
        return;
      this.StartTimerUnlessWriterActive(false);
    }
    catch (Exception ex)
    {
      InternalLogger.Error(ex, "AsyncWrapper(Name={0}): Error in flush procedure.", (object) this.Name);
      if (!ex.MustBeRethrownImmediately())
        return;
      throw;
    }
  }

  private int WriteEventsInQueue(int batchSize, string reason)
  {
    if (this.WrappedTarget == null)
    {
      InternalLogger.Error<string>("AsyncWrapper(Name={0}): WrappedTarget is NULL", this.Name);
      return 0;
    }
    int num = 0;
    for (int index = 0; index < this.FullBatchSizeWriteLimit; ++index)
    {
      if (!this.OptimizeBufferReuse || batchSize == int.MaxValue)
      {
        AsyncLogEventInfo[] asyncLogEventInfoArray = this._requestQueue.DequeueBatch(batchSize);
        if (asyncLogEventInfoArray.Length != 0)
        {
          if (reason != null)
            InternalLogger.Trace<string, int, string>("AsyncWrapper(Name={0}): Writing {1} events ({2})", this.Name, asyncLogEventInfoArray.Length, reason);
          this.WrappedTarget.WriteAsyncLogEvents(asyncLogEventInfoArray);
        }
        num = asyncLogEventInfoArray.Length;
      }
      else
      {
        using (ReusableObjectCreator<IList<AsyncLogEventInfo>>.LockOject lockOject = this._reusableAsyncLogEventList.Allocate())
        {
          IList<AsyncLogEventInfo> result = lockOject.Result;
          this._requestQueue.DequeueBatch(batchSize, result);
          if (result.Count > 0)
          {
            if (reason != null)
              InternalLogger.Trace<string, int, string>("AsyncWrapper(Name={0}): Writing {1} events ({2})", this.Name, result.Count, reason);
            this.WrappedTarget.WriteAsyncLogEvents(result);
          }
          num = result.Count;
        }
      }
      if (num < batchSize)
        break;
    }
    return num;
  }

  private void OnRequestQueueDropItem(
    object sender,
    LogEventDroppedEventArgs logEventDroppedEventArgs)
  {
    EventHandler<LogEventDroppedEventArgs> eventDroppedEvent = this._logEventDroppedEvent;
    if (eventDroppedEvent == null)
      return;
    eventDroppedEvent((object) this, logEventDroppedEventArgs);
  }

  private void OnRequestQueueGrow(
    object sender,
    LogEventQueueGrowEventArgs logEventQueueGrowEventArgs)
  {
    EventHandler<LogEventQueueGrowEventArgs> eventQueueGrowEvent = this._eventQueueGrowEvent;
    if (eventQueueGrowEvent == null)
      return;
    eventQueueGrowEvent((object) this, logEventQueueGrowEventArgs);
  }
}
