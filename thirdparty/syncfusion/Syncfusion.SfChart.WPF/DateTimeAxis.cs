// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.DateTimeAxis
// Assembly: Syncfusion.SfChart.WPF, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 279D70C6-7936-4F3E-B6CF-79EFEF734826
// Assembly location: C:\Program Files\PDFgear\Syncfusion.SfChart.WPF.dll

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;

#nullable disable
namespace Syncfusion.UI.Xaml.Charts;

public class DateTimeAxis : RangeAxisBase
{
  public static readonly DependencyProperty AutoScrollingDeltaTypeProperty = DependencyProperty.Register(nameof (AutoScrollingDeltaType), typeof (DateTimeIntervalType), typeof (DateTimeAxis), new PropertyMetadata((object) DateTimeIntervalType.Auto, new PropertyChangedCallback(DateTimeAxis.OnAutoScrollingDeltaTypeChanged)));
  public static readonly DependencyProperty EnableBusinessHoursProperty = DependencyProperty.Register(nameof (EnableBusinessHours), typeof (bool), typeof (DateTimeAxis), new PropertyMetadata((object) false, new PropertyChangedCallback(DateTimeAxis.OnEnableBusinessHoursChanged)));
  public static readonly DependencyProperty OpenTimeProperty = DependencyProperty.Register(nameof (OpenTime), typeof (double), typeof (DateTimeAxis), new PropertyMetadata((object) 0.0));
  public static readonly DependencyProperty CloseTimeProperty = DependencyProperty.Register(nameof (CloseTime), typeof (double), typeof (DateTimeAxis), new PropertyMetadata((object) 24.0));
  public static readonly DependencyProperty WorkingDaysProperty = DependencyProperty.Register(nameof (WorkingDays), typeof (Day), typeof (DateTimeAxis), new PropertyMetadata((object) (Day.Monday | Day.Tuesday | Day.Wednesday | Day.Thursday | Day.Friday), new PropertyChangedCallback(DateTimeAxis.OnWorkDaysChanged)));
  public static readonly DependencyProperty MinimumProperty = DependencyProperty.Register(nameof (Minimum), typeof (DateTime?), typeof (DateTimeAxis), new PropertyMetadata((object) null, new PropertyChangedCallback(DateTimeAxis.OnMinimumChanged)));
  public static readonly DependencyProperty MaximumProperty = DependencyProperty.Register(nameof (Maximum), typeof (DateTime?), typeof (DateTimeAxis), new PropertyMetadata((object) null, new PropertyChangedCallback(DateTimeAxis.OnMaximumChanged)));
  public static readonly DependencyProperty IntervalProperty = DependencyProperty.Register(nameof (Interval), typeof (double), typeof (DateTimeAxis), new PropertyMetadata((object) 0.0, new PropertyChangedCallback(DateTimeAxis.OnIntervalChanged)));
  public static readonly DependencyProperty RangePaddingProperty = DependencyProperty.Register(nameof (RangePadding), typeof (DateTimeRangePadding), typeof (DateTimeAxis), new PropertyMetadata((object) DateTimeRangePadding.Auto, new PropertyChangedCallback(DateTimeAxis.OnRangePaddingChanged)));
  public static readonly DependencyProperty IntervalTypeProperty = DependencyProperty.Register(nameof (IntervalType), typeof (DateTimeIntervalType), typeof (DateTimeAxis), new PropertyMetadata((object) DateTimeIntervalType.Auto, new PropertyChangedCallback(DateTimeAxis.OnIntervalTypeChanged)));
  internal double TotalNonWorkingHours;
  internal List<string> NonWorkingDays;
  private DateTimeIntervalType dateTimeIntervalType;
  private double nonWorkingHoursPerDay;

  public DateTimeIntervalType AutoScrollingDeltaType
  {
    get => (DateTimeIntervalType) this.GetValue(DateTimeAxis.AutoScrollingDeltaTypeProperty);
    set => this.SetValue(DateTimeAxis.AutoScrollingDeltaTypeProperty, (object) value);
  }

  public bool EnableBusinessHours
  {
    get => (bool) this.GetValue(DateTimeAxis.EnableBusinessHoursProperty);
    set => this.SetValue(DateTimeAxis.EnableBusinessHoursProperty, (object) value);
  }

  public double OpenTime
  {
    get => (double) this.GetValue(DateTimeAxis.OpenTimeProperty);
    set => this.SetValue(DateTimeAxis.OpenTimeProperty, (object) value);
  }

  public double CloseTime
  {
    get => (double) this.GetValue(DateTimeAxis.CloseTimeProperty);
    set => this.SetValue(DateTimeAxis.CloseTimeProperty, (object) value);
  }

  public Day WorkingDays
  {
    get => (Day) this.GetValue(DateTimeAxis.WorkingDaysProperty);
    set => this.SetValue(DateTimeAxis.WorkingDaysProperty, (object) value);
  }

  public DateTime? Minimum
  {
    get => (DateTime?) this.GetValue(DateTimeAxis.MinimumProperty);
    set => this.SetValue(DateTimeAxis.MinimumProperty, (object) value);
  }

  public DateTime? Maximum
  {
    get => (DateTime?) this.GetValue(DateTimeAxis.MaximumProperty);
    set => this.SetValue(DateTimeAxis.MaximumProperty, (object) value);
  }

  public double Interval
  {
    get => (double) this.GetValue(DateTimeAxis.IntervalProperty);
    set => this.SetValue(DateTimeAxis.IntervalProperty, (object) value);
  }

  public DateTimeRangePadding RangePadding
  {
    get => (DateTimeRangePadding) this.GetValue(DateTimeAxis.RangePaddingProperty);
    set => this.SetValue(DateTimeAxis.RangePaddingProperty, (object) value);
  }

  public DateTimeIntervalType IntervalType
  {
    get => (DateTimeIntervalType) this.GetValue(DateTimeAxis.IntervalTypeProperty);
    set => this.SetValue(DateTimeAxis.IntervalTypeProperty, (object) value);
  }

  internal string InternalWorkingDays { get; set; }

  internal DateTimeIntervalType ActualIntervalType { get; set; }

  public double CalcNonWorkingHours(
    DateTime startDate,
    DateTime endDate,
    string workingDays,
    double nonWorkingHoursPerDay)
  {
    DateTime dateTime1 = DateTime.MinValue;
    DateTime dateTime2 = DateTime.MinValue;
    double num1 = 0.0;
    int num2 = 0;
    int num3 = 0;
    if (startDate == endDate)
      return 0.0;
    for (DateTime dateTime3 = startDate; dateTime3 <= endDate.AddHours(new TimeSpan(23, 59, 59).TotalHours - (double) endDate.Hour); dateTime3 = dateTime3.AddDays(1.0))
    {
      if (dateTime3.DayOfWeek != DayOfWeek.Saturday)
      {
        if (!workingDays.Contains(dateTime3.DayOfWeek.ToString()) && dateTime3 != startDate)
          ++num3;
        else if (dateTime3 != startDate)
          ++num2;
      }
      else if (dateTime3 != startDate)
      {
        dateTime1 = dateTime3;
        break;
      }
    }
    for (DateTime dateTime4 = endDate; dateTime4 >= startDate.AddHours((double) -startDate.Hour); dateTime4 = dateTime4.AddDays(-1.0))
    {
      if (dateTime4.DayOfWeek != DayOfWeek.Friday)
      {
        if (!workingDays.Contains(dateTime4.DayOfWeek.ToString()) && (int) dateTime4.ToOADate() != (int) startDate.ToOADate() && dateTime4 != endDate && dateTime1 != DateTime.MinValue)
          ++num3;
        else if ((int) dateTime4.ToOADate() != (int) startDate.ToOADate() && dateTime1 != DateTime.MinValue)
          ++num2;
      }
      else
      {
        dateTime2 = dateTime4;
        break;
      }
    }
    if (dateTime2 != DateTime.MinValue && dateTime1 != DateTime.MinValue)
      num1 = Math.Round((double.IsNegativeInfinity(dateTime2.ToOADate() - dateTime1.ToOADate()) ? 0.0 : dateTime2.ToOADate() - dateTime1.ToOADate()) / 7.0);
    return num1 * (double) this.NonWorkingDays.Count * 24.0 + (double) (num3 * 24) + num1 * (double) (7 - this.NonWorkingDays.Count) * nonWorkingHoursPerDay + (double) num2 * nonWorkingHoursPerDay;
  }

  public override object GetLabelContent(double position)
  {
    return this.CustomLabels.Count > 0 || this.GetLabelSource() != null ? this.GetCustomLabelContent(position) ?? this.GetActualLabelContent(position) : this.GetActualLabelContent(position);
  }

  internal override void UpdateAutoScrollingDelta(double autoScrollingDelta)
  {
    DateTime dateTime = this.ActualRange.End.FromOADate();
    if (double.IsNaN(this.AutoScrollingDelta))
      return;
    double autoScrollingDelta1 = this.AutoScrollingDelta;
    switch (this.GetActualAutoScrollingDeltaType())
    {
      case DateTimeIntervalType.Milliseconds:
        autoScrollingDelta = TimeSpan.FromMilliseconds(autoScrollingDelta1).TotalDays;
        break;
      case DateTimeIntervalType.Seconds:
        autoScrollingDelta = TimeSpan.FromSeconds(autoScrollingDelta1).TotalDays;
        break;
      case DateTimeIntervalType.Minutes:
        autoScrollingDelta = TimeSpan.FromMinutes(autoScrollingDelta1).TotalDays;
        break;
      case DateTimeIntervalType.Hours:
        autoScrollingDelta = TimeSpan.FromHours(autoScrollingDelta1).TotalDays;
        break;
      case DateTimeIntervalType.Days:
        autoScrollingDelta = autoScrollingDelta1;
        break;
      case DateTimeIntervalType.Months:
        autoScrollingDelta = this.ActualRange.End - dateTime.AddMonths((int) -this.AutoScrollingDelta).ToOADate();
        break;
      case DateTimeIntervalType.Years:
        autoScrollingDelta = this.ActualRange.End - dateTime.AddYears((int) -this.AutoScrollingDelta).ToOADate();
        break;
    }
    base.UpdateAutoScrollingDelta(autoScrollingDelta);
  }

  internal override object GetActualLabelContent(double position)
  {
    return (object) position.FromOADate().ToString(this.LabelFormat, (IFormatProvider) CultureInfo.CurrentCulture);
  }

  protected internal override double CalculateNiceInterval(
    DoubleRange actualRange,
    Size availableSize)
  {
    DateTime dateTime = actualRange.Start.FromOADate();
    TimeSpan timeSpan = actualRange.End.FromOADate().Subtract(dateTime);
    double niceInterval1 = 0.0;
    switch (this.IntervalType)
    {
      case DateTimeIntervalType.Auto:
        double niceInterval2 = base.CalculateNiceInterval(new DoubleRange(0.0, timeSpan.TotalDays / 365.0), availableSize);
        if (niceInterval2 >= 1.0)
        {
          this.ActualIntervalType = DateTimeIntervalType.Years;
          return niceInterval2;
        }
        double niceInterval3 = base.CalculateNiceInterval(new DoubleRange(0.0, timeSpan.TotalDays / 30.0), availableSize);
        if (niceInterval3 >= 1.0)
        {
          this.ActualIntervalType = DateTimeIntervalType.Months;
          return niceInterval3;
        }
        double niceInterval4 = base.CalculateNiceInterval(new DoubleRange(0.0, timeSpan.TotalDays), availableSize);
        if (niceInterval4 >= 1.0)
        {
          this.ActualIntervalType = DateTimeIntervalType.Days;
          return niceInterval4;
        }
        double niceInterval5 = base.CalculateNiceInterval(new DoubleRange(0.0, timeSpan.TotalHours), availableSize);
        if (niceInterval5 >= 1.0)
        {
          this.ActualIntervalType = DateTimeIntervalType.Hours;
          return niceInterval5;
        }
        double niceInterval6 = base.CalculateNiceInterval(new DoubleRange(0.0, timeSpan.TotalMinutes), availableSize);
        if (niceInterval6 >= 1.0)
        {
          this.ActualIntervalType = DateTimeIntervalType.Minutes;
          return niceInterval6;
        }
        double niceInterval7 = base.CalculateNiceInterval(new DoubleRange(0.0, timeSpan.TotalSeconds), availableSize);
        if (niceInterval7 >= 1.0)
        {
          this.ActualIntervalType = DateTimeIntervalType.Seconds;
          return niceInterval7;
        }
        niceInterval1 = base.CalculateNiceInterval(new DoubleRange(0.0, timeSpan.TotalMilliseconds), availableSize);
        this.ActualIntervalType = DateTimeIntervalType.Milliseconds;
        break;
      case DateTimeIntervalType.Milliseconds:
        niceInterval1 = base.CalculateNiceInterval(new DoubleRange(0.0, timeSpan.TotalMilliseconds), availableSize);
        break;
      case DateTimeIntervalType.Seconds:
        niceInterval1 = base.CalculateNiceInterval(new DoubleRange(0.0, timeSpan.TotalSeconds), availableSize);
        break;
      case DateTimeIntervalType.Minutes:
        niceInterval1 = base.CalculateNiceInterval(new DoubleRange(0.0, timeSpan.TotalMinutes), availableSize);
        break;
      case DateTimeIntervalType.Hours:
        niceInterval1 = base.CalculateNiceInterval(new DoubleRange(0.0, timeSpan.TotalHours), availableSize);
        break;
      case DateTimeIntervalType.Days:
        niceInterval1 = base.CalculateNiceInterval(new DoubleRange(0.0, timeSpan.TotalDays), availableSize);
        break;
      case DateTimeIntervalType.Months:
        niceInterval1 = base.CalculateNiceInterval(new DoubleRange(0.0, timeSpan.TotalDays / 30.0), availableSize);
        break;
      case DateTimeIntervalType.Years:
        niceInterval1 = base.CalculateNiceInterval(new DoubleRange(0.0, timeSpan.TotalDays / 365.0), availableSize);
        break;
    }
    return niceInterval1;
  }

  protected internal override double CalculateActualInterval(DoubleRange range, Size availableSize)
  {
    if (this.EnableBusinessHours)
    {
      this.CalculateNonWorkingDays(range);
      range = new DoubleRange(range.Start, range.End - this.nonWorkingHoursPerDay / 24.0);
    }
    if (this.Interval == 0.0 || double.IsNaN(this.Interval))
      return this.CalculateNiceInterval(range, availableSize);
    if (this.IntervalType == DateTimeIntervalType.Auto)
      this.CalculateNiceInterval(range, availableSize);
    return this.Interval;
  }

  protected internal override void CalculateVisibleRange(Size availableSize)
  {
    base.CalculateVisibleRange(availableSize);
    if (this.EnableBusinessHours)
    {
      DateTime dateTime1 = this.ActualRange.Start.FromOADate();
      DateTime endDate = this.ActualRange.End.FromOADate();
      while (this.NonWorkingDays.Contains(dateTime1.DayOfWeek.ToString()))
        dateTime1 = dateTime1.AddDays(-1.0).AddHours(this.CloseTime == 24.0 ? 0.0 : this.CloseTime);
      while (this.NonWorkingDays.Contains(endDate.DayOfWeek.ToString()))
        endDate = endDate.AddDays(-1.0).AddHours(this.CloseTime == 24.0 ? 0.0 : this.CloseTime);
      if (dateTime1.TimeOfDay.TotalHours < this.OpenTime)
        dateTime1 = dateTime1.AddHours(this.OpenTime - dateTime1.TimeOfDay.TotalHours);
      else if (dateTime1.TimeOfDay.TotalHours > this.CloseTime)
        dateTime1 = dateTime1.AddHours(-(dateTime1.TimeOfDay.TotalHours - this.CloseTime));
      if (endDate.TimeOfDay.TotalHours < this.OpenTime)
        endDate = endDate.AddHours(this.OpenTime - endDate.TimeOfDay.TotalHours);
      else if (endDate.TimeOfDay.TotalHours > this.CloseTime)
        endDate = endDate.AddHours(-(endDate.TimeOfDay.TotalHours - this.CloseTime));
      if (this.ZoomPosition > 0.0 || this.ZoomFactor < 1.0)
      {
        double num1 = this.CloseTime - this.OpenTime;
        double num2 = this.CalcNonWorkingHours(dateTime1, endDate, this.InternalWorkingDays.ToString(), this.nonWorkingHoursPerDay);
        double num3 = (this.ActualRange.Delta * 24.0 - num2) * this.ZoomPosition / num1;
        double num4 = num3 - (double) (int) num3;
        DateTime dateTime2 = dateTime1.AddDays((double) (int) num3).AddHours(num4 * num1);
        if (this.NonWorkingDays.Count > 0)
        {
          int weekEndDayCount = DateTimeAxis.CalculateWeekEndDayCount(this.NonWorkingDays[0], dateTime1, dateTime2);
          dateTime2 = dateTime2.AddDays((double) (weekEndDayCount * this.NonWorkingDays.Count));
          while (this.NonWorkingDays.Contains(dateTime2.DayOfWeek.ToString()))
            dateTime2 = dateTime2.AddDays(1.0);
        }
        double num5 = (this.ActualRange.Delta * 24.0 - num2) * this.ZoomFactor / num1;
        double num6 = num5 - (double) (int) num5;
        DateTime end = dateTime2.AddDays((double) (int) num5);
        end = num6 * num1 > this.CloseTime - end.TimeOfDay.TotalHours ? new DateTime(end.Year, end.Month, end.Day).AddDays(1.0).AddHours(this.OpenTime + (num6 * num1 - (this.CloseTime - end.TimeOfDay.TotalHours))) : end.AddHours(num6 * num1);
        if (this.NonWorkingDays.Count > 0)
        {
          int weekEndDayCount = DateTimeAxis.CalculateWeekEndDayCount(this.NonWorkingDays[0], dateTime2, end);
          end = end.AddDays((double) (weekEndDayCount * this.NonWorkingDays.Count));
          while (this.NonWorkingDays.Contains(end.DayOfWeek.ToString()))
            end = end.AddDays(1.0);
        }
        this.VisibleRange = new DoubleRange(dateTime2.ToOADate(), end.ToOADate());
      }
      else
        this.VisibleRange = new DoubleRange(dateTime1.ToOADate(), endDate.ToOADate());
    }
    DateTimeAxisHelper.CalculateVisibleRange((ChartAxisBase2D) this, availableSize, this.Interval);
    if (!this.EnableBusinessHours)
      return;
    this.CalculateNonWorkingDays(this.VisibleRange);
  }

  protected virtual void OnMaximumChanged(DependencyPropertyChangedEventArgs args)
  {
    this.OnMinMaxChanged();
  }

  protected virtual void OnMinimumChanged(DependencyPropertyChangedEventArgs args)
  {
    this.OnMinMaxChanged();
  }

  protected virtual void OnIntervalChanged(DependencyPropertyChangedEventArgs e)
  {
    if (this.Area == null)
      return;
    this.Area.ScheduleUpdate();
  }

  protected override void GenerateVisibleLabels()
  {
    this.SetRangeForAxisStyle();
    DateTimeAxisHelper.GenerateVisibleLabels((ChartAxisBase2D) this, (object) this.Minimum, (object) this.Maximum, this.ActualIntervalType);
  }

  protected override DoubleRange CalculateActualRange()
  {
    if (this.ActualRange.IsEmpty)
    {
      DoubleRange actualRange = base.CalculateActualRange();
      if (this.IncludeAnnotationRange && this.Area != null)
      {
        foreach (Annotation annotation in (Collection<Annotation>) (this.Area as SfChart).Annotations)
        {
          if (this.Orientation == Orientation.Horizontal && annotation.CoordinateUnit == CoordinateUnit.Axis && annotation.XAxis == this)
            actualRange += new DoubleRange(Annotation.ConvertData(annotation.X1, (ChartAxis) this) == 0.0 ? actualRange.Start : Annotation.ConvertData(annotation.X1, (ChartAxis) this), annotation is TextAnnotation ? (Annotation.ConvertData(annotation.X1, (ChartAxis) this) == 0.0 ? actualRange.Start : Annotation.ConvertData(annotation.X1, (ChartAxis) this)) : (annotation is ImageAnnotation ? (Annotation.ConvertData((annotation as ImageAnnotation).X2, (ChartAxis) this) == 0.0 ? actualRange.Start : Annotation.ConvertData((annotation as ImageAnnotation).X2, (ChartAxis) this)) : (Annotation.ConvertData((annotation as ShapeAnnotation).X2, (ChartAxis) this) == 0.0 ? actualRange.Start : Annotation.ConvertData((annotation as ShapeAnnotation).X2, (ChartAxis) this))));
        }
      }
      return actualRange;
    }
    if (this.Minimum.HasValue && this.Maximum.HasValue)
      return this.ActualRange;
    DoubleRange actualRange1 = base.CalculateActualRange();
    if (this.Minimum.HasValue)
      return new DoubleRange(this.ActualRange.Start, double.IsNaN(actualRange1.End) ? this.ActualRange.Start + 1.0 : actualRange1.End);
    if (this.Maximum.HasValue)
      return new DoubleRange(double.IsNaN(actualRange1.Start) ? this.ActualRange.End - 1.0 : actualRange1.Start, this.ActualRange.End);
    if (this.IncludeAnnotationRange && this.Area != null)
    {
      foreach (Annotation annotation in (Collection<Annotation>) (this.Area as SfChart).Annotations)
      {
        if (this.Orientation == Orientation.Horizontal && annotation.CoordinateUnit == CoordinateUnit.Axis && annotation.XAxis == this)
          actualRange1 += new DoubleRange(Annotation.ConvertData(annotation.X1, (ChartAxis) this) == 0.0 ? actualRange1.Start : Annotation.ConvertData(annotation.X1, (ChartAxis) this), annotation is TextAnnotation ? (Annotation.ConvertData(annotation.X1, (ChartAxis) this) == 0.0 ? actualRange1.Start : Annotation.ConvertData(annotation.X1, (ChartAxis) this)) : (annotation is ImageAnnotation ? (Annotation.ConvertData((annotation as ImageAnnotation).X2, (ChartAxis) this) == 0.0 ? actualRange1.Start : Annotation.ConvertData((annotation as ImageAnnotation).X2, (ChartAxis) this)) : (Annotation.ConvertData((annotation as ShapeAnnotation).X2, (ChartAxis) this) == 0.0 ? actualRange1.Start : Annotation.ConvertData((annotation as ShapeAnnotation).X2, (ChartAxis) this))));
      }
    }
    return actualRange1;
  }

  protected override DoubleRange ApplyRangePadding(DoubleRange range, double interval)
  {
    if (!this.Minimum.HasValue && !this.Maximum.HasValue)
      return DateTimeAxisHelper.ApplyRangePadding((ChartAxis) this, base.ApplyRangePadding(range, interval), interval, this.RangePadding, this.ActualIntervalType);
    if (this.Minimum.HasValue && this.Maximum.HasValue)
      return range;
    DoubleRange doubleRange = DateTimeAxisHelper.ApplyRangePadding((ChartAxis) this, base.ApplyRangePadding(range, interval), interval, this.RangePadding, this.ActualIntervalType);
    return !this.Minimum.HasValue ? new DoubleRange(doubleRange.Start, range.End) : new DoubleRange(range.Start, doubleRange.End);
  }

  protected override DependencyObject CloneAxis(DependencyObject obj)
  {
    obj = (DependencyObject) new DateTimeAxis()
    {
      Interval = this.Interval,
      Minimum = this.Minimum,
      Maximum = this.Maximum,
      IntervalType = this.IntervalType,
      EnableBusinessHours = this.EnableBusinessHours,
      OpenTime = this.OpenTime,
      CloseTime = this.CloseTime,
      WorkingDays = this.WorkingDays,
      RangePadding = this.RangePadding
    };
    return base.CloneAxis(obj);
  }

  private static void OnAutoScrollingDeltaTypeChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    if (!(d is DateTimeAxis dateTimeAxis) || dateTimeAxis.Area == null)
      return;
    dateTimeAxis.CanAutoScroll = true;
    dateTimeAxis.OnPropertyChanged();
  }

  private static int CalculateWeekEndDayCount(string dayName, DateTime start, DateTime end)
  {
    int num1;
    switch (dayName)
    {
      case "Monday":
        num1 = 1;
        break;
      case "Tuesday":
        num1 = 2;
        break;
      case "Wednesday":
        num1 = 3;
        break;
      case "Thursday":
        num1 = 4;
        break;
      case "Friday":
        num1 = 5;
        break;
      case "Saturday":
        num1 = 6;
        break;
      default:
        num1 = 0;
        break;
    }
    DayOfWeek dayOfWeek = (DayOfWeek) num1;
    TimeSpan timeSpan = end - start;
    int weekEndDayCount = (int) Math.Floor(timeSpan.TotalDays / 7.0);
    int num2 = (int) (timeSpan.TotalDays % 7.0);
    int num3 = end.DayOfWeek - dayOfWeek;
    if (num3 < 0)
      num3 += 7;
    if (num2 >= num3)
      ++weekEndDayCount;
    return weekEndDayCount;
  }

  private static void OnEnableBusinessHoursChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs args)
  {
    (d as DateTimeAxis).OnEnableBusinessHoursChanged((bool) args.NewValue);
  }

  private static void OnWorkDaysChanged(DependencyObject d, DependencyPropertyChangedEventArgs args)
  {
    ((DateTimeAxis) d).InternalWorkingDays = args.NewValue.ToString();
  }

  private static void OnRangePaddingChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    if (e.NewValue == null)
      return;
    (d as DateTimeAxis).OnPropertyChanged();
  }

  private static void OnIntervalTypeChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    if (!(d is DateTimeAxis dateTimeAxis) || e.NewValue == null)
      return;
    dateTimeAxis.ActualIntervalType = dateTimeAxis.IntervalType;
    dateTimeAxis.OnPropertyChanged();
  }

  private static void OnMinimumChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    (d as DateTimeAxis).OnMinimumChanged(e);
  }

  private static void OnMaximumChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    (d as DateTimeAxis).OnMaximumChanged(e);
  }

  private static void OnIntervalChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    (d as DateTimeAxis).OnIntervalChanged(e);
  }

  private DateTimeIntervalType GetActualAutoScrollingDeltaType()
  {
    if (this.AutoScrollingDeltaType != DateTimeIntervalType.Auto)
      return this.AutoScrollingDeltaType;
    this.CalculateDateTimeIntervalType(this.ActualRange, this.AvailableSize, ref this.dateTimeIntervalType);
    return this.dateTimeIntervalType;
  }

  private object CalculateDateTimeIntervalType(
    DoubleRange actualRange,
    Size availableSize,
    ref DateTimeIntervalType dateTimeIntervalType)
  {
    DateTime dateTime = actualRange.Start.FromOADate();
    TimeSpan timeSpan = actualRange.End.FromOADate().Subtract(dateTime);
    double timeIntervalType = 0.0;
    switch (dateTimeIntervalType)
    {
      case DateTimeIntervalType.Auto:
        double niceInterval1 = base.CalculateNiceInterval(new DoubleRange(0.0, timeSpan.TotalDays / 365.0), availableSize);
        if (niceInterval1 >= 1.0)
        {
          dateTimeIntervalType = DateTimeIntervalType.Years;
          return (object) niceInterval1;
        }
        double niceInterval2 = base.CalculateNiceInterval(new DoubleRange(0.0, timeSpan.TotalDays / 30.0), availableSize);
        if (niceInterval2 >= 1.0)
        {
          dateTimeIntervalType = DateTimeIntervalType.Months;
          return (object) niceInterval2;
        }
        double niceInterval3 = base.CalculateNiceInterval(new DoubleRange(0.0, timeSpan.TotalDays), availableSize);
        if (niceInterval3 >= 1.0)
        {
          dateTimeIntervalType = DateTimeIntervalType.Days;
          return (object) niceInterval3;
        }
        double niceInterval4 = base.CalculateNiceInterval(new DoubleRange(0.0, timeSpan.TotalHours), availableSize);
        if (niceInterval4 >= 1.0)
        {
          dateTimeIntervalType = DateTimeIntervalType.Hours;
          return (object) niceInterval4;
        }
        double niceInterval5 = base.CalculateNiceInterval(new DoubleRange(0.0, timeSpan.TotalMinutes), availableSize);
        if (niceInterval5 >= 1.0)
        {
          dateTimeIntervalType = DateTimeIntervalType.Minutes;
          return (object) niceInterval5;
        }
        double niceInterval6 = base.CalculateNiceInterval(new DoubleRange(0.0, timeSpan.TotalSeconds), availableSize);
        if (niceInterval6 >= 1.0)
        {
          dateTimeIntervalType = DateTimeIntervalType.Seconds;
          return (object) niceInterval6;
        }
        timeIntervalType = base.CalculateNiceInterval(new DoubleRange(0.0, timeSpan.TotalMilliseconds), availableSize);
        dateTimeIntervalType = DateTimeIntervalType.Milliseconds;
        break;
      case DateTimeIntervalType.Milliseconds:
        timeIntervalType = base.CalculateNiceInterval(new DoubleRange(0.0, timeSpan.TotalMilliseconds), availableSize);
        break;
      case DateTimeIntervalType.Seconds:
        timeIntervalType = base.CalculateNiceInterval(new DoubleRange(0.0, timeSpan.TotalSeconds), availableSize);
        break;
      case DateTimeIntervalType.Minutes:
        timeIntervalType = base.CalculateNiceInterval(new DoubleRange(0.0, timeSpan.TotalMinutes), availableSize);
        break;
      case DateTimeIntervalType.Hours:
        timeIntervalType = base.CalculateNiceInterval(new DoubleRange(0.0, timeSpan.TotalHours), availableSize);
        break;
      case DateTimeIntervalType.Days:
        timeIntervalType = base.CalculateNiceInterval(new DoubleRange(0.0, timeSpan.TotalDays), availableSize);
        break;
      case DateTimeIntervalType.Months:
        timeIntervalType = base.CalculateNiceInterval(new DoubleRange(0.0, timeSpan.TotalDays / 30.0), availableSize);
        break;
      case DateTimeIntervalType.Years:
        timeIntervalType = base.CalculateNiceInterval(new DoubleRange(0.0, timeSpan.TotalDays / 365.0), availableSize);
        break;
    }
    return (object) timeIntervalType;
  }

  private void OnMinMaxChanged()
  {
    DateTimeAxisHelper.OnMinMaxChanged((ChartAxis) this, this.Minimum, this.Maximum);
  }

  private void OnEnableBusinessHoursChanged(bool value)
  {
    if (value)
    {
      this.ValueToCoefficientCalc = new ChartAxis.ValueToCoefficientHandler(this.ValueToBusinesshoursCoefficient);
      this.CoefficientToValueCalc = new ChartAxis.ValueToCoefficientHandler(this.CoefficientToBusinesshoursValue);
    }
    else
    {
      this.ValueToCoefficientCalc = new ChartAxis.ValueToCoefficientHandler(((ChartAxis) this).ValueToCoefficient);
      this.CoefficientToValueCalc = new ChartAxis.ValueToCoefficientHandler(((ChartAxis) this).CoefficientToValue);
    }
    if (this.Area == null)
      return;
    this.Area.ScheduleUpdate();
  }

  private void CalculateNonWorkingDays(DoubleRange range)
  {
    string[] strArray = new string[7]
    {
      "Monday",
      "Tuesday",
      "Wednesday",
      "Thursday",
      "Friday",
      "Saturday",
      "Sunday"
    };
    this.NonWorkingDays = new List<string>();
    if (double.IsNaN(range.Start))
      return;
    DateTime startDate = range.Start.FromOADate();
    DateTime endDate = range.End.FromOADate();
    this.InternalWorkingDays = this.WorkingDays.ToString();
    foreach (string str in strArray)
    {
      if (!this.InternalWorkingDays.Contains(str))
        this.NonWorkingDays.Add(str);
    }
    this.nonWorkingHoursPerDay = 24.0 - (this.CloseTime - this.OpenTime);
    this.TotalNonWorkingHours = this.CalcNonWorkingHours(startDate, endDate, this.InternalWorkingDays, this.nonWorkingHoursPerDay);
  }

  private double CoefficientToBusinesshoursValue(double value)
  {
    double num1 = this.CloseTime - this.OpenTime;
    double start = this.VisibleRange.Start;
    double num2 = (this.VisibleRange.End - this.TotalNonWorkingHours / 24.0 - start) * value * 24.0 / num1;
    double num3 = num2 - (double) (int) num2;
    DateTime end = start.FromOADate().AddDays((double) (int) num2).AddHours(num3 * num1);
    if (this.NonWorkingDays.Count > 0)
    {
      int weekEndDayCount = DateTimeAxis.CalculateWeekEndDayCount(this.NonWorkingDays[0], start.FromOADate(), end);
      end = end.AddDays((double) (weekEndDayCount * this.NonWorkingDays.Count));
      if (this.NonWorkingDays.Contains(end.DayOfWeek.ToString()))
        end = end.AddDays((double) this.NonWorkingDays.Count);
    }
    return end.ToOADate();
  }

  private double ValueToBusinesshoursCoefficient(double value)
  {
    double num1 = double.NaN;
    double start = this.VisibleRange.Start;
    double num2 = this.VisibleRange.End - this.TotalNonWorkingHours / 24.0 - start;
    if (!double.IsNaN(value))
    {
      value -= this.CalcNonWorkingHours(start.FromOADate(), value.FromOADate(), this.InternalWorkingDays, this.nonWorkingHoursPerDay) / 24.0;
      num1 = num2 == 0.0 ? 0.0 : (value - start) / num2;
    }
    return !this.IsActualInversed ? num1 : 1.0 - num1;
  }
}
