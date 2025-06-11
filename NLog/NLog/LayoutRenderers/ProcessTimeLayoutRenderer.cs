// Decompiled with JetBrains decompiler
// Type: NLog.LayoutRenderers.ProcessTimeLayoutRenderer
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 265002D3-C651-48A7-814C-2126824E1816
// Assembly location: D:\PDFGear\bin\NLog.dll

using NLog.Config;
using NLog.Internal;
using System;
using System.ComponentModel;
using System.Globalization;
using System.Text;

#nullable disable
namespace NLog.LayoutRenderers;

[LayoutRenderer("processtime")]
[ThreadAgnostic]
[ThreadSafe]
public class ProcessTimeLayoutRenderer : LayoutRenderer, IRawValue
{
  [DefaultValue(false)]
  public bool Invariant { get; set; }

  protected override void Append(StringBuilder builder, LogEventInfo logEvent)
  {
    TimeSpan ts = ProcessTimeLayoutRenderer.GetValue(logEvent);
    CultureInfo culture = this.Invariant ? (CultureInfo) null : this.GetCulture(logEvent);
    ProcessTimeLayoutRenderer.WritetTimestamp(builder, ts, culture);
  }

  bool IRawValue.TryGetRawValue(LogEventInfo logEvent, out object value)
  {
    value = (object) ProcessTimeLayoutRenderer.GetValue(logEvent);
    return true;
  }

  internal static void WritetTimestamp(StringBuilder builder, TimeSpan ts, CultureInfo culture)
  {
    string str1 = ":";
    string str2 = ".";
    if (culture != null)
    {
      str1 = culture.DateTimeFormat.TimeSeparator;
      str2 = culture.NumberFormat.NumberDecimalSeparator;
    }
    builder.Append2DigitsZeroPadded(ts.Hours);
    builder.Append(str1);
    builder.Append2DigitsZeroPadded(ts.Minutes);
    builder.Append(str1);
    builder.Append2DigitsZeroPadded(ts.Seconds);
    builder.Append(str2);
    int milliseconds = ts.Milliseconds;
    if (milliseconds < 100)
    {
      builder.Append('0');
      if (milliseconds < 10)
      {
        builder.Append('0');
        if (milliseconds < 0)
        {
          builder.Append('0');
          return;
        }
      }
    }
    builder.AppendInvariant(milliseconds);
  }

  private static TimeSpan GetValue(LogEventInfo logEvent)
  {
    return logEvent.TimeStamp.ToUniversalTime() - LogEventInfo.ZeroDate;
  }
}
