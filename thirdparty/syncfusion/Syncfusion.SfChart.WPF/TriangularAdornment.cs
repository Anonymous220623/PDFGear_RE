// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.TriangularAdornment
// Assembly: Syncfusion.SfChart.WPF, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 279D70C6-7936-4F3E-B6CF-79EFEF734826
// Assembly location: C:\Program Files\PDFgear\Syncfusion.SfChart.WPF.dll

using System.Windows;

#nullable disable
namespace Syncfusion.UI.Xaml.Charts;

public class TriangularAdornment : ChartAdornment
{
  private double CurrY;
  private double Height;

  public TriangularAdornment(
    double xVal,
    double yVal,
    double currY,
    double height,
    AdornmentSeries series)
  {
    this.XPos = this.XData = xVal;
    this.YPos = this.YData = yVal;
    this.CurrY = currY;
    this.Height = height;
    this.Series = this.Series = (ChartSeriesBase) series;
  }

  public override void SetData(params double[] Values)
  {
    this.XPos = this.XData = Values[0];
    this.YPos = this.YData = Values[1];
  }

  public override void Update(IChartTransformer transformer)
  {
    Point center = ChartLayoutUtils.GetCenter(transformer.Viewport);
    double y = center.Y;
    center.Y = 0.0;
    center.Y += this.CurrY * y * 2.0 - this.Height / 2.0 * 4.0;
    this.X = center.X;
    this.Y = center.Y;
  }
}
