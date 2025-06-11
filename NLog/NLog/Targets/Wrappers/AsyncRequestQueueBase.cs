// Decompiled with JetBrains decompiler
// Type: NLog.Targets.Wrappers.AsyncRequestQueueBase
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using NLog.Common;
using System;
using System.Collections.Generic;

#nullable disable
namespace NLog.Targets.Wrappers;

internal abstract class AsyncRequestQueueBase
{
  public abstract bool IsEmpty { get; }

  public int RequestLimit { get; set; }

  public AsyncTargetWrapperOverflowAction OnOverflow { get; set; }

  public event EventHandler<LogEventDroppedEventArgs> LogEventDropped;

  public event EventHandler<LogEventQueueGrowEventArgs> LogEventQueueGrow;

  public abstract bool Enqueue(AsyncLogEventInfo logEventInfo);

  public abstract AsyncLogEventInfo[] DequeueBatch(int count);

  public abstract void DequeueBatch(int count, IList<AsyncLogEventInfo> result);

  public abstract void Clear();

  protected void OnLogEventDropped(LogEventInfo logEventInfo)
  {
    EventHandler<LogEventDroppedEventArgs> logEventDropped = this.LogEventDropped;
    if (logEventDropped == null)
      return;
    logEventDropped((object) this, new LogEventDroppedEventArgs(logEventInfo));
  }

  protected void OnLogEventQueueGrows(long requestsCount)
  {
    EventHandler<LogEventQueueGrowEventArgs> logEventQueueGrow = this.LogEventQueueGrow;
    if (logEventQueueGrow == null)
      return;
    logEventQueueGrow((object) this, new LogEventQueueGrowEventArgs((long) this.RequestLimit, requestsCount));
  }
}
