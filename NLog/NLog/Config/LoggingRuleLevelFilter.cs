// Decompiled with JetBrains decompiler
// Type: NLog.Config.LoggingRuleLevelFilter
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using System;

#nullable disable
namespace NLog.Config;

internal class LoggingRuleLevelFilter : ILoggingRuleLevelFilter
{
  public static readonly ILoggingRuleLevelFilter Off = (ILoggingRuleLevelFilter) new LoggingRuleLevelFilter();

  public bool[] LogLevels { get; }

  public LoggingRuleLevelFilter(bool[] logLevels = null)
  {
    this.LogLevels = new bool[NLog.LogLevel.MaxLevel.Ordinal + 1];
    if (logLevels == null)
      return;
    for (int index = 0; index < Math.Min(logLevels.Length, this.LogLevels.Length); ++index)
      this.LogLevels[index] = logLevels[index];
  }

  public LoggingRuleLevelFilter GetSimpleFilterForUpdate()
  {
    return this.LogLevels != LoggingRuleLevelFilter.Off.LogLevels ? this : new LoggingRuleLevelFilter();
  }

  public LoggingRuleLevelFilter SetLoggingLevels(NLog.LogLevel minLevel, NLog.LogLevel maxLevel, bool enable)
  {
    for (int ordinal = minLevel.Ordinal; ordinal <= Math.Min(maxLevel.Ordinal, this.LogLevels.Length - 1); ++ordinal)
      this.LogLevels[ordinal] = enable;
    return this;
  }
}
