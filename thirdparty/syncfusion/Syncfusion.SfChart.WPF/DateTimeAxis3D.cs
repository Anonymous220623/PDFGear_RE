// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.DateTimeAxis3D
// Assembly: Syncfusion.SfChart.WPF, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 279D70C6-7936-4F3E-B6CF-79EFEF734826
// Assembly location: C:\Program Files\PDFgear\Syncfusion.SfChart.WPF.dll

using System;
using System.Globalization;
using System.Windows;

#nullable disable
namespace Syncfusion.UI.Xaml.Charts;

public class DateTimeAxis3D : RangeAxisBase3D
{
  public static readonly DependencyProperty MinimumProperty = DependencyProperty.Register(nameof (Minimum), typeof (DateTime?), typeof (DateTimeAxis3D), new PropertyMetadata((object) null, new PropertyChangedCallback(DateTimeAxis3D.OnMinimumChanged)));
  public static readonly DependencyProperty MaximumProperty = DependencyProperty.Register(nameof (Maximum), typeof (DateTime?), typeof (DateTimeAxis3D), new PropertyMetadata((object) null, new PropertyChangedCallback(DateTimeAxis3D.OnMaximumChanged)));
  public static readonly DependencyProperty IntervalProperty = DependencyProperty.Register(nameof (Interval), typeof (double), typeof (DateTimeAxis3D), new PropertyMetadata((object) 0.0, new PropertyChangedCallback(DateTimeAxis3D.OnIntervalChanged)));
  public static readonly DependencyProperty RangePaddingProperty = DependencyProperty.Register(nameof (RangePadding), typeof (DateTimeRangePadding), typeof (DateTimeAxis3D), new PropertyMetadata((object) DateTimeRangePadding.Auto, new PropertyChangedCallback(DateTimeAxis3D.OnRangePaddingChanged)));
  public static readonly DependencyProperty IntervalTypeProperty = DependencyProperty.Register(nameof (IntervalType), typeof (DateTimeIntervalType), typeof (DateTimeAxis3D), new PropertyMetadata((object) DateTimeIntervalType.Auto, new PropertyChangedCallback(DateTimeAxis3D.OnIntervalTypeChanged)));

  public DateTime? Minimum
  {
    get => (DateTime?) this.GetValue(DateTimeAxis3D.MinimumProperty);
    set => this.SetValue(DateTimeAxis3D.MinimumProperty, (object) value);
  }

  public DateTime? Maximum
  {
    get => (DateTime?) this.GetValue(DateTimeAxis3D.MaximumProperty);
    set => this.SetValue(DateTimeAxis3D.MaximumProperty, (object) value);
  }

  public double Interval
  {
    get => (double) this.GetValue(DateTimeAxis3D.IntervalProperty);
    set => this.SetValue(DateTimeAxis3D.IntervalProperty, (object) value);
  }

  public DateTimeRangePadding RangePadding
  {
    get => (DateTimeRangePadding) this.GetValue(DateTimeAxis3D.RangePaddingProperty);
    set => this.SetValue(DateTimeAxis3D.RangePaddingProperty, (object) value);
  }

  public DateTimeIntervalType IntervalType
  {
    get => (DateTimeIntervalType) this.GetValue(DateTimeAxis3D.IntervalTypeProperty);
    set => this.SetValue(DateTimeAxis3D.IntervalTypeProperty, (object) value);
  }

  internal DateTimeIntervalType ActualIntervalType3D { get; set; }

  public override object GetLabelContent(double position)
  {
    return (object) position.FromOADate().ToString(this.LabelFormat, (IFormatProvider) CultureInfo.CurrentCulture);
  }

  protected internal override double CalculateActualInterval(DoubleRange range, Size availableSize)
  {
    if (this.Interval == 0.0 || double.IsNaN(this.Interval))
      return this.CalculateNiceInterval(range, availableSize);
    if (this.IntervalType == DateTimeIntervalType.Auto)
      this.CalculateNiceInterval(range, availableSize);
    return this.Interval;
  }

  protected internal override double CalculateNiceInterval(
    DoubleRange actualRange,
    Size availableSize)
  {
    DateTime dateTime = actualRange.Start.FromOADate();
    TimeSpan timeSpan = actualRange.End.FromOADate().Subtract(dateTime);
    double niceInterval1 = base.CalculateNiceInterval(new DoubleRange(0.0, timeSpan.TotalDays / 365.0), availableSize);
    if (niceInterval1 >= 1.0)
    {
      this.ActualIntervalType3D = DateTimeIntervalType.Years;
      return niceInterval1;
    }
    double niceInterval2 = base.CalculateNiceInterval(new DoubleRange(0.0, timeSpan.TotalDays / 30.0), availableSize);
    if (niceInterval2 >= 1.0)
    {
      this.ActualIntervalType3D = DateTimeIntervalType.Months;
      return niceInterval2;
    }
    double niceInterval3 = base.CalculateNiceInterval(new DoubleRange(0.0, timeSpan.TotalDays), availableSize);
    if (niceInterval3 >= 1.0)
    {
      this.ActualIntervalType3D = DateTimeIntervalType.Days;
      return niceInterval3;
    }
    double niceInterval4 = base.CalculateNiceInterval(new DoubleRange(0.0, timeSpan.TotalHours), availableSize);
    if (niceInterval4 >= 1.0)
    {
      this.ActualIntervalType3D = DateTimeIntervalType.Hours;
      return niceInterval4;
    }
    double niceInterval5 = base.CalculateNiceInterval(new DoubleRange(0.0, timeSpan.TotalMinutes), availableSize);
    if (niceInterval5 >= 1.0)
    {
      this.ActualIntervalType3D = DateTimeIntervalType.Minutes;
      return niceInterval5;
    }
    double niceInterval6 = base.CalculateNiceInterval(new DoubleRange(0.0, timeSpan.TotalSeconds), availableSize);
    if (niceInterval6 >= 1.0)
    {
      this.ActualIntervalType3D = DateTimeIntervalType.Seconds;
      return niceInterval6;
    }
    double niceInterval7 = base.CalculateNiceInterval(new DoubleRange(0.0, timeSpan.TotalMilliseconds), availableSize);
    this.ActualIntervalType3D = DateTimeIntervalType.Milliseconds;
    return niceInterval7;
  }

  protected virtual void OnIntervalChanged(DependencyPropertyChangedEventArgs e)
  {
    if (this.Area == null)
      return;
    this.Area.ScheduleUpdate();
  }

  protected override DoubleRange CalculateActualRange()
  {
    if (this.ActualRange.IsEmpty)
      return base.CalculateActualRange();
    if (this.Minimum.HasValue && this.Maximum.HasValue)
      return this.ActualRange;
    DoubleRange actualRange = base.CalculateActualRange();
    if (this.Minimum.HasValue)
      return new DoubleRange(this.ActualRange.Start, double.IsNaN(actualRange.End) ? DateTime.MaxValue.ToOADate() : actualRange.End);
    return this.Maximum.HasValue ? new DoubleRange(double.IsNaN(actualRange.Start) ? DateTime.MinValue.ToOADate() : actualRange.Start, this.ActualRange.End) : actualRange;
  }

  protected override DoubleRange ApplyRangePadding(DoubleRange range, double interval)
  {
    if (!this.Minimum.HasValue && !this.Maximum.HasValue)
      return DateTimeAxisHelper.ApplyRangePadding((ChartAxis) this, base.ApplyRangePadding(range, interval), interval, this.RangePadding, this.ActualIntervalType3D);
    if (this.Minimum.HasValue && this.Maximum.HasValue)
      return range;
    DoubleRange doubleRange = DateTimeAxisHelper.ApplyRangePadding((ChartAxis) this, base.ApplyRangePadding(range, interval), interval, this.RangePadding, this.ActualIntervalType3D);
    return !this.Minimum.HasValue ? new DoubleRange(doubleRange.Start, range.End) : new DoubleRange(range.Start, doubleRange.End);
  }

  protected override DependencyObject CloneAxis(DependencyObject obj)
  {
    obj = (DependencyObject) new DateTimeAxis3D()
    {
      Interval = this.Interval,
      Minimum = this.Minimum,
      Maximum = this.Maximum,
      RangePadding = this.RangePadding,
      IntervalType = this.IntervalType
    };
    return base.CloneAxis(obj);
  }

  protected virtual void OnMaximumChanged(DependencyPropertyChangedEventArgs args)
  {
    this.OnMinMaxChanged();
  }

  protected virtual void OnMinimumChanged(DependencyPropertyChangedEventArgs args)
  {
    this.OnMinMaxChanged();
  }

  protected override void GenerateVisibleLabels()
  {
    DateTimeAxisHelper.GenerateVisibleLabels3D((ChartAxis) this, (object) this.Minimum, (object) this.Maximum, this.ActualIntervalType3D);
  }

  private static void OnIntervalChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    ((DateTimeAxis3D) d).OnIntervalChanged(e);
  }

  private static void OnMaximumChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    ((DateTimeAxis3D) d).OnMaximumChanged(e);
  }

  private static void OnMinimumChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    ((DateTimeAxis3D) d).OnMinimumChanged(e);
  }

  private static void OnRangePaddingChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    if (e.NewValue == null)
      return;
    ((ChartAxis) d).OnPropertyChanged();
  }

  private static void OnIntervalTypeChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    if (!(d is DateTimeAxis3D dateTimeAxis3D) || e.NewValue == null)
      return;
    dateTimeAxis3D.ActualIntervalType3D = dateTimeAxis3D.IntervalType;
  }

  private void OnMinMaxChanged()
  {
    DateTimeAxisHelper.OnMinMaxChanged((ChartAxis) this, this.Minimum, this.Maximum);
  }
}
