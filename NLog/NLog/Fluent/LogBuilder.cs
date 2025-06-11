// Decompiled with JetBrains decompiler
// Type: NLog.Fluent.LogBuilder
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using System;
using System.Collections;
using System.Runtime.CompilerServices;

#nullable disable
namespace NLog.Fluent;

public class LogBuilder
{
  private readonly LogEventInfo _logEvent;
  private readonly ILogger _logger;

  [CLSCompliant(false)]
  public LogBuilder(ILogger logger)
    : this(logger, NLog.LogLevel.Debug)
  {
  }

  [CLSCompliant(false)]
  public LogBuilder(ILogger logger, NLog.LogLevel logLevel)
  {
    if (logger == null)
      throw new ArgumentNullException(nameof (logger));
    if (logLevel == (NLog.LogLevel) null)
      throw new ArgumentNullException(nameof (logLevel));
    this._logger = logger;
    this._logEvent = new LogEventInfo()
    {
      LoggerName = logger.Name,
      Level = logLevel
    };
  }

  public LogEventInfo LogEventInfo => this._logEvent;

  public LogBuilder Exception(System.Exception exception)
  {
    this._logEvent.Exception = exception;
    return this;
  }

  public LogBuilder Level(NLog.LogLevel logLevel)
  {
    this._logEvent.Level = !(logLevel == (NLog.LogLevel) null) ? logLevel : throw new ArgumentNullException(nameof (logLevel));
    return this;
  }

  public LogBuilder LoggerName(string loggerName)
  {
    this._logEvent.LoggerName = loggerName;
    return this;
  }

  public LogBuilder Message(string message)
  {
    this._logEvent.Message = message;
    return this;
  }

  [MessageTemplateFormatMethod("format")]
  public LogBuilder Message(string format, object arg0)
  {
    this._logEvent.Message = format;
    this._logEvent.Parameters = new object[1]{ arg0 };
    return this;
  }

  [MessageTemplateFormatMethod("format")]
  public LogBuilder Message(string format, object arg0, object arg1)
  {
    this._logEvent.Message = format;
    this._logEvent.Parameters = new object[2]{ arg0, arg1 };
    return this;
  }

  [MessageTemplateFormatMethod("format")]
  public LogBuilder Message(string format, object arg0, object arg1, object arg2)
  {
    this._logEvent.Message = format;
    this._logEvent.Parameters = new object[3]
    {
      arg0,
      arg1,
      arg2
    };
    return this;
  }

  [MessageTemplateFormatMethod("format")]
  public LogBuilder Message(string format, object arg0, object arg1, object arg2, object arg3)
  {
    this._logEvent.Message = format;
    this._logEvent.Parameters = new object[4]
    {
      arg0,
      arg1,
      arg2,
      arg3
    };
    return this;
  }

  [MessageTemplateFormatMethod("format")]
  public LogBuilder Message(string format, params object[] args)
  {
    this._logEvent.Message = format;
    this._logEvent.Parameters = args;
    return this;
  }

  [MessageTemplateFormatMethod("format")]
  public LogBuilder Message(IFormatProvider provider, string format, params object[] args)
  {
    this._logEvent.FormatProvider = provider;
    this._logEvent.Message = format;
    this._logEvent.Parameters = args;
    return this;
  }

  public LogBuilder Property(object name, object value)
  {
    if (name == null)
      throw new ArgumentNullException(nameof (name));
    this._logEvent.Properties[name] = value;
    return this;
  }

  public LogBuilder Properties(IDictionary properties)
  {
    if (properties == null)
      throw new ArgumentNullException(nameof (properties));
    foreach (object key in (IEnumerable) properties.Keys)
      this._logEvent.Properties[key] = properties[key];
    return this;
  }

  public LogBuilder TimeStamp(DateTime timeStamp)
  {
    this._logEvent.TimeStamp = timeStamp;
    return this;
  }

  public LogBuilder StackTrace(System.Diagnostics.StackTrace stackTrace, int userStackFrame)
  {
    this._logEvent.SetStackTrace(stackTrace, userStackFrame);
    return this;
  }

  public void Write([CallerMemberName] string callerMemberName = null, [CallerFilePath] string callerFilePath = null, [CallerLineNumber] int callerLineNumber = 0)
  {
    if (!this._logger.IsEnabled(this._logEvent.Level))
      return;
    if (callerMemberName != null)
      this.Property((object) "CallerMemberName", (object) callerMemberName);
    if (callerFilePath != null)
      this.Property((object) "CallerFilePath", (object) callerFilePath);
    if (callerLineNumber != 0)
      this.Property((object) "CallerLineNumber", (object) callerLineNumber);
    this._logEvent.SetCallerInfo((string) null, callerMemberName, callerFilePath, callerLineNumber);
    this._logger.Log(this._logEvent);
  }

  public void WriteIf(
    Func<bool> condition,
    [CallerMemberName] string callerMemberName = null,
    [CallerFilePath] string callerFilePath = null,
    [CallerLineNumber] int callerLineNumber = 0)
  {
    if (condition == null || !condition() || !this._logger.IsEnabled(this._logEvent.Level))
      return;
    if (callerMemberName != null)
      this.Property((object) "CallerMemberName", (object) callerMemberName);
    if (callerFilePath != null)
      this.Property((object) "CallerFilePath", (object) callerFilePath);
    if (callerLineNumber != 0)
      this.Property((object) "CallerLineNumber", (object) callerLineNumber);
    this._logEvent.SetCallerInfo((string) null, callerMemberName, callerFilePath, callerLineNumber);
    this._logger.Log(this._logEvent);
  }

  public void WriteIf(
    bool condition,
    [CallerMemberName] string callerMemberName = null,
    [CallerFilePath] string callerFilePath = null,
    [CallerLineNumber] int callerLineNumber = 0)
  {
    if (!condition || !this._logger.IsEnabled(this._logEvent.Level))
      return;
    if (callerMemberName != null)
      this.Property((object) "CallerMemberName", (object) callerMemberName);
    if (callerFilePath != null)
      this.Property((object) "CallerFilePath", (object) callerFilePath);
    if (callerLineNumber != 0)
      this.Property((object) "CallerLineNumber", (object) callerLineNumber);
    this._logEvent.SetCallerInfo((string) null, callerMemberName, callerFilePath, callerLineNumber);
    this._logger.Log(this._logEvent);
  }
}
