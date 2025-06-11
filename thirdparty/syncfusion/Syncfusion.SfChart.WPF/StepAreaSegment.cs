// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.StepAreaSegment
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

public class StepAreaSegment : ChartSegment
{
  private bool isEmpty;
  private bool isSegmentUpdated;
  private Path segPath;
  private Canvas segmentCanvas;
  private List<ChartPoint> stepAreaPoints = new List<ChartPoint>();
  private PathGeometry strokeGeometry;
  private PathFigure strokeFigure;
  private PolyLineSegment strokePolyLine;
  private Path strokePath;
  private double _xData;
  private double _yData;

  public StepAreaSegment()
  {
    this.XRange = DoubleRange.Empty;
    this.YRange = DoubleRange.Empty;
  }

  [Obsolete("Use StepAreaSegment(List<ChartPoint> pointsCollection, StepAreaSeries series)")]
  public StepAreaSegment(List<Point> pointsCollection, StepAreaSeries series)
  {
  }

  public StepAreaSegment(List<ChartPoint> pointsCollection, StepAreaSeries series)
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

  [Obsolete("Use StepAreaSegment(List<ChartPoint> pointsCollection, StepAreaSeries series)")]
  public override void SetData(List<Point> StepAreaPoints)
  {
    List<ChartPoint> chartPointList = new List<ChartPoint>();
    foreach (Point stepAreaPoint in StepAreaPoints)
      chartPointList.Add(new ChartPoint(stepAreaPoint.X, stepAreaPoint.Y));
    this.stepAreaPoints = chartPointList;
    this.isEmpty = false;
    foreach (ChartPoint chartPoint in chartPointList)
    {
      StepAreaSegment stepAreaSegment1 = this;
      stepAreaSegment1.XRange = stepAreaSegment1.XRange + chartPoint.X;
      if (double.IsNaN(chartPoint.Y))
      {
        this.isEmpty = true;
      }
      else
      {
        StepAreaSegment stepAreaSegment2 = this;
        stepAreaSegment2.YRange = stepAreaSegment2.YRange + chartPoint.Y;
      }
    }
    if (this.isEmpty || this.segPath == null)
      return;
    this.segPath.SetBinding(Shape.StrokeThicknessProperty, (BindingBase) new Binding()
    {
      Source = (object) this,
      Path = new PropertyPath("StrokeThickness", new object[0])
    });
  }

  public override void SetData(List<ChartPoint> StepAreaPoints)
  {
    this.stepAreaPoints = StepAreaPoints;
    this.isEmpty = false;
    foreach (ChartPoint stepAreaPoint in StepAreaPoints)
    {
      StepAreaSegment stepAreaSegment1 = this;
      stepAreaSegment1.XRange = stepAreaSegment1.XRange + stepAreaPoint.X;
      if (double.IsNaN(stepAreaPoint.Y))
      {
        this.isEmpty = true;
      }
      else
      {
        StepAreaSegment stepAreaSegment2 = this;
        stepAreaSegment2.YRange = stepAreaSegment2.YRange + stepAreaPoint.Y;
      }
    }
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

  public override UIElement GetRenderedVisual() => (UIElement) this.segPath;

  public override void Update(IChartTransformer transformer)
  {
    PathFigure pathFigure = new PathFigure();
    if (this.stepAreaPoints.Count <= 0)
      return;
    if (this.isSegmentUpdated)
      this.Series.SeriesRootPanel.Clip = (Geometry) null;
    pathFigure.StartPoint = transformer.TransformToVisible(this.stepAreaPoints[1].X, this.stepAreaPoints[1].Y);
    PathGeometry pathGeometry = new PathGeometry();
    double y = this.Series.ActualXAxis != null ? this.Series.ActualXAxis.Origin : 0.0;
    this.strokeGeometry = new PathGeometry();
    this.strokeFigure = new PathFigure();
    this.strokePolyLine = new PolyLineSegment();
    this.strokePath = new Path();
    if (!(this.Series as StepAreaSeries).IsClosed && !double.IsNaN(this.stepAreaPoints[3].Y))
      this.AddStroke(transformer.TransformToVisible(this.stepAreaPoints[2].X, this.stepAreaPoints[3].Y));
    else if (this.isEmpty)
      this.AddStroke(pathFigure.StartPoint);
    for (int index = 1; index < this.stepAreaPoints.Count; index += 2)
    {
      if (!double.IsNaN(this.stepAreaPoints[index].Y) && !double.IsNaN(this.stepAreaPoints[index + 1].Y))
      {
        Point visible1 = transformer.TransformToVisible(this.stepAreaPoints[index].X, this.stepAreaPoints[index].Y);
        Point visible2 = transformer.TransformToVisible(this.stepAreaPoints[index + 1].X, this.stepAreaPoints[index + 1].Y);
        pathFigure.Segments.Add((PathSegment) new System.Windows.Media.LineSegment()
        {
          Point = visible1
        });
        pathFigure.Segments.Add((PathSegment) new System.Windows.Media.LineSegment()
        {
          Point = visible2
        });
        if (this.isEmpty && !this.Series.ShowEmptyPoints && (this.Series as StepAreaSeries).IsClosed)
        {
          this.strokePolyLine.Points.Add(visible1);
          if (index <= this.stepAreaPoints.Count - 3 && !double.IsNaN(this.stepAreaPoints[index + 3].Y))
            this.strokePolyLine.Points.Add(visible2);
        }
        else if (!(this.Series as StepAreaSeries).IsClosed && index > 1)
        {
          this.strokePolyLine.Points.Add(visible1);
          if (index <= this.stepAreaPoints.Count - 3 && !double.IsNaN(this.stepAreaPoints[index + 3].Y))
            this.strokePolyLine.Points.Add(visible2);
        }
      }
      else
      {
        pathFigure.Segments.Add((PathSegment) new System.Windows.Media.LineSegment()
        {
          Point = transformer.TransformToVisible(this.stepAreaPoints[index - 1].X, y)
        });
        if (double.IsNaN(this.stepAreaPoints[index - 1].Y))
          pathFigure.Segments.Add((PathSegment) new System.Windows.Media.LineSegment()
          {
            Point = transformer.TransformToVisible(this.stepAreaPoints[index - 1].X, y)
          });
        if (index < this.stepAreaPoints.Count - 1)
        {
          pathFigure.Segments.Add((PathSegment) new System.Windows.Media.LineSegment()
          {
            Point = transformer.TransformToVisible(this.stepAreaPoints[index + 1].X, y)
          });
          if (double.IsNaN(this.stepAreaPoints[index - 1].Y) && !double.IsNaN(this.stepAreaPoints[index + 1].Y))
          {
            Point visible = transformer.TransformToVisible(this.stepAreaPoints[index].X, this.stepAreaPoints[index + 1].Y);
            if (!this.Series.ShowEmptyPoints && (this.Series as StepAreaSeries).IsClosed)
            {
              this.strokeFigure = new PathFigure();
              this.strokePolyLine = new PolyLineSegment();
              this.strokeFigure.StartPoint = visible;
              this.strokeGeometry.Figures.Add(this.strokeFigure);
              this.strokeFigure.Segments.Add((PathSegment) this.strokePolyLine);
            }
            else if (!(this.Series as StepAreaSeries).IsClosed && index > 1)
            {
              this.strokeFigure = new PathFigure();
              this.strokePolyLine = new PolyLineSegment();
              this.strokeFigure.StartPoint = visible;
              this.strokeGeometry.Figures.Add(this.strokeFigure);
              this.strokeFigure.Segments.Add((PathSegment) this.strokePolyLine);
            }
            pathFigure.Segments.Add((PathSegment) new System.Windows.Media.LineSegment()
            {
              Point = visible
            });
          }
        }
      }
    }
    System.Windows.Media.LineSegment lineSegment = new System.Windows.Media.LineSegment();
    lineSegment.Point = transformer.TransformToVisible(this.stepAreaPoints[this.stepAreaPoints.Count - 1].X, y);
    pathFigure.Segments.Add((PathSegment) lineSegment);
    if ((this.Series as StepAreaSeries).IsClosed && this.isEmpty && !double.IsNaN(this.stepAreaPoints[this.stepAreaPoints.Count - 1].Y))
      this.strokePolyLine.Points.Add(lineSegment.Point);
    pathGeometry.Figures.Add(pathFigure);
    this.segPath.Data = (Geometry) pathGeometry;
    this.isSegmentUpdated = true;
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
    this.strokePath = new Path();
    this.strokeFigure.StartPoint = startPoint;
    this.strokeFigure.Segments.Add((PathSegment) this.strokePolyLine);
    this.strokeGeometry.Figures.Add(this.strokeFigure);
    this.strokePath.Data = (Geometry) this.strokeGeometry;
    this.segmentCanvas.Children.Add((UIElement) this.strokePath);
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
