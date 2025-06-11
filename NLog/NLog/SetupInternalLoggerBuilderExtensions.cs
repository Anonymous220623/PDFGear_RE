// Decompiled with JetBrains decompiler
// Type: NLog.SetupInternalLoggerBuilderExtensions
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using NLog.Common;
using NLog.Config;
using System;
using System.IO;

#nullable disable
namespace NLog;

public static class SetupInternalLoggerBuilderExtensions
{
  public static ISetupInternalLoggerBuilder SetMinimumLogLevel(
    this ISetupInternalLoggerBuilder setupBuilder,
    LogLevel logLevel)
  {
    InternalLogger.LogLevel = logLevel;
    return setupBuilder;
  }

  public static ISetupInternalLoggerBuilder LogToFile(
    this ISetupInternalLoggerBuilder setupBuilder,
    string fileName)
  {
    InternalLogger.LogFile = fileName;
    return setupBuilder;
  }

  public static ISetupInternalLoggerBuilder LogToConsole(
    this ISetupInternalLoggerBuilder setupBuilder,
    bool enabled)
  {
    InternalLogger.LogToConsole = enabled;
    return setupBuilder;
  }

  public static ISetupInternalLoggerBuilder LogToTrace(
    this ISetupInternalLoggerBuilder setupBuilder,
    bool enabled)
  {
    InternalLogger.LogToTrace = enabled;
    return setupBuilder;
  }

  public static ISetupInternalLoggerBuilder LogToWriter(
    this ISetupInternalLoggerBuilder setupBuilder,
    TextWriter writer)
  {
    InternalLogger.LogWriter = writer;
    return setupBuilder;
  }

  public static ISetupInternalLoggerBuilder AddLogSubscription(
    this ISetupInternalLoggerBuilder setupBuilder,
    EventHandler<InternalLoggerMessageEventArgs> eventSubscriber)
  {
    InternalLogger.LogMessageReceived += eventSubscriber;
    return setupBuilder;
  }

  public static ISetupInternalLoggerBuilder RemoveLogSubscription(
    this ISetupInternalLoggerBuilder setupBuilder,
    EventHandler<InternalLoggerMessageEventArgs> eventSubscriber)
  {
    InternalLogger.LogMessageReceived -= eventSubscriber;
    return setupBuilder;
  }
}
