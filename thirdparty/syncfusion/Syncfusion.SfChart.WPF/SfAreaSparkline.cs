// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.SfAreaSparkline
// Assembly: Syncfusion.SfChart.WPF, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 279D70C6-7936-4F3E-B6CF-79EFEF734826
// Assembly location: C:\Program Files\PDFgear\Syncfusion.SfChart.WPF.dll

using System;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Shapes;

#nullable disable
namespace Syncfusion.UI.Xaml.Charts;

public class SfAreaSparkline : MarkerBase
{
  private Path segmentPath;

  internal override void SetBinding(Shape element)
  {
    element.SetBinding(Shape.FillProperty, (BindingBase) new Binding()
    {
      Source = (object) this,
      Path = new PropertyPath("Interior", new object[0])
    });
    base.SetBinding(element);
  }

  protected override void RenderSegments()
  {
    bool flag = true;
    base.RenderSegments();
    double num1 = Math.Abs(this.deltaX / this.availableWidth);
    double num2 = Math.Abs(this.deltaY / this.availableHeight);
    double num3 = 1.0;
    double num4 = 0.0;
    int index1 = 0;
    PathGeometry segmentGeometry = new PathGeometry();
    PathFigure figure = new PathFigure();
    figure.StartPoint = this.TransformToVisible(this.xValues[0], 0.0);
    int num5 = this.EmptyPointValue == EmptyPointValues.None ? this.EmptyPointIndexes.Count : 1;
    for (int index2 = 0; index2 < num5; ++index2)
    {
      if ((double) this.xValues.Count > this.EmptyPointIndexes[index2] + 1.0)
      {
        flag = false;
        if (index2 != 0)
        {
          segmentGeometry = new PathGeometry();
          figure = new PathFigure();
          figure.StartPoint = this.TransformToVisible(this.xValues[(int) this.EmptyPointIndexes[index2] + 1], 0.0);
        }
        if (this.SegmentPresenter.Children.Count > index2)
        {
          this.segmentPath = this.SegmentPresenter.Children[index2] as Path;
        }
        else
        {
          this.segmentPath = new Path();
          this.SetBinding((Shape) this.segmentPath);
          this.SegmentPresenter.Children.Add((UIElement) this.segmentPath);
        }
        this.segmentPath.Clip = (Geometry) null;
        for (double emptyPointIndex = this.EmptyPointIndexes[index2]; emptyPointIndex < (double) this.yValues.Count; ++emptyPointIndex)
        {
          if (this.yValues.Count > index1)
          {
            double yValue = this.yValues[index1];
            double xValue = this.xValues[index1];
            if (!double.IsNaN(yValue))
            {
              if (Math.Abs(num3 - xValue) >= num1 || Math.Abs(num4 - yValue) >= num2)
              {
                num3 = xValue;
                num4 = yValue;
                System.Windows.Media.LineSegment lineSegment = new System.Windows.Media.LineSegment();
                Point visible = this.TransformToVisible(this.xValues[index1], this.yValues[index1]);
                lineSegment.Point = visible;
                figure.Segments.Add((PathSegment) lineSegment);
                if (this.MarkerVisibility == Visibility.Visible)
                  this.AddMarker(visible, xValue, yValue, index1);
              }
              ++index1;
            }
            else
            {
              flag = true;
              SfAreaSparkline.AddSegment(figure, segmentGeometry, ref this.segmentPath, this.TransformToVisible(this.xValues[index1 != 0 ? index1 - 1 : index1], 0.0));
              ++index1;
              break;
            }
          }
        }
      }
      if (!flag)
      {
        SfAreaSparkline.AddSegment(figure, segmentGeometry, ref this.segmentPath, this.TransformToVisible(this.xValues[index1 - 1], 0.0));
        this.segmentPath.Data = (Geometry) segmentGeometry;
      }
    }
  }

  private static void AddSegment(
    PathFigure figure,
    PathGeometry segmentGeometry,
    ref Path segmantPath,
    Point screenPoint)
  {
    figure.Segments.Add((PathSegment) new System.Windows.Media.LineSegment()
    {
      Point = screenPoint
    });
    figure.IsClosed = true;
    segmentGeometry.Figures.Add(figure);
    segmantPath.Data = (Geometry) segmentGeometry;
  }
}
