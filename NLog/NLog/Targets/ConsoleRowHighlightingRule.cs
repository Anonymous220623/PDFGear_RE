// Decompiled with JetBrains decompiler
// Type: NLog.Targets.ConsoleRowHighlightingRule
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using NLog.Conditions;
using NLog.Config;
using System.ComponentModel;

#nullable disable
namespace NLog.Targets;

[NLogConfigurationItem]
public class ConsoleRowHighlightingRule
{
  static ConsoleRowHighlightingRule()
  {
    ConsoleRowHighlightingRule.Default = new ConsoleRowHighlightingRule((ConditionExpression) null, ConsoleOutputColor.NoChange, ConsoleOutputColor.NoChange);
  }

  public ConsoleRowHighlightingRule()
    : this((ConditionExpression) null, ConsoleOutputColor.NoChange, ConsoleOutputColor.NoChange)
  {
  }

  public ConsoleRowHighlightingRule(
    ConditionExpression condition,
    ConsoleOutputColor foregroundColor,
    ConsoleOutputColor backgroundColor)
  {
    this.Condition = condition;
    this.ForegroundColor = foregroundColor;
    this.BackgroundColor = backgroundColor;
  }

  public static ConsoleRowHighlightingRule Default { get; private set; }

  [RequiredParameter]
  public ConditionExpression Condition { get; set; }

  [DefaultValue("NoChange")]
  public ConsoleOutputColor ForegroundColor { get; set; }

  [DefaultValue("NoChange")]
  public ConsoleOutputColor BackgroundColor { get; set; }

  public bool CheckCondition(LogEventInfo logEvent)
  {
    return this.Condition == null || true.Equals(this.Condition.Evaluate(logEvent));
  }
}
