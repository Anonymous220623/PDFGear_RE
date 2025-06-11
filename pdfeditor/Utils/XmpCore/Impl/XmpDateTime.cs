// Decompiled with JetBrains decompiler
// Type: XmpCore.Impl.XmpDateTime
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using Sharpen;
using System;
using System.Globalization;

#nullable disable
namespace XmpCore.Impl;

public sealed class XmpDateTime : IXmpDateTime, IComparable
{
  private int _year;
  private int _month;
  private int _day;
  private int _hour;
  private int _minute;
  private int _second;
  private TimeZoneInfo _timeZone;
  private int _nanoseconds;
  private static readonly DateTime _unixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

  public XmpDateTime()
  {
  }

  public XmpDateTime(Sharpen.Calendar calendar)
  {
    DateTime time = calendar.GetTime();
    TimeZoneInfo timeZone = calendar.GetTimeZone();
    Sharpen.GregorianCalendar instance = (Sharpen.GregorianCalendar) Sharpen.Calendar.GetInstance(CultureInfo.InvariantCulture);
    instance.SetGregorianChange(XmpDateTime.UnixTimeToDateTime(long.MinValue));
    instance.SetTimeZone(timeZone);
    instance.SetTime(time);
    this._year = instance.Get(CalendarEnum.Year);
    this._month = instance.Get(CalendarEnum.Month) + 1;
    this._day = instance.Get(CalendarEnum.DayOfMonth);
    this._hour = instance.Get(CalendarEnum.HourOfDay);
    this._minute = instance.Get(CalendarEnum.Minute);
    this._second = instance.Get(CalendarEnum.Second);
    this._nanoseconds = instance.Get(CalendarEnum.Millisecond) * 1000000;
    this._timeZone = instance.GetTimeZone();
    this.HasDate = this.HasTime = this.HasTimeZone = true;
  }

  public XmpDateTime(DateTime date, TimeZoneInfo timeZone)
  {
    Sharpen.GregorianCalendar gregorianCalendar = new Sharpen.GregorianCalendar(timeZone);
    gregorianCalendar.SetTime(date);
    this._year = gregorianCalendar.Get(CalendarEnum.Year);
    this._month = gregorianCalendar.Get(CalendarEnum.Month) + 1;
    this._day = gregorianCalendar.Get(CalendarEnum.DayOfMonth);
    this._hour = gregorianCalendar.Get(CalendarEnum.HourOfDay);
    this._minute = gregorianCalendar.Get(CalendarEnum.Minute);
    this._second = gregorianCalendar.Get(CalendarEnum.Second);
    this._nanoseconds = gregorianCalendar.Get(CalendarEnum.Millisecond) * 1000000;
    this._timeZone = timeZone;
    this.HasDate = this.HasTime = this.HasTimeZone = true;
  }

  public XmpDateTime(string strValue) => Iso8601Converter.Parse(strValue, (IXmpDateTime) this);

  public int Year
  {
    get => this._year;
    set
    {
      this._year = Math.Min(Math.Abs(value), 9999);
      this.HasDate = true;
    }
  }

  public int Month
  {
    get => this._month;
    set
    {
      this._month = value < 1 ? 1 : (value > 12 ? 12 : value);
      this.HasDate = true;
    }
  }

  public int Day
  {
    get => this._day;
    set
    {
      this._day = value < 1 ? 1 : (value > 31 /*0x1F*/ ? 31 /*0x1F*/ : value);
      this.HasDate = true;
    }
  }

  public int Hour
  {
    get => this._hour;
    set
    {
      this._hour = Math.Min(Math.Abs(value), 23);
      this.HasTime = true;
    }
  }

  public int Minute
  {
    get => this._minute;
    set
    {
      this._minute = Math.Min(Math.Abs(value), 59);
      this.HasTime = true;
    }
  }

  public int Second
  {
    get => this._second;
    set
    {
      this._second = Math.Min(Math.Abs(value), 59);
      this.HasTime = true;
    }
  }

  public int Nanosecond
  {
    get => this._nanoseconds;
    set
    {
      this._nanoseconds = value;
      this.HasTime = true;
    }
  }

  public int CompareTo(object dt)
  {
    IXmpDateTime xmpDateTime = (IXmpDateTime) dt;
    long num = this.Calendar.GetTimeInMillis() - xmpDateTime.Calendar.GetTimeInMillis();
    return num != 0L ? Math.Sign(num) : Math.Sign((long) (this._nanoseconds - xmpDateTime.Nanosecond));
  }

  public TimeZoneInfo TimeZone
  {
    get => this._timeZone;
    set
    {
      this._timeZone = value;
      this.HasTime = true;
      this.HasTimeZone = true;
    }
  }

  public TimeSpan Offset { get; set; }

  public bool HasDate { get; private set; }

  public bool HasTime { get; private set; }

  public bool HasTimeZone { get; private set; }

  public Sharpen.Calendar Calendar
  {
    get
    {
      Sharpen.GregorianCalendar instance = (Sharpen.GregorianCalendar) Sharpen.Calendar.GetInstance(CultureInfo.InvariantCulture);
      instance.SetGregorianChange(XmpDateTime.UnixTimeToDateTime(long.MinValue));
      if (this.HasTimeZone)
        instance.SetTimeZone(this._timeZone);
      instance.Set(CalendarEnum.Year, this._year);
      instance.Set(CalendarEnum.Month, this._month - 1);
      instance.Set(CalendarEnum.DayOfMonth, this._day);
      instance.Set(CalendarEnum.HourOfDay, this._hour);
      instance.Set(CalendarEnum.Minute, this._minute);
      instance.Set(CalendarEnum.Second, this._second);
      instance.Set(CalendarEnum.Millisecond, this._nanoseconds / 1000000);
      return (Sharpen.Calendar) instance;
    }
  }

  public string ToIso8601String() => Iso8601Converter.Render((IXmpDateTime) this);

  public override string ToString() => this.ToIso8601String();

  internal static DateTime UnixTimeToDateTime(long unixTime)
  {
    return new DateTime(XmpDateTime._unixEpoch.Ticks + unixTime * 10000L);
  }

  public static DateTimeOffset UnixTimeToDateTimeOffset(long unixTime)
  {
    return new DateTimeOffset(XmpDateTime._unixEpoch.Ticks + unixTime * 10000L, TimeSpan.Zero);
  }
}
