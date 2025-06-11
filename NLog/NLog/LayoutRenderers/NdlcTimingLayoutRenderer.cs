// Decompiled with JetBrains decompiler
// Type: NLog.LayoutRenderers.NdlcTimingLayoutRenderer
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using NLog.Config;
using NLog.Time;
using System;
using System.Text;

#nullable disable
namespace NLog.LayoutRenderers;

[LayoutRenderer("ndlctiming")]
[ThreadSafe]
public class NdlcTimingLayoutRenderer : LayoutRenderer
{
  public bool CurrentScope { get; set; }

  public bool ScopeBeginTime { get; set; }

  public string Format { get; set; }

  protected override void Append(StringBuilder builder, LogEventInfo logEvent)
  {
    DateTime systemTime = this.CurrentScope ? NestedDiagnosticsLogicalContext.PeekTopScopeBeginTime() : NestedDiagnosticsLogicalContext.PeekBottomScopeBeginTime();
    if (!(systemTime != DateTime.MinValue))
      return;
    if (this.ScopeBeginTime)
    {
      IFormatProvider formatProvider = this.GetFormatProvider(logEvent);
      DateTime dateTime = TimeSource.Current.FromSystemTime(systemTime);
      builder.Append(dateTime.ToString(this.Format, formatProvider));
    }
    else
    {
      TimeSpan timeSpan = systemTime != DateTime.MinValue ? DateTime.UtcNow - systemTime : TimeSpan.Zero;
      if (timeSpan < TimeSpan.Zero)
        timeSpan = TimeSpan.Zero;
      IFormatProvider formatProvider = this.GetFormatProvider(logEvent);
      builder.Append(timeSpan.ToString(this.Format, formatProvider));
    }
  }
}
