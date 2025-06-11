// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.SurfaceAxisElementPanel
// Assembly: Syncfusion.SfChart.WPF, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 279D70C6-7936-4F3E-B6CF-79EFEF734826
// Assembly location: C:\Program Files\PDFgear\Syncfusion.SfChart.WPF.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Shapes;

#nullable disable
namespace Syncfusion.UI.Xaml.Charts;

public class SurfaceAxisElementPanel : ILayoutCalculator
{
  private Line mainAxisLine;
  private UIElementsRecycler<Line> majorTicksRecycler;
  private UIElementsRecycler<Line> minorTicksRecycler;
  private Size desiredSize;
  private SurfaceAxis axis;
  private Panel labelsPanels;

  public double Left { get; set; }

  public double Top { get; set; }

  public Panel Panel => this.labelsPanels;

  internal SurfaceAxis Axis
  {
    get => this.axis;
    set
    {
      this.axis = value;
      this.SetAxisLineBinding();
    }
  }

  private void SetAxisLineBinding()
  {
    this.mainAxisLine.SetBinding(FrameworkElement.StyleProperty, (BindingBase) new Binding()
    {
      Source = (object) this.axis,
      Path = new PropertyPath("AxisLineStyle", new object[0])
    });
  }

  public Size DesiredSize => this.desiredSize;

  public List<UIElement> Children
  {
    get
    {
      return this.labelsPanels != null ? this.labelsPanels.Children.Cast<UIElement>().ToList<UIElement>() : (List<UIElement>) null;
    }
  }

  public SurfaceAxisElementPanel(Panel panel)
  {
    this.labelsPanels = panel;
    this.mainAxisLine = new Line();
    panel?.Children.Add((UIElement) this.mainAxisLine);
    this.majorTicksRecycler = new UIElementsRecycler<Line>(panel);
    this.minorTicksRecycler = new UIElementsRecycler<Line>(panel);
  }

  public Size Measure(Size availableSize)
  {
    Size size = Size.Empty;
    double val2 = 5.0;
    size = this.Axis.Orientation != Orientation.Horizontal ? new Size(Math.Max(Math.Max(this.Axis.TickLineSize, val2), 0.0) + this.mainAxisLine.StrokeThickness, availableSize.Height) : new Size(availableSize.Width, Math.Max(Math.Max(this.Axis.TickLineSize, val2), 0.0) + this.mainAxisLine.StrokeThickness);
    this.desiredSize = size;
    return size;
  }

  public void DetachElements()
  {
    if (this.mainAxisLine != null && this.Children != null && this.Children.Contains((UIElement) this.mainAxisLine))
      this.Children.Remove((UIElement) this.mainAxisLine);
    if (this.majorTicksRecycler != null)
      this.majorTicksRecycler.Clear();
    if (this.minorTicksRecycler != null)
      this.minorTicksRecycler.Clear();
    this.labelsPanels = (Panel) null;
  }

  public Size Arrange(Size finalSize)
  {
    double[] array1 = this.Axis.VisibleLabels.Select<ChartAxisLabel, double>((Func<ChartAxisLabel, double>) (val => val.Position)).ToArray<double>();
    this.RenderAxisLine(finalSize);
    this.RenderTicks(finalSize, this.majorTicksRecycler, this.Axis.Orientation, this.Axis.TickLineSize, array1);
    if (this.Axis.smallTicksRequired)
    {
      double[] array2 = this.Axis.SmallTickPoints.Select<double, double>((Func<double, double>) (val => val)).ToArray<double>();
      this.RenderTicks(finalSize, this.minorTicksRecycler, this.Axis.Orientation, this.Axis.TickLineSize, array2);
    }
    return finalSize;
  }

  internal void UpdateTicks()
  {
    this.UpdateTicks(this.Axis.VisibleLabels.Count, this.majorTicksRecycler, "MajorTickLineStyle");
    if (!this.Axis.smallTicksRequired)
      return;
    this.UpdateTicks(this.Axis.SmallTickPoints.Count, this.minorTicksRecycler, "MinorTickLineStyle");
  }

  private void UpdateTicks(
    int linescount,
    UIElementsRecycler<Line> lineRecycler,
    string lineStylePath)
  {
    int count = linescount;
    if (!lineRecycler.BindingProvider.Keys.Contains<DependencyProperty>(FrameworkElement.StyleProperty))
    {
      lineRecycler.BindingProvider.Add(FrameworkElement.StyleProperty, new Binding()
      {
        Source = (object) this.Axis,
        Path = new PropertyPath(lineStylePath, new object[0])
      });
      lineRecycler.BindingProvider.Add(UIElement.VisibilityProperty, new Binding()
      {
        Source = (object) this.Axis,
        Path = new PropertyPath("Visibility", new object[0])
      });
    }
    lineRecycler.GenerateElements(count);
  }

  private void RenderAxisLine(Size finalSize)
  {
    double width = finalSize.Width;
    double height = finalSize.Height;
    Orientation orientation = this.Axis.Orientation;
    bool opposedPosition = this.Axis.OpposedPosition;
    double num1;
    double num2;
    double num3;
    double num4;
    if (orientation == Orientation.Horizontal)
    {
      num1 = 0.0;
      num2 = width - 0.0;
      num4 = num3 = opposedPosition ? height : 0.0;
    }
    else
    {
      num1 = num2 = opposedPosition ? 0.0 : width;
      num4 = 0.0;
      num3 = height;
    }
    if (this.mainAxisLine == null)
      return;
    this.mainAxisLine.X1 = num1;
    this.mainAxisLine.Y1 = num4;
    this.mainAxisLine.X2 = num2;
    this.mainAxisLine.Y2 = num3;
  }

  private void RenderTicks(
    Size finalSize,
    UIElementsRecycler<Line> linesRecycler,
    Orientation orientation,
    double tickSize,
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
        double coefficient = this.Axis.ValueToCoefficient(Values[index]);
        double num = double.IsNaN(coefficient) ? 0.0 : coefficient;
        if (orientation == Orientation.Horizontal)
        {
          Size axisDesiredSize = this.Axis.AxisDesiredSize;
          x1 = x2 = Math.Round(axisDesiredSize.Width * num);
        }
        else
        {
          Size axisDesiredSize = this.Axis.AxisDesiredSize;
          y1 = y2 = Math.Round(axisDesiredSize.Height * (1.0 - num));
        }
        this.CalculatePosition(tickSize, width, height, ref x1, ref y1, ref x2, ref y2);
        line.X1 = x1;
        line.X2 = x2;
        line.Y1 = y1;
        line.Y2 = y2;
      }
    }
  }

  private void CalculatePosition(
    double tickSize,
    double width,
    double height,
    ref double x1,
    ref double y1,
    ref double x2,
    ref double y2)
  {
    Orientation orientation = this.Axis.Orientation;
    bool opposedPosition = this.Axis.OpposedPosition;
    if (orientation == Orientation.Horizontal)
    {
      y1 = this.mainAxisLine.Y1 + this.mainAxisLine.StrokeThickness / 2.0;
      y2 = opposedPosition ? y1 - tickSize : y1 + tickSize;
    }
    else
    {
      x1 = this.mainAxisLine.X1 - this.mainAxisLine.StrokeThickness / 2.0;
      x2 = opposedPosition ? x1 + tickSize : x1 - tickSize;
    }
  }

  public void UpdateElements() => this.UpdateTicks();
}
