// Decompiled with JetBrains decompiler
// Type: NLog.Targets.Wrappers.ConcurrentRequestQueue
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using NLog.Common;
using NLog.Internal;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;

#nullable disable
namespace NLog.Targets.Wrappers;

internal class ConcurrentRequestQueue : AsyncRequestQueueBase
{
  private readonly ConcurrentQueue<AsyncLogEventInfo> _logEventInfoQueue = new ConcurrentQueue<AsyncLogEventInfo>();
  private long _count;

  public ConcurrentRequestQueue(int requestLimit, AsyncTargetWrapperOverflowAction overflowAction)
  {
    this.RequestLimit = requestLimit;
    this.OnOverflow = overflowAction;
  }

  public override bool IsEmpty
  {
    get => this._logEventInfoQueue.IsEmpty && Interlocked.Read(ref this._count) == 0L;
  }

  public int Count => (int) this._count;

  public override bool Enqueue(AsyncLogEventInfo logEventInfo)
  {
    long requestsCount = Interlocked.Increment(ref this._count);
    bool flag = requestsCount == 1L;
    if (requestsCount > (long) this.RequestLimit)
    {
      InternalLogger.Debug("Async queue is full");
      switch (this.OnOverflow)
      {
        case AsyncTargetWrapperOverflowAction.Grow:
          InternalLogger.Debug("The overflow action is Grow, adding element anyway");
          this.OnLogEventQueueGrows(requestsCount);
          this.RequestLimit *= 2;
          break;
        case AsyncTargetWrapperOverflowAction.Discard:
          AsyncLogEventInfo result;
          while (!this._logEventInfoQueue.TryDequeue(out result))
          {
            long num = Interlocked.Read(ref this._count);
            flag = true;
            if (num <= (long) this.RequestLimit)
              goto label_7;
          }
          InternalLogger.Debug("Discarding one element from queue");
          flag = Interlocked.Decrement(ref this._count) == 1L | flag;
          this.OnLogEventDropped(result.LogEvent);
          break;
        case AsyncTargetWrapperOverflowAction.Block:
          this.WaitForBelowRequestLimit();
          flag = true;
          break;
      }
    }
label_7:
    this._logEventInfoQueue.Enqueue(logEventInfo);
    return flag;
  }

  private void WaitForBelowRequestLimit()
  {
    if (this.TrySpinWaitForLowerCount() > (long) this.RequestLimit)
    {
      InternalLogger.Debug("Blocking because the overflow action is Block...");
      lock (this._logEventInfoQueue)
      {
        InternalLogger.Trace("Entered critical section.");
        for (long index = Interlocked.Read(ref this._count); index > (long) this.RequestLimit; index = Interlocked.Increment(ref this._count))
        {
          Interlocked.Decrement(ref this._count);
          Monitor.Wait((object) this._logEventInfoQueue);
          InternalLogger.Trace("Entered critical section.");
        }
      }
    }
    InternalLogger.Trace("Async queue limit ok.");
  }

  private long TrySpinWaitForLowerCount()
  {
    long num = 0;
    bool flag = true;
    SpinWait spinWait = new SpinWait();
    for (int index = 0; index <= 20; ++index)
    {
      if (spinWait.NextSpinWillYield)
      {
        if (flag)
          InternalLogger.Debug("Yielding because the overflow action is Block...");
        flag = false;
      }
      spinWait.SpinOnce();
      num = Interlocked.Read(ref this._count);
      if (num <= (long) this.RequestLimit)
        break;
    }
    return num;
  }

  public override AsyncLogEventInfo[] DequeueBatch(int count)
  {
    if (this._logEventInfoQueue.IsEmpty)
      return ArrayHelper.Empty<AsyncLogEventInfo>();
    if (this._count < (long) count)
      count = Math.Min(count, this.Count);
    List<AsyncLogEventInfo> result = new List<AsyncLogEventInfo>(count);
    this.DequeueBatch(count, (IList<AsyncLogEventInfo>) result);
    return result.Count == 0 ? ArrayHelper.Empty<AsyncLogEventInfo>() : result.ToArray();
  }

  public override void DequeueBatch(int count, IList<AsyncLogEventInfo> result)
  {
    bool flag = this.OnOverflow == AsyncTargetWrapperOverflowAction.Block;
    for (int index = 0; index < count; ++index)
    {
      AsyncLogEventInfo result1;
      if (this._logEventInfoQueue.TryDequeue(out result1))
      {
        if (!flag)
          Interlocked.Decrement(ref this._count);
        result.Add(result1);
      }
      else
      {
        count = index;
        break;
      }
    }
    if (!flag)
      return;
    lock (this._logEventInfoQueue)
    {
      Interlocked.Add(ref this._count, (long) -count);
      Monitor.PulseAll((object) this._logEventInfoQueue);
    }
  }

  public override void Clear()
  {
    while (!this._logEventInfoQueue.IsEmpty)
      this._logEventInfoQueue.TryDequeue(out AsyncLogEventInfo _);
    Interlocked.Exchange(ref this._count, 0L);
    if (this.OnOverflow != AsyncTargetWrapperOverflowAction.Block)
      return;
    lock (this._logEventInfoQueue)
      Monitor.PulseAll((object) this._logEventInfoQueue);
  }
}
