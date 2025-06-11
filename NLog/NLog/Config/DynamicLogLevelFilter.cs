// Decompiled with JetBrains decompiler
// Type: NLog.Config.DynamicLogLevelFilter
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using NLog.Common;
using NLog.Internal;
using NLog.Layouts;
using System;
using System.Collections.Generic;

#nullable disable
namespace NLog.Config;

internal class DynamicLogLevelFilter : ILoggingRuleLevelFilter
{
  private readonly LoggingRule _loggingRule;
  private readonly SimpleLayout _levelFilter;
  private KeyValuePair<string, bool[]> _activeFilter;

  public bool[] LogLevels => this.GenerateLogLevels();

  public DynamicLogLevelFilter(LoggingRule loggingRule, SimpleLayout levelFilter)
  {
    this._loggingRule = loggingRule;
    this._levelFilter = levelFilter;
    this._activeFilter = new KeyValuePair<string, bool[]>(string.Empty, LoggingRuleLevelFilter.Off.LogLevels);
  }

  public LoggingRuleLevelFilter GetSimpleFilterForUpdate()
  {
    return new LoggingRuleLevelFilter(this.LogLevels);
  }

  private bool[] GenerateLogLevels()
  {
    string str = this._levelFilter.Render(LogEventInfo.CreateNullEvent());
    if (string.IsNullOrEmpty(str))
      return LoggingRuleLevelFilter.Off.LogLevels;
    KeyValuePair<string, bool[]> keyValuePair = this._activeFilter;
    if (keyValuePair.Key != str)
    {
      bool[] logLevels = str.IndexOf(',') < 0 ? this.ParseSingleLevel(str) : this.ParseLevels(str);
      if (logLevels == LoggingRuleLevelFilter.Off.LogLevels)
        return logLevels;
      keyValuePair = new KeyValuePair<string, bool[]>(str, logLevels);
      this._activeFilter = keyValuePair;
    }
    return keyValuePair.Value;
  }

  private bool[] ParseSingleLevel(string levelFilter)
  {
    try
    {
      if (StringHelpers.IsNullOrWhiteSpace(levelFilter))
        return LoggingRuleLevelFilter.Off.LogLevels;
      NLog.LogLevel logLevel = NLog.LogLevel.FromString(levelFilter.Trim());
      if (logLevel == NLog.LogLevel.Off)
        return LoggingRuleLevelFilter.Off.LogLevels;
      bool[] singleLevel = new bool[NLog.LogLevel.MaxLevel.Ordinal + 1];
      singleLevel[logLevel.Ordinal] = true;
      return singleLevel;
    }
    catch (ArgumentException ex)
    {
      object[] objArray = new object[3]
      {
        (object) this._loggingRule.RuleName,
        (object) this._loggingRule.LoggerNamePattern,
        (object) levelFilter
      };
      InternalLogger.Warn((Exception) ex, "Logging rule {0} with filter `{1}` has invalid level filter: {2}", objArray);
      return LoggingRuleLevelFilter.Off.LogLevels;
    }
  }

  private bool[] ParseLevels(string levelFilter)
  {
    string[] strArray = levelFilter.SplitAndTrimTokens(',');
    if (strArray.Length == 0)
      return LoggingRuleLevelFilter.Off.LogLevels;
    if (strArray.Length == 1)
      return this.ParseSingleLevel(strArray[0]);
    bool[] levels = new bool[NLog.LogLevel.MaxLevel.Ordinal + 1];
    foreach (string levelName in strArray)
    {
      try
      {
        NLog.LogLevel logLevel = NLog.LogLevel.FromString(levelName);
        if (!(logLevel == NLog.LogLevel.Off))
          levels[logLevel.Ordinal] = true;
      }
      catch (ArgumentException ex)
      {
        object[] objArray = new object[3]
        {
          (object) this._loggingRule.RuleName,
          (object) this._loggingRule.LoggerNamePattern,
          (object) levelFilter
        };
        InternalLogger.Warn((Exception) ex, "Logging rule {0} with filter `{1}` has invalid level filter: {2}", objArray);
      }
    }
    return levels;
  }
}
