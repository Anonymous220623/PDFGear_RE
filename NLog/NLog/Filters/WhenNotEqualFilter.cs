// Decompiled with JetBrains decompiler
// Type: NLog.Filters.WhenNotEqualFilter
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using NLog.Config;
using System;
using System.ComponentModel;

#nullable disable
namespace NLog.Filters;

[Filter("whenNotEqual")]
public class WhenNotEqualFilter : LayoutBasedFilter
{
  [RequiredParameter]
  public string CompareTo { get; set; }

  [DefaultValue(false)]
  public bool IgnoreCase { get; set; }

  protected override FilterResult Check(LogEventInfo logEvent)
  {
    StringComparison comparisonType = this.IgnoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal;
    return !this.Layout.Render(logEvent).Equals(this.CompareTo, comparisonType) ? this.Action : FilterResult.Neutral;
  }
}
