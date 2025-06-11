// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.ChartCartesianAxisElementsPanel
// Assembly: Syncfusion.SfChart.WPF, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 279D70C6-7936-4F3E-B6CF-79EFEF734826
// Assembly location: C:\Program Files\PDFgear\Syncfusion.SfChart.WPF.dll

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Shapes;

#nullable disable
namespace Syncfusion.UI.Xaml.Charts;

public class ChartCartesianAxisElementsPanel : ILayoutCalculator
{
  private ChartAxis axis;
  private UIElementsRecycler<Line> majorTicksRecycler;
  private UIElementsRecycler<Line> minorTicksRecycler;
  private Size desiredSize;
  private Panel labelsPanels;

  public ChartCartesianAxisElementsPanel(Panel panel)
  {
    this.labelsPanels = panel;
    this.MainAxisLine = new Line();
    panel?.Children.Add((UIElement) this.MainAxisLine);
    this.majorTicksRecycler = new UIElementsRecycler<Line>(panel);
    this.minorTicksRecycler = new UIElementsRecycler<Line>(panel);
  }

  public double Left { get; set; }

  public double Top { get; set; }

  public Panel Panel => this.labelsPanels;

  public Size DesiredSize => this.desiredSize;

  public List<UIElement> Children
  {
    get
    {
      return this.labelsPanels != null ? this.labelsPanels.Children.Cast<UIElement>().ToList<UIElement>() : (List<UIElement>) null;
    }
  }

  internal ChartAxis Axis
  {
    get => this.axis;
    set
    {
      this.axis = value;
      this.SetAxisLineBinding();
    }
  }

  internal Line MainAxisLine { get; set; }

  public Size Measure(Size availableSize)
  {
    Size size = Size.Empty;
    double val2 = this.Axis.Area is SfChart3D ? (this.Axis is CategoryAxis3D ? 5.0 : (this.Axis as RangeAxisBase3D).SmallTickLineSize) : (this.Axis is CategoryAxis || this.Axis is DateTimeCategoryAxis ? 5.0 : (this.Axis as RangeAxisBase).SmallTickLineSize);
    size = !this.Axis.Orientation.Equals((object) Orientation.Horizontal) ? new Size(Math.Max(Math.Max(this.Axis.TickLineSize, val2), 0.0) + this.MainAxisLine.StrokeThickness, availableSize.Height) : new Size(availableSize.Width, Math.Max(Math.Max(this.Axis.TickLineSize, val2), 0.0) + this.MainAxisLine.StrokeThickness);
    this.desiredSize = size;
    return size;
  }

  public void DetachElements()
  {
    if (this.MainAxisLine != null && this.labelsPanels != null && this.labelsPanels.Children != null && this.labelsPanels.Children.Contains((UIElement) this.MainAxisLine))
      this.labelsPanels.Children.Remove((UIElement) this.MainAxisLine);
    if (this.majorTicksRecycler != null)
      this.majorTicksRecycler.Clear();
    if (this.minorTicksRecycler != null)
      this.minorTicksRecycler.Clear();
    this.labelsPanels = (Panel) null;
  }

  public Size Arrange(Size finalSize)
  {
    double[] array1 = this.Axis.VisibleLabels.Select<ChartAxisLabel, double>((Func<ChartAxisLabel, double>) (val => val.Position)).ToArray<double>();
    if (this.Axis.Area is SfChart)
    {
      this.RenderAxisLine(finalSize);
      if (this.Axis is CategoryAxis && (this.Axis as CategoryAxis).LabelPlacement == LabelPlacement.BetweenTicks)
        array1 = this.Axis.SmallTickPoints.Select<double, double>((Func<double, double>) (val => val)).ToArray<double>();
      this.RenderTicks(finalSize, this.majorTicksRecycler, this.Axis.Orientation, this.Axis.TickLineSize, this.Axis.TickLinesPosition, array1);
      if (this.Axis.smallTicksRequired)
      {
        double[] array2 = this.Axis.SmallTickPoints.Select<double, double>((Func<double, double>) (val => val)).ToArray<double>();
        this.RenderTicks(finalSize, this.minorTicksRecycler, this.Axis.Orientation, (this.Axis as RangeAxisBase).SmallTickLineSize, (this.Axis as RangeAxisBase).SmallTickLinesPosition, array2);
      }
    }
    else
    {
      if (this.Axis is CategoryAxis3D && (this.Axis as CategoryAxis3D).LabelPlacement == LabelPlacement.BetweenTicks)
        array1 = this.Axis.SmallTickPoints.Select<double, double>((Func<double, double>) (val => val)).ToArray<double>();
      this.RenderTicks3D(this.majorTicksRecycler, this.Axis.Orientation, this.Axis.TickLineSize, this.Axis.TickLinesPosition, (IList<double>) array1);
      if (this.Axis.smallTicksRequired)
        this.RenderTicks3D(this.minorTicksRecycler, this.Axis.Orientation, (this.Axis as RangeAxisBase3D).SmallTickLineSize, (this.Axis as RangeAxisBase3D).SmallTickLinesPosition, (IList<double>) this.Axis.SmallTickPoints.Select<double, double>((Func<double, double>) (val => val)).ToArray<double>());
    }
    return finalSize;
  }

  public void UpdateElements() => this.UpdateTicks();

  internal void Dispose()
  {
    this.axis = (ChartAxis) null;
    if (this.majorTicksRecycler != null && this.majorTicksRecycler.Count > 0)
    {
      this.majorTicksRecycler.Clear();
      this.majorTicksRecycler = (UIElementsRecycler<Line>) null;
    }
    if (this.minorTicksRecycler == null || this.minorTicksRecycler.Count <= 0)
      return;
    this.minorTicksRecycler.Clear();
    this.minorTicksRecycler = (UIElementsRecycler<Line>) null;
  }

  internal void UpdateTicks()
  {
    int count = this.Axis.SmallTickPoints.Count;
    if ((!(this.Axis is CategoryAxis) || (this.Axis as CategoryAxis).LabelPlacement != LabelPlacement.BetweenTicks) && (!(this.Axis is CategoryAxis3D) || (this.Axis as CategoryAxis3D).LabelPlacement != LabelPlacement.BetweenTicks))
      count = this.Axis.VisibleLabels.Count;
    this.UpdateTicks(count, this.majorTicksRecycler, "MajorTickLineStyle");
    if (!this.Axis.smallTicksRequired)
      return;
    this.UpdateTicks(this.Axis.SmallTickPoints.Count, this.minorTicksRecycler, "MinorTickLineStyle");
  }

  private void SetAxisLineBinding()
  {
    this.MainAxisLine.SetBinding(FrameworkElement.StyleProperty, (BindingBase) new Binding()
    {
      Source = (object) this.axis,
      Path = new PropertyPath("AxisLineStyle", new object[0])
    });
  }

  private void UpdateTicks(
    int linescount,
    UIElementsRecycler<Line> lineRecycler,
    string lineStylePath)
  {
    int count = linescount;
    foreach (DependencyObject dependencyObject in lineRecycler)
      dependencyObject.ClearValue(FrameworkElement.StyleProperty);
    if (!lineRecycler.BindingProvider.Keys.Contains<DependencyProperty>(FrameworkElement.StyleProperty))
    {
      lineRecycler.BindingProvider.Add(FrameworkElement.StyleProperty, ChartCartesianAxisElementsPanel.GetTickLineBinding((object) this.Axis, lineStylePath));
      lineRecycler.BindingProvider.Add(UIElement.VisibilityProperty, ChartCartesianAxisElementsPanel.GetTickLineBinding((object) this.Axis, "Visibility"));
    }
    if (lineRecycler.Count > 0)
    {
      foreach (Line line in lineRecycler)
      {
        line.SetBinding(FrameworkElement.StyleProperty, (BindingBase) ChartCartesianAxisElementsPanel.GetTickLineBinding((object) this.Axis, lineStylePath));
        line.SetBinding(UIElement.VisibilityProperty, (BindingBase) ChartCartesianAxisElementsPanel.GetTickLineBinding((object) this.Axis, "Visibility"));
      }
    }
    lineRecycler.GenerateElements(count);
    ChartAxisRangeStyleCollection rangeStyles = this.axis.RangeStyles;
    if (rangeStyles == null || rangeStyles.Count <= 0)
      return;
    List<double> doubleList = !lineStylePath.Equals("MajorTickLineStyle") ? this.axis.SmallTickPoints : this.axis.VisibleLabels.Select<ChartAxisLabel, double>((Func<ChartAxisLabel, double>) (label => label.Position)).ToList<double>();
    for (int index = 0; index < doubleList.Count; ++index)
    {
      foreach (ChartAxisRangeStyle source in (Collection<ChartAxisRangeStyle>) rangeStyles)
      {
        DoubleRange range = source.Range;
        Style style = lineStylePath.Equals("MajorTickLineStyle") ? source.MajorTickLineStyle : source.MinorTickLineStyle;
        if (range.Start <= doubleList[index] && range.End >= doubleList[index] && style != null)
        {
          lineRecycler[index].SetBinding(FrameworkElement.StyleProperty, (BindingBase) ChartCartesianAxisElementsPanel.GetTickLineBinding((object) source, lineStylePath));
          lineRecycler[index].SetBinding(UIElement.VisibilityProperty, (BindingBase) ChartCartesianAxisElementsPanel.GetTickLineBinding((object) this.axis, "Visibility"));
          break;
        }
      }
    }
  }

  private void RenderAxisLine(Size finalSize)
  {
    double num1 = 0.0;
    double width = finalSize.Width;
    double height = finalSize.Height;
    double num2 = this.MainAxisLine.StrokeThickness / 2.0;
    bool flag = this.Axis.TickLinesPosition == AxisElementPosition.Inside;
    Orientation orientation = this.Axis.Orientation;
    bool opposedPosition = this.Axis.OpposedPosition;
    double num3;
    double num4;
    double num5;
    double num6;
    if (orientation == Orientation.Horizontal)
    {
      num3 = this.Axis.AxisLineOffset;
      num4 = width - this.Axis.AxisLineOffset;
      num6 = num5 = opposedPosition ? (flag ? -num2 : height - num2) : (flag ? (num1 = num2 + height) : num2);
    }
    else
    {
      num4 = num3 = opposedPosition ? (flag ? num2 + width : num2) : (flag ? -num2 : width - num2);
      num5 = this.Axis.AxisLineOffset;
      num6 = height - this.Axis.AxisLineOffset;
    }
    if (this.MainAxisLine == null)
      return;
    this.MainAxisLine.X1 = num3;
    this.MainAxisLine.Y1 = num5;
    this.MainAxisLine.X2 = num4;
    this.MainAxisLine.Y2 = num6;
  }

  private void RenderTicks(
    Size finalSize,
    UIElementsRecycler<Line> linesRecycler,
    Orientation orientation,
    double tickSize,
    AxisElementPosition tickPosition,
    double[] Values)
  {
    int length = Values.Length;
    int count = linesRecycler.Count;
    double width = finalSize.Width;
    double height = finalSize.Height;
    for (int index = 0; index < length; ++index)
    {
      if (index < count)
      {
        double x1 = 0.0;
        double y1 = 0.0;
        double x2 = 0.0;
        double y2 = 0.0;
        Line line = linesRecycler[index];
        double d = this.Axis.ValueToCoefficientCalc(Values[index]);
        double num1 = double.IsNaN(d) ? 0.0 : d;
        if (orientation.Equals((object) Orientation.Horizontal))
        {
          double actualPlotOffsetStart = this.Axis.GetActualPlotOffsetStart();
          double num2 = Math.Round(this.Axis.RenderedRect.Width * num1);
          x1 = x2 = actualPlotOffsetStart + num2;
        }
        else
        {
          double actualPlotOffsetEnd = this.Axis.GetActualPlotOffsetEnd();
          double num3 = Math.Round(this.Axis.RenderedRect.Height * (1.0 - num1));
          y1 = y2 = actualPlotOffsetEnd + num3 + 0.5;
        }
        this.CalculatePosition(tickPosition, tickSize, ref x1, ref y1, ref x2, ref y2);
        line.X1 = x1;
        line.X2 = x2;
        line.Y1 = y1;
        line.Y2 = y2;
      }
    }
  }

  private void CalculatePosition(
    AxisElementPosition ticksPosition,
    double tickSize,
    ref double x1,
    ref double y1,
    ref double x2,
    ref double y2)
  {
    Orientation orientation = this.Axis.Orientation;
    bool opposedPosition = this.Axis.OpposedPosition;
    double num = this.MainAxisLine.StrokeThickness / 2.0;
    bool flag = ticksPosition == AxisElementPosition.Inside;
    if (orientation == Orientation.Horizontal)
    {
      y1 = opposedPosition ? (!flag ? this.MainAxisLine.Y1 - num : this.MainAxisLine.Y1 + num) : (flag ? this.MainAxisLine.Y1 - num : this.MainAxisLine.Y1 + num);
      y2 = opposedPosition ? (!flag ? y1 - tickSize : y1 + tickSize) : (flag ? y1 - tickSize : y1 + tickSize);
    }
    else
    {
      x1 = opposedPosition ? (!flag ? this.MainAxisLine.X1 + num : this.MainAxisLine.X1 - num) : (flag ? this.MainAxisLine.X1 + num : this.MainAxisLine.X1 - num);
      x2 = opposedPosition ? (!flag ? x1 + tickSize : x1 - tickSize) : (flag ? x1 + tickSize : x1 - tickSize);
    }
  }

  private void RenderTicks3D(
    UIElementsRecycler<Line> linesRecycler,
    Orientation orientation,
    double tickSize,
    AxisElementPosition tickPosition,
    IList<double> values)
  {
    SfChart3D area = this.Axis.Area as SfChart3D;
    double actualRotationAngle = area.ActualRotationAngle;
    int count1 = values.Count;
    int count2 = linesRecycler.Count;
    for (int index = 0; index < count1; ++index)
    {
      if (index < count2)
      {
        double x1 = 0.0;
        double y1 = 0.0;
        double x2 = 0.0;
        double y2 = 0.0;
        Line line1 = linesRecycler[index];
        double d = this.Axis.ValueToCoefficientCalc(values[index]);
        double num1 = double.IsNaN(d) ? 0.0 : d;
        if (orientation.Equals((object) Orientation.Horizontal))
        {
          if ((this.axis as ChartAxisBase3D).IsManhattanAxis && (this.axis.RegisteredSeries[index] as ChartSeries3D).Segments != null && (this.axis.RegisteredSeries[index] as ChartSeries3D).Segments.Count > 0)
          {
            ChartSegment3D segment = (this.axis.RegisteredSeries[index] as ChartSeries3D).Segments[0] as ChartSegment3D;
            x1 = x2 = segment.startDepth + (segment.endDepth - segment.startDepth) / 2.0;
          }
          else
          {
            double actualPlotOffsetStart = this.Axis.GetActualPlotOffsetStart();
            double num2 = this.Axis.RenderedRect.Width * num1;
            x1 = x2 = Math.Round(actualPlotOffsetStart + num2);
          }
        }
        else
        {
          double actualPlotOffsetEnd = this.Axis.GetActualPlotOffsetEnd();
          double num3 = this.Axis.RenderedRect.Height * (1.0 - num1);
          y1 = y2 = Math.Round(actualPlotOffsetEnd + num3);
        }
        this.CalculatePosition3D(tickPosition, tickSize, ref x1, ref y1, ref x2, ref y2, actualRotationAngle);
        ChartAxis chartAxis = area.InternalSecondaryAxis.Orientation.Equals((object) Orientation.Vertical) ? area.InternalSecondaryAxis : area.InternalPrimaryAxis;
        if ((this.axis as ChartAxisBase3D).IsZAxis)
        {
          double num4 = chartAxis.OpposedPosition || actualRotationAngle < 0.0 || actualRotationAngle >= 180.0 ? this.axis.ArrangeRect.Left : this.axis.ArrangeRect.Left + 1.0;
          ((SfChart3D) this.Axis.Area).Graphics3D.AddVisual((Polygon3D) Polygon3D.CreateLine(line1, num4, y1, num4, y2, x1, x1, false));
        }
        else
        {
          double num5 = 0.0;
          ChartAxisBase3D axis = this.axis as ChartAxisBase3D;
          Line3D line2;
          if (orientation.Equals((object) Orientation.Vertical))
          {
            if (this.axis.ShowAxisNextToOrigin)
            {
              if (actualRotationAngle >= 90.0 && actualRotationAngle < 270.0)
                num5 = (this.axis.Area as SfChart3D).ActualDepth;
              line2 = Polygon3D.CreateLine(line1, x1, y1, x2, y2, num5, num5, true);
            }
            else if (axis.AxisPosition3D == AxisPosition3D.DepthBackRight || axis.AxisPosition3D == AxisPosition3D.DepthBackLeft || axis.AxisPosition3D == AxisPosition3D.DepthFrontRight || axis.AxisPosition3D == AxisPosition3D.DepthFrontLeft)
            {
              line2 = Polygon3D.CreateLine(line1, this.axis.ArrangeRect.Left, y1, this.axis.ArrangeRect.Left, y2, x1, x2, false);
            }
            else
            {
              double axisDepth = axis.AxisDepth;
              line2 = Polygon3D.CreateLine(line1, x1, y1, x2, y2, axisDepth, axisDepth, true);
            }
          }
          else
          {
            double axisDepth = axis.AxisDepth;
            line2 = Polygon3D.CreateLine(line1, x1, y1, x2, y2, axisDepth, axisDepth, true);
          }
          ((SfChart3D) this.Axis.Area).Graphics3D.AddVisual((Polygon3D) line2);
        }
      }
    }
  }

  private void CalculatePosition3D(
    AxisElementPosition ticksPosition,
    double tickSize,
    ref double x1,
    ref double y1,
    ref double x2,
    ref double y2,
    double actualRotationAngle)
  {
    Orientation orientation = this.Axis.Orientation;
    bool opposedPosition = this.Axis.OpposedPosition;
    if (orientation.Equals((object) Orientation.Horizontal))
    {
      bool flag = opposedPosition || this.Axis.Area.Axes.Where<ChartAxis>((Func<ChartAxis, bool>) (x => x is ChartAxisBase3D && x.Orientation.Equals((object) Orientation.Horizontal) && x.OpposedPosition)).Any<ChartAxis>();
      switch (ticksPosition)
      {
        case AxisElementPosition.Inside:
          y1 = flag ? this.MainAxisLine.StrokeThickness : 0.0;
          y2 = flag ? y1 + tickSize : tickSize;
          break;
        case AxisElementPosition.Outside:
          y1 = flag ? 0.0 : this.MainAxisLine.StrokeThickness;
          y2 = flag ? tickSize : y1 + tickSize;
          break;
      }
      double num = this.axis.ArrangeRect.Top + this.Top;
      double left = this.axis.ArrangeRect.Left;
      y1 += num;
      y2 += num;
      if ((this.Axis as ChartAxisBase3D).IsZAxis)
        return;
      x1 += left;
      x2 += left;
    }
    else
    {
      ChartAxisBase3D axis = this.Axis as ChartAxisBase3D;
      bool flag = opposedPosition || this.Axis.ShowAxisNextToOrigin && actualRotationAngle >= 180.0 && actualRotationAngle < 360.0 || !this.Axis.ShowAxisNextToOrigin && (axis.AxisPosition3D == AxisPosition3D.DepthBackRight || axis.AxisPosition3D == AxisPosition3D.DepthFrontRight || axis.AxisPosition3D == AxisPosition3D.BackRight || axis.AxisPosition3D == AxisPosition3D.DepthFrontLeft || axis.AxisPosition3D == AxisPosition3D.FrontRight);
      switch (ticksPosition)
      {
        case AxisElementPosition.Inside:
          x1 = flag ? 0.0 : this.MainAxisLine.StrokeThickness;
          x2 = flag ? tickSize : x1 + tickSize;
          break;
        case AxisElementPosition.Outside:
          x1 = flag ? this.MainAxisLine.StrokeThickness : 0.0;
          x2 = flag ? x1 + tickSize : tickSize;
          break;
      }
      double num;
      switch (axis.AxisPosition3D)
      {
        case AxisPosition3D.DepthFrontLeft:
        case AxisPosition3D.DepthBackRight:
          num = axis.AxisDepth + this.Left;
          break;
        case AxisPosition3D.DepthFrontRight:
        case AxisPosition3D.DepthBackLeft:
          num = axis.AxisDepth - this.Left;
          x1 = -x1;
          x2 = -x2;
          break;
        default:
          num = this.axis.ArrangeRect.Left + this.Left;
          break;
      }
      x1 += num;
      x2 += num;
      double top = this.axis.ArrangeRect.Top;
      y1 += top;
      y2 += top;
    }
  }

  private static Binding GetTickLineBinding(object source, string propertyPath)
  {
    return new Binding()
    {
      Path = new PropertyPath(propertyPath, new object[0]),
      Source = source
    };
  }
}
