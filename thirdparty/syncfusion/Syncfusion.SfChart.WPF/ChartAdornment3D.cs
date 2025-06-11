// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.ChartAdornment3D
// Assembly: Syncfusion.SfChart.WPF, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 279D70C6-7936-4F3E-B6CF-79EFEF734826
// Assembly location: C:\Program Files\PDFgear\Syncfusion.SfChart.WPF.dll

#nullable disable
namespace Syncfusion.UI.Xaml.Charts;

public class ChartAdornment3D : ChartAdornment
{
  internal double StartDepth { get; set; }

  internal double ActualStartDepth { get; set; }

  public ChartAdornment3D()
  {
  }

  public ChartAdornment3D(
    double xVal,
    double yVal,
    double x,
    double y,
    double startDepth,
    ChartSeriesBase series)
  {
    this.StartDepth = startDepth;
    this.XData = xVal;
    this.YData = yVal;
    this.XPos = x;
    this.YPos = y;
    this.Series = series;
  }

  public override void SetData(params double[] Values)
  {
    this.XData = Values[0];
    this.YData = Values[1];
    this.XPos = Values[2];
    this.YPos = Values[3];
    this.StartDepth = Values[4];
  }
}
