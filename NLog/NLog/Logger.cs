// Decompiled with JetBrains decompiler
// Type: NLog.Logger
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using JetBrains.Annotations;
using NLog.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

#nullable disable
namespace NLog;

[CLSCompliant(true)]
public class Logger : ILogger, ILoggerBase, ISuppress
{
  internal static readonly Type DefaultLoggerType = typeof (Logger);
  private Logger _contextLogger;
  private ThreadSafeDictionary<string, object> _contextProperties;
  private LoggerConfiguration _configuration;
  private volatile bool _isTraceEnabled;
  private volatile bool _isDebugEnabled;
  private volatile bool _isInfoEnabled;
  private volatile bool _isWarnEnabled;
  private volatile bool _isErrorEnabled;
  private volatile bool _isFatalEnabled;

  [Conditional("DEBUG")]
  public void ConditionalDebug<T>(T value) => this.Debug<T>(value);

  [Conditional("DEBUG")]
  public void ConditionalDebug<T>(IFormatProvider formatProvider, T value)
  {
    this.Debug<T>(formatProvider, value);
  }

  [Conditional("DEBUG")]
  public void ConditionalDebug(LogMessageGenerator messageFunc) => this.Debug(messageFunc);

  [Conditional("DEBUG")]
  [MessageTemplateFormatMethod("message")]
  public void ConditionalDebug(Exception exception, string message, params object[] args)
  {
    this.Debug(exception, message, args);
  }

  [Conditional("DEBUG")]
  [MessageTemplateFormatMethod("message")]
  public void ConditionalDebug(
    Exception exception,
    IFormatProvider formatProvider,
    string message,
    params object[] args)
  {
    this.Debug(exception, formatProvider, message, args);
  }

  [Conditional("DEBUG")]
  [MessageTemplateFormatMethod("message")]
  public void ConditionalDebug(
    IFormatProvider formatProvider,
    string message,
    params object[] args)
  {
    this.Debug(formatProvider, message, args);
  }

  [Conditional("DEBUG")]
  public void ConditionalDebug(string message) => this.Debug(message);

  [Conditional("DEBUG")]
  [MessageTemplateFormatMethod("message")]
  public void ConditionalDebug(string message, params object[] args) => this.Debug(message, args);

  [Conditional("DEBUG")]
  [MessageTemplateFormatMethod("message")]
  public void ConditionalDebug<TArgument>(
    IFormatProvider formatProvider,
    string message,
    TArgument argument)
  {
    this.Debug<TArgument>(formatProvider, message, argument);
  }

  [Conditional("DEBUG")]
  [MessageTemplateFormatMethod("message")]
  public void ConditionalDebug<TArgument>(string message, TArgument argument)
  {
    this.Debug<TArgument>(message, argument);
  }

  [Conditional("DEBUG")]
  [MessageTemplateFormatMethod("message")]
  public void ConditionalDebug<TArgument1, TArgument2>(
    IFormatProvider formatProvider,
    string message,
    TArgument1 argument1,
    TArgument2 argument2)
  {
    this.Debug<TArgument1, TArgument2>(formatProvider, message, argument1, argument2);
  }

  [Conditional("DEBUG")]
  [MessageTemplateFormatMethod("message")]
  public void ConditionalDebug<TArgument1, TArgument2>(
    string message,
    TArgument1 argument1,
    TArgument2 argument2)
  {
    this.Debug<TArgument1, TArgument2>(message, argument1, argument2);
  }

  [Conditional("DEBUG")]
  [MessageTemplateFormatMethod("message")]
  public void ConditionalDebug<TArgument1, TArgument2, TArgument3>(
    IFormatProvider formatProvider,
    string message,
    TArgument1 argument1,
    TArgument2 argument2,
    TArgument3 argument3)
  {
    this.Debug<TArgument1, TArgument2, TArgument3>(formatProvider, message, argument1, argument2, argument3);
  }

  [Conditional("DEBUG")]
  [MessageTemplateFormatMethod("message")]
  public void ConditionalDebug<TArgument1, TArgument2, TArgument3>(
    string message,
    TArgument1 argument1,
    TArgument2 argument2,
    TArgument3 argument3)
  {
    this.Debug<TArgument1, TArgument2, TArgument3>(message, argument1, argument2, argument3);
  }

  [Conditional("DEBUG")]
  public void ConditionalDebug(object value) => this.Debug(value);

  [Conditional("DEBUG")]
  public void ConditionalDebug(IFormatProvider formatProvider, object value)
  {
    this.Debug(formatProvider, value);
  }

  [Conditional("DEBUG")]
  [MessageTemplateFormatMethod("message")]
  public void ConditionalDebug(string message, object arg1, object arg2)
  {
    this.Debug(message, arg1, arg2);
  }

  [Conditional("DEBUG")]
  [MessageTemplateFormatMethod("message")]
  public void ConditionalDebug(string message, object arg1, object arg2, object arg3)
  {
    this.Debug(message, arg1, arg2, arg3);
  }

  [Conditional("DEBUG")]
  [MessageTemplateFormatMethod("message")]
  public void ConditionalDebug(IFormatProvider formatProvider, string message, bool argument)
  {
    this.Debug(formatProvider, message, argument);
  }

  [Conditional("DEBUG")]
  [MessageTemplateFormatMethod("message")]
  public void ConditionalDebug(string message, bool argument) => this.Debug(message, argument);

  [Conditional("DEBUG")]
  [MessageTemplateFormatMethod("message")]
  public void ConditionalDebug(IFormatProvider formatProvider, string message, char argument)
  {
    this.Debug(formatProvider, message, argument);
  }

  [Conditional("DEBUG")]
  [MessageTemplateFormatMethod("message")]
  public void ConditionalDebug(string message, char argument) => this.Debug(message, argument);

  [Conditional("DEBUG")]
  [MessageTemplateFormatMethod("message")]
  public void ConditionalDebug(IFormatProvider formatProvider, string message, byte argument)
  {
    this.Debug(formatProvider, message, argument);
  }

  [Conditional("DEBUG")]
  [MessageTemplateFormatMethod("message")]
  public void ConditionalDebug(string message, byte argument) => this.Debug(message, argument);

  [Conditional("DEBUG")]
  [MessageTemplateFormatMethod("message")]
  public void ConditionalDebug(IFormatProvider formatProvider, string message, string argument)
  {
    this.Debug(formatProvider, message, argument);
  }

  [Conditional("DEBUG")]
  [MessageTemplateFormatMethod("message")]
  public void ConditionalDebug(string message, string argument) => this.Debug(message, argument);

  [Conditional("DEBUG")]
  [MessageTemplateFormatMethod("message")]
  public void ConditionalDebug(IFormatProvider formatProvider, string message, int argument)
  {
    this.Debug(formatProvider, message, argument);
  }

  [Conditional("DEBUG")]
  [MessageTemplateFormatMethod("message")]
  public void ConditionalDebug(string message, int argument) => this.Debug(message, argument);

  [Conditional("DEBUG")]
  [MessageTemplateFormatMethod("message")]
  public void ConditionalDebug(IFormatProvider formatProvider, string message, long argument)
  {
    this.Debug(formatProvider, message, argument);
  }

  [Conditional("DEBUG")]
  [MessageTemplateFormatMethod("message")]
  public void ConditionalDebug(string message, long argument) => this.Debug(message, argument);

  [Conditional("DEBUG")]
  [MessageTemplateFormatMethod("message")]
  public void ConditionalDebug(IFormatProvider formatProvider, string message, float argument)
  {
    this.Debug(formatProvider, message, argument);
  }

  [Conditional("DEBUG")]
  [MessageTemplateFormatMethod("message")]
  public void ConditionalDebug(string message, float argument) => this.Debug(message, argument);

  [Conditional("DEBUG")]
  [MessageTemplateFormatMethod("message")]
  public void ConditionalDebug(IFormatProvider formatProvider, string message, double argument)
  {
    this.Debug(formatProvider, message, argument);
  }

  [Conditional("DEBUG")]
  [MessageTemplateFormatMethod("message")]
  public void ConditionalDebug(string message, double argument) => this.Debug(message, argument);

  [Conditional("DEBUG")]
  [MessageTemplateFormatMethod("message")]
  public void ConditionalDebug(IFormatProvider formatProvider, string message, Decimal argument)
  {
    this.Debug(formatProvider, message, argument);
  }

  [Conditional("DEBUG")]
  [MessageTemplateFormatMethod("message")]
  public void ConditionalDebug(string message, Decimal argument) => this.Debug(message, argument);

  [Conditional("DEBUG")]
  [MessageTemplateFormatMethod("message")]
  public void ConditionalDebug(IFormatProvider formatProvider, string message, object argument)
  {
    this.Debug(formatProvider, message, argument);
  }

  [Conditional("DEBUG")]
  [MessageTemplateFormatMethod("message")]
  public void ConditionalDebug(string message, object argument) => this.Debug(message, argument);

  [Conditional("DEBUG")]
  public void ConditionalTrace<T>(T value) => this.Trace<T>(value);

  [Conditional("DEBUG")]
  public void ConditionalTrace<T>(IFormatProvider formatProvider, T value)
  {
    this.Trace<T>(formatProvider, value);
  }

  [Conditional("DEBUG")]
  public void ConditionalTrace(LogMessageGenerator messageFunc) => this.Trace(messageFunc);

  [Conditional("DEBUG")]
  [MessageTemplateFormatMethod("message")]
  public void ConditionalTrace(Exception exception, string message, params object[] args)
  {
    this.Trace(exception, message, args);
  }

  [Conditional("DEBUG")]
  [MessageTemplateFormatMethod("message")]
  public void ConditionalTrace(
    Exception exception,
    IFormatProvider formatProvider,
    string message,
    params object[] args)
  {
    this.Trace(exception, formatProvider, message, args);
  }

  [Conditional("DEBUG")]
  [MessageTemplateFormatMethod("message")]
  public void ConditionalTrace(
    IFormatProvider formatProvider,
    string message,
    params object[] args)
  {
    this.Trace(formatProvider, message, args);
  }

  [Conditional("DEBUG")]
  public void ConditionalTrace(string message) => this.Trace(message);

  [Conditional("DEBUG")]
  public void ConditionalTrace(string message, params object[] args) => this.Trace(message, args);

  [Conditional("DEBUG")]
  [MessageTemplateFormatMethod("message")]
  public void ConditionalTrace<TArgument>(
    IFormatProvider formatProvider,
    string message,
    TArgument argument)
  {
    this.Trace<TArgument>(formatProvider, message, argument);
  }

  [Conditional("DEBUG")]
  [MessageTemplateFormatMethod("message")]
  public void ConditionalTrace<TArgument>(string message, TArgument argument)
  {
    this.Trace<TArgument>(message, argument);
  }

  [Conditional("DEBUG")]
  [MessageTemplateFormatMethod("message")]
  public void ConditionalTrace<TArgument1, TArgument2>(
    IFormatProvider formatProvider,
    string message,
    TArgument1 argument1,
    TArgument2 argument2)
  {
    this.Trace<TArgument1, TArgument2>(formatProvider, message, argument1, argument2);
  }

  [Conditional("DEBUG")]
  [MessageTemplateFormatMethod("message")]
  public void ConditionalTrace<TArgument1, TArgument2>(
    string message,
    TArgument1 argument1,
    TArgument2 argument2)
  {
    this.Trace<TArgument1, TArgument2>(message, argument1, argument2);
  }

  [Conditional("DEBUG")]
  [MessageTemplateFormatMethod("message")]
  public void ConditionalTrace<TArgument1, TArgument2, TArgument3>(
    IFormatProvider formatProvider,
    string message,
    TArgument1 argument1,
    TArgument2 argument2,
    TArgument3 argument3)
  {
    this.Trace<TArgument1, TArgument2, TArgument3>(formatProvider, message, argument1, argument2, argument3);
  }

  [Conditional("DEBUG")]
  [MessageTemplateFormatMethod("message")]
  public void ConditionalTrace<TArgument1, TArgument2, TArgument3>(
    string message,
    TArgument1 argument1,
    TArgument2 argument2,
    TArgument3 argument3)
  {
    this.Trace<TArgument1, TArgument2, TArgument3>(message, argument1, argument2, argument3);
  }

  [Conditional("DEBUG")]
  public void ConditionalTrace(object value) => this.Trace(value);

  [Conditional("DEBUG")]
  public void ConditionalTrace(IFormatProvider formatProvider, object value)
  {
    this.Trace(formatProvider, value);
  }

  [Conditional("DEBUG")]
  [MessageTemplateFormatMethod("message")]
  public void ConditionalTrace(string message, object arg1, object arg2)
  {
    this.Trace(message, arg1, arg2);
  }

  [Conditional("DEBUG")]
  [MessageTemplateFormatMethod("message")]
  public void ConditionalTrace(string message, object arg1, object arg2, object arg3)
  {
    this.Trace(message, arg1, arg2, arg3);
  }

  [Conditional("DEBUG")]
  [MessageTemplateFormatMethod("message")]
  public void ConditionalTrace(IFormatProvider formatProvider, string message, bool argument)
  {
    this.Trace(formatProvider, message, argument);
  }

  [Conditional("DEBUG")]
  [MessageTemplateFormatMethod("message")]
  public void ConditionalTrace(string message, bool argument) => this.Trace(message, argument);

  [Conditional("DEBUG")]
  [MessageTemplateFormatMethod("message")]
  public void ConditionalTrace(IFormatProvider formatProvider, string message, char argument)
  {
    this.Trace(formatProvider, message, argument);
  }

  [Conditional("DEBUG")]
  [MessageTemplateFormatMethod("message")]
  public void ConditionalTrace(string message, char argument) => this.Trace(message, argument);

  [Conditional("DEBUG")]
  [MessageTemplateFormatMethod("message")]
  public void ConditionalTrace(IFormatProvider formatProvider, string message, byte argument)
  {
    this.Trace(formatProvider, message, argument);
  }

  [Conditional("DEBUG")]
  [MessageTemplateFormatMethod("message")]
  public void ConditionalTrace(string message, byte argument) => this.Trace(message, argument);

  [Conditional("DEBUG")]
  [MessageTemplateFormatMethod("message")]
  public void ConditionalTrace(IFormatProvider formatProvider, string message, string argument)
  {
    this.Trace(formatProvider, message, argument);
  }

  [Conditional("DEBUG")]
  [MessageTemplateFormatMethod("message")]
  public void ConditionalTrace(string message, string argument) => this.Trace(message, argument);

  [Conditional("DEBUG")]
  [MessageTemplateFormatMethod("message")]
  public void ConditionalTrace(IFormatProvider formatProvider, string message, int argument)
  {
    this.Trace(formatProvider, message, argument);
  }

  [Conditional("DEBUG")]
  [MessageTemplateFormatMethod("message")]
  public void ConditionalTrace(string message, int argument) => this.Trace(message, argument);

  [Conditional("DEBUG")]
  [MessageTemplateFormatMethod("message")]
  public void ConditionalTrace(IFormatProvider formatProvider, string message, long argument)
  {
    this.Trace(formatProvider, message, argument);
  }

  [Conditional("DEBUG")]
  [MessageTemplateFormatMethod("message")]
  public void ConditionalTrace(string message, long argument) => this.Trace(message, argument);

  [Conditional("DEBUG")]
  [MessageTemplateFormatMethod("message")]
  public void ConditionalTrace(IFormatProvider formatProvider, string message, float argument)
  {
    this.Trace(formatProvider, message, argument);
  }

  [Conditional("DEBUG")]
  [MessageTemplateFormatMethod("message")]
  public void ConditionalTrace(string message, float argument) => this.Trace(message, argument);

  [Conditional("DEBUG")]
  [MessageTemplateFormatMethod("message")]
  public void ConditionalTrace(IFormatProvider formatProvider, string message, double argument)
  {
    this.Trace(formatProvider, message, argument);
  }

  [Conditional("DEBUG")]
  [MessageTemplateFormatMethod("message")]
  public void ConditionalTrace(string message, double argument) => this.Trace(message, argument);

  [Conditional("DEBUG")]
  [MessageTemplateFormatMethod("message")]
  public void ConditionalTrace(IFormatProvider formatProvider, string message, Decimal argument)
  {
    this.Trace(formatProvider, message, argument);
  }

  [Conditional("DEBUG")]
  [MessageTemplateFormatMethod("message")]
  public void ConditionalTrace(string message, Decimal argument) => this.Trace(message, argument);

  [Conditional("DEBUG")]
  [MessageTemplateFormatMethod("message")]
  public void ConditionalTrace(IFormatProvider formatProvider, string message, object argument)
  {
    this.Trace(formatProvider, message, argument);
  }

  [Conditional("DEBUG")]
  [MessageTemplateFormatMethod("message")]
  public void ConditionalTrace(string message, object argument) => this.Trace(message, argument);

  public bool IsTraceEnabled => this._contextLogger._isTraceEnabled;

  public bool IsDebugEnabled => this._contextLogger._isDebugEnabled;

  public bool IsInfoEnabled => this._contextLogger._isInfoEnabled;

  public bool IsWarnEnabled => this._contextLogger._isWarnEnabled;

  public bool IsErrorEnabled => this._contextLogger._isErrorEnabled;

  public bool IsFatalEnabled => this._contextLogger._isFatalEnabled;

  public void Trace<T>(T value)
  {
    if (!this.IsTraceEnabled)
      return;
    this.WriteToTargets<T>(LogLevel.Trace, (IFormatProvider) null, value);
  }

  public void Trace<T>(IFormatProvider formatProvider, T value)
  {
    if (!this.IsTraceEnabled)
      return;
    this.WriteToTargets<T>(LogLevel.Trace, formatProvider, value);
  }

  public void Trace(LogMessageGenerator messageFunc)
  {
    if (!this.IsTraceEnabled)
      return;
    if (messageFunc == null)
      throw new ArgumentNullException(nameof (messageFunc));
    this.WriteToTargets(LogLevel.Trace, (IFormatProvider) null, messageFunc());
  }

  [Obsolete("Use Trace(Exception exception, string message, params object[] args) method instead. Marked obsolete before v4.3.11")]
  public void TraceException([Localizable(false)] string message, Exception exception)
  {
    this.Trace(message, exception);
  }

  [MessageTemplateFormatMethod("message")]
  public void Trace(IFormatProvider formatProvider, [Localizable(false)] string message, params object[] args)
  {
    if (!this.IsTraceEnabled)
      return;
    this.WriteToTargets(LogLevel.Trace, formatProvider, message, args);
  }

  public void Trace([Localizable(false)] string message)
  {
    if (!this.IsTraceEnabled)
      return;
    this.WriteToTargets(LogLevel.Trace, (IFormatProvider) null, message);
  }

  [MessageTemplateFormatMethod("message")]
  public void Trace([Localizable(false)] string message, params object[] args)
  {
    if (!this.IsTraceEnabled)
      return;
    this.WriteToTargets(LogLevel.Trace, message, args);
  }

  [Obsolete("Use Trace(Exception exception, string message, params object[] args) method instead. Marked obsolete before v4.3.11")]
  public void Trace([Localizable(false)] string message, Exception exception)
  {
    if (!this.IsTraceEnabled)
      return;
    this.WriteToTargets(LogLevel.Trace, exception, message, (object[]) null);
  }

  public void Trace(Exception exception, [Localizable(false)] string message)
  {
    if (!this.IsTraceEnabled)
      return;
    this.WriteToTargets(LogLevel.Trace, exception, message, (object[]) null);
  }

  [MessageTemplateFormatMethod("message")]
  public void Trace(Exception exception, [Localizable(false)] string message, params object[] args)
  {
    if (!this.IsTraceEnabled)
      return;
    this.WriteToTargets(LogLevel.Trace, exception, message, args);
  }

  [MessageTemplateFormatMethod("message")]
  public void Trace(
    Exception exception,
    IFormatProvider formatProvider,
    [Localizable(false)] string message,
    params object[] args)
  {
    if (!this.IsTraceEnabled)
      return;
    this.WriteToTargets(LogLevel.Trace, exception, formatProvider, message, args);
  }

  [MessageTemplateFormatMethod("message")]
  public void Trace<TArgument>(IFormatProvider formatProvider, [Localizable(false)] string message, TArgument argument)
  {
    if (!this.IsTraceEnabled)
      return;
    this.WriteToTargets(LogLevel.Trace, formatProvider, message, new object[1]
    {
      (object) argument
    });
  }

  [MessageTemplateFormatMethod("message")]
  public void Trace<TArgument>([Localizable(false)] string message, TArgument argument)
  {
    if (!this.IsTraceEnabled)
      return;
    if (this._configuration.ExceptionLoggingOldStyle && argument is Exception exception)
      this.Trace(message, exception);
    else
      this.WriteToTargets(LogLevel.Trace, message, new object[1]
      {
        (object) argument
      });
  }

  [MessageTemplateFormatMethod("message")]
  public void Trace<TArgument1, TArgument2>(
    IFormatProvider formatProvider,
    [Localizable(false)] string message,
    TArgument1 argument1,
    TArgument2 argument2)
  {
    if (!this.IsTraceEnabled)
      return;
    this.WriteToTargets(LogLevel.Trace, formatProvider, message, new object[2]
    {
      (object) argument1,
      (object) argument2
    });
  }

  [MessageTemplateFormatMethod("message")]
  public void Trace<TArgument1, TArgument2>(
    [Localizable(false)] string message,
    TArgument1 argument1,
    TArgument2 argument2)
  {
    if (!this.IsTraceEnabled)
      return;
    this.WriteToTargets(LogLevel.Trace, message, new object[2]
    {
      (object) argument1,
      (object) argument2
    });
  }

  [MessageTemplateFormatMethod("message")]
  public void Trace<TArgument1, TArgument2, TArgument3>(
    IFormatProvider formatProvider,
    [Localizable(false)] string message,
    TArgument1 argument1,
    TArgument2 argument2,
    TArgument3 argument3)
  {
    if (!this.IsTraceEnabled)
      return;
    this.WriteToTargets(LogLevel.Trace, formatProvider, message, new object[3]
    {
      (object) argument1,
      (object) argument2,
      (object) argument3
    });
  }

  [MessageTemplateFormatMethod("message")]
  public void Trace<TArgument1, TArgument2, TArgument3>(
    [Localizable(false)] string message,
    TArgument1 argument1,
    TArgument2 argument2,
    TArgument3 argument3)
  {
    if (!this.IsTraceEnabled)
      return;
    this.WriteToTargets(LogLevel.Trace, message, new object[3]
    {
      (object) argument1,
      (object) argument2,
      (object) argument3
    });
  }

  public void Debug<T>(T value)
  {
    if (!this.IsDebugEnabled)
      return;
    this.WriteToTargets<T>(LogLevel.Debug, (IFormatProvider) null, value);
  }

  public void Debug<T>(IFormatProvider formatProvider, T value)
  {
    if (!this.IsDebugEnabled)
      return;
    this.WriteToTargets<T>(LogLevel.Debug, formatProvider, value);
  }

  public void Debug(LogMessageGenerator messageFunc)
  {
    if (!this.IsDebugEnabled)
      return;
    if (messageFunc == null)
      throw new ArgumentNullException(nameof (messageFunc));
    this.WriteToTargets(LogLevel.Debug, (IFormatProvider) null, messageFunc());
  }

  [Obsolete("Use Debug(Exception exception, string message, params object[] args) method instead. Marked obsolete before v4.3.11")]
  public void DebugException([Localizable(false)] string message, Exception exception)
  {
    this.Debug(message, exception);
  }

  [MessageTemplateFormatMethod("message")]
  public void Debug(IFormatProvider formatProvider, [Localizable(false)] string message, params object[] args)
  {
    if (!this.IsDebugEnabled)
      return;
    this.WriteToTargets(LogLevel.Debug, formatProvider, message, args);
  }

  public void Debug([Localizable(false)] string message)
  {
    if (!this.IsDebugEnabled)
      return;
    this.WriteToTargets(LogLevel.Debug, (IFormatProvider) null, message);
  }

  [MessageTemplateFormatMethod("message")]
  public void Debug([Localizable(false)] string message, params object[] args)
  {
    if (!this.IsDebugEnabled)
      return;
    this.WriteToTargets(LogLevel.Debug, message, args);
  }

  [Obsolete("Use Debug(Exception exception, string message, params object[] args) method instead. Marked obsolete before v4.3.11")]
  public void Debug([Localizable(false)] string message, Exception exception)
  {
    if (!this.IsDebugEnabled)
      return;
    this.WriteToTargets(LogLevel.Debug, exception, message, (object[]) null);
  }

  public void Debug(Exception exception, [Localizable(false)] string message)
  {
    if (!this.IsDebugEnabled)
      return;
    this.WriteToTargets(LogLevel.Debug, exception, message, (object[]) null);
  }

  [MessageTemplateFormatMethod("message")]
  public void Debug(Exception exception, [Localizable(false)] string message, params object[] args)
  {
    if (!this.IsDebugEnabled)
      return;
    this.WriteToTargets(LogLevel.Debug, exception, message, args);
  }

  [MessageTemplateFormatMethod("message")]
  public void Debug(
    Exception exception,
    IFormatProvider formatProvider,
    [Localizable(false)] string message,
    params object[] args)
  {
    if (!this.IsDebugEnabled)
      return;
    this.WriteToTargets(LogLevel.Debug, exception, formatProvider, message, args);
  }

  [MessageTemplateFormatMethod("message")]
  public void Debug<TArgument>(IFormatProvider formatProvider, [Localizable(false)] string message, TArgument argument)
  {
    if (!this.IsDebugEnabled)
      return;
    this.WriteToTargets(LogLevel.Debug, formatProvider, message, new object[1]
    {
      (object) argument
    });
  }

  [MessageTemplateFormatMethod("message")]
  public void Debug<TArgument>([Localizable(false)] string message, TArgument argument)
  {
    if (!this.IsDebugEnabled)
      return;
    if (this._configuration.ExceptionLoggingOldStyle && argument is Exception exception)
      this.Debug(message, exception);
    else
      this.WriteToTargets(LogLevel.Debug, message, new object[1]
      {
        (object) argument
      });
  }

  [MessageTemplateFormatMethod("message")]
  public void Debug<TArgument1, TArgument2>(
    IFormatProvider formatProvider,
    [Localizable(false)] string message,
    TArgument1 argument1,
    TArgument2 argument2)
  {
    if (!this.IsDebugEnabled)
      return;
    this.WriteToTargets(LogLevel.Debug, formatProvider, message, new object[2]
    {
      (object) argument1,
      (object) argument2
    });
  }

  [MessageTemplateFormatMethod("message")]
  public void Debug<TArgument1, TArgument2>(
    [Localizable(false)] string message,
    TArgument1 argument1,
    TArgument2 argument2)
  {
    if (!this.IsDebugEnabled)
      return;
    this.WriteToTargets(LogLevel.Debug, message, new object[2]
    {
      (object) argument1,
      (object) argument2
    });
  }

  [MessageTemplateFormatMethod("message")]
  public void Debug<TArgument1, TArgument2, TArgument3>(
    IFormatProvider formatProvider,
    [Localizable(false)] string message,
    TArgument1 argument1,
    TArgument2 argument2,
    TArgument3 argument3)
  {
    if (!this.IsDebugEnabled)
      return;
    this.WriteToTargets(LogLevel.Debug, formatProvider, message, new object[3]
    {
      (object) argument1,
      (object) argument2,
      (object) argument3
    });
  }

  [MessageTemplateFormatMethod("message")]
  public void Debug<TArgument1, TArgument2, TArgument3>(
    [Localizable(false)] string message,
    TArgument1 argument1,
    TArgument2 argument2,
    TArgument3 argument3)
  {
    if (!this.IsDebugEnabled)
      return;
    this.WriteToTargets(LogLevel.Debug, message, new object[3]
    {
      (object) argument1,
      (object) argument2,
      (object) argument3
    });
  }

  public void Info<T>(T value)
  {
    if (!this.IsInfoEnabled)
      return;
    this.WriteToTargets<T>(LogLevel.Info, (IFormatProvider) null, value);
  }

  public void Info<T>(IFormatProvider formatProvider, T value)
  {
    if (!this.IsInfoEnabled)
      return;
    this.WriteToTargets<T>(LogLevel.Info, formatProvider, value);
  }

  public void Info(LogMessageGenerator messageFunc)
  {
    if (!this.IsInfoEnabled)
      return;
    if (messageFunc == null)
      throw new ArgumentNullException(nameof (messageFunc));
    this.WriteToTargets(LogLevel.Info, (IFormatProvider) null, messageFunc());
  }

  [Obsolete("Use Info(Exception exception, string message, params object[] args) method instead. Marked obsolete before v4.3.11")]
  public void InfoException([Localizable(false)] string message, Exception exception)
  {
    this.Info(message, exception);
  }

  [MessageTemplateFormatMethod("message")]
  public void Info(IFormatProvider formatProvider, [Localizable(false)] string message, params object[] args)
  {
    if (!this.IsInfoEnabled)
      return;
    this.WriteToTargets(LogLevel.Info, formatProvider, message, args);
  }

  public void Info([Localizable(false)] string message)
  {
    if (!this.IsInfoEnabled)
      return;
    this.WriteToTargets(LogLevel.Info, (IFormatProvider) null, message);
  }

  [MessageTemplateFormatMethod("message")]
  public void Info([Localizable(false)] string message, params object[] args)
  {
    if (!this.IsInfoEnabled)
      return;
    this.WriteToTargets(LogLevel.Info, message, args);
  }

  [Obsolete("Use Info(Exception exception, string message, params object[] args) method instead. Marked obsolete before v4.3.11")]
  public void Info([Localizable(false)] string message, Exception exception)
  {
    if (!this.IsInfoEnabled)
      return;
    this.WriteToTargets(LogLevel.Info, exception, message, (object[]) null);
  }

  public void Info(Exception exception, [Localizable(false)] string message)
  {
    if (!this.IsInfoEnabled)
      return;
    this.WriteToTargets(LogLevel.Info, exception, message, (object[]) null);
  }

  [MessageTemplateFormatMethod("message")]
  public void Info(Exception exception, [Localizable(false)] string message, params object[] args)
  {
    if (!this.IsInfoEnabled)
      return;
    this.WriteToTargets(LogLevel.Info, exception, message, args);
  }

  [MessageTemplateFormatMethod("message")]
  public void Info(
    Exception exception,
    IFormatProvider formatProvider,
    [Localizable(false)] string message,
    params object[] args)
  {
    if (!this.IsInfoEnabled)
      return;
    this.WriteToTargets(LogLevel.Info, exception, formatProvider, message, args);
  }

  [MessageTemplateFormatMethod("message")]
  public void Info<TArgument>(IFormatProvider formatProvider, [Localizable(false)] string message, TArgument argument)
  {
    if (!this.IsInfoEnabled)
      return;
    this.WriteToTargets(LogLevel.Info, formatProvider, message, new object[1]
    {
      (object) argument
    });
  }

  [MessageTemplateFormatMethod("message")]
  public void Info<TArgument>([Localizable(false)] string message, TArgument argument)
  {
    if (!this.IsInfoEnabled)
      return;
    if (this._configuration.ExceptionLoggingOldStyle && argument is Exception exception)
      this.Info(message, exception);
    else
      this.WriteToTargets(LogLevel.Info, message, new object[1]
      {
        (object) argument
      });
  }

  [MessageTemplateFormatMethod("message")]
  public void Info<TArgument1, TArgument2>(
    IFormatProvider formatProvider,
    [Localizable(false)] string message,
    TArgument1 argument1,
    TArgument2 argument2)
  {
    if (!this.IsInfoEnabled)
      return;
    this.WriteToTargets(LogLevel.Info, formatProvider, message, new object[2]
    {
      (object) argument1,
      (object) argument2
    });
  }

  [MessageTemplateFormatMethod("message")]
  public void Info<TArgument1, TArgument2>(
    [Localizable(false)] string message,
    TArgument1 argument1,
    TArgument2 argument2)
  {
    if (!this.IsInfoEnabled)
      return;
    this.WriteToTargets(LogLevel.Info, message, new object[2]
    {
      (object) argument1,
      (object) argument2
    });
  }

  [MessageTemplateFormatMethod("message")]
  public void Info<TArgument1, TArgument2, TArgument3>(
    IFormatProvider formatProvider,
    [Localizable(false)] string message,
    TArgument1 argument1,
    TArgument2 argument2,
    TArgument3 argument3)
  {
    if (!this.IsInfoEnabled)
      return;
    this.WriteToTargets(LogLevel.Info, formatProvider, message, new object[3]
    {
      (object) argument1,
      (object) argument2,
      (object) argument3
    });
  }

  [MessageTemplateFormatMethod("message")]
  public void Info<TArgument1, TArgument2, TArgument3>(
    [Localizable(false)] string message,
    TArgument1 argument1,
    TArgument2 argument2,
    TArgument3 argument3)
  {
    if (!this.IsInfoEnabled)
      return;
    this.WriteToTargets(LogLevel.Info, message, new object[3]
    {
      (object) argument1,
      (object) argument2,
      (object) argument3
    });
  }

  public void Warn<T>(T value)
  {
    if (!this.IsWarnEnabled)
      return;
    this.WriteToTargets<T>(LogLevel.Warn, (IFormatProvider) null, value);
  }

  public void Warn<T>(IFormatProvider formatProvider, T value)
  {
    if (!this.IsWarnEnabled)
      return;
    this.WriteToTargets<T>(LogLevel.Warn, formatProvider, value);
  }

  public void Warn(LogMessageGenerator messageFunc)
  {
    if (!this.IsWarnEnabled)
      return;
    if (messageFunc == null)
      throw new ArgumentNullException(nameof (messageFunc));
    this.WriteToTargets(LogLevel.Warn, (IFormatProvider) null, messageFunc());
  }

  [Obsolete("Use Warn(Exception exception, string message, params object[] args) method instead. Marked obsolete before v4.3.11")]
  public void WarnException([Localizable(false)] string message, Exception exception)
  {
    this.Warn(message, exception);
  }

  [MessageTemplateFormatMethod("message")]
  public void Warn(IFormatProvider formatProvider, [Localizable(false)] string message, params object[] args)
  {
    if (!this.IsWarnEnabled)
      return;
    this.WriteToTargets(LogLevel.Warn, formatProvider, message, args);
  }

  public void Warn([Localizable(false)] string message)
  {
    if (!this.IsWarnEnabled)
      return;
    this.WriteToTargets(LogLevel.Warn, (IFormatProvider) null, message);
  }

  [MessageTemplateFormatMethod("message")]
  public void Warn([Localizable(false)] string message, params object[] args)
  {
    if (!this.IsWarnEnabled)
      return;
    this.WriteToTargets(LogLevel.Warn, message, args);
  }

  [Obsolete("Use Warn(Exception exception, string message, params object[] args) method instead. Marked obsolete before v4.3.11")]
  public void Warn([Localizable(false)] string message, Exception exception)
  {
    if (!this.IsWarnEnabled)
      return;
    this.WriteToTargets(LogLevel.Warn, exception, message, (object[]) null);
  }

  public void Warn(Exception exception, [Localizable(false)] string message)
  {
    if (!this.IsWarnEnabled)
      return;
    this.WriteToTargets(LogLevel.Warn, exception, message, (object[]) null);
  }

  [MessageTemplateFormatMethod("message")]
  public void Warn(Exception exception, [Localizable(false)] string message, params object[] args)
  {
    if (!this.IsWarnEnabled)
      return;
    this.WriteToTargets(LogLevel.Warn, exception, message, args);
  }

  [MessageTemplateFormatMethod("message")]
  public void Warn(
    Exception exception,
    IFormatProvider formatProvider,
    [Localizable(false)] string message,
    params object[] args)
  {
    if (!this.IsWarnEnabled)
      return;
    this.WriteToTargets(LogLevel.Warn, exception, formatProvider, message, args);
  }

  [MessageTemplateFormatMethod("message")]
  public void Warn<TArgument>(IFormatProvider formatProvider, [Localizable(false)] string message, TArgument argument)
  {
    if (!this.IsWarnEnabled)
      return;
    this.WriteToTargets(LogLevel.Warn, formatProvider, message, new object[1]
    {
      (object) argument
    });
  }

  [MessageTemplateFormatMethod("message")]
  public void Warn<TArgument>([Localizable(false)] string message, TArgument argument)
  {
    if (!this.IsWarnEnabled)
      return;
    if (this._configuration.ExceptionLoggingOldStyle && argument is Exception exception)
      this.Warn(message, exception);
    else
      this.WriteToTargets(LogLevel.Warn, message, new object[1]
      {
        (object) argument
      });
  }

  [MessageTemplateFormatMethod("message")]
  public void Warn<TArgument1, TArgument2>(
    IFormatProvider formatProvider,
    [Localizable(false)] string message,
    TArgument1 argument1,
    TArgument2 argument2)
  {
    if (!this.IsWarnEnabled)
      return;
    this.WriteToTargets(LogLevel.Warn, formatProvider, message, new object[2]
    {
      (object) argument1,
      (object) argument2
    });
  }

  [MessageTemplateFormatMethod("message")]
  public void Warn<TArgument1, TArgument2>(
    [Localizable(false)] string message,
    TArgument1 argument1,
    TArgument2 argument2)
  {
    if (!this.IsWarnEnabled)
      return;
    this.WriteToTargets(LogLevel.Warn, message, new object[2]
    {
      (object) argument1,
      (object) argument2
    });
  }

  [MessageTemplateFormatMethod("message")]
  public void Warn<TArgument1, TArgument2, TArgument3>(
    IFormatProvider formatProvider,
    [Localizable(false)] string message,
    TArgument1 argument1,
    TArgument2 argument2,
    TArgument3 argument3)
  {
    if (!this.IsWarnEnabled)
      return;
    this.WriteToTargets(LogLevel.Warn, formatProvider, message, new object[3]
    {
      (object) argument1,
      (object) argument2,
      (object) argument3
    });
  }

  [MessageTemplateFormatMethod("message")]
  public void Warn<TArgument1, TArgument2, TArgument3>(
    [Localizable(false)] string message,
    TArgument1 argument1,
    TArgument2 argument2,
    TArgument3 argument3)
  {
    if (!this.IsWarnEnabled)
      return;
    this.WriteToTargets(LogLevel.Warn, message, new object[3]
    {
      (object) argument1,
      (object) argument2,
      (object) argument3
    });
  }

  public void Error<T>(T value)
  {
    if (!this.IsErrorEnabled)
      return;
    this.WriteToTargets<T>(LogLevel.Error, (IFormatProvider) null, value);
  }

  public void Error<T>(IFormatProvider formatProvider, T value)
  {
    if (!this.IsErrorEnabled)
      return;
    this.WriteToTargets<T>(LogLevel.Error, formatProvider, value);
  }

  public void Error(LogMessageGenerator messageFunc)
  {
    if (!this.IsErrorEnabled)
      return;
    if (messageFunc == null)
      throw new ArgumentNullException(nameof (messageFunc));
    this.WriteToTargets(LogLevel.Error, (IFormatProvider) null, messageFunc());
  }

  [Obsolete("Use Error(Exception exception, string message, params object[] args) method instead. Marked obsolete before v4.3.11")]
  public void ErrorException([Localizable(false)] string message, Exception exception)
  {
    this.Error(message, exception);
  }

  [MessageTemplateFormatMethod("message")]
  public void Error(IFormatProvider formatProvider, [Localizable(false)] string message, params object[] args)
  {
    if (!this.IsErrorEnabled)
      return;
    this.WriteToTargets(LogLevel.Error, formatProvider, message, args);
  }

  public void Error([Localizable(false)] string message)
  {
    if (!this.IsErrorEnabled)
      return;
    this.WriteToTargets(LogLevel.Error, (IFormatProvider) null, message);
  }

  [MessageTemplateFormatMethod("message")]
  public void Error([Localizable(false)] string message, params object[] args)
  {
    if (!this.IsErrorEnabled)
      return;
    this.WriteToTargets(LogLevel.Error, message, args);
  }

  [Obsolete("Use Error(Exception exception, string message, params object[] args) method instead. Marked obsolete before v4.3.11")]
  public void Error([Localizable(false)] string message, Exception exception)
  {
    if (!this.IsErrorEnabled)
      return;
    this.WriteToTargets(LogLevel.Error, exception, message, (object[]) null);
  }

  public void Error(Exception exception, [Localizable(false)] string message)
  {
    if (!this.IsErrorEnabled)
      return;
    this.WriteToTargets(LogLevel.Error, exception, message, (object[]) null);
  }

  [MessageTemplateFormatMethod("message")]
  public void Error(Exception exception, [Localizable(false)] string message, params object[] args)
  {
    if (!this.IsErrorEnabled)
      return;
    this.WriteToTargets(LogLevel.Error, exception, message, args);
  }

  [MessageTemplateFormatMethod("message")]
  public void Error(
    Exception exception,
    IFormatProvider formatProvider,
    [Localizable(false)] string message,
    params object[] args)
  {
    if (!this.IsErrorEnabled)
      return;
    this.WriteToTargets(LogLevel.Error, exception, formatProvider, message, args);
  }

  [MessageTemplateFormatMethod("message")]
  public void Error<TArgument>(IFormatProvider formatProvider, [Localizable(false)] string message, TArgument argument)
  {
    if (!this.IsErrorEnabled)
      return;
    this.WriteToTargets(LogLevel.Error, formatProvider, message, new object[1]
    {
      (object) argument
    });
  }

  [MessageTemplateFormatMethod("message")]
  public void Error<TArgument>([Localizable(false)] string message, TArgument argument)
  {
    if (!this.IsErrorEnabled)
      return;
    if (this._configuration.ExceptionLoggingOldStyle && argument is Exception exception)
      this.Error(message, exception);
    else
      this.WriteToTargets(LogLevel.Error, message, new object[1]
      {
        (object) argument
      });
  }

  [MessageTemplateFormatMethod("message")]
  public void Error<TArgument1, TArgument2>(
    IFormatProvider formatProvider,
    [Localizable(false)] string message,
    TArgument1 argument1,
    TArgument2 argument2)
  {
    if (!this.IsErrorEnabled)
      return;
    this.WriteToTargets(LogLevel.Error, formatProvider, message, new object[2]
    {
      (object) argument1,
      (object) argument2
    });
  }

  [MessageTemplateFormatMethod("message")]
  public void Error<TArgument1, TArgument2>(
    [Localizable(false)] string message,
    TArgument1 argument1,
    TArgument2 argument2)
  {
    if (!this.IsErrorEnabled)
      return;
    this.WriteToTargets(LogLevel.Error, message, new object[2]
    {
      (object) argument1,
      (object) argument2
    });
  }

  [MessageTemplateFormatMethod("message")]
  public void Error<TArgument1, TArgument2, TArgument3>(
    IFormatProvider formatProvider,
    [Localizable(false)] string message,
    TArgument1 argument1,
    TArgument2 argument2,
    TArgument3 argument3)
  {
    if (!this.IsErrorEnabled)
      return;
    this.WriteToTargets(LogLevel.Error, formatProvider, message, new object[3]
    {
      (object) argument1,
      (object) argument2,
      (object) argument3
    });
  }

  [MessageTemplateFormatMethod("message")]
  public void Error<TArgument1, TArgument2, TArgument3>(
    [Localizable(false)] string message,
    TArgument1 argument1,
    TArgument2 argument2,
    TArgument3 argument3)
  {
    if (!this.IsErrorEnabled)
      return;
    this.WriteToTargets(LogLevel.Error, message, new object[3]
    {
      (object) argument1,
      (object) argument2,
      (object) argument3
    });
  }

  public void Fatal<T>(T value)
  {
    if (!this.IsFatalEnabled)
      return;
    this.WriteToTargets<T>(LogLevel.Fatal, (IFormatProvider) null, value);
  }

  public void Fatal<T>(IFormatProvider formatProvider, T value)
  {
    if (!this.IsFatalEnabled)
      return;
    this.WriteToTargets<T>(LogLevel.Fatal, formatProvider, value);
  }

  public void Fatal(LogMessageGenerator messageFunc)
  {
    if (!this.IsFatalEnabled)
      return;
    if (messageFunc == null)
      throw new ArgumentNullException(nameof (messageFunc));
    this.WriteToTargets(LogLevel.Fatal, (IFormatProvider) null, messageFunc());
  }

  [Obsolete("Use Fatal(Exception exception, string message, params object[] args) method instead. Marked obsolete before v4.3.11")]
  public void FatalException([Localizable(false)] string message, Exception exception)
  {
    this.Fatal(message, exception);
  }

  [MessageTemplateFormatMethod("message")]
  public void Fatal(IFormatProvider formatProvider, [Localizable(false)] string message, params object[] args)
  {
    if (!this.IsFatalEnabled)
      return;
    this.WriteToTargets(LogLevel.Fatal, formatProvider, message, args);
  }

  public void Fatal([Localizable(false)] string message)
  {
    if (!this.IsFatalEnabled)
      return;
    this.WriteToTargets(LogLevel.Fatal, (IFormatProvider) null, message);
  }

  [MessageTemplateFormatMethod("message")]
  public void Fatal([Localizable(false)] string message, params object[] args)
  {
    if (!this.IsFatalEnabled)
      return;
    this.WriteToTargets(LogLevel.Fatal, message, args);
  }

  [Obsolete("Use Fatal(Exception exception, string message, params object[] args) method instead. Marked obsolete before v4.3.11")]
  public void Fatal([Localizable(false)] string message, Exception exception)
  {
    if (!this.IsFatalEnabled)
      return;
    this.WriteToTargets(LogLevel.Fatal, exception, message, (object[]) null);
  }

  public void Fatal(Exception exception, [Localizable(false)] string message)
  {
    if (!this.IsFatalEnabled)
      return;
    this.WriteToTargets(LogLevel.Fatal, exception, message, (object[]) null);
  }

  [MessageTemplateFormatMethod("message")]
  public void Fatal(Exception exception, [Localizable(false)] string message, params object[] args)
  {
    if (!this.IsFatalEnabled)
      return;
    this.WriteToTargets(LogLevel.Fatal, exception, message, args);
  }

  [MessageTemplateFormatMethod("message")]
  public void Fatal(
    Exception exception,
    IFormatProvider formatProvider,
    [Localizable(false)] string message,
    params object[] args)
  {
    if (!this.IsFatalEnabled)
      return;
    this.WriteToTargets(LogLevel.Fatal, exception, formatProvider, message, args);
  }

  [MessageTemplateFormatMethod("message")]
  public void Fatal<TArgument>(IFormatProvider formatProvider, [Localizable(false)] string message, TArgument argument)
  {
    if (!this.IsFatalEnabled)
      return;
    this.WriteToTargets(LogLevel.Fatal, formatProvider, message, new object[1]
    {
      (object) argument
    });
  }

  [MessageTemplateFormatMethod("message")]
  public void Fatal<TArgument>([Localizable(false)] string message, TArgument argument)
  {
    if (!this.IsFatalEnabled)
      return;
    if (this._configuration.ExceptionLoggingOldStyle && argument is Exception exception)
      this.Fatal(message, exception);
    else
      this.WriteToTargets(LogLevel.Fatal, message, new object[1]
      {
        (object) argument
      });
  }

  [MessageTemplateFormatMethod("message")]
  public void Fatal<TArgument1, TArgument2>(
    IFormatProvider formatProvider,
    [Localizable(false)] string message,
    TArgument1 argument1,
    TArgument2 argument2)
  {
    if (!this.IsFatalEnabled)
      return;
    this.WriteToTargets(LogLevel.Fatal, formatProvider, message, new object[2]
    {
      (object) argument1,
      (object) argument2
    });
  }

  [MessageTemplateFormatMethod("message")]
  public void Fatal<TArgument1, TArgument2>(
    [Localizable(false)] string message,
    TArgument1 argument1,
    TArgument2 argument2)
  {
    if (!this.IsFatalEnabled)
      return;
    this.WriteToTargets(LogLevel.Fatal, message, new object[2]
    {
      (object) argument1,
      (object) argument2
    });
  }

  [MessageTemplateFormatMethod("message")]
  public void Fatal<TArgument1, TArgument2, TArgument3>(
    IFormatProvider formatProvider,
    [Localizable(false)] string message,
    TArgument1 argument1,
    TArgument2 argument2,
    TArgument3 argument3)
  {
    if (!this.IsFatalEnabled)
      return;
    this.WriteToTargets(LogLevel.Fatal, formatProvider, message, new object[3]
    {
      (object) argument1,
      (object) argument2,
      (object) argument3
    });
  }

  [MessageTemplateFormatMethod("message")]
  public void Fatal<TArgument1, TArgument2, TArgument3>(
    [Localizable(false)] string message,
    TArgument1 argument1,
    TArgument2 argument2,
    TArgument3 argument3)
  {
    if (!this.IsFatalEnabled)
      return;
    this.WriteToTargets(LogLevel.Fatal, message, new object[3]
    {
      (object) argument1,
      (object) argument2,
      (object) argument3
    });
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  public void Log(LogLevel level, object value)
  {
    if (!this.IsEnabled(level))
      return;
    this.WriteToTargets<object>(level, (IFormatProvider) null, value);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  public void Log(LogLevel level, IFormatProvider formatProvider, object value)
  {
    if (!this.IsEnabled(level))
      return;
    this.WriteToTargets<object>(level, formatProvider, value);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  public void Log(LogLevel level, string message, object arg1, object arg2)
  {
    if (!this.IsEnabled(level))
      return;
    this.WriteToTargets(level, message, new object[2]
    {
      arg1,
      arg2
    });
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  public void Log(LogLevel level, string message, object arg1, object arg2, object arg3)
  {
    if (!this.IsEnabled(level))
      return;
    this.WriteToTargets(level, message, new object[3]
    {
      arg1,
      arg2,
      arg3
    });
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  public void Log(LogLevel level, IFormatProvider formatProvider, string message, bool argument)
  {
    if (!this.IsEnabled(level))
      return;
    this.WriteToTargets(level, formatProvider, message, new object[1]
    {
      (object) argument
    });
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  public void Log(LogLevel level, string message, bool argument)
  {
    if (!this.IsEnabled(level))
      return;
    this.WriteToTargets(level, message, new object[1]
    {
      (object) argument
    });
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  public void Log(LogLevel level, IFormatProvider formatProvider, string message, char argument)
  {
    if (!this.IsEnabled(level))
      return;
    this.WriteToTargets(level, formatProvider, message, new object[1]
    {
      (object) argument
    });
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  public void Log(LogLevel level, string message, char argument)
  {
    if (!this.IsEnabled(level))
      return;
    this.WriteToTargets(level, message, new object[1]
    {
      (object) argument
    });
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  public void Log(LogLevel level, IFormatProvider formatProvider, string message, byte argument)
  {
    if (!this.IsEnabled(level))
      return;
    this.WriteToTargets(level, formatProvider, message, new object[1]
    {
      (object) argument
    });
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  public void Log(LogLevel level, string message, byte argument)
  {
    if (!this.IsEnabled(level))
      return;
    this.WriteToTargets(level, message, new object[1]
    {
      (object) argument
    });
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  public void Log(LogLevel level, IFormatProvider formatProvider, string message, string argument)
  {
    if (!this.IsEnabled(level))
      return;
    this.WriteToTargets(level, formatProvider, message, new object[1]
    {
      (object) argument
    });
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  public void Log(LogLevel level, string message, string argument)
  {
    if (!this.IsEnabled(level))
      return;
    this.WriteToTargets(level, message, new object[1]
    {
      (object) argument
    });
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  public void Log(LogLevel level, IFormatProvider formatProvider, string message, int argument)
  {
    if (!this.IsEnabled(level))
      return;
    this.WriteToTargets(level, formatProvider, message, new object[1]
    {
      (object) argument
    });
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  public void Log(LogLevel level, string message, int argument)
  {
    if (!this.IsEnabled(level))
      return;
    this.WriteToTargets(level, message, new object[1]
    {
      (object) argument
    });
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  public void Log(LogLevel level, IFormatProvider formatProvider, string message, long argument)
  {
    if (!this.IsEnabled(level))
      return;
    this.WriteToTargets(level, formatProvider, message, new object[1]
    {
      (object) argument
    });
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  public void Log(LogLevel level, string message, long argument)
  {
    if (!this.IsEnabled(level))
      return;
    this.WriteToTargets(level, message, new object[1]
    {
      (object) argument
    });
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  public void Log(LogLevel level, IFormatProvider formatProvider, string message, float argument)
  {
    if (!this.IsEnabled(level))
      return;
    this.WriteToTargets(level, formatProvider, message, new object[1]
    {
      (object) argument
    });
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  public void Log(LogLevel level, string message, float argument)
  {
    if (!this.IsEnabled(level))
      return;
    this.WriteToTargets(level, message, new object[1]
    {
      (object) argument
    });
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  public void Log(LogLevel level, IFormatProvider formatProvider, string message, double argument)
  {
    if (!this.IsEnabled(level))
      return;
    this.WriteToTargets(level, formatProvider, message, new object[1]
    {
      (object) argument
    });
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  public void Log(LogLevel level, string message, double argument)
  {
    if (!this.IsEnabled(level))
      return;
    this.WriteToTargets(level, message, new object[1]
    {
      (object) argument
    });
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  public void Log(
    LogLevel level,
    IFormatProvider formatProvider,
    string message,
    Decimal argument)
  {
    if (!this.IsEnabled(level))
      return;
    this.WriteToTargets(level, formatProvider, message, new object[1]
    {
      (object) argument
    });
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  public void Log(LogLevel level, string message, Decimal argument)
  {
    if (!this.IsEnabled(level))
      return;
    this.WriteToTargets(level, message, new object[1]
    {
      (object) argument
    });
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  public void Log(LogLevel level, IFormatProvider formatProvider, string message, object argument)
  {
    if (!this.IsEnabled(level))
      return;
    this.WriteToTargets(level, formatProvider, message, new object[1]
    {
      argument
    });
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  public void Log(LogLevel level, string message, object argument)
  {
    if (!this.IsEnabled(level))
      return;
    this.WriteToTargets(level, message, new object[1]
    {
      argument
    });
  }

  [CLSCompliant(false)]
  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  public void Log(LogLevel level, IFormatProvider formatProvider, string message, sbyte argument)
  {
    if (!this.IsEnabled(level))
      return;
    this.WriteToTargets(level, formatProvider, message, new object[1]
    {
      (object) argument
    });
  }

  [CLSCompliant(false)]
  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  public void Log(LogLevel level, string message, sbyte argument)
  {
    if (!this.IsEnabled(level))
      return;
    this.WriteToTargets(level, message, new object[1]
    {
      (object) argument
    });
  }

  [CLSCompliant(false)]
  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  public void Log(LogLevel level, IFormatProvider formatProvider, string message, uint argument)
  {
    if (!this.IsEnabled(level))
      return;
    this.WriteToTargets(level, formatProvider, message, new object[1]
    {
      (object) argument
    });
  }

  [CLSCompliant(false)]
  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  public void Log(LogLevel level, string message, uint argument)
  {
    if (!this.IsEnabled(level))
      return;
    this.WriteToTargets(level, message, new object[1]
    {
      (object) argument
    });
  }

  [CLSCompliant(false)]
  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  public void Log(LogLevel level, IFormatProvider formatProvider, string message, ulong argument)
  {
    if (!this.IsEnabled(level))
      return;
    this.WriteToTargets(level, formatProvider, message, new object[1]
    {
      (object) argument
    });
  }

  [CLSCompliant(false)]
  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  public void Log(LogLevel level, string message, ulong argument)
  {
    if (!this.IsEnabled(level))
      return;
    this.WriteToTargets(level, message, new object[1]
    {
      (object) argument
    });
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  public void Trace(object value)
  {
    if (!this.IsTraceEnabled)
      return;
    this.WriteToTargets<object>(LogLevel.Trace, (IFormatProvider) null, value);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  public void Trace(IFormatProvider formatProvider, object value)
  {
    if (!this.IsTraceEnabled)
      return;
    this.WriteToTargets<object>(LogLevel.Trace, formatProvider, value);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  public void Trace(string message, object arg1, object arg2)
  {
    if (!this.IsTraceEnabled)
      return;
    this.WriteToTargets(LogLevel.Trace, message, new object[2]
    {
      arg1,
      arg2
    });
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  public void Trace(string message, object arg1, object arg2, object arg3)
  {
    if (!this.IsTraceEnabled)
      return;
    this.WriteToTargets(LogLevel.Trace, message, new object[3]
    {
      arg1,
      arg2,
      arg3
    });
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  public void Trace(IFormatProvider formatProvider, string message, bool argument)
  {
    if (!this.IsTraceEnabled)
      return;
    this.WriteToTargets(LogLevel.Trace, formatProvider, message, new object[1]
    {
      (object) argument
    });
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  public void Trace(string message, bool argument)
  {
    if (!this.IsTraceEnabled)
      return;
    this.WriteToTargets(LogLevel.Trace, message, new object[1]
    {
      (object) argument
    });
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  public void Trace(IFormatProvider formatProvider, string message, char argument)
  {
    if (!this.IsTraceEnabled)
      return;
    this.WriteToTargets(LogLevel.Trace, formatProvider, message, new object[1]
    {
      (object) argument
    });
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  public void Trace(string message, char argument)
  {
    if (!this.IsTraceEnabled)
      return;
    this.WriteToTargets(LogLevel.Trace, message, new object[1]
    {
      (object) argument
    });
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  public void Trace(IFormatProvider formatProvider, string message, byte argument)
  {
    if (!this.IsTraceEnabled)
      return;
    this.WriteToTargets(LogLevel.Trace, formatProvider, message, new object[1]
    {
      (object) argument
    });
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  public void Trace(string message, byte argument)
  {
    if (!this.IsTraceEnabled)
      return;
    this.WriteToTargets(LogLevel.Trace, message, new object[1]
    {
      (object) argument
    });
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  public void Trace(IFormatProvider formatProvider, string message, string argument)
  {
    if (!this.IsTraceEnabled)
      return;
    this.WriteToTargets(LogLevel.Trace, formatProvider, message, new object[1]
    {
      (object) argument
    });
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  public void Trace(string message, string argument)
  {
    if (!this.IsTraceEnabled)
      return;
    this.WriteToTargets(LogLevel.Trace, message, new object[1]
    {
      (object) argument
    });
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  public void Trace(IFormatProvider formatProvider, string message, int argument)
  {
    if (!this.IsTraceEnabled)
      return;
    this.WriteToTargets(LogLevel.Trace, formatProvider, message, new object[1]
    {
      (object) argument
    });
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  public void Trace(string message, int argument)
  {
    if (!this.IsTraceEnabled)
      return;
    this.WriteToTargets(LogLevel.Trace, message, new object[1]
    {
      (object) argument
    });
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  public void Trace(IFormatProvider formatProvider, string message, long argument)
  {
    if (!this.IsTraceEnabled)
      return;
    this.WriteToTargets(LogLevel.Trace, formatProvider, message, new object[1]
    {
      (object) argument
    });
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  public void Trace(string message, long argument)
  {
    if (!this.IsTraceEnabled)
      return;
    this.WriteToTargets(LogLevel.Trace, message, new object[1]
    {
      (object) argument
    });
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  public void Trace(IFormatProvider formatProvider, string message, float argument)
  {
    if (!this.IsTraceEnabled)
      return;
    this.WriteToTargets(LogLevel.Trace, formatProvider, message, new object[1]
    {
      (object) argument
    });
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  public void Trace(string message, float argument)
  {
    if (!this.IsTraceEnabled)
      return;
    this.WriteToTargets(LogLevel.Trace, message, new object[1]
    {
      (object) argument
    });
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  public void Trace(IFormatProvider formatProvider, string message, double argument)
  {
    if (!this.IsTraceEnabled)
      return;
    this.WriteToTargets(LogLevel.Trace, formatProvider, message, new object[1]
    {
      (object) argument
    });
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  public void Trace(string message, double argument)
  {
    if (!this.IsTraceEnabled)
      return;
    this.WriteToTargets(LogLevel.Trace, message, new object[1]
    {
      (object) argument
    });
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  public void Trace(IFormatProvider formatProvider, string message, Decimal argument)
  {
    if (!this.IsTraceEnabled)
      return;
    this.WriteToTargets(LogLevel.Trace, formatProvider, message, new object[1]
    {
      (object) argument
    });
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  public void Trace(string message, Decimal argument)
  {
    if (!this.IsTraceEnabled)
      return;
    this.WriteToTargets(LogLevel.Trace, message, new object[1]
    {
      (object) argument
    });
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  public void Trace(IFormatProvider formatProvider, string message, object argument)
  {
    if (!this.IsTraceEnabled)
      return;
    this.WriteToTargets(LogLevel.Trace, formatProvider, message, new object[1]
    {
      argument
    });
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  public void Trace(string message, object argument)
  {
    if (!this.IsTraceEnabled)
      return;
    this.WriteToTargets(LogLevel.Trace, message, new object[1]
    {
      argument
    });
  }

  [CLSCompliant(false)]
  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  public void Trace(IFormatProvider formatProvider, string message, sbyte argument)
  {
    if (!this.IsTraceEnabled)
      return;
    this.WriteToTargets(LogLevel.Trace, formatProvider, message, new object[1]
    {
      (object) argument
    });
  }

  [CLSCompliant(false)]
  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  public void Trace(string message, sbyte argument)
  {
    if (!this.IsTraceEnabled)
      return;
    this.WriteToTargets(LogLevel.Trace, message, new object[1]
    {
      (object) argument
    });
  }

  [CLSCompliant(false)]
  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  public void Trace(IFormatProvider formatProvider, string message, uint argument)
  {
    if (!this.IsTraceEnabled)
      return;
    this.WriteToTargets(LogLevel.Trace, formatProvider, message, new object[1]
    {
      (object) argument
    });
  }

  [CLSCompliant(false)]
  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  public void Trace(string message, uint argument)
  {
    if (!this.IsTraceEnabled)
      return;
    this.WriteToTargets(LogLevel.Trace, message, new object[1]
    {
      (object) argument
    });
  }

  [CLSCompliant(false)]
  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  public void Trace(IFormatProvider formatProvider, string message, ulong argument)
  {
    if (!this.IsTraceEnabled)
      return;
    this.WriteToTargets(LogLevel.Trace, formatProvider, message, new object[1]
    {
      (object) argument
    });
  }

  [CLSCompliant(false)]
  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  public void Trace(string message, ulong argument)
  {
    if (!this.IsTraceEnabled)
      return;
    this.WriteToTargets(LogLevel.Trace, message, new object[1]
    {
      (object) argument
    });
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  public void Debug(object value)
  {
    if (!this.IsDebugEnabled)
      return;
    this.WriteToTargets<object>(LogLevel.Debug, (IFormatProvider) null, value);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  public void Debug(IFormatProvider formatProvider, object value)
  {
    if (!this.IsDebugEnabled)
      return;
    this.WriteToTargets<object>(LogLevel.Debug, formatProvider, value);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  public void Debug(string message, object arg1, object arg2)
  {
    if (!this.IsDebugEnabled)
      return;
    this.WriteToTargets(LogLevel.Debug, message, new object[2]
    {
      arg1,
      arg2
    });
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  public void Debug(string message, object arg1, object arg2, object arg3)
  {
    if (!this.IsDebugEnabled)
      return;
    this.WriteToTargets(LogLevel.Debug, message, new object[3]
    {
      arg1,
      arg2,
      arg3
    });
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  public void Debug(IFormatProvider formatProvider, string message, bool argument)
  {
    if (!this.IsDebugEnabled)
      return;
    this.WriteToTargets(LogLevel.Debug, formatProvider, message, new object[1]
    {
      (object) argument
    });
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  public void Debug(string message, bool argument)
  {
    if (!this.IsDebugEnabled)
      return;
    this.WriteToTargets(LogLevel.Debug, message, new object[1]
    {
      (object) argument
    });
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  public void Debug(IFormatProvider formatProvider, string message, char argument)
  {
    if (!this.IsDebugEnabled)
      return;
    this.WriteToTargets(LogLevel.Debug, formatProvider, message, new object[1]
    {
      (object) argument
    });
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  public void Debug(string message, char argument)
  {
    if (!this.IsDebugEnabled)
      return;
    this.WriteToTargets(LogLevel.Debug, message, new object[1]
    {
      (object) argument
    });
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  public void Debug(IFormatProvider formatProvider, string message, byte argument)
  {
    if (!this.IsDebugEnabled)
      return;
    this.WriteToTargets(LogLevel.Debug, formatProvider, message, new object[1]
    {
      (object) argument
    });
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  public void Debug(string message, byte argument)
  {
    if (!this.IsDebugEnabled)
      return;
    this.WriteToTargets(LogLevel.Debug, message, new object[1]
    {
      (object) argument
    });
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  public void Debug(IFormatProvider formatProvider, string message, string argument)
  {
    if (!this.IsDebugEnabled)
      return;
    this.WriteToTargets(LogLevel.Debug, formatProvider, message, new object[1]
    {
      (object) argument
    });
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  public void Debug(string message, string argument)
  {
    if (!this.IsDebugEnabled)
      return;
    this.WriteToTargets(LogLevel.Debug, message, new object[1]
    {
      (object) argument
    });
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  public void Debug(IFormatProvider formatProvider, string message, int argument)
  {
    if (!this.IsDebugEnabled)
      return;
    this.WriteToTargets(LogLevel.Debug, formatProvider, message, new object[1]
    {
      (object) argument
    });
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  public void Debug(string message, int argument)
  {
    if (!this.IsDebugEnabled)
      return;
    this.WriteToTargets(LogLevel.Debug, message, new object[1]
    {
      (object) argument
    });
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  public void Debug(IFormatProvider formatProvider, string message, long argument)
  {
    if (!this.IsDebugEnabled)
      return;
    this.WriteToTargets(LogLevel.Debug, formatProvider, message, new object[1]
    {
      (object) argument
    });
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  public void Debug(string message, long argument)
  {
    if (!this.IsDebugEnabled)
      return;
    this.WriteToTargets(LogLevel.Debug, message, new object[1]
    {
      (object) argument
    });
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  public void Debug(IFormatProvider formatProvider, string message, float argument)
  {
    if (!this.IsDebugEnabled)
      return;
    this.WriteToTargets(LogLevel.Debug, formatProvider, message, new object[1]
    {
      (object) argument
    });
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  public void Debug(string message, float argument)
  {
    if (!this.IsDebugEnabled)
      return;
    this.WriteToTargets(LogLevel.Debug, message, new object[1]
    {
      (object) argument
    });
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  public void Debug(IFormatProvider formatProvider, string message, double argument)
  {
    if (!this.IsDebugEnabled)
      return;
    this.WriteToTargets(LogLevel.Debug, formatProvider, message, new object[1]
    {
      (object) argument
    });
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  public void Debug(string message, double argument)
  {
    if (!this.IsDebugEnabled)
      return;
    this.WriteToTargets(LogLevel.Debug, message, new object[1]
    {
      (object) argument
    });
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  public void Debug(IFormatProvider formatProvider, string message, Decimal argument)
  {
    if (!this.IsDebugEnabled)
      return;
    this.WriteToTargets(LogLevel.Debug, formatProvider, message, new object[1]
    {
      (object) argument
    });
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  public void Debug(string message, Decimal argument)
  {
    if (!this.IsDebugEnabled)
      return;
    this.WriteToTargets(LogLevel.Debug, message, new object[1]
    {
      (object) argument
    });
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  public void Debug(IFormatProvider formatProvider, string message, object argument)
  {
    if (!this.IsDebugEnabled)
      return;
    this.WriteToTargets(LogLevel.Debug, formatProvider, message, new object[1]
    {
      argument
    });
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  public void Debug(string message, object argument)
  {
    if (!this.IsDebugEnabled)
      return;
    this.WriteToTargets(LogLevel.Debug, message, new object[1]
    {
      argument
    });
  }

  [CLSCompliant(false)]
  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  public void Debug(IFormatProvider formatProvider, string message, sbyte argument)
  {
    if (!this.IsDebugEnabled)
      return;
    this.WriteToTargets(LogLevel.Debug, formatProvider, message, new object[1]
    {
      (object) argument
    });
  }

  [CLSCompliant(false)]
  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  public void Debug(string message, sbyte argument)
  {
    if (!this.IsDebugEnabled)
      return;
    this.WriteToTargets(LogLevel.Debug, message, new object[1]
    {
      (object) argument
    });
  }

  [CLSCompliant(false)]
  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  public void Debug(IFormatProvider formatProvider, string message, uint argument)
  {
    if (!this.IsDebugEnabled)
      return;
    this.WriteToTargets(LogLevel.Debug, formatProvider, message, new object[1]
    {
      (object) argument
    });
  }

  [CLSCompliant(false)]
  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  public void Debug(string message, uint argument)
  {
    if (!this.IsDebugEnabled)
      return;
    this.WriteToTargets(LogLevel.Debug, message, new object[1]
    {
      (object) argument
    });
  }

  [CLSCompliant(false)]
  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  public void Debug(IFormatProvider formatProvider, string message, ulong argument)
  {
    if (!this.IsDebugEnabled)
      return;
    this.WriteToTargets(LogLevel.Debug, formatProvider, message, new object[1]
    {
      (object) argument
    });
  }

  [CLSCompliant(false)]
  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  public void Debug(string message, ulong argument)
  {
    if (!this.IsDebugEnabled)
      return;
    this.WriteToTargets(LogLevel.Debug, message, new object[1]
    {
      (object) argument
    });
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  public void Info(object value)
  {
    if (!this.IsInfoEnabled)
      return;
    this.WriteToTargets<object>(LogLevel.Info, (IFormatProvider) null, value);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  public void Info(IFormatProvider formatProvider, object value)
  {
    if (!this.IsInfoEnabled)
      return;
    this.WriteToTargets<object>(LogLevel.Info, formatProvider, value);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  public void Info(string message, object arg1, object arg2)
  {
    if (!this.IsInfoEnabled)
      return;
    this.WriteToTargets(LogLevel.Info, message, new object[2]
    {
      arg1,
      arg2
    });
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  public void Info(string message, object arg1, object arg2, object arg3)
  {
    if (!this.IsInfoEnabled)
      return;
    this.WriteToTargets(LogLevel.Info, message, new object[3]
    {
      arg1,
      arg2,
      arg3
    });
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  public void Info(IFormatProvider formatProvider, string message, bool argument)
  {
    if (!this.IsInfoEnabled)
      return;
    this.WriteToTargets(LogLevel.Info, formatProvider, message, new object[1]
    {
      (object) argument
    });
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  public void Info(string message, bool argument)
  {
    if (!this.IsInfoEnabled)
      return;
    this.WriteToTargets(LogLevel.Info, message, new object[1]
    {
      (object) argument
    });
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  public void Info(IFormatProvider formatProvider, string message, char argument)
  {
    if (!this.IsInfoEnabled)
      return;
    this.WriteToTargets(LogLevel.Info, formatProvider, message, new object[1]
    {
      (object) argument
    });
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  public void Info(string message, char argument)
  {
    if (!this.IsInfoEnabled)
      return;
    this.WriteToTargets(LogLevel.Info, message, new object[1]
    {
      (object) argument
    });
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  public void Info(IFormatProvider formatProvider, string message, byte argument)
  {
    if (!this.IsInfoEnabled)
      return;
    this.WriteToTargets(LogLevel.Info, formatProvider, message, new object[1]
    {
      (object) argument
    });
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  public void Info(string message, byte argument)
  {
    if (!this.IsInfoEnabled)
      return;
    this.WriteToTargets(LogLevel.Info, message, new object[1]
    {
      (object) argument
    });
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  public void Info(IFormatProvider formatProvider, string message, string argument)
  {
    if (!this.IsInfoEnabled)
      return;
    this.WriteToTargets(LogLevel.Info, formatProvider, message, new object[1]
    {
      (object) argument
    });
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  public void Info(string message, string argument)
  {
    if (!this.IsInfoEnabled)
      return;
    this.WriteToTargets(LogLevel.Info, message, new object[1]
    {
      (object) argument
    });
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  public void Info(IFormatProvider formatProvider, string message, int argument)
  {
    if (!this.IsInfoEnabled)
      return;
    this.WriteToTargets(LogLevel.Info, formatProvider, message, new object[1]
    {
      (object) argument
    });
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  public void Info(string message, int argument)
  {
    if (!this.IsInfoEnabled)
      return;
    this.WriteToTargets(LogLevel.Info, message, new object[1]
    {
      (object) argument
    });
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  public void Info(IFormatProvider formatProvider, string message, long argument)
  {
    if (!this.IsInfoEnabled)
      return;
    this.WriteToTargets(LogLevel.Info, formatProvider, message, new object[1]
    {
      (object) argument
    });
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  public void Info(string message, long argument)
  {
    if (!this.IsInfoEnabled)
      return;
    this.WriteToTargets(LogLevel.Info, message, new object[1]
    {
      (object) argument
    });
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  public void Info(IFormatProvider formatProvider, string message, float argument)
  {
    if (!this.IsInfoEnabled)
      return;
    this.WriteToTargets(LogLevel.Info, formatProvider, message, new object[1]
    {
      (object) argument
    });
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  public void Info(string message, float argument)
  {
    if (!this.IsInfoEnabled)
      return;
    this.WriteToTargets(LogLevel.Info, message, new object[1]
    {
      (object) argument
    });
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  public void Info(IFormatProvider formatProvider, string message, double argument)
  {
    if (!this.IsInfoEnabled)
      return;
    this.WriteToTargets(LogLevel.Info, formatProvider, message, new object[1]
    {
      (object) argument
    });
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  public void Info(string message, double argument)
  {
    if (!this.IsInfoEnabled)
      return;
    this.WriteToTargets(LogLevel.Info, message, new object[1]
    {
      (object) argument
    });
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  public void Info(IFormatProvider formatProvider, string message, Decimal argument)
  {
    if (!this.IsInfoEnabled)
      return;
    this.WriteToTargets(LogLevel.Info, formatProvider, message, new object[1]
    {
      (object) argument
    });
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  public void Info(string message, Decimal argument)
  {
    if (!this.IsInfoEnabled)
      return;
    this.WriteToTargets(LogLevel.Info, message, new object[1]
    {
      (object) argument
    });
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  public void Info(IFormatProvider formatProvider, string message, object argument)
  {
    if (!this.IsInfoEnabled)
      return;
    this.WriteToTargets(LogLevel.Info, formatProvider, message, new object[1]
    {
      argument
    });
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  public void Info(string message, object argument)
  {
    if (!this.IsInfoEnabled)
      return;
    this.WriteToTargets(LogLevel.Info, message, new object[1]
    {
      argument
    });
  }

  [CLSCompliant(false)]
  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  public void Info(IFormatProvider formatProvider, string message, sbyte argument)
  {
    if (!this.IsInfoEnabled)
      return;
    this.WriteToTargets(LogLevel.Info, formatProvider, message, new object[1]
    {
      (object) argument
    });
  }

  [CLSCompliant(false)]
  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  public void Info(string message, sbyte argument)
  {
    if (!this.IsInfoEnabled)
      return;
    this.WriteToTargets(LogLevel.Info, message, new object[1]
    {
      (object) argument
    });
  }

  [CLSCompliant(false)]
  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  public void Info(IFormatProvider formatProvider, string message, uint argument)
  {
    if (!this.IsInfoEnabled)
      return;
    this.WriteToTargets(LogLevel.Info, formatProvider, message, new object[1]
    {
      (object) argument
    });
  }

  [CLSCompliant(false)]
  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  public void Info(string message, uint argument)
  {
    if (!this.IsInfoEnabled)
      return;
    this.WriteToTargets(LogLevel.Info, message, new object[1]
    {
      (object) argument
    });
  }

  [CLSCompliant(false)]
  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  public void Info(IFormatProvider formatProvider, string message, ulong argument)
  {
    if (!this.IsInfoEnabled)
      return;
    this.WriteToTargets(LogLevel.Info, formatProvider, message, new object[1]
    {
      (object) argument
    });
  }

  [CLSCompliant(false)]
  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  public void Info(string message, ulong argument)
  {
    if (!this.IsInfoEnabled)
      return;
    this.WriteToTargets(LogLevel.Info, message, new object[1]
    {
      (object) argument
    });
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  public void Warn(object value)
  {
    if (!this.IsWarnEnabled)
      return;
    this.WriteToTargets<object>(LogLevel.Warn, (IFormatProvider) null, value);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  public void Warn(IFormatProvider formatProvider, object value)
  {
    if (!this.IsWarnEnabled)
      return;
    this.WriteToTargets<object>(LogLevel.Warn, formatProvider, value);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  public void Warn(string message, object arg1, object arg2)
  {
    if (!this.IsWarnEnabled)
      return;
    this.WriteToTargets(LogLevel.Warn, message, new object[2]
    {
      arg1,
      arg2
    });
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  public void Warn(string message, object arg1, object arg2, object arg3)
  {
    if (!this.IsWarnEnabled)
      return;
    this.WriteToTargets(LogLevel.Warn, message, new object[3]
    {
      arg1,
      arg2,
      arg3
    });
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  public void Warn(IFormatProvider formatProvider, string message, bool argument)
  {
    if (!this.IsWarnEnabled)
      return;
    this.WriteToTargets(LogLevel.Warn, formatProvider, message, new object[1]
    {
      (object) argument
    });
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  public void Warn(string message, bool argument)
  {
    if (!this.IsWarnEnabled)
      return;
    this.WriteToTargets(LogLevel.Warn, message, new object[1]
    {
      (object) argument
    });
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  public void Warn(IFormatProvider formatProvider, string message, char argument)
  {
    if (!this.IsWarnEnabled)
      return;
    this.WriteToTargets(LogLevel.Warn, formatProvider, message, new object[1]
    {
      (object) argument
    });
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  public void Warn(string message, char argument)
  {
    if (!this.IsWarnEnabled)
      return;
    this.WriteToTargets(LogLevel.Warn, message, new object[1]
    {
      (object) argument
    });
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  public void Warn(IFormatProvider formatProvider, string message, byte argument)
  {
    if (!this.IsWarnEnabled)
      return;
    this.WriteToTargets(LogLevel.Warn, formatProvider, message, new object[1]
    {
      (object) argument
    });
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  public void Warn(string message, byte argument)
  {
    if (!this.IsWarnEnabled)
      return;
    this.WriteToTargets(LogLevel.Warn, message, new object[1]
    {
      (object) argument
    });
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  public void Warn(IFormatProvider formatProvider, string message, string argument)
  {
    if (!this.IsWarnEnabled)
      return;
    this.WriteToTargets(LogLevel.Warn, formatProvider, message, new object[1]
    {
      (object) argument
    });
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  public void Warn(string message, string argument)
  {
    if (!this.IsWarnEnabled)
      return;
    this.WriteToTargets(LogLevel.Warn, message, new object[1]
    {
      (object) argument
    });
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  public void Warn(IFormatProvider formatProvider, string message, int argument)
  {
    if (!this.IsWarnEnabled)
      return;
    this.WriteToTargets(LogLevel.Warn, formatProvider, message, new object[1]
    {
      (object) argument
    });
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  public void Warn(string message, int argument)
  {
    if (!this.IsWarnEnabled)
      return;
    this.WriteToTargets(LogLevel.Warn, message, new object[1]
    {
      (object) argument
    });
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  public void Warn(IFormatProvider formatProvider, string message, long argument)
  {
    if (!this.IsWarnEnabled)
      return;
    this.WriteToTargets(LogLevel.Warn, formatProvider, message, new object[1]
    {
      (object) argument
    });
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  public void Warn(string message, long argument)
  {
    if (!this.IsWarnEnabled)
      return;
    this.WriteToTargets(LogLevel.Warn, message, new object[1]
    {
      (object) argument
    });
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  public void Warn(IFormatProvider formatProvider, string message, float argument)
  {
    if (!this.IsWarnEnabled)
      return;
    this.WriteToTargets(LogLevel.Warn, formatProvider, message, new object[1]
    {
      (object) argument
    });
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  public void Warn(string message, float argument)
  {
    if (!this.IsWarnEnabled)
      return;
    this.WriteToTargets(LogLevel.Warn, message, new object[1]
    {
      (object) argument
    });
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  public void Warn(IFormatProvider formatProvider, string message, double argument)
  {
    if (!this.IsWarnEnabled)
      return;
    this.WriteToTargets(LogLevel.Warn, formatProvider, message, new object[1]
    {
      (object) argument
    });
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  public void Warn(string message, double argument)
  {
    if (!this.IsWarnEnabled)
      return;
    this.WriteToTargets(LogLevel.Warn, message, new object[1]
    {
      (object) argument
    });
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  public void Warn(IFormatProvider formatProvider, string message, Decimal argument)
  {
    if (!this.IsWarnEnabled)
      return;
    this.WriteToTargets(LogLevel.Warn, formatProvider, message, new object[1]
    {
      (object) argument
    });
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  public void Warn(string message, Decimal argument)
  {
    if (!this.IsWarnEnabled)
      return;
    this.WriteToTargets(LogLevel.Warn, message, new object[1]
    {
      (object) argument
    });
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  public void Warn(IFormatProvider formatProvider, string message, object argument)
  {
    if (!this.IsWarnEnabled)
      return;
    this.WriteToTargets(LogLevel.Warn, formatProvider, message, new object[1]
    {
      argument
    });
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  public void Warn(string message, object argument)
  {
    if (!this.IsWarnEnabled)
      return;
    this.WriteToTargets(LogLevel.Warn, message, new object[1]
    {
      argument
    });
  }

  [CLSCompliant(false)]
  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  public void Warn(IFormatProvider formatProvider, string message, sbyte argument)
  {
    if (!this.IsWarnEnabled)
      return;
    this.WriteToTargets(LogLevel.Warn, formatProvider, message, new object[1]
    {
      (object) argument
    });
  }

  [CLSCompliant(false)]
  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  public void Warn(string message, sbyte argument)
  {
    if (!this.IsWarnEnabled)
      return;
    this.WriteToTargets(LogLevel.Warn, message, new object[1]
    {
      (object) argument
    });
  }

  [CLSCompliant(false)]
  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  public void Warn(IFormatProvider formatProvider, string message, uint argument)
  {
    if (!this.IsWarnEnabled)
      return;
    this.WriteToTargets(LogLevel.Warn, formatProvider, message, new object[1]
    {
      (object) argument
    });
  }

  [CLSCompliant(false)]
  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  public void Warn(string message, uint argument)
  {
    if (!this.IsWarnEnabled)
      return;
    this.WriteToTargets(LogLevel.Warn, message, new object[1]
    {
      (object) argument
    });
  }

  [CLSCompliant(false)]
  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  public void Warn(IFormatProvider formatProvider, string message, ulong argument)
  {
    if (!this.IsWarnEnabled)
      return;
    this.WriteToTargets(LogLevel.Warn, formatProvider, message, new object[1]
    {
      (object) argument
    });
  }

  [CLSCompliant(false)]
  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  public void Warn(string message, ulong argument)
  {
    if (!this.IsWarnEnabled)
      return;
    this.WriteToTargets(LogLevel.Warn, message, new object[1]
    {
      (object) argument
    });
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  public void Error(object value)
  {
    if (!this.IsErrorEnabled)
      return;
    this.WriteToTargets<object>(LogLevel.Error, (IFormatProvider) null, value);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  public void Error(IFormatProvider formatProvider, object value)
  {
    if (!this.IsErrorEnabled)
      return;
    this.WriteToTargets<object>(LogLevel.Error, formatProvider, value);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  public void Error(string message, object arg1, object arg2)
  {
    if (!this.IsErrorEnabled)
      return;
    this.WriteToTargets(LogLevel.Error, message, new object[2]
    {
      arg1,
      arg2
    });
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  public void Error(string message, object arg1, object arg2, object arg3)
  {
    if (!this.IsErrorEnabled)
      return;
    this.WriteToTargets(LogLevel.Error, message, new object[3]
    {
      arg1,
      arg2,
      arg3
    });
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  public void Error(IFormatProvider formatProvider, string message, bool argument)
  {
    if (!this.IsErrorEnabled)
      return;
    this.WriteToTargets(LogLevel.Error, formatProvider, message, new object[1]
    {
      (object) argument
    });
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  public void Error(string message, bool argument)
  {
    if (!this.IsErrorEnabled)
      return;
    this.WriteToTargets(LogLevel.Error, message, new object[1]
    {
      (object) argument
    });
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  public void Error(IFormatProvider formatProvider, string message, char argument)
  {
    if (!this.IsErrorEnabled)
      return;
    this.WriteToTargets(LogLevel.Error, formatProvider, message, new object[1]
    {
      (object) argument
    });
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  public void Error(string message, char argument)
  {
    if (!this.IsErrorEnabled)
      return;
    this.WriteToTargets(LogLevel.Error, message, new object[1]
    {
      (object) argument
    });
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  public void Error(IFormatProvider formatProvider, string message, byte argument)
  {
    if (!this.IsErrorEnabled)
      return;
    this.WriteToTargets(LogLevel.Error, formatProvider, message, new object[1]
    {
      (object) argument
    });
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  public void Error(string message, byte argument)
  {
    if (!this.IsErrorEnabled)
      return;
    this.WriteToTargets(LogLevel.Error, message, new object[1]
    {
      (object) argument
    });
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  public void Error(IFormatProvider formatProvider, string message, string argument)
  {
    if (!this.IsErrorEnabled)
      return;
    this.WriteToTargets(LogLevel.Error, formatProvider, message, new object[1]
    {
      (object) argument
    });
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  public void Error(string message, string argument)
  {
    if (!this.IsErrorEnabled)
      return;
    this.WriteToTargets(LogLevel.Error, message, new object[1]
    {
      (object) argument
    });
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  public void Error(IFormatProvider formatProvider, string message, int argument)
  {
    if (!this.IsErrorEnabled)
      return;
    this.WriteToTargets(LogLevel.Error, formatProvider, message, new object[1]
    {
      (object) argument
    });
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  public void Error(string message, int argument)
  {
    if (!this.IsErrorEnabled)
      return;
    this.WriteToTargets(LogLevel.Error, message, new object[1]
    {
      (object) argument
    });
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  public void Error(IFormatProvider formatProvider, string message, long argument)
  {
    if (!this.IsErrorEnabled)
      return;
    this.WriteToTargets(LogLevel.Error, formatProvider, message, new object[1]
    {
      (object) argument
    });
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  public void Error(string message, long argument)
  {
    if (!this.IsErrorEnabled)
      return;
    this.WriteToTargets(LogLevel.Error, message, new object[1]
    {
      (object) argument
    });
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  public void Error(IFormatProvider formatProvider, string message, float argument)
  {
    if (!this.IsErrorEnabled)
      return;
    this.WriteToTargets(LogLevel.Error, formatProvider, message, new object[1]
    {
      (object) argument
    });
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  public void Error(string message, float argument)
  {
    if (!this.IsErrorEnabled)
      return;
    this.WriteToTargets(LogLevel.Error, message, new object[1]
    {
      (object) argument
    });
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  public void Error(IFormatProvider formatProvider, string message, double argument)
  {
    if (!this.IsErrorEnabled)
      return;
    this.WriteToTargets(LogLevel.Error, formatProvider, message, new object[1]
    {
      (object) argument
    });
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  public void Error(string message, double argument)
  {
    if (!this.IsErrorEnabled)
      return;
    this.WriteToTargets(LogLevel.Error, message, new object[1]
    {
      (object) argument
    });
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  public void Error(IFormatProvider formatProvider, string message, Decimal argument)
  {
    if (!this.IsErrorEnabled)
      return;
    this.WriteToTargets(LogLevel.Error, formatProvider, message, new object[1]
    {
      (object) argument
    });
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  public void Error(string message, Decimal argument)
  {
    if (!this.IsErrorEnabled)
      return;
    this.WriteToTargets(LogLevel.Error, message, new object[1]
    {
      (object) argument
    });
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  public void Error(IFormatProvider formatProvider, string message, object argument)
  {
    if (!this.IsErrorEnabled)
      return;
    this.WriteToTargets(LogLevel.Error, formatProvider, message, new object[1]
    {
      argument
    });
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  public void Error(string message, object argument)
  {
    if (!this.IsErrorEnabled)
      return;
    this.WriteToTargets(LogLevel.Error, message, new object[1]
    {
      argument
    });
  }

  [CLSCompliant(false)]
  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  public void Error(IFormatProvider formatProvider, string message, sbyte argument)
  {
    if (!this.IsErrorEnabled)
      return;
    this.WriteToTargets(LogLevel.Error, formatProvider, message, new object[1]
    {
      (object) argument
    });
  }

  [CLSCompliant(false)]
  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  public void Error(string message, sbyte argument)
  {
    if (!this.IsErrorEnabled)
      return;
    this.WriteToTargets(LogLevel.Error, message, new object[1]
    {
      (object) argument
    });
  }

  [CLSCompliant(false)]
  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  public void Error(IFormatProvider formatProvider, string message, uint argument)
  {
    if (!this.IsErrorEnabled)
      return;
    this.WriteToTargets(LogLevel.Error, formatProvider, message, new object[1]
    {
      (object) argument
    });
  }

  [CLSCompliant(false)]
  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  public void Error(string message, uint argument)
  {
    if (!this.IsErrorEnabled)
      return;
    this.WriteToTargets(LogLevel.Error, message, new object[1]
    {
      (object) argument
    });
  }

  [CLSCompliant(false)]
  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  public void Error(IFormatProvider formatProvider, string message, ulong argument)
  {
    if (!this.IsErrorEnabled)
      return;
    this.WriteToTargets(LogLevel.Error, formatProvider, message, new object[1]
    {
      (object) argument
    });
  }

  [CLSCompliant(false)]
  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  public void Error(string message, ulong argument)
  {
    if (!this.IsErrorEnabled)
      return;
    this.WriteToTargets(LogLevel.Error, message, new object[1]
    {
      (object) argument
    });
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  public void Fatal(object value)
  {
    if (!this.IsFatalEnabled)
      return;
    this.WriteToTargets<object>(LogLevel.Fatal, (IFormatProvider) null, value);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  public void Fatal(IFormatProvider formatProvider, object value)
  {
    if (!this.IsFatalEnabled)
      return;
    this.WriteToTargets<object>(LogLevel.Fatal, formatProvider, value);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  public void Fatal(string message, object arg1, object arg2)
  {
    if (!this.IsFatalEnabled)
      return;
    this.WriteToTargets(LogLevel.Fatal, message, new object[2]
    {
      arg1,
      arg2
    });
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  public void Fatal(string message, object arg1, object arg2, object arg3)
  {
    if (!this.IsFatalEnabled)
      return;
    this.WriteToTargets(LogLevel.Fatal, message, new object[3]
    {
      arg1,
      arg2,
      arg3
    });
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  public void Fatal(IFormatProvider formatProvider, string message, bool argument)
  {
    if (!this.IsFatalEnabled)
      return;
    this.WriteToTargets(LogLevel.Fatal, formatProvider, message, new object[1]
    {
      (object) argument
    });
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  public void Fatal(string message, bool argument)
  {
    if (!this.IsFatalEnabled)
      return;
    this.WriteToTargets(LogLevel.Fatal, message, new object[1]
    {
      (object) argument
    });
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  public void Fatal(IFormatProvider formatProvider, string message, char argument)
  {
    if (!this.IsFatalEnabled)
      return;
    this.WriteToTargets(LogLevel.Fatal, formatProvider, message, new object[1]
    {
      (object) argument
    });
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  public void Fatal(string message, char argument)
  {
    if (!this.IsFatalEnabled)
      return;
    this.WriteToTargets(LogLevel.Fatal, message, new object[1]
    {
      (object) argument
    });
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  public void Fatal(IFormatProvider formatProvider, string message, byte argument)
  {
    if (!this.IsFatalEnabled)
      return;
    this.WriteToTargets(LogLevel.Fatal, formatProvider, message, new object[1]
    {
      (object) argument
    });
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  public void Fatal(string message, byte argument)
  {
    if (!this.IsFatalEnabled)
      return;
    this.WriteToTargets(LogLevel.Fatal, message, new object[1]
    {
      (object) argument
    });
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  public void Fatal(IFormatProvider formatProvider, string message, string argument)
  {
    if (!this.IsFatalEnabled)
      return;
    this.WriteToTargets(LogLevel.Fatal, formatProvider, message, new object[1]
    {
      (object) argument
    });
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  public void Fatal(string message, string argument)
  {
    if (!this.IsFatalEnabled)
      return;
    this.WriteToTargets(LogLevel.Fatal, message, new object[1]
    {
      (object) argument
    });
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  public void Fatal(IFormatProvider formatProvider, string message, int argument)
  {
    if (!this.IsFatalEnabled)
      return;
    this.WriteToTargets(LogLevel.Fatal, formatProvider, message, new object[1]
    {
      (object) argument
    });
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  public void Fatal(string message, int argument)
  {
    if (!this.IsFatalEnabled)
      return;
    this.WriteToTargets(LogLevel.Fatal, message, new object[1]
    {
      (object) argument
    });
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  public void Fatal(IFormatProvider formatProvider, string message, long argument)
  {
    if (!this.IsFatalEnabled)
      return;
    this.WriteToTargets(LogLevel.Fatal, formatProvider, message, new object[1]
    {
      (object) argument
    });
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  public void Fatal(string message, long argument)
  {
    if (!this.IsFatalEnabled)
      return;
    this.WriteToTargets(LogLevel.Fatal, message, new object[1]
    {
      (object) argument
    });
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  public void Fatal(IFormatProvider formatProvider, string message, float argument)
  {
    if (!this.IsFatalEnabled)
      return;
    this.WriteToTargets(LogLevel.Fatal, formatProvider, message, new object[1]
    {
      (object) argument
    });
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  public void Fatal(string message, float argument)
  {
    if (!this.IsFatalEnabled)
      return;
    this.WriteToTargets(LogLevel.Fatal, message, new object[1]
    {
      (object) argument
    });
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  public void Fatal(IFormatProvider formatProvider, string message, double argument)
  {
    if (!this.IsFatalEnabled)
      return;
    this.WriteToTargets(LogLevel.Fatal, formatProvider, message, new object[1]
    {
      (object) argument
    });
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  public void Fatal(string message, double argument)
  {
    if (!this.IsFatalEnabled)
      return;
    this.WriteToTargets(LogLevel.Fatal, message, new object[1]
    {
      (object) argument
    });
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  public void Fatal(IFormatProvider formatProvider, string message, Decimal argument)
  {
    if (!this.IsFatalEnabled)
      return;
    this.WriteToTargets(LogLevel.Fatal, formatProvider, message, new object[1]
    {
      (object) argument
    });
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  public void Fatal(string message, Decimal argument)
  {
    if (!this.IsFatalEnabled)
      return;
    this.WriteToTargets(LogLevel.Fatal, message, new object[1]
    {
      (object) argument
    });
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  public void Fatal(IFormatProvider formatProvider, string message, object argument)
  {
    if (!this.IsFatalEnabled)
      return;
    this.WriteToTargets(LogLevel.Fatal, formatProvider, message, new object[1]
    {
      argument
    });
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  public void Fatal(string message, object argument)
  {
    if (!this.IsFatalEnabled)
      return;
    this.WriteToTargets(LogLevel.Fatal, message, new object[1]
    {
      argument
    });
  }

  [CLSCompliant(false)]
  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  public void Fatal(IFormatProvider formatProvider, string message, sbyte argument)
  {
    if (!this.IsFatalEnabled)
      return;
    this.WriteToTargets(LogLevel.Fatal, formatProvider, message, new object[1]
    {
      (object) argument
    });
  }

  [CLSCompliant(false)]
  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  public void Fatal(string message, sbyte argument)
  {
    if (!this.IsFatalEnabled)
      return;
    this.WriteToTargets(LogLevel.Fatal, message, new object[1]
    {
      (object) argument
    });
  }

  [CLSCompliant(false)]
  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  public void Fatal(IFormatProvider formatProvider, string message, uint argument)
  {
    if (!this.IsFatalEnabled)
      return;
    this.WriteToTargets(LogLevel.Fatal, formatProvider, message, new object[1]
    {
      (object) argument
    });
  }

  [CLSCompliant(false)]
  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  public void Fatal(string message, uint argument)
  {
    if (!this.IsFatalEnabled)
      return;
    this.WriteToTargets(LogLevel.Fatal, message, new object[1]
    {
      (object) argument
    });
  }

  [CLSCompliant(false)]
  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  public void Fatal(IFormatProvider formatProvider, string message, ulong argument)
  {
    if (!this.IsFatalEnabled)
      return;
    this.WriteToTargets(LogLevel.Fatal, formatProvider, message, new object[1]
    {
      (object) argument
    });
  }

  [CLSCompliant(false)]
  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  public void Fatal(string message, ulong argument)
  {
    if (!this.IsFatalEnabled)
      return;
    this.WriteToTargets(LogLevel.Fatal, message, new object[1]
    {
      (object) argument
    });
  }

  protected internal Logger() => this._contextLogger = this;

  public event EventHandler<EventArgs> LoggerReconfigured;

  public string Name { get; private set; }

  public LogFactory Factory { get; private set; }

  public IDictionary<string, object> Properties
  {
    get
    {
      return (IDictionary<string, object>) this._contextProperties ?? (IDictionary<string, object>) Interlocked.CompareExchange<ThreadSafeDictionary<string, object>>(ref this._contextProperties, Logger.CreateContextPropertiesDictionary((ThreadSafeDictionary<string, object>) null), (ThreadSafeDictionary<string, object>) null) ?? (IDictionary<string, object>) this._contextProperties;
    }
  }

  public bool IsEnabled(LogLevel level)
  {
    if (level == (LogLevel) null)
      throw new InvalidOperationException("Log level must be defined");
    return this.GetTargetsForLevel(level) != null;
  }

  public Logger WithProperty(string propertyKey, object propertyValue)
  {
    if (string.IsNullOrEmpty(propertyKey))
      throw new ArgumentException(nameof (propertyKey));
    Logger logger = this.Factory.CreateNewLogger(this.GetType()) ?? new Logger();
    logger.Initialize(this.Name, this._configuration, this.Factory);
    logger._contextProperties = Logger.CreateContextPropertiesDictionary(this._contextProperties);
    logger._contextProperties[propertyKey] = propertyValue;
    logger._contextLogger = this._contextLogger;
    return logger;
  }

  public void SetProperty(string propertyKey, object propertyValue)
  {
    if (string.IsNullOrEmpty(propertyKey))
      throw new ArgumentException(nameof (propertyKey));
    this.Properties[propertyKey] = propertyValue;
  }

  private static ThreadSafeDictionary<string, object> CreateContextPropertiesDictionary(
    ThreadSafeDictionary<string, object> contextProperties)
  {
    contextProperties = contextProperties != null ? new ThreadSafeDictionary<string, object>(contextProperties) : new ThreadSafeDictionary<string, object>();
    return contextProperties;
  }

  public void Log(LogEventInfo logEvent)
  {
    TargetWithFilterChain targetsForLevel = this.IsEnabled(logEvent.Level) ? this.GetTargetsForLevel(logEvent.Level) : (TargetWithFilterChain) null;
    if (targetsForLevel == null)
      return;
    if (logEvent.LoggerName == null)
      logEvent.LoggerName = this.Name;
    this.WriteToTargets(logEvent, targetsForLevel);
  }

  public void Log(Type wrapperType, LogEventInfo logEvent)
  {
    TargetWithFilterChain targetsForLevel = this.IsEnabled(logEvent.Level) ? this.GetTargetsForLevel(logEvent.Level) : (TargetWithFilterChain) null;
    if (targetsForLevel == null)
      return;
    if (logEvent.LoggerName == null)
      logEvent.LoggerName = this.Name;
    this.WriteToTargets(wrapperType, logEvent, targetsForLevel);
  }

  public void Log<T>(LogLevel level, T value)
  {
    if (!this.IsEnabled(level))
      return;
    this.WriteToTargets<T>(level, (IFormatProvider) null, value);
  }

  public void Log<T>(LogLevel level, IFormatProvider formatProvider, T value)
  {
    if (!this.IsEnabled(level))
      return;
    this.WriteToTargets<T>(level, formatProvider, value);
  }

  public void Log(LogLevel level, LogMessageGenerator messageFunc)
  {
    if (!this.IsEnabled(level))
      return;
    if (messageFunc == null)
      throw new ArgumentNullException(nameof (messageFunc));
    this.WriteToTargets(level, (IFormatProvider) null, messageFunc());
  }

  [Obsolete("Use Log(LogLevel, String, Exception) method instead. Marked obsolete before v4.3.11")]
  public void LogException(LogLevel level, [Localizable(false)] string message, Exception exception)
  {
    this.Log(level, message, exception);
  }

  [MessageTemplateFormatMethod("message")]
  public void Log(
    LogLevel level,
    IFormatProvider formatProvider,
    [Localizable(false)] string message,
    params object[] args)
  {
    if (!this.IsEnabled(level))
      return;
    this.WriteToTargets(level, formatProvider, message, args);
  }

  public void Log(LogLevel level, [Localizable(false)] string message)
  {
    if (!this.IsEnabled(level))
      return;
    this.WriteToTargets(level, (IFormatProvider) null, message);
  }

  [MessageTemplateFormatMethod("message")]
  public void Log(LogLevel level, [Localizable(false)] string message, params object[] args)
  {
    if (!this.IsEnabled(level))
      return;
    this.WriteToTargets(level, message, args);
  }

  [Obsolete("Use Log(LogLevel level, Exception exception, [Localizable(false)] string message, params object[] args) instead. Marked obsolete before v4.3.11")]
  public void Log(LogLevel level, [Localizable(false)] string message, Exception exception)
  {
    if (!this.IsEnabled(level))
      return;
    this.WriteToTargets(level, exception, message, (object[]) null);
  }

  [MessageTemplateFormatMethod("message")]
  public void Log(LogLevel level, Exception exception, [Localizable(false)] string message, params object[] args)
  {
    if (!this.IsEnabled(level))
      return;
    this.WriteToTargets(level, exception, message, args);
  }

  [MessageTemplateFormatMethod("message")]
  public void Log(
    LogLevel level,
    Exception exception,
    IFormatProvider formatProvider,
    [Localizable(false)] string message,
    params object[] args)
  {
    if (!this.IsEnabled(level))
      return;
    this.WriteToTargets(level, exception, formatProvider, message, args);
  }

  [MessageTemplateFormatMethod("message")]
  public void Log<TArgument>(
    LogLevel level,
    IFormatProvider formatProvider,
    [Localizable(false)] string message,
    TArgument argument)
  {
    if (!this.IsEnabled(level))
      return;
    this.WriteToTargets(level, formatProvider, message, new object[1]
    {
      (object) argument
    });
  }

  [MessageTemplateFormatMethod("message")]
  public void Log<TArgument>(LogLevel level, [Localizable(false)] string message, TArgument argument)
  {
    if (!this.IsEnabled(level))
      return;
    this.WriteToTargets(level, message, new object[1]
    {
      (object) argument
    });
  }

  [MessageTemplateFormatMethod("message")]
  public void Log<TArgument1, TArgument2>(
    LogLevel level,
    IFormatProvider formatProvider,
    [Localizable(false)] string message,
    TArgument1 argument1,
    TArgument2 argument2)
  {
    if (!this.IsEnabled(level))
      return;
    this.WriteToTargets(level, formatProvider, message, new object[2]
    {
      (object) argument1,
      (object) argument2
    });
  }

  [MessageTemplateFormatMethod("message")]
  public void Log<TArgument1, TArgument2>(
    LogLevel level,
    [Localizable(false)] string message,
    TArgument1 argument1,
    TArgument2 argument2)
  {
    if (!this.IsEnabled(level))
      return;
    this.WriteToTargets(level, message, new object[2]
    {
      (object) argument1,
      (object) argument2
    });
  }

  [MessageTemplateFormatMethod("message")]
  public void Log<TArgument1, TArgument2, TArgument3>(
    LogLevel level,
    IFormatProvider formatProvider,
    [Localizable(false)] string message,
    TArgument1 argument1,
    TArgument2 argument2,
    TArgument3 argument3)
  {
    if (!this.IsEnabled(level))
      return;
    this.WriteToTargets(level, formatProvider, message, new object[3]
    {
      (object) argument1,
      (object) argument2,
      (object) argument3
    });
  }

  [MessageTemplateFormatMethod("message")]
  public void Log<TArgument1, TArgument2, TArgument3>(
    LogLevel level,
    [Localizable(false)] string message,
    TArgument1 argument1,
    TArgument2 argument2,
    TArgument3 argument3)
  {
    if (!this.IsEnabled(level))
      return;
    this.WriteToTargets(level, message, new object[3]
    {
      (object) argument1,
      (object) argument2,
      (object) argument3
    });
  }

  private LogEventInfo PrepareLogEventInfo(LogEventInfo logEvent)
  {
    if (logEvent.FormatProvider == null)
      logEvent.FormatProvider = (IFormatProvider) this.Factory.DefaultCultureInfo;
    if (this._contextProperties != null)
    {
      foreach (KeyValuePair<string, object> contextProperty in this._contextProperties)
      {
        if (!logEvent.Properties.ContainsKey((object) contextProperty.Key))
          logEvent.Properties[(object) contextProperty.Key] = contextProperty.Value;
      }
    }
    return logEvent;
  }

  public void Swallow(Action action)
  {
    try
    {
      action();
    }
    catch (Exception ex)
    {
      this.Error<Exception>(ex);
    }
  }

  public T Swallow<T>(Func<T> func) => this.Swallow<T>(func, default (T));

  public T Swallow<T>(Func<T> func, T fallback)
  {
    try
    {
      return func();
    }
    catch (Exception ex)
    {
      this.Error<Exception>(ex);
      return fallback;
    }
  }

  public async void Swallow(Task task)
  {
    try
    {
      await task;
    }
    catch (Exception ex)
    {
      this.Error<Exception>(ex);
    }
  }

  public async Task SwallowAsync(Task task)
  {
    try
    {
      await task;
    }
    catch (Exception ex)
    {
      this.Error<Exception>(ex);
    }
  }

  public async Task SwallowAsync(Func<Task> asyncAction)
  {
    try
    {
      await asyncAction();
    }
    catch (Exception ex)
    {
      this.Error<Exception>(ex);
    }
  }

  public async Task<TResult> SwallowAsync<TResult>(Func<Task<TResult>> asyncFunc)
  {
    return await this.SwallowAsync<TResult>(asyncFunc, default (TResult));
  }

  public async Task<TResult> SwallowAsync<TResult>(Func<Task<TResult>> asyncFunc, TResult fallback)
  {
    try
    {
      return await asyncFunc();
    }
    catch (Exception ex)
    {
      this.Error<Exception>(ex);
      return fallback;
    }
  }

  internal void Initialize(
    string name,
    LoggerConfiguration loggerConfiguration,
    LogFactory factory)
  {
    this.Name = name;
    this.Factory = factory;
    this.SetConfiguration(loggerConfiguration);
  }

  private void WriteToTargets(LogLevel level, [Localizable(false)] string message, object[] args)
  {
    this.WriteToTargets(level, (IFormatProvider) this.Factory.DefaultCultureInfo, message, args);
  }

  private void WriteToTargets(
    LogLevel level,
    IFormatProvider formatProvider,
    [Localizable(false)] string message,
    object[] args)
  {
    TargetWithFilterChain targetsForLevel = this.GetTargetsForLevel(level);
    if (targetsForLevel == null)
      return;
    this.WriteToTargets(LogEventInfo.Create(level, this.Name, formatProvider, message, args), targetsForLevel);
  }

  private void WriteToTargets(LogLevel level, IFormatProvider formatProvider, [Localizable(false)] string message)
  {
    TargetWithFilterChain targetsForLevel = this.GetTargetsForLevel(level);
    if (targetsForLevel == null)
      return;
    this.WriteToTargets(LogEventInfo.Create(level, this.Name, formatProvider, message, (object[]) null), targetsForLevel);
  }

  private void WriteToTargets<T>(LogLevel level, IFormatProvider formatProvider, T value)
  {
    TargetWithFilterChain targetsForLevel = this.GetTargetsForLevel(level);
    if (targetsForLevel == null)
      return;
    this.WriteToTargets(LogEventInfo.Create(level, this.Name, formatProvider, (object) value), targetsForLevel);
  }

  private void WriteToTargets(LogLevel level, Exception ex, [Localizable(false)] string message, object[] args)
  {
    TargetWithFilterChain targetsForLevel = this.GetTargetsForLevel(level);
    if (targetsForLevel == null)
      return;
    this.WriteToTargets(LogEventInfo.Create(level, this.Name, ex, (IFormatProvider) this.Factory.DefaultCultureInfo, message, args), targetsForLevel);
  }

  private void WriteToTargets(
    LogLevel level,
    Exception ex,
    IFormatProvider formatProvider,
    [Localizable(false)] string message,
    object[] args)
  {
    TargetWithFilterChain targetsForLevel = this.GetTargetsForLevel(level);
    if (targetsForLevel == null)
      return;
    this.WriteToTargets(LogEventInfo.Create(level, this.Name, ex, formatProvider, message, args), targetsForLevel);
  }

  private void WriteToTargets([NotNull] LogEventInfo logEvent, [NotNull] TargetWithFilterChain targetsForLevel)
  {
    LoggerImpl.Write(Logger.DefaultLoggerType, targetsForLevel, this.PrepareLogEventInfo(logEvent), this.Factory);
  }

  private void WriteToTargets(
    Type wrapperType,
    [NotNull] LogEventInfo logEvent,
    [NotNull] TargetWithFilterChain targetsForLevel)
  {
    Type loggerType = wrapperType;
    if ((object) loggerType == null)
      loggerType = Logger.DefaultLoggerType;
    LoggerImpl.Write(loggerType, targetsForLevel, this.PrepareLogEventInfo(logEvent), this.Factory);
  }

  internal void SetConfiguration(LoggerConfiguration newConfiguration)
  {
    this._configuration = newConfiguration;
    this._isTraceEnabled = this.IsEnabled(LogLevel.Trace);
    this._isDebugEnabled = this.IsEnabled(LogLevel.Debug);
    this._isInfoEnabled = this.IsEnabled(LogLevel.Info);
    this._isWarnEnabled = this.IsEnabled(LogLevel.Warn);
    this._isErrorEnabled = this.IsEnabled(LogLevel.Error);
    this._isFatalEnabled = this.IsEnabled(LogLevel.Fatal);
    this.OnLoggerReconfigured(EventArgs.Empty);
  }

  private TargetWithFilterChain GetTargetsForLevel(LogLevel level)
  {
    return this._contextLogger == this ? this._configuration.GetTargetsForLevel(level) : this._contextLogger.GetTargetsForLevel(level);
  }

  protected virtual void OnLoggerReconfigured(EventArgs e)
  {
    EventHandler<EventArgs> loggerReconfigured = this.LoggerReconfigured;
    if (loggerReconfigured == null)
      return;
    loggerReconfigured((object) this, e);
  }
}
