// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.RangeAxisBase3D
// Assembly: Syncfusion.SfChart.WPF, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 279D70C6-7936-4F3E-B6CF-79EFEF734826
// Assembly location: C:\Program Files\PDFgear\Syncfusion.SfChart.WPF.dll

using System.Windows;

#nullable disable
namespace Syncfusion.UI.Xaml.Charts;

public abstract class RangeAxisBase3D : ChartAxisBase3D, IRangeAxis
{
  public static readonly DependencyProperty SmallTicksPerIntervalProperty = DependencyProperty.Register(nameof (SmallTicksPerInterval), typeof (int), typeof (RangeAxisBase3D), new PropertyMetadata((object) 0, new PropertyChangedCallback(RangeAxisBase3D.OnSmallTicksPerIntervalPropertyChanged)));
  public static readonly DependencyProperty SmallTickLineSizeProperty = DependencyProperty.Register(nameof (SmallTickLineSize), typeof (double), typeof (RangeAxisBase3D), new PropertyMetadata((object) 5.0));
  public static readonly DependencyProperty SmallTickLinesPositionProperty = DependencyProperty.Register(nameof (SmallTickLinesPosition), typeof (AxisElementPosition), typeof (RangeAxisBase3D), new PropertyMetadata((object) AxisElementPosition.Outside));

  public int SmallTicksPerInterval
  {
    get => (int) this.GetValue(RangeAxisBase3D.SmallTicksPerIntervalProperty);
    set => this.SetValue(RangeAxisBase3D.SmallTicksPerIntervalProperty, (object) value);
  }

  public double SmallTickLineSize
  {
    get => (double) this.GetValue(RangeAxisBase3D.SmallTickLineSizeProperty);
    set => this.SetValue(RangeAxisBase3D.SmallTickLineSizeProperty, (object) value);
  }

  public AxisElementPosition SmallTickLinesPosition
  {
    get => (AxisElementPosition) this.GetValue(RangeAxisBase3D.SmallTickLinesPositionProperty);
    set => this.SetValue(RangeAxisBase3D.SmallTickLinesPositionProperty, (object) value);
  }

  DoubleRange IRangeAxis.Range => this.ActualRange;

  protected DoubleRange Range => this.ActualRange;

  protected internal override void AddSmallTicksPoint(double position)
  {
    RangeAxisBaseHelper.AddSmallTicksPoint((ChartAxis) this, position, this.VisibleInterval, this.SmallTicksPerInterval);
  }

  protected internal override void AddSmallTicksPoint(double position, double interval)
  {
    RangeAxisBaseHelper.AddSmallTicksPoint((ChartAxis) this, position, interval, this.SmallTicksPerInterval);
  }

  protected override void GenerateVisibleLabels()
  {
    RangeAxisBaseHelper.GenerateVisibleLabels((ChartAxis) this, this.SmallTickLineSize);
  }

  protected override DependencyObject CloneAxis(DependencyObject obj)
  {
    RangeAxisBase3D rangeAxisBase3D = (RangeAxisBase3D) obj;
    rangeAxisBase3D.SmallTicksPerInterval = this.SmallTicksPerInterval;
    rangeAxisBase3D.SmallTickLinesPosition = this.SmallTickLinesPosition;
    rangeAxisBase3D.SmallTickLineSize = this.SmallTickLineSize;
    return base.CloneAxis(obj);
  }

  private static void OnSmallTicksPerIntervalPropertyChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    if (!(d is RangeAxisBase3D rangeAxisBase3D))
      return;
    rangeAxisBase3D.smallTicksRequired = (int) e.NewValue > 0 || rangeAxisBase3D.smallTicksRequired;
  }
}
