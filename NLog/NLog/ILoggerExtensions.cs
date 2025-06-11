// Decompiled with JetBrains decompiler
// Type: NLog.ILoggerExtensions
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using System;

#nullable disable
namespace NLog;

public static class ILoggerExtensions
{
  [CLSCompliant(false)]
  public static void Log(
    this ILogger logger,
    LogLevel level,
    Exception exception,
    LogMessageGenerator messageFunc)
  {
    if (!logger.IsEnabled(level))
      return;
    logger.Log(level, exception, messageFunc(), (object[]) null);
  }

  [CLSCompliant(false)]
  public static void Trace(
    this ILogger logger,
    Exception exception,
    LogMessageGenerator messageFunc)
  {
    if (!logger.IsTraceEnabled)
      return;
    if (messageFunc == null)
      throw new ArgumentNullException(nameof (messageFunc));
    logger.Trace(exception, messageFunc());
  }

  [CLSCompliant(false)]
  public static void Debug(
    this ILogger logger,
    Exception exception,
    LogMessageGenerator messageFunc)
  {
    if (!logger.IsDebugEnabled)
      return;
    if (messageFunc == null)
      throw new ArgumentNullException(nameof (messageFunc));
    logger.Debug(exception, messageFunc());
  }

  [CLSCompliant(false)]
  public static void Info(
    this ILogger logger,
    Exception exception,
    LogMessageGenerator messageFunc)
  {
    if (!logger.IsInfoEnabled)
      return;
    if (messageFunc == null)
      throw new ArgumentNullException(nameof (messageFunc));
    logger.Info(exception, messageFunc());
  }

  [CLSCompliant(false)]
  public static void Warn(
    this ILogger logger,
    Exception exception,
    LogMessageGenerator messageFunc)
  {
    if (!logger.IsWarnEnabled)
      return;
    if (messageFunc == null)
      throw new ArgumentNullException(nameof (messageFunc));
    logger.Warn(exception, messageFunc());
  }

  [CLSCompliant(false)]
  public static void Error(
    this ILogger logger,
    Exception exception,
    LogMessageGenerator messageFunc)
  {
    if (!logger.IsErrorEnabled)
      return;
    if (messageFunc == null)
      throw new ArgumentNullException(nameof (messageFunc));
    logger.Error(exception, messageFunc());
  }

  [CLSCompliant(false)]
  public static void Fatal(
    this ILogger logger,
    Exception exception,
    LogMessageGenerator messageFunc)
  {
    if (!logger.IsFatalEnabled)
      return;
    if (messageFunc == null)
      throw new ArgumentNullException(nameof (messageFunc));
    logger.Fatal(exception, messageFunc());
  }
}
