// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.DateTimeAxisHelper
// Assembly: Syncfusion.SfChart.WPF, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 279D70C6-7936-4F3E-B6CF-79EFEF734826
// Assembly location: C:\Program Files\PDFgear\Syncfusion.SfChart.WPF.dll

using System;
using System.Globalization;
using System.Windows;

#nullable disable
namespace Syncfusion.UI.Xaml.Charts;

internal static class DateTimeAxisHelper
{
  internal static void GenerateVisibleLabels(
    ChartAxisBase2D axis,
    object minimum,
    object maximum,
    DateTimeIntervalType intervalType)
  {
    DateTime dateTime1 = axis.VisibleRange.Start.FromOADate();
    double interval = DateTimeAxisHelper.IncreaseInterval(dateTime1, axis.VisibleInterval, intervalType).ToOADate() - axis.VisibleRange.Start;
    DateTime dateTime2 = axis.EdgeLabelsVisibilityMode == EdgeLabelsVisibilityMode.AlwaysVisible || axis.EdgeLabelsVisibilityMode == EdgeLabelsVisibilityMode.Visible && !axis.IsZoomed ? dateTime1 : DateTimeAxisHelper.AlignRangeStart(dateTime1, axis.VisibleInterval, intervalType);
    DateTimeAxis axis1 = axis as DateTimeAxis;
    string days = "";
    double distinctDate = 0.0;
    double oaDate = dateTime2.ToOADate();
    if (axis1 != null && axis1.EnableBusinessHours)
    {
      days = axis1.InternalWorkingDays;
      dateTime2 = new DateTime(dateTime1.Year, dateTime1.Month, dateTime1.Day).AddHours(axis1.OpenTime);
      oaDate = dateTime2.ToOADate();
    }
    double num = double.NaN;
    if (axis.IsDefaultRange && axis1.Interval != 0.0)
    {
      switch (intervalType)
      {
        case DateTimeIntervalType.Milliseconds:
          axis.VisibleRange = new DoubleRange(0.0, 1.138888888888889E-05);
          break;
        case DateTimeIntervalType.Seconds:
          axis.VisibleRange = new DoubleRange(0.0, 0.00068333333333333332);
          break;
        case DateTimeIntervalType.Minutes:
          axis.VisibleRange = new DoubleRange(0.0, 0.041);
          break;
      }
    }
    for (; oaDate <= axis.VisibleRange.End && oaDate != num; oaDate = dateTime2.ToOADate())
    {
      if (axis.VisibleRange.Inside(oaDate))
        axis.VisibleLabels.Add((ChartAxisLabel) new DateTimeAxisLabel(oaDate, (object) dateTime2.ToString(axis.LabelFormat, (IFormatProvider) CultureInfo.CurrentCulture), oaDate)
        {
          IntervalType = axis1.ActualIntervalType,
          IsTransition = DateTimeAxisHelper.GetTransitionState(ref distinctDate, dateTime2, axis1.ActualIntervalType)
        });
      dateTime2 = DateTimeAxisHelper.IncreaseNonWorkingInterval(days, axis1, dateTime2, axis.VisibleInterval, intervalType);
      if (axis1 != null && axis1.EnableBusinessHours)
        dateTime2 = DateTimeAxisHelper.NonWorkingDaysIntervals(axis1, dateTime2);
      if (axis.smallTicksRequired)
        axis.AddSmallTicksPoint(oaDate, interval);
      num = oaDate;
    }
    double end = axis.VisibleRange.End;
    if ((maximum == null || !end.FromOADate().Equals(maximum)) && axis.EdgeLabelsVisibilityMode != EdgeLabelsVisibilityMode.AlwaysVisible && (axis.EdgeLabelsVisibilityMode != EdgeLabelsVisibilityMode.Visible || axis.IsZoomed) || end.Equals(oaDate - interval))
      return;
    dateTime2 = end.FromOADate();
    axis.VisibleLabels.Add((ChartAxisLabel) new DateTimeAxisLabel(end, (object) dateTime2.ToString(axis.LabelFormat, (IFormatProvider) CultureInfo.CurrentCulture), end)
    {
      IntervalType = axis1.ActualIntervalType,
      IsTransition = DateTimeAxisHelper.GetTransitionState(ref distinctDate, dateTime2, axis1.ActualIntervalType)
    });
  }

  internal static void GenerateVisibleLabels3D(
    ChartAxis axis,
    object minimum,
    object maximum,
    DateTimeIntervalType intervalType)
  {
    DateTime dateTime1 = axis.VisibleRange.Start.FromOADate();
    double interval = DateTimeAxisHelper.IncreaseInterval(dateTime1, axis.VisibleInterval, intervalType).ToOADate() - axis.VisibleRange.Start;
    DateTime dateTime2 = axis.EdgeLabelsVisibilityMode == EdgeLabelsVisibilityMode.AlwaysVisible || axis.EdgeLabelsVisibilityMode == EdgeLabelsVisibilityMode.Visible ? dateTime1 : DateTimeAxisHelper.AlignRangeStart(dateTime1, axis.VisibleInterval, intervalType);
    DateTimeAxis axis1 = axis as DateTimeAxis;
    string days = "";
    double distinctDate = 0.0;
    double oaDate = dateTime2.ToOADate();
    if (axis1 != null && axis1.EnableBusinessHours)
    {
      days = axis1.InternalWorkingDays;
      dateTime2 = new DateTime(dateTime1.Year, dateTime1.Month, dateTime1.Day).AddHours(axis1.OpenTime);
      oaDate = dateTime2.ToOADate();
    }
    DateTimeAxis3D dateTimeAxis3D = axis as DateTimeAxis3D;
    for (; oaDate <= axis.VisibleRange.End; oaDate = dateTime2.ToOADate())
    {
      if (axis.VisibleRange.Inside(oaDate))
        axis.VisibleLabels.Add((ChartAxisLabel) new DateTimeAxisLabel(oaDate, (object) dateTime2.ToString(axis.LabelFormat, (IFormatProvider) CultureInfo.CurrentCulture), oaDate)
        {
          IntervalType = dateTimeAxis3D.ActualIntervalType3D,
          IsTransition = DateTimeAxisHelper.GetTransitionState(ref distinctDate, dateTime2, dateTimeAxis3D.ActualIntervalType3D)
        });
      dateTime2 = DateTimeAxisHelper.IncreaseNonWorkingInterval(days, axis1, dateTime2, axis.VisibleInterval, intervalType);
      if (axis1 != null && axis1.EnableBusinessHours)
        dateTime2 = DateTimeAxisHelper.NonWorkingDaysIntervals(axis1, dateTime2);
      if (axis.smallTicksRequired)
        axis.AddSmallTicksPoint(oaDate, interval);
    }
    if ((maximum == null || !axis.VisibleRange.End.FromOADate().Equals(maximum)) && axis.EdgeLabelsVisibilityMode != EdgeLabelsVisibilityMode.AlwaysVisible && axis.EdgeLabelsVisibilityMode != EdgeLabelsVisibilityMode.Visible || axis.VisibleRange.End.Equals(oaDate - interval))
      return;
    axis.VisibleLabels.Add((ChartAxisLabel) new DateTimeAxisLabel(oaDate, (object) dateTime2.ToString(axis.LabelFormat, (IFormatProvider) CultureInfo.CurrentCulture), oaDate)
    {
      IntervalType = dateTimeAxis3D.ActualIntervalType3D,
      IsTransition = DateTimeAxisHelper.GetTransitionState(ref distinctDate, dateTime2, dateTimeAxis3D.ActualIntervalType3D)
    });
  }

  internal static bool GetTransitionState(
    ref double distinctDate,
    DateTime currentDate,
    DateTimeIntervalType intervalType)
  {
    switch (intervalType)
    {
      case DateTimeIntervalType.Milliseconds:
        if (distinctDate != (double) currentDate.Second)
        {
          distinctDate = (double) currentDate.Second;
          return true;
        }
        break;
      case DateTimeIntervalType.Seconds:
        if (distinctDate != (double) currentDate.Minute)
        {
          distinctDate = (double) currentDate.Minute;
          return true;
        }
        break;
      case DateTimeIntervalType.Minutes:
        if (distinctDate != (double) currentDate.Hour)
        {
          distinctDate = (double) currentDate.Hour;
          return true;
        }
        break;
      case DateTimeIntervalType.Hours:
        if (distinctDate != (double) currentDate.Day)
        {
          distinctDate = (double) currentDate.Day;
          return true;
        }
        break;
      case DateTimeIntervalType.Days:
        if (distinctDate != (double) currentDate.Month)
        {
          distinctDate = (double) currentDate.Month;
          return true;
        }
        break;
      case DateTimeIntervalType.Months:
        if (distinctDate != (double) currentDate.Year)
        {
          distinctDate = (double) currentDate.Year;
          return true;
        }
        break;
    }
    return false;
  }

  internal static DateTime IncreaseInterval(
    DateTime date,
    double interval,
    DateTimeIntervalType intervalType)
  {
    TimeSpan timeSpan = new TimeSpan(0L);
    switch (intervalType)
    {
      case DateTimeIntervalType.Milliseconds:
        timeSpan = TimeSpan.FromMilliseconds(interval);
        break;
      case DateTimeIntervalType.Seconds:
        timeSpan = TimeSpan.FromSeconds(interval);
        break;
      case DateTimeIntervalType.Minutes:
        timeSpan = TimeSpan.FromMinutes(interval);
        break;
      case DateTimeIntervalType.Hours:
        timeSpan = TimeSpan.FromHours(interval);
        break;
      case DateTimeIntervalType.Days:
        timeSpan = TimeSpan.FromDays(interval);
        break;
      case DateTimeIntervalType.Months:
        date = date.AddMonths((int) Math.Floor(interval));
        timeSpan = TimeSpan.FromDays(30.0 * (interval - Math.Floor(interval)));
        break;
      case DateTimeIntervalType.Years:
        date = date.AddYears((int) Math.Floor(interval));
        timeSpan = TimeSpan.FromDays(365.0 * (interval - Math.Floor(interval)));
        break;
    }
    return date.Add(timeSpan);
  }

  internal static void CalculateVisibleRange(
    ChartAxisBase2D axis,
    Size availableSize,
    double interval)
  {
    if (axis.ZoomFactor < 1.0 || axis.ZoomPosition > 0.0)
    {
      if (interval != 0.0 || !double.IsNaN(interval))
        axis.VisibleInterval = axis.EnableAutoIntervalOnZooming ? axis.CalculateNiceInterval(axis.VisibleRange, availableSize) : axis.ActualInterval;
      else
        axis.VisibleInterval = axis.CalculateNiceInterval(axis.VisibleRange, availableSize);
    }
    else
    {
      DateTimeAxis dateTimeAxis = axis as DateTimeAxis;
      if (dateTimeAxis.IntervalType != DateTimeIntervalType.Auto)
        dateTimeAxis.ActualIntervalType = dateTimeAxis.IntervalType;
      if (interval != 0.0 || !double.IsNaN(interval))
        axis.VisibleInterval = axis.ActualInterval;
      else
        axis.VisibleInterval = axis.CalculateNiceInterval(axis.VisibleRange, availableSize);
    }
  }

  internal static DoubleRange ApplyRangePadding(
    ChartAxis axis,
    DoubleRange range,
    double interval,
    DateTimeRangePadding rangePadding,
    DateTimeIntervalType intervalType)
  {
    DateTime startDate = range.Start.FromOADate();
    DateTime endDate = range.End.FromOADate();
    if (axis is DateTimeAxis axis1 && axis1.EnableBusinessHours)
      return DateTimeAxisHelper.ApplyBusinessHoursRangePadding(axis1, range, interval, rangePadding, intervalType);
    if (rangePadding == DateTimeRangePadding.Auto || rangePadding == DateTimeRangePadding.None)
      return range;
    switch (intervalType)
    {
      case DateTimeIntervalType.Milliseconds:
        int millisecond1 = (int) ((double) (int) ((double) startDate.Millisecond / interval) * interval);
        int millisecond2 = range.End.FromOADate().Millisecond + (startDate.Millisecond - millisecond1);
        if (millisecond2 > 999)
          millisecond2 = 999;
        DateTime roundStartDate1 = new DateTime(startDate.Year, startDate.Month, startDate.Day, startDate.Hour, startDate.Minute, startDate.Second, millisecond1);
        DateTime roundEndDate1 = new DateTime(endDate.Year, endDate.Month, endDate.Day, endDate.Hour, endDate.Minute, endDate.Second, millisecond2);
        DateTime additionalStartDate1 = startDate.AddMilliseconds(-interval);
        DateTime additionalEndDate1 = endDate.AddMilliseconds(interval);
        return DateTimeAxisHelper.ApplyRangePadding(rangePadding, additionalStartDate1, additionalEndDate1, roundStartDate1, roundEndDate1, startDate, endDate);
      case DateTimeIntervalType.Seconds:
        int second1 = (int) ((double) (int) ((double) startDate.Second / interval) * interval);
        int second2 = range.End.FromOADate().Second + (startDate.Second - second1);
        if (second2 > 59)
          second2 = 59;
        DateTime roundStartDate2 = new DateTime(startDate.Year, startDate.Month, startDate.Day, startDate.Hour, startDate.Minute, second1, 0);
        DateTime roundEndDate2 = new DateTime(endDate.Year, endDate.Month, endDate.Day, endDate.Hour, endDate.Minute, second2, 999);
        DateTime additionalStartDate2 = new DateTime(startDate.Year, startDate.Month, startDate.Day, startDate.Hour, startDate.Minute, startDate.Second, 0).AddSeconds(-interval);
        DateTime additionalEndDate2 = new DateTime(endDate.Year, endDate.Month, endDate.Day, endDate.Hour, endDate.Minute, endDate.Second, 999).AddSeconds(interval);
        return DateTimeAxisHelper.ApplyRangePadding(rangePadding, additionalStartDate2, additionalEndDate2, roundStartDate2, roundEndDate2, startDate, endDate);
      case DateTimeIntervalType.Minutes:
        int minute1 = (int) ((double) (int) ((double) startDate.Minute / interval) * interval);
        int minute2 = range.End.FromOADate().Minute + (startDate.Minute - minute1);
        if (minute2 > 59)
          minute2 = 59;
        DateTime roundStartDate3 = new DateTime(startDate.Year, startDate.Month, startDate.Day, startDate.Hour, minute1, 0);
        DateTime roundEndDate3 = new DateTime(endDate.Year, endDate.Month, endDate.Day, endDate.Hour, minute2, 59);
        DateTime additionalStartDate3 = new DateTime(startDate.Year, startDate.Month, startDate.Day, startDate.Hour, startDate.Minute, 0).AddMinutes(-interval);
        DateTime additionalEndDate3 = new DateTime(endDate.Year, endDate.Month, endDate.Day, endDate.Hour, endDate.Minute, 59).AddMinutes(interval);
        return DateTimeAxisHelper.ApplyRangePadding(rangePadding, additionalStartDate3, additionalEndDate3, roundStartDate3, roundEndDate3, startDate, endDate);
      case DateTimeIntervalType.Hours:
        int hour1 = (int) ((double) (int) ((double) startDate.Hour / interval) * interval);
        int hour2 = endDate.Hour + (startDate.Hour - hour1);
        if (hour2 > 23)
          hour2 = 23;
        DateTime roundStartDate4 = new DateTime(startDate.Year, startDate.Month, startDate.Day, hour1, 0, 0);
        DateTime roundEndDate4 = new DateTime(endDate.Year, endDate.Month, endDate.Day, hour2, 59, 59);
        DateTime additionalStartDate4 = new DateTime(startDate.Year, startDate.Month, startDate.Day, startDate.Hour, 0, 0).AddHours((double) (int) -interval);
        DateTime additionalEndDate4 = new DateTime(endDate.Year, endDate.Month, endDate.Day, endDate.Hour, 59, 59).AddHours((double) (int) interval);
        return DateTimeAxisHelper.ApplyRangePadding(rangePadding, additionalStartDate4, additionalEndDate4, roundStartDate4, roundEndDate4, startDate, endDate);
      case DateTimeIntervalType.Days:
        int day = (int) ((double) (int) ((double) startDate.Day / interval) * interval);
        if (day <= 0)
          day = 1;
        if (startDate.Day - day <= 0)
          ;
        DateTime roundStartDate5 = new DateTime(startDate.Year, startDate.Month, day, 0, 0, 0);
        DateTime roundEndDate5 = new DateTime(endDate.Year, endDate.Month, endDate.Day, 23, 59, 59);
        DateTime additionalStartDate5 = new DateTime(startDate.Year, startDate.Month, day, 0, 0, 0).AddDays((double) (int) -interval);
        DateTime additionalEndDate5 = new DateTime(endDate.Year, endDate.Month, endDate.Day, 23, 59, 59).AddDays((double) (int) interval);
        return DateTimeAxisHelper.ApplyRangePadding(rangePadding, additionalStartDate5, additionalEndDate5, roundStartDate5, roundEndDate5, startDate, endDate);
      case DateTimeIntervalType.Months:
        int month1 = (int) ((double) (int) ((double) startDate.Month / interval) * interval);
        if (month1 <= 0)
          month1 = 1;
        int month2 = range.End.FromOADate().Month + (startDate.Month - month1);
        if (month2 <= 0)
          month2 = 1;
        if (month2 > 12)
          month2 = 12;
        DateTime roundStartDate6 = new DateTime(startDate.Year, month1, 1, 0, 0, 0);
        DateTime roundEndDate6 = new DateTime(endDate.Year, month2, month2 == 2 ? 28 : 30, 0, 0, 0);
        DateTime additionalStartDate6 = new DateTime(startDate.Year, month1, 1, 0, 0, 0).AddMonths((int) -interval);
        DateTime additionalEndDate6 = new DateTime(endDate.Year, month2, month2 == 2 ? 28 : 30, 0, 0, 0).AddMonths((int) interval);
        return DateTimeAxisHelper.ApplyRangePadding(rangePadding, additionalStartDate6, additionalEndDate6, roundStartDate6, roundEndDate6, startDate, endDate);
      case DateTimeIntervalType.Years:
        int year1 = (int) ((double) (int) ((double) startDate.Year / interval) * interval);
        int year2 = endDate.Year + (startDate.Year - year1);
        if (year1 <= 0)
          year1 = 1;
        if (year2 <= 0)
          year2 = 1;
        DateTime roundStartDate7 = new DateTime(year1, 1, 1, 0, 0, 0);
        DateTime roundEndDate7 = new DateTime(year2, 12, 31 /*0x1F*/, 23, 59, 59);
        DateTime additionalStartDate7 = new DateTime(year1 - (int) interval, 1, 1, 0, 0, 0);
        DateTime additionalEndDate7 = new DateTime(year2 + (int) interval, 12, 31 /*0x1F*/, 23, 59, 59);
        return DateTimeAxisHelper.ApplyRangePadding(rangePadding, additionalStartDate7, additionalEndDate7, roundStartDate7, roundEndDate7, startDate, endDate);
      default:
        return range;
    }
  }

  private static DoubleRange ApplyRangePadding(
    DateTimeRangePadding rangePadding,
    DateTime additionalStartDate,
    DateTime additionalEndDate,
    DateTime roundStartDate,
    DateTime roundEndDate,
    DateTime startDate,
    DateTime endDate)
  {
    switch (rangePadding)
    {
      case DateTimeRangePadding.Round:
        return new DoubleRange(roundStartDate.ToOADate(), roundEndDate.ToOADate());
      case DateTimeRangePadding.Additional:
        return new DoubleRange(additionalStartDate.ToOADate(), additionalEndDate.ToOADate());
      case DateTimeRangePadding.RoundStart:
        return new DoubleRange(roundStartDate.ToOADate(), endDate.ToOADate());
      case DateTimeRangePadding.RoundEnd:
        return new DoubleRange(startDate.ToOADate(), roundEndDate.ToOADate());
      case DateTimeRangePadding.PrependInterval:
        return new DoubleRange(additionalStartDate.ToOADate(), endDate.ToOADate());
      case DateTimeRangePadding.AppendInterval:
        return new DoubleRange(startDate.ToOADate(), additionalEndDate.ToOADate());
      default:
        return new DoubleRange(startDate.ToOADate(), endDate.ToOADate());
    }
  }

  internal static DoubleRange ApplyBusinessHoursRangePadding(
    DateTimeAxis axis,
    DoubleRange range,
    double interval,
    DateTimeRangePadding rangePadding,
    DateTimeIntervalType intervalType)
  {
    DateTime startDate = range.Start.FromOADate();
    DateTime endDate = range.End.FromOADate();
    TimeSpan timeSpan1 = new TimeSpan((int) axis.OpenTime, 0, 0);
    TimeSpan timeSpan2 = new TimeSpan((int) axis.CloseTime, 0, 0);
    if (rangePadding == DateTimeRangePadding.Auto || rangePadding == DateTimeRangePadding.None)
      return range;
    switch (intervalType)
    {
      case DateTimeIntervalType.Milliseconds:
        int millisecond1 = (int) ((double) (int) ((double) startDate.Millisecond / interval) * interval);
        int millisecond2 = range.End.FromOADate().Millisecond + (startDate.Millisecond - millisecond1);
        if (millisecond2 > 999)
          millisecond2 = 999;
        DateTime roundStartDate1 = new DateTime(startDate.Year, startDate.Month, startDate.Day, startDate.Hour, startDate.Minute, startDate.Second, millisecond1);
        DateTime roundEndDate1 = new DateTime(endDate.Year, endDate.Month, endDate.Day, endDate.Hour, endDate.Minute, endDate.Second, millisecond2);
        DateTime additionalStartDate1 = startDate.AddMilliseconds(-interval);
        DateTime additionalEndDate1 = endDate.AddMilliseconds(interval);
        return DateTimeAxisHelper.ApplyRangePadding(rangePadding, additionalStartDate1, additionalEndDate1, roundStartDate1, roundEndDate1, startDate, endDate);
      case DateTimeIntervalType.Seconds:
        int second1 = (int) ((double) (int) ((double) startDate.Second / interval) * interval);
        int second2 = range.End.FromOADate().Second + (startDate.Second - second1);
        if (second2 > 59)
          second2 = 59;
        DateTime roundStartDate2 = new DateTime(startDate.Year, startDate.Month, startDate.Day, startDate.Hour, startDate.Minute, second1, 0);
        DateTime roundEndDate2 = new DateTime(endDate.Year, endDate.Month, endDate.Day, endDate.Hour, endDate.Minute, second2, 999);
        DateTime additionalStartDate2 = new DateTime(startDate.Year, startDate.Month, startDate.Day, startDate.Hour, startDate.Minute, startDate.Second, 0).AddSeconds(-interval);
        DateTime additionalEndDate2 = new DateTime(endDate.Year, endDate.Month, endDate.Day, endDate.Hour, endDate.Minute, endDate.Second, 999).AddSeconds(interval);
        return DateTimeAxisHelper.ApplyRangePadding(rangePadding, additionalStartDate2, additionalEndDate2, roundStartDate2, roundEndDate2, startDate, endDate);
      case DateTimeIntervalType.Minutes:
        int minute1 = (int) ((double) (int) ((double) startDate.Minute / interval) * interval);
        int minute2 = range.End.FromOADate().Minute + (startDate.Minute - minute1);
        if (minute2 > 59)
          minute2 = 59;
        DateTime roundStartDate3 = new DateTime(startDate.Year, startDate.Month, startDate.Day, startDate.Hour, minute1, 0);
        DateTime roundEndDate3 = new DateTime(endDate.Year, endDate.Month, endDate.Day, endDate.Hour, minute2, 59);
        DateTime additionalStartDate3 = new DateTime(startDate.Year, startDate.Month, startDate.Day, startDate.Hour, startDate.Minute, 0).AddMinutes(-interval);
        DateTime additionalEndDate3 = new DateTime(endDate.Year, endDate.Month, endDate.Day, endDate.Hour, endDate.Minute, 59).AddMinutes(interval);
        return DateTimeAxisHelper.ApplyRangePadding(rangePadding, additionalStartDate3, additionalEndDate3, roundStartDate3, roundEndDate3, startDate, endDate);
      case DateTimeIntervalType.Hours:
        DateTime roundStartDate4 = new DateTime(startDate.Year, startDate.Month, startDate.Day, timeSpan1.Hours, 0, 0);
        DateTime roundEndDate4 = new DateTime(endDate.Year, endDate.Month, endDate.Day, timeSpan2.Hours, 0, 0);
        DateTime additionalStartDate4 = new DateTime(startDate.Year, startDate.Month, startDate.Day, startDate.Hour, 0, 0).AddHours(-interval);
        DateTime additionalEndDate4 = new DateTime(endDate.Year, endDate.Month, endDate.Day, endDate.Hour, 0, 0).AddHours(interval);
        return DateTimeAxisHelper.ApplyRangePadding(rangePadding, additionalStartDate4, additionalEndDate4, roundStartDate4, roundEndDate4, startDate, endDate);
      case DateTimeIntervalType.Days:
        int day = (int) ((double) (int) ((double) startDate.Day / interval) * interval);
        if (day <= 0)
          day = 1;
        int num = startDate.Day - day;
        if (num < 0)
          num = 1;
        DateTime roundStartDate5 = new DateTime(startDate.Year, startDate.Month, day, timeSpan1.Hours, timeSpan1.Minutes, timeSpan1.Seconds);
        DateTime roundEndDate5 = new DateTime(endDate.Year, endDate.Month, endDate.Day, timeSpan2.Hours, timeSpan2.Minutes, timeSpan2.Seconds).AddDays((double) num);
        DateTime additionalStartDate5 = new DateTime(startDate.Year, startDate.Month, day, timeSpan1.Hours, timeSpan1.Minutes, timeSpan1.Seconds).AddDays(-interval);
        DateTime additionalEndDate5 = new DateTime(endDate.Year, endDate.Month, endDate.Day, timeSpan2.Hours, timeSpan2.Minutes, timeSpan2.Seconds).AddDays(interval + (double) num);
        return DateTimeAxisHelper.ApplyRangePadding(rangePadding, additionalStartDate5, additionalEndDate5, roundStartDate5, roundEndDate5, startDate, endDate);
      case DateTimeIntervalType.Months:
        int month1 = (int) ((double) (int) ((double) startDate.Month / interval) * interval);
        if (month1 <= 0)
          month1 = 1;
        int month2 = range.End.FromOADate().Month + (startDate.Month - month1);
        if (month2 <= 0)
          month2 = 1;
        if (month2 > 12)
          month2 = 12;
        DateTime roundStartDate6 = new DateTime(startDate.Year, month1, 1, 0, 0, 0);
        DateTime roundEndDate6 = new DateTime(endDate.Year, month2, month2 == 2 ? 28 : 30, 0, 0, 0);
        DateTime additionalStartDate6 = new DateTime(startDate.Year, month1, 1, 0, 0, 0).AddMonths((int) -interval);
        DateTime additionalEndDate6 = new DateTime(endDate.Year, month2, month2 == 2 ? 28 : 30, 0, 0, 0).AddMonths((int) interval);
        return DateTimeAxisHelper.ApplyRangePadding(rangePadding, additionalStartDate6, additionalEndDate6, roundStartDate6, roundEndDate6, startDate, endDate);
      case DateTimeIntervalType.Years:
        int year1 = (int) ((double) (int) ((double) startDate.Year / interval) * interval);
        int year2 = endDate.Year + (startDate.Year - year1);
        if (year1 <= 0)
          year1 = 1;
        if (year2 <= 0)
          year2 = 1;
        DateTime roundStartDate7 = new DateTime(year1, 1, 1, 0, 0, 0);
        DateTime roundEndDate7 = new DateTime(year2, 12, 31 /*0x1F*/, 23, 59, 59);
        DateTime additionalStartDate7 = new DateTime(year1 - (int) interval, 1, 1, 0, 0, 0);
        DateTime additionalEndDate7 = new DateTime(year2 + (int) interval, 12, 31 /*0x1F*/, 23, 59, 59);
        return DateTimeAxisHelper.ApplyRangePadding(rangePadding, additionalStartDate7, additionalEndDate7, roundStartDate7, roundEndDate7, startDate, endDate);
      default:
        DateTime date1 = range.Start.FromOADate();
        DateTime date2 = range.End.FromOADate();
        DateTime dateTime1 = date1.ValidateNonWorkingDate(axis.InternalWorkingDays, true, axis.NonWorkingDays.Count).ValidateNonWorkingHours(axis.OpenTime, axis.CloseTime, true).ValidateNonWorkingDate(axis.InternalWorkingDays, true, axis.NonWorkingDays.Count);
        DateTime dateTime2 = date2.ValidateNonWorkingDate(axis.InternalWorkingDays, false, axis.NonWorkingDays.Count).ValidateNonWorkingHours(axis.OpenTime, axis.CloseTime, false).ValidateNonWorkingDate(axis.InternalWorkingDays, false, axis.NonWorkingDays.Count);
        return new DoubleRange(dateTime1.ToOADate(), dateTime2.ToOADate());
    }
  }

  internal static void OnMinMaxChanged(ChartAxis axis, DateTime? minimum, DateTime? maximum)
  {
    if (minimum.HasValue || maximum.HasValue)
    {
      double start = !minimum.HasValue ? DateTime.MinValue.ToOADate() : minimum.Value.ToOADate();
      double end = !maximum.HasValue ? DateTime.MaxValue.ToOADate() : maximum.Value.ToOADate();
      axis.ActualRange = new DoubleRange(start, end);
    }
    if (axis.Area == null)
      return;
    axis.Area.ScheduleUpdate();
  }

  private static DateTime AlignRangeStart(
    DateTime start,
    double intervalValue,
    DateTimeIntervalType type)
  {
    if (type == DateTimeIntervalType.Auto)
      return start;
    DateTime dateTime = start;
    switch (type - 1)
    {
      case DateTimeIntervalType.Auto:
        int millisecond = (int) ((double) (int) ((double) dateTime.Millisecond / intervalValue) * intervalValue);
        dateTime = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, dateTime.Minute, dateTime.Second, millisecond);
        break;
      case DateTimeIntervalType.Milliseconds:
        int second = (int) ((double) (int) ((double) dateTime.Second / intervalValue) * intervalValue);
        dateTime = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, dateTime.Minute, second, 0);
        break;
      case DateTimeIntervalType.Seconds:
        int minute = (int) ((double) (int) ((double) dateTime.Minute / intervalValue) * intervalValue);
        dateTime = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, minute, 0);
        break;
      case DateTimeIntervalType.Minutes:
        int hour = (int) ((double) (int) ((double) dateTime.Hour / intervalValue) * intervalValue);
        dateTime = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, hour, 0, 0);
        break;
      case DateTimeIntervalType.Hours:
        int day = (int) ((double) (int) ((double) dateTime.Day / intervalValue) * intervalValue);
        if (day <= 0)
          day = 1;
        dateTime = new DateTime(dateTime.Year, dateTime.Month, day, 0, 0, 0);
        break;
      case DateTimeIntervalType.Days:
        int month = (int) ((double) (int) ((double) dateTime.Month / intervalValue) * intervalValue);
        if (month <= 0)
          month = 1;
        dateTime = new DateTime(dateTime.Year, month, 1, 0, 0, 0);
        break;
      case DateTimeIntervalType.Months:
        int year = (int) ((double) (int) ((double) dateTime.Year / intervalValue) * intervalValue);
        if (year <= 0)
          year = 1;
        dateTime = new DateTime(year, 1, 1, 0, 0, 0);
        break;
    }
    return dateTime;
  }

  private static DateTime IncreaseNonWorkingInterval(
    string days,
    DateTimeAxis axis,
    DateTime date,
    double interval,
    DateTimeIntervalType intervalType)
  {
    DateTime date1 = DateTimeAxisHelper.IncreaseInterval(date, interval, intervalType);
    if (axis != null && axis.EnableBusinessHours && !days.Contains(date1.DayOfWeek.ToString()))
      date1 = date1.ValidateNonWorkingDate(days, false, axis.NonWorkingDays.Count);
    return date1;
  }

  private static DateTime NonWorkingDaysIntervals(DateTimeAxis axis, DateTime date)
  {
    string internalWorkingDays = axis.InternalWorkingDays;
    if (axis != null)
    {
      date = date.ValidateNonWorkingDate(internalWorkingDays, false, axis.NonWorkingDays.Count);
      date = date.ValidateNonWorkingHours(axis.OpenTime, axis.CloseTime, false);
      date = date.ValidateNonWorkingDate(internalWorkingDays, false, axis.NonWorkingDays.Count);
    }
    return date;
  }
}
