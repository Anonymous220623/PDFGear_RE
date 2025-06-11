// Decompiled with JetBrains decompiler
// Type: NLog.Common.AsyncLogEventInfo
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

#nullable disable
namespace NLog.Common;

public struct AsyncLogEventInfo(LogEventInfo logEvent, AsyncContinuation continuation)
{
  public LogEventInfo LogEvent { get; } = logEvent;

  public AsyncContinuation Continuation { get; } = continuation;

  public static bool operator ==(AsyncLogEventInfo eventInfo1, AsyncLogEventInfo eventInfo2)
  {
    return eventInfo1.Continuation == eventInfo2.Continuation && eventInfo1.LogEvent == eventInfo2.LogEvent;
  }

  public static bool operator !=(AsyncLogEventInfo eventInfo1, AsyncLogEventInfo eventInfo2)
  {
    return eventInfo1.Continuation != eventInfo2.Continuation || eventInfo1.LogEvent != eventInfo2.LogEvent;
  }

  public override bool Equals(object obj) => this == (AsyncLogEventInfo) obj;

  public override int GetHashCode()
  {
    return this.LogEvent.GetHashCode() ^ this.Continuation.GetHashCode();
  }
}
