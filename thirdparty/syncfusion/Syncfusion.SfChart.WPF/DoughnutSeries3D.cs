// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.DoughnutSeries3D
// Assembly: Syncfusion.SfChart.WPF, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 279D70C6-7936-4F3E-B6CF-79EFEF734826
// Assembly location: C:\Program Files\PDFgear\Syncfusion.SfChart.WPF.dll

using System;
using System.Windows;

#nullable disable
namespace Syncfusion.UI.Xaml.Charts;

public class DoughnutSeries3D : PieSeries3D
{
  public static readonly DependencyProperty DoughnutCoefficientProperty = DependencyProperty.Register(nameof (DoughnutCoefficient), typeof (double), typeof (DoughnutSeries3D), new PropertyMetadata((object) 0.8, new PropertyChangedCallback(DoughnutSeries3D.OnDoughnutCoefficientChanged)));
  public static readonly DependencyProperty DoughnutHoleSizeProperty = DependencyProperty.RegisterAttached("DoughnutHoleSize", typeof (double), typeof (DoughnutSeries3D), new PropertyMetadata((object) 0.5, new PropertyChangedCallback(DoughnutSeries3D.OnDoughnutHoleSizeChanged)));

  public DoughnutSeries3D() => this.InternlDoughnutCoefficient = 0.8;

  public double DoughnutCoefficient
  {
    get => (double) this.GetValue(DoughnutSeries3D.DoughnutCoefficientProperty);
    set => this.SetValue(DoughnutSeries3D.DoughnutCoefficientProperty, (object) value);
  }

  internal double InternlDoughnutCoefficient { get; set; }

  public static double GetDoughnutHoleSize(DependencyObject obj)
  {
    return (double) obj.GetValue(DoughnutSeries3D.DoughnutHoleSizeProperty);
  }

  public static void SetDoughnutHoleSize(DependencyObject obj, double value)
  {
    obj.SetValue(DoughnutSeries3D.DoughnutHoleSizeProperty, (object) value);
  }

  protected override DependencyObject CloneSeries(DependencyObject obj)
  {
    return base.CloneSeries((DependencyObject) new DoughnutSeries3D()
    {
      DoughnutCoefficient = this.DoughnutCoefficient
    });
  }

  protected override void CreatePoints()
  {
    if (this.Area.RootPanelDesiredSize.HasValue)
    {
      this.actualWidth = this.Area.RootPanelDesiredSize.Value.Width;
      this.actualHeight = this.Area.RootPanelDesiredSize.Value.Height;
    }
    int pieSeriesIndex = this.GetPieSeriesIndex();
    int circularSeriesCount = this.GetCircularSeriesCount();
    double num1 = this.InternalCircleCoefficient * Math.Min(this.actualWidth, this.actualHeight) / 2.0;
    double num2 = (num1 - num1 * this.Area.InternalDoughnutHoleSize) / (double) circularSeriesCount;
    this.Radius = num1 - num2 * (double) (circularSeriesCount - (pieSeriesIndex + 1));
    this.InnerRadius = this.Radius - num2 * this.InternlDoughnutCoefficient;
    this.InnerRadius = ChartMath.MaxZero(this.InnerRadius);
    base.CreatePoints();
  }

  private static void OnDoughnutCoefficientChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    DoughnutSeries3D doughnutSeries3D = d as DoughnutSeries3D;
    doughnutSeries3D.InternlDoughnutCoefficient = ChartMath.MinMax((double) e.NewValue, 0.0, 1.0);
    doughnutSeries3D.UpdateArea();
  }

  private static void OnDoughnutHoleSizeChanged(
    DependencyObject sender,
    DependencyPropertyChangedEventArgs e)
  {
    if (!(sender is SfChart3D sfChart3D))
      return;
    sfChart3D.InternalDoughnutHoleSize = ChartMath.MinMax((double) e.NewValue, 0.0, 1.0);
    sfChart3D.ScheduleUpdate();
  }
}
