// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.RangeAxisBase
// Assembly: Syncfusion.SfChart.WPF, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 279D70C6-7936-4F3E-B6CF-79EFEF734826
// Assembly location: C:\Program Files\PDFgear\Syncfusion.SfChart.WPF.dll

using System.Windows;

#nullable disable
namespace Syncfusion.UI.Xaml.Charts;

public abstract class RangeAxisBase : ChartAxisBase2D, IRangeAxis
{
  public static readonly DependencyProperty IncludeAnnotationRangeProperty = DependencyProperty.Register(nameof (IncludeAnnotationRange), typeof (bool), typeof (RangeAxisBase), new PropertyMetadata((object) false, new PropertyChangedCallback(RangeAxisBase.OnIncludeAnnotationRangeChanged)));
  public static readonly DependencyProperty SmallTicksPerIntervalProperty = DependencyProperty.Register(nameof (SmallTicksPerInterval), typeof (int), typeof (ChartAxis), new PropertyMetadata((object) 0, new PropertyChangedCallback(RangeAxisBase.OnSmallTicksPerIntervalPropertyChanged)));
  public static readonly DependencyProperty SmallTickLineSizeProperty = DependencyProperty.Register(nameof (SmallTickLineSize), typeof (double), typeof (ChartAxis), new PropertyMetadata((object) 5.0, new PropertyChangedCallback(RangeAxisBase.OnSmallTicksPropertyChanged)));
  public static readonly DependencyProperty SmallTickLinesPositionProperty = DependencyProperty.Register(nameof (SmallTickLinesPosition), typeof (AxisElementPosition), typeof (ChartAxis), new PropertyMetadata((object) AxisElementPosition.Outside, new PropertyChangedCallback(RangeAxisBase.OnSmallTicksPropertyChanged)));

  public bool IncludeAnnotationRange
  {
    get => (bool) this.GetValue(RangeAxisBase.IncludeAnnotationRangeProperty);
    set => this.SetValue(RangeAxisBase.IncludeAnnotationRangeProperty, (object) value);
  }

  public int SmallTicksPerInterval
  {
    get => (int) this.GetValue(RangeAxisBase.SmallTicksPerIntervalProperty);
    set => this.SetValue(RangeAxisBase.SmallTicksPerIntervalProperty, (object) value);
  }

  public double SmallTickLineSize
  {
    get => (double) this.GetValue(RangeAxisBase.SmallTickLineSizeProperty);
    set => this.SetValue(RangeAxisBase.SmallTickLineSizeProperty, (object) value);
  }

  public AxisElementPosition SmallTickLinesPosition
  {
    get => (AxisElementPosition) this.GetValue(RangeAxisBase.SmallTickLinesPositionProperty);
    set => this.SetValue(RangeAxisBase.SmallTickLinesPositionProperty, (object) value);
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
    RangeAxisBase rangeAxisBase = obj as RangeAxisBase;
    rangeAxisBase.SmallTicksPerInterval = this.SmallTicksPerInterval;
    rangeAxisBase.SmallTickLinesPosition = this.SmallTickLinesPosition;
    rangeAxisBase.SmallTickLineSize = this.SmallTickLineSize;
    rangeAxisBase.IncludeAnnotationRange = this.IncludeAnnotationRange;
    return base.CloneAxis(obj);
  }

  private static void OnIncludeAnnotationRangeChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    if (!(d is ChartAxis chartAxis) || chartAxis.Area == null)
      return;
    chartAxis.Area.ScheduleUpdate();
  }

  private static void OnSmallTicksPropertyChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    if (!(d is ChartAxis chartAxis) || chartAxis.Area == null)
      return;
    chartAxis.Area.ScheduleUpdate();
  }

  private static void OnSmallTicksPerIntervalPropertyChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    if (!(d is ChartAxis chartAxis))
      return;
    chartAxis.smallTicksRequired = (int) e.NewValue > 0 || chartAxis.smallTicksRequired;
    if (chartAxis.Area == null)
      return;
    chartAxis.Area.ScheduleUpdate();
  }
}
