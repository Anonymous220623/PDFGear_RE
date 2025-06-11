// Decompiled with JetBrains decompiler
// Type: NLog.Targets.Wrappers.AsyncRequestQueue
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using NLog.Common;
using NLog.Internal;
using System.Collections.Generic;
using System.Threading;

#nullable disable
namespace NLog.Targets.Wrappers;

internal class AsyncRequestQueue : AsyncRequestQueueBase
{
  private readonly Queue<AsyncLogEventInfo> _logEventInfoQueue = new Queue<AsyncLogEventInfo>(1000);

  public AsyncRequestQueue(int requestLimit, AsyncTargetWrapperOverflowAction overflowAction)
  {
    this.RequestLimit = requestLimit;
    this.OnOverflow = overflowAction;
  }

  public int RequestCount
  {
    get
    {
      lock (this._logEventInfoQueue)
        return this._logEventInfoQueue.Count;
    }
  }

  public override bool IsEmpty => this.RequestCount == 0;

  public override bool Enqueue(AsyncLogEventInfo logEventInfo)
  {
    lock (this._logEventInfoQueue)
    {
      if (this._logEventInfoQueue.Count >= this.RequestLimit)
      {
        InternalLogger.Debug("Async queue is full");
        switch (this.OnOverflow)
        {
          case AsyncTargetWrapperOverflowAction.Grow:
            InternalLogger.Debug("The overflow action is Grow, adding element anyway");
            this.OnLogEventQueueGrows((long) (this.RequestCount + 1));
            this.RequestLimit *= 2;
            break;
          case AsyncTargetWrapperOverflowAction.Discard:
            InternalLogger.Debug("Discarding one element from queue");
            this.OnLogEventDropped(this._logEventInfoQueue.Dequeue().LogEvent);
            break;
          case AsyncTargetWrapperOverflowAction.Block:
            while (this._logEventInfoQueue.Count >= this.RequestLimit)
            {
              InternalLogger.Debug("Blocking because the overflow action is Block...");
              Monitor.Wait((object) this._logEventInfoQueue);
              InternalLogger.Trace("Entered critical section.");
            }
            InternalLogger.Trace("Async queue limit ok.");
            break;
        }
      }
      this._logEventInfoQueue.Enqueue(logEventInfo);
      return this._logEventInfoQueue.Count == 1;
    }
  }

  public override AsyncLogEventInfo[] DequeueBatch(int count)
  {
    AsyncLogEventInfo[] asyncLogEventInfoArray;
    lock (this._logEventInfoQueue)
    {
      if (this._logEventInfoQueue.Count < count)
        count = this._logEventInfoQueue.Count;
      if (count == 0)
        return ArrayHelper.Empty<AsyncLogEventInfo>();
      asyncLogEventInfoArray = new AsyncLogEventInfo[count];
      for (int index = 0; index < count; ++index)
        asyncLogEventInfoArray[index] = this._logEventInfoQueue.Dequeue();
      if (this.OnOverflow == AsyncTargetWrapperOverflowAction.Block)
        Monitor.PulseAll((object) this._logEventInfoQueue);
    }
    return asyncLogEventInfoArray;
  }

  public override void DequeueBatch(int count, IList<AsyncLogEventInfo> result)
  {
    lock (this._logEventInfoQueue)
    {
      if (this._logEventInfoQueue.Count < count)
        count = this._logEventInfoQueue.Count;
      for (int index = 0; index < count; ++index)
        result.Add(this._logEventInfoQueue.Dequeue());
      if (this.OnOverflow != AsyncTargetWrapperOverflowAction.Block)
        return;
      Monitor.PulseAll((object) this._logEventInfoQueue);
    }
  }

  public override void Clear()
  {
    lock (this._logEventInfoQueue)
    {
      this._logEventInfoQueue.Clear();
      if (this.OnOverflow != AsyncTargetWrapperOverflowAction.Block)
        return;
      Monitor.PulseAll((object) this._logEventInfoQueue);
    }
  }
}
