// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.ClearUIElementProperties
// Assembly: Syncfusion.SfChart.WPF, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 279D70C6-7936-4F3E-B6CF-79EFEF734826
// Assembly location: C:\Program Files\PDFgear\Syncfusion.SfChart.WPF.dll

using System.Windows;
using System.Windows.Shapes;

#nullable disable
namespace Syncfusion.UI.Xaml.Charts;

internal static class ClearUIElementProperties
{
  internal static void ClearUIValues(this Shape element)
  {
    switch (element)
    {
      case Line _:
        element.ClearValue(Line.X1Property);
        element.ClearValue(Line.X2Property);
        element.ClearValue(Line.Y1Property);
        element.ClearValue(Line.Y2Property);
        break;
      case Rectangle _:
        element.ClearValue(FrameworkElement.WidthProperty);
        element.ClearValue(FrameworkElement.HeightProperty);
        break;
      case Ellipse _:
        element.ClearValue(FrameworkElement.WidthProperty);
        element.ClearValue(FrameworkElement.HeightProperty);
        break;
    }
  }
}
