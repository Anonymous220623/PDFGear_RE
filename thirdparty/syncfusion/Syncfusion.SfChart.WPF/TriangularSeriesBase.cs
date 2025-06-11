// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.TriangularSeriesBase
// Assembly: Syncfusion.SfChart.WPF, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 279D70C6-7936-4F3E-B6CF-79EFEF734826
// Assembly location: C:\Program Files\PDFgear\Syncfusion.SfChart.WPF.dll

using System.Windows;

#nullable disable
namespace Syncfusion.UI.Xaml.Charts;

public abstract class TriangularSeriesBase : AccumulationSeriesBase
{
  public static readonly DependencyProperty GapRatioProperty = DependencyProperty.Register(nameof (GapRatio), typeof (double), typeof (TriangularSeriesBase), new PropertyMetadata((object) 0.0, new PropertyChangedCallback(TriangularSeriesBase.OnGapRatioChanged)));
  public static readonly DependencyProperty ExplodeOffsetProperty = DependencyProperty.Register(nameof (ExplodeOffset), typeof (double), typeof (TriangularSeriesBase), new PropertyMetadata((object) 40.0, new PropertyChangedCallback(TriangularSeriesBase.OnExplodeOffsetChanged)));

  public double GapRatio
  {
    get => (double) this.GetValue(TriangularSeriesBase.GapRatioProperty);
    set => this.SetValue(TriangularSeriesBase.GapRatioProperty, (object) value);
  }

  public double ExplodeOffset
  {
    get => (double) this.GetValue(TriangularSeriesBase.ExplodeOffsetProperty);
    set => this.SetValue(TriangularSeriesBase.ExplodeOffsetProperty, (object) value);
  }

  protected override DependencyObject CloneSeries(DependencyObject obj)
  {
    if (obj is TriangularSeriesBase triangularSeriesBase)
    {
      triangularSeriesBase.GapRatio = this.GapRatio;
      triangularSeriesBase.ExplodeOffset = this.ExplodeOffset;
    }
    return base.CloneSeries(obj);
  }

  internal override Point GetDataPointPosition(ChartTooltip tooltip)
  {
    Point dataPointPosition = new Point();
    if (this.Area == null || !this.Area.RootPanelDesiredSize.HasValue)
      return dataPointPosition;
    Size size = this.Area.RootPanelDesiredSize.Value;
    double width = this.Area.RootPanelDesiredSize.Value.Width;
    double height = this.Area.RootPanelDesiredSize.Value.Height;
    dataPointPosition.X = width / 2.0;
    if (tooltip.DataContext is FunnelSegment)
    {
      FunnelSegment dataContext = tooltip.DataContext as FunnelSegment;
      dataPointPosition.Y = dataContext.top * height + dataContext.height;
    }
    else if (tooltip.DataContext is PyramidSegment)
    {
      PyramidSegment dataContext = tooltip.DataContext as PyramidSegment;
      dataPointPosition.Y = dataContext.y * height + dataContext.height;
    }
    return dataPointPosition;
  }

  private static void OnGapRatioChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    if (!(d is TriangularSeriesBase triangularSeriesBase) || triangularSeriesBase.Area == null)
      return;
    triangularSeriesBase.Area.ScheduleUpdate();
  }

  private static void OnExplodeOffsetChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    if (!(d is TriangularSeriesBase triangularSeriesBase) || triangularSeriesBase.Area == null)
      return;
    triangularSeriesBase.Area.ScheduleUpdate();
  }
}
