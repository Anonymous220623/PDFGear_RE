// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.ChartCartesianGridLinesPanel
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
using System.Windows.Media;
using System.Windows.Shapes;

#nullable disable
namespace Syncfusion.UI.Xaml.Charts;

public class ChartCartesianGridLinesPanel : ILayoutCalculator
{
  private Panel panel;
  private Size desiredSize;
  private readonly UIElementsRecycler<Border> stripLines;
  private Line xOriginLine;
  private Line yOriginLine;

  public ChartCartesianGridLinesPanel(Panel panel)
  {
    this.panel = panel;
    this.stripLines = new UIElementsRecycler<Border>(this.panel);
    this.xOriginLine = new Line();
    this.yOriginLine = new Line();
  }

  public Size DesiredSize => this.desiredSize;

  public Panel Panel => this.panel;

  public List<UIElement> Children
  {
    get
    {
      return this.panel != null ? this.panel.Children.Cast<UIElement>().ToList<UIElement>() : (List<UIElement>) null;
    }
  }

  public double Left { get; set; }

  public double Top { get; set; }

  internal ChartBase Area { get; set; }

  public void DrawGridLines(ChartAxis axis)
  {
    if (axis == null)
      return;
    double left1 = axis.RenderedRect.Left - this.Area.AxisThickness.Left;
    double num1 = axis.RenderedRect.Right - this.Area.AxisThickness.Left;
    double top1 = axis.RenderedRect.Top - this.Area.AxisThickness.Top;
    double num2 = axis.RenderedRect.Bottom - this.Area.AxisThickness.Top;
    double[] array1 = axis.VisibleLabels.Select<ChartAxisLabel, double>((Func<ChartAxisLabel, double>) (label => label.Position)).ToArray<double>();
    if (axis is CategoryAxis categoryAxis && categoryAxis.LabelPlacement == LabelPlacement.BetweenTicks)
      array1 = axis.SmallTickPoints.Select<double, double>((Func<double, double>) (pointValues => pointValues)).ToArray<double>();
    if (axis.Orientation == Orientation.Horizontal)
    {
      double width = num1 - left1;
      IEnumerable<ChartAxis> chartAxises = (IEnumerable<ChartAxis>) null;
      if (axis.RegisteredSeries.Count > 0)
        chartAxises = this.Area.RowDefinitions.Count > 1 ? (IEnumerable<ChartAxis>) axis.AssociatedAxes : axis.AssociatedAxes.DistinctBy<ChartAxis, int>(new Func<ChartAxis, int>(this.Area.GetActualRow));
      else if (this.Area.InternalPrimaryAxis != null)
        chartAxises = (IEnumerable<ChartAxis>) new List<ChartAxis>()
        {
          this.Area.InternalSecondaryAxis
        };
      int index1 = 0;
      int index2 = 0;
      if (chartAxises == null)
        return;
      foreach (ChartAxis chartAxis in chartAxises)
      {
        ChartAxis supportAxis = chartAxis;
        if (supportAxis != null)
        {
          int num3 = axis.RegisteredSeries.Where<ISupportAxes>((Func<ISupportAxes, bool>) (item => item.ActualXAxis == supportAxis || item.ActualYAxis == supportAxis)).Count<ISupportAxes>();
          if (num3 == 0)
            num3 = 1;
          if (num3 > 0)
          {
            double top2 = supportAxis.ArrangeRect.Top - this.Area.AxisThickness.Top;
            double height = top2 + supportAxis.ArrangeRect.Height;
            if (array1.Length > 0)
              this.DrawGridLines(axis, axis.GridLinesRecycler, left1, top2, width, height, array1, true, index1);
            if (axis.smallTicksRequired)
            {
              double[] array2 = axis.SmallTickPoints.Select<double, double>((Func<double, double>) (pointValues => pointValues)).ToArray<double>();
              if (array2.Length > 0)
                this.DrawGridLines(axis, axis.MinorGridLinesRecycler, left1, top2, width, height, array2, false, index2);
              index2 += array2.Length;
            }
            index1 += array1.Length;
          }
        }
      }
    }
    else
    {
      double height = num2 - top1;
      IEnumerable<ChartAxis> chartAxises = (IEnumerable<ChartAxis>) null;
      if (axis.RegisteredSeries.Count > 0)
        chartAxises = this.Area.ColumnDefinitions.Count > 1 ? (IEnumerable<ChartAxis>) axis.AssociatedAxes : axis.AssociatedAxes.DistinctBy<ChartAxis, int>(new Func<ChartAxis, int>(this.Area.GetActualColumn));
      else if (this.Area.InternalPrimaryAxis != null)
        chartAxises = (IEnumerable<ChartAxis>) new List<ChartAxis>()
        {
          this.Area.InternalPrimaryAxis
        };
      int index3 = 0;
      int index4 = 0;
      if (chartAxises == null)
        return;
      foreach (ChartAxis chartAxis in chartAxises)
      {
        ChartAxis supportAxis = chartAxis;
        int num4 = axis.RegisteredSeries.Where<ISupportAxes>((Func<ISupportAxes, bool>) (item => item.ActualXAxis == supportAxis || item.ActualYAxis == supportAxis)).Count<ISupportAxes>();
        if (num4 == 0)
          num4 = 1;
        if (num4 > 0)
        {
          double left2 = supportAxis.ArrangeRect.Left - this.Area.AxisThickness.Left;
          double width = left2 + supportAxis.ArrangeRect.Width;
          if (array1.Length > 0)
            this.DrawGridLines(axis, axis.GridLinesRecycler, left2, top1, width, height, array1, true, index3);
          if (axis.smallTicksRequired)
          {
            double[] array3 = axis.SmallTickPoints.Select<double, double>((Func<double, double>) (pointValues => pointValues)).ToArray<double>();
            if (array3.Length > 0)
              this.DrawGridLines(axis, axis.MinorGridLinesRecycler, left2, top1, width, height, array3, false, index4);
            index4 += array3.Length;
          }
          index3 += array1.Length;
        }
      }
    }
  }

  public void DrawGridLines3D(ChartAxis axis)
  {
    if (axis == null)
      return;
    double left1 = axis.RenderedRect.Left;
    double right = axis.RenderedRect.Right;
    double top1 = axis.RenderedRect.Top;
    double bottom = axis.RenderedRect.Bottom;
    double[] array1 = axis.VisibleLabels.Select<ChartAxisLabel, double>((Func<ChartAxisLabel, double>) (label => label.Position)).ToArray<double>();
    if (axis is CategoryAxis categoryAxis && categoryAxis.LabelPlacement == LabelPlacement.BetweenTicks)
      array1 = axis.SmallTickPoints.Select<double, double>((Func<double, double>) (pointValues => pointValues)).ToArray<double>();
    if (axis.Orientation == Orientation.Horizontal)
    {
      double width = right - left1;
      IEnumerable<ChartAxis> source = (IEnumerable<ChartAxis>) null;
      if (axis.RegisteredSeries.Count > 0)
        source = axis.AssociatedAxes.DistinctBy<ChartAxis, int>(new Func<ChartAxis, int>(this.Area.GetActualRow));
      else if (this.Area.InternalPrimaryAxis != null)
        source = (IEnumerable<ChartAxis>) new List<ChartAxis>()
        {
          this.Area.InternalSecondaryAxis
        };
      int index1 = 0;
      int index2 = 0;
      for (int index3 = 0; index3 < source.Count<ChartAxis>(); ++index3)
      {
        ChartAxis chartAxis = this.Area.InternalSecondaryAxis.Orientation == Orientation.Vertical ? this.Area.InternalSecondaryAxis : this.Area.InternalPrimaryAxis;
        double top2 = chartAxis.ArrangeRect.Top;
        double height = top2 + chartAxis.ArrangeRect.Height;
        if (array1.Length > 0)
          this.DrawGridLines3D(axis, axis.GridLinesRecycler, left1, top2, width, height, array1, index1);
        if (axis.smallTicksRequired)
        {
          double[] array2 = axis.SmallTickPoints.Select<double, double>((Func<double, double>) (pointValues => pointValues)).ToArray<double>();
          if (array2.Length > 0)
            this.DrawGridLines3D(axis, axis.MinorGridLinesRecycler, left1, top2, width, height, array2, index2);
          index2 += array2.Length;
        }
        index1 += array1.Length;
      }
    }
    else
    {
      double height = bottom - top1;
      IEnumerable<ChartAxis> chartAxises = (IEnumerable<ChartAxis>) null;
      if (axis.RegisteredSeries.Count > 0)
        chartAxises = axis.AssociatedAxes.DistinctBy<ChartAxis, int>(new Func<ChartAxis, int>(this.Area.GetActualColumn));
      else if (this.Area.InternalPrimaryAxis != null)
        chartAxises = (IEnumerable<ChartAxis>) new List<ChartAxis>()
        {
          this.Area.InternalPrimaryAxis
        };
      int index4 = 0;
      int index5 = 0;
      foreach (ChartAxis chartAxis in chartAxises)
      {
        double left2 = chartAxis.ArrangeRect.Left;
        double width = left2 + chartAxis.ArrangeRect.Width;
        if (array1.Length > 0)
          this.DrawGridLines3D(axis, axis.GridLinesRecycler, left2, top1, width, height, array1, index4);
        if (axis.smallTicksRequired)
        {
          double[] array3 = axis.SmallTickPoints.Select<double, double>((Func<double, double>) (pointValues => pointValues)).ToArray<double>();
          if (array3.Length > 0)
            this.DrawGridLines3D(axis, axis.MinorGridLinesRecycler, left2, top1, width, height, array3, index5);
          index5 += array3.Length;
        }
        index4 += array1.Length;
      }
    }
  }

  public Size Measure(Size availableSize)
  {
    this.desiredSize = availableSize;
    return availableSize;
  }

  public Size Arrange(Size finalSize)
  {
    foreach (ChartAxis ax in (Collection<ChartAxis>) this.Area.Axes)
    {
      if (ax.ShowGridLines)
        this.DrawGridLines(ax);
    }
    return finalSize;
  }

  public Size Arrange3D(Size finalSize)
  {
    foreach (ChartAxis axis in this.Area.Axes.Where<ChartAxis>((Func<ChartAxis, bool>) (axis => axis.ShowGridLines)))
      this.DrawGridLines3D(axis);
    return finalSize;
  }

  public void DetachElements()
  {
    this.panel.Children.Clear();
    this.panel = (Panel) null;
    if (this.stripLines == null)
      return;
    this.stripLines.Clear();
  }

  public void UpdateElements()
  {
    foreach (ChartAxis ax in (Collection<ChartAxis>) this.Area.Axes)
      this.UpdateGridLines(ax);
    this.UpdateStripLines();
  }

  public void UpdateGridLines(ChartAxis axis)
  {
    if (axis == null)
      return;
    if (axis.GridLinesRecycler == null)
      axis.CreateLineRecycler();
    int num = 1;
    if (axis.RegisteredSeries.Count > 0)
      num = axis.Orientation == Orientation.Horizontal ? (this.Area.RowDefinitions.Count > 1 ? axis.AssociatedAxes.Count : axis.AssociatedAxes.DistinctBy<ChartAxis, int>(new Func<ChartAxis, int>(this.Area.GetActualRow)).Count<ChartAxis>()) : (this.Area.ColumnDefinitions.Count > 1 ? axis.AssociatedAxes.Count : axis.AssociatedAxes.DistinctBy<ChartAxis, int>(new Func<ChartAxis, int>(this.Area.GetActualColumn)).Count<ChartAxis>());
    int requiredLinescount = axis.SmallTickPoints.Count * num;
    if (!(axis is CategoryAxis categoryAxis) || categoryAxis.LabelPlacement != LabelPlacement.BetweenTicks)
      requiredLinescount = axis.VisibleLabels.Count * num;
    if (axis.smallTicksRequired)
      ChartCartesianGridLinesPanel.UpdateGridlines(axis, axis.MinorGridLinesRecycler, axis.SmallTickPoints.Count * num, false, false);
    ChartCartesianGridLinesPanel.UpdateGridlines(axis, axis.GridLinesRecycler, requiredLinescount, true, true);
  }

  internal void UpdateStripLines()
  {
    if (this.Area is SfChart3D)
      return;
    int stripCount = 0;
    foreach (ChartAxisBase2D axis in this.Area.Axes.Where<ChartAxis>((Func<ChartAxis, bool>) (axis => axis != null && !axis.VisibleRange.IsEmpty && (axis as ChartAxisBase2D).StripLines != null && (axis as ChartAxisBase2D).StripLines.Count > 0)))
    {
      if (axis.Orientation == Orientation.Horizontal)
        this.UpdateHorizontalStripLine(axis, ref stripCount);
      else
        this.UpdateVerticalStripLine(axis, ref stripCount);
    }
  }

  internal void ClearStripLines()
  {
    if (this.stripLines == null)
      return;
    this.stripLines.Clear();
  }

  private static void UpdateGridlines(
    ChartAxis axis,
    UIElementsRecycler<Line> linesRecycler,
    int requiredLinescount,
    bool isMajor,
    bool checkOrginFlag)
  {
    if (linesRecycler == null || axis == null)
      return;
    foreach (DependencyObject dependencyObject in linesRecycler)
      dependencyObject.ClearValue(FrameworkElement.StyleProperty);
    int count = requiredLinescount;
    if (axis.ShowOrigin && checkOrginFlag)
      ++count;
    if (!linesRecycler.BindingProvider.Keys.Contains<DependencyProperty>(FrameworkElement.StyleProperty))
      linesRecycler.BindingProvider.Add(FrameworkElement.StyleProperty, ChartCartesianGridLinesPanel.GetGridLineBinding((object) axis, isMajor));
    if (linesRecycler.Count > 0)
    {
      foreach (FrameworkElement frameworkElement in linesRecycler)
        frameworkElement.SetBinding(FrameworkElement.StyleProperty, (BindingBase) ChartCartesianGridLinesPanel.GetGridLineBinding((object) axis, isMajor));
    }
    linesRecycler.GenerateElements(count);
    ChartAxisRangeStyleCollection rangeStyles = axis.RangeStyles;
    if (rangeStyles == null || rangeStyles.Count <= 0)
      return;
    List<double> doubleList = !isMajor ? axis.SmallTickPoints : axis.VisibleLabels.Select<ChartAxisLabel, double>((Func<ChartAxisLabel, double>) (label => label.Position)).ToList<double>();
    for (int index = 0; index < doubleList.Count; ++index)
    {
      foreach (ChartAxisRangeStyle source in (Collection<ChartAxisRangeStyle>) rangeStyles)
      {
        DoubleRange range = source.Range;
        Style style = isMajor ? source.MajorGridLineStyle : source.MinorGridLineStyle;
        if (range.Start <= doubleList[index] && range.End >= doubleList[index] && style != null)
        {
          linesRecycler[index].SetBinding(FrameworkElement.StyleProperty, (BindingBase) ChartCartesianGridLinesPanel.GetGridLineBinding((object) source, isMajor));
          break;
        }
      }
    }
  }

  private static Binding CreateBinding(string path, object source)
  {
    return new Binding()
    {
      Path = new PropertyPath(path, new object[0]),
      Source = source,
      Mode = BindingMode.OneWay
    };
  }

  private static Binding GetGridLineBinding(object source, bool isMajor)
  {
    return new Binding()
    {
      Path = isMajor ? new PropertyPath("MajorGridLineStyle", new object[0]) : new PropertyPath("MinorGridLineStyle", new object[0]),
      Source = source
    };
  }

  private void DrawGridLines(
    ChartAxis axis,
    UIElementsRecycler<Line> lines,
    double left,
    double top,
    double width,
    double height,
    double[] values,
    bool drawOrigin,
    int index)
  {
    if (lines == null || axis == null)
      return;
    int length = values.Length;
    int count = lines.Count;
    if (axis.Orientation == Orientation.Horizontal)
    {
      for (int index1 = 0; index1 < length; ++index1)
      {
        if (index1 < count)
        {
          Line line = lines[index];
          double d = axis.ValueToCoefficientCalc(values[index1]);
          double num = double.IsNaN(d) ? 0.0 : d;
          line.X1 = Math.Round(width * num) + left;
          line.Y1 = top;
          line.X2 = line.X1;
          line.Y2 = height;
        }
        ++index;
      }
      if (!axis.ShowOrigin && this.panel.Children.Contains((UIElement) this.xOriginLine))
        this.panel.Children.Remove((UIElement) this.xOriginLine);
      if (!axis.ShowOrigin || !drawOrigin)
        return;
      double d1 = this.Area.InternalSecondaryAxis.ValueToCoefficientCalc(axis.Origin);
      double num1 = double.IsNaN(d1) ? 0.0 : d1;
      this.xOriginLine.X1 = 0.0;
      this.xOriginLine.Y1 = this.xOriginLine.Y2 = Math.Round(height * (1.0 - num1));
      this.xOriginLine.X2 = width;
      this.xOriginLine.SetBinding(FrameworkElement.StyleProperty, (BindingBase) new Binding()
      {
        Path = new PropertyPath("OriginLineStyle", new object[0]),
        Source = (object) axis
      });
      if (this.panel.Children.Contains((UIElement) this.xOriginLine))
        return;
      this.panel.Children.Add((UIElement) this.xOriginLine);
    }
    else
    {
      for (int index2 = 0; index2 < length; ++index2)
      {
        if (index2 < count)
        {
          Line line = lines[index];
          double d = axis.ValueToCoefficientCalc(values[index2]);
          double num = double.IsNaN(d) ? 0.0 : d;
          line.X1 = left;
          line.Y1 = Math.Round(height * (1.0 - num)) + 0.5 + top;
          line.X2 = width;
          line.Y2 = line.Y1;
        }
        ++index;
      }
      if (!axis.ShowOrigin && this.panel.Children.Contains((UIElement) this.yOriginLine))
        this.panel.Children.Remove((UIElement) this.yOriginLine);
      if (!axis.ShowOrigin || axis.VisibleRange.Delta <= 0.0 || !drawOrigin)
        return;
      double d2 = this.Area.InternalPrimaryAxis.ValueToCoefficientCalc(axis.Origin);
      double num2 = double.IsNaN(d2) ? 0.0 : d2;
      this.yOriginLine.X1 = this.yOriginLine.X2 = Math.Round(width * num2);
      this.yOriginLine.Y1 = 0.0;
      this.yOriginLine.Y2 = height;
      this.yOriginLine.SetBinding(FrameworkElement.StyleProperty, (BindingBase) new Binding()
      {
        Path = new PropertyPath("OriginLineStyle", new object[0]),
        Source = (object) axis
      });
      if (this.panel.Children.Contains((UIElement) this.yOriginLine))
        return;
      this.panel.Children.Add((UIElement) this.yOriginLine);
    }
  }

  private void DrawGridLines3D(
    ChartAxis axis,
    UIElementsRecycler<Line> lines,
    double left,
    double top,
    double width,
    double height,
    double[] values,
    int index)
  {
    if (lines == null || axis == null)
      return;
    int length = values.Length;
    int count = lines.Count;
    Orientation orientation = axis.Orientation;
    SfChart3D area = this.Area as SfChart3D;
    double actualRotationAngle = area.ActualRotationAngle;
    if (orientation == Orientation.Horizontal)
    {
      ChartAxis chartAxis = this.Area.InternalSecondaryAxis.Orientation == Orientation.Vertical ? this.Area.InternalSecondaryAxis : this.Area.InternalPrimaryAxis;
      for (int index1 = 0; index1 < length; ++index1)
      {
        if (index1 < count)
        {
          Line line1 = lines[index];
          double d = axis.ValueToCoefficientCalc(values[index1]);
          double num1 = double.IsNaN(d) ? 0.0 : d;
          ChartAxisBase3D chartAxisBase3D = axis as ChartAxisBase3D;
          double x1_1;
          double num2;
          if (chartAxisBase3D.IsManhattanAxis && (axis.RegisteredSeries[index1] as ChartSeries3D).Segments != null && (axis.RegisteredSeries[index1] as ChartSeries3D).Segments.Count > 0)
          {
            ChartSegment3D segment = (axis.RegisteredSeries[index1] as ChartSeries3D).Segments[0] as ChartSegment3D;
            num2 = x1_1 = segment.startDepth + (segment.endDepth - segment.startDepth) / 2.0;
          }
          else
            num2 = x1_1 = Math.Round(width * num1) + left;
          double y1 = top;
          double y2 = height;
          if (area != null)
          {
            double num3 = area.ActualDepth > 2.0 ? area.ActualDepth - 2.0 : 1.0;
            Graphics3D graphics3D = ((SfChart3D) this.Area).Graphics3D;
            if (chartAxisBase3D != null && chartAxisBase3D.IsZAxis)
            {
              double num4 = chartAxis.OpposedPosition || actualRotationAngle < 0.0 || actualRotationAngle >= 180.0 ? -axis.ArrangeRect.Right - 1.0 : -axis.ArrangeRect.Left + axis.ArrangeRect.Width;
              Line3D line2 = Polygon3D.CreateLine(line1, x1_1, top, num2, height, num4, num4, true);
              line2.Transform(Matrix3D.Turn(1.5707963705062866));
              graphics3D.AddVisual((Polygon3D) line2);
            }
            else
            {
              double num5 = chartAxis.OpposedPosition || actualRotationAngle < 90.0 || actualRotationAngle >= 270.0 ? num3 : 1.0;
              graphics3D.AddVisual((Polygon3D) Polygon3D.CreateLine(line1, x1_1, y1, num2, y2, num5, num5, true));
            }
            double num6 = area.Axes.Where<ChartAxis>((Func<ChartAxis, bool>) (x => x.Orientation == Orientation.Horizontal && x.OpposedPosition)).Any<ChartAxis>() ? axis.Area.SeriesClipRect.Top : axis.Area.SeriesClipRect.Height + axis.Area.SeriesClipRect.Top;
            Line line3 = new Line();
            line3.Opacity = line1.Opacity;
            line3.StrokeThickness = line1.StrokeThickness;
            line3.Stroke = line1.Stroke;
            Line line4 = line3;
            line4.SetBinding(UIElement.VisibilityProperty, (BindingBase) new Binding()
            {
              Path = new PropertyPath("Visibility", new object[0]),
              Source = (object) line1
            });
            if (chartAxisBase3D != null && chartAxisBase3D.IsZAxis)
            {
              double x1_2;
              double x2;
              if (!chartAxis.OpposedPosition && actualRotationAngle >= 0.0 && actualRotationAngle < 180.0)
              {
                x1_2 = axis.ArrangeRect.Left - axis.ArrangeRect.Width;
                x2 = axis.ArrangeRect.Left;
              }
              else
              {
                x1_2 = axis.ArrangeRect.Left;
                x2 = axis.ArrangeRect.Right;
              }
              Line3D line5 = Polygon3D.CreateLine(line4, x1_2, -x1_1, x2, -x1_1, num6, num6, true);
              line5.Transform(Matrix3D.Tilt(1.5707963705062866));
              graphics3D.AddVisual((Polygon3D) line5);
            }
            else
            {
              Line3D line6 = Polygon3D.CreateLine(line4, num2, 0.0, num2, -num3, num6, num6, true);
              line6.Transform(Matrix3D.Tilt(1.5707963705062866));
              graphics3D.AddVisual((Polygon3D) line6);
            }
          }
        }
        ++index;
      }
    }
    else
    {
      for (int index2 = 0; index2 < length; ++index2)
      {
        if (index2 < count)
        {
          Line line7 = lines[index];
          double d = axis.ValueToCoefficientCalc(values[index2]);
          double num7 = double.IsNaN(d) ? 0.0 : d;
          double x1 = left;
          double y1 = Math.Round(height * (1.0 - num7)) + 0.5 + top;
          double x2 = width;
          double num8 = y1;
          double num9 = area.ActualDepth > 2.0 ? area.ActualDepth - 2.0 : 1.0;
          double num10 = axis.OpposedPosition || actualRotationAngle < 90.0 || actualRotationAngle >= 270.0 ? num9 : 0.0;
          area.Graphics3D.AddVisual((Polygon3D) Polygon3D.CreateLine(line7, x1, y1, x2, num8, num10, num10, true));
          Line line8 = new Line();
          line8.Opacity = line7.Opacity;
          line8.StrokeThickness = line7.StrokeThickness;
          line8.Stroke = line7.Stroke;
          Line line9 = line8;
          line9.SetBinding(UIElement.VisibilityProperty, (BindingBase) new Binding()
          {
            Path = new PropertyPath("Visibility", new object[0]),
            Source = (object) line7
          });
          double num11 = axis.OpposedPosition || !axis.OpposedPosition && actualRotationAngle >= 180.0 && actualRotationAngle < 360.0 ? axis.Area.SeriesClipRect.Width + axis.Area.SeriesClipRect.Left + 1.0 : axis.Area.SeriesClipRect.Left;
          Line3D line10 = Polygon3D.CreateLine(line9, -num9, num8, 0.0, num8, num11, num11, true);
          line10.Transform(Matrix3D.Turn(-1.5707963705062866));
          area.Graphics3D.AddVisual((Polygon3D) line10);
        }
        ++index;
      }
    }
  }

  private void RenderStripLine(Rect stripRect, ChartStripLine stripLine, int index)
  {
    if (stripRect.IsEmpty || index >= this.stripLines.Count)
      return;
    Border generatedElement = this.stripLines.generatedElements[index];
    ContentControl contentControl = new ContentControl();
    generatedElement.SetBinding(Border.BackgroundProperty, (BindingBase) ChartCartesianGridLinesPanel.CreateBinding("Background", (object) stripLine));
    generatedElement.SetBinding(Border.BorderBrushProperty, (BindingBase) ChartCartesianGridLinesPanel.CreateBinding("BorderBrush", (object) stripLine));
    generatedElement.SetBinding(Border.BorderThicknessProperty, (BindingBase) ChartCartesianGridLinesPanel.CreateBinding("BorderThickness", (object) stripLine));
    generatedElement.SetBinding(UIElement.OpacityProperty, (BindingBase) ChartCartesianGridLinesPanel.CreateBinding("Opacity", (object) stripLine));
    generatedElement.SetBinding(UIElement.VisibilityProperty, (BindingBase) ChartCartesianGridLinesPanel.CreateBinding("Visibility", (object) stripLine));
    contentControl.SetBinding(ContentControl.ContentProperty, (BindingBase) ChartCartesianGridLinesPanel.CreateBinding("Label", (object) stripLine));
    contentControl.SetBinding(ContentControl.ContentTemplateProperty, (BindingBase) ChartCartesianGridLinesPanel.CreateBinding("LabelTemplate", (object) stripLine));
    contentControl.SetBinding(UIElement.OpacityProperty, (BindingBase) ChartCartesianGridLinesPanel.CreateBinding("Opacity", (object) stripLine));
    contentControl.RenderTransformOrigin = new Point(0.5, 0.5);
    contentControl.RenderTransform = (Transform) new RotateTransform()
    {
      Angle = stripLine.LabelAngle
    };
    contentControl.SetBinding(FrameworkElement.HorizontalAlignmentProperty, (BindingBase) ChartCartesianGridLinesPanel.CreateBinding("LabelHorizontalAlignment", (object) stripLine));
    contentControl.SetBinding(FrameworkElement.VerticalAlignmentProperty, (BindingBase) ChartCartesianGridLinesPanel.CreateBinding("LabelVerticalAlignment", (object) stripLine));
    generatedElement.Child = (UIElement) contentControl;
    generatedElement.Width = stripRect.Width;
    generatedElement.Height = stripRect.Height;
    Canvas.SetLeft((UIElement) generatedElement, double.IsNaN(stripRect.Left) ? 0.0 : stripRect.Left);
    Canvas.SetTop((UIElement) generatedElement, double.IsNaN(stripRect.Top) ? 0.0 : stripRect.Top);
    Panel.SetZIndex((UIElement) generatedElement, 2);
  }

  private void UpdateHorizontalStripLine(ChartAxisBase2D axis, ref int stripCount)
  {
    Rect seriesClipRect = this.Area.SeriesClipRect;
    DoubleRange visibleRange = axis.VisibleRange;
    Rect arrangeRect1 = axis.ArrangeRect;
    axis.StriplineXRange = DoubleRange.Empty;
    axis.StriplineYRange = DoubleRange.Empty;
    int index = stripCount;
    foreach (ChartStripLine stripLine in (Collection<ChartStripLine>) axis.StripLines)
    {
      ChartAxis ax = this.Area.Axes[stripLine.SegmentAxisName];
      IEnumerable<ChartAxis> chartAxises;
      if (ax != null)
        chartAxises = (IEnumerable<ChartAxis>) new List<ChartAxis>()
        {
          ax
        };
      else if (axis.RegisteredSeries.Count > 0)
        chartAxises = this.Area.RowDefinitions.Count > 1 ? (IEnumerable<ChartAxis>) axis.AssociatedAxes : axis.AssociatedAxes.DistinctBy<ChartAxis, int>(new Func<ChartAxis, int>(this.Area.GetActualRow));
      else
        chartAxises = (IEnumerable<ChartAxis>) new List<ChartAxis>()
        {
          this.Area.InternalSecondaryAxis
        };
      foreach (ChartAxis chartAxis in chartAxises)
      {
        ChartAxis currAxis = chartAxis;
        int num1 = axis.RegisteredSeries.Where<ISupportAxes>((Func<ISupportAxes, bool>) (item => item.ActualXAxis == currAxis || item.ActualYAxis == currAxis)).Count<ISupportAxes>();
        if (num1 == 0 && axis.RegisteredSeries.Count == 0)
          num1 = 1;
        if (num1 > 0)
        {
          Rect arrangeRect2 = currAxis.ArrangeRect;
          Rect stripRect;
          if (!stripLine.IsSegmented)
          {
            double num2 = stripLine.Start;
            double num3 = double.IsNaN(stripLine.RepeatUntil) ? visibleRange.End : stripLine.RepeatUntil;
            double repeatEvery = stripLine.RepeatEvery;
            do
            {
              double start = num2;
              double num4 = !double.IsNaN(stripLine.RepeatUntil) ? (num2 < stripLine.RepeatUntil ? num2 + stripLine.Width : num2 - stripLine.Width) : num2 + stripLine.Width;
              double point = this.Area.ValueToPoint((ChartAxis) axis, num2);
              double x1 = !stripLine.IsPixelWidth ? this.Area.ValueToPoint((ChartAxis) axis, num4) : point + stripLine.Width;
              ChartAxisBase2D chartAxisBase2D1 = axis;
              chartAxisBase2D1.StriplineXRange = chartAxisBase2D1.StriplineXRange + new DoubleRange(start, this.Area.PointToValue((ChartAxis) axis, new Point(x1, 0.0)));
              ChartAxisBase2D chartAxisBase2D2 = axis;
              chartAxisBase2D2.StriplineXRange = chartAxisBase2D2.StriplineXRange + currAxis.StriplineXRange;
              double num5 = arrangeRect1.Width + arrangeRect1.Left - seriesClipRect.Left;
              double x2 = num5 > x1 ? x1 : num5;
              stripRect = new Rect(new Point(point, arrangeRect2.Top - seriesClipRect.Top), new Point(x2, arrangeRect2.Height + arrangeRect2.Top - seriesClipRect.Top));
              if (index >= this.stripLines.Count)
                this.stripLines.CreateNewInstance();
              this.RenderStripLine(stripRect, stripLine, index);
              ++index;
              num2 = num2 < num3 ? num2 + repeatEvery : num2 - repeatEvery;
            }
            while (repeatEvery != 0.0 && (double.IsNaN(stripLine.RepeatUntil) ? (num2 < num3 ? 1 : 0) : (stripLine.Start < stripLine.RepeatUntil ? (num2 < num3 ? 1 : 0) : (num2 > num3 ? 1 : 0))) != 0);
          }
          else
          {
            double start = stripLine.Start;
            double num6 = double.IsNaN(stripLine.RepeatUntil) ? visibleRange.End : stripLine.RepeatUntil;
            double repeatEvery = stripLine.RepeatEvery;
            if (!double.IsNaN(stripLine.SegmentStartValue) && !double.IsNaN(stripLine.SegmentEndValue))
            {
              do
              {
                double point1 = this.Area.ValueToPoint((ChartAxis) axis, start);
                double x;
                if (stripLine.IsPixelWidth)
                {
                  x = point1 + stripLine.Width;
                }
                else
                {
                  double num7 = !double.IsNaN(stripLine.RepeatUntil) ? (start < stripLine.RepeatUntil ? start + stripLine.Width : start - stripLine.Width) : start + stripLine.Width;
                  x = this.Area.ValueToPoint((ChartAxis) axis, num7);
                }
                ChartAxisBase2D chartAxisBase2D3 = axis;
                chartAxisBase2D3.StriplineXRange = chartAxisBase2D3.StriplineXRange + new DoubleRange(start, this.Area.PointToValue((ChartAxis) axis, new Point(x, 0.0)));
                ChartAxisBase2D chartAxisBase2D4 = axis;
                chartAxisBase2D4.StriplineXRange = chartAxisBase2D4.StriplineXRange + currAxis.StriplineXRange;
                if (axis.Area != null)
                {
                  double point2 = this.Area.ValueToPoint(currAxis, stripLine.SegmentStartValue);
                  double point3 = this.Area.ValueToPoint(currAxis, stripLine.SegmentEndValue);
                  stripRect = new Rect(new Point(point1, point2), new Point(x, point3));
                  if (index >= this.stripLines.Count)
                    this.stripLines.CreateNewInstance();
                  this.RenderStripLine(stripRect, stripLine, index);
                  ++index;
                  if (axis.IncludeStripLineRange)
                    currAxis.StriplineYRange += new DoubleRange(stripLine.SegmentStartValue, stripLine.SegmentEndValue);
                }
                start = start < num6 ? start + repeatEvery : start - repeatEvery;
              }
              while (repeatEvery != 0.0 && (double.IsNaN(stripLine.RepeatUntil) ? (start < num6 ? 1 : 0) : (stripLine.Start < stripLine.RepeatUntil ? (start < num6 ? 1 : 0) : (start > num6 ? 1 : 0))) != 0);
            }
          }
        }
      }
    }
    stripCount = index;
  }

  private void UpdateVerticalStripLine(ChartAxisBase2D axis, ref int stripCount)
  {
    DoubleRange visibleRange = axis.VisibleRange;
    Rect seriesClipRect = this.Area.SeriesClipRect;
    axis.StriplineXRange = DoubleRange.Empty;
    axis.StriplineYRange = DoubleRange.Empty;
    int index = stripCount;
    foreach (ChartStripLine stripLine in (Collection<ChartStripLine>) axis.StripLines)
    {
      ChartAxis ax = this.Area.Axes[stripLine.SegmentAxisName];
      IEnumerable<ChartAxis> chartAxises;
      if (ax != null)
        chartAxises = (IEnumerable<ChartAxis>) new List<ChartAxis>()
        {
          ax
        };
      else if (axis.RegisteredSeries.Count > 0)
        chartAxises = this.Area.ColumnDefinitions.Count > 1 ? (IEnumerable<ChartAxis>) axis.AssociatedAxes : axis.AssociatedAxes.DistinctBy<ChartAxis, int>(new Func<ChartAxis, int>(this.Area.GetActualColumn));
      else
        chartAxises = (IEnumerable<ChartAxis>) new List<ChartAxis>()
        {
          this.Area.InternalPrimaryAxis
        };
      foreach (ChartAxis chartAxis in chartAxises)
      {
        ChartAxis xAxis = chartAxis;
        int num1 = axis.RegisteredSeries.Where<ISupportAxes>((Func<ISupportAxes, bool>) (item => item.ActualXAxis == xAxis || item.ActualYAxis == xAxis)).Count<ISupportAxes>();
        if (num1 == 0 && axis.RegisteredSeries.Count == 0)
          num1 = 1;
        if (num1 > 0)
        {
          Rect arrangeRect1 = xAxis.ArrangeRect;
          Rect arrangeRect2 = axis.ArrangeRect;
          Rect stripRect;
          if (!stripLine.IsSegmented)
          {
            double start = stripLine.Start;
            double num2 = double.IsNaN(stripLine.RepeatUntil) ? visibleRange.End : stripLine.RepeatUntil;
            double repeatEvery = stripLine.RepeatEvery;
            do
            {
              double point = this.Area.ValueToPoint((ChartAxis) axis, start);
              double y1;
              if (stripLine.IsPixelWidth)
              {
                y1 = stripLine.Width < arrangeRect2.Height ? point - stripLine.Width : point - arrangeRect2.Height;
              }
              else
              {
                double num3 = !double.IsNaN(stripLine.RepeatUntil) ? (start < stripLine.RepeatUntil ? start + stripLine.Width : start - stripLine.Width) : start + stripLine.Width;
                y1 = this.Area.ValueToPoint((ChartAxis) axis, num3);
              }
              ChartAxisBase2D chartAxisBase2D = axis;
              chartAxisBase2D.StriplineYRange = chartAxisBase2D.StriplineYRange + new DoubleRange(start, this.Area.PointToValue((ChartAxis) axis, new Point(0.0, y1)));
              double num4 = arrangeRect2.Top - seriesClipRect.Top;
              double y2 = y1 > num4 ? y1 : num4;
              stripRect = new Rect(new Point(arrangeRect1.Left - seriesClipRect.Left, point), new Point(arrangeRect1.Width + arrangeRect1.Left - seriesClipRect.Left, y2));
              if (index >= this.stripLines.Count)
                this.stripLines.CreateNewInstance();
              this.RenderStripLine(stripRect, stripLine, index);
              ++index;
              start = start < num2 ? start + repeatEvery : start - repeatEvery;
            }
            while (repeatEvery != 0.0 && (double.IsNaN(stripLine.RepeatUntil) ? (start < num2 ? 1 : 0) : (stripLine.Start < stripLine.RepeatUntil ? (start < num2 ? 1 : 0) : (start > num2 ? 1 : 0))) != 0);
          }
          else
          {
            double start = stripLine.Start;
            double num5 = double.IsNaN(stripLine.RepeatUntil) ? visibleRange.End : stripLine.RepeatUntil;
            double repeatEvery = stripLine.RepeatEvery;
            if (!double.IsNaN(stripLine.SegmentStartValue) && !double.IsNaN(stripLine.SegmentEndValue))
            {
              do
              {
                double point1 = this.Area.ValueToPoint((ChartAxis) axis, start);
                double y;
                if (stripLine.IsPixelWidth)
                {
                  y = point1 - stripLine.Width;
                }
                else
                {
                  double num6 = !double.IsNaN(stripLine.RepeatUntil) ? (start < stripLine.RepeatUntil ? start + stripLine.Width : start - stripLine.Width) : start + stripLine.Width;
                  y = this.Area.ValueToPoint((ChartAxis) axis, num6);
                }
                ChartAxisBase2D chartAxisBase2D1 = axis;
                chartAxisBase2D1.StriplineYRange = chartAxisBase2D1.StriplineYRange + new DoubleRange(start, this.Area.PointToValue((ChartAxis) axis, new Point(0.0, y)));
                if (axis.Area != null && axis.Area.InternalPrimaryAxis != null)
                {
                  double point2 = this.Area.ValueToPoint(xAxis, stripLine.SegmentStartValue);
                  double point3 = this.Area.ValueToPoint(xAxis, stripLine.SegmentEndValue);
                  stripRect = new Rect(new Point(point2, point1), new Point(point3, y));
                  if (index >= this.stripLines.Count)
                    this.stripLines.CreateNewInstance();
                  this.RenderStripLine(stripRect, stripLine, index);
                  ++index;
                  if (axis.IncludeStripLineRange)
                  {
                    ChartAxisBase2D chartAxisBase2D2 = axis;
                    chartAxisBase2D2.StriplineXRange = chartAxisBase2D2.StriplineXRange + new DoubleRange(stripLine.SegmentStartValue, stripLine.SegmentEndValue);
                  }
                }
                start = start < num5 ? start + repeatEvery : start - repeatEvery;
              }
              while (repeatEvery != 0.0 && (double.IsNaN(stripLine.RepeatUntil) ? (start < num5 ? 1 : 0) : (stripLine.Start < stripLine.RepeatUntil ? (start < num5 ? 1 : 0) : (start > num5 ? 1 : 0))) != 0);
            }
          }
        }
      }
    }
    stripCount = index;
  }
}
