// Decompiled with JetBrains decompiler
// Type: NLog.Fluent.Log
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using NLog.Common;
using System;
using System.IO;
using System.Runtime.CompilerServices;

#nullable disable
namespace NLog.Fluent;

public static class Log
{
  private static readonly ILogger _logger = (ILogger) LogManager.GetCurrentClassLogger();

  public static LogBuilder Level(NLog.LogLevel logLevel, [CallerFilePath] string callerFilePath = null)
  {
    return Log.Create(logLevel, callerFilePath);
  }

  public static LogBuilder Trace([CallerFilePath] string callerFilePath = null)
  {
    return Log.Create(NLog.LogLevel.Trace, callerFilePath);
  }

  public static LogBuilder Debug([CallerFilePath] string callerFilePath = null)
  {
    return Log.Create(NLog.LogLevel.Debug, callerFilePath);
  }

  public static LogBuilder Info([CallerFilePath] string callerFilePath = null)
  {
    return Log.Create(NLog.LogLevel.Info, callerFilePath);
  }

  public static LogBuilder Warn([CallerFilePath] string callerFilePath = null)
  {
    return Log.Create(NLog.LogLevel.Warn, callerFilePath);
  }

  public static LogBuilder Error([CallerFilePath] string callerFilePath = null)
  {
    return Log.Create(NLog.LogLevel.Error, callerFilePath);
  }

  public static LogBuilder Fatal([CallerFilePath] string callerFilePath = null)
  {
    return Log.Create(NLog.LogLevel.Fatal, callerFilePath);
  }

  private static LogBuilder Create(NLog.LogLevel logLevel, string callerFilePath)
  {
    LogBuilder logBuilder = new LogBuilder(Log.GetLogger(callerFilePath), logLevel);
    if (callerFilePath != null)
      logBuilder.Property((object) "CallerFilePath", (object) callerFilePath);
    return logBuilder;
  }

  private static ILogger GetLogger(string callerFilePath)
  {
    try
    {
      string withoutExtension = Path.GetFileNameWithoutExtension(callerFilePath ?? string.Empty);
      return string.IsNullOrWhiteSpace(withoutExtension) ? Log._logger : (ILogger) LogManager.GetLogger(withoutExtension);
    }
    catch (Exception ex)
    {
      InternalLogger.Warn(ex, "Error when converting CallerFilePath to logger name.");
      return Log._logger;
    }
  }
}
