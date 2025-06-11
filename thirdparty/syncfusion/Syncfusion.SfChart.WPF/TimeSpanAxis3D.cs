// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.TimeSpanAxis3D
// Assembly: Syncfusion.SfChart.WPF, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 279D70C6-7936-4F3E-B6CF-79EFEF734826
// Assembly location: C:\Program Files\PDFgear\Syncfusion.SfChart.WPF.dll

using System;
using System.Globalization;
using System.Windows;

#nullable disable
namespace Syncfusion.UI.Xaml.Charts;

public class TimeSpanAxis3D : RangeAxisBase3D
{
  public static readonly DependencyProperty IntervalProperty = DependencyProperty.Register(nameof (Interval), typeof (TimeSpan?), typeof (TimeSpanAxis3D), new PropertyMetadata((object) null, new PropertyChangedCallback(TimeSpanAxis3D.OnIntervalChanged)));
  public static readonly DependencyProperty MinimumProperty = DependencyProperty.Register(nameof (Minimum), typeof (TimeSpan?), typeof (TimeSpanAxis3D), new PropertyMetadata((object) null, new PropertyChangedCallback(TimeSpanAxis3D.OnMinimumChanged)));
  public static readonly DependencyProperty MaximumProperty = DependencyProperty.Register(nameof (Maximum), typeof (TimeSpan?), typeof (TimeSpanAxis3D), new PropertyMetadata((object) null, new PropertyChangedCallback(TimeSpanAxis3D.OnMaximumChanged)));

  public TimeSpan? Interval
  {
    get => (TimeSpan?) this.GetValue(TimeSpanAxis3D.IntervalProperty);
    set => this.SetValue(TimeSpanAxis3D.IntervalProperty, (object) value);
  }

  public TimeSpan? Minimum
  {
    get => (TimeSpan?) this.GetValue(TimeSpanAxis3D.MinimumProperty);
    set => this.SetValue(TimeSpanAxis3D.MinimumProperty, (object) value);
  }

  public TimeSpan? Maximum
  {
    get => (TimeSpan?) this.GetValue(TimeSpanAxis3D.MaximumProperty);
    set => this.SetValue(TimeSpanAxis3D.MaximumProperty, (object) value);
  }

  public override object GetLabelContent(double position)
  {
    return (object) TimeSpan.FromMilliseconds(position).ToString(this.LabelFormat, (IFormatProvider) CultureInfo.CurrentCulture);
  }

  protected internal override double CalculateActualInterval(DoubleRange range, Size availableSize)
  {
    return !this.Interval.HasValue ? this.CalculateNiceInterval(range, availableSize) : this.Interval.Value.TotalMilliseconds;
  }

  protected override void GenerateVisibleLabels()
  {
    TimeSpanAxisHelper.GenerateVisibleLabels3D((ChartAxis) this, (object) this.Minimum, (object) this.Maximum, (object) this.Interval);
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
      return base.CalculateActualRange();
    if (this.Minimum.HasValue && this.Maximum.HasValue)
      return this.ActualRange;
    DoubleRange actualRange = base.CalculateActualRange();
    if (this.Minimum.HasValue)
      return new DoubleRange(this.ActualRange.Start, double.IsNaN(actualRange.End) ? TimeSpan.MaxValue.TotalMilliseconds : actualRange.End);
    return this.Maximum.HasValue ? new DoubleRange(double.IsNaN(actualRange.Start) ? TimeSpan.MinValue.TotalMilliseconds : actualRange.Start, this.ActualRange.End) : actualRange;
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
    obj = (DependencyObject) new TimeSpanAxis3D()
    {
      Interval = this.Interval,
      Minimum = this.Minimum,
      Maximum = this.Maximum
    };
    return base.CloneAxis(obj);
  }

  private static void OnMinimumChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    ((TimeSpanAxis3D) d).OnMinimumChanged(e);
  }

  private static void OnMaximumChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    ((TimeSpanAxis3D) d).OnMaximumChanged(e);
  }

  private static void OnIntervalChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    ((TimeSpanAxis3D) d).OnIntervalChanged(e);
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
