// Decompiled with JetBrains decompiler
// Type: Sharpen.Calendar
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using System;
using System.Globalization;

#nullable disable
namespace Sharpen;

public abstract class Calendar
{
  private static readonly TimeZoneInfo DefaultTimeZone = TimeZoneInfo.Local;
  private DateTime _mCalendarDate;
  private TimeZoneInfo _mTz;

  protected Calendar(TimeZoneInfo value)
  {
    this._mTz = value;
    this._mCalendarDate = TimeZoneInfo.ConvertTime(DateTime.Now, this._mTz);
  }

  protected Calendar()
  {
    this._mTz = Calendar.DefaultTimeZone;
    this._mCalendarDate = new DateTime(DateTime.Now.Ticks, DateTimeKind.Unspecified);
  }

  protected Calendar(int year, int month, int dayOfMonth)
  {
    this._mTz = Calendar.DefaultTimeZone;
    this._mCalendarDate = new DateTime(year, month + 1, dayOfMonth);
  }

  protected Calendar(int year, int month, int dayOfMonth, int hourOfDay, int minute, int second)
  {
    this._mTz = Calendar.DefaultTimeZone;
    bool flag = false;
    if (hourOfDay == 24)
    {
      hourOfDay = 0;
      flag = true;
    }
    this._mCalendarDate = new DateTime(year, month + 1, dayOfMonth, hourOfDay, minute, second);
    if (!flag)
      return;
    this._mCalendarDate = this._mCalendarDate.AddDays(1.0);
  }

  public long GetTimeInMillis() => this.GetTime().Ticks / 10000L;

  public void SetTimeInMillis(long millis)
  {
    this._mCalendarDate = new DateTime(millis * 10000L, DateTimeKind.Unspecified);
  }

  public DateTime GetTime()
  {
    return TimeZoneInfo.ConvertTime(this._mCalendarDate, Calendar.DefaultTimeZone);
  }

  public void SetTime(DateTime date) => this._mCalendarDate = date;

  public TimeZoneInfo GetTimeZone() => this._mTz;

  public void SetTimeZone(TimeZoneInfo value) => this._mTz = value;

  public int Get(CalendarEnum field)
  {
    switch (field)
    {
      case CalendarEnum.Year:
        return this._mCalendarDate.Year;
      case CalendarEnum.Month:
        return this._mCalendarDate.Month - 1;
      case CalendarEnum.MonthOneBased:
        return this._mCalendarDate.Month;
      case CalendarEnum.DayOfMonth:
        return this._mCalendarDate.Day;
      case CalendarEnum.Hour:
      case CalendarEnum.HourOfDay:
        return this._mCalendarDate.Hour;
      case CalendarEnum.Minute:
        return this._mCalendarDate.Minute;
      case CalendarEnum.Second:
        return this._mCalendarDate.Second;
      case CalendarEnum.Millisecond:
        return this._mCalendarDate.Millisecond;
      default:
        throw new NotSupportedException();
    }
  }

  public void Set(CalendarEnum field, int value)
  {
    int num = this.GetMaximum(field) + 1;
    switch (field)
    {
      case CalendarEnum.Year:
        value %= num;
        this._mCalendarDate = this._mCalendarDate.AddYears(value - this._mCalendarDate.Year);
        break;
      case CalendarEnum.Month:
        this._mCalendarDate = this._mCalendarDate.AddMonths(value + 1 - this._mCalendarDate.Month);
        break;
      case CalendarEnum.MonthOneBased:
        this._mCalendarDate = this._mCalendarDate.AddMonths(value - this._mCalendarDate.Month);
        break;
      case CalendarEnum.DayOfMonth:
        this._mCalendarDate = this._mCalendarDate.AddDays((double) (value - this._mCalendarDate.Day));
        break;
      case CalendarEnum.Hour:
        this._mCalendarDate = this._mCalendarDate.AddHours((double) (value - this._mCalendarDate.Hour));
        break;
      case CalendarEnum.HourOfDay:
        if (value == 24)
        {
          this.Set(CalendarEnum.Hour, 0);
          this._mCalendarDate = this._mCalendarDate.AddDays(1.0);
          break;
        }
        this.Set(CalendarEnum.Hour, value);
        break;
      case CalendarEnum.Minute:
        this._mCalendarDate = this._mCalendarDate.AddMinutes((double) (value - this._mCalendarDate.Minute));
        break;
      case CalendarEnum.Second:
        this._mCalendarDate = this._mCalendarDate.AddSeconds((double) (value - this._mCalendarDate.Second));
        break;
      case CalendarEnum.Millisecond:
        this._mCalendarDate = new DateTime(this._mCalendarDate.Year, this._mCalendarDate.Month, this._mCalendarDate.Day, this._mCalendarDate.Hour, this._mCalendarDate.Minute, this._mCalendarDate.Second, value, this._mCalendarDate.Kind);
        break;
      default:
        throw new NotSupportedException();
    }
  }

  public abstract int GetMaximum(CalendarEnum field);

  public void Set(int year, int month, int day, int hourOfDay, int minute, int second)
  {
    this.Set(CalendarEnum.Year, year);
    this.Set(CalendarEnum.Month, month);
    this.Set(CalendarEnum.DayOfMonth, day);
    this.Set(CalendarEnum.HourOfDay, hourOfDay);
    this.Set(CalendarEnum.Minute, minute);
    this.Set(CalendarEnum.Second, second);
    this.Set(CalendarEnum.Millisecond, 0);
  }

  public static Calendar GetInstance(CultureInfo culture) => (Calendar) new GregorianCalendar();

  public static Calendar GetInstance(TimeZoneInfo value) => (Calendar) new GregorianCalendar(value);
}
