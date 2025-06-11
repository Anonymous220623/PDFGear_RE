// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.BoxAndWhiskerSegment
// Assembly: Syncfusion.SfChart.WPF, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 279D70C6-7936-4F3E-B6CF-79EFEF734826
// Assembly location: C:\Program Files\PDFgear\Syncfusion.SfChart.WPF.dll

using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Shapes;

#nullable disable
namespace Syncfusion.UI.Xaml.Charts;

public class BoxAndWhiskerSegment : ChartSegment
{
  private const int averagePathSize = 10;
  private Line lowerQuartileLine;
  private Line upperQuartileLine;
  private Line medianLine;
  private Line maximumLine;
  private Line minimumLine;
  private Canvas segmentCanvas;
  private Rectangle rectangle;
  private Path averagePath;
  private double average;

  public BoxAndWhiskerSegment() => this.Outliers = new List<double>();

  public BoxAndWhiskerSegment(ChartSeriesBase series)
  {
    this.Series = series;
    this.Outliers = new List<double>();
  }

  public Brush ActualStroke
  {
    get
    {
      ChartSeries series = this.Series as ChartSeries;
      return series.Stroke != null ? series.Stroke : ChartColorModifier.GetDarkenedColor(this.Interior, 0.6);
    }
  }

  public double Minimum { get; internal set; }

  public double Maximum { get; internal set; }

  public double Median { get; internal set; }

  public double LowerQuartile { get; internal set; }

  public double UppperQuartile { get; internal set; }

  internal double WhiskerWidth { get; set; }

  internal double Left { get; set; }

  internal double Right { get; set; }

  internal double Top { get; set; }

  internal double Bottom { get; set; }

  internal double Center { get; set; }

  internal List<double> Outliers { get; set; }

  public override void SetData(params double[] Values)
  {
    this.Left = Values[0];
    this.Right = Values[1];
    this.Top = Values[8];
    this.Bottom = Values[2];
    this.Minimum = Values[3];
    this.LowerQuartile = Values[4];
    this.Median = Values[5];
    this.UppperQuartile = Values[6];
    this.Maximum = Values[7];
    this.Center = Values[9];
    this.average = Values[10];
    this.XRange = new DoubleRange(this.Left, this.Right);
    this.YRange = new DoubleRange(this.Top, this.Bottom);
  }

  public override UIElement CreateVisual(Size size)
  {
    this.segmentCanvas = new Canvas();
    this.lowerQuartileLine = new Line();
    this.SetVisualBindings((Shape) this.lowerQuartileLine);
    this.segmentCanvas.Children.Add((UIElement) this.lowerQuartileLine);
    this.lowerQuartileLine.Tag = (object) this;
    this.upperQuartileLine = new Line();
    this.SetVisualBindings((Shape) this.upperQuartileLine);
    this.segmentCanvas.Children.Add((UIElement) this.upperQuartileLine);
    this.upperQuartileLine.Tag = (object) this;
    this.medianLine = new Line();
    this.SetVisualBindings((Shape) this.medianLine);
    this.medianLine.Tag = (object) this;
    Panel.SetZIndex((UIElement) this.medianLine, 1);
    this.segmentCanvas.Children.Add((UIElement) this.medianLine);
    this.maximumLine = new Line();
    this.SetVisualBindings((Shape) this.maximumLine);
    this.maximumLine.Tag = (object) this;
    this.segmentCanvas.Children.Add((UIElement) this.maximumLine);
    this.minimumLine = new Line();
    this.SetVisualBindings((Shape) this.minimumLine);
    this.minimumLine.Tag = (object) this;
    this.segmentCanvas.Children.Add((UIElement) this.minimumLine);
    this.rectangle = new Rectangle();
    this.rectangle.Tag = (object) this;
    this.SetVisualBindings((Shape) this.rectangle);
    this.segmentCanvas.Children.Add((UIElement) this.rectangle);
    this.averagePath = new Path();
    this.averagePath.Tag = (object) this;
    this.averagePath.Stretch = Stretch.Fill;
    this.DrawPathGeometry();
    this.SetVisualBindings((Shape) this.averagePath);
    return (UIElement) this.segmentCanvas;
  }

  public override UIElement GetRenderedVisual() => (UIElement) this.segmentCanvas;

  public override void OnSizeChanged(Size size)
  {
  }

  public override void Update(IChartTransformer transformer)
  {
    if (double.IsNaN(this.Minimum) && double.IsNaN(this.LowerQuartile) && double.IsNaN(this.Median) && double.IsNaN(this.UppperQuartile) && double.IsNaN(this.Maximum))
      return;
    ChartTransform.ChartCartesianTransformer cartesianTransformer = transformer as ChartTransform.ChartCartesianTransformer;
    double num1 = Math.Floor(cartesianTransformer.XAxis.VisibleRange.Start);
    double num2 = Math.Ceiling(cartesianTransformer.XAxis.VisibleRange.End);
    double newBase = cartesianTransformer.XAxis.IsLogarithmic ? (cartesianTransformer.XAxis as LogarithmicAxis).LogarithmicBase : 1.0;
    bool isLogarithmic = cartesianTransformer.XAxis.IsLogarithmic;
    double num3 = isLogarithmic ? Math.Log(this.Left, newBase) : this.Left;
    double num4 = isLogarithmic ? Math.Log(this.Right, newBase) : this.Right;
    if (num3 <= num2 && num4 >= num1)
    {
      this.lowerQuartileLine.Visibility = Visibility.Visible;
      this.upperQuartileLine.Visibility = Visibility.Visible;
      this.medianLine.Visibility = Visibility.Visible;
      this.minimumLine.Visibility = Visibility.Visible;
      this.maximumLine.Visibility = Visibility.Visible;
      this.rectangle.Visibility = Visibility.Visible;
      Point visible1 = transformer.TransformToVisible(this.Center, this.Minimum);
      Point visible2 = transformer.TransformToVisible(this.Center, this.LowerQuartile);
      Point visible3 = transformer.TransformToVisible(this.Center, this.Median);
      Point visible4 = transformer.TransformToVisible(this.Center, this.UppperQuartile);
      Point visible5 = transformer.TransformToVisible(this.Center, this.Maximum);
      Point visible6 = transformer.TransformToVisible(this.Left, this.UppperQuartile);
      Point visible7 = transformer.TransformToVisible(this.Right, this.LowerQuartile);
      double segmentSpacing = (this.Series as ISegmentSpacing).SegmentSpacing;
      this.rect = new Rect(visible6, visible7);
      if (this.Series.IsActualTransposed)
      {
        if (segmentSpacing > 0.0 && segmentSpacing <= 1.0)
        {
          this.rect.Y = (this.Series as ISegmentSpacing).CalculateSegmentSpacing(segmentSpacing, this.rect.Bottom, this.rect.Top);
          this.rect.Height = (1.0 - segmentSpacing) * this.rect.Height;
        }
        double num5 = this.rect.Height / 2.0 * this.WhiskerWidth;
        double num6 = this.rect.Y + this.rect.Height / 2.0;
        double num7 = num6 - num5;
        double num8 = num6 + num5;
        this.medianLine.X1 = visible3.X;
        this.medianLine.X2 = visible3.X;
        this.medianLine.Y1 = this.rect.Top;
        this.medianLine.Y2 = this.rect.Bottom;
        this.maximumLine.X1 = visible5.X;
        this.maximumLine.Y1 = num7;
        this.maximumLine.X2 = visible5.X;
        this.maximumLine.Y2 = num8;
        this.minimumLine.X1 = visible1.X;
        this.minimumLine.Y1 = num7;
        this.minimumLine.X2 = visible1.X;
        this.minimumLine.Y2 = num8;
      }
      else
      {
        if (segmentSpacing > 0.0 && segmentSpacing <= 1.0)
        {
          this.rect.X = (this.Series as ISegmentSpacing).CalculateSegmentSpacing(segmentSpacing, this.rect.Right, this.rect.Left);
          this.rect.Width = (1.0 - segmentSpacing) * this.rect.Width;
        }
        double num9 = this.rect.Width / 2.0 * this.WhiskerWidth;
        double num10 = this.rect.X + this.rect.Width / 2.0;
        double num11 = num10 - num9;
        double num12 = num10 + num9;
        this.medianLine.X1 = this.rect.Left;
        this.medianLine.X2 = this.rect.Right;
        this.medianLine.Y1 = visible3.Y;
        this.medianLine.Y2 = visible3.Y;
        this.maximumLine.X1 = num11;
        this.maximumLine.Y1 = visible5.Y;
        this.maximumLine.X2 = num12;
        this.maximumLine.Y2 = visible5.Y;
        this.minimumLine.X1 = num11;
        this.minimumLine.Y1 = visible1.Y;
        this.minimumLine.X2 = num12;
        this.minimumLine.Y2 = visible1.Y;
      }
      this.rectangle.Width = this.rect.Width;
      this.rectangle.Height = this.rect.Height;
      this.rectangle.SetValue(Canvas.LeftProperty, (object) this.rect.X);
      this.rectangle.SetValue(Canvas.TopProperty, (object) this.rect.Y);
      this.lowerQuartileLine.X1 = visible1.X;
      this.lowerQuartileLine.X2 = visible2.X;
      this.lowerQuartileLine.Y1 = visible1.Y;
      this.lowerQuartileLine.Y2 = visible2.Y;
      this.upperQuartileLine.X1 = visible4.X;
      this.upperQuartileLine.X2 = visible5.X;
      this.upperQuartileLine.Y1 = visible4.Y;
      this.upperQuartileLine.Y2 = visible5.Y;
      this.UpdateMeanSymbol((IChartTransformer) cartesianTransformer, (this.Series as BoxAndWhiskerSeries).ShowMedian);
    }
    else
    {
      this.medianLine.Visibility = Visibility.Collapsed;
      this.minimumLine.Visibility = Visibility.Collapsed;
      this.maximumLine.Visibility = Visibility.Collapsed;
      this.lowerQuartileLine.Visibility = Visibility.Collapsed;
      this.upperQuartileLine.Visibility = Visibility.Collapsed;
      this.rectangle.Visibility = Visibility.Collapsed;
      this.averagePath.Visibility = Visibility.Collapsed;
    }
  }

  internal void UpdateMeanSymbol(IChartTransformer cartesianTransformer, bool showMedian)
  {
    if (showMedian)
    {
      this.averagePath.Visibility = Visibility.Visible;
      Point visible = cartesianTransformer.TransformToVisible(this.Center, this.average);
      double num1 = 0.25;
      double num2;
      double num3;
      if (this.Series.IsActualTransposed)
      {
        double num4 = this.rect.Height * num1;
        double num5 = num4 > 10.0 ? 10.0 : num4;
        this.averagePath.Width = num5;
        this.averagePath.Height = num5;
        num2 = visible.X;
        num3 = this.rect.Top + this.rect.Height / 2.0;
      }
      else
      {
        double num6 = this.rect.Width * num1;
        double num7 = num6 > 10.0 ? 10.0 : num6;
        this.averagePath.Width = num7;
        this.averagePath.Height = num7;
        num2 = this.rect.X + this.rect.Width / 2.0;
        num3 = visible.Y;
      }
      Canvas.SetLeft((UIElement) this.averagePath, num2 - this.averagePath.Width / 2.0);
      Canvas.SetTop((UIElement) this.averagePath, num3 - this.averagePath.Height / 2.0);
      if (this.segmentCanvas.Children.Contains((UIElement) this.averagePath))
        return;
      this.segmentCanvas.Children.Add((UIElement) this.averagePath);
    }
    else
    {
      if (!this.segmentCanvas.Children.Contains((UIElement) this.averagePath))
        return;
      this.segmentCanvas.Children.Remove((UIElement) this.averagePath);
    }
  }

  protected override void SetVisualBindings(Shape element)
  {
    if (!(element is Path))
      base.SetVisualBindings(element);
    element.SetBinding(Shape.StrokeProperty, (BindingBase) new Binding()
    {
      Source = (object) this,
      Path = new PropertyPath("ActualStroke", new object[0])
    });
  }

  protected override void OnPropertyChanged(string name)
  {
    switch (name)
    {
      case "Interior":
        base.OnPropertyChanged("ActualStroke");
        break;
      case "Stroke":
        name = "ActualStroke";
        break;
    }
    base.OnPropertyChanged(name);
  }

  private void DrawPathGeometry()
  {
    PathFigure pathFigure1 = new PathFigure();
    pathFigure1.StartPoint = new Point(0.0, 0.0);
    System.Windows.Media.LineSegment lineSegment1 = new System.Windows.Media.LineSegment();
    System.Windows.Media.LineSegment lineSegment2 = new System.Windows.Media.LineSegment();
    lineSegment1.Point = new Point(10.0, 10.0);
    pathFigure1.Segments.Add((PathSegment) lineSegment1);
    PathFigure pathFigure2 = new PathFigure();
    pathFigure2.StartPoint = new Point(10.0, 0.0);
    lineSegment2.Point = new Point(0.0, 10.0);
    pathFigure2.Segments.Add((PathSegment) lineSegment2);
    this.averagePath.Data = (Geometry) new PathGeometry()
    {
      Figures = {
        pathFigure1,
        pathFigure2
      }
    };
  }
}
