// Decompiled with JetBrains decompiler
// Type: NLog.LayoutRenderers.ShortDateLayoutRenderer
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

[LayoutRenderer("shortdate")]
[ThreadAgnostic]
[ThreadSafe]
public class ShortDateLayoutRenderer : LayoutRenderer, IRawValue, IStringValueRenderer
{
  private ShortDateLayoutRenderer.CachedDateFormatted _cachedDateFormatted = new ShortDateLayoutRenderer.CachedDateFormatted(DateTime.MaxValue, string.Empty);

  [DefaultValue(false)]
  public bool UniversalTime { get; set; }

  protected override void Append(StringBuilder builder, LogEventInfo logEvent)
  {
    string stringValue = this.GetStringValue(logEvent);
    builder.Append(stringValue);
  }

  private string GetStringValue(LogEventInfo logEvent)
  {
    DateTime dateTime = this.GetValue(logEvent);
    ShortDateLayoutRenderer.CachedDateFormatted cachedDateFormatted = this._cachedDateFormatted;
    if (cachedDateFormatted.Date != dateTime.Date)
    {
      string formattedDate = dateTime.ToString("yyyy-MM-dd", (IFormatProvider) CultureInfo.InvariantCulture);
      this._cachedDateFormatted = cachedDateFormatted = new ShortDateLayoutRenderer.CachedDateFormatted(dateTime.Date, formattedDate);
    }
    return cachedDateFormatted.FormattedDate;
  }

  private DateTime GetValue(LogEventInfo logEvent)
  {
    DateTime dateTime = logEvent.TimeStamp;
    if (this.UniversalTime)
      dateTime = dateTime.ToUniversalTime();
    return dateTime;
  }

  bool IRawValue.TryGetRawValue(LogEventInfo logEvent, out object value)
  {
    value = (object) this.GetValue(logEvent).Date;
    return true;
  }

  string IStringValueRenderer.GetFormattedString(LogEventInfo logEvent)
  {
    return this.GetStringValue(logEvent);
  }

  private class CachedDateFormatted
  {
    public readonly DateTime Date;
    public readonly string FormattedDate;

    public CachedDateFormatted(DateTime date, string formattedDate)
    {
      this.Date = date;
      this.FormattedDate = formattedDate;
    }
  }
}
