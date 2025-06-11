// Decompiled with JetBrains decompiler
// Type: NLog.Filters.ConditionBasedFilter
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using NLog.Conditions;
using NLog.Config;

#nullable disable
namespace NLog.Filters;

[Filter("when")]
public class ConditionBasedFilter : Filter
{
  private static readonly object boxedTrue = (object) true;

  [RequiredParameter]
  public ConditionExpression Condition { get; set; }

  internal FilterResult DefaultFilterResult { get; set; }

  protected override FilterResult Check(LogEventInfo logEvent)
  {
    object obj = this.Condition.Evaluate(logEvent);
    return ConditionBasedFilter.boxedTrue.Equals(obj) ? this.Action : this.DefaultFilterResult;
  }
}
