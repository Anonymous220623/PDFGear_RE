// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.LogarithmicAxis
// Assembly: Syncfusion.SfChart.WPF, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 279D70C6-7936-4F3E-B6CF-79EFEF734826
// Assembly location: C:\Program Files\PDFgear\Syncfusion.SfChart.WPF.dll

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

#nullable disable
namespace Syncfusion.UI.Xaml.Charts;

public class LogarithmicAxis : RangeAxisBase
{
  internal const int CLogRoundDecimals = 3;
  public static readonly DependencyProperty IntervalProperty = DependencyProperty.Register(nameof (Interval), typeof (double?), typeof (LogarithmicAxis), new PropertyMetadata((object) null, new PropertyChangedCallback(LogarithmicAxis.OnIntervalChanged)));
  public static readonly DependencyProperty MinimumProperty = DependencyProperty.Register(nameof (Minimum), typeof (double?), typeof (LogarithmicAxis), new PropertyMetadata((object) null, new PropertyChangedCallback(LogarithmicAxis.OnMinimumChanged)));
  public static readonly DependencyProperty MaximumProperty = DependencyProperty.Register(nameof (Maximum), typeof (double?), typeof (LogarithmicAxis), new PropertyMetadata((object) null, new PropertyChangedCallback(LogarithmicAxis.OnMaximumChanged)));
  public static readonly DependencyProperty LogarithmicBaseProperty = DependencyProperty.Register(nameof (LogarithmicBase), typeof (double), typeof (ChartAxis), new PropertyMetadata((object) 10.0, new PropertyChangedCallback(LogarithmicAxis.OnLogarithmicAxisValueChanged)));

  public LogarithmicAxis() => this.IsLogarithmic = true;

  public double? Interval
  {
    get => (double?) this.GetValue(LogarithmicAxis.IntervalProperty);
    set => this.SetValue(LogarithmicAxis.IntervalProperty, (object) value);
  }

  public double? Minimum
  {
    get => (double?) this.GetValue(LogarithmicAxis.MinimumProperty);
    set => this.SetValue(LogarithmicAxis.MinimumProperty, (object) value);
  }

  public double? Maximum
  {
    get => (double?) this.GetValue(LogarithmicAxis.MaximumProperty);
    set => this.SetValue(LogarithmicAxis.MaximumProperty, (object) value);
  }

  public double LogarithmicBase
  {
    get => (double) this.GetValue(LogarithmicAxis.LogarithmicBaseProperty);
    set => this.SetValue(LogarithmicAxis.LogarithmicBaseProperty, (object) value);
  }

  public override double CoefficientToValue(double value)
  {
    return Math.Pow(this.LogarithmicBase, base.CoefficientToValue(value));
  }

  public override object GetLabelContent(double position)
  {
    if (this.VisibleLabels == null || this.CustomLabels.Count <= 0 && this.GetLabelSource() == null)
      return this.GetActualLabelContent(position);
    position = Math.Log(position, this.LogarithmicBase);
    return this.GetCustomLabelContent(position) ?? this.GetActualLabelContent(position);
  }

  internal override void UpdateAutoScrollingDelta(double autoScrollingDelta)
  {
    this.ZoomFactor = new DoubleRange(this.ActualRange.End - this.AutoScrollingDelta, this.VisibleRange.End).Delta / this.VisibleRange.Delta;
    this.ZoomPosition = 1.0 - this.ZoomFactor;
  }

  internal override object GetActualLabelContent(double position)
  {
    return (object) Math.Round(Math.Pow(this.LogarithmicBase, Math.Log(position, this.LogarithmicBase)), 3).ToString(this.LabelFormat, (IFormatProvider) CultureInfo.CurrentCulture);
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

  protected internal override void CalculateVisibleRange(Size avalableSize)
  {
    base.CalculateVisibleRange(avalableSize);
    LogarithmicAxisHelper.CalculateVisibleRange((ChartAxisBase2D) this, avalableSize, (object) this.Interval);
  }

  protected override void GenerateVisibleLabels()
  {
    this.SetRangeForAxisStyle();
    LogarithmicAxisHelper.GenerateVisibleLabels((ChartAxisBase2D) this, (object) this.Minimum, (object) this.Maximum, (object) this.Interval, this.LogarithmicBase);
  }

  protected override DoubleRange CalculateActualRange()
  {
    if (!this.Minimum.HasValue && !this.Maximum.HasValue)
    {
      DoubleRange actualRange = LogarithmicAxisHelper.CalculateActualRange((ChartAxis) this, this.CalculateBaseActualRange(), this.LogarithmicBase);
      if (this.IncludeAnnotationRange)
      {
        foreach (Annotation annotation in (Collection<Annotation>) (this.Area as SfChart).Annotations)
        {
          if (this.Orientation == Orientation.Vertical && annotation.CoordinateUnit == CoordinateUnit.Axis && annotation.YAxis == this)
          {
            DoubleRange doubleRange1 = actualRange;
            double start = Annotation.ConvertData(annotation.Y1, (ChartAxis) this);
            double end;
            switch (annotation)
            {
              case TextAnnotation _:
                end = Annotation.ConvertData(annotation.Y1, (ChartAxis) this);
                break;
              case ImageAnnotation _:
                end = Annotation.ConvertData((annotation as ImageAnnotation).Y2, (ChartAxis) this);
                break;
              default:
                end = Annotation.ConvertData((annotation as ShapeAnnotation).Y2, (ChartAxis) this);
                break;
            }
            DoubleRange doubleRange2 = new DoubleRange(start, end);
            actualRange = doubleRange1 + doubleRange2;
          }
          else if (this.Orientation == Orientation.Horizontal && annotation.CoordinateUnit == CoordinateUnit.Axis && annotation.XAxis == this)
          {
            DoubleRange doubleRange3 = actualRange;
            double start = Annotation.ConvertData(annotation.X1, (ChartAxis) this);
            double end;
            switch (annotation)
            {
              case TextAnnotation _:
                end = Annotation.ConvertData(annotation.X1, (ChartAxis) this);
                break;
              case ImageAnnotation _:
                end = Annotation.ConvertData((annotation as ImageAnnotation).X2, (ChartAxis) this);
                break;
              default:
                end = Annotation.ConvertData((annotation as ShapeAnnotation).X2, (ChartAxis) this);
                break;
            }
            DoubleRange doubleRange4 = new DoubleRange(start, end);
            actualRange = doubleRange3 + doubleRange4;
          }
        }
      }
      return actualRange;
    }
    if (this.Minimum.HasValue && this.Maximum.HasValue)
      return this.ActualRange;
    DoubleRange actualRange1 = LogarithmicAxisHelper.CalculateActualRange((ChartAxis) this, this.CalculateBaseActualRange(), this.LogarithmicBase);
    if (this.Minimum.HasValue)
      return new DoubleRange(this.ActualRange.Start, actualRange1.End);
    return this.Maximum.HasValue ? new DoubleRange(actualRange1.Start, this.ActualRange.End) : actualRange1;
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
    obj = (DependencyObject) new LogarithmicAxis()
    {
      Minimum = this.Minimum,
      Maximum = this.Maximum,
      Interval = this.Interval,
      LogarithmicBase = this.LogarithmicBase
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

  protected virtual void OnIntervalChanged(DependencyPropertyChangedEventArgs e)
  {
    if (this.Area == null)
      return;
    this.Area.ScheduleUpdate();
  }

  private static void OnLogarithmicAxisValueChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    (d as LogarithmicAxis).OnLogBaseChanged();
  }

  private static void OnMinimumChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    (d as LogarithmicAxis).OnMinimumChanged(e);
  }

  private static void OnMaximumChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    (d as LogarithmicAxis).OnMaximumChanged(e);
  }

  private static void OnIntervalChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    (d as LogarithmicAxis).OnIntervalChanged(e);
  }

  private static IList<double> GetYValues(ChartSeriesBase chartSeries)
  {
    switch (chartSeries)
    {
      case XyDataSeries _:
        return (chartSeries as XyDataSeries).YValues;
      case RangeSeriesBase _:
        return (IList<double>) (chartSeries as RangeSeriesBase).HighValues.Union<double>((IEnumerable<double>) (chartSeries as RangeSeriesBase).LowValues).ToList<double>();
      case FinancialSeriesBase _:
        return (IList<double>) (chartSeries as FinancialSeriesBase).HighValues.Union<double>((IEnumerable<double>) (chartSeries as FinancialSeriesBase).LowValues).ToList<double>();
      default:
        return (IList<double>) new List<double>();
    }
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

  private void OnLogBaseChanged()
  {
    if (this.Area == null)
      return;
    this.Area.ScheduleUpdate();
  }

  private void OnMinMaxChanged()
  {
    LogarithmicAxisHelper.OnMinMaxChanged((ChartAxis) this, (object) this.Minimum, (object) this.Maximum, this.LogarithmicBase);
  }

  private DoubleRange CalculateBaseActualRange()
  {
    if (this.Area == null)
      return DoubleRange.Empty;
    List<ChartSeriesBase> second = new List<ChartSeriesBase>();
    if (this.Area is SfChart)
    {
      foreach (ChartSeries technicalIndicator in (Collection<ChartSeries>) (this.Area as SfChart).TechnicalIndicators)
        second.Add((ChartSeriesBase) technicalIndicator);
    }
    foreach (ChartSeriesBase chartSeries in this.Area.VisibleSeries.Union<ChartSeriesBase>((IEnumerable<ChartSeriesBase>) second).Where<ChartSeriesBase>((Func<ChartSeriesBase, bool>) (series => series.ActualXAxis == this || series.ActualYAxis == this)))
    {
      IList<double> yvalues = LogarithmicAxis.GetYValues(chartSeries);
      if (chartSeries.ActualYAxis == this && yvalues.Count > 0 && (double.IsNaN(yvalues.Min()) || yvalues.Min() <= 0.0))
      {
        if (chartSeries is PolarRadarSeriesBase)
          (chartSeries as PolarRadarSeriesBase).YRange = LogarithmicAxis.GetRange(yvalues as List<double>, (chartSeries as PolarRadarSeriesBase).YRange.End);
        else
          (chartSeries as CartesianSeries).YRange = LogarithmicAxis.GetRange(yvalues as List<double>, (chartSeries as CartesianSeries).YRange.End);
      }
      if (chartSeries.ActualXValues is List<double> actualXvalues && chartSeries.ActualXAxis == this && actualXvalues.Count > 0 && (double.IsNaN(actualXvalues.Min()) || actualXvalues.Min() <= 0.0))
      {
        if (chartSeries is PolarRadarSeriesBase)
          (chartSeries as PolarRadarSeriesBase).XRange = LogarithmicAxis.GetRange(actualXvalues, (chartSeries as PolarRadarSeriesBase).XRange.End);
        else
          (chartSeries as CartesianSeries).XRange = LogarithmicAxis.GetRange(actualXvalues, (chartSeries as CartesianSeries).XRange.End);
      }
    }
    return this.Area.VisibleSeries.OfType<ISupportAxes>().Select<ISupportAxes, DoubleRange>((Func<ISupportAxes, DoubleRange>) (series =>
    {
      if (series.ActualXAxis == this)
        return series.XRange;
      return series.ActualYAxis == this ? series.YRange : DoubleRange.Empty;
    })).Sum();
  }
}
