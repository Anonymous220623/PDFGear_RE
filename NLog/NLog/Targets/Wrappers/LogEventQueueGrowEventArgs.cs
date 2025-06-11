// Decompiled with JetBrains decompiler
// Type: NLog.Targets.Wrappers.LogEventQueueGrowEventArgs
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using System;

#nullable disable
namespace NLog.Targets.Wrappers;

public class LogEventQueueGrowEventArgs : EventArgs
{
  public LogEventQueueGrowEventArgs(long newQueueSize, long requestsCount)
  {
    this.NewQueueSize = newQueueSize;
    this.RequestsCount = requestsCount;
  }

  public long NewQueueSize { get; }

  public long RequestsCount { get; }
}
