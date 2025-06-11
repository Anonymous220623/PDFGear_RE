// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.ChartPieAdornment
// Assembly: Syncfusion.SfChart.WPF, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 279D70C6-7936-4F3E-B6CF-79EFEF734826
// Assembly location: C:\Program Files\PDFgear\Syncfusion.SfChart.WPF.dll

using System;
using System.Linq;
using System.Windows;

#nullable disable
namespace Syncfusion.UI.Xaml.Charts;

public class ChartPieAdornment : ChartAdornment
{
  private double angle;
  private double radius;
  private int pieIndex;

  public ChartPieAdornment(
    double xVal,
    double yVal,
    double angle,
    double radius,
    AdornmentSeries series)
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

  internal double InnerDoughnutRadius { get; set; }

  public override void SetData(params double[] Values)
  {
    this.XPos = this.XData = Values[0];
    this.YPos = this.YData = Values[1];
    this.Angle = Values[2];
    this.Radius = Values[3];
  }

  public override void Update(IChartTransformer transformer)
  {
    double num1 = this.Radius;
    double num2 = Math.Min(transformer.Viewport.Width, transformer.Viewport.Height) / 2.0;
    if (this.Series is PieSeries)
    {
      PieSeries series = this.Series as PieSeries;
      double pieSeriesCount = (double) series.GetPieSeriesCount();
      double num3 = num2 / pieSeriesCount * (double) this.pieIndex;
      Point point = pieSeriesCount != 1.0 ? ChartLayoutUtils.GetCenter(transformer.Viewport) : series.Center;
      if (series != null && series.LabelPosition == CircularSeriesLabelPosition.Inside)
      {
        if (this.pieIndex > 0)
        {
          double num4 = (num1 - num3) / 2.0;
          num1 -= num4;
        }
        else
          num1 /= 2.0;
      }
      this.X = point.X + num1 * Math.Cos(this.Angle);
      this.Y = point.Y + num1 * Math.Sin(this.Angle);
    }
    else
    {
      if (!(this.Series is DoughnutSeries))
        return;
      DoughnutSeries series = this.Series as DoughnutSeries;
      double num5 = num2 * series.InternalDoughnutSize;
      double num6 = this.Angle;
      Point point;
      if (series.IsStackedDoughnut)
      {
        int index = this.Series.Adornments.IndexOf((ChartAdornment) this);
        DoughnutSegment segment = series.Segments[index] as DoughnutSegment;
        int count = series.Segments.Count;
        point = series.Center;
        double num7 = (num5 - num5 * this.Series.ActualArea.InternalDoughnutHoleSize) / (double) count * series.InternalDoughnutCoefficient;
        double num8 = num5 - num7 * (double) (count - (segment.DoughnutSegmentIndex + 1));
        double num9;
        this.InnerDoughnutRadius = num9 = num8 - num7;
        double num10 = num8 - num7 * series.SegmentSpacing;
        this.Radius = num10;
        double num11 = ChartMath.MaxZero(num9);
        double num12 = (num10 - num11) / 2.0;
        num1 = num10 - num12;
        num6 = series.LabelPosition == CircularSeriesLabelPosition.Outside ? segment.StartAngle : (series.LabelPosition == CircularSeriesLabelPosition.OutsideExtended ? segment.EndAngle : this.Angle);
      }
      else
      {
        int doughnutSeriesCount = series.GetDoughnutSeriesCount();
        point = doughnutSeriesCount == 1 ? series.Center : ChartLayoutUtils.GetCenter(transformer.Viewport);
        double num13 = (num5 - num5 * this.Series.ActualArea.InternalDoughnutHoleSize) / (double) doughnutSeriesCount;
        double num14 = ChartMath.MaxZero(this.InnerDoughnutRadius = num1 - num13 * series.InternalDoughnutCoefficient);
        if (series != null && series.LabelPosition == CircularSeriesLabelPosition.Inside)
        {
          double num15 = (num1 - num14) / 2.0;
          num1 -= num15;
        }
      }
      this.X = point.X + num1 * Math.Cos(num6);
      this.Y = point.Y + num1 * Math.Sin(num6);
    }
  }

  internal void SetValues(
    double xVal,
    double yVal,
    double angle,
    double radius,
    AdornmentSeries series)
  {
    this.XPos = this.XData = xVal;
    this.YPos = this.YData = yVal;
    this.Radius = radius;
    this.Angle = angle;
    this.Series = this.Series = (ChartSeriesBase) series;
    this.pieIndex = this.Series.ActualArea.VisibleSeries.Where<ChartSeriesBase>((Func<ChartSeriesBase, bool>) (pieSeries => pieSeries is CircularSeriesBase)).ToList<ChartSeriesBase>().IndexOf((ChartSeriesBase) series);
  }
}
