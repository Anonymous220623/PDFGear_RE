// Decompiled with JetBrains decompiler
// Type: NLog.ILoggerBase
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using System;
using System.ComponentModel;

#nullable disable
namespace NLog;

[CLSCompliant(false)]
public interface ILoggerBase
{
  [EditorBrowsable(EditorBrowsableState.Never)]
  void Log(LogLevel level, object value);

  [EditorBrowsable(EditorBrowsableState.Never)]
  void Log(LogLevel level, IFormatProvider formatProvider, object value);

  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  void Log(LogLevel level, string message, object arg1, object arg2);

  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  void Log(LogLevel level, string message, object arg1, object arg2, object arg3);

  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  void Log(LogLevel level, IFormatProvider formatProvider, string message, bool argument);

  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  void Log(LogLevel level, string message, bool argument);

  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  void Log(LogLevel level, IFormatProvider formatProvider, string message, char argument);

  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  void Log(LogLevel level, string message, char argument);

  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  void Log(LogLevel level, IFormatProvider formatProvider, string message, byte argument);

  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  void Log(LogLevel level, string message, byte argument);

  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  void Log(LogLevel level, IFormatProvider formatProvider, string message, string argument);

  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  void Log(LogLevel level, string message, string argument);

  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  void Log(LogLevel level, IFormatProvider formatProvider, string message, int argument);

  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  void Log(LogLevel level, string message, int argument);

  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  void Log(LogLevel level, IFormatProvider formatProvider, string message, long argument);

  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  void Log(LogLevel level, string message, long argument);

  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  void Log(LogLevel level, IFormatProvider formatProvider, string message, float argument);

  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  void Log(LogLevel level, string message, float argument);

  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  void Log(LogLevel level, IFormatProvider formatProvider, string message, double argument);

  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  void Log(LogLevel level, string message, double argument);

  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  void Log(LogLevel level, IFormatProvider formatProvider, string message, Decimal argument);

  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  void Log(LogLevel level, string message, Decimal argument);

  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  void Log(LogLevel level, IFormatProvider formatProvider, string message, object argument);

  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  void Log(LogLevel level, string message, object argument);

  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  void Log(LogLevel level, IFormatProvider formatProvider, string message, sbyte argument);

  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  void Log(LogLevel level, string message, sbyte argument);

  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  void Log(LogLevel level, IFormatProvider formatProvider, string message, uint argument);

  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  void Log(LogLevel level, string message, uint argument);

  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  void Log(LogLevel level, IFormatProvider formatProvider, string message, ulong argument);

  [EditorBrowsable(EditorBrowsableState.Never)]
  [MessageTemplateFormatMethod("message")]
  void Log(LogLevel level, string message, ulong argument);

  event EventHandler<EventArgs> LoggerReconfigured;

  string Name { get; }

  LogFactory Factory { get; }

  bool IsEnabled(LogLevel level);

  void Log(LogEventInfo logEvent);

  void Log(Type wrapperType, LogEventInfo logEvent);

  void Log<T>(LogLevel level, T value);

  void Log<T>(LogLevel level, IFormatProvider formatProvider, T value);

  void Log(LogLevel level, LogMessageGenerator messageFunc);

  [Obsolete("Use Log(LogLevel level, Exception exception, [Localizable(false)] string message, params object[] args) instead. Marked obsolete before v4.3.11")]
  void LogException(LogLevel level, [Localizable(false)] string message, Exception exception);

  [MessageTemplateFormatMethod("message")]
  void Log(LogLevel level, Exception exception, [Localizable(false)] string message, params object[] args);

  [MessageTemplateFormatMethod("message")]
  void Log(
    LogLevel level,
    Exception exception,
    IFormatProvider formatProvider,
    [Localizable(false)] string message,
    params object[] args);

  [MessageTemplateFormatMethod("message")]
  void Log(LogLevel level, IFormatProvider formatProvider, [Localizable(false)] string message, params object[] args);

  void Log(LogLevel level, [Localizable(false)] string message);

  [MessageTemplateFormatMethod("message")]
  void Log(LogLevel level, [Localizable(false)] string message, params object[] args);

  [Obsolete("Use Log(LogLevel level, Exception exception, [Localizable(false)] string message, params object[] args) instead. Marked obsolete before v4.3.11")]
  void Log(LogLevel level, [Localizable(false)] string message, Exception exception);

  [MessageTemplateFormatMethod("message")]
  void Log<TArgument>(
    LogLevel level,
    IFormatProvider formatProvider,
    [Localizable(false)] string message,
    TArgument argument);

  [MessageTemplateFormatMethod("message")]
  void Log<TArgument>(LogLevel level, [Localizable(false)] string message, TArgument argument);

  [MessageTemplateFormatMethod("message")]
  void Log<TArgument1, TArgument2>(
    LogLevel level,
    IFormatProvider formatProvider,
    [Localizable(false)] string message,
    TArgument1 argument1,
    TArgument2 argument2);

  [MessageTemplateFormatMethod("message")]
  void Log<TArgument1, TArgument2>(
    LogLevel level,
    [Localizable(false)] string message,
    TArgument1 argument1,
    TArgument2 argument2);

  [MessageTemplateFormatMethod("message")]
  void Log<TArgument1, TArgument2, TArgument3>(
    LogLevel level,
    IFormatProvider formatProvider,
    [Localizable(false)] string message,
    TArgument1 argument1,
    TArgument2 argument2,
    TArgument3 argument3);

  [MessageTemplateFormatMethod("message")]
  void Log<TArgument1, TArgument2, TArgument3>(
    LogLevel level,
    [Localizable(false)] string message,
    TArgument1 argument1,
    TArgument2 argument2,
    TArgument3 argument3);
}
