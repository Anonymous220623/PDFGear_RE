// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.ColorExtension
// Assembly: Syncfusion.SfChart.WPF, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 279D70C6-7936-4F3E-B6CF-79EFEF734826
// Assembly location: C:\Program Files\PDFgear\Syncfusion.SfChart.WPF.dll

using System.Windows.Media;

#nullable disable
namespace Syncfusion.UI.Xaml.Charts;

internal static class ColorExtension
{
  internal static Brush GetContrastColor(this Brush brush)
  {
    if (!(brush is SolidColorBrush solidColorBrush))
      return (Brush) new SolidColorBrush(Colors.Black);
    Color color = solidColorBrush.Color;
    return 1.0 - (0.299 * (double) color.R + 0.587 * (double) color.G + 0.114 * (double) color.B) / (double) byte.MaxValue >= 0.5 ? (Brush) new SolidColorBrush(Colors.White) : (Brush) new SolidColorBrush(Colors.Black);
  }
}
