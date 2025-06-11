// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.ScatterSegment3D
// Assembly: Syncfusion.SfChart.WPF, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 279D70C6-7936-4F3E-B6CF-79EFEF734826
// Assembly location: C:\Program Files\PDFgear\Syncfusion.SfChart.WPF.dll

using System;
using System.Windows;
using System.Windows.Data;
using System.Windows.Shapes;

#nullable disable
namespace Syncfusion.UI.Xaml.Charts;

public class ScatterSegment3D : ChartSegment3D
{
  public static readonly DependencyProperty ScatterWidthProperty = DependencyProperty.Register(nameof (ScatterWidth), typeof (double), typeof (ScatterSegment3D), new PropertyMetadata((object) double.NaN, new PropertyChangedCallback(ScatterSegment3D.OnValueChanged)));
  public static readonly DependencyProperty ScatterHeightProperty = DependencyProperty.Register(nameof (ScatterHeight), typeof (double), typeof (ScatterSegment3D), new PropertyMetadata((object) double.NaN, new PropertyChangedCallback(ScatterSegment3D.OnValueChanged)));
  internal static readonly DependencyProperty XProperty = DependencyProperty.Register(nameof (X), typeof (double), typeof (ScatterSegment3D), new PropertyMetadata((object) 0.0, new PropertyChangedCallback(ScatterSegment3D.OnValueChanged)));
  internal static readonly DependencyProperty YProperty = DependencyProperty.Register(nameof (Y), typeof (double), typeof (ScatterSegment3D), new PropertyMetadata((object) 0.0, new PropertyChangedCallback(ScatterSegment3D.OnValueChanged)));
  private Polygon3D[] plans;

  public ScatterSegment3D(
    double x1,
    double y1,
    double startDepth,
    double endDepth,
    ScatterSeries3D scatterSeries3D)
  {
    this.Series = (ChartSeriesBase) scatterSeries3D;
    this.SetData(x1, y1, startDepth, endDepth);
  }

  public double ScatterWidth
  {
    get => (double) this.GetValue(ScatterSegment3D.ScatterWidthProperty);
    set => this.SetValue(ScatterSegment3D.ScatterWidthProperty, (object) value);
  }

  public double ScatterHeight
  {
    get => (double) this.GetValue(ScatterSegment3D.ScatterHeightProperty);
    set => this.SetValue(ScatterSegment3D.ScatterHeightProperty, (object) value);
  }

  public double XData { get; internal set; }

  public double YData { get; internal set; }

  public double ZData { get; internal set; }

  internal Polygon3D[] Plans
  {
    get => this.plans;
    set => this.plans = value;
  }

  internal double X
  {
    get => (double) this.GetValue(ScatterSegment3D.XProperty);
    set => this.SetValue(ScatterSegment3D.XProperty, (object) value);
  }

  internal double Y
  {
    get => (double) this.GetValue(ScatterSegment3D.YProperty);
    set => this.SetValue(ScatterSegment3D.YProperty, (object) value);
  }

  public override void SetData(params double[] Values)
  {
    this.X = Values[0];
    this.Y = Values[1];
    this.startDepth = Values[2];
    this.endDepth = Values[3];
    if (!double.IsNaN(this.X))
      this.XRange = DoubleRange.Union(this.X);
    else
      this.XRange = DoubleRange.Empty;
    if (!double.IsNaN(this.Y))
      this.YRange = DoubleRange.Union(this.Y);
    else
      this.YRange = DoubleRange.Empty;
    this.ZRange = new DoubleRange(this.startDepth, this.endDepth);
  }

  public override UIElement CreateVisual(Size size)
  {
    this.SetVisualBindings((Shape) null);
    return (UIElement) null;
  }

  public override UIElement GetRenderedVisual() => (UIElement) null;

  public override void Update(IChartTransformer transformer)
  {
    if (transformer == null || !(transformer is ChartTransform.ChartCartesianTransformer cartesianTransformer) || double.IsNaN(this.YData) && !this.Series.ShowEmptyPoints)
      return;
    ScatterSeries3D series1 = this.Series as ScatterSeries3D;
    double newBase1 = cartesianTransformer.XAxis.IsLogarithmic ? ((LogarithmicAxis3D) cartesianTransformer.XAxis).LogarithmicBase : 1.0;
    double newBase2 = cartesianTransformer.YAxis.IsLogarithmic ? ((LogarithmicAxis3D) cartesianTransformer.YAxis).LogarithmicBase : 1.0;
    bool isLogarithmic1 = cartesianTransformer.XAxis.IsLogarithmic;
    bool isLogarithmic2 = cartesianTransformer.YAxis.IsLogarithmic;
    double x1 = isLogarithmic1 ? Math.Log(this.X, newBase1) : this.X;
    double y1 = isLogarithmic2 ? Math.Log(this.Y, newBase2) : this.Y;
    double start1 = cartesianTransformer.YAxis.VisibleRange.Start;
    double end1 = cartesianTransformer.YAxis.VisibleRange.End;
    double start2 = cartesianTransformer.XAxis.VisibleRange.Start;
    double end2 = cartesianTransformer.XAxis.VisibleRange.End;
    XyzDataSeries3D series2 = this.Series as XyzDataSeries3D;
    bool flag = cartesianTransformer.ZAxis != null && series2.ActualZAxis != null && series2.ActualZValues != null;
    Point point = new Point(0.0, 0.0);
    double vz1 = 0.0;
    double vz2 = 0.0;
    if (flag)
    {
      double startDepth = this.startDepth;
      double endDepth = this.endDepth;
      double num1 = (this.Series as ScatterSeries3D).GetSegmentDepth((this.Series.ActualArea as SfChart3D).ActualDepth).Delta / 2.0;
      double start3 = cartesianTransformer.ZAxis.VisibleRange.Start;
      double end3 = cartesianTransformer.ZAxis.VisibleRange.End;
      bool isLogarithmic3 = cartesianTransformer.ZAxis.IsLogarithmic;
      double num2 = isLogarithmic3 ? ((LogarithmicAxis3D) cartesianTransformer.ZAxis).LogarithmicBase : 1.0;
      double num3 = isLogarithmic3 ? Math.Log(startDepth, num2) : startDepth;
      double num4 = isLogarithmic3 ? Math.Log(endDepth, num2) : endDepth;
      if (num3 > end3 || num4 < start3)
        return;
      double num5 = isLogarithmic3 ? Math.Pow(num2, start3) : start3;
      double num6 = isLogarithmic3 ? Math.Pow(num2, end3) : end3;
      Vector3D visible3D = cartesianTransformer.TransformToVisible3D(x1, y1, num3 < start3 ? num5 : (num3 > end3 ? num6 : startDepth));
      point = new Point(visible3D.X, visible3D.Y);
      vz1 = startDepth == start3 ? visible3D.Z : (visible3D.Z - this.ScatterHeight / 2.0 < visible3D.Z - num1 ? visible3D.Z - num1 : visible3D.Z - this.ScatterHeight / 2.0);
      vz2 = endDepth == end3 ? visible3D.Z : (visible3D.Z + this.ScatterHeight / 2.0 > visible3D.Z + num1 ? visible3D.Z + num1 : visible3D.Z + this.ScatterHeight / 2.0);
    }
    else
      point = transformer.TransformToVisible(this.X, this.Y);
    if (!series1.Area.SeriesClipRect.Contains(point) && (x1 != end2 && x1 != start2 || y1 != end1 && y1 != start1) && !series1.IsTransposed || series1.ScatterHeight <= 0.0 || series1.ScatterWidth <= 0.0)
      return;
    double x2 = x1 == (series1.IsTransposed ? end2 : start2) ? point.X : point.X - (series1.IsTransposed ? this.ScatterHeight / 2.0 : this.ScatterWidth / 2.0);
    double y2 = y1 == (series1.IsTransposed ? start1 : end1) ? point.Y : point.Y - (series1.IsTransposed ? this.ScatterWidth / 2.0 : this.ScatterHeight / 2.0);
    double num7 = x1 == start2 || x1 == end2 ? this.ScatterWidth / 2.0 : this.ScatterWidth;
    double num8 = y1 == start1 || y1 == end1 ? this.ScatterHeight / 2.0 : this.ScatterHeight;
    Rect rect = new Rect(x2, y2, num7, num8);
    if (!series1.IsTransposed)
    {
      if (!this.Series.ActualArea.SeriesClipRect.Contains(new Point(x2, y2)))
        rect = new Rect(x2, this.Series.ActualArea.SeriesClipRect.Top, num7, num8 + y2);
      if (!this.Series.ActualArea.SeriesClipRect.Contains(new Point(rect.Left, rect.Bottom)))
        rect = new Rect(x2, y2, num7, Math.Abs(num8 + (this.Series.ActualArea.SeriesClipRect.Bottom - rect.Bottom) > this.ScatterHeight ? num8 : num8 + (this.Series.ActualArea.SeriesClipRect.Bottom - rect.Bottom)));
      if (!this.Series.ActualArea.SeriesClipRect.Contains(new Point(rect.Left, rect.Top)))
      {
        double width = Math.Abs(num7 - (this.Series.ActualXAxis.RenderedRect.X - rect.X));
        rect = new Rect(this.Series.ActualXAxis.RenderedRect.X, y2, width, rect.Height);
      }
      if (!this.Series.ActualArea.SeriesClipRect.Contains(new Point(rect.Left + rect.Width, rect.Top)))
      {
        double width = Math.Abs(rect.Width + (this.Series.ActualArea.SeriesClipRect.Right - rect.Right));
        rect = new Rect(rect.X, rect.Y, width, rect.Height);
      }
    }
    else
    {
      rect = new Rect(x2, y2, num8, num7);
      if (x2 < point.X && x1 == start2 && y1 == start1)
        rect = new Rect(point.X, y2, num8, num7);
      if (y2 < point.Y && y1 == start1)
        rect = new Rect(point.X, point.Y, num8, num7);
      if (y2 == point.Y && y1 == start1)
        rect = new Rect(point.X, point.Y - this.ScatterWidth / 2.0, num8, num7);
      if (y2 < point.Y && x1 == end2)
        rect = new Rect(point.X, point.Y, num8, num7);
      if (x2 == point.X && x1 == end2)
        rect = new Rect(point.X, point.Y, num8, num7);
      if (x2 == point.X && x1 == end2 && y1 != start1)
        rect = new Rect(point.X - this.ScatterHeight / 2.0, point.Y, num8, num7);
    }
    SfChart3D actualArea = this.Series.ActualArea as SfChart3D;
    Vector3D vector1 = new Vector3D(0.0, 0.0, 0.0);
    Vector3D vector2 = new Vector3D(0.0, 0.0, 0.0);
    if (flag)
    {
      vector1 = new Vector3D(rect.Left, rect.Top, vz1);
      vector2 = new Vector3D(rect.Right, rect.Bottom, vz2);
    }
    else
    {
      vector1 = new Vector3D(rect.Left, rect.Top, this.startDepth);
      vector2 = new Vector3D(rect.Right, rect.Bottom, this.startDepth + this.ScatterHeight > this.endDepth ? this.endDepth : this.startDepth + this.ScatterHeight);
    }
    if (this.plans == null)
      this.plans = Polygon3D.CreateBox(vector1, vector2, (DependencyObject) this, series1.Segments.IndexOf((ChartSegment) this), actualArea.Graphics3D, this.Stroke, this.Interior, this.StrokeThickness, this.Series.IsActualTransposed);
    else
      Polygon3D.UpdateBox(this.plans, vector1, vector2, this.Interior, Visibility.Visible);
  }

  public override void OnSizeChanged(Size size)
  {
  }

  protected override void SetVisualBindings(Shape element)
  {
    BindingOperations.SetBinding((DependencyObject) this, ScatterSegment3D.ScatterWidthProperty, (BindingBase) new Binding()
    {
      Source = (object) this.Series,
      Path = new PropertyPath("ScatterWidth", new object[0])
    });
    BindingOperations.SetBinding((DependencyObject) this, ScatterSegment3D.ScatterHeightProperty, (BindingBase) new Binding()
    {
      Source = (object) this.Series,
      Path = new PropertyPath("ScatterHeight", new object[0])
    });
  }

  private static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs args)
  {
    if (!(d is ScatterSegment3D scatterSegment3D))
      return;
    scatterSegment3D.OnValueChanged();
  }

  private void OnValueChanged()
  {
    if (this.Series == null || !this.Series.GetAnimationIsActive())
      return;
    this.Update(this.Series.CreateTransformer(new Size(), false));
  }
}
