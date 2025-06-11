// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.ChartColumnDefinition
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

public class ChartColumnDefinition : DependencyObject, ICloneable
{
  public static readonly DependencyProperty WidthProperty = DependencyProperty.Register(nameof (Width), typeof (double), typeof (ChartColumnDefinition), new PropertyMetadata((object) 1.0, new PropertyChangedCallback(ChartColumnDefinition.OnColumnPropertyChanged)));
  public static readonly DependencyProperty UnitProperty = DependencyProperty.Register(nameof (Unit), typeof (ChartUnitType), typeof (ChartColumnDefinition), new PropertyMetadata((object) ChartUnitType.Star, new PropertyChangedCallback(ChartColumnDefinition.OnColumnPropertyChanged)));
  public static readonly DependencyProperty BorderThicknessProperty = DependencyProperty.Register(nameof (BorderThickness), typeof (double), typeof (ChartColumnDefinition), new PropertyMetadata((object) 0.0));
  public static readonly DependencyProperty BorderStrokeProperty = DependencyProperty.Register(nameof (BorderStroke), typeof (Brush), typeof (ChartColumnDefinition), new PropertyMetadata((object) new SolidColorBrush(Colors.Red)));
  internal Line BorderLine;
  private List<ChartAxis> axis;
  private double headerMargin = 2.0;
  private double computedWidth;
  private double computedLeft;
  private List<ChartLegend> legends = new List<ChartLegend>();

  public ChartColumnDefinition() => this.axis = new List<ChartAxis>();

  public double Width
  {
    get => (double) this.GetValue(ChartColumnDefinition.WidthProperty);
    set => this.SetValue(ChartColumnDefinition.WidthProperty, (object) value);
  }

  public ChartUnitType Unit
  {
    get => (ChartUnitType) this.GetValue(ChartColumnDefinition.UnitProperty);
    set => this.SetValue(ChartColumnDefinition.UnitProperty, (object) value);
  }

  public double BorderThickness
  {
    get => (double) this.GetValue(ChartColumnDefinition.BorderThicknessProperty);
    set => this.SetValue(ChartColumnDefinition.BorderThicknessProperty, (object) value);
  }

  public Brush BorderStroke
  {
    get => (Brush) this.GetValue(ChartColumnDefinition.BorderStrokeProperty);
    set => this.SetValue(ChartColumnDefinition.BorderStrokeProperty, (object) value);
  }

  internal double ComputedWidth
  {
    get => this.computedWidth;
    set => this.computedWidth = value;
  }

  internal double ComputedLeft
  {
    get => this.computedLeft;
    set => this.computedLeft = value;
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
    return (DependencyObject) new ChartColumnDefinition()
    {
      BorderStroke = this.BorderStroke,
      BorderThickness = this.BorderThickness,
      Width = this.Width,
      Unit = this.Unit
    };
  }

  internal void Measure(Size size, List<double> nearSizes, List<double> farSizes)
  {
    int index1 = 0;
    int index2 = 0;
    bool flag1 = true;
    bool flag2 = true;
    if (this.axis.Count > 1 && this.axis[0] is ChartAxisBase3D)
    {
      bool flag3 = this.axis.Where<ChartAxis>((Func<ChartAxis, bool>) (x => x is ChartAxisBase3D && x.Orientation == Orientation.Horizontal && x.OpposedPosition)).Any<ChartAxis>() && this.axis.Where<ChartAxis>((Func<ChartAxis, bool>) (x => x is ChartAxisBase3D && (x as ChartAxisBase3D).IsZAxis)).Any<ChartAxis>();
      for (int index3 = 0; index3 < this.axis.Count; ++index3)
      {
        ChartAxis axi = this.Axis[index3];
        flag3 = flag3 || axi.OpposedPosition;
        if (axi != null)
        {
          int actualColumnSpan = axi.Area != null ? axi.Area.GetActualColumnSpan((UIElement) axi) : 0;
          double width = this.CalcColumnSpanAxisWidth(size.Width, axi, actualColumnSpan);
          if (axi.Area != null && axi.Area.GetActualColumn((UIElement) axi) == axi.Area.ColumnDefinitions.IndexOf(this))
            axi.ComputeDesiredSize(new Size(width, size.Height));
          if (axi.ShowAxisNextToOrigin && axi.Area != null && axi.Area.RowDefinitions.Count <= 1)
          {
            bool isContinue = false;
            this.MeasureRectBasedOnOrigin(axi, nearSizes, farSizes, ref isContinue);
            if (isContinue)
              continue;
          }
          if (flag3)
          {
            double num = flag1 ? axi.InsidePadding : 0.0;
            if (farSizes.Count <= index2)
              farSizes.Add(axi.ComputedDesiredSize.Height - num);
            else if (farSizes[index2] < axi.ComputedDesiredSize.Height - num)
              farSizes[index2] = axi.ComputedDesiredSize.Height - num;
            if (!(this.Axis[index3] as ChartAxisBase3D).IsZAxis && (index3 + 1 >= this.Axis.Count || !(this.Axis[index3 + 1] as ChartAxisBase3D).IsZAxis))
            {
              ++index2;
              flag1 = false;
            }
          }
          else
          {
            double num = flag2 ? axi.InsidePadding : 0.0;
            if (nearSizes.Count <= index1)
              nearSizes.Add(axi.ComputedDesiredSize.Height - num);
            else if (nearSizes[index1] < axi.ComputedDesiredSize.Height - num)
              nearSizes[index1] = axi.ComputedDesiredSize.Height - num;
            if (!(this.Axis[index3] as ChartAxisBase3D).IsZAxis && (index3 + 1 >= this.Axis.Count || !(this.Axis[index3 + 1] as ChartAxisBase3D).IsZAxis))
            {
              ++index1;
              flag2 = false;
            }
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
          int actualColumnSpan = axi.Area != null ? axi.Area.GetActualColumnSpan((UIElement) axi) : 0;
          double width = this.CalcColumnSpanAxisWidth(size.Width, axi, actualColumnSpan);
          if (axi.Area != null && axi.Area.GetActualColumn((UIElement) axi) == axi.Area.ColumnDefinitions.IndexOf(this))
            axi.ComputeDesiredSize(new Size(width, size.Height));
          if (axi.ShowAxisNextToOrigin && axi.Area != null && axi.Area.RowDefinitions.Count <= 1)
          {
            bool isContinue = false;
            this.MeasureRectBasedOnOrigin(axi, nearSizes, farSizes, ref isContinue);
            if (isContinue)
              continue;
          }
          if (axi.OpposedPosition)
          {
            double num = flag1 ? axi.InsidePadding : 0.0;
            if (farSizes.Count <= index2)
              farSizes.Add(axi.ComputedDesiredSize.Height - num);
            else if (farSizes[index2] < axi.ComputedDesiredSize.Height - num)
              farSizes[index2] = axi.ComputedDesiredSize.Height - num;
            ++index2;
            flag1 = false;
          }
          else
          {
            double num = flag2 ? axi.InsidePadding : 0.0;
            if (nearSizes.Count <= index1)
              nearSizes.Add(axi.ComputedDesiredSize.Height - num);
            else if (nearSizes[index1] < axi.ComputedDesiredSize.Height - num)
              nearSizes[index1] = axi.ComputedDesiredSize.Height - num;
            ++index1;
            flag2 = false;
          }
        }
      }
    }
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
        if (legend.DockPosition == ChartDock.Top)
        {
          if (farSizes.Count <= index2)
            farSizes.Add(legend.DesiredSize.Height);
          else if (farSizes[index2] < legend.DesiredSize.Height)
            farSizes[index2] = legend.DesiredSize.Height;
          ++index2;
        }
        else
        {
          if (nearSizes.Count <= index1)
            nearSizes.Add(legend.DesiredSize.Height);
          else if (nearSizes[index1] < legend.DesiredSize.Height)
            nearSizes[index1] = legend.DesiredSize.Height;
          ++index1;
        }
      }
    }
  }

  internal void UpdateLegendsArrangeRect(
    double left,
    double width,
    double areaHeight,
    List<double> nearSizes,
    List<double> farSizes)
  {
    int index1 = 0;
    int index2 = 0;
    double num1 = nearSizes.Sum();
    double num2 = farSizes.Sum();
    double num3 = num1;
    for (int index3 = 0; index3 < this.Legends.Count; ++index3)
    {
      ChartLegend legend = this.Legends[index3];
      if (legend != null && legend.DockPosition != ChartDock.Floating && legend.GetPosition() == LegendPosition.Outside)
      {
        double width1 = legend.XAxis == null || legend.ChartArea == null ? width : this.CalcColumnSpanAxisWidth(width, legend.XAxis, legend.ChartArea.GetActualColumnSpan((UIElement) legend));
        Size desiredSize = legend.DesiredSize;
        if (legend.DockPosition == ChartDock.Bottom)
        {
          legend.ArrangeRect = new Rect(left, areaHeight + num3 - num1, width1, desiredSize.Height);
          if (index1 < nearSizes.Count)
            num1 -= nearSizes[index1];
          ++index1;
        }
        else
        {
          legend.ArrangeRect = new Rect(left, num2 - desiredSize.Height, width1, desiredSize.Height);
          if (index2 < farSizes.Count)
            num2 -= farSizes[index2];
          ++index2;
        }
      }
    }
  }

  internal void UpdateArrangeRect(
    double left,
    double width,
    double areaHeight,
    List<double> nearSizes,
    List<double> farSizes)
  {
    int index1 = 0;
    int index2 = 0;
    bool flag1 = true;
    bool flag2 = true;
    double num1 = nearSizes.Sum();
    double num2 = farSizes.Sum();
    double num3 = 0.0;
    int actualColumnIndex = 0;
    int elementColumnIndex = 0;
    if (this.axis.Count > 0 && this.axis[0] is ChartAxisBase3D)
    {
      bool flag3 = this.Axis.Where<ChartAxis>((Func<ChartAxis, bool>) (x => x is ChartAxisBase3D && x.Orientation == Orientation.Horizontal && x.OpposedPosition)).Any<ChartAxis>();
      for (int index3 = 0; index3 < this.Axis.Count; ++index3)
      {
        ChartAxis axi = this.Axis[index3];
        flag3 = flag3 || axi.OpposedPosition;
        if (axi != null)
        {
          if (axi.Area != null)
          {
            num3 = this.CalcColumnSpanAxisWidth(width, axi, axi.Area.GetActualColumnSpan((UIElement) axi));
            actualColumnIndex = axi.Area.ColumnDefinitions.IndexOf(this);
            elementColumnIndex = axi.Area.GetActualColumn((UIElement) axi);
          }
          Size computedDesiredSize = axi.ComputedDesiredSize;
          try
          {
            ChartAxisBase3D chartAxisBase3D = axi as ChartAxisBase3D;
            SfChart3D area = axi.Area as SfChart3D;
            ChartAxis chartAxis = area.InternalSecondaryAxis.Orientation == Orientation.Vertical ? area.InternalSecondaryAxis : area.InternalPrimaryAxis;
            double actualRotationAngle = (chartAxisBase3D.Area as SfChart3D).ActualRotationAngle;
            double num4 = chartAxisBase3D.IsZAxis ? (chartAxis.OpposedPosition || actualRotationAngle < 0.0 || actualRotationAngle >= 180.0 ? left - 1.0 : left + width) : left;
            chartAxisBase3D.AxisDepth = chartAxisBase3D.IsZAxis || chartAxis.OpposedPosition || actualRotationAngle < 90.0 || actualRotationAngle >= 270.0 ? 0.0 : area.ActualDepth;
            if (axi.ShowAxisNextToOrigin && axi.Area != null && axi.Area.RowDefinitions.Count <= 1)
            {
              bool isContinue = false;
              this.ArragneRectBasedOnOrigin(axi, elementColumnIndex, actualColumnIndex, num4, num3, areaHeight, ref isContinue);
              if (isContinue)
                continue;
            }
            if (flag3)
            {
              double num5 = flag1 ? axi.InsidePadding : 0.0;
              if (elementColumnIndex == actualColumnIndex)
                axi.ArrangeRect = new Rect(num4, num2 - computedDesiredSize.Height + num5, num3, computedDesiredSize.Height);
              axi.Measure(new Size(axi.ArrangeRect.Width, axi.ArrangeRect.Height));
              if (!(this.Axis[index3] as ChartAxisBase3D).IsZAxis)
              {
                if (index3 + 1 < this.Axis.Count)
                {
                  if ((this.Axis[index3 + 1] as ChartAxisBase3D).IsZAxis)
                    continue;
                }
                num2 -= farSizes[index2];
                ++index2;
                flag1 = false;
              }
            }
            else
            {
              double num6 = flag2 ? axi.InsidePadding : 0.0;
              if (elementColumnIndex == actualColumnIndex)
                axi.ArrangeRect = new Rect(num4, areaHeight - num1 - num6, num3, computedDesiredSize.Height);
              axi.Measure(new Size(axi.ArrangeRect.Width, axi.ArrangeRect.Height));
              if (!(this.Axis[index3] as ChartAxisBase3D).IsZAxis)
              {
                if (index3 + 1 < this.Axis.Count)
                {
                  if ((this.Axis[index3 + 1] as ChartAxisBase3D).IsZAxis)
                    continue;
                }
                num1 -= nearSizes[index1];
                ++index1;
                flag2 = false;
              }
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
            num3 = this.CalcColumnSpanAxisWidth(width, axi, axi.Area.GetActualColumnSpan((UIElement) axi));
            actualColumnIndex = axi.Area.ColumnDefinitions.IndexOf(this);
            elementColumnIndex = axi.Area.GetActualColumn((UIElement) axi);
          }
          Size computedDesiredSize = axi.ComputedDesiredSize;
          try
          {
            if (axi.ShowAxisNextToOrigin && axi.Area != null && axi.Area.RowDefinitions.Count <= 1)
            {
              bool isContinue = false;
              this.ArragneRectBasedOnOrigin(axi, elementColumnIndex, actualColumnIndex, left, num3, areaHeight, ref isContinue);
              if (isContinue)
                continue;
            }
            if (axi.OpposedPosition)
            {
              double num7 = flag1 ? axi.InsidePadding : 0.0;
              if (elementColumnIndex == actualColumnIndex)
                axi.ArrangeRect = new Rect(left, num2 - computedDesiredSize.Height + num7, num3, computedDesiredSize.Height);
              axi.Measure(new Size(axi.ArrangeRect.Width, axi.ArrangeRect.Height));
              num2 -= farSizes[index2];
              ++index2;
              flag1 = false;
            }
            else
            {
              double num8 = flag2 ? axi.InsidePadding : 0.0;
              if (elementColumnIndex == actualColumnIndex)
                axi.ArrangeRect = new Rect(left, areaHeight - num1 - num8, num3, computedDesiredSize.Height);
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

  private static void OnColumnPropertyChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    if (!(d is ChartColumnDefinition columnDefinition) || columnDefinition.Axis == null || columnDefinition.Axis.Count <= 0)
      return;
    columnDefinition.Axis[0].Area.ScheduleUpdate();
  }

  private void MeasureRectBasedOnOrigin(
    ChartAxis content,
    List<double> nearSizes,
    List<double> farSizes,
    ref bool isContinue)
  {
    double num1 = 0.0;
    ChartAxis chartAxis = (ChartAxis) null;
    if (content.Area.InternalPrimaryAxis == content || content.Area.InternalDepthAxis == content)
    {
      num1 = content.Area.InternalSecondaryAxis.ValueToCoefficientCalc(content.Origin);
      chartAxis = content.Area.InternalSecondaryAxis;
    }
    else if (content.Area.InternalPrimaryAxis.Orientation == Orientation.Vertical && content.Area.InternalSecondaryAxis == content)
    {
      num1 = content.Area.InternalPrimaryAxis.ValueToCoefficientCalc(content.Origin);
      chartAxis = content.Area.InternalPrimaryAxis;
    }
    Size computedDesiredSize = content.ComputedDesiredSize;
    if (0.0 >= num1 || 1.0 <= num1)
      return;
    if (content.HeaderPosition == AxisHeaderPosition.Far)
    {
      double actualPlotOffset = chartAxis.GetActualPlotOffset();
      double num2 = content.OpposedPosition ? (1.0 - num1) * (content.Area.SeriesClipRect.Height - actualPlotOffset) + chartAxis.GetActualPlotOffsetStart() + content.InsidePadding : num1 * (content.Area.SeriesClipRect.Height - actualPlotOffset) + chartAxis.GetActualPlotOffsetEnd() + content.InsidePadding;
      if (content.OpposedPosition && num2 > computedDesiredSize.Height - content.headerContent.DesiredSize.Height)
        farSizes.Add(content.headerContent.DesiredSize.Height + this.headerMargin);
      else if (num2 > computedDesiredSize.Height - content.headerContent.DesiredSize.Height)
        nearSizes.Add(content.headerContent.DesiredSize.Height + this.headerMargin);
      isContinue = true;
    }
    else
      isContinue = true;
  }

  private double CalcColumnSpanAxisWidth(double width, ChartAxis axis, int columnSpan)
  {
    if (axis.Area != null)
    {
      int actualColumn = axis.Area.GetActualColumn((UIElement) axis);
      if (axis.Area != null && columnSpan > 1 && actualColumn == axis.Area.ColumnDefinitions.IndexOf(this))
      {
        ChartColumnDefinitions columnDefinitions = axis.Area.ColumnDefinitions;
        int index = columnDefinitions.IndexOf(this);
        int num = 0;
        width = 0.0;
        for (; index < columnDefinitions.Count; ++index)
        {
          if (num < columnSpan)
          {
            width += columnDefinitions[index].ComputedWidth;
            ++num;
          }
        }
      }
    }
    return width;
  }

  private void ArragneRectBasedOnOrigin(
    ChartAxis element,
    int elementColumnIndex,
    int actualColumnIndex,
    double actualLeft,
    double axisWidth,
    double areaHeight,
    ref bool isContinue)
  {
    ChartAxis chartAxis = (ChartAxis) null;
    Size computedDesiredSize = element.ComputedDesiredSize;
    if (element.Area.InternalPrimaryAxis == element || element.Area.InternalDepthAxis == element)
      chartAxis = element.Area.InternalSecondaryAxis;
    else if (element.Area.InternalPrimaryAxis.Orientation == Orientation.Vertical && element.Area.InternalSecondaryAxis == element)
      chartAxis = element.Area.InternalPrimaryAxis;
    if (chartAxis == null)
      return;
    double num1 = chartAxis.ValueToCoefficientCalc(element.Origin);
    double actualPlotOffset = chartAxis.GetActualPlotOffset();
    if (0.0 >= num1 || 1.0 <= num1)
      return;
    Rect rect = ChartLayoutUtils.Subtractthickness(new Rect(new Point(0.0, 0.0), element.AvailableSize), element.Area.AxisThickness);
    if (element.OpposedPosition && elementColumnIndex == actualColumnIndex)
    {
      double height = element.headerContent.DesiredSize.Height;
      double num2 = element.HeaderPosition == AxisHeaderPosition.Far ? (1.0 - num1) * (rect.Height - actualPlotOffset) + chartAxis.GetActualPlotOffsetEnd() + element.InsidePadding : 0.0;
      element.ArrangeRect = num2 <= 0.0 || num2 <= computedDesiredSize.Height - element.headerContent.DesiredSize.Height ? new Rect(actualLeft, (areaHeight - actualPlotOffset) * (1.0 - num1) - computedDesiredSize.Height + element.InsidePadding + chartAxis.GetActualPlotOffsetEnd(), axisWidth, computedDesiredSize.Height) : new Rect(actualLeft, num2 - computedDesiredSize.Height + height + this.headerMargin, axisWidth, computedDesiredSize.Height);
    }
    else if (elementColumnIndex == actualColumnIndex)
    {
      double num3 = element.HeaderPosition == AxisHeaderPosition.Far ? num1 * (rect.Height - actualPlotOffset) + chartAxis.GetActualPlotOffsetEnd() + element.InsidePadding : 0.0;
      element.ArrangeRect = num3 <= 0.0 || num3 <= computedDesiredSize.Height - element.headerContent.DesiredSize.Height ? new Rect(actualLeft, (areaHeight - actualPlotOffset) * (1.0 - num1) - element.InsidePadding + chartAxis.GetActualPlotOffsetEnd(), axisWidth, computedDesiredSize.Height) : new Rect(actualLeft, (rect.Height - actualPlotOffset) * (1.0 - num1) - element.InsidePadding + chartAxis.GetActualPlotOffsetEnd(), axisWidth, computedDesiredSize.Height);
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
    Line borderLine1 = this.BorderLine;
    Line borderLine2 = this.BorderLine;
    double left1 = chartAxis.ArrangeRect.Left;
    double left2 = chartAxis.Area.SeriesClipRect.Left;
    double num1;
    double num2 = num1 = left1 - left2;
    borderLine2.X2 = num1;
    double num3 = num2;
    borderLine1.X1 = num3;
    this.BorderLine.Y1 = 0.0;
    this.BorderLine.Y2 = chartAxis.Area.SeriesClipRect.Height;
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
