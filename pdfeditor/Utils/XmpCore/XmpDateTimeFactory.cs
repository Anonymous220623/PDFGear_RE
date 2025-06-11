// Decompiled with JetBrains decompiler
// Type: XmpCore.XmpDateTimeFactory
// Assembly: pdfeditor, Version=2.1.12.0, Culture=neutral, PublicKeyToken=null
// MVID: 2F5F9A68-6C4C-49FE-9E35-B99EB66F0DB7
// Assembly location: C:\Program Files\PDFgear\pdfeditor.exe

using Sharpen;
using System;
using XmpCore.Impl;

#nullable disable
namespace XmpCore;

public static class XmpDateTimeFactory
{
  public static IXmpDateTime CreateFromCalendar(Calendar calendar)
  {
    return (IXmpDateTime) new XmpDateTime(calendar);
  }

  public static IXmpDateTime Create() => (IXmpDateTime) new XmpDateTime();

  public static IXmpDateTime Create(int year, int month, int day)
  {
    return (IXmpDateTime) new XmpDateTime()
    {
      Year = year,
      Month = month,
      Day = day
    };
  }

  public static IXmpDateTime Create(
    int year,
    int month,
    int day,
    int hour,
    int minute,
    int second,
    int nanoSecond)
  {
    return (IXmpDateTime) new XmpDateTime()
    {
      Year = year,
      Month = month,
      Day = day,
      Hour = hour,
      Minute = minute,
      Second = second,
      Nanosecond = nanoSecond
    };
  }

  public static IXmpDateTime CreateFromIso8601(string strValue)
  {
    return (IXmpDateTime) new XmpDateTime(strValue);
  }

  public static IXmpDateTime GetCurrentDateTime()
  {
    return (IXmpDateTime) new XmpDateTime((Calendar) new GregorianCalendar());
  }

  public static IXmpDateTime SetLocalTimeZone(IXmpDateTime dateTime)
  {
    Calendar calendar = dateTime.Calendar;
    calendar.SetTimeZone(TimeZoneInfo.Local);
    return (IXmpDateTime) new XmpDateTime(calendar);
  }

  public static IXmpDateTime ConvertToUtcTime(IXmpDateTime dateTime)
  {
    long timeInMillis = dateTime.Calendar.GetTimeInMillis();
    GregorianCalendar gregorianCalendar = new GregorianCalendar(TimeZoneInfo.Utc);
    gregorianCalendar.SetGregorianChange(XmpDateTime.UnixTimeToDateTime(long.MinValue));
    gregorianCalendar.SetTimeInMillis(timeInMillis);
    return (IXmpDateTime) new XmpDateTime((Calendar) gregorianCalendar);
  }

  public static IXmpDateTime ConvertToLocalTime(IXmpDateTime dateTime)
  {
    long timeInMillis = dateTime.Calendar.GetTimeInMillis();
    GregorianCalendar gregorianCalendar = new GregorianCalendar();
    gregorianCalendar.SetTimeInMillis(timeInMillis);
    return (IXmpDateTime) new XmpDateTime((Calendar) gregorianCalendar);
  }
}
