// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.ChartCartesianAxisPanel3D
// Assembly: Syncfusion.SfChart.WPF, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 279D70C6-7936-4F3E-B6CF-79EFEF734826
// Assembly location: C:\Program Files\PDFgear\Syncfusion.SfChart.WPF.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

#nullable disable
namespace Syncfusion.UI.Xaml.Charts;

public class ChartCartesianAxisPanel3D
{
  public ChartCartesianAxisPanel3D() => this.LayoutCalc = new List<ILayoutCalculator>();

  internal ChartAxisBase3D Axis { get; set; }

  internal FrameworkElement HeaderContent { get; set; }

  internal List<ILayoutCalculator> LayoutCalc { get; set; }

  internal Size ComputeSize(Size availableSize)
  {
    double num1 = 0.0;
    double num2 = 0.0;
    this.HeaderContent.Measure(availableSize);
    this.HeaderContent.HorizontalAlignment = HorizontalAlignment.Center;
    this.HeaderContent.VerticalAlignment = VerticalAlignment.Center;
    double actualRotationAngle = (this.Axis.Area as SfChart3D).ActualRotationAngle;
    double actualTiltAngle = (this.Axis.Area as SfChart3D).ActualTiltAngle;
    if (this.Axis.Orientation == Orientation.Vertical)
    {
      double num3 = (this.Axis.OpposedPosition || this.Axis.ShowAxisNextToOrigin && (actualRotationAngle >= 90.0 && actualRotationAngle < 180.0 || actualRotationAngle >= 270.0 && actualRotationAngle < 360.0) || !this.Axis.ShowAxisNextToOrigin && (this.Axis.AxisPosition3D == AxisPosition3D.DepthBackRight || this.Axis.AxisPosition3D == AxisPosition3D.BackLeft || this.Axis.AxisPosition3D == AxisPosition3D.DepthFrontLeft || this.Axis.AxisPosition3D == AxisPosition3D.FrontRight) ? 1.0 : -1.0) * 90.0;
      this.HeaderContent.RenderTransform = (Transform) new RotateTransform()
      {
        Angle = num3
      };
    }
    else if (this.Axis.Orientation == Orientation.Horizontal && actualTiltAngle >= 45.0 && actualTiltAngle < 315.0)
    {
      if (this.Axis.IsZAxis)
      {
        if (actualRotationAngle >= 0.0 && actualRotationAngle < 45.0 || actualRotationAngle >= 180.0 && actualRotationAngle < 225.0)
          this.HeaderContent.RenderTransform = (Transform) new RotateTransform()
          {
            Angle = 90.0
          };
        else if (actualRotationAngle >= 135.0 && actualRotationAngle < 180.0 || actualRotationAngle >= 315.0 && actualRotationAngle < 360.0)
          this.HeaderContent.RenderTransform = (Transform) new RotateTransform()
          {
            Angle = -90.0
          };
        else
          this.HeaderContent.RenderTransform = (Transform) null;
      }
      else if (actualRotationAngle >= 45.0 && actualRotationAngle < 90.0 || actualRotationAngle >= 225.0 && actualRotationAngle < 270.0)
        this.HeaderContent.RenderTransform = (Transform) new RotateTransform()
        {
          Angle = -90.0
        };
      else if (actualRotationAngle >= 90.0 && actualRotationAngle < 135.0 || actualRotationAngle >= 270.0 && actualRotationAngle < 315.0)
        this.HeaderContent.RenderTransform = (Transform) new RotateTransform()
        {
          Angle = 90.0
        };
      else
        this.HeaderContent.RenderTransform = (Transform) null;
    }
    else
      this.HeaderContent.RenderTransform = (Transform) null;
    double num4 = 0.0;
    double num5 = 0.0;
    foreach (ILayoutCalculator layoutCalculator in this.LayoutCalc)
    {
      layoutCalculator.Measure(availableSize);
      if (layoutCalculator is ChartCartesianAxisLabelsPanel && this.Axis.GetLabelPosition() == AxisElementPosition.Inside || layoutCalculator is ChartCartesianAxisElementsPanel && this.Axis.TickLinesPosition == AxisElementPosition.Inside)
      {
        num4 += layoutCalculator.DesiredSize.Width;
        num5 += layoutCalculator.DesiredSize.Height;
      }
      num1 += layoutCalculator.DesiredSize.Width;
      num2 += layoutCalculator.DesiredSize.Height;
    }
    bool flag = this.Axis.Orientation == Orientation.Vertical;
    double width = num1 + (flag ? this.HeaderContent.DesiredSize.Height : this.HeaderContent.DesiredSize.Width);
    double height = num2 + (flag ? this.HeaderContent.DesiredSize.Width : this.HeaderContent.DesiredSize.Height);
    Size size;
    if (this.Axis.Orientation == Orientation.Vertical)
    {
      this.Axis.InsidePadding = num4;
      size = new Size(width, availableSize.Height);
    }
    else
    {
      this.Axis.InsidePadding = num5;
      size = new Size(availableSize.Width, height);
    }
    return ChartLayoutUtils.CheckSize(size);
  }

  internal void ArrangeElements(Size finalSize) => this.ArrangeCartesianElements(finalSize);

  private void ArrangeCartesianElements(Size finalSize)
  {
    double actualRotationAngle = (this.Axis.Area as SfChart3D).ActualRotationAngle;
    if (this.Axis == null)
      return;
    ILayoutCalculator layoutCalculator1 = this.LayoutCalc[0];
    ILayoutCalculator layoutCalculator2 = this.LayoutCalc[1];
    List<object> objectList = new List<object>();
    List<Size> sizeList = new List<Size>();
    bool flag1 = this.Axis.Orientation == Orientation.Vertical;
    bool flag2 = this.Axis.OpposedPosition || !flag1 && this.Axis.Area.Axes.Where<ChartAxis>((Func<ChartAxis, bool>) (x => x is ChartAxisBase3D && x.Orientation == Orientation.Horizontal && x.OpposedPosition)).Any<ChartAxis>();
    bool flag3 = !flag2 && flag1 && (this.Axis.ShowAxisNextToOrigin && actualRotationAngle >= 0.0 && actualRotationAngle < 180.0 || !this.Axis.ShowAxisNextToOrigin && (this.Axis.AxisPosition3D == AxisPosition3D.FrontLeft || this.Axis.AxisPosition3D == AxisPosition3D.BackLeft || this.Axis.AxisPosition3D == AxisPosition3D.DepthFrontLeft || this.Axis.AxisPosition3D == AxisPosition3D.DepthBackLeft)) || !flag1 && flag2;
    if (this.Axis.TickLinesPosition == AxisElementPosition.Inside)
    {
      objectList.Insert(0, (object) layoutCalculator2);
      sizeList.Insert(0, layoutCalculator2.DesiredSize);
    }
    else
    {
      objectList.Add((object) layoutCalculator2);
      sizeList.Add(layoutCalculator2.DesiredSize);
    }
    if (this.Axis.GetLabelPosition() == AxisElementPosition.Inside)
    {
      objectList.Insert(0, (object) layoutCalculator1);
      sizeList.Insert(0, layoutCalculator1.DesiredSize);
    }
    else
    {
      objectList.Add((object) layoutCalculator1);
      sizeList.Add(layoutCalculator1.DesiredSize);
    }
    objectList.Add((object) this.HeaderContent);
    Size size = this.HeaderContent.DesiredSize;
    if (flag1)
      size = new Size(size.Height, size.Width);
    sizeList.Add(size);
    if (flag3)
    {
      objectList.Reverse();
      sizeList.Reverse();
    }
    double num1 = 0.0;
    double num2 = this.Axis.Area is SfChart3D ? (this.Axis.Area as SfChart3D).WallSize : 0.0;
    double depth = 0.0;
    UIElementLeftShift elementLeftShift1 = UIElementLeftShift.LeftHalfShift;
    UIElementTopShift uiElementTopShift1 = UIElementTopShift.TopHalfShift;
    for (int index = 0; index < objectList.Count; ++index)
    {
      object obj = objectList[index];
      ILayoutCalculator layoutCalculator3 = obj as ILayoutCalculator;
      Size desiredSize;
      Rect arrangeRect;
      if (flag1)
      {
        if (obj as FrameworkElement == this.HeaderContent)
        {
          desiredSize = this.HeaderContent.DesiredSize;
          double height = desiredSize.Height;
          double num3 = finalSize.Height / 2.0;
          arrangeRect = this.Axis.ArrangeRect;
          double top1 = arrangeRect.Top;
          double top2 = num3 + top1;
          double left1;
          if (this.Axis.ShowAxisNextToOrigin)
          {
            flag2 = flag2 || actualRotationAngle >= 180.0 && actualRotationAngle < 360.0;
            double num4 = flag3 ? num1 + this.Axis.ArrangeRect.Left - num2 : num1 + sizeList[index].Width + this.Axis.ArrangeRect.Left + num2;
            if (actualRotationAngle >= 90.0 && actualRotationAngle < 270.0)
              depth = (this.Axis.Area as SfChart3D).ActualDepth;
            if (this.Axis.ShowAxisNextToOrigin && this.Axis.HeaderPosition == AxisHeaderPosition.Far)
            {
              ChartAxis chartAxis = (ChartAxis) null;
              double num5 = 0.0;
              if (this.Axis.Area.InternalSecondaryAxis == this.Axis)
              {
                num5 = this.Axis.Area.InternalPrimaryAxis.ValueToCoefficientCalc(this.Axis.Origin);
                chartAxis = this.Axis.Area.InternalPrimaryAxis;
              }
              else if (this.Axis.Area.InternalSecondaryAxis.Orientation == Orientation.Horizontal && this.Axis.Area.InternalPrimaryAxis == this.Axis)
              {
                num5 = this.Axis.Area.InternalSecondaryAxis.ValueToCoefficientCalc(this.Axis.Origin);
                chartAxis = this.Axis.Area.InternalSecondaryAxis;
              }
              if (0.0 < num5 && 1.0 > num5)
              {
                Rect rect = ChartLayoutUtils.Subtractthickness(new Rect(new Point(0.0, 0.0), this.Axis.AvailableSize), this.Axis.Area.AxisThickness);
                double actualPlotOffset = chartAxis.GetActualPlotOffset();
                double num6 = flag2 ? (1.0 - num5) * (rect.Width - actualPlotOffset) + chartAxis.GetActualPlotOffsetStart() + this.Axis.InsidePadding : num5 * (rect.Width - actualPlotOffset) + chartAxis.ActualPlotOffset + this.Axis.InsidePadding;
                left1 = !flag2 ? (num6 <= finalSize.Width - height ? num4 : -num2 / 2.0) : (num6 <= finalSize.Width - height ? num4 : rect.Width + height + num2 / 2.0);
              }
              else
                left1 = num4;
            }
            else
              left1 = num4;
          }
          else
          {
            double num7 = 0.0;
            double num8;
            switch (this.Axis.AxisPosition3D)
            {
              case AxisPosition3D.DepthFrontLeft:
                depth = num1 + this.Axis.AxisDepth - num2 - num7;
                arrangeRect = this.Axis.ArrangeRect;
                num8 = arrangeRect.Left;
                break;
              case AxisPosition3D.FrontRight:
                double num9 = num1;
                desiredSize = sizeList[index];
                double width1 = desiredSize.Width;
                double num10 = num9 + width1;
                arrangeRect = this.Axis.ArrangeRect;
                double left2 = arrangeRect.Left;
                num8 = num10 + left2 + num2 + num7;
                break;
              case AxisPosition3D.DepthFrontRight:
                num8 = this.Axis.ArrangeRect.Left;
                depth = this.Axis.AxisDepth - num1 - sizeList[index].Width - num2 - num7;
                break;
              case AxisPosition3D.BackRight:
                double num11 = num1;
                desiredSize = sizeList[index];
                double width2 = desiredSize.Width;
                double num12 = num11 + width2;
                arrangeRect = this.Axis.ArrangeRect;
                double left3 = arrangeRect.Left;
                num8 = num12 + left3 + num2 + num7;
                depth = this.Axis.AxisDepth;
                break;
              case AxisPosition3D.DepthBackRight:
                num8 = this.Axis.ArrangeRect.Left;
                depth = this.Axis.AxisDepth + num1 + sizeList[index].Width + num2 + num7;
                break;
              case AxisPosition3D.BackLeft:
                double num13 = num1;
                arrangeRect = this.Axis.ArrangeRect;
                double left4 = arrangeRect.Left;
                num8 = num13 + left4 - num2 - num7;
                depth = this.Axis.AxisDepth;
                break;
              case AxisPosition3D.DepthBackLeft:
                depth = num1 + this.Axis.AxisDepth + num2 + num7;
                arrangeRect = this.Axis.ArrangeRect;
                num8 = arrangeRect.Left;
                break;
              default:
                double num14 = num1;
                arrangeRect = this.Axis.ArrangeRect;
                double left5 = arrangeRect.Left;
                num8 = num14 + left5 - num2 - num7;
                break;
            }
            left1 = num8;
          }
          ChartAxisBase3D axis = this.Axis;
          Vector3D[] vector3DArray = new Vector3D[3];
          ((SfChart3D) this.Axis.Area).Graphics3D.AddVisual((Polygon3D) new UIElement3D((UIElement) this.HeaderContent, axis.AxisPosition3D == AxisPosition3D.DepthBackLeft || axis.AxisPosition3D == AxisPosition3D.DepthBackRight || axis.AxisPosition3D == AxisPosition3D.DepthFrontLeft || axis.AxisPosition3D == AxisPosition3D.DepthFrontRight ? ChartCartesianAxisPanel3D.GetHeaderVectoColl(left1, top2, depth, false) : ChartCartesianAxisPanel3D.GetHeaderVectoColl(left1, top2, depth, true))
          {
            LeftShift = elementLeftShift1,
            TopShift = uiElementTopShift1
          });
        }
        else
          layoutCalculator3.Left = !flag3 ? num1 + num2 / 2.0 : num1 - num2 / 2.0;
        double num15 = num1;
        desiredSize = sizeList[index];
        double width = desiredSize.Width;
        num1 = num15 + width;
      }
      else
      {
        if (obj as FrameworkElement == this.HeaderContent)
        {
          double num16 = finalSize.Width / 2.0;
          double num17;
          if (this.Axis.IsZAxis)
          {
            num17 = 0.0;
          }
          else
          {
            arrangeRect = this.Axis.ArrangeRect;
            num17 = arrangeRect.Left;
          }
          double num18 = num16 + num17;
          double num19;
          if (!flag3)
          {
            double num20 = num1;
            desiredSize = sizeList[index];
            double height = desiredSize.Height;
            double num21 = num20 + height;
            arrangeRect = this.Axis.ArrangeRect;
            double top = arrangeRect.Top;
            num19 = num21 + top + num2;
          }
          else
          {
            double num22 = num1;
            arrangeRect = this.Axis.ArrangeRect;
            double top = arrangeRect.Top;
            num19 = num22 + top - num2;
          }
          double num23 = num19;
          double top3;
          if (this.Axis.ShowAxisNextToOrigin && this.Axis.HeaderPosition == AxisHeaderPosition.Far)
          {
            ChartAxis chartAxis = (ChartAxis) null;
            double num24 = 0.0;
            if (this.Axis.Area.InternalPrimaryAxis == this.Axis)
            {
              num24 = this.Axis.Area.InternalSecondaryAxis.ValueToCoefficientCalc(this.Axis.Origin);
              chartAxis = this.Axis.Area.InternalSecondaryAxis;
            }
            else if (this.Axis.Area.InternalPrimaryAxis.Orientation == Orientation.Vertical && this.Axis.Area.InternalSecondaryAxis == this.Axis)
            {
              num24 = this.Axis.Area.InternalPrimaryAxis.ValueToCoefficientCalc(this.Axis.Origin);
              chartAxis = this.Axis.Area.InternalPrimaryAxis;
            }
            if (0.0 < num24 && 1.0 > num24)
            {
              Rect rect = ChartLayoutUtils.Subtractthickness(new Rect(new Point(0.0, 0.0), this.Axis.AvailableSize), this.Axis.Area.AxisThickness);
              double actualPlotOffset = chartAxis.GetActualPlotOffset();
              desiredSize = this.HeaderContent.DesiredSize;
              double height = desiredSize.Height;
              double num25 = flag2 ? (1.0 - num24) * (rect.Height - actualPlotOffset) + chartAxis.GetActualPlotOffsetEnd() + this.Axis.InsidePadding : num24 * (rect.Height - actualPlotOffset) + chartAxis.ActualPlotOffset + this.Axis.InsidePadding;
              top3 = !flag2 ? (num25 <= finalSize.Height - height ? num23 : rect.Height + height + num2 / 2.0) : (num25 <= finalSize.Height - height ? num23 : height / 2.0 - num2);
            }
            else
              top3 = num23;
          }
          else
            top3 = num23;
          if (this.Axis != null && this.Axis.IsZAxis && this.Axis is CategoryAxis3D && !this.Axis.RegisteredSeries.Where<ISupportAxes>((Func<ISupportAxes, bool>) (x => x is XyzDataSeries3D && (x as XyzDataSeries3D).ZBindingPath.Length != 0)).Any<ISupportAxes>())
            num18 = (this.Axis.Area as SfChart3D).ActualDepth / 2.0;
          double actualTiltAngle = (this.Axis.Area as SfChart3D).ActualTiltAngle;
          bool flag4 = this.Axis.Area.Axes.Any<ChartAxis>((Func<ChartAxis, bool>) (x => x.Orientation == Orientation.Vertical && x.OpposedPosition));
          UIElementLeftShift elementLeftShift2 = UIElementLeftShift.LeftHalfShift;
          UIElementTopShift uiElementTopShift2 = UIElementTopShift.TopHalfShift;
          double num26;
          if (!flag3)
          {
            double num27 = top3;
            arrangeRect = this.Axis.ArrangeRect;
            double top4 = arrangeRect.Top;
            num26 = num27 - top4 + size.Height;
          }
          else
          {
            double num28 = -top3;
            arrangeRect = this.Axis.ArrangeRect;
            double height = arrangeRect.Height;
            num26 = num28 + height;
          }
          double num29 = num26;
          bool flag5 = actualTiltAngle >= 45.0 && actualTiltAngle < 315.0;
          List<Dictionary<int, Rect>> rectssByRowsAndCols = (layoutCalculator1 as ChartCartesianAxisLabelsPanel).LabelLayout.RectssByRowsAndCols;
          double num30 = rectssByRowsAndCols == null || rectssByRowsAndCols.Count <= 0 ? 0.0 : rectssByRowsAndCols.Sum<Dictionary<int, Rect>>((Func<Dictionary<int, Rect>, double>) (dictionary => dictionary.Values.Max<Rect>((Func<Rect, double>) (rect => rect.Width))));
          if (this.Axis.IsZAxis)
          {
            double num31 = 0.0;
            if (flag5)
            {
              if (actualRotationAngle >= 0.0 && actualRotationAngle < 45.0 || actualRotationAngle >= 135.0 && actualRotationAngle < 180.0 || flag4)
                num31 += num29 + num30;
              else if (actualRotationAngle >= 180.0 && actualRotationAngle < 225.0 || actualRotationAngle >= 315.0 && actualRotationAngle < 360.0)
                num31 -= num29 + num30;
              else if (actualRotationAngle >= 225.0 && actualRotationAngle < 315.0)
                num31 -= num29;
              else
                num31 += num29;
            }
            else if (actualRotationAngle >= 135.0 && actualRotationAngle < 180.0 || actualRotationAngle >= 315.0 && actualRotationAngle < 360.0)
              elementLeftShift2 = UIElementLeftShift.LeftShift;
            else if (actualRotationAngle >= 0.0 && actualRotationAngle < 45.0 || actualRotationAngle >= 180.0 && actualRotationAngle < 225.0)
              elementLeftShift2 = UIElementLeftShift.Default;
            double num32 = num31;
            arrangeRect = this.Axis.ArrangeRect;
            double left = arrangeRect.Left;
            ((SfChart3D) this.Axis.Area).Graphics3D.AddVisual((Polygon3D) new UIElement3D((UIElement) this.HeaderContent, ChartCartesianAxisPanel3D.GetHeaderVectoColl(num32 + left, top3, num18, false))
            {
              LeftShift = elementLeftShift2,
              TopShift = uiElementTopShift2
            });
          }
          else
          {
            if (flag5)
            {
              if (actualRotationAngle >= 45.0 && actualRotationAngle < 90.0 || actualRotationAngle >= 270.0 && actualRotationAngle < 315.0 || flag4)
                depth -= num29 + num30;
              else if (actualRotationAngle >= 90.0 && actualRotationAngle < 135.0 || actualRotationAngle >= 225.0 && actualRotationAngle < 270.0)
                depth += num29 + num30;
              else if (actualRotationAngle >= 315.0 || actualRotationAngle < 45.0)
                depth -= num29;
              else
                depth += num29;
            }
            else if (actualRotationAngle >= 45.0 && actualRotationAngle < 90.0 || actualRotationAngle >= 225.0 && actualRotationAngle < 270.0)
              elementLeftShift2 = UIElementLeftShift.LeftShift;
            else if (actualRotationAngle >= 90.0 && actualRotationAngle < 135.0 || actualRotationAngle >= 270.0 && actualRotationAngle < 315.0)
              elementLeftShift2 = UIElementLeftShift.Default;
            depth += this.Axis.AxisDepth;
            ((SfChart3D) this.Axis.Area).Graphics3D.AddVisual((Polygon3D) new UIElement3D((UIElement) this.HeaderContent, ChartCartesianAxisPanel3D.GetHeaderVectoColl(num18, top3, depth, true))
            {
              LeftShift = elementLeftShift2,
              TopShift = uiElementTopShift2
            });
          }
        }
        else
          layoutCalculator3.Top = !flag3 ? num1 + num2 / 2.0 : num1 - num2 / 2.0;
        double num33 = num1;
        desiredSize = sizeList[index];
        double height1 = desiredSize.Height;
        num1 = num33 + height1;
      }
    }
    foreach (ILayoutCalculator layoutCalculator4 in this.LayoutCalc)
      layoutCalculator4.Arrange(layoutCalculator4.DesiredSize);
  }

  private static Vector3D[] GetHeaderVectoColl(
    double left,
    double top,
    double depth,
    bool isFront)
  {
    Vector3D[] headerVectoColl = new Vector3D[3];
    if (isFront)
    {
      headerVectoColl[0] = new Vector3D(left, top, depth);
      headerVectoColl[1] = new Vector3D(left, top + 10.0, depth);
      headerVectoColl[2] = new Vector3D(left + 10.0, top + 10.0, depth);
    }
    else
    {
      headerVectoColl[0] = new Vector3D(left, top, depth);
      headerVectoColl[1] = new Vector3D(left, top + 10.0, depth);
      headerVectoColl[2] = new Vector3D(left, top + 10.0, depth + 10.0);
    }
    return headerVectoColl;
  }
}
