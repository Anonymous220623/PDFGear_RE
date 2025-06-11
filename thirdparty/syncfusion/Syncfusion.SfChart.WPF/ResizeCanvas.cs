// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.ResizeCanvas
// Assembly: Syncfusion.SfChart.WPF, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 279D70C6-7936-4F3E-B6CF-79EFEF734826
// Assembly location: C:\Program Files\PDFgear\Syncfusion.SfChart.WPF.dll

using System.Windows;
using System.Windows.Controls;

#nullable disable
namespace Syncfusion.UI.Xaml.Charts;

public class ResizeCanvas : Canvas
{
  protected override Size MeasureOverride(Size constraint)
  {
    Size availableSize = new Size(double.PositiveInfinity, double.PositiveInfinity);
    double num1 = 0.0;
    double num2 = 0.0;
    foreach (UIElement child in this.Children)
    {
      if (child != null)
      {
        child.Measure(availableSize);
        double left = Canvas.GetLeft(child);
        double height = child.DesiredSize.Height;
        double num3 = left + child.DesiredSize.Width;
        num2 = num2 < num3 ? num3 : num2;
        num1 = num1 < height ? height : num1;
      }
    }
    return new Size()
    {
      Height = num1 == 0.0 ? 17.0 : num1,
      Width = constraint.Width
    };
  }

  protected override Size ArrangeOverride(Size arrangeSize)
  {
    foreach (UIElement child in this.Children)
    {
      if (child is TextBlock)
      {
        double length = arrangeSize.Height / 2.0 - child.DesiredSize.Height / 2.0;
        Canvas.SetTop(child, length);
      }
    }
    return base.ArrangeOverride(arrangeSize);
  }
}
