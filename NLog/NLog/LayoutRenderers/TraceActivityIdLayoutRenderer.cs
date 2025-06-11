// Decompiled with JetBrains decompiler
// Type: NLog.LayoutRenderers.TraceActivityIdLayoutRenderer
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using NLog.Config;
using NLog.Internal;
using System;
using System.Diagnostics;
using System.Globalization;
using System.Text;

#nullable disable
namespace NLog.LayoutRenderers;

[LayoutRenderer("activityid")]
[ThreadSafe]
public class TraceActivityIdLayoutRenderer : LayoutRenderer, IStringValueRenderer
{
  protected override void Append(StringBuilder builder, LogEventInfo logEvent)
  {
    builder.Append(TraceActivityIdLayoutRenderer.GetStringValue());
  }

  string IStringValueRenderer.GetFormattedString(LogEventInfo logEvent)
  {
    return TraceActivityIdLayoutRenderer.GetStringValue();
  }

  private static string GetStringValue()
  {
    Guid g = TraceActivityIdLayoutRenderer.GetValue();
    return !Guid.Empty.Equals(g) ? g.ToString("D", (IFormatProvider) CultureInfo.InvariantCulture) : string.Empty;
  }

  private static Guid GetValue() => Trace.CorrelationManager.ActivityId;
}
