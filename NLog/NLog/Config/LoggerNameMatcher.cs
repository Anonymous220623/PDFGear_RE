// Decompiled with JetBrains decompiler
// Type: NLog.Config.LoggerNameMatcher
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using System;
using System.Text.RegularExpressions;

#nullable disable
namespace NLog.Config;

internal abstract class LoggerNameMatcher
{
  protected readonly string _matchingArgument;
  private readonly string _toString;

  public static LoggerNameMatcher Create(string loggerNamePattern)
  {
    if (loggerNamePattern == null)
      return (LoggerNameMatcher) LoggerNameMatcher.NoneLoggerNameMatcher.Instance;
    int starPos1 = loggerNamePattern.IndexOf('*');
    int num1 = loggerNamePattern.IndexOf('*', starPos1 + 1);
    int num2 = loggerNamePattern.IndexOf('?');
    if (starPos1 < 0 && num2 < 0)
      return (LoggerNameMatcher) new LoggerNameMatcher.EqualsLoggerNameMatcher(loggerNamePattern);
    if (loggerNamePattern == "*")
      return (LoggerNameMatcher) LoggerNameMatcher.AllLoggerNameMatcher.Instance;
    if (num2 < 0)
    {
      if (starPos1 == 0 && num1 == loggerNamePattern.Length - 1)
        return (LoggerNameMatcher) new LoggerNameMatcher.ContainsLoggerNameMatcher(loggerNamePattern);
      if (num1 < 0)
      {
        LoggerNameMatcher loggerNameMatcher = LoggerNameMatcher.CreateStartsOrEndsWithLoggerNameMatcher(loggerNamePattern, starPos1);
        if (loggerNameMatcher != null)
          return loggerNameMatcher;
      }
    }
    return (LoggerNameMatcher) new LoggerNameMatcher.MultiplePatternLoggerNameMatcher(loggerNamePattern);
  }

  private static LoggerNameMatcher CreateStartsOrEndsWithLoggerNameMatcher(
    string loggerNamePattern,
    int starPos1)
  {
    if (starPos1 == 0)
      return (LoggerNameMatcher) new LoggerNameMatcher.EndsWithLoggerNameMatcher(loggerNamePattern);
    return starPos1 == loggerNamePattern.Length - 1 ? (LoggerNameMatcher) new LoggerNameMatcher.StartsWithLoggerNameMatcher(loggerNamePattern) : (LoggerNameMatcher) null;
  }

  public string Pattern { get; }

  protected LoggerNameMatcher(string pattern, string matchingArgument)
  {
    this.Pattern = pattern;
    this._matchingArgument = matchingArgument;
    this._toString = $"logNamePattern: ({matchingArgument}:{this.Name})";
  }

  public override string ToString() => this._toString;

  protected abstract string Name { get; }

  public abstract bool NameMatches(string loggerName);

  private class NoneLoggerNameMatcher : LoggerNameMatcher
  {
    public static readonly LoggerNameMatcher.NoneLoggerNameMatcher Instance = new LoggerNameMatcher.NoneLoggerNameMatcher();

    protected override string Name => "None";

    private NoneLoggerNameMatcher()
      : base((string) null, (string) null)
    {
    }

    public override bool NameMatches(string loggerName) => false;
  }

  private class AllLoggerNameMatcher : LoggerNameMatcher
  {
    public static readonly LoggerNameMatcher.AllLoggerNameMatcher Instance = new LoggerNameMatcher.AllLoggerNameMatcher();

    protected override string Name => "All";

    private AllLoggerNameMatcher()
      : base("*", (string) null)
    {
    }

    public override bool NameMatches(string loggerName) => true;
  }

  private class EqualsLoggerNameMatcher(string pattern) : LoggerNameMatcher(pattern, pattern)
  {
    protected override string Name => "Equals";

    public override bool NameMatches(string loggerName)
    {
      return loggerName == null ? this._matchingArgument == null : loggerName.Equals(this._matchingArgument, StringComparison.Ordinal);
    }
  }

  private class StartsWithLoggerNameMatcher(string pattern) : LoggerNameMatcher(pattern, pattern.Substring(0, pattern.Length - 1))
  {
    protected override string Name => "StartsWith";

    public override bool NameMatches(string loggerName)
    {
      return loggerName == null ? this._matchingArgument == null : loggerName.StartsWith(this._matchingArgument, StringComparison.Ordinal);
    }
  }

  private class EndsWithLoggerNameMatcher(string pattern) : LoggerNameMatcher(pattern, pattern.Substring(1))
  {
    protected override string Name => "EndsWith";

    public override bool NameMatches(string loggerName)
    {
      return loggerName == null ? this._matchingArgument == null : loggerName.EndsWith(this._matchingArgument, StringComparison.Ordinal);
    }
  }

  private class ContainsLoggerNameMatcher(string pattern) : LoggerNameMatcher(pattern, pattern.Substring(1, pattern.Length - 2))
  {
    protected override string Name => "Contains";

    public override bool NameMatches(string loggerName)
    {
      return loggerName == null ? this._matchingArgument == null : loggerName.IndexOf(this._matchingArgument, StringComparison.Ordinal) >= 0;
    }
  }

  private class MultiplePatternLoggerNameMatcher : LoggerNameMatcher
  {
    private readonly Regex _regex;

    protected override string Name => "MultiplePattern";

    private static string ConvertToRegex(string wildcardsPattern)
    {
      return $"^{Regex.Escape(wildcardsPattern).Replace("\\*", ".*").Replace("\\?", ".")}$";
    }

    public MultiplePatternLoggerNameMatcher(string pattern)
      : base(pattern, LoggerNameMatcher.MultiplePatternLoggerNameMatcher.ConvertToRegex(pattern))
    {
      this._regex = new Regex(this._matchingArgument, RegexOptions.CultureInvariant);
    }

    public override bool NameMatches(string loggerName)
    {
      return loggerName != null && this._regex.IsMatch(loggerName);
    }
  }
}
