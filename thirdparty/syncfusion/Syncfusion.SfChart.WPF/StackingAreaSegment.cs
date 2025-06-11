// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.StackingAreaSegment
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

public class StackingAreaSegment : ChartSegment
{
  private bool segmentUpdated;
  private bool isEmpty;
  private List<double> XValues;
  private List<double> YValues;
  private Canvas segmentCanvas;
  private Path segPath;
  private PathGeometry strokeGeometry;
  private PathFigure strokeFigure;
  private PolyLineSegment strokePolyLine;
  private Path strokePath;
  private double _xData;
  private double _yData;

  public StackingAreaSegment()
  {
  }

  public StackingAreaSegment(List<double> xValues, List<double> yValues, StackingAreaSeries series)
  {
    this.Series = (ChartSeriesBase) series;
  }

  public StackingAreaSegment(List<double> xValues, List<double> yValues)
  {
  }

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

  public override UIElement CreateVisual(Size size)
  {
    this.segmentCanvas = new Canvas();
    this.segPath = new Path();
    this.segPath.Tag = (object) this;
    this.SetVisualBindings((Shape) this.segPath);
    this.segmentCanvas.Children.Add((UIElement) this.segPath);
    return (UIElement) this.segmentCanvas;
  }

  public override UIElement GetRenderedVisual() => (UIElement) null;

  public override void SetData(IList<double> xValues, IList<double> yValues)
  {
    this.XValues = xValues as List<double>;
    this.YValues = yValues as List<double>;
    double end1 = xValues.Max();
    double end2 = yValues.Max();
    double start1 = xValues.Min();
    double d = this.YValues.Min();
    this.isEmpty = double.IsNaN(d);
    double start2 = this.isEmpty ? this.YValues.Where<double>((Func<double, bool>) (e => !double.IsNaN(e))).Min() : d;
    this.XRange = new DoubleRange(start1, end1);
    this.YRange = new DoubleRange(start2, end2);
    if (this.isEmpty || this.segPath == null)
      return;
    this.segPath.SetBinding(Shape.StrokeThicknessProperty, (BindingBase) new Binding()
    {
      Source = (object) this,
      Path = new PropertyPath("StrokeThickness", new object[0])
    });
  }

  public override void Update(IChartTransformer transformer)
  {
    StackingAreaSeries series = this.Series as StackingAreaSeries;
    if (transformer == null)
      return;
    if (this.segmentUpdated)
      this.Series.SeriesRootPanel.Clip = (Geometry) null;
    PathFigure pathFigure = new PathFigure();
    PathGeometry pathGeometry = new PathGeometry();
    double origin = series.ActualXAxis.Origin;
    if (series.ActualXAxis != null && series.ActualXAxis.Origin == 0.0 && series.ActualYAxis is LogarithmicAxis && (series.ActualYAxis as LogarithmicAxis).Minimum.HasValue)
      origin = (series.ActualYAxis as LogarithmicAxis).Minimum.Value;
    pathFigure.StartPoint = transformer.TransformToVisible(this.XValues[0], origin);
    this.strokeGeometry = new PathGeometry();
    this.strokeFigure = new PathFigure();
    this.strokePolyLine = new PolyLineSegment();
    this.strokePath = new Path();
    if (!series.IsClosed && !double.IsNaN(this.YValues[this.YValues.Count / 2]))
      this.AddStroke(transformer.TransformToVisible(this.XValues[this.YValues.Count / 2], this.YValues[this.YValues.Count / 2]));
    else if (this.isEmpty)
      this.AddStroke(transformer.TransformToVisible(this.XValues[this.YValues.Count / 2 - 1], this.YValues[this.YValues.Count / 2 - 1]));
    for (int index = 0; index < this.XValues.Count; ++index)
    {
      if (!double.IsNaN(this.YValues[index]))
      {
        Point visible = transformer.TransformToVisible(this.XValues[index], this.YValues[index]);
        pathFigure.Segments.Add((PathSegment) new System.Windows.Media.LineSegment()
        {
          Point = visible
        });
        if ((this.isEmpty && !this.Series.ShowEmptyPoints || !series.IsClosed) && index > this.YValues.Count / 2 - 1)
        {
          if (double.IsNaN(this.YValues[index - 1]))
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
        if (index > 0 && index < this.XValues.Count - 1)
        {
          pathFigure.Segments.Add((PathSegment) new System.Windows.Media.LineSegment()
          {
            Point = transformer.TransformToVisible(this.XValues[index - 1], origin)
          });
          pathFigure.Segments.Add((PathSegment) new System.Windows.Media.LineSegment()
          {
            Point = transformer.TransformToVisible(this.XValues[index], origin)
          });
          pathFigure.Segments.Add((PathSegment) new System.Windows.Media.LineSegment()
          {
            Point = transformer.TransformToVisible(this.XValues[index + 1], origin)
          });
        }
        else if (index == this.YValues.Count - 1)
        {
          pathFigure.Segments.Add((PathSegment) new System.Windows.Media.LineSegment()
          {
            Point = transformer.TransformToVisible(this.XValues[index - 1], origin)
          });
          pathFigure.Segments.Add((PathSegment) new System.Windows.Media.LineSegment()
          {
            Point = transformer.TransformToVisible(this.XValues[index], origin)
          });
        }
        else
        {
          pathFigure.Segments.Add((PathSegment) new System.Windows.Media.LineSegment()
          {
            Point = transformer.TransformToVisible(this.XValues[index], origin)
          });
          pathFigure.Segments.Add((PathSegment) new System.Windows.Media.LineSegment()
          {
            Point = transformer.TransformToVisible(this.XValues[index + 1], origin)
          });
        }
      }
    }
    System.Windows.Media.LineSegment lineSegment = new System.Windows.Media.LineSegment();
    lineSegment.Point = transformer.TransformToVisible(this.XValues[this.XValues.Count - 1], origin);
    pathFigure.Segments.Add((PathSegment) lineSegment);
    if (series.IsClosed && this.isEmpty && !double.IsNaN(this.YValues[this.YValues.Count - 1]))
      this.strokePolyLine.Points.Add(lineSegment.Point);
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

  private void AddStroke(Point startPoint)
  {
    if (this.segmentCanvas.Children.Count > 1)
      this.segmentCanvas.Children.RemoveAt(1);
    this.segPath.StrokeThickness = 0.0;
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
