// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.NumericalAxis3D
// Assembly: Syncfusion.SfChart.WPF, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 279D70C6-7936-4F3E-B6CF-79EFEF734826
// Assembly location: C:\Program Files\PDFgear\Syncfusion.SfChart.WPF.dll

using System.Windows;
using System.Windows.Controls;

#nullable disable
namespace Syncfusion.UI.Xaml.Charts;

public class NumericalAxis3D : RangeAxisBase3D
{
  public static readonly DependencyProperty IntervalProperty = DependencyProperty.Register(nameof (Interval), typeof (double?), typeof (NumericalAxis3D), new PropertyMetadata((object) null, new PropertyChangedCallback(NumericalAxis3D.OnIntervalChanged)));
  public static readonly DependencyProperty MinimumProperty = DependencyProperty.Register(nameof (Minimum), typeof (double?), typeof (NumericalAxis3D), new PropertyMetadata((object) null, new PropertyChangedCallback(NumericalAxis3D.OnMinimumChanged)));
  public static readonly DependencyProperty MaximumProperty = DependencyProperty.Register(nameof (Maximum), typeof (double?), typeof (NumericalAxis3D), new PropertyMetadata((object) null, new PropertyChangedCallback(NumericalAxis3D.OnMaximumChanged)));
  public static readonly DependencyProperty RangePaddingProperty = DependencyProperty.Register(nameof (RangePadding), typeof (NumericalPadding), typeof (NumericalAxis3D), new PropertyMetadata((object) NumericalPadding.Auto, new PropertyChangedCallback(NumericalAxis3D.OnPropertyChanged)));
  public static readonly DependencyProperty StartRangeFromZeroProperty = DependencyProperty.Register(nameof (StartRangeFromZero), typeof (bool), typeof (NumericalAxis3D), new PropertyMetadata((object) false, new PropertyChangedCallback(NumericalAxis3D.OnPropertyChanged)));

  public double? Interval
  {
    get => (double?) this.GetValue(NumericalAxis3D.IntervalProperty);
    set => this.SetValue(NumericalAxis3D.IntervalProperty, (object) value);
  }

  public double? Minimum
  {
    get => (double?) this.GetValue(NumericalAxis3D.MinimumProperty);
    set => this.SetValue(NumericalAxis3D.MinimumProperty, (object) value);
  }

  public double? Maximum
  {
    get => (double?) this.GetValue(NumericalAxis3D.MaximumProperty);
    set => this.SetValue(NumericalAxis3D.MaximumProperty, (object) value);
  }

  public NumericalPadding RangePadding
  {
    get => (NumericalPadding) this.GetValue(NumericalAxis3D.RangePaddingProperty);
    set => this.SetValue(NumericalAxis3D.RangePaddingProperty, (object) value);
  }

  public bool StartRangeFromZero
  {
    get => (bool) this.GetValue(NumericalAxis3D.StartRangeFromZeroProperty);
    set => this.SetValue(NumericalAxis3D.StartRangeFromZeroProperty, (object) value);
  }

  internal NumericalPadding ActualRangePadding
  {
    get
    {
      SfChart3D area = this.Area as SfChart3D;
      return this.RangePadding == NumericalPadding.Auto && area != null && area.Series != null && (this.Area as SfChart3D).Series.Count > 0 && (this.Orientation.Equals((object) Orientation.Vertical) && !area.Series[0].IsActualTransposed || this.Orientation.Equals((object) Orientation.Horizontal) && area.Series[0].IsActualTransposed) ? NumericalPadding.Round : (NumericalPadding) this.GetValue(NumericalAxis3D.RangePaddingProperty);
    }
  }

  protected internal override double CalculateActualInterval(DoubleRange range, Size availableSize)
  {
    return !this.Interval.HasValue ? this.CalculateNiceInterval(range, availableSize) : this.Interval.Value;
  }

  protected override void GenerateVisibleLabels()
  {
    NumericalAxisHelper.GenerateVisibleLabels3D((ChartAxis) this, (object) this.Minimum, (object) this.Maximum, (object) this.Interval, (double) this.SmallTicksPerInterval);
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
    if (this.ActualRange.IsEmpty)
      return base.CalculateActualRange();
    if (this.Minimum.HasValue && this.Maximum.HasValue)
      return this.ActualRange;
    DoubleRange actualRange = base.CalculateActualRange();
    if (this.StartRangeFromZero && actualRange.Start > 0.0)
      return new DoubleRange(0.0, actualRange.End);
    if (this.Minimum.HasValue)
      return new DoubleRange(this.ActualRange.Start, double.IsNaN(actualRange.End) ? this.ActualRange.Start + 1.0 : actualRange.End);
    return this.Maximum.HasValue ? new DoubleRange(double.IsNaN(actualRange.Start) ? this.ActualRange.End - 1.0 : actualRange.Start, this.ActualRange.End) : actualRange;
  }

  protected override DoubleRange ApplyRangePadding(DoubleRange range, double interval)
  {
    if (!this.Minimum.HasValue && !this.Maximum.HasValue)
      return NumericalAxisHelper.ApplyRangePadding((ChartAxis) this, base.ApplyRangePadding(range, interval), interval, this.ActualRangePadding);
    if (this.Minimum.HasValue && this.Maximum.HasValue)
      return range;
    DoubleRange doubleRange = NumericalAxisHelper.ApplyRangePadding((ChartAxis) this, base.ApplyRangePadding(range, interval), interval, this.ActualRangePadding);
    return !this.Minimum.HasValue ? new DoubleRange(doubleRange.Start, range.End) : new DoubleRange(range.Start, doubleRange.End);
  }

  protected override DependencyObject CloneAxis(DependencyObject obj)
  {
    obj = (DependencyObject) new NumericalAxis3D()
    {
      Minimum = this.Minimum,
      Maximum = this.Maximum,
      StartRangeFromZero = this.StartRangeFromZero,
      Interval = this.Interval,
      RangePadding = this.RangePadding
    };
    return base.CloneAxis(obj);
  }

  private static void OnPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    if (!(d is NumericalAxis3D numericalAxis3D))
      return;
    numericalAxis3D.OnPropertyChanged();
  }

  private static void OnMinimumChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    ((NumericalAxis3D) d).OnMinimumChanged(e);
  }

  private static void OnMaximumChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    ((NumericalAxis3D) d).OnMaximumChanged(e);
  }

  private static void OnIntervalChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    ((NumericalAxis3D) d).OnIntervalChanged(e);
  }

  private void OnMinMaxChanged()
  {
    NumericalAxisHelper.OnMinMaxChanged((ChartAxis) this, (object) this.Maximum, (object) this.Minimum);
  }
}
