// Decompiled with JetBrains decompiler
// Type: NLog.Filters.WhenNotContainsFilter
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using NLog.Config;
using System;
using System.ComponentModel;

#nullable disable
namespace NLog.Filters;

[Filter("whenNotContains")]
public class WhenNotContainsFilter : LayoutBasedFilter
{
  [RequiredParameter]
  public string Substring { get; set; }

  [DefaultValue(false)]
  public bool IgnoreCase { get; set; }

  protected override FilterResult Check(LogEventInfo logEvent)
  {
    StringComparison comparisonType = this.IgnoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal;
    return this.Layout.Render(logEvent).IndexOf(this.Substring, comparisonType) < 0 ? this.Action : FilterResult.Neutral;
  }
}
