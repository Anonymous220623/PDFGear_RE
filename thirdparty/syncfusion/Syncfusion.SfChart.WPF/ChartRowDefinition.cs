// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.ChartRowDefinition
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

public class ChartRowDefinition : DependencyObject, ICloneable
{
  public static readonly DependencyProperty HeightProperty = DependencyProperty.Register(nameof (Height), typeof (double), typeof (ChartRowDefinition), new PropertyMetadata((object) 1.0, new PropertyChangedCallback(ChartRowDefinition.OnRowPropertyChanged)));
  public static readonly DependencyProperty UnitProperty = DependencyProperty.Register(nameof (Unit), typeof (ChartUnitType), typeof (ChartRowDefinition), new PropertyMetadata((object) ChartUnitType.Star, new PropertyChangedCallback(ChartRowDefinition.OnRowPropertyChanged)));
  public static readonly DependencyProperty BorderThicknessProperty = DependencyProperty.Register(nameof (BorderThickness), typeof (double), typeof (ChartRowDefinition), new PropertyMetadata((object) 0.0));
  public static readonly DependencyProperty BorderStrokeProperty = DependencyProperty.Register(nameof (BorderStroke), typeof (Brush), typeof (ChartRowDefinition), new PropertyMetadata((object) new SolidColorBrush(Colors.Red)));
  internal Line BorderLine;
  private List<ChartAxis> axis;
  private double headerMargin = 2.0;
  private double computedHeight;
  private double computedTop;
  private List<ChartLegend> legends = new List<ChartLegend>();

  public ChartRowDefinition() => this.axis = new List<ChartAxis>();

  public double RowTop { get; internal set; }

  public double Height
  {
    get => (double) this.GetValue(ChartRowDefinition.HeightProperty);
    set => this.SetValue(ChartRowDefinition.HeightProperty, (object) value);
  }

  public ChartUnitType Unit
  {
    get => (ChartUnitType) this.GetValue(ChartRowDefinition.UnitProperty);
    set => this.SetValue(ChartRowDefinition.UnitProperty, (object) value);
  }

  public double BorderThickness
  {
    get => (double) this.GetValue(ChartRowDefinition.BorderThicknessProperty);
    set => this.SetValue(ChartRowDefinition.BorderThicknessProperty, (object) value);
  }

  public Brush BorderStroke
  {
    get => (Brush) this.GetValue(ChartRowDefinition.BorderStrokeProperty);
    set => this.SetValue(ChartRowDefinition.BorderStrokeProperty, (object) value);
  }

  internal double ComputedHeight
  {
    get => this.computedHeight;
    set => this.computedHeight = value;
  }

  internal double ComputedTop
  {
    get => this.computedTop;
    set => this.computedTop = value;
  }

  internal List<ChartAxis> Axis
  {
    get => this.axis;
    set => this.axis = value;
  }

  internal List<ChartLegend> Legends
  {
    get => this.legends;
    set => this.legends = value;
  }

  public DependencyObject Clone()
  {
    return (DependencyObject) new ChartRowDefinition()
    {
      BorderStroke = this.BorderStroke,
      BorderThickness = this.BorderThickness,
      Height = this.Height,
      Unit = this.Unit
    };
  }

  internal void MeasureLegends(Size size, List<double> nearSizes, List<double> farSizes)
  {
    int index1 = 0;
    int index2 = 0;
    foreach (ChartLegend legend in this.Legends)
    {
      if (legend != null && legend.DockPosition != ChartDock.Floating && legend.GetPosition() == LegendPosition.Outside)
      {
        if (legend.DesiredSize.Width == 0.0)
          legend.Measure(size);
        if (legend.DockPosition == ChartDock.Left)
        {
          if (nearSizes.Count <= index1)
            nearSizes.Add(legend.DesiredSize.Width);
          else if (nearSizes[index1] < legend.DesiredSize.Width)
            nearSizes[index1] = legend.DesiredSize.Width;
          ++index1;
        }
        else
        {
          if (farSizes.Count <= index2)
            farSizes.Add(legend.DesiredSize.Width);
          else if (farSizes[index2] < legend.DesiredSize.Width)
            farSizes[index2] = legend.DesiredSize.Width;
          ++index2;
        }
      }
    }
  }

  internal void Measure(
    Size size,
    List<double> nearSizes,
    List<double> farSizes,
    bool isFirstLayout)
  {
    int index1 = 0;
    int index2 = 0;
    bool flag1 = true;
    bool flag2 = true;
    double newHeight = 0.0;
    double newTop = 0.0;
    if (this.axis.Count > 0 && this.axis[0] is ChartAxisBase3D)
    {
      foreach (ChartAxis axi in this.axis)
      {
        if (axi != null)
        {
          bool flag3 = axi.OpposedPosition;
          if (axi.Area != null)
          {
            this.CalcRowSpanAxisWidthandTop(newTop, axi.Area.GetActualRowSpan((UIElement) axi), size.Height, axi, out newTop, out newHeight);
            if (axi.Area.GetActualRow((UIElement) axi) == axi.Area.RowDefinitions.IndexOf(this))
              axi.ComputeDesiredSize(new Size(size.Width, newHeight));
          }
          if (axi.ShowAxisNextToOrigin && axi.Area != null && axi.Area.ColumnDefinitions.Count <= 1)
          {
            bool isContinue = false;
            this.MeasureRectBasedOnOrigin(axi, nearSizes, farSizes, ref isContinue, isFirstLayout);
            if (isContinue)
              continue;
          }
          SfChart3D area = axi.Area as SfChart3D;
          ChartAxisBase3D chartAxisBase3D = axi as ChartAxisBase3D;
          if (!axi.OpposedPosition)
          {
            double num1 = area.Rotation % 360.0;
            double num2 = area.ActualRotationAngle = num1 < 0.0 ? num1 + 360.0 : num1;
            if (num2 >= 45.0 && num2 < 90.0)
            {
              flag3 = true;
              chartAxisBase3D.AxisPosition3D = AxisPosition3D.DepthBackRight;
            }
            else if (num2 >= 90.0 && num2 < 135.0)
            {
              flag3 = true;
              chartAxisBase3D.AxisPosition3D = AxisPosition3D.DepthFrontRight;
            }
            else if (num2 >= 135.0 && num2 < 180.0)
              chartAxisBase3D.AxisPosition3D = AxisPosition3D.BackLeft;
            else if (num2 >= 180.0 && num2 < 225.0)
            {
              flag3 = true;
              chartAxisBase3D.AxisPosition3D = AxisPosition3D.BackRight;
            }
            else if (num2 >= 225.0 && num2 < 270.0)
              chartAxisBase3D.AxisPosition3D = AxisPosition3D.DepthFrontLeft;
            else if (num2 >= 270.0 && num2 < 315.0)
              chartAxisBase3D.AxisPosition3D = AxisPosition3D.DepthBackLeft;
            else if (num2 >= 315.0 && num2 < 360.0)
            {
              flag3 = true;
              chartAxisBase3D.AxisPosition3D = AxisPosition3D.FrontRight;
            }
            else
              chartAxisBase3D.AxisPosition3D = AxisPosition3D.FrontLeft;
          }
          else
          {
            flag3 = true;
            chartAxisBase3D.AxisPosition3D = AxisPosition3D.FrontRight;
          }
          if (flag3)
          {
            double num = flag1 ? axi.InsidePadding : 0.0;
            if (farSizes.Count <= index2)
              farSizes.Add(axi.ComputedDesiredSize.Width - num);
            else if (farSizes[index2] < axi.ComputedDesiredSize.Width - num)
              farSizes[index2] = axi.ComputedDesiredSize.Width - num;
            ++index2;
            flag1 = false;
          }
          else
          {
            double num = flag2 ? axi.InsidePadding : 0.0;
            if (nearSizes.Count <= index1)
              nearSizes.Add(axi.ComputedDesiredSize.Width - num);
            else if (nearSizes[index1] < axi.ComputedDesiredSize.Width - num)
              nearSizes[index1] = axi.ComputedDesiredSize.Width - num;
            ++index1;
            flag2 = false;
          }
        }
      }
    }
    else
    {
      foreach (ChartAxis axi in this.axis)
      {
        if (axi != null)
        {
          if (axi.Area != null)
          {
            this.CalcRowSpanAxisWidthandTop(newTop, axi.Area.GetActualRowSpan((UIElement) axi), size.Height, axi, out newTop, out newHeight);
            if (axi.Area.GetActualRow((UIElement) axi) == axi.Area.RowDefinitions.IndexOf(this))
              axi.ComputeDesiredSize(new Size(size.Width, newHeight));
          }
          if (axi.ShowAxisNextToOrigin && axi.Area != null && axi.Area.ColumnDefinitions.Count <= 1)
          {
            bool isContinue = false;
            this.MeasureRectBasedOnOrigin(axi, nearSizes, farSizes, ref isContinue, isFirstLayout);
            if (isContinue)
              continue;
          }
          if (axi.OpposedPosition)
          {
            double num = flag1 ? axi.InsidePadding : 0.0;
            if (farSizes.Count <= index2)
              farSizes.Add(axi.ComputedDesiredSize.Width - num);
            else if (farSizes[index2] < axi.ComputedDesiredSize.Width - num)
              farSizes[index2] = axi.ComputedDesiredSize.Width - num;
            ++index2;
            flag1 = false;
          }
          else
          {
            double num = flag2 ? axi.InsidePadding : 0.0;
            if (nearSizes.Count <= index1)
              nearSizes.Add(axi.ComputedDesiredSize.Width - num);
            else if (nearSizes[index1] < axi.ComputedDesiredSize.Width - num)
              nearSizes[index1] = axi.ComputedDesiredSize.Width - num;
            ++index1;
            flag2 = false;
          }
        }
      }
    }
  }

  internal void UpdateLegendArrangeRect(
    double top,
    double height,
    double areaWidth,
    List<double> nearSizes,
    List<double> farSizes)
  {
    int index1 = 0;
    int index2 = 0;
    double num1 = nearSizes.Sum();
    double num2 = farSizes.Sum();
    double num3 = num2;
    double newHeight = height;
    double newTop = top;
    for (int index3 = 0; index3 < this.Legends.Count; ++index3)
    {
      ChartLegend legend = this.Legends[index3];
      if (legend != null && legend.DockPosition != ChartDock.Floating && legend.GetPosition() == LegendPosition.Outside)
      {
        if (legend.ChartArea != null && legend.YAxis != null)
          this.CalcRowSpanAxisWidthandTop(top, legend.ChartArea.GetActualRowSpan((UIElement) legend), height, legend.YAxis, out newTop, out newHeight);
        Size desiredSize = legend.DesiredSize;
        if (legend.DockPosition == ChartDock.Left)
        {
          legend.ArrangeRect = new Rect(num1 - desiredSize.Width, newTop, desiredSize.Width, newHeight);
          num1 -= nearSizes[index1];
          ++index1;
        }
        else
        {
          legend.ArrangeRect = new Rect(areaWidth + num3 - num2, newTop, desiredSize.Width, newHeight);
          num2 -= farSizes[index2];
          ++index2;
        }
      }
    }
  }

  internal void UpdateArrangeRect(
    double top,
    double height,
    double areaWidth,
    List<double> nearSizes,
    List<double> farSizes)
  {
    int index1 = 0;
    int index2 = 0;
    bool flag1 = true;
    bool flag2 = true;
    double num1 = nearSizes.Sum();
    double num2 = farSizes.Sum();
    double newHeight = 0.0;
    double newTop = 0.0;
    int actualRowIndex = 0;
    int elementRowIndex = 0;
    if (this.axis.Count > 0 && this.axis[0] is ChartAxisBase3D)
    {
      for (int index3 = 0; index3 < this.Axis.Count; ++index3)
      {
        ChartAxis axi = this.Axis[index3];
        if (axi != null)
        {
          if (axi.Area != null)
          {
            elementRowIndex = axi.Area.GetActualRow((UIElement) axi);
            actualRowIndex = axi.Area.RowDefinitions.IndexOf(this);
            this.CalcRowSpanAxisWidthandTop(top, axi.Area.GetActualRowSpan((UIElement) axi), height, axi, out newTop, out newHeight);
          }
          Size computedDesiredSize = axi.ComputedDesiredSize;
          try
          {
            SfChart3D area = axi.Area as SfChart3D;
            ChartAxisBase3D chartAxisBase3D = axi as ChartAxisBase3D;
            chartAxisBase3D.AxisDepth = 0.0;
            double actualRotationAngle = area.ActualRotationAngle;
            bool flag3 = axi.OpposedPosition || axi.ShowAxisNextToOrigin && actualRotationAngle >= 180.0 && actualRotationAngle < 360.0;
            if (axi.ShowAxisNextToOrigin && axi.Area != null && axi.Area.ColumnDefinitions.Count <= 1)
            {
              bool isContinue = false;
              this.ArragneRectBasedOnOrigin(axi, elementRowIndex, actualRowIndex, newTop, areaWidth, newHeight, ref isContinue);
              if (isContinue)
                continue;
            }
            chartAxisBase3D.AxisDepth = 0.0;
            if (elementRowIndex == actualRowIndex)
            {
              double num3 = 0.0;
              double x;
              switch (chartAxisBase3D.AxisPosition3D)
              {
                case AxisPosition3D.DepthFrontLeft:
                  chartAxisBase3D.AxisDepth = -computedDesiredSize.Width + axi.InsidePadding;
                  x = num1 - 1.0;
                  break;
                case AxisPosition3D.FrontRight:
                  x = areaWidth - num2 - axi.InsidePadding;
                  break;
                case AxisPosition3D.DepthFrontRight:
                  chartAxisBase3D.AxisDepth = axi.InsidePadding;
                  x = areaWidth - num2 + 1.0;
                  break;
                case AxisPosition3D.BackRight:
                  chartAxisBase3D.AxisDepth = area.ActualDepth + 1.0;
                  x = areaWidth - computedDesiredSize.Width;
                  break;
                case AxisPosition3D.DepthBackRight:
                  chartAxisBase3D.AxisDepth = area.ActualDepth - axi.InsidePadding;
                  x = areaWidth - num2;
                  break;
                case AxisPosition3D.BackLeft:
                  chartAxisBase3D.AxisDepth = area.ActualDepth + 1.0;
                  x = 0.0;
                  break;
                case AxisPosition3D.DepthBackLeft:
                  chartAxisBase3D.AxisDepth = area.ActualDepth + computedDesiredSize.Width - axi.InsidePadding;
                  x = num1 - 1.0;
                  break;
                default:
                  double num4;
                  if (!flag3)
                    num4 = num1 - computedDesiredSize.Width + axi.InsidePadding;
                  else
                    num3 = num4 = areaWidth - num2 - axi.InsidePadding;
                  x = num4;
                  break;
              }
              axi.ArrangeRect = new Rect(x, newTop, computedDesiredSize.Width, newHeight);
              axi.Measure(new Size(axi.ArrangeRect.Width, axi.ArrangeRect.Height));
            }
          }
          catch (Exception ex)
          {
          }
        }
      }
    }
    else
    {
      for (int index4 = 0; index4 < this.Axis.Count; ++index4)
      {
        ChartAxis axi = this.Axis[index4];
        if (axi != null)
        {
          if (axi.Area != null)
          {
            elementRowIndex = axi.Area.GetActualRow((UIElement) axi);
            actualRowIndex = axi.Area.RowDefinitions.IndexOf(this);
            this.CalcRowSpanAxisWidthandTop(top, axi.Area.GetActualRowSpan((UIElement) axi), height, axi, out newTop, out newHeight);
          }
          Size computedDesiredSize = axi.ComputedDesiredSize;
          try
          {
            if (axi.ShowAxisNextToOrigin && axi.Area != null && axi.Area.ColumnDefinitions.Count <= 1)
            {
              bool isContinue = false;
              this.ArragneRectBasedOnOrigin(axi, elementRowIndex, actualRowIndex, newTop, areaWidth, newHeight, ref isContinue);
              if (isContinue)
                continue;
            }
            if (axi.OpposedPosition)
            {
              double num5 = flag1 ? axi.InsidePadding : 0.0;
              if (elementRowIndex == actualRowIndex)
                axi.ArrangeRect = new Rect(areaWidth - num2 - num5, newTop, computedDesiredSize.Width, newHeight);
              axi.Measure(new Size(axi.ArrangeRect.Width, axi.ArrangeRect.Height));
              num2 -= farSizes[index2];
              ++index2;
              flag1 = false;
            }
            else
            {
              double num6 = flag2 ? axi.InsidePadding : 0.0;
              if (elementRowIndex == actualRowIndex)
                axi.ArrangeRect = new Rect(num1 - computedDesiredSize.Width + num6, newTop, computedDesiredSize.Width, newHeight);
              axi.Measure(new Size(axi.ArrangeRect.Width, axi.ArrangeRect.Height));
              num1 -= nearSizes[index1];
              ++index1;
              flag2 = false;
            }
          }
          catch (Exception ex)
          {
          }
        }
      }
    }
  }

  internal void Arrange()
  {
    foreach (ChartAxis axi in this.Axis)
    {
      Canvas.SetLeft((UIElement) axi, axi.ArrangeRect.Left);
      Canvas.SetTop((UIElement) axi, axi.ArrangeRect.Top);
    }
    this.RenderBorderLine();
  }

  internal void Dispose()
  {
    if (this.Legends != null)
    {
      this.Legends.Clear();
      this.Legends = (List<ChartLegend>) null;
    }
    if (this.Axis == null)
      return;
    this.Axis.Clear();
    this.Axis = (List<ChartAxis>) null;
  }

  private static void OnRowPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    if (!(d is ChartRowDefinition chartRowDefinition) || chartRowDefinition.Axis == null || chartRowDefinition.Axis.Count <= 0)
      return;
    chartRowDefinition.Axis[0].Area.ScheduleUpdate();
  }

  private void MeasureRectBasedOnOrigin(
    ChartAxis content,
    List<double> nearSizes,
    List<double> farSizes,
    ref bool isContinue,
    bool isFirstLayout)
  {
    double num1 = 0.0;
    ChartAxis chartAxis = (ChartAxis) null;
    if (content.Area.InternalSecondaryAxis == content)
    {
      num1 = content.Area.InternalPrimaryAxis.ValueToCoefficientCalc(content.Origin);
      chartAxis = content.Area.InternalPrimaryAxis;
    }
    else if (content.Area.InternalSecondaryAxis.Orientation == Orientation.Horizontal && content.Area.InternalPrimaryAxis == content)
    {
      num1 = content.Area.InternalSecondaryAxis.ValueToCoefficientCalc(content.Origin);
      chartAxis = content.Area.InternalSecondaryAxis;
    }
    Size computedDesiredSize = content.ComputedDesiredSize;
    if ((content.Origin == 0.0 || !isFirstLayout) && (0.0 >= num1 || 1.0 <= num1))
      return;
    if (content.HeaderPosition == AxisHeaderPosition.Far)
    {
      SfChart3D area = content.Area as SfChart3D;
      double actualPlotOffset = chartAxis.GetActualPlotOffset();
      bool flag = area != null && area.ActualRotationAngle >= 180.0 && area.ActualRotationAngle < 360.0 || content.OpposedPosition;
      double num2 = flag ? (1.0 - num1) * (content.Area.SeriesClipRect.Width - actualPlotOffset) + chartAxis.GetActualPlotOffsetEnd() + content.InsidePadding : num1 * (content.Area.SeriesClipRect.Width - actualPlotOffset) + chartAxis.GetActualPlotOffsetStart() + content.InsidePadding;
      if (flag && num2 > computedDesiredSize.Width - content.headerContent.DesiredSize.Height)
        farSizes.Add(content.headerContent.DesiredSize.Height + this.headerMargin);
      else if (num2 > computedDesiredSize.Width - content.headerContent.DesiredSize.Height)
        nearSizes.Add(content.headerContent.DesiredSize.Height + this.headerMargin);
      isContinue = true;
    }
    else
      isContinue = true;
  }

  private void CalcRowSpanAxisWidthandTop(
    double oldTop,
    int rowSpan,
    double oldHeight,
    ChartAxis axis,
    out double newTop,
    out double newHeight)
  {
    int actualRow = axis.Area.GetActualRow((UIElement) axis);
    if (axis.Area != null && rowSpan > 1 && actualRow == axis.Area.RowDefinitions.IndexOf(this))
    {
      ChartRowDefinitions rowDefinitions = axis.Area.RowDefinitions;
      int index = rowDefinitions.IndexOf(this);
      int num = 0;
      newTop = 0.0;
      newHeight = 0.0;
      for (; index < rowDefinitions.Count; ++index)
      {
        if (num < rowSpan)
        {
          newHeight += rowDefinitions[index].computedHeight;
          newTop = rowDefinitions[index].ComputedTop;
          ++num;
        }
      }
    }
    else
    {
      newTop = oldTop;
      newHeight = oldHeight;
    }
  }

  private void ArragneRectBasedOnOrigin(
    ChartAxis element,
    int elementRowIndex,
    int actualRowIndex,
    double axisTop,
    double areaWidth,
    double axisHeight,
    ref bool isContinue)
  {
    ChartAxis chartAxis = (ChartAxis) null;
    Size computedDesiredSize = element.ComputedDesiredSize;
    if (element.Area.InternalSecondaryAxis == element)
      chartAxis = element.Area.InternalPrimaryAxis;
    else if (element.Area.InternalSecondaryAxis.Orientation == Orientation.Horizontal && element.Area.InternalPrimaryAxis == element)
      chartAxis = element.Area.InternalSecondaryAxis;
    if (chartAxis == null)
      return;
    Rect rect = ChartLayoutUtils.Subtractthickness(new Rect(new Point(0.0, 0.0), element.AvailableSize), element.Area.AxisThickness);
    double num1 = chartAxis.ValueToCoefficientCalc(element.Origin);
    double actualPlotOffset = chartAxis.GetActualPlotOffset();
    if (0.0 >= num1 || 1.0 <= num1)
      return;
    if ((element.Area is SfChart3D area && area.ActualRotationAngle >= 180.0 && area.ActualRotationAngle < 360.0 || element.OpposedPosition) && elementRowIndex == actualRowIndex)
    {
      double num2 = element.HeaderPosition == AxisHeaderPosition.Far ? (1.0 - num1) * (rect.Width - actualPlotOffset) + chartAxis.GetActualPlotOffsetStart() + element.InsidePadding : 0.0;
      element.ArrangeRect = num2 <= 0.0 || num2 <= computedDesiredSize.Width - element.headerContent.DesiredSize.Height ? new Rect((areaWidth - actualPlotOffset) * num1 - element.InsidePadding + chartAxis.GetActualPlotOffsetStart(), axisTop, computedDesiredSize.Width, axisHeight) : new Rect((rect.Width - actualPlotOffset) * num1 - element.InsidePadding + chartAxis.GetActualPlotOffsetStart(), axisTop, computedDesiredSize.Width, axisHeight);
    }
    else if (elementRowIndex == actualRowIndex)
    {
      double num3 = element.HeaderPosition == AxisHeaderPosition.Far ? num1 * (rect.Width - actualPlotOffset) + chartAxis.GetActualPlotOffsetStart() + element.InsidePadding : 0.0;
      element.ArrangeRect = num3 <= 0.0 || num3 <= computedDesiredSize.Width - element.headerContent.DesiredSize.Height ? new Rect((areaWidth - actualPlotOffset) * num1 - computedDesiredSize.Width + element.InsidePadding + chartAxis.GetActualPlotOffsetStart(), axisTop, computedDesiredSize.Width, axisHeight) : new Rect(num3 - computedDesiredSize.Width + element.headerContent.DesiredSize.Height + this.headerMargin, axisTop, computedDesiredSize.Width, axisHeight);
    }
    element.Measure(new Size(element.ArrangeRect.Width, element.ArrangeRect.Height));
    isContinue = true;
  }

  private void RenderBorderLine()
  {
    if (this.BorderLine == null)
    {
      this.BorderLine = new Line();
      this.BindBorder((UIElement) this.BorderLine);
    }
    if (this.Axis == null || this.Axis.Count <= 0)
      return;
    ChartAxis chartAxis = this.Axis.FirstOrDefault<ChartAxis>();
    if (chartAxis.Area == null)
      return;
    this.BorderLine.X1 = 0.0;
    this.BorderLine.X2 = chartAxis.Area.SeriesClipRect.Width;
    Line borderLine1 = this.BorderLine;
    Line borderLine2 = this.BorderLine;
    double top1 = chartAxis.ArrangeRect.Top;
    double top2 = chartAxis.Area.SeriesClipRect.Top;
    double num1;
    double num2 = num1 = top1 - top2;
    borderLine2.Y2 = num1;
    double num3 = num2;
    borderLine1.Y1 = num3;
  }

  private void BindBorder(UIElement element)
  {
    BindingOperations.SetBinding((DependencyObject) element, Shape.StrokeProperty, (BindingBase) new Binding()
    {
      Source = (object) this,
      Path = new PropertyPath("BorderStroke", new object[0])
    });
    BindingOperations.SetBinding((DependencyObject) element, Shape.StrokeThicknessProperty, (BindingBase) new Binding()
    {
      Source = (object) this,
      Path = new PropertyPath("BorderThickness", new object[0])
    });
  }
}
