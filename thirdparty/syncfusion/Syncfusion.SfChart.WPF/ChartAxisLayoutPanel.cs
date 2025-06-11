// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.ChartAxisLayoutPanel
// Assembly: Syncfusion.SfChart.WPF, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 279D70C6-7936-4F3E-B6CF-79EFEF734826
// Assembly location: C:\Program Files\PDFgear\Syncfusion.SfChart.WPF.dll

using System.Windows;
using System.Windows.Controls;

#nullable disable
namespace Syncfusion.UI.Xaml.Charts;

public class ChartAxisLayoutPanel : Panel
{
  public ILayoutCalculator AxisLayout { get; set; }

  protected override Size MeasureOverride(Size availableSize)
  {
    availableSize = ChartLayoutUtils.CheckSize(availableSize);
    if (this.AxisLayout != null)
      this.AxisLayout.Measure(availableSize);
    return availableSize;
  }

  protected override Size ArrangeOverride(Size finalSize)
  {
    if (this.AxisLayout != null)
      this.AxisLayout.Arrange(finalSize);
    return finalSize;
  }
}
