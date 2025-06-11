// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.ChartSeries
// Assembly: Syncfusion.SfChart.WPF, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 279D70C6-7936-4F3E-B6CF-79EFEF734826
// Assembly location: C:\Program Files\PDFgear\Syncfusion.SfChart.WPF.dll

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

#nullable disable
namespace Syncfusion.UI.Xaml.Charts;

public abstract class ChartSeries : ChartSeriesBase
{
  public static readonly DependencyProperty StrokeProperty = DependencyProperty.Register(nameof (Stroke), typeof (Brush), typeof (ChartSeries), new PropertyMetadata((object) null, new PropertyChangedCallback(ChartSeries.OnAppearanceChanged)));
  public static readonly DependencyProperty StrokeThicknessProperty = DependencyProperty.Register(nameof (StrokeThickness), typeof (double), typeof (ChartSeries), new PropertyMetadata((object) 2.0, new PropertyChangedCallback(ChartSeries.OnAppearanceChanged)));
  internal List<int> selectedSegmentPixels = new List<int>();
  internal HashSet<int> upperSeriesPixels = new HashSet<int>();
  private bool isLoading = true;

  public double StrokeThickness
  {
    get => (double) this.GetValue(ChartSeries.StrokeThicknessProperty);
    set => this.SetValue(ChartSeries.StrokeThicknessProperty, (object) value);
  }

  public SfChart Area
  {
    get => this.ActualArea as SfChart;
    internal set
    {
      this.ActualArea = (ChartBase) value;
      if (this.ActualArea == null)
        return;
      this.ActualArea.IsLoading = true;
    }
  }

  public Brush Stroke
  {
    get => (Brush) this.GetValue(ChartSeries.StrokeProperty);
    set => this.SetValue(ChartSeries.StrokeProperty, (object) value);
  }

  internal bool IsLoading
  {
    get => this.isLoading;
    set
    {
      if (this.isLoading == value || this.Area == null)
        return;
      this.isLoading = value;
      if (!this.isLoading)
      {
        if (this.Area.VisibleSeries != null && this.Area.VisibleSeries.Count == 0 && this.Area.TechnicalIndicators != null && this.Area.TechnicalIndicators.Count == 0)
        {
          this.Area.IsLoading = false;
        }
        else
        {
          if (this.Area.TechnicalIndicators != null)
          {
            foreach (ChartSeries technicalIndicator in (Collection<ChartSeries>) this.Area.TechnicalIndicators)
            {
              if (technicalIndicator.IsLoading)
                return;
            }
          }
          if (this.Area.VisibleSeries != null)
          {
            foreach (ChartSeries chartSeries in (Collection<ChartSeriesBase>) this.Area.VisibleSeries)
            {
              if (chartSeries.IsLoading)
                return;
            }
          }
          this.Area.IsLoading = false;
        }
      }
      else
        this.Area.IsLoading = true;
    }
  }

  internal override void Dispose()
  {
    this.Area = (SfChart) null;
    base.Dispose();
  }

  public int GetDataPointIndex(double x, double y) => this.GetDataPointIndex(new Point(x, y));

  internal static int ConvertColor(Color color)
  {
    int num = (int) color.A + 1;
    return (int) color.A << 24 | (int) (byte) ((int) color.R * num >> 8) << 16 /*0x10*/ | (int) (byte) ((int) color.G * num >> 8) << 8 | (int) (byte) ((int) color.B * num >> 8);
  }

  internal virtual int GetDataPointIndex(Point point)
  {
    Canvas adorningCanvas = this.Area.GetAdorningCanvas();
    double num1 = this.Area.ActualWidth - adorningCanvas.ActualWidth;
    double num2 = this.Area.ActualHeight - adorningCanvas.ActualHeight;
    point.X = point.X - num1 + this.Area.Margin.Left;
    point.Y = point.Y - num2 + this.Area.Margin.Top;
    ChartDataPointInfo dataPoint = this.GetDataPoint(point);
    return dataPoint != null ? dataPoint.Index : -1;
  }

  internal virtual void GeneratePixels()
  {
  }

  internal virtual bool IsHitTestSeries()
  {
    if (!this.Area.isBitmapPixelsConverted)
      this.Area.ConvertBitmapPixels();
    return this.Pixels.Contains(this.Area.currentBitmapPixel);
  }

  internal unsafe void OnBitmapSelection(List<int> pixels, Brush brush, bool isSelected)
  {
    if (pixels == null || pixels.Count <= 0)
      return;
    int seriesIndex = this.Area.Series.IndexOf(this);
    if (!this.Area.isBitmapPixelsConverted)
      this.Area.ConvertBitmapPixels();
    foreach (ChartSeriesBase chartSeriesBase in this.Area.Series.Where<ChartSeries>((Func<ChartSeries, bool>) (series => this.Area.Series.IndexOf(series) > seriesIndex)).ToList<ChartSeries>())
      this.upperSeriesPixels.UnionWith((IEnumerable<int>) chartSeriesBase.Pixels);
    WriteableBitmap fastRenderSurface = this.Area.fastRenderSurface;
    fastRenderSurface.Lock();
    int* pixels1 = fastRenderSurface.GetPixels();
    int num;
    if (isSelected && brush != null)
    {
      num = ChartSeries.ConvertColor((brush as SolidColorBrush).Color);
    }
    else
    {
      switch (this)
      {
        case FastHiLoOpenCloseBitmapSeries _:
          num = ChartSeries.ConvertColor((this.Segments[0] as FastHiLoOpenCloseSegment).GetSegmentBrush(this.dataPoint.Index));
          break;
        case FastCandleBitmapSeries _:
          num = ChartSeries.ConvertColor((this.Segments[0] as FastCandleBitmapSegment).GetSegmentBrush(this.dataPoint.Index));
          break;
        default:
          Brush interiorColor = this.GetInteriorColor(this.dataPoint.Index);
          num = ChartSeries.ConvertColor(interiorColor is LinearGradientBrush ? (interiorColor as LinearGradientBrush).GradientStops[0].Color : (interiorColor as SolidColorBrush).Color);
          break;
      }
    }
    foreach (int pixel in pixels)
    {
      if (this.Pixels.Contains(pixel) && !this.upperSeriesPixels.Contains(pixel))
        pixels1[pixel] = num;
    }
    fastRenderSurface.AddDirtyRect(new Int32Rect(0, 0, fastRenderSurface.PixelWidth, fastRenderSurface.PixelHeight));
    fastRenderSurface.Unlock();
    this.upperSeriesPixels.Clear();
  }

  public override void OnApplyTemplate()
  {
    base.OnApplyTemplate();
    this.AdornmentPresenter = this.GetTemplateChild("adornmentPresenter") as ChartAdornmentPresenter;
    this.SeriesRootPanel = this.GetTemplateChild("PART_SeriesRootPanel") as Panel;
    this.SeriesPanel = this.GetTemplateChild("seriesPanel") as ChartSeriesPanel;
    this.SeriesPanel.Series = this;
    if (!(this is StackingSeriesBase stackingSeriesBase) || this is FastStackingColumnBitmapSeries)
      return;
    if (stackingSeriesBase.IsSideBySide)
    {
      if (this.adornmentInfo == null)
        return;
      switch (this.adornmentInfo.GetAdornmentPosition())
      {
        case AdornmentsPosition.Top:
        case AdornmentsPosition.TopAndBottom:
          Panel.SetZIndex((UIElement) this.SeriesPanel.Series, -this.ActualArea.GetSeriesIndex((ChartSeriesBase) this));
          break;
        default:
          Panel.SetZIndex((UIElement) this.SeriesPanel.Series, this.ActualArea.GetSeriesIndex((ChartSeriesBase) this));
          break;
      }
    }
    else
      Panel.SetZIndex((UIElement) this.SeriesPanel.Series, -this.ActualArea.GetSeriesIndex((ChartSeriesBase) this));
  }

  protected override DependencyObject CloneSeries(DependencyObject obj)
  {
    ChartSeries chartSeries = obj as ChartSeries;
    chartSeries.Stroke = this.Stroke;
    chartSeries.StrokeThickness = this.StrokeThickness;
    return base.CloneSeries((DependencyObject) chartSeries);
  }

  protected override AutomationPeer OnCreateAutomationPeer()
  {
    return (AutomationPeer) new ChartSeriesAutomationPeer(this);
  }

  protected abstract ChartSegment CreateSegment();

  private static void OnAppearanceChanged(
    DependencyObject obj,
    DependencyPropertyChangedEventArgs args)
  {
    if (!(obj is ChartSeries chartSeries))
      return;
    ChartSeries.OnAppearanceChanged(chartSeries);
  }

  private static void OnAppearanceChanged(ChartSeries obj)
  {
    if (!obj.IsBitmapSeries)
      return;
    obj.UpdateArea();
  }
}
