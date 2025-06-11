// Decompiled with JetBrains decompiler
// Type: NLog.LayoutRenderers.DateLayoutRenderer
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

[LayoutRenderer("date")]
[ThreadAgnostic]
[ThreadSafe]
public class DateLayoutRenderer : LayoutRenderer, IRawValue, IStringValueRenderer
{
  private string _format;
  private const string _lowTimeResolutionChars = "YyMDdHh";
  private DateLayoutRenderer.CachedDateFormatted _cachedDateFormatted = new DateLayoutRenderer.CachedDateFormatted(DateTime.MinValue, string.Empty);

  public DateLayoutRenderer()
  {
    this.Format = "yyyy/MM/dd HH:mm:ss.fff";
    this.Culture = CultureInfo.InvariantCulture;
  }

  public CultureInfo Culture { get; set; }

  [DefaultParameter]
  public string Format
  {
    get => this._format;
    set
    {
      this._format = value;
      if (DateLayoutRenderer.IsLowTimeResolutionLayout(this._format))
        this._cachedDateFormatted = new DateLayoutRenderer.CachedDateFormatted(DateTime.MaxValue, string.Empty);
      else
        this._cachedDateFormatted = new DateLayoutRenderer.CachedDateFormatted(DateTime.MinValue, string.Empty);
    }
  }

  [DefaultValue(false)]
  public bool UniversalTime { get; set; }

  protected override void Append(StringBuilder builder, LogEventInfo logEvent)
  {
    builder.Append(this.GetStringValue(logEvent));
  }

  bool IRawValue.TryGetRawValue(LogEventInfo logEvent, out object value)
  {
    value = (object) this.GetDate(logEvent);
    return true;
  }

  string IStringValueRenderer.GetFormattedString(LogEventInfo logEvent)
  {
    return this.GetStringValue(logEvent);
  }

  private string GetStringValue(LogEventInfo logEvent)
  {
    IFormatProvider formatProvider = this.GetFormatProvider(logEvent, (IFormatProvider) this.Culture);
    DateTime date1 = this.GetDate(logEvent);
    DateLayoutRenderer.CachedDateFormatted cachedDateFormatted = this._cachedDateFormatted;
    DateTime date2;
    if (formatProvider != CultureInfo.InvariantCulture || cachedDateFormatted.Date == DateTime.MinValue)
    {
      cachedDateFormatted = (DateLayoutRenderer.CachedDateFormatted) null;
    }
    else
    {
      DateTime date3 = cachedDateFormatted.Date;
      date2 = date1.Date;
      DateTime dateTime = date2.AddHours((double) date1.Hour);
      if (date3 == dateTime)
        return cachedDateFormatted.FormattedDate;
    }
    string formattedDate = date1.ToString(this._format, formatProvider);
    if (cachedDateFormatted != null)
    {
      date2 = date1.Date;
      this._cachedDateFormatted = new DateLayoutRenderer.CachedDateFormatted(date2.AddHours((double) date1.Hour), formattedDate);
    }
    return formattedDate;
  }

  private DateTime GetDate(LogEventInfo logEvent)
  {
    DateTime date = logEvent.TimeStamp;
    if (this.UniversalTime)
      date = date.ToUniversalTime();
    return date;
  }

  private static bool IsLowTimeResolutionLayout(string dateTimeFormat)
  {
    for (int index = 0; index < dateTimeFormat.Length; ++index)
    {
      char c = dateTimeFormat[index];
      if (char.IsLetter(c) && "YyMDdHh".IndexOf(c) < 0)
        return false;
    }
    return true;
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
