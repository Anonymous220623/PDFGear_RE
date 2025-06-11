// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.SurfaceAxisPanel
// Assembly: Syncfusion.SfChart.WPF, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 279D70C6-7936-4F3E-B6CF-79EFEF734826
// Assembly location: C:\Program Files\PDFgear\Syncfusion.SfChart.WPF.dll

using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

#nullable disable
namespace Syncfusion.UI.Xaml.Charts;

public class SurfaceAxisPanel : Canvas
{
  internal SurfaceAxis Axis;
  internal List<ILayoutCalculator> LayoutCalc;

  public SurfaceAxisPanel() => this.LayoutCalc = new List<ILayoutCalculator>();

  internal Size ComputeSize(Size availableSize)
  {
    Size size = Size.Empty;
    double width = 0.0;
    double height = 0.0;
    FrameworkElement child1 = this.Children[0] as FrameworkElement;
    child1.HorizontalAlignment = HorizontalAlignment.Center;
    child1.VerticalAlignment = VerticalAlignment.Center;
    if (this.Axis.Orientation == Orientation.Vertical)
    {
      double num = (this.Axis.OpposedPosition ? 1.0 : -1.0) * 90.0;
      RotateTransform rotateTransform = new RotateTransform()
      {
        Angle = num
      };
      child1.RenderTransform = (Transform) rotateTransform;
    }
    else
      child1.RenderTransform = (Transform) null;
    foreach (UIElement child2 in this.Children)
    {
      child2.Measure(availableSize);
      bool flag = child1 == child2 && this.Axis.Orientation == Orientation.Vertical;
      width += flag ? child2.DesiredSize.Height : child2.DesiredSize.Width;
      height += flag ? child2.DesiredSize.Width : child2.DesiredSize.Height;
    }
    foreach (ILayoutCalculator layoutCalculator in this.LayoutCalc)
    {
      layoutCalculator.Measure(availableSize);
      width += layoutCalculator.DesiredSize.Width;
      height += layoutCalculator.DesiredSize.Height;
      size = this.Axis.Orientation != Orientation.Vertical ? new Size(availableSize.Width, height) : new Size(width, availableSize.Height);
    }
    return ChartLayoutUtils.CheckSize(size);
  }

  internal void ArrangeElements(Size finalSize)
  {
    if (this.Axis == null)
      return;
    this.ArrangeCartesianElements(finalSize);
  }

  private void ArrangeCartesianElements(Size finalSize)
  {
    if (this.Axis == null)
      return;
    foreach (UIElement child in this.Children)
    {
      Canvas.SetLeft(child, 0.0);
      Canvas.SetTop(child, 0.0);
    }
    UIElement child1 = this.Children[0];
    ILayoutCalculator layoutCalculator1 = this.LayoutCalc[0];
    ILayoutCalculator layoutCalculator2 = this.LayoutCalc[1];
    List<UIElement> uiElementList = new List<UIElement>();
    List<Size> sizeList = new List<Size>();
    bool flag1 = this.Axis.Orientation == Orientation.Vertical;
    bool flag2 = this.Axis.OpposedPosition ^ flag1;
    uiElementList.Add((UIElement) layoutCalculator2.Panel);
    sizeList.Add(layoutCalculator2.DesiredSize);
    uiElementList.Add((UIElement) layoutCalculator1.Panel);
    sizeList.Add(layoutCalculator1.DesiredSize);
    uiElementList.Add(child1);
    Size size = child1.DesiredSize;
    if (flag1)
      size = new Size(size.Height, size.Width);
    sizeList.Add(size);
    if (flag2)
    {
      uiElementList.Reverse();
      sizeList.Reverse();
    }
    double length = 0.0;
    for (int index = 0; index < uiElementList.Count; ++index)
    {
      UIElement element = uiElementList[index];
      if (flag1)
      {
        if (element == child1)
        {
          Canvas.SetTop(element, (finalSize.Height - element.DesiredSize.Height) / 2.0);
          Canvas.SetLeft(element, length - (element.DesiredSize.Width - sizeList[index].Width) / 2.0);
        }
        else
          Canvas.SetLeft(element, length);
        length += sizeList[index].Width;
      }
      else
      {
        if (element == child1)
          Canvas.SetLeft(element, (finalSize.Width - sizeList[index].Width) / 2.0);
        Canvas.SetTop(element, length);
        length += sizeList[index].Height;
      }
    }
    foreach (ILayoutCalculator layoutCalculator3 in this.LayoutCalc)
      layoutCalculator3.Arrange(layoutCalculator3.DesiredSize);
  }
}
