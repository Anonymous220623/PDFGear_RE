// Decompiled with JetBrains decompiler
// Type: NLog.Config.SimpleConfigurator
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using NLog.Layouts;
using NLog.Targets;
using System;

#nullable disable
namespace NLog.Config;

public static class SimpleConfigurator
{
  public static void ConfigureForConsoleLogging()
  {
    SimpleConfigurator.ConfigureForConsoleLogging(NLog.LogLevel.Info);
  }

  public static void ConfigureForConsoleLogging(NLog.LogLevel minLevel)
  {
    ConsoleTarget consoleTarget = new ConsoleTarget();
    LoggingConfiguration loggingConfiguration = new LoggingConfiguration();
    loggingConfiguration.AddRule(minLevel, NLog.LogLevel.MaxLevel, (Target) consoleTarget);
    LogManager.Configuration = loggingConfiguration;
  }

  public static void ConfigureForTargetLogging(Target target)
  {
    if (target == null)
      throw new ArgumentNullException(nameof (target));
    SimpleConfigurator.ConfigureForTargetLogging(target, NLog.LogLevel.Info);
  }

  public static void ConfigureForTargetLogging(Target target, NLog.LogLevel minLevel)
  {
    if (target == null)
      throw new ArgumentNullException(nameof (target));
    LoggingConfiguration loggingConfiguration = new LoggingConfiguration();
    loggingConfiguration.AddRule(minLevel, NLog.LogLevel.MaxLevel, target);
    LogManager.Configuration = loggingConfiguration;
  }

  public static void ConfigureForFileLogging(string fileName)
  {
    SimpleConfigurator.ConfigureForFileLogging(fileName, NLog.LogLevel.Info);
  }

  public static void ConfigureForFileLogging(string fileName, NLog.LogLevel minLevel)
  {
    SimpleConfigurator.ConfigureForTargetLogging((Target) new FileTarget()
    {
      FileName = (Layout) fileName
    }, minLevel);
  }
}
