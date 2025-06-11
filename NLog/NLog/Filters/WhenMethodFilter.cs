// Decompiled with JetBrains decompiler
// Type: NLog.Filters.WhenMethodFilter
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using System;

#nullable disable
namespace NLog.Filters;

public class WhenMethodFilter : Filter
{
  private readonly Func<LogEventInfo, FilterResult> _filterMethod;

  public WhenMethodFilter(Func<LogEventInfo, FilterResult> filterMethod)
  {
    this._filterMethod = filterMethod != null ? filterMethod : throw new ArgumentNullException(nameof (filterMethod));
  }

  protected override FilterResult Check(LogEventInfo logEvent) => this._filterMethod(logEvent);
}
