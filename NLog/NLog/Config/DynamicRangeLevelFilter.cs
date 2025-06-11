// Decompiled with JetBrains decompiler
// Type: NLog.Config.DynamicRangeLevelFilter
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using NLog.Common;
using NLog.Layouts;
using System;
using System.Collections.Generic;

#nullable disable
namespace NLog.Config;

internal class DynamicRangeLevelFilter : ILoggingRuleLevelFilter
{
  private readonly LoggingRule _loggingRule;
  private readonly SimpleLayout _minLevel;
  private readonly SimpleLayout _maxLevel;
  private KeyValuePair<DynamicRangeLevelFilter.MinMaxLevels, bool[]> _activeFilter;

  public bool[] LogLevels => this.GenerateLogLevels();

  public DynamicRangeLevelFilter(
    LoggingRule loggingRule,
    SimpleLayout minLevel,
    SimpleLayout maxLevel)
  {
    this._loggingRule = loggingRule;
    this._minLevel = minLevel;
    this._maxLevel = maxLevel;
    this._activeFilter = new KeyValuePair<DynamicRangeLevelFilter.MinMaxLevels, bool[]>(new DynamicRangeLevelFilter.MinMaxLevels(string.Empty, string.Empty), LoggingRuleLevelFilter.Off.LogLevels);
  }

  public LoggingRuleLevelFilter GetSimpleFilterForUpdate()
  {
    return new LoggingRuleLevelFilter(this.LogLevels);
  }

  private bool[] GenerateLogLevels()
  {
    string str1 = this._minLevel?.Render(LogEventInfo.CreateNullEvent()) ?? string.Empty;
    string str2 = this._maxLevel?.Render(LogEventInfo.CreateNullEvent()) ?? string.Empty;
    if (string.IsNullOrEmpty(str1) && string.IsNullOrEmpty(str2))
      return LoggingRuleLevelFilter.Off.LogLevels;
    KeyValuePair<DynamicRangeLevelFilter.MinMaxLevels, bool[]> keyValuePair = this._activeFilter;
    if (!keyValuePair.Key.Equals(new DynamicRangeLevelFilter.MinMaxLevels(str1, str2)))
    {
      bool[] levelRange = this.ParseLevelRange(str1, str2);
      keyValuePair = new KeyValuePair<DynamicRangeLevelFilter.MinMaxLevels, bool[]>(new DynamicRangeLevelFilter.MinMaxLevels(str1, str2), levelRange);
      this._activeFilter = keyValuePair;
    }
    return keyValuePair.Value;
  }

  private bool[] ParseLevelRange(string minLevelFilter, string maxLevelFilter)
  {
    NLog.LogLevel logLevel1 = this.ParseLogLevel(minLevelFilter, NLog.LogLevel.MinLevel);
    NLog.LogLevel logLevel2 = this.ParseLogLevel(maxLevelFilter, NLog.LogLevel.MaxLevel);
    bool[] levelRange = new bool[NLog.LogLevel.MaxLevel.Ordinal + 1];
    if (logLevel1 != (NLog.LogLevel) null && logLevel2 != (NLog.LogLevel) null)
    {
      for (int ordinal = logLevel1.Ordinal; ordinal <= levelRange.Length - 1 && ordinal <= logLevel2.Ordinal; ++ordinal)
        levelRange[ordinal] = true;
    }
    return levelRange;
  }

  private NLog.LogLevel ParseLogLevel(string logLevel, NLog.LogLevel levelIfEmpty)
  {
    try
    {
      return string.IsNullOrEmpty(logLevel) ? levelIfEmpty : NLog.LogLevel.FromString(logLevel.Trim());
    }
    catch (ArgumentException ex)
    {
      object[] objArray = new object[3]
      {
        (object) this._loggingRule.RuleName,
        (object) this._loggingRule.LoggerNamePattern,
        (object) logLevel
      };
      InternalLogger.Warn((Exception) ex, "Logging rule {0} with filter `{1}` has invalid level filter: {2}", objArray);
      return (NLog.LogLevel) null;
    }
  }

  private struct MinMaxLevels(string minLevel, string maxLevel) : 
    IEquatable<DynamicRangeLevelFilter.MinMaxLevels>
  {
    private readonly string _minLevel = minLevel;
    private readonly string _maxLevel = maxLevel;

    public bool Equals(DynamicRangeLevelFilter.MinMaxLevels other)
    {
      return this._minLevel == other._minLevel && this._maxLevel == other._maxLevel;
    }
  }
}
