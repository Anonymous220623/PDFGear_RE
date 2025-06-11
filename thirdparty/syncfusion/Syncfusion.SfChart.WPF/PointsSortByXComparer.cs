// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.PointsSortByXComparer
// Assembly: Syncfusion.SfChart.WPF, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 279D70C6-7936-4F3E-B6CF-79EFEF734826
// Assembly location: C:\Program Files\PDFgear\Syncfusion.SfChart.WPF.dll

using System.Collections.Generic;
using System.Windows;

#nullable disable
namespace Syncfusion.UI.Xaml.Charts;

public class PointsSortByXComparer : Comparer<Point>
{
  private double diff;

  public override int Compare(Point point1, Point point2)
  {
    this.diff = point1.X - point2.X;
    if (this.diff == 0.0)
      return 0;
    return this.diff >= 0.0 ? 1 : -1;
  }
}
