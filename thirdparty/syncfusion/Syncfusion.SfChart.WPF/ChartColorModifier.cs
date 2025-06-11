// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.ChartColorModifier
// Assembly: Syncfusion.SfChart.WPF, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 279D70C6-7936-4F3E-B6CF-79EFEF734826
// Assembly location: C:\Program Files\PDFgear\Syncfusion.SfChart.WPF.dll

using System.Windows.Media;

#nullable disable
namespace Syncfusion.UI.Xaml.Charts;

internal static class ChartColorModifier
{
  internal static Brush GetDarkenedColor(Brush brush, double darkCoefficient)
  {
    Color color1;
    switch (brush)
    {
      case SolidColorBrush _:
        color1 = (brush as SolidColorBrush).Color;
        break;
      case GradientBrush _:
        if ((brush as GradientBrush).GradientStops.Count > 0)
        {
          color1 = (brush as GradientBrush).GradientStops[0].Color;
          break;
        }
        goto default;
      default:
        color1 = new SolidColorBrush(Colors.Transparent).Color;
        break;
    }
    Color color2 = color1;
    return (Brush) new SolidColorBrush(Color.FromArgb(color2.A, (byte) ((double) color2.R * darkCoefficient), (byte) ((double) color2.G * darkCoefficient), (byte) ((double) color2.B * darkCoefficient)));
  }
}
