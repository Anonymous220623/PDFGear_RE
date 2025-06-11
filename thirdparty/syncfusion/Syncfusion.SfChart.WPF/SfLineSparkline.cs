// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.SfLineSparkline
// Assembly: Syncfusion.SfChart.WPF, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 279D70C6-7936-4F3E-B6CF-79EFEF734826
// Assembly location: C:\Program Files\PDFgear\Syncfusion.SfChart.WPF.dll

using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

#nullable disable
namespace Syncfusion.UI.Xaml.Charts;

public class SfLineSparkline : MarkerBase
{
  private Polyline segmentLine;

  protected override void RenderSegments()
  {
    if (this.yValues == null)
      return;
    base.RenderSegments();
    double num1 = Math.Abs(this.deltaX / this.availableWidth);
    double num2 = Math.Abs(this.deltaY / this.availableHeight);
    double num3 = 1.0;
    double num4 = 0.0;
    int index1 = 0;
    int num5 = this.EmptyPointValue == EmptyPointValues.None ? this.EmptyPointIndexes.Count : 1;
    for (int index2 = 0; index2 < num5; ++index2)
    {
      if (this.SegmentPresenter.Children.Count > index2)
      {
        this.segmentLine = this.SegmentPresenter.Children[index2] as Polyline;
        this.segmentLine.Points.Clear();
      }
      else
      {
        this.segmentLine = new Polyline();
        this.SetBinding((Shape) this.segmentLine);
        this.SegmentPresenter.Children.Add((UIElement) this.segmentLine);
      }
      this.segmentLine.Clip = (Geometry) null;
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
              Point visible = this.TransformToVisible(xValue, yValue);
              this.segmentLine.Points.Add(visible);
              num3 = xValue;
              num4 = yValue;
              if (this.MarkerVisibility == Visibility.Visible)
                this.AddMarker(visible, xValue, yValue, index1);
            }
          }
          else
          {
            ++index1;
            break;
          }
        }
        ++index1;
      }
    }
  }
}
