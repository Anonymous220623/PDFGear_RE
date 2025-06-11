// Decompiled with JetBrains decompiler
// Type: NLog.Targets.LogReceiverWebServiceTarget
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using NLog.Common;
using NLog.Config;
using NLog.Internal;
using NLog.Layouts;
using NLog.LogReceiverService;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.ServiceModel;
using System.ServiceModel.Channels;

#nullable disable
namespace NLog.Targets;

[Target("LogReceiverService")]
public class LogReceiverWebServiceTarget : Target
{
  private readonly LogEventInfoBuffer buffer = new LogEventInfoBuffer(10000, false, 10000);
  private bool inCall;

  public LogReceiverWebServiceTarget()
  {
    this.Parameters = (IList<MethodCallParameter>) new List<MethodCallParameter>();
  }

  public LogReceiverWebServiceTarget(string name)
    : this()
  {
    this.Name = name;
  }

  [RequiredParameter]
  public virtual string EndpointAddress { get; set; }

  public string EndpointConfigurationName { get; set; }

  public bool UseBinaryEncoding { get; set; }

  public bool UseOneWayContract { get; set; }

  public Layout ClientId { get; set; }

  [ArrayParameter(typeof (MethodCallParameter), "parameter")]
  public IList<MethodCallParameter> Parameters { get; private set; }

  public bool IncludeEventProperties { get; set; }

  protected internal virtual bool OnSend(
    NLogEvents events,
    IEnumerable<AsyncLogEventInfo> asyncContinuations)
  {
    return true;
  }

  protected override void Write(AsyncLogEventInfo logEvent)
  {
    this.Write((IList<AsyncLogEventInfo>) new AsyncLogEventInfo[1]
    {
      logEvent
    });
  }

  [Obsolete("Instead override Write(IList<AsyncLogEventInfo> logEvents. Marked obsolete on NLog 4.5")]
  protected override void Write(AsyncLogEventInfo[] logEvents)
  {
    this.Write((IList<AsyncLogEventInfo>) logEvents);
  }

  protected override void Write(IList<AsyncLogEventInfo> logEvents)
  {
    if (this.inCall)
    {
      for (int index = 0; index < logEvents.Count; ++index)
      {
        this.PrecalculateVolatileLayouts(logEvents[index].LogEvent);
        this.buffer.Append(logEvents[index]);
      }
    }
    else
    {
      AsyncLogEventInfo[] asyncLogEventInfoArray = new AsyncLogEventInfo[logEvents.Count];
      logEvents.CopyTo(asyncLogEventInfoArray, 0);
      this.Send(this.TranslateLogEvents((IList<AsyncLogEventInfo>) asyncLogEventInfoArray), (IList<AsyncLogEventInfo>) asyncLogEventInfoArray, (AsyncContinuation) null);
    }
  }

  protected override void FlushAsync(AsyncContinuation asyncContinuation)
  {
    this.SendBufferedEvents(asyncContinuation);
  }

  private static int AddValueAndGetStringOrdinal(
    NLogEvents context,
    Dictionary<string, int> stringTable,
    string value)
  {
    int count;
    if (value == null || !stringTable.TryGetValue(value, out count))
    {
      count = context.Strings.Count;
      if (value != null)
        stringTable.Add(value, count);
      context.Strings.Add(value);
    }
    return count;
  }

  private NLogEvents TranslateLogEvents(IList<AsyncLogEventInfo> logEvents)
  {
    if (logEvents.Count == 0 && !LogManager.ThrowExceptions)
    {
      InternalLogger.Error<string>("LogReceiverServiceTarget(Name={0}): LogEvents array is empty, sending empty event...", this.Name);
      return new NLogEvents();
    }
    string str = string.Empty;
    if (this.ClientId != null)
      str = this.ClientId.Render(logEvents[0].LogEvent);
    NLogEvents nlogEvents = new NLogEvents()
    {
      ClientName = str,
      LayoutNames = new StringCollection(),
      Strings = new StringCollection(),
      BaseTimeUtc = logEvents[0].LogEvent.TimeStamp.ToUniversalTime().Ticks
    };
    Dictionary<string, int> stringTable = new Dictionary<string, int>();
    for (int index = 0; index < this.Parameters.Count; ++index)
      nlogEvents.LayoutNames.Add(this.Parameters[index].Name);
    if (this.IncludeEventProperties)
      LogReceiverWebServiceTarget.AddEventProperties(logEvents, nlogEvents);
    nlogEvents.Events = new NLogEvent[logEvents.Count];
    for (int index = 0; index < logEvents.Count; ++index)
    {
      AsyncLogEventInfo logEvent = logEvents[index];
      nlogEvents.Events[index] = this.TranslateEvent(logEvent.LogEvent, nlogEvents, stringTable);
    }
    return nlogEvents;
  }

  private static void AddEventProperties(
    IList<AsyncLogEventInfo> logEvents,
    NLogEvents networkLogEvents)
  {
    for (int index = 0; index < logEvents.Count; ++index)
    {
      LogEventInfo logEvent = logEvents[index].LogEvent;
      if (logEvent.HasProperties)
      {
        foreach (KeyValuePair<object, object> property in (IEnumerable<KeyValuePair<object, object>>) logEvent.Properties)
        {
          if (property.Key is string key && !networkLogEvents.LayoutNames.Contains(key))
            networkLogEvents.LayoutNames.Add(key);
        }
      }
    }
  }

  private void Send(
    NLogEvents events,
    IList<AsyncLogEventInfo> asyncContinuations,
    AsyncContinuation flushContinuations)
  {
    if (!this.OnSend(events, (IEnumerable<AsyncLogEventInfo>) asyncContinuations))
    {
      if (flushContinuations == null)
        return;
      flushContinuations((Exception) null);
    }
    else
    {
      IWcfLogReceiverClient logReceiver = this.CreateLogReceiver();
      logReceiver.ProcessLogMessagesCompleted += (EventHandler<AsyncCompletedEventArgs>) ((sender, e) =>
      {
        if (e.Error != null)
          InternalLogger.Error(e.Error, "LogReceiverServiceTarget(Name={0}): Error while sending", (object) this.Name);
        for (int index = 0; index < asyncContinuations.Count; ++index)
          asyncContinuations[index].Continuation(e.Error);
        if (flushContinuations != null)
          flushContinuations(e.Error);
        this.SendBufferedEvents((AsyncContinuation) null);
      });
      this.inCall = true;
      logReceiver.ProcessLogMessagesAsync(events);
    }
  }

  [Obsolete("Use CreateLogReceiver instead. Marked obsolete before v4.3.11 and it may be removed in a future release.")]
  protected virtual WcfLogReceiverClient CreateWcfLogReceiverClient()
  {
    WcfLogReceiverClient logReceiverClient;
    if (string.IsNullOrEmpty(this.EndpointConfigurationName))
    {
      Binding binding;
      if (this.UseBinaryEncoding)
        binding = (Binding) new CustomBinding(new BindingElement[2]
        {
          (BindingElement) new BinaryMessageEncodingBindingElement(),
          (BindingElement) new HttpTransportBindingElement()
        });
      else
        binding = (Binding) new BasicHttpBinding();
      logReceiverClient = new WcfLogReceiverClient(this.UseOneWayContract, binding, new System.ServiceModel.EndpointAddress(this.EndpointAddress));
    }
    else
      logReceiverClient = new WcfLogReceiverClient(this.UseOneWayContract, this.EndpointConfigurationName, new System.ServiceModel.EndpointAddress(this.EndpointAddress));
    logReceiverClient.ProcessLogMessagesCompleted += new EventHandler<AsyncCompletedEventArgs>(this.ClientOnProcessLogMessagesCompleted);
    return logReceiverClient;
  }

  protected virtual IWcfLogReceiverClient CreateLogReceiver()
  {
    return (IWcfLogReceiverClient) this.CreateWcfLogReceiverClient();
  }

  private void ClientOnProcessLogMessagesCompleted(
    object sender,
    AsyncCompletedEventArgs asyncCompletedEventArgs)
  {
    if (!(sender is IWcfLogReceiverClient logReceiverClient))
      return;
    if (logReceiverClient.State != CommunicationState.Opened)
      return;
    try
    {
      logReceiverClient.Close();
    }
    catch
    {
      logReceiverClient.Abort();
    }
  }

  private void SendBufferedEvents(AsyncContinuation flushContinuation)
  {
    try
    {
      lock (this.SyncRoot)
      {
        AsyncLogEventInfo[] eventsAndClear = this.buffer.GetEventsAndClear();
        if (eventsAndClear.Length != 0)
        {
          this.Send(this.TranslateLogEvents((IList<AsyncLogEventInfo>) eventsAndClear), (IList<AsyncLogEventInfo>) eventsAndClear, flushContinuation);
        }
        else
        {
          this.inCall = false;
          if (flushContinuation == null)
            return;
          flushContinuation((Exception) null);
        }
      }
    }
    catch (Exception ex)
    {
      if (flushContinuation != null)
      {
        InternalLogger.Error(ex, "LogReceiverServiceTarget(Name={0}): Error in flush async", (object) this.Name);
        if (this.ExceptionMustBeRethrown(ex))
          throw;
        flushContinuation(ex);
      }
      else
      {
        InternalLogger.Error(ex, "LogReceiverServiceTarget(Name={0}): Error in send async", (object) this.Name);
        if (!ex.MustBeRethrownImmediately())
          return;
        throw;
      }
    }
  }

  internal NLogEvent TranslateEvent(
    LogEventInfo eventInfo,
    NLogEvents context,
    Dictionary<string, int> stringTable)
  {
    NLogEvent nlogEvent1 = new NLogEvent();
    nlogEvent1.Id = eventInfo.SequenceID;
    nlogEvent1.MessageOrdinal = LogReceiverWebServiceTarget.AddValueAndGetStringOrdinal(context, stringTable, eventInfo.FormattedMessage);
    nlogEvent1.LevelOrdinal = eventInfo.Level.Ordinal;
    nlogEvent1.LoggerOrdinal = LogReceiverWebServiceTarget.AddValueAndGetStringOrdinal(context, stringTable, eventInfo.LoggerName);
    NLogEvent nlogEvent2 = nlogEvent1;
    DateTime dateTime = eventInfo.TimeStamp;
    dateTime = dateTime.ToUniversalTime();
    long num = dateTime.Ticks - context.BaseTimeUtc;
    nlogEvent2.TimeDelta = num;
    for (int index = 0; index < this.Parameters.Count; ++index)
    {
      string str = this.Parameters[index].Layout.Render(eventInfo);
      int stringOrdinal = LogReceiverWebServiceTarget.AddValueAndGetStringOrdinal(context, stringTable, str);
      nlogEvent1.ValueIndexes.Add(stringOrdinal);
    }
    for (int count = this.Parameters.Count; count < context.LayoutNames.Count; ++count)
    {
      object obj;
      string str = !eventInfo.HasProperties || !eventInfo.Properties.TryGetValue((object) context.LayoutNames[count], out obj) ? string.Empty : Convert.ToString(obj, (IFormatProvider) CultureInfo.InvariantCulture);
      int stringOrdinal = LogReceiverWebServiceTarget.AddValueAndGetStringOrdinal(context, stringTable, str);
      nlogEvent1.ValueIndexes.Add(stringOrdinal);
    }
    if (eventInfo.Exception != null)
      nlogEvent1.ValueIndexes.Add(LogReceiverWebServiceTarget.AddValueAndGetStringOrdinal(context, stringTable, eventInfo.Exception.ToString()));
    return nlogEvent1;
  }
}
