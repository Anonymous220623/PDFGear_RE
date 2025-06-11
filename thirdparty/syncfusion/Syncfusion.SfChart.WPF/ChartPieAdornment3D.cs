// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.ChartPieAdornment3D
// Assembly: Syncfusion.SfChart.WPF, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 279D70C6-7936-4F3E-B6CF-79EFEF734826
// Assembly location: C:\Program Files\PDFgear\Syncfusion.SfChart.WPF.dll

using System;
using System.Linq;
using System.Windows;

#nullable disable
namespace Syncfusion.UI.Xaml.Charts;

public class ChartPieAdornment3D : ChartAdornment3D
{
  private double angle;
  private double radius;
  private readonly int pieIndex;

  public ChartPieAdornment3D(
    double startDepth,
    double xVal,
    double yVal,
    double angle,
    double radius,
    ChartSeries3D series)
  {
    this.pieIndex = series.ActualArea.VisibleSeries.Where<ChartSeriesBase>((Func<ChartSeriesBase, bool>) (pieSeries => pieSeries is PieSeries3D)).ToList<ChartSeriesBase>().IndexOf((ChartSeriesBase) series);
  }

  public ChartPieAdornment3D()
  {
  }

  public double Angle
  {
    get => this.angle;
    internal set
    {
      this.angle = value;
      this.OnPropertyChanged(nameof (Angle));
    }
  }

  public double Radius
  {
    get => this.radius;
    internal set
    {
      this.radius = value;
      this.OnPropertyChanged(nameof (Radius));
    }
  }

  public override void SetData(params double[] values)
  {
    this.XPos = this.XData = values[0];
    this.YPos = this.YData = values[1];
    this.Angle = values[2];
    this.Radius = values[3];
  }

  public override void Update(IChartTransformer transformer)
  {
    double radius = this.Radius;
    double num1 = Math.Min(transformer.Viewport.Width, transformer.Viewport.Height) / 2.0;
    if (this.Series is DoughnutSeries3D)
    {
      DoughnutSeries3D series = this.Series as DoughnutSeries3D;
      double circularSeriesCount = (double) series.GetCircularSeriesCount();
      double num2 = num1 * series.InternalCircleCoefficient;
      Point point = series.Center;
      if (circularSeriesCount > 1.0)
        point = new Point(transformer.Viewport.Width / 2.0, transformer.Viewport.Height / 2.0);
      double num3 = (num2 - num2 * this.Series.ActualArea.InternalDoughnutHoleSize) / circularSeriesCount;
      double num4 = ChartMath.MaxZero(radius - num3 * series.InternlDoughnutCoefficient);
      if (series.ExplodeIndex == series.Segments.Count - 1 || series.ExplodeAll)
      {
        Rect rect = new Rect(0.0, 0.0, transformer.Viewport.Width, transformer.Viewport.Height);
        point = series.GetActualCenter(new Point(rect.X + rect.Width / 2.0, rect.Y + rect.Height / 2.0), this.Radius);
      }
      if (series != null && series.LabelPosition == CircularSeriesLabelPosition.Inside)
      {
        double num5 = (radius - num4) / 2.0;
        radius -= num5;
      }
      this.X = point.X + radius * Math.Cos(this.Angle);
      this.Y = point.Y + radius * Math.Sin(this.Angle);
    }
    else if (this.Series is PieSeries3D)
    {
      PieSeries3D series = this.Series as PieSeries3D;
      Point point = series.Center;
      double circularSeriesCount = (double) series.GetCircularSeriesCount();
      double num6 = num1 / circularSeriesCount * (double) this.pieIndex;
      if (series.ExplodeIndex == series.Segments.Count - 1 || series.ExplodeAll)
      {
        Rect rect = new Rect(0.0, 0.0, transformer.Viewport.Width, transformer.Viewport.Height);
        point = series.GetActualCenter(new Point(rect.X + rect.Width / 2.0, rect.Y + rect.Height / 2.0), this.Radius);
      }
      if (series != null && series.LabelPosition == CircularSeriesLabelPosition.Inside)
      {
        if (this.pieIndex == 0)
        {
          radius /= 2.0;
        }
        else
        {
          double num7 = (radius - num6) / 2.0;
          radius -= num7;
        }
      }
      this.X = point.X + radius * Math.Cos(this.Angle);
      this.Y = point.Y + radius * Math.Sin(this.Angle);
    }
    this.ActualStartDepth = this.StartDepth;
  }
}
