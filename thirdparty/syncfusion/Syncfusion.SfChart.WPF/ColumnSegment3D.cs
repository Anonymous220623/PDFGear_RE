// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.ColumnSegment3D
// Assembly: Syncfusion.SfChart.WPF, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 279D70C6-7936-4F3E-B6CF-79EFEF734826
// Assembly location: C:\Program Files\PDFgear\Syncfusion.SfChart.WPF.dll

using System;
using System.Windows;
using System.Windows.Shapes;

#nullable disable
namespace Syncfusion.UI.Xaml.Charts;

public class ColumnSegment3D : ChartSegment3D
{
  public static readonly DependencyProperty TopProperty = DependencyProperty.Register(nameof (Top), typeof (double), typeof (ColumnSegment3D), new PropertyMetadata((object) 0.0, new PropertyChangedCallback(ColumnSegment3D.OnValueChanged)));
  public static readonly DependencyProperty BottomProperty = DependencyProperty.Register(nameof (Bottom), typeof (double), typeof (ColumnSegment3D), new PropertyMetadata((object) 0.0, new PropertyChangedCallback(ColumnSegment3D.OnValueChanged)));
  private double rectX;
  private double rectY;
  private double width;
  private double height;
  private double internaltop;
  private Polygon3D[] plans;
  private double internalBottom;

  public ColumnSegment3D(
    double x1,
    double y1,
    double x2,
    double y2,
    double startDepth,
    double endDepth,
    ChartSeriesBase series)
  {
    this.Left = 0.0;
    this.Right = 0.0;
    this.Series = series;
    this.SetData(x1, y1, x2, y2, startDepth, endDepth);
  }

  public ColumnSegment3D(
    double x1,
    double y1,
    double x2,
    double y2,
    double startDepth,
    double endDepth)
  {
    this.Left = 0.0;
    this.Right = 0.0;
    this.SetData(x1, y1, x2, y2, startDepth, endDepth);
  }

  public double XData { get; internal set; }

  public double YData { get; internal set; }

  public double ZData { get; internal set; }

  public double Width
  {
    get => this.width;
    set
    {
      this.width = value;
      this.OnPropertyChanged(nameof (Width));
    }
  }

  public double Height
  {
    get => this.height;
    set
    {
      this.height = value;
      this.OnPropertyChanged(nameof (Height));
    }
  }

  public double RectX
  {
    get => this.rectX;
    set
    {
      this.rectX = value;
      this.OnPropertyChanged(nameof (RectX));
    }
  }

  public double RectY
  {
    get => this.rectY;
    set
    {
      this.rectY = value;
      this.OnPropertyChanged(nameof (RectY));
    }
  }

  public double Top
  {
    get => (double) this.GetValue(ColumnSegment3D.TopProperty);
    set => this.SetValue(ColumnSegment3D.TopProperty, (object) value);
  }

  public double Bottom
  {
    get => (double) this.GetValue(ColumnSegment3D.BottomProperty);
    set => this.SetValue(ColumnSegment3D.BottomProperty, (object) value);
  }

  public double InternalBottom
  {
    get => this.internalBottom;
    set => this.internalBottom = value;
  }

  internal Polygon3D[] Plans
  {
    get => this.plans;
    set => this.plans = value;
  }

  internal double InternalTop
  {
    get => this.internaltop;
    set => this.internaltop = value;
  }

  protected double Left { get; set; }

  protected double Right { get; set; }

  public override void SetData(params double[] values)
  {
    this.Plans = (Polygon3D[]) null;
    this.Left = values[0];
    this.internalBottom = this.Bottom = values[3];
    this.internaltop = this.Top = values[1];
    this.Right = values[2];
    this.startDepth = values[4];
    this.endDepth = values[5];
    this.XRange = new DoubleRange(this.Left, this.Right);
    this.ZRange = new DoubleRange(this.startDepth, this.endDepth);
    if (double.IsNaN(this.Top) || double.IsNaN(this.Bottom))
      return;
    this.YRange = new DoubleRange(this.Top, this.Bottom);
  }

  public override UIElement CreateVisual(Size size) => (UIElement) null;

  public override UIElement GetRenderedVisual() => (UIElement) null;

  public override void Update(IChartTransformer transformer)
  {
    if (transformer == null || !(transformer is ChartTransform.ChartCartesianTransformer cartesianTransformer) || double.IsNaN(this.YData) && !this.Series.ShowEmptyPoints)
      return;
    double newBase1 = cartesianTransformer.XAxis.IsLogarithmic ? ((LogarithmicAxis3D) cartesianTransformer.XAxis).LogarithmicBase : 1.0;
    bool isLogarithmic1 = cartesianTransformer.XAxis.IsLogarithmic;
    double num1 = isLogarithmic1 ? Math.Log(this.Left, newBase1) : this.Left;
    double num2 = isLogarithmic1 ? Math.Log(this.Right, newBase1) : this.Right;
    double x = cartesianTransformer.YAxis.IsLogarithmic ? ((LogarithmicAxis3D) cartesianTransformer.YAxis).LogarithmicBase : 1.0;
    bool isLogarithmic2 = cartesianTransformer.YAxis.IsLogarithmic;
    double num3 = isLogarithmic2 ? Math.Pow(x, cartesianTransformer.YAxis.VisibleRange.Start) : cartesianTransformer.YAxis.VisibleRange.Start;
    double num4 = isLogarithmic2 ? Math.Pow(x, cartesianTransformer.YAxis.VisibleRange.End) : cartesianTransformer.YAxis.VisibleRange.End;
    double start1 = cartesianTransformer.XAxis.VisibleRange.Start;
    double end1 = cartesianTransformer.XAxis.VisibleRange.End;
    double startDepth = this.startDepth;
    double endDepth = this.endDepth;
    XyzDataSeries3D series = this.Series as XyzDataSeries3D;
    bool flag = cartesianTransformer.ZAxis != null && series.ActualZAxis != null && series.ActualZValues != null;
    double segmentSpacing1 = (this.Series as ISegmentSpacing).SegmentSpacing;
    if (num1 > end1 || num2 < start1)
      return;
    double num5 = this.Top >= 0.0 ? (this.Top < num4 ? this.Top : num4) : (this.Top > num3 ? this.Top : num3);
    this.Bottom = this.Bottom > num4 ? num4 : this.Bottom;
    if (segmentSpacing1 > 0.0 && segmentSpacing1 <= 1.0)
    {
      double segmentSpacing2 = (this.Series as ISegmentSpacing).CalculateSegmentSpacing(segmentSpacing1, num2, num1);
      double segmentSpacing3 = (this.Series as ISegmentSpacing).CalculateSegmentSpacing(segmentSpacing1, num1, num2);
      this.Left = segmentSpacing2;
      this.Right = segmentSpacing3;
    }
    SfChart3D actualArea = this.Series.ActualArea as SfChart3D;
    Vector3D vector1 = new Vector3D(0.0, 0.0, 0.0);
    Vector3D vector2 = new Vector3D(0.0, 0.0, 0.0);
    Point visible1 = transformer.TransformToVisible(num1 > start1 ? this.Left : start1, num5 < num3 ? num3 : num5);
    Point visible2 = transformer.TransformToVisible(end1 > num2 ? this.Right : end1, num3 > this.Bottom ? num3 : this.Bottom);
    if (flag)
    {
      double start2 = cartesianTransformer.ZAxis.VisibleRange.Start;
      double end2 = cartesianTransformer.ZAxis.VisibleRange.End;
      bool isLogarithmic3 = cartesianTransformer.ZAxis.IsLogarithmic;
      double newBase2 = isLogarithmic3 ? ((LogarithmicAxis3D) cartesianTransformer.ZAxis).LogarithmicBase : 1.0;
      double num6 = isLogarithmic3 ? Math.Log(startDepth, newBase2) : startDepth;
      double num7 = isLogarithmic3 ? Math.Log(endDepth, newBase2) : endDepth;
      if (num6 > end2 || num7 < start2)
        return;
      vector1 = cartesianTransformer.TransformToVisible3D(this.Left > start1 ? this.Left : start1, num5 < num3 ? num3 : num5, startDepth > start2 ? startDepth : start2);
      vector2 = cartesianTransformer.TransformToVisible3D(end1 > this.Right ? this.Right : end1, num3 > this.Bottom ? num3 : this.Bottom, end2 > endDepth ? endDepth : end2);
    }
    else
    {
      Rect rect = new Rect(visible1, visible2);
      vector1 = new Vector3D(rect.Left, rect.Top, startDepth);
      vector2 = new Vector3D(rect.Right, rect.Bottom, endDepth);
    }
    if (this.plans == null)
      this.plans = Polygon3D.CreateBox(vector1, vector2, (DependencyObject) this, this.Series.Segments.IndexOf((ChartSegment) this), actualArea.Graphics3D, this.Stroke, this.Interior, this.StrokeThickness, this.Series.IsActualTransposed);
    else
      Polygon3D.UpdateBox(this.plans, vector1, vector2, this.Interior, visible1.Y == visible2.Y ? Visibility.Collapsed : Visibility.Visible);
  }

  public override void OnSizeChanged(Size size)
  {
  }

  protected override void SetVisualBindings(Shape element) => base.SetVisualBindings(element);

  private static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs args)
  {
    if (!(d is ColumnSegment3D columnSegment3D))
      return;
    columnSegment3D.OnValueChanged();
  }

  private void OnValueChanged()
  {
    if (this.Series == null || !this.Series.GetAnimationIsActive() || this.Series.ActualArea == null || (this.Series.ActualArea as SfChart3D).IsRotationScheduleUpdate)
      return;
    this.Update(this.Series.CreateTransformer(new Size(), false));
  }
}
