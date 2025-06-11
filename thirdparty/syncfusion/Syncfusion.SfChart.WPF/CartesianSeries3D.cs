// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.CartesianSeries3D
// Assembly: Syncfusion.SfChart.WPF, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 279D70C6-7936-4F3E-B6CF-79EFEF734826
// Assembly location: C:\Program Files\PDFgear\Syncfusion.SfChart.WPF.dll

using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

#nullable disable
namespace Syncfusion.UI.Xaml.Charts;

public abstract class CartesianSeries3D : ChartSeries3D, ISupportAxes3D, ISupportAxes
{
  public static readonly DependencyProperty IsTransposedProperty = DependencyProperty.Register(nameof (IsTransposed), typeof (bool), typeof (CartesianSeries3D), new PropertyMetadata((object) false, new PropertyChangedCallback(CartesianSeries3D.OnTransposeChanged)));
  internal static readonly DependencyProperty XAxisProperty = DependencyProperty.Register(nameof (XAxis), typeof (ChartAxisBase3D), typeof (CartesianSeries3D), new PropertyMetadata((object) null, new PropertyChangedCallback(CartesianSeries3D.OnXAxisChanged)));
  internal static readonly DependencyProperty YAxisProperty = DependencyProperty.Register(nameof (YAxis), typeof (RangeAxisBase3D), typeof (CartesianSeries3D), new PropertyMetadata((object) null, new PropertyChangedCallback(CartesianSeries3D.OnYAxisChanged)));

  public DoubleRange XRange { get; internal set; }

  public DoubleRange YRange { get; internal set; }

  public bool IsTransposed
  {
    get => (bool) this.GetValue(CartesianSeries3D.IsTransposedProperty);
    set => this.SetValue(CartesianSeries3D.IsTransposedProperty, (object) value);
  }

  ChartAxis ISupportAxes.ActualXAxis => this.ActualXAxis;

  ChartAxis ISupportAxes.ActualYAxis => this.ActualYAxis;

  internal ChartAxisBase3D XAxis
  {
    get => (ChartAxisBase3D) this.GetValue(CartesianSeries3D.XAxisProperty);
    set => this.SetValue(CartesianSeries3D.XAxisProperty, (object) value);
  }

  internal RangeAxisBase3D YAxis
  {
    get => (RangeAxisBase3D) this.GetValue(CartesianSeries3D.YAxisProperty);
    set => this.SetValue(CartesianSeries3D.YAxisProperty, (object) value);
  }

  internal override void UpdateRange()
  {
    this.XRange = DoubleRange.Empty;
    this.YRange = DoubleRange.Empty;
    bool flag1 = this is XyzDataSeries3D xyzDataSeries3D && !string.IsNullOrEmpty(xyzDataSeries3D.ZBindingPath);
    if (flag1)
      xyzDataSeries3D.ZRange = DoubleRange.Empty;
    foreach (ChartSegment segment in (Collection<ChartSegment>) this.Segments)
    {
      this.XRange += segment.XRange;
      this.YRange += segment.YRange;
      if (flag1)
        xyzDataSeries3D.ZRange += (segment as ChartSegment3D).ZRange;
    }
    if (!this.IsSideBySide)
      return;
    DoubleRange sideInfoRangePad = this.SideBySideInfoRangePad;
    if (this.SideBySideInfoRangePad.IsEmpty)
      return;
    this.XRange = this.ActualXAxis is NumericalAxis3D && (this.ActualXAxis as NumericalAxis3D).RangePadding == NumericalPadding.None || this.ActualXAxis is DateTimeAxis3D && (this.ActualXAxis as DateTimeAxis3D).RangePadding == DateTimeRangePadding.None ? new DoubleRange(this.XRange.Start - this.SideBySideInfoRangePad.Start, this.XRange.End - this.SideBySideInfoRangePad.End) : new DoubleRange(this.XRange.Start + this.SideBySideInfoRangePad.Start, this.XRange.End + this.SideBySideInfoRangePad.End);
    if (!flag1)
      return;
    bool flag2 = xyzDataSeries3D.ActualZAxis is NumericalAxis3D && (xyzDataSeries3D.ActualZAxis as NumericalAxis3D).RangePadding == NumericalPadding.None || xyzDataSeries3D.ActualZAxis is DateTimeAxis3D && (xyzDataSeries3D.ActualZAxis as DateTimeAxis3D).RangePadding == DateTimeRangePadding.None;
    xyzDataSeries3D.ZRange = flag2 ? new DoubleRange(xyzDataSeries3D.ZRange.Start - xyzDataSeries3D.ZSideBySideInfoRangePad.Start, xyzDataSeries3D.ZRange.End - xyzDataSeries3D.ZSideBySideInfoRangePad.End) : new DoubleRange(xyzDataSeries3D.ZRange.Start + xyzDataSeries3D.ZSideBySideInfoRangePad.Start, xyzDataSeries3D.ZRange.End + xyzDataSeries3D.ZSideBySideInfoRangePad.End);
  }

  protected virtual void OnYAxisChanged(ChartAxis oldAxis, ChartAxis newAxis)
  {
    if (newAxis != null && !newAxis.RegisteredSeries.Contains((ISupportAxes) this))
    {
      if (this.Area != null && !this.Area.Axes.Contains(newAxis))
        this.Area.Axes.Add(newAxis);
      newAxis.Area = (ChartBase) this.Area;
      newAxis.Orientation = Orientation.Vertical;
      newAxis.RegisteredSeries.Add((ISupportAxes) this);
    }
    if (oldAxis != null && oldAxis.RegisteredSeries != null)
    {
      if (oldAxis.RegisteredSeries.Contains((ISupportAxes) this))
        oldAxis.RegisteredSeries.Remove((ISupportAxes) this);
      if (this.Area != null && oldAxis.RegisteredSeries.Count == 0 && this.Area.Axes.Contains(oldAxis) && this.Area.InternalPrimaryAxis != oldAxis && this.Area.InternalSecondaryAxis != oldAxis)
        this.Area.Axes.Remove(oldAxis);
    }
    if (this.Area != null)
      this.Area.ScheduleUpdate();
    if (newAxis == null)
      return;
    newAxis.Orientation = this.IsActualTransposed ? Orientation.Horizontal : Orientation.Vertical;
  }

  protected virtual void OnXAxisChanged(ChartAxis oldAxis, ChartAxis newAxis)
  {
    if (oldAxis != null && oldAxis.RegisteredSeries != null)
    {
      oldAxis.VisibleRangeChanged -= new EventHandler<VisibleRangeChangedEventArgs>(this.OnVisibleRangeChanged);
      if (oldAxis.RegisteredSeries.Contains((ISupportAxes) this))
        oldAxis.RegisteredSeries.Remove((ISupportAxes) this);
      if (this.Area != null && oldAxis.RegisteredSeries.Count == 0 && this.Area.Axes.Contains(oldAxis) && this.Area.InternalPrimaryAxis != oldAxis && this.Area.InternalSecondaryAxis != oldAxis)
        this.Area.Axes.Remove(oldAxis);
    }
    if (newAxis != null)
    {
      if (this.Area != null && !this.Area.Axes.Contains(newAxis) && newAxis != this.Area.InternalPrimaryAxis)
        this.Area.Axes.Add(newAxis);
      newAxis.Area = (ChartBase) this.Area;
      newAxis.Orientation = Orientation.Horizontal;
      if (!newAxis.RegisteredSeries.Contains((ISupportAxes) this))
        newAxis.RegisteredSeries.Add((ISupportAxes) this);
      newAxis.VisibleRangeChanged += new EventHandler<VisibleRangeChangedEventArgs>(this.OnVisibleRangeChanged);
    }
    if (this.Area != null)
      this.Area.ScheduleUpdate();
    if (newAxis == null)
      return;
    newAxis.Orientation = this.IsActualTransposed ? Orientation.Vertical : Orientation.Horizontal;
  }

  protected virtual void OnVisibleRangeChanged(VisibleRangeChangedEventArgs e)
  {
  }

  protected override DependencyObject CloneSeries(DependencyObject obj)
  {
    if (obj is CartesianSeries)
      (obj as CartesianSeries3D).IsTransposed = this.IsTransposed;
    return base.CloneSeries(obj);
  }

  private static void OnTransposeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    (d as CartesianSeries3D).OnTransposeChanged((bool) e.NewValue);
  }

  private static void OnXAxisChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    ((CartesianSeries3D) d).OnXAxisChanged(e.OldValue as ChartAxis, e.NewValue as ChartAxis);
  }

  private static void OnYAxisChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    ((CartesianSeries3D) d).OnYAxisChanged(e.OldValue as ChartAxis, e.NewValue as ChartAxis);
  }

  private void OnVisibleRangeChanged(object sender, VisibleRangeChangedEventArgs e)
  {
    this.OnVisibleRangeChanged(e);
  }
}
