// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.ChartRootPanel
// Assembly: Syncfusion.SfChart.WPF, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 279D70C6-7936-4F3E-B6CF-79EFEF734826
// Assembly location: C:\Program Files\PDFgear\Syncfusion.SfChart.WPF.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

#nullable disable
namespace Syncfusion.UI.Xaml.Charts;

public class ChartRootPanel : Panel
{
  public static readonly DependencyProperty MeasurePriorityIndexProperty = DependencyProperty.RegisterAttached("MeasurePriorityIndex", typeof (int), typeof (ChartRootPanel), new PropertyMetadata((object) 0));

  internal ChartBase Area { get; set; }

  internal SfSurfaceChart Surface { get; set; }

  public static int GetMeasurePriorityIndex(DependencyObject obj)
  {
    return (int) obj.GetValue(ChartRootPanel.MeasurePriorityIndexProperty);
  }

  public static void SetMeasurePriorityIndex(DependencyObject obj, int value)
  {
    obj.SetValue(ChartRootPanel.MeasurePriorityIndexProperty, (object) value);
  }

  protected override Size MeasureOverride(Size availableSize)
  {
    List<UIElement> source = new List<UIElement>();
    Size size = ChartLayoutUtils.CheckSize(availableSize);
    if (this.Area != null)
      this.Area.RootPanelDesiredSize = new Size?(size);
    if (this.Surface != null)
      this.Surface.RootPanelDesiredSize = new Size?(size);
    foreach (UIElement child in this.Children)
      source.Add(child);
    foreach (UIElement uiElement in (IEnumerable<UIElement>) source.OrderBy<UIElement, int>(new Func<UIElement, int>(ChartRootPanel.GetMeasurePriorityIndex)))
      uiElement.Measure(availableSize);
    return size;
  }

  protected override Size ArrangeOverride(Size finalSize)
  {
    for (int index = this.Children.Count - 1; index >= 0; --index)
      this.Children[index].Arrange(new Rect(0.0, 0.0, finalSize.Width, finalSize.Height));
    return finalSize;
  }
}
