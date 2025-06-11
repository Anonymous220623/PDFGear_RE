// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.TimeSpanAxis
// Assembly: Syncfusion.SfChart.WPF, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 279D70C6-7936-4F3E-B6CF-79EFEF734826
// Assembly location: C:\Program Files\PDFgear\Syncfusion.SfChart.WPF.dll

using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;

#nullable disable
namespace Syncfusion.UI.Xaml.Charts;

public class TimeSpanAxis : RangeAxisBase
{
  public static readonly DependencyProperty IntervalProperty = DependencyProperty.Register(nameof (Interval), typeof (TimeSpan?), typeof (TimeSpanAxis), new PropertyMetadata((object) null, new PropertyChangedCallback(TimeSpanAxis.OnIntervalChanged)));
  public static readonly DependencyProperty MinimumProperty = DependencyProperty.Register(nameof (Minimum), typeof (TimeSpan?), typeof (TimeSpanAxis), new PropertyMetadata((object) null, new PropertyChangedCallback(TimeSpanAxis.OnMinimumChanged)));
  public static readonly DependencyProperty MaximumProperty = DependencyProperty.Register(nameof (Maximum), typeof (TimeSpan?), typeof (TimeSpanAxis), new PropertyMetadata((object) null, new PropertyChangedCallback(TimeSpanAxis.OnMaximumChanged)));

  public TimeSpan? Interval
  {
    get => (TimeSpan?) this.GetValue(TimeSpanAxis.IntervalProperty);
    set => this.SetValue(TimeSpanAxis.IntervalProperty, (object) value);
  }

  public TimeSpan? Minimum
  {
    get => (TimeSpan?) this.GetValue(TimeSpanAxis.MinimumProperty);
    set => this.SetValue(TimeSpanAxis.MinimumProperty, (object) value);
  }

  public TimeSpan? Maximum
  {
    get => (TimeSpan?) this.GetValue(TimeSpanAxis.MaximumProperty);
    set => this.SetValue(TimeSpanAxis.MaximumProperty, (object) value);
  }

  public override object GetLabelContent(double position)
  {
    return this.CustomLabels.Count > 0 || this.GetLabelSource() != null ? this.GetCustomLabelContent(position) ?? this.GetActualLabelContent(position) : this.GetActualLabelContent(position);
  }

  internal override object GetActualLabelContent(double position)
  {
    return (object) TimeSpan.FromMilliseconds(position).ToString(this.LabelFormat, (IFormatProvider) CultureInfo.CurrentCulture);
  }

  protected internal override void CalculateVisibleRange(Size avalableSize)
  {
    base.CalculateVisibleRange(avalableSize);
    TimeSpanAxisHelper.CalculateVisibleRange((ChartAxisBase2D) this, (object) this.Interval, avalableSize);
  }

  protected internal override double CalculateActualInterval(DoubleRange range, Size availableSize)
  {
    return !this.Interval.HasValue ? this.CalculateNiceInterval(range, availableSize) : this.Interval.Value.TotalMilliseconds;
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
    TimeSpanAxisHelper.GenerateVisibleLabels((ChartAxisBase2D) this, (object) this.Minimum, (object) this.Maximum, (object) this.Interval);
  }

  protected override DoubleRange CalculateActualRange()
  {
    if (!this.Minimum.HasValue && !this.Maximum.HasValue)
    {
      DoubleRange actualRange = base.CalculateActualRange();
      if (this.Orientation.Equals((object) Orientation.Horizontal) && this.IncludeAnnotationRange && this.Area != null)
      {
        foreach (Annotation annotation in (Collection<Annotation>) (this.Area as SfChart).Annotations)
        {
          if (annotation.CoordinateUnit == CoordinateUnit.Axis && annotation.XAxis == this)
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
    return this.Maximum.HasValue ? new DoubleRange(double.IsNaN(actualRange1.Start) ? this.ActualRange.End - 1.0 : actualRange1.Start, this.ActualRange.End) : actualRange1;
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
    obj = (DependencyObject) new TimeSpanAxis()
    {
      Interval = this.Interval,
      Minimum = this.Minimum,
      Maximum = this.Maximum
    };
    return base.CloneAxis(obj);
  }

  private static void OnMinimumChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    (d as TimeSpanAxis).OnMinimumChanged(e);
  }

  private static void OnMaximumChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    (d as TimeSpanAxis).OnMaximumChanged(e);
  }

  private static void OnIntervalChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    (d as TimeSpanAxis).OnIntervalChanged(e);
  }

  private void OnMinMaxChanged()
  {
    if (this.Minimum.HasValue || this.Maximum.HasValue)
      this.ActualRange = new DoubleRange(!this.Minimum.HasValue ? TimeSpan.MinValue.TotalMilliseconds : this.Minimum.Value.TotalMilliseconds, !this.Maximum.HasValue ? TimeSpan.MaxValue.TotalMilliseconds : this.Maximum.Value.TotalMilliseconds);
    if (this.Area == null)
      return;
    this.Area.ScheduleUpdate();
  }
}
