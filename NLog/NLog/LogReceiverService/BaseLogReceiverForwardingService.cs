// Decompiled with JetBrains decompiler
// Type: NLog.LogReceiverService.BaseLogReceiverForwardingService
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using System;

#nullable disable
namespace NLog.LogReceiverService;

public abstract class BaseLogReceiverForwardingService
{
  private readonly LogFactory _logFactory;

  protected BaseLogReceiverForwardingService()
    : this((LogFactory) null)
  {
  }

  protected BaseLogReceiverForwardingService(LogFactory logFactory)
  {
    this._logFactory = logFactory;
  }

  public void ProcessLogMessages(NLogEvents events)
  {
    DateTime dateTime = new DateTime(events.BaseTimeUtc, DateTimeKind.Utc);
    LogEventInfo[] logEvents = new LogEventInfo[events.Events.Length];
    for (int index1 = 0; index1 < events.Events.Length; ++index1)
    {
      NLogEvent nlogEvent = events.Events[index1];
      NLog.LogLevel logLevel = NLog.LogLevel.FromOrdinal(nlogEvent.LevelOrdinal);
      string str = events.Strings[nlogEvent.LoggerOrdinal];
      LogEventInfo logEventInfo = new LogEventInfo();
      logEventInfo.Level = logLevel;
      logEventInfo.LoggerName = str;
      logEventInfo.TimeStamp = dateTime.AddTicks(nlogEvent.TimeDelta).ToLocalTime();
      logEventInfo.Message = events.Strings[nlogEvent.MessageOrdinal];
      logEventInfo.Properties.Add((object) "ClientName", (object) events.ClientName);
      for (int index2 = 0; index2 < events.LayoutNames.Count; ++index2)
        logEventInfo.Properties.Add((object) events.LayoutNames[index2], (object) events.Strings[nlogEvent.ValueIndexes[index2]]);
      logEvents[index1] = logEventInfo;
    }
    this.ProcessLogMessages(logEvents);
  }

  protected virtual void ProcessLogMessages(LogEventInfo[] logEvents)
  {
    ILogger logger = (ILogger) null;
    string str = string.Empty;
    foreach (LogEventInfo logEvent in logEvents)
    {
      if (logEvent.LoggerName != str)
      {
        logger = this._logFactory == null ? (ILogger) LogManager.GetLogger(logEvent.LoggerName) : (ILogger) this._logFactory.GetLogger(logEvent.LoggerName);
        str = logEvent.LoggerName;
      }
      logger?.Log(logEvent);
    }
  }
}
