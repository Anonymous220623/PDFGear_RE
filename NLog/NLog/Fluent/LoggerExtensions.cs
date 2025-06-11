// Decompiled with JetBrains decompiler
// Type: NLog.Fluent.LoggerExtensions
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using System;

#nullable disable
namespace NLog.Fluent;

public static class LoggerExtensions
{
  [CLSCompliant(false)]
  public static LogBuilder Log(this ILogger logger, NLog.LogLevel logLevel)
  {
    return new LogBuilder(logger, logLevel);
  }

  [CLSCompliant(false)]
  public static LogBuilder Trace(this ILogger logger) => new LogBuilder(logger, NLog.LogLevel.Trace);

  [CLSCompliant(false)]
  public static LogBuilder Debug(this ILogger logger) => new LogBuilder(logger, NLog.LogLevel.Debug);

  [CLSCompliant(false)]
  public static LogBuilder Info(this ILogger logger) => new LogBuilder(logger, NLog.LogLevel.Info);

  [CLSCompliant(false)]
  public static LogBuilder Warn(this ILogger logger) => new LogBuilder(logger, NLog.LogLevel.Warn);

  [CLSCompliant(false)]
  public static LogBuilder Error(this ILogger logger) => new LogBuilder(logger, NLog.LogLevel.Error);

  [CLSCompliant(false)]
  public static LogBuilder Fatal(this ILogger logger) => new LogBuilder(logger, NLog.LogLevel.Fatal);
}
