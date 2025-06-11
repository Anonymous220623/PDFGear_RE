// Decompiled with JetBrains decompiler
// Type: Syncfusion.Windows.Controls.DateTimeHelper
// Assembly: Syncfusion.Shared.Wpf, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 18386E21-CB25-4FB0-B138-3460FC9545EB
// Assembly location: C:\Program Files\PDFgear\Syncfusion.Shared.WPF.dll

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Markup;

#nullable disable
namespace Syncfusion.Windows.Controls;

internal static class DateTimeHelper
{
  public static DateTime? AddDays(DateTime time, int days, System.Globalization.Calendar cal)
  {
    try
    {
      return new DateTime?(cal.AddDays(time, days));
    }
    catch (ArgumentException ex)
    {
      return new DateTime?();
    }
  }

  public static DateTime? AddMonths(DateTime time, int months, System.Globalization.Calendar cal)
  {
    try
    {
      return new DateTime?(cal.AddMonths(time, months));
    }
    catch (ArgumentException ex)
    {
      return new DateTime?();
    }
  }

  public static DateTime? AddYears(DateTime time, int years, System.Globalization.Calendar cal)
  {
    try
    {
      return new DateTime?(cal.AddYears(time, years));
    }
    catch (ArgumentException ex)
    {
      return new DateTime?();
    }
  }

  public static DateTime? SetYear(DateTime date, int year, System.Globalization.Calendar calendar)
  {
    return DateTimeHelper.AddYears(date, year - calendar.GetYear(date), calendar);
  }

  public static DateTime? SetDay(DateTime date, int day, System.Globalization.Calendar calendar)
  {
    return DateTimeHelper.AddDays(date, day - date.Day, calendar);
  }

  public static DateTime? SetYearMonth(DateTime date, DateTime yearMonth, System.Globalization.Calendar calendar)
  {
    DateTime? nullable = DateTimeHelper.SetYear(date, calendar.GetYear(yearMonth), calendar);
    if (nullable.HasValue)
      nullable = DateTimeHelper.AddMonths(nullable.Value, calendar.GetMonth(yearMonth) - calendar.GetMonth(date), calendar);
    return nullable;
  }

  public static int CompareDays(DateTime dt1, DateTime dt2, System.Globalization.Calendar calendar)
  {
    return DateTime.Compare(DateTimeHelper.DiscardTime(new DateTime?(dt1), calendar).Value, DateTimeHelper.DiscardTime(new DateTime?(dt2), calendar).Value);
  }

  internal static int CompareYearMonth(DateTime selectedDate, DateTime dateTime, System.Globalization.Calendar calendar)
  {
    return calendar != null ? (calendar.GetYear(selectedDate) - calendar.GetYear(dateTime)) * 12 + (calendar.GetMonth(selectedDate) - calendar.GetMonth(dateTime)) : 0;
  }

  public static int DecadeOfDate(DateTime date, System.Globalization.Calendar calendar)
  {
    date = DateTimeHelper.GetValidDateTime(date, calendar);
    return calendar.GetYear(date) - calendar.GetYear(date) % 10;
  }

  public static DateTime DiscardDayTime(DateTime d, System.Globalization.Calendar calendar)
  {
    if (calendar == null)
      return DateTime.Today;
    d = DateTimeHelper.GetValidDateTime(d, calendar);
    return new DateTime(calendar.GetYear(d), calendar.GetMonth(d), 1, 0, 0, 0, 0, calendar);
  }

  public static DateTime GetValidDateTime(DateTime d, System.Globalization.Calendar calendar)
  {
    if (calendar != null && d > calendar.MaxSupportedDateTime)
      return calendar.MaxSupportedDateTime;
    return calendar != null && d < calendar.MinSupportedDateTime ? calendar.MinSupportedDateTime : d;
  }

  public static DateTime? DiscardTime(DateTime? d, System.Globalization.Calendar calendar)
  {
    return !d.HasValue ? new DateTime?() : new DateTime?(d.Value.Date);
  }

  public static int EndOfDecade(DateTime date, System.Globalization.Calendar calendar)
  {
    return DateTimeHelper.DecadeOfDate(date, calendar) + 9;
  }

  public static DateTimeFormatInfo GetCurrentDateFormat(object culture)
  {
    return DateTimeHelper.GetDateFormat(culture as CultureInfo);
  }

  internal static CultureInfo GetCulture(FrameworkElement element)
  {
    return DependencyPropertyHelper.GetValueSource((DependencyObject) element, FrameworkElement.LanguageProperty).BaseValueSource == BaseValueSource.Default ? CultureInfo.CurrentCulture : DateTimeHelper.GetCultureInfo((DependencyObject) element);
  }

  internal static CultureInfo GetCultureInfo(DependencyObject element)
  {
    XmlLanguage xmlLanguage = (XmlLanguage) element.GetValue(FrameworkElement.LanguageProperty);
    try
    {
      return xmlLanguage.GetSpecificCulture();
    }
    catch (InvalidOperationException ex)
    {
      return CultureInfo.ReadOnly(new CultureInfo("en-us", false));
    }
  }

  internal static DateTimeFormatInfo GetDateFormat(CultureInfo culture)
  {
    if (culture.Calendar is GregorianCalendar)
      return culture.DateTimeFormat;
    GregorianCalendar gregorianCalendar = (GregorianCalendar) null;
    foreach (System.Globalization.Calendar optionalCalendar in culture.OptionalCalendars)
    {
      if (optionalCalendar is GregorianCalendar)
      {
        if (gregorianCalendar == null)
          gregorianCalendar = optionalCalendar as GregorianCalendar;
        if (((GregorianCalendar) optionalCalendar).CalendarType == GregorianCalendarTypes.Localized)
        {
          gregorianCalendar = optionalCalendar as GregorianCalendar;
          break;
        }
      }
    }
    DateTimeFormatInfo dateTimeFormat;
    if (gregorianCalendar == null)
    {
      dateTimeFormat = ((CultureInfo) CultureInfo.InvariantCulture.Clone()).DateTimeFormat;
      dateTimeFormat.Calendar = (System.Globalization.Calendar) new GregorianCalendar();
    }
    else
    {
      dateTimeFormat = ((CultureInfo) culture.Clone()).DateTimeFormat;
      dateTimeFormat.Calendar = (System.Globalization.Calendar) gregorianCalendar;
    }
    return dateTimeFormat;
  }

  public static bool InRange(DateTime date, CalendarDateRange range, System.Globalization.Calendar calendar)
  {
    return DateTimeHelper.InRange(date, range.Start, range.End, calendar);
  }

  public static bool InRange(DateTime date, DateTime start, DateTime end, System.Globalization.Calendar calendar)
  {
    return DateTimeHelper.CompareDays(date, start, calendar) > -1 && DateTimeHelper.CompareDays(date, end, calendar) < 1;
  }

  public static string ToDayString(DateTime? date, CultureInfo culture, System.Globalization.Calendar calendar)
  {
    string empty = string.Empty;
    DateTimeFormatInfo dateTimeFormat = culture.DateTimeFormat;
    if (date.HasValue && dateTimeFormat != null)
      empty = calendar.GetDayOfMonth(date.Value).ToString((IFormatProvider) dateTimeFormat);
    return empty;
  }

  public static string ToDecadeRangeString(int decade, CultureInfo culture)
  {
    string decadeRangeString = string.Empty;
    DateTimeFormatInfo dateTimeFormat = culture.DateTimeFormat;
    if (dateTimeFormat != null)
    {
      int num = decade + 9;
      decadeRangeString = $"{decade.ToString((IFormatProvider) dateTimeFormat)}-{num.ToString((IFormatProvider) dateTimeFormat)}";
    }
    return decadeRangeString;
  }

  public static string ToYearMonthPatternString(
    DateTime? date,
    CultureInfo culture,
    string[] abbreviatedMonthNames,
    System.Globalization.Calendar calendar)
  {
    string monthPatternString = string.Empty;
    DateTimeFormatInfo dateTimeFormat = culture.DateTimeFormat;
    if (date.HasValue)
      monthPatternString = abbreviatedMonthNames == null || !((IEnumerable<string>) abbreviatedMonthNames).Any<string>() ? date.Value.ToString(dateTimeFormat.YearMonthPattern, (IFormatProvider) dateTimeFormat) : (culture.TextInfo.IsRightToLeft ? $"{calendar.GetYear(date.Value).ToString()},{abbreviatedMonthNames[calendar.GetMonth(date.Value) - 1]}" : $"{abbreviatedMonthNames[calendar.GetMonth(date.Value) - 1]} {calendar.GetYear(date.Value).ToString()}");
    return monthPatternString;
  }

  public static string ToYearString(DateTime? date, CultureInfo culture, System.Globalization.Calendar calendar)
  {
    date = new DateTime?(DateTimeHelper.GetValidDateTime(date.Value, calendar));
    string empty = string.Empty;
    DateTimeFormatInfo dateTimeFormat = culture.DateTimeFormat;
    if (date.HasValue && dateTimeFormat != null)
      empty = calendar.GetYear(date.Value).ToString((IFormatProvider) dateTimeFormat);
    return empty;
  }

  public static string ToAbbreviatedMonthString(
    DateTime? date,
    CultureInfo culture,
    string[] abbreviatedMonthNames,
    System.Globalization.Calendar calendar)
  {
    string empty = string.Empty;
    DateTimeFormatInfo dateTimeFormat = culture.DateTimeFormat;
    if (date.HasValue && dateTimeFormat != null)
    {
      string[] strArray = abbreviatedMonthNames ?? dateTimeFormat.AbbreviatedMonthNames;
      if (strArray != null && strArray.Length > 0)
        empty = strArray[(calendar.GetMonth(date.Value) - 1) % strArray.Length];
    }
    return empty;
  }

  public static string ToLongDateString(DateTime? date, CultureInfo culture)
  {
    string empty = string.Empty;
    DateTimeFormatInfo dateFormat = DateTimeHelper.GetDateFormat(culture);
    if (date.HasValue && dateFormat != null)
      empty = date.Value.Date.ToString(dateFormat.LongDatePattern, (IFormatProvider) dateFormat);
    return empty;
  }

  public static DateTime GetCulturedDateTime(this DateTime d, System.Globalization.Calendar calendar)
  {
    int year = calendar.GetYear(d);
    int month = calendar.GetMonth(d);
    int dayOfMonth = calendar.GetDayOfMonth(d);
    int hour = calendar.GetHour(d);
    int minute = calendar.GetMinute(d);
    int second = calendar.GetSecond(d);
    calendar.GetMilliseconds(d);
    return new DateTime(year, month, dayOfMonth, hour, minute, second, 1, calendar);
  }
}
