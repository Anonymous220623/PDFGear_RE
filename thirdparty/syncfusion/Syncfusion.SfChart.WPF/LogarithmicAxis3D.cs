// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.LogarithmicAxis3D
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

public class LogarithmicAxis3D : RangeAxisBase3D
{
  public static readonly DependencyProperty IntervalProperty = DependencyProperty.Register(nameof (Interval), typeof (double?), typeof (LogarithmicAxis3D), new PropertyMetadata((object) null, new PropertyChangedCallback(LogarithmicAxis3D.OnIntervalChanged)));
  public static readonly DependencyProperty MinimumProperty = DependencyProperty.Register(nameof (Minimum), typeof (double?), typeof (LogarithmicAxis3D), new PropertyMetadata((object) null, new PropertyChangedCallback(LogarithmicAxis3D.OnMinimumChanged)));
  public static readonly DependencyProperty MaximumProperty = DependencyProperty.Register(nameof (Maximum), typeof (double?), typeof (LogarithmicAxis3D), new PropertyMetadata((object) null, new PropertyChangedCallback(LogarithmicAxis3D.OnMaximumChanged)));
  public static readonly DependencyProperty LogarithmicBaseProperty = DependencyProperty.Register(nameof (LogarithmicBase), typeof (double), typeof (LogarithmicAxis3D), new PropertyMetadata((object) 10.0, new PropertyChangedCallback(LogarithmicAxis3D.OnLogarithmicAxisValueChanged)));

  public LogarithmicAxis3D() => this.IsLogarithmic = true;

  public double? Interval
  {
    get => (double?) this.GetValue(LogarithmicAxis3D.IntervalProperty);
    set => this.SetValue(LogarithmicAxis3D.IntervalProperty, (object) value);
  }

  public double? Minimum
  {
    get => (double?) this.GetValue(LogarithmicAxis3D.MinimumProperty);
    set => this.SetValue(LogarithmicAxis3D.MinimumProperty, (object) value);
  }

  public double? Maximum
  {
    get => (double?) this.GetValue(LogarithmicAxis3D.MaximumProperty);
    set => this.SetValue(LogarithmicAxis3D.MaximumProperty, (object) value);
  }

  public double LogarithmicBase
  {
    get => (double) this.GetValue(LogarithmicAxis3D.LogarithmicBaseProperty);
    set => this.SetValue(LogarithmicAxis3D.LogarithmicBaseProperty, (object) value);
  }

  public override double CoefficientToValue(double value)
  {
    return Math.Pow(this.LogarithmicBase, base.CoefficientToValue(value));
  }

  public override object GetLabelContent(double position)
  {
    return (object) Math.Round(Math.Pow(this.LogarithmicBase, Math.Log(position, this.LogarithmicBase)), 11).ToString(this.LabelFormat, (IFormatProvider) CultureInfo.CurrentCulture);
  }

  protected internal override double CalculateNiceInterval(
    DoubleRange actualRange,
    Size availableSize)
  {
    return LogarithmicAxisHelper.CalculateNiceInterval((ChartAxis) this, actualRange, availableSize);
  }

  protected internal override void AddSmallTicksPoint(double position, double logarithmicBase)
  {
    LogarithmicAxisHelper.AddSmallTicksPoint((ChartAxis) this, position, logarithmicBase, (double) this.SmallTicksPerInterval);
  }

  protected internal override double CalculateActualInterval(DoubleRange range, Size availableSize)
  {
    return !this.Interval.HasValue ? this.CalculateNiceInterval(range, availableSize) : this.Interval.Value;
  }

  protected override void GenerateVisibleLabels()
  {
    LogarithmicAxisHelper.GenerateVisibleLabels3D((ChartAxis) this, (object) this.Minimum, (object) this.Maximum, (object) this.Interval, this.LogarithmicBase);
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

  protected override DoubleRange CalculateActualRange()
  {
    if (!this.Minimum.HasValue && !this.Maximum.HasValue)
      return LogarithmicAxisHelper.CalculateActualRange((ChartAxis) this, base.CalculateActualRange(), this.LogarithmicBase);
    if (this.Minimum.HasValue && this.Maximum.HasValue)
      return this.ActualRange;
    DoubleRange actualRange = LogarithmicAxisHelper.CalculateActualRange((ChartAxis) this, this.CalculateBaseActualRange(), this.LogarithmicBase);
    if (this.Minimum.HasValue)
      return new DoubleRange(this.ActualRange.Start, actualRange.End);
    return this.Maximum.HasValue ? new DoubleRange(actualRange.Start, this.ActualRange.End) : actualRange;
  }

  protected override DoubleRange ApplyRangePadding(DoubleRange range, double interval)
  {
    if (!this.Minimum.HasValue && !this.Maximum.HasValue)
      return base.ApplyRangePadding(range, interval);
    if (this.Minimum.HasValue && this.Maximum.HasValue)
      return range;
    DoubleRange doubleRange = base.ApplyRangePadding(range, interval);
    return !this.Minimum.HasValue ? new DoubleRange(doubleRange.Start, range.End) : new DoubleRange(range.Start, doubleRange.End);
  }

  protected override DependencyObject CloneAxis(DependencyObject obj)
  {
    obj = (DependencyObject) new LogarithmicAxis3D()
    {
      Interval = this.Interval,
      Minimum = this.Minimum,
      Maximum = this.Maximum,
      LogarithmicBase = this.LogarithmicBase
    };
    return base.CloneAxis(obj);
  }

  private static DoubleRange GetRange(List<double> values, double rangeEnd)
  {
    if (values.All<double>((Func<double, bool>) (value => double.IsNaN(value) || value <= 0.0)))
      return DoubleRange.Empty;
    IEnumerable<double> source = values.Where<double>((Func<double, bool>) (value => value > 0.0));
    if (!source.Any<double>())
      return DoubleRange.Empty;
    double start = source.Min();
    return start > 0.0 && start < 1.0 ? new DoubleRange(start, rangeEnd) : new DoubleRange(1.0, rangeEnd);
  }

  private static void OnLogarithmicAxisValueChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
  }

  private static void OnMinimumChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    ((LogarithmicAxis3D) d).OnMinimumChanged(e);
  }

  private static void OnMaximumChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    ((LogarithmicAxis3D) d).OnMaximumChanged(e);
  }

  private static void OnIntervalChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    ((LogarithmicAxis3D) d).OnIntervalChanged(e);
  }

  private void OnMinMaxChanged()
  {
    LogarithmicAxisHelper.OnMinMaxChanged((ChartAxis) this, (object) this.Minimum, (object) this.Maximum, this.LogarithmicBase);
  }

  private DoubleRange CalculateBaseActualRange()
  {
    if (this.Area == null)
      return DoubleRange.Empty;
    foreach (XyDataSeries xyDataSeries in this.Area.VisibleSeries.OfType<XyDataSeries>())
    {
      if (xyDataSeries.ActualYAxis == this && xyDataSeries.YValues.Count > 0 && (double.IsNaN(xyDataSeries.YValues.Min()) || xyDataSeries.YValues.Min() <= 0.0))
        xyDataSeries.YRange = LogarithmicAxis3D.GetRange(xyDataSeries.YValues as List<double>, xyDataSeries.YRange.End);
      if (xyDataSeries.ActualXValues is List<double> actualXvalues && xyDataSeries.ActualXAxis == this && actualXvalues.Count > 0 && (double.IsNaN(actualXvalues.Min()) || actualXvalues.Min() <= 0.0))
        xyDataSeries.XRange = LogarithmicAxis3D.GetRange(actualXvalues, xyDataSeries.XRange.End);
    }
    return this.Area.VisibleSeries.OfType<ISupportAxes>().Select<ISupportAxes, DoubleRange>((Func<ISupportAxes, DoubleRange>) (series =>
    {
      if (series.ActualXAxis == this)
        return series.XRange;
      return series.ActualYAxis == this ? series.YRange : DoubleRange.Empty;
    })).Sum();
  }
}
