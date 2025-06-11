// Decompiled with JetBrains decompiler
// Type: NLog.Targets.ConsoleWordHighlightingRule
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using NLog.Conditions;
using NLog.Config;
using NLog.Internal;
using System.ComponentModel;
using System.Text.RegularExpressions;

#nullable disable
namespace NLog.Targets;

[NLogConfigurationItem]
public class ConsoleWordHighlightingRule
{
  private readonly RegexHelper _regexHelper = new RegexHelper();

  public ConsoleWordHighlightingRule()
  {
    this.BackgroundColor = ConsoleOutputColor.NoChange;
    this.ForegroundColor = ConsoleOutputColor.NoChange;
  }

  public ConsoleWordHighlightingRule(
    string text,
    ConsoleOutputColor foregroundColor,
    ConsoleOutputColor backgroundColor)
  {
    this._regexHelper.SearchText = text;
    this.ForegroundColor = foregroundColor;
    this.BackgroundColor = backgroundColor;
  }

  public string Regex
  {
    get => this._regexHelper.RegexPattern;
    set => this._regexHelper.RegexPattern = value;
  }

  public ConditionExpression Condition { get; set; }

  [DefaultValue(false)]
  public bool CompileRegex
  {
    get => this._regexHelper.CompileRegex;
    set => this._regexHelper.CompileRegex = value;
  }

  public string Text
  {
    get => this._regexHelper.SearchText;
    set => this._regexHelper.SearchText = value;
  }

  [DefaultValue(false)]
  public bool WholeWords
  {
    get => this._regexHelper.WholeWords;
    set => this._regexHelper.WholeWords = value;
  }

  [DefaultValue(false)]
  public bool IgnoreCase
  {
    get => this._regexHelper.IgnoreCase;
    set => this._regexHelper.IgnoreCase = value;
  }

  [DefaultValue("NoChange")]
  public ConsoleOutputColor ForegroundColor { get; set; }

  [DefaultValue("NoChange")]
  public ConsoleOutputColor BackgroundColor { get; set; }

  public System.Text.RegularExpressions.Regex CompiledRegex => this._regexHelper.Regex;

  internal MatchCollection Matches(LogEventInfo logEvent, string message)
  {
    return this.Condition != null && false.Equals(this.Condition.Evaluate(logEvent)) ? (MatchCollection) null : this._regexHelper.Matches(message);
  }
}
