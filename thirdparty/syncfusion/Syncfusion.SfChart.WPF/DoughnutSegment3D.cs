// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.DoughnutSegment3D
// Assembly: Syncfusion.SfChart.WPF, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 279D70C6-7936-4F3E-B6CF-79EFEF734826
// Assembly location: C:\Program Files\PDFgear\Syncfusion.SfChart.WPF.dll

#nullable disable
namespace Syncfusion.UI.Xaml.Charts;

public class DoughnutSegment3D(
  ChartSeries3D series,
  Vector3D center,
  double start,
  double end,
  double height,
  double r,
  int i,
  double y,
  double insideRadius) : PieSegment3D(series, center, start, end, height, r, i, y, insideRadius)
{
}
