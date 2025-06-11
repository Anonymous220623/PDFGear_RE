// Decompiled with JetBrains decompiler
// Type: NLog.Targets.EventLogTarget
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using NLog.Common;
using NLog.Config;
using NLog.Internal.Fakeables;
using NLog.Layouts;
using System;
using System.ComponentModel;
using System.Diagnostics;

#nullable disable
namespace NLog.Targets;

[Target("EventLog")]
public class EventLogTarget : TargetWithLayout, IInstallable
{
  internal const int EventLogMaxMessageLength = 16384 /*0x4000*/;
  private readonly EventLogTarget.IEventLogWrapper _eventLogWrapper;
  private int _maxMessageLength;
  private long? _maxKilobytes;

  public EventLogTarget()
    : this((EventLogTarget.IEventLogWrapper) null, (IAppDomain) null)
  {
  }

  public EventLogTarget(string name)
    : this((EventLogTarget.IEventLogWrapper) null, (IAppDomain) null)
  {
    this.Name = name;
  }

  [Obsolete("This constructor will be removed in NLog 5. Marked obsolete on NLog 4.6")]
  public EventLogTarget(IAppDomain appDomain)
    : this((EventLogTarget.IEventLogWrapper) null, appDomain)
  {
  }

  internal EventLogTarget(EventLogTarget.IEventLogWrapper eventLogWrapper, IAppDomain appDomain)
  {
    this._eventLogWrapper = eventLogWrapper ?? (EventLogTarget.IEventLogWrapper) new EventLogTarget.EventLogWrapper();
    appDomain = appDomain ?? LogFactory.CurrentAppDomain;
    this.Source = (Layout) appDomain.FriendlyName;
    this.Log = "Application";
    this.MachineName = ".";
    this.MaxMessageLength = 16384 /*0x4000*/;
    this.OptimizeBufferReuse = this.GetType() == typeof (EventLogTarget);
  }

  [DefaultValue(".")]
  public string MachineName { get; set; }

  public Layout EventId { get; set; }

  public Layout Category { get; set; }

  public Layout EntryType { get; set; }

  public Layout Source { get; set; }

  [DefaultValue("Application")]
  public string Log { get; set; }

  [DefaultValue(16384 /*0x4000*/)]
  public int MaxMessageLength
  {
    get => this._maxMessageLength;
    set
    {
      this._maxMessageLength = value > 0 ? value : throw new ArgumentException("MaxMessageLength cannot be zero or negative.");
    }
  }

  [DefaultValue(null)]
  public long? MaxKilobytes
  {
    get => this._maxKilobytes;
    set
    {
      if (value.HasValue)
      {
        long? nullable1 = value;
        long num1 = 64 /*0x40*/;
        if (!(nullable1.GetValueOrDefault() < num1 & nullable1.HasValue))
        {
          nullable1 = value;
          long num2 = 4194240;
          if (!(nullable1.GetValueOrDefault() > num2 & nullable1.HasValue))
          {
            long? nullable2 = value;
            long num3 = 64 /*0x40*/;
            nullable1 = nullable2.HasValue ? new long?(nullable2.GetValueOrDefault() % num3) : new long?();
            long num4 = 0;
            if (nullable1.GetValueOrDefault() == num4 & nullable1.HasValue)
              goto label_5;
          }
        }
        throw new ArgumentException("MaxKilobytes must be a multiple of 64, and between 64 and 4194240");
      }
label_5:
      this._maxKilobytes = value;
    }
  }

  [DefaultValue(EventLogTargetOverflowAction.Truncate)]
  public EventLogTargetOverflowAction OnOverflow { get; set; }

  public void Install(InstallationContext installationContext)
  {
    this.CreateEventSourceIfNeeded(this.GetFixedSource(), true);
  }

  public void Uninstall(InstallationContext installationContext)
  {
    string fixedSource = this.GetFixedSource();
    if (string.IsNullOrEmpty(fixedSource))
      InternalLogger.Debug<string>("EventLogTarget(Name={0}): Skipping removing of event source because it contains layout renderers", this.Name);
    else
      this._eventLogWrapper.DeleteEventSource(fixedSource, this.MachineName);
  }

  public bool? IsInstalled(InstallationContext installationContext)
  {
    string fixedSource = this.GetFixedSource();
    if (!string.IsNullOrEmpty(fixedSource))
      return new bool?(this._eventLogWrapper.SourceExists(fixedSource, this.MachineName));
    InternalLogger.Debug<string>("EventLogTarget(Name={0}): Unclear if event source exists because it contains layout renderers", this.Name);
    return new bool?();
  }

  protected override void InitializeTarget()
  {
    base.InitializeTarget();
    this.CreateEventSourceIfNeeded(this.GetFixedSource(), false);
  }

  protected override void Write(LogEventInfo logEvent)
  {
    string message1 = this.RenderLogEvent(this.Layout, logEvent);
    EventLogEntryType entryType = this.GetEntryType(logEvent);
    int result1 = 0;
    string s1 = this.RenderLogEvent(this.EventId, logEvent);
    if (!string.IsNullOrEmpty(s1) && !int.TryParse(s1, out result1))
      InternalLogger.Warn<string, string>("EventLogTarget(Name={0}): WriteEntry failed to parse EventId={1}", this.Name, s1);
    short result2 = 0;
    string s2 = this.RenderLogEvent(this.Category, logEvent);
    if (!string.IsNullOrEmpty(s2) && !short.TryParse(s2, out result2))
      InternalLogger.Warn<string, string>("EventLogTarget(Name={0}): WriteEntry failed to parse Category={1}", this.Name, s2);
    string eventLogSource = this.RenderLogEvent(this.Source, logEvent);
    if (string.IsNullOrEmpty(eventLogSource))
      InternalLogger.Warn<string>("EventLogTarget(Name={0}): WriteEntry discarded because Source rendered as empty string", this.Name);
    else if (message1.Length > this.MaxMessageLength)
    {
      if (this.OnOverflow == EventLogTargetOverflowAction.Truncate)
      {
        string message2 = message1.Substring(0, this.MaxMessageLength);
        this.WriteEntry(eventLogSource, message2, entryType, result1, result2);
      }
      else if (this.OnOverflow == EventLogTargetOverflowAction.Split)
      {
        for (int startIndex = 0; startIndex < message1.Length; startIndex += this.MaxMessageLength)
        {
          string message3 = message1.Substring(startIndex, Math.Min(this.MaxMessageLength, message1.Length - startIndex));
          this.WriteEntry(eventLogSource, message3, entryType, result1, result2);
        }
      }
      else
      {
        if (this.OnOverflow != EventLogTargetOverflowAction.Discard)
          return;
        InternalLogger.Debug<string, int>("EventLogTarget(Name={0}): WriteEntry discarded because too big message size: {1}", this.Name, message1.Length);
      }
    }
    else
      this.WriteEntry(eventLogSource, message1, entryType, result1, result2);
  }

  private void WriteEntry(
    string eventLogSource,
    string message,
    EventLogEntryType entryType,
    int eventId,
    short category)
  {
    if ((!this._eventLogWrapper.IsEventLogAssociated || !(this._eventLogWrapper.Log == this.Log) || !(this._eventLogWrapper.MachineName == this.MachineName) ? 0 : (this._eventLogWrapper.Source == eventLogSource ? 1 : 0)) == 0)
    {
      InternalLogger.Debug<string, string, string>("EventLogTarget(Name={0}): Refresh EventLog Source {1} and Log {2}", this.Name, eventLogSource, this.Log);
      this._eventLogWrapper.AssociateNewEventLog(this.Log, this.MachineName, eventLogSource);
      try
      {
        if (!this._eventLogWrapper.SourceExists(eventLogSource, this.MachineName))
        {
          InternalLogger.Warn<string, string>("EventLogTarget(Name={0}): Source {1} does not exist", this.Name, eventLogSource);
        }
        else
        {
          string str = this._eventLogWrapper.LogNameFromSourceName(eventLogSource, this.MachineName);
          if (!str.Equals(this.Log, StringComparison.OrdinalIgnoreCase))
            InternalLogger.Debug("EventLogTarget(Name={0}): Source {1} should be mapped to Log {2}, but EventLog.LogNameFromSourceName returns {3}", (object) this.Name, (object) eventLogSource, (object) this.Log, (object) str);
        }
      }
      catch (Exception ex)
      {
        if (LogManager.ThrowExceptions)
          throw;
        object[] objArray = new object[3]
        {
          (object) this.Name,
          (object) eventLogSource,
          (object) this.Log
        };
        InternalLogger.Warn(ex, "EventLogTarget(Name={0}): Exception thrown when checking if Source {1} and LogName {2} are valid", objArray);
      }
    }
    this._eventLogWrapper.WriteEntry(message, entryType, eventId, category);
  }

  private EventLogEntryType GetEntryType(LogEventInfo logEvent)
  {
    string inputValue = this.RenderLogEvent(this.EntryType, logEvent);
    if (!string.IsNullOrEmpty(inputValue))
    {
      EventLogEntryType resultValue;
      if (ConversionHelpers.TryParseEnum<EventLogEntryType>(inputValue, out resultValue))
        return resultValue;
      InternalLogger.Warn<string, string>("EventLogTarget(Name={0}): WriteEntry failed to parse EntryType={1}", this.Name, inputValue);
    }
    if (logEvent.Level >= NLog.LogLevel.Error)
      return EventLogEntryType.Error;
    return logEvent.Level >= NLog.LogLevel.Warn ? EventLogEntryType.Warning : EventLogEntryType.Information;
  }

  internal string GetFixedSource()
  {
    return this.Source is SimpleLayout source && source.IsFixedText ? source.FixedText : (string) null;
  }

  private void CreateEventSourceIfNeeded(string fixedSource, bool alwaysThrowError)
  {
    if (string.IsNullOrEmpty(fixedSource))
    {
      InternalLogger.Debug<string>("EventLogTarget(Name={0}): Skipping creation of event source because it contains layout renderers", this.Name);
    }
    else
    {
      try
      {
        if (this._eventLogWrapper.SourceExists(fixedSource, this.MachineName))
        {
          string str = this._eventLogWrapper.LogNameFromSourceName(fixedSource, this.MachineName);
          if (!str.Equals(this.Log, StringComparison.OrdinalIgnoreCase))
          {
            InternalLogger.Debug("EventLogTarget(Name={0}): Updating source {1} to use log {2}, instead of {3} (Computer restart is needed)", (object) this.Name, (object) fixedSource, (object) this.Log, (object) str);
            this._eventLogWrapper.DeleteEventSource(fixedSource, this.MachineName);
            this._eventLogWrapper.CreateEventSource(new EventSourceCreationData(fixedSource, this.Log)
            {
              MachineName = this.MachineName
            });
          }
        }
        else
        {
          InternalLogger.Debug<string, string, string>("EventLogTarget(Name={0}): Creating source {1} to use log {2}", this.Name, fixedSource, this.Log);
          this._eventLogWrapper.CreateEventSource(new EventSourceCreationData(fixedSource, this.Log)
          {
            MachineName = this.MachineName
          });
        }
        this._eventLogWrapper.AssociateNewEventLog(this.Log, this.MachineName, fixedSource);
        long? maxKilobytes = this.MaxKilobytes;
        if (!maxKilobytes.HasValue)
          return;
        long maximumKilobytes = this._eventLogWrapper.MaximumKilobytes;
        maxKilobytes = this.MaxKilobytes;
        long valueOrDefault = maxKilobytes.GetValueOrDefault();
        if (!(maximumKilobytes < valueOrDefault & maxKilobytes.HasValue))
          return;
        EventLogTarget.IEventLogWrapper eventLogWrapper = this._eventLogWrapper;
        maxKilobytes = this.MaxKilobytes;
        long num = maxKilobytes.Value;
        eventLogWrapper.MaximumKilobytes = num;
      }
      catch (Exception ex)
      {
        object[] objArray = new object[3]
        {
          (object) this.Name,
          (object) fixedSource,
          (object) this.Log
        };
        InternalLogger.Error(ex, "EventLogTarget(Name={0}): Error when connecting to EventLog. Source={1} in Log={2}", objArray);
        if (!alwaysThrowError && !LogManager.ThrowExceptions)
          return;
        throw;
      }
    }
  }

  internal interface IEventLogWrapper
  {
    string Source { get; }

    string Log { get; }

    string MachineName { get; }

    long MaximumKilobytes { get; set; }

    bool IsEventLogAssociated { get; }

    void WriteEntry(string message, EventLogEntryType entryType, int eventId, short category);

    void AssociateNewEventLog(string logName, string machineName, string source);

    void DeleteEventSource(string source, string machineName);

    bool SourceExists(string source, string machineName);

    string LogNameFromSourceName(string source, string machineName);

    void CreateEventSource(EventSourceCreationData sourceData);
  }

  private sealed class EventLogWrapper : EventLogTarget.IEventLogWrapper, IDisposable
  {
    private EventLog _windowsEventLog;

    public string Source { get; private set; }

    public string Log { get; private set; }

    public string MachineName { get; private set; }

    public long MaximumKilobytes
    {
      get => this._windowsEventLog.MaximumKilobytes;
      set => this._windowsEventLog.MaximumKilobytes = value;
    }

    public bool IsEventLogAssociated => this._windowsEventLog != null;

    public void WriteEntry(
      string message,
      EventLogEntryType entryType,
      int eventId,
      short category)
    {
      this._windowsEventLog.WriteEntry(message, entryType, eventId, category);
    }

    public void AssociateNewEventLog(string logName, string machineName, string source)
    {
      EventLog windowsEventLog = this._windowsEventLog;
      this._windowsEventLog = new EventLog(logName, machineName, source);
      this.Source = source;
      this.Log = logName;
      this.MachineName = machineName;
      windowsEventLog?.Dispose();
    }

    public void DeleteEventSource(string source, string machineName)
    {
      EventLog.DeleteEventSource(source, machineName);
    }

    public bool SourceExists(string source, string machineName)
    {
      return EventLog.SourceExists(source, machineName);
    }

    public string LogNameFromSourceName(string source, string machineName)
    {
      return EventLog.LogNameFromSourceName(source, machineName);
    }

    public void CreateEventSource(EventSourceCreationData sourceData)
    {
      EventLog.CreateEventSource(sourceData);
    }

    public void Dispose()
    {
      this._windowsEventLog?.Dispose();
      this._windowsEventLog = (EventLog) null;
    }
  }
}
