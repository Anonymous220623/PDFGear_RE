// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.AreaSegment
// Assembly: Syncfusion.SfChart.WPF, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 279D70C6-7936-4F3E-B6CF-79EFEF734826
// Assembly location: C:\Program Files\PDFgear\Syncfusion.SfChart.WPF.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Shapes;

#nullable disable
namespace Syncfusion.UI.Xaml.Charts;

public class AreaSegment : ChartSegment
{
  private IList<double> XValues;
  private IList<double> YValues;
  private Canvas segmentCanvas;
  private bool isEmpty;
  private Path segPath;
  private bool segmentUpdated;
  private PathGeometry strokeGeometry;
  private PathFigure strokeFigure;
  private PolyLineSegment strokePolyLine;
  private Path strokePath;
  private double _xData;
  private double _yData;

  public double XData
  {
    get => this._xData;
    set
    {
      this._xData = value;
      this.OnPropertyChanged(nameof (XData));
    }
  }

  public double YData
  {
    get => this._yData;
    set
    {
      this._yData = value;
      this.OnPropertyChanged(nameof (YData));
    }
  }

  public AreaSegment()
  {
  }

  public AreaSegment(
    List<double> xValues,
    List<double> yValues,
    AdornmentSeries series,
    object item)
  {
    this.Series = (ChartSeriesBase) series;
    this.Item = item;
  }

  public AreaSegment(List<double> xValues, IList<double> yValues)
  {
  }

  public override void SetData(IList<double> XValues, IList<double> YValues)
  {
    this.XValues = XValues;
    this.YValues = YValues;
    double d = YValues.Min();
    this.isEmpty = double.IsNaN(d);
    double start;
    if (double.IsNaN(d))
    {
      IEnumerable<double> source = YValues.Where<double>((Func<double, bool>) (e => !double.IsNaN(e)));
      start = !source.Any<double>() ? 0.0 : source.Min();
    }
    else
      start = d;
    this.XRange = new DoubleRange(XValues.Min(), XValues.Max());
    this.YRange = new DoubleRange(start, YValues.Max());
    if (this.isEmpty || this.segPath == null)
      return;
    this.segPath.SetBinding(Shape.StrokeThicknessProperty, (BindingBase) new Binding()
    {
      Source = (object) this,
      Path = new PropertyPath("StrokeThickness", new object[0])
    });
  }

  public override UIElement CreateVisual(Size size)
  {
    this.segmentCanvas = new Canvas();
    this.segPath = new Path();
    this.segPath.Tag = (object) this;
    this.SetVisualBindings((Shape) this.segPath);
    this.segmentCanvas.Children.Add((UIElement) this.segPath);
    return (UIElement) this.segmentCanvas;
  }

  public override UIElement GetRenderedVisual() => (UIElement) this.segmentCanvas;

  public override void Update(IChartTransformer transformer)
  {
    ChartAxis actualXaxis = this.Series.ActualXAxis;
    int index1 = 0;
    if (transformer == null)
      return;
    if (this.segmentUpdated)
      this.Series.SeriesRootPanel.Clip = (Geometry) null;
    PathGeometry pathGeometry = new PathGeometry();
    PathFigure pathFigure = new PathFigure();
    double start = this.Series.ActualYAxis.ActualRange.Start;
    double origin = this.Series.ActualXAxis.Origin;
    if (this.Series.ActualXAxis != null && this.Series.ActualXAxis.Origin == 0.0 && this.Series.ActualYAxis is LogarithmicAxis && (this.Series.ActualYAxis as LogarithmicAxis).Minimum.HasValue)
      origin = (this.Series.ActualYAxis as LogarithmicAxis).Minimum.Value;
    double y = origin == 0.0 ? (start < 0.0 ? 0.0 : start) : origin;
    pathFigure.StartPoint = transformer.TransformToVisible(this.XValues[0], y);
    this.ResetStrokeShapes();
    if (this.Series is PolarRadarSeriesBase && !double.IsNaN(this.YValues[0]))
      pathFigure.StartPoint = transformer.TransformToVisible(this.XValues[0], this.YValues[0]);
    if (this.Series is AreaSeries && !(this.Series as AreaSeries).IsClosed && !double.IsNaN(this.YValues[0]))
      this.AddStroke(transformer.TransformToVisible(this.XValues[0], this.YValues[0]));
    else if (this.isEmpty)
      this.AddStroke(pathFigure.StartPoint);
    double num1 = Math.Floor(actualXaxis is LogarithmicAxis logarithmicAxis ? Math.Pow(logarithmicAxis.LogarithmicBase, actualXaxis.VisibleRange.Start) : actualXaxis.VisibleRange.Start);
    double num2 = Math.Ceiling(logarithmicAxis != null ? Math.Pow(logarithmicAxis.LogarithmicBase, actualXaxis.VisibleRange.End) : actualXaxis.VisibleRange.End);
    for (int index2 = 0; index2 < this.XValues.Count; ++index2)
    {
      double xvalue = this.XValues[index2];
      if (num1 > xvalue || num2 < xvalue)
      {
        if (num1 >= xvalue && index2 + 1 < this.XValues.Count && num1 <= this.XValues[index2 + 1])
        {
          pathFigure.StartPoint = transformer.TransformToVisible(xvalue, y);
          if (this.Series is AreaSeries && !(this.Series as AreaSeries).IsClosed && !double.IsNaN(this.YValues[0]))
          {
            this.ResetStrokeShapes();
            this.AddStroke(transformer.TransformToVisible(xvalue, this.YValues[index2]));
          }
          else if (this.isEmpty)
          {
            this.ResetStrokeShapes();
            this.AddStroke(pathFigure.StartPoint);
          }
        }
        else if (num2 > xvalue || index2 - 1 <= -1 || num2 < this.XValues[index2 - 1])
          continue;
      }
      if (!double.IsNaN(this.YValues[index2]))
      {
        Point visible = transformer.TransformToVisible(xvalue, this.YValues[index2]);
        pathFigure.Segments.Add((PathSegment) new System.Windows.Media.LineSegment()
        {
          Point = visible
        });
        if (this.isEmpty && !this.Series.ShowEmptyPoints || this.Series is AreaSeries && !(this.Series as AreaSeries).IsClosed)
        {
          if (index2 > 0 && double.IsNaN(this.YValues[index2 - 1]))
          {
            this.strokeFigure = new PathFigure();
            this.strokePolyLine = new PolyLineSegment();
            this.strokeFigure.StartPoint = visible;
            this.strokeGeometry.Figures.Add(this.strokeFigure);
            this.strokeFigure.Segments.Add((PathSegment) this.strokePolyLine);
          }
          this.strokePolyLine.Points.Add(visible);
        }
      }
      else if (this.XValues.Count > 1)
      {
        if (index2 > 0 && index2 < this.XValues.Count - 1)
        {
          pathFigure.Segments.Add((PathSegment) new System.Windows.Media.LineSegment()
          {
            Point = transformer.TransformToVisible(this.XValues[index2 - 1], y)
          });
          pathFigure.Segments.Add((PathSegment) new System.Windows.Media.LineSegment()
          {
            Point = transformer.TransformToVisible(xvalue, y)
          });
          pathFigure.Segments.Add((PathSegment) new System.Windows.Media.LineSegment()
          {
            Point = transformer.TransformToVisible(this.XValues[index2 + 1], y)
          });
        }
        else if (index2 == 0)
        {
          pathFigure.Segments.Add((PathSegment) new System.Windows.Media.LineSegment()
          {
            Point = transformer.TransformToVisible(xvalue, y)
          });
          pathFigure.Segments.Add((PathSegment) new System.Windows.Media.LineSegment()
          {
            Point = transformer.TransformToVisible(this.XValues[index2 + 1], y)
          });
        }
        else
          pathFigure.Segments.Add((PathSegment) new System.Windows.Media.LineSegment()
          {
            Point = transformer.TransformToVisible(this.XValues[index2 - 1], y)
          });
      }
      index1 = index2;
    }
    if (!(this.Series is PolarRadarSeriesBase))
    {
      System.Windows.Media.LineSegment lineSegment = new System.Windows.Media.LineSegment();
      lineSegment.Point = transformer.TransformToVisible(this.XValues[index1], y);
      pathFigure.Segments.Add((PathSegment) lineSegment);
      if (this.Series is AreaSeries && (this.Series as AreaSeries).IsClosed && this.isEmpty && !double.IsNaN(this.YValues[index1]))
        this.strokePolyLine.Points.Add(lineSegment.Point);
    }
    pathGeometry.Figures.Add(pathFigure);
    this.segPath.Data = (Geometry) pathGeometry;
    this.segmentUpdated = true;
  }

  public override void OnSizeChanged(Size size)
  {
  }

  protected override void SetVisualBindings(Shape element)
  {
    base.SetVisualBindings(element);
    element.SetBinding(Shape.StrokeProperty, (BindingBase) new Binding()
    {
      Source = (object) this,
      Path = new PropertyPath("Stroke", new object[0])
    });
  }

  private void ResetStrokeShapes()
  {
    this.strokeGeometry = new PathGeometry();
    this.strokeFigure = new PathFigure();
    this.strokePolyLine = new PolyLineSegment();
    this.strokePath = new Path();
  }

  private void AddStroke(Point startPoint)
  {
    if (this.segmentCanvas.Children.Count > 1)
      this.segmentCanvas.Children.RemoveAt(1);
    this.segPath.StrokeThickness = 0.0;
    this.strokePath = new Path();
    this.strokeFigure.StartPoint = startPoint;
    this.strokeFigure.Segments.Add((PathSegment) this.strokePolyLine);
    this.strokeGeometry.Figures.Add(this.strokeFigure);
    this.strokePath.Data = (Geometry) this.strokeGeometry;
    this.strokePath.SetBinding(Shape.StrokeProperty, (BindingBase) new Binding()
    {
      Source = (object) this,
      Path = new PropertyPath("Stroke", new object[0])
    });
    this.strokePath.SetBinding(Shape.StrokeThicknessProperty, (BindingBase) new Binding()
    {
      Source = (object) this,
      Path = new PropertyPath("StrokeThickness", new object[0])
    });
    this.segmentCanvas.Children.Add((UIElement) this.strokePath);
  }

  internal override void Dispose()
  {
    if (this.segmentCanvas != null)
    {
      this.segmentCanvas.Children.Clear();
      this.segmentCanvas.Tag = (object) null;
      this.segmentCanvas = (Canvas) null;
    }
    if (this.segPath != null)
    {
      this.segPath.Tag = (object) null;
      this.segPath = (Path) null;
    }
    base.Dispose();
  }
}
