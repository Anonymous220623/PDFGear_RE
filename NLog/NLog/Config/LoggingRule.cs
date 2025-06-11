// Decompiled with JetBrains decompiler
// Type: NLog.Config.LoggingRule
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using NLog.Filters;
using NLog.Layouts;
using NLog.Targets;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;

#nullable disable
namespace NLog.Config;

[NLogConfigurationItem]
public class LoggingRule
{
  private ILoggingRuleLevelFilter _logLevelFilter = LoggingRuleLevelFilter.Off;
  private LoggerNameMatcher _loggerNameMatcher = LoggerNameMatcher.Create((string) null);

  public LoggingRule()
    : this((string) null)
  {
  }

  public LoggingRule(string ruleName)
  {
    this.RuleName = ruleName;
    this.Filters = (IList<Filter>) new List<Filter>();
    this.ChildRules = (IList<LoggingRule>) new List<LoggingRule>();
    this.Targets = (IList<Target>) new List<Target>();
  }

  public LoggingRule(
    string loggerNamePattern,
    NLog.LogLevel minLevel,
    NLog.LogLevel maxLevel,
    Target target)
    : this()
  {
    this.LoggerNamePattern = loggerNamePattern;
    this.Targets.Add(target);
    this.EnableLoggingForLevels(minLevel, maxLevel);
  }

  public LoggingRule(string loggerNamePattern, NLog.LogLevel minLevel, Target target)
    : this()
  {
    this.LoggerNamePattern = loggerNamePattern;
    this.Targets.Add(target);
    this.EnableLoggingForLevels(minLevel, NLog.LogLevel.MaxLevel);
  }

  public LoggingRule(string loggerNamePattern, Target target)
    : this()
  {
    this.LoggerNamePattern = loggerNamePattern;
    this.Targets.Add(target);
  }

  public string RuleName { get; set; }

  public IList<Target> Targets { get; }

  public IList<LoggingRule> ChildRules { get; }

  internal List<LoggingRule> GetChildRulesThreadSafe()
  {
    lock (this.ChildRules)
      return this.ChildRules.ToList<LoggingRule>();
  }

  internal List<Target> GetTargetsThreadSafe()
  {
    lock (this.Targets)
      return this.Targets.ToList<Target>();
  }

  internal bool RemoveTargetThreadSafe(Target target)
  {
    lock (this.Targets)
      return this.Targets.Remove(target);
  }

  public IList<Filter> Filters { get; }

  public bool Final { get; set; }

  public string LoggerNamePattern
  {
    get => this._loggerNameMatcher.Pattern;
    set => this._loggerNameMatcher = LoggerNameMatcher.Create(value);
  }

  [NLogConfigurationIgnoreProperty]
  public ReadOnlyCollection<NLog.LogLevel> Levels
  {
    get
    {
      List<NLog.LogLevel> logLevelList = new List<NLog.LogLevel>();
      bool[] logLevels = this._logLevelFilter.LogLevels;
      for (int ordinal = NLog.LogLevel.MinLevel.Ordinal; ordinal <= NLog.LogLevel.MaxLevel.Ordinal; ++ordinal)
      {
        if (logLevels[ordinal])
          logLevelList.Add(NLog.LogLevel.FromOrdinal(ordinal));
      }
      return logLevelList.AsReadOnly();
    }
  }

  public FilterResult DefaultFilterResult { get; set; }

  public void EnableLoggingForLevel(NLog.LogLevel level)
  {
    if (level == NLog.LogLevel.Off)
      return;
    this._logLevelFilter = (ILoggingRuleLevelFilter) this._logLevelFilter.GetSimpleFilterForUpdate().SetLoggingLevels(level, level, true);
  }

  public void EnableLoggingForLevels(NLog.LogLevel minLevel, NLog.LogLevel maxLevel)
  {
    this._logLevelFilter = (ILoggingRuleLevelFilter) this._logLevelFilter.GetSimpleFilterForUpdate().SetLoggingLevels(minLevel, maxLevel, true);
  }

  internal void EnableLoggingForLevels(SimpleLayout simpleLayout)
  {
    this._logLevelFilter = (ILoggingRuleLevelFilter) new DynamicLogLevelFilter(this, simpleLayout);
  }

  internal void EnableLoggingForRange(SimpleLayout minLevel, SimpleLayout maxLevel)
  {
    this._logLevelFilter = (ILoggingRuleLevelFilter) new DynamicRangeLevelFilter(this, minLevel, maxLevel);
  }

  public void DisableLoggingForLevel(NLog.LogLevel level)
  {
    if (level == NLog.LogLevel.Off)
      return;
    this._logLevelFilter = (ILoggingRuleLevelFilter) this._logLevelFilter.GetSimpleFilterForUpdate().SetLoggingLevels(level, level, false);
  }

  public void DisableLoggingForLevels(NLog.LogLevel minLevel, NLog.LogLevel maxLevel)
  {
    this._logLevelFilter = (ILoggingRuleLevelFilter) this._logLevelFilter.GetSimpleFilterForUpdate().SetLoggingLevels(minLevel, maxLevel, false);
  }

  public void SetLoggingLevels(NLog.LogLevel minLevel, NLog.LogLevel maxLevel)
  {
    this._logLevelFilter = (ILoggingRuleLevelFilter) this._logLevelFilter.GetSimpleFilterForUpdate().SetLoggingLevels(NLog.LogLevel.MinLevel, NLog.LogLevel.MaxLevel, false).SetLoggingLevels(minLevel, maxLevel, true);
  }

  public override string ToString()
  {
    StringBuilder stringBuilder = new StringBuilder();
    stringBuilder.Append(this._loggerNameMatcher.ToString());
    stringBuilder.Append(" levels: [ ");
    bool[] logLevels = this._logLevelFilter.LogLevels;
    for (int ordinal = 0; ordinal < logLevels.Length; ++ordinal)
    {
      if (logLevels[ordinal])
        stringBuilder.AppendFormat((IFormatProvider) CultureInfo.InvariantCulture, "{0} ", new object[1]
        {
          (object) NLog.LogLevel.FromOrdinal(ordinal).ToString()
        });
    }
    stringBuilder.Append("] appendTo: [ ");
    foreach (Target target in this.GetTargetsThreadSafe())
      stringBuilder.AppendFormat((IFormatProvider) CultureInfo.InvariantCulture, "{0} ", new object[1]
      {
        (object) target.Name
      });
    stringBuilder.Append("]");
    return stringBuilder.ToString();
  }

  public bool IsLoggingEnabledForLevel(NLog.LogLevel level)
  {
    return !(level == NLog.LogLevel.Off) && this._logLevelFilter.LogLevels[level.Ordinal];
  }

  public bool NameMatches(string loggerName) => this._loggerNameMatcher.NameMatches(loggerName);
}
