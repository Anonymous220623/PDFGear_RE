// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.DateTimeCategoryAxis
// Assembly: Syncfusion.SfChart.WPF, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 279D70C6-7936-4F3E-B6CF-79EFEF734826
// Assembly location: C:\Program Files\PDFgear\Syncfusion.SfChart.WPF.dll

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;

#nullable disable
namespace Syncfusion.UI.Xaml.Charts;

public class DateTimeCategoryAxis : ChartAxisBase2D
{
  public static readonly DependencyProperty IntervalProperty = DependencyProperty.Register(nameof (Interval), typeof (double), typeof (DateTimeCategoryAxis), new PropertyMetadata((object) 0.0, new PropertyChangedCallback(DateTimeCategoryAxis.OnIntervalChanged)));
  public static readonly DependencyProperty IntervalTypeProperty = DependencyProperty.Register(nameof (IntervalType), typeof (DateTimeIntervalType), typeof (DateTimeCategoryAxis), new PropertyMetadata((object) DateTimeIntervalType.Auto, new PropertyChangedCallback(DateTimeCategoryAxis.OnIntervalTypeChanged)));

  public double Interval
  {
    get => (double) this.GetValue(DateTimeCategoryAxis.IntervalProperty);
    set => this.SetValue(DateTimeCategoryAxis.IntervalProperty, (object) value);
  }

  public DateTimeIntervalType IntervalType
  {
    get => (DateTimeIntervalType) this.GetValue(DateTimeCategoryAxis.IntervalTypeProperty);
    set => this.SetValue(DateTimeCategoryAxis.IntervalTypeProperty, (object) value);
  }

  internal DateTimeIntervalType ActualIntervalType { get; set; }

  public override object GetLabelContent(double position)
  {
    ChartSeriesBase actualSeries = this.Area.VisibleSeries.Where<ChartSeriesBase>((Func<ChartSeriesBase, bool>) (series => series.ActualXAxis == this)).Max((Func<ChartSeriesBase, double>) (filteredSeries => (double) filteredSeries.DataCount));
    if (actualSeries == null)
      return (object) position;
    return this.CustomLabels.Count > 0 || this.GetLabelSource() != null ? this.GetCustomLabelContent(position) ?? this.GetLabelContent((int) Math.Round(position), actualSeries) : this.GetLabelContent((int) Math.Round(position), actualSeries);
  }

  protected internal override double CalculateActualInterval(DoubleRange range, Size availableSize)
  {
    return this.Interval == 0.0 || double.IsNaN(this.Interval) ? this.CalculateNiceInterval(range, availableSize) : this.Interval;
  }

  protected internal override double CalculateNiceInterval(
    DoubleRange actualRange,
    Size availableSize)
  {
    DateTime dateTime = actualRange.Start.FromOADate();
    TimeSpan timeSpan = actualRange.End.FromOADate().Subtract(dateTime);
    double val2 = 0.0;
    switch (this.IntervalType)
    {
      case DateTimeIntervalType.Auto:
        val2 = actualRange.Delta / this.GetActualDesiredIntervalsCount(availableSize);
        if (val2 <= base.CalculateNiceInterval(new DoubleRange(0.0, timeSpan.TotalDays / 365.0), availableSize))
        {
          val2 = base.CalculateNiceInterval(new DoubleRange(0.0, timeSpan.TotalDays / 365.0), availableSize);
          this.ActualIntervalType = DateTimeIntervalType.Years;
          break;
        }
        if (val2 <= base.CalculateNiceInterval(new DoubleRange(0.0, timeSpan.TotalDays / 30.0), availableSize))
        {
          val2 = base.CalculateNiceInterval(new DoubleRange(0.0, timeSpan.TotalDays / 30.0), availableSize);
          this.ActualIntervalType = DateTimeIntervalType.Months;
          break;
        }
        if (val2 <= base.CalculateNiceInterval(new DoubleRange(0.0, timeSpan.TotalDays), availableSize))
        {
          val2 = base.CalculateNiceInterval(new DoubleRange(0.0, timeSpan.TotalDays), availableSize);
          this.ActualIntervalType = DateTimeIntervalType.Days;
          break;
        }
        if (val2 <= base.CalculateNiceInterval(new DoubleRange(0.0, timeSpan.TotalHours), availableSize))
        {
          val2 = base.CalculateNiceInterval(new DoubleRange(0.0, timeSpan.TotalHours), availableSize);
          this.ActualIntervalType = DateTimeIntervalType.Hours;
          break;
        }
        if (val2 <= base.CalculateNiceInterval(new DoubleRange(0.0, timeSpan.TotalMinutes), availableSize))
        {
          val2 = base.CalculateNiceInterval(new DoubleRange(0.0, timeSpan.TotalMinutes), availableSize);
          this.ActualIntervalType = DateTimeIntervalType.Minutes;
          break;
        }
        if (val2 <= base.CalculateNiceInterval(new DoubleRange(0.0, timeSpan.TotalSeconds), availableSize))
        {
          val2 = base.CalculateNiceInterval(new DoubleRange(0.0, timeSpan.TotalSeconds), availableSize);
          this.ActualIntervalType = DateTimeIntervalType.Seconds;
          break;
        }
        if (val2 <= base.CalculateNiceInterval(new DoubleRange(0.0, timeSpan.TotalMilliseconds), availableSize))
        {
          val2 = base.CalculateNiceInterval(new DoubleRange(0.0, timeSpan.TotalMilliseconds), availableSize);
          this.ActualIntervalType = DateTimeIntervalType.Milliseconds;
          break;
        }
        break;
      case DateTimeIntervalType.Milliseconds:
        val2 = base.CalculateNiceInterval(new DoubleRange(0.0, timeSpan.TotalMilliseconds), availableSize);
        break;
      case DateTimeIntervalType.Seconds:
        val2 = base.CalculateNiceInterval(new DoubleRange(0.0, timeSpan.TotalSeconds), availableSize);
        break;
      case DateTimeIntervalType.Minutes:
        val2 = base.CalculateNiceInterval(new DoubleRange(0.0, timeSpan.TotalMinutes), availableSize);
        break;
      case DateTimeIntervalType.Hours:
        val2 = base.CalculateNiceInterval(new DoubleRange(0.0, timeSpan.TotalHours), availableSize);
        break;
      case DateTimeIntervalType.Days:
        val2 = base.CalculateNiceInterval(new DoubleRange(0.0, timeSpan.TotalDays), availableSize);
        break;
      case DateTimeIntervalType.Months:
        val2 = base.CalculateNiceInterval(new DoubleRange(0.0, timeSpan.TotalDays / 30.0), availableSize);
        break;
      case DateTimeIntervalType.Years:
        val2 = base.CalculateNiceInterval(new DoubleRange(0.0, timeSpan.TotalDays / 365.0), availableSize);
        break;
    }
    return Math.Max(1.0, val2);
  }

  protected virtual void OnIntervalChanged(DependencyPropertyChangedEventArgs e)
  {
    if (this.Area == null)
      return;
    this.Area.ScheduleUpdate();
  }

  protected override DependencyObject CloneAxis(DependencyObject obj)
  {
    obj = (DependencyObject) new DateTimeCategoryAxis()
    {
      Interval = this.Interval,
      IntervalType = this.IntervalType
    };
    return base.CloneAxis(obj);
  }

  protected override void GenerateVisibleLabels()
  {
    ChartSeriesBase actualSeries = this.Area.VisibleSeries.Where<ChartSeriesBase>((Func<ChartSeriesBase, bool>) (series => series.ActualXAxis == this)).Max((Func<ChartSeriesBase, double>) (filteredSeries => (double) filteredSeries.DataCount)) ?? (this.Area is SfChart ? ((IEnumerable<ChartSeriesBase>) (this.Area as SfChart).TechnicalIndicators.Where<ChartSeries>((Func<ChartSeries, bool>) (series => series.ActualXAxis == this))).Max((Func<ChartSeriesBase, double>) (filteredSeries => (double) filteredSeries.DataCount)) : (ChartSeriesBase) null);
    this.SetRangeForAxisStyle();
    if (actualSeries == null || !(actualSeries.ActualXValues is List<double> actualXvalues1) || actualXvalues1.Count <= 0)
      return;
    double months = this.Interval != 0.0 ? this.Interval : this.ActualInterval;
    double a = this.VisibleRange.Start - this.VisibleRange.Start % this.ActualInterval;
    List<double> actualXvalues2 = actualSeries.ActualXValues as List<double>;
    DateTime dateTime1 = actualXvalues2[0].FromOADate();
    double distinctDate = 0.0;
    int year = dateTime1.Year;
    int month = dateTime1.Month;
    int hour = dateTime1.Hour;
    int minute = dateTime1.Minute;
    int second = dateTime1.Second;
    int millisecond = dateTime1.Millisecond;
    DateTime date = dateTime1.Date;
    switch (this.ActualIntervalType)
    {
      case DateTimeIntervalType.Auto:
        for (; a <= this.VisibleRange.End; a += months)
        {
          if (this.VisibleRange.Inside(a) && a < (double) actualSeries.DataCount && a > -1.0)
          {
            int num = (int) Math.Round(a);
            DateTime currentDate = actualXvalues2[num].FromOADate();
            object labelContent = this.GetLabelContent(num, actualSeries);
            this.VisibleLabels.Add((ChartAxisLabel) new DateTimeAxisLabel((double) num, labelContent, actualXvalues2[num])
            {
              IntervalType = this.ActualIntervalType,
              IsTransition = DateTimeAxisHelper.GetTransitionState(ref distinctDate, currentDate, this.ActualIntervalType)
            });
          }
        }
        break;
      case DateTimeIntervalType.Milliseconds:
        for (; a <= this.VisibleRange.End; ++a)
        {
          int num = (int) Math.Round(a);
          if (this.VisibleRange.Inside(a) && num > -1 && num < actualXvalues2.Count)
          {
            DateTime currentDate = actualXvalues2[num].FromOADate();
            if (currentDate.Date > date)
            {
              date = currentDate.Date;
              hour = currentDate.Hour;
            }
            if (currentDate.Date == date)
            {
              if (currentDate.Hour > hour)
              {
                hour = currentDate.Hour;
                minute = currentDate.Minute;
              }
              if (currentDate.Hour == hour)
              {
                if (currentDate.Minute > minute)
                {
                  minute = currentDate.Minute;
                  second = currentDate.Second;
                }
                if (currentDate.Minute == minute)
                {
                  if (currentDate.Second > second)
                  {
                    second = currentDate.Second;
                    millisecond = currentDate.Millisecond;
                  }
                  if (currentDate.Second == second)
                  {
                    if (currentDate.Millisecond > millisecond)
                      millisecond = currentDate.Millisecond;
                    if (currentDate.Millisecond == millisecond)
                    {
                      object labelContent = this.GetLabelContent(num, actualSeries);
                      this.VisibleLabels.Add((ChartAxisLabel) new DateTimeAxisLabel((double) num, labelContent, actualXvalues2[num])
                      {
                        IntervalType = this.ActualIntervalType,
                        IsTransition = DateTimeAxisHelper.GetTransitionState(ref distinctDate, currentDate, this.ActualIntervalType)
                      });
                      DateTime dateTime2 = currentDate.AddMilliseconds((double) (int) months);
                      hour = dateTime2.Hour;
                      dateTime2 = currentDate.AddMilliseconds((double) (int) months);
                      minute = dateTime2.Minute;
                      dateTime2 = currentDate.AddMilliseconds((double) (int) months);
                      date = dateTime2.Date;
                      dateTime2 = currentDate.AddMilliseconds((double) (int) months);
                      second = dateTime2.Second;
                      dateTime2 = currentDate.AddMilliseconds((double) (int) months);
                      millisecond = dateTime2.Millisecond;
                    }
                  }
                }
              }
            }
          }
        }
        break;
      case DateTimeIntervalType.Seconds:
        for (; a <= this.VisibleRange.End; ++a)
        {
          int num = (int) Math.Round(a);
          if (this.VisibleRange.Inside(a) && num > -1 && num < actualXvalues2.Count)
          {
            DateTime currentDate = actualXvalues2[num].FromOADate();
            if (currentDate.Date > date)
            {
              date = currentDate.Date;
              hour = currentDate.Hour;
            }
            if (currentDate.Date == date)
            {
              if (currentDate.Hour > hour)
              {
                hour = currentDate.Hour;
                minute = currentDate.Minute;
              }
              if (currentDate.Hour == hour)
              {
                if (currentDate.Minute > minute)
                {
                  minute = currentDate.Minute;
                  second = currentDate.Second;
                }
                if (currentDate.Minute == minute)
                {
                  if (currentDate.Second > second)
                    second = currentDate.Second;
                  if (currentDate.Second == second)
                  {
                    object labelContent = this.GetLabelContent(num, actualSeries);
                    this.VisibleLabels.Add((ChartAxisLabel) new DateTimeAxisLabel((double) num, labelContent, actualXvalues2[num])
                    {
                      IntervalType = this.ActualIntervalType,
                      IsTransition = DateTimeAxisHelper.GetTransitionState(ref distinctDate, currentDate, this.ActualIntervalType)
                    });
                    DateTime dateTime3 = currentDate.AddSeconds((double) (int) months);
                    hour = dateTime3.Hour;
                    dateTime3 = currentDate.AddSeconds((double) (int) months);
                    minute = dateTime3.Minute;
                    dateTime3 = currentDate.AddSeconds((double) (int) months);
                    date = dateTime3.Date;
                    dateTime3 = currentDate.AddSeconds((double) (int) months);
                    second = dateTime3.Second;
                  }
                }
              }
            }
          }
        }
        break;
      case DateTimeIntervalType.Minutes:
        for (; a <= this.VisibleRange.End; ++a)
        {
          int num = (int) Math.Round(a);
          if (this.VisibleRange.Inside(a) && num > -1 && num < actualXvalues2.Count)
          {
            DateTime currentDate = actualXvalues2[num].FromOADate();
            if (currentDate.Date > date)
            {
              date = currentDate.Date;
              hour = currentDate.Hour;
            }
            if (currentDate.Date == date)
            {
              if (currentDate.Hour > hour)
              {
                hour = currentDate.Hour;
                minute = currentDate.Minute;
              }
              if (currentDate.Hour == hour)
              {
                if (currentDate.Minute > minute)
                  minute = currentDate.Minute;
                if (currentDate.Minute == minute)
                {
                  object labelContent = this.GetLabelContent(num, actualSeries);
                  this.VisibleLabels.Add((ChartAxisLabel) new DateTimeAxisLabel((double) num, labelContent, actualXvalues2[num])
                  {
                    IntervalType = this.ActualIntervalType,
                    IsTransition = DateTimeAxisHelper.GetTransitionState(ref distinctDate, currentDate, this.ActualIntervalType)
                  });
                  DateTime dateTime4 = currentDate.AddMinutes((double) (int) months);
                  hour = dateTime4.Hour;
                  dateTime4 = currentDate.AddMinutes((double) (int) months);
                  minute = dateTime4.Minute;
                  dateTime4 = currentDate.AddMinutes((double) (int) months);
                  date = dateTime4.Date;
                }
              }
            }
          }
        }
        break;
      case DateTimeIntervalType.Hours:
        for (; a <= this.VisibleRange.End; ++a)
        {
          int num = (int) Math.Round(a);
          if (this.VisibleRange.Inside(a) && num > -1 && num < actualXvalues2.Count)
          {
            DateTime currentDate = actualXvalues2[num].FromOADate();
            if (currentDate.Date > date)
            {
              date = currentDate.Date;
              hour = currentDate.Hour;
            }
            if (currentDate.Date == date)
            {
              if (currentDate.Hour > hour)
                hour = currentDate.Hour;
              if (currentDate.Hour == hour)
              {
                object labelContent = this.GetLabelContent(num, actualSeries);
                this.VisibleLabels.Add((ChartAxisLabel) new DateTimeAxisLabel((double) num, labelContent, actualXvalues2[num])
                {
                  IntervalType = this.ActualIntervalType,
                  IsTransition = DateTimeAxisHelper.GetTransitionState(ref distinctDate, currentDate, this.ActualIntervalType)
                });
                hour = currentDate.AddHours((double) (int) months).Hour;
                date = currentDate.AddHours((double) (int) months).Date;
              }
            }
          }
        }
        break;
      case DateTimeIntervalType.Days:
        for (; a <= this.VisibleRange.End; ++a)
        {
          int num = (int) Math.Round(a);
          if (this.VisibleRange.Inside(a) && num > -1 && num < actualXvalues2.Count)
          {
            DateTime currentDate = actualXvalues2[num].FromOADate();
            if (currentDate.Date > date)
              date = currentDate.Date;
            if (currentDate.Date == date)
            {
              object labelContent = this.GetLabelContent(num, actualSeries);
              this.VisibleLabels.Add((ChartAxisLabel) new DateTimeAxisLabel((double) num, labelContent, actualXvalues2[num])
              {
                IntervalType = this.ActualIntervalType,
                IsTransition = DateTimeAxisHelper.GetTransitionState(ref distinctDate, currentDate, this.ActualIntervalType)
              });
              date = currentDate.AddDays((double) (int) months).Date;
            }
          }
        }
        break;
      case DateTimeIntervalType.Months:
        for (; a <= this.VisibleRange.End; ++a)
        {
          int num = (int) Math.Round(a);
          if (this.VisibleRange.Inside(a) && num > -1 && num < actualXvalues2.Count)
          {
            DateTime currentDate = actualXvalues2[num].FromOADate();
            if (currentDate.Year > year)
            {
              year = currentDate.Year;
              month = currentDate.Month;
            }
            if (currentDate.Year == year)
            {
              if (currentDate.Month > month)
                month = currentDate.Month;
              if (currentDate.Month == month)
              {
                object labelContent = this.GetLabelContent(num, actualSeries);
                this.VisibleLabels.Add((ChartAxisLabel) new DateTimeAxisLabel((double) num, labelContent, actualXvalues2[num])
                {
                  IntervalType = this.ActualIntervalType,
                  IsTransition = DateTimeAxisHelper.GetTransitionState(ref distinctDate, currentDate, this.ActualIntervalType)
                });
                month = currentDate.AddMonths((int) months).Month;
                year = currentDate.AddMonths((int) months).Year;
              }
            }
          }
        }
        break;
      case DateTimeIntervalType.Years:
        for (; a <= this.VisibleRange.End; ++a)
        {
          int num = (int) Math.Round(a);
          if (this.VisibleRange.Inside(a) && num > -1 && num < actualXvalues2.Count)
          {
            DateTime currentDate = actualXvalues2[num].FromOADate();
            if (currentDate.Year > year)
              year = currentDate.Year;
            if (currentDate.Year == year)
            {
              object labelContent = this.GetLabelContent(num, actualSeries);
              this.VisibleLabels.Add((ChartAxisLabel) new DateTimeAxisLabel((double) num, labelContent, actualXvalues2[num])
              {
                IntervalType = this.ActualIntervalType,
                IsTransition = DateTimeAxisHelper.GetTransitionState(ref distinctDate, currentDate, this.ActualIntervalType)
              });
              year = currentDate.AddYears((int) months).Year;
            }
          }
        }
        break;
    }
  }

  private static void OnIntervalChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    (d as DateTimeCategoryAxis).OnIntervalChanged(e);
  }

  private static void OnIntervalTypeChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    if (!(d is DateTimeCategoryAxis timeCategoryAxis) || e.NewValue == null || e.NewValue == e.OldValue)
      return;
    timeCategoryAxis.ActualIntervalType = timeCategoryAxis.IntervalType;
    timeCategoryAxis.OnIntervalChanged(e);
  }

  private object GetLabelContent(int pos, ChartSeriesBase actualSeries)
  {
    if (actualSeries != null)
    {
      if (actualSeries.ActualXValues is List<double> actualXvalues1 && pos < actualXvalues1.Count && pos >= 0)
        return (object) actualXvalues1[pos].FromOADate().ToString(this.LabelFormat, (IFormatProvider) CultureInfo.CurrentCulture);
      if (actualSeries.ActualXValues is List<string> actualXvalues2 && pos < actualXvalues2.Count && pos >= 0)
        return (object) actualXvalues2[pos];
    }
    return (object) pos;
  }
}
