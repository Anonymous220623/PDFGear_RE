// Decompiled with JetBrains decompiler
// Type: Sharpen.GregorianCalendar
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using System;

#nullable disable
namespace Sharpen;

public class GregorianCalendar : Calendar
{
  public const int January = 0;

  public GregorianCalendar()
  {
  }

  public GregorianCalendar(TimeZoneInfo timeZoneInfo)
    : base(timeZoneInfo)
  {
  }

  public GregorianCalendar(int year, int month, int day)
    : base(year, month, day)
  {
  }

  public GregorianCalendar(
    int year,
    int month,
    int dayOfMonth,
    int hourOfDay,
    int minute,
    int second)
    : base(year, month, dayOfMonth, hourOfDay, minute, second)
  {
  }

  public void SetGregorianChange(DateTime date)
  {
  }

  public override int GetMaximum(CalendarEnum field)
  {
    switch (field)
    {
      case CalendarEnum.Year:
        return DateTime.MaxValue.Year;
      case CalendarEnum.Month:
        return 11;
      case CalendarEnum.MonthOneBased:
        return 12;
      case CalendarEnum.DayOfMonth:
        return DateTime.DaysInMonth(this.GetTime().Year, this.GetTime().Month);
      case CalendarEnum.Hour:
        return 23;
      case CalendarEnum.HourOfDay:
        return 23;
      case CalendarEnum.Minute:
        return 60;
      case CalendarEnum.Second:
        return 60;
      case CalendarEnum.Millisecond:
        return 999;
      default:
        throw new NotSupportedException();
    }
  }
}
