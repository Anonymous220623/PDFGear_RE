// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.SparklinePointsInfo
// Assembly: Syncfusion.SfChart.WPF, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 279D70C6-7936-4F3E-B6CF-79EFEF734826
// Assembly location: C:\Program Files\PDFgear\Syncfusion.SfChart.WPF.dll

using System.Windows;

#nullable disable
namespace Syncfusion.UI.Xaml.Charts;

public class SparklinePointsInfo
{
  private Point coordinate;
  private Point values;

  public Point Coordinate
  {
    get => this.coordinate;
    set => this.coordinate = value;
  }

  public Point Value
  {
    get => this.values;
    set => this.values = value;
  }
}
