// Decompiled with JetBrains decompiler
// Type: NLog.Filters.Filter
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using NLog.Config;

#nullable disable
namespace NLog.Filters;

[NLogConfigurationItem]
public abstract class Filter
{
  protected Filter() => this.Action = FilterResult.Neutral;

  [RequiredParameter]
  public FilterResult Action { get; set; }

  internal FilterResult GetFilterResult(LogEventInfo logEvent) => this.Check(logEvent);

  protected abstract FilterResult Check(LogEventInfo logEvent);
}
