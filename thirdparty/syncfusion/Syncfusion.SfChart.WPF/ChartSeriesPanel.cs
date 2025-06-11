// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.ChartSeriesPanel
// Assembly: Syncfusion.SfChart.WPF, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 279D70C6-7936-4F3E-B6CF-79EFEF734826
// Assembly location: C:\Program Files\PDFgear\Syncfusion.SfChart.WPF.dll

using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;

#nullable disable
namespace Syncfusion.UI.Xaml.Charts;

public class ChartSeriesPanel : Canvas
{
  private ChartSeries series;

  internal ChartSeries Series
  {
    get => this.series;
    set
    {
      if (this.series != null)
        this.series.Segments.CollectionChanged -= new NotifyCollectionChangedEventHandler(this.OnSegmentsCollectionChanged);
      this.series = value;
      if (this.series == null)
        return;
      this.series.Segments.CollectionChanged += new NotifyCollectionChangedEventHandler(this.OnSegmentsCollectionChanged);
      this.AddItems();
    }
  }

  internal void Dispose()
  {
    if (this.series != null && this.series.Segments != null)
      this.series.Segments.CollectionChanged -= new NotifyCollectionChangedEventHandler(this.OnSegmentsCollectionChanged);
    this.Children.Clear();
    this.series = (ChartSeries) null;
  }

  internal void Update(Size finalSize)
  {
    bool flag = !(this.Series is ISupportAxes);
    if (this.Series is ISupportAxes && this.Series.ActualXAxis != null && this.Series.ActualYAxis != null)
    {
      flag = true;
      if (this.Series.Area != null)
        this.Series.Area.ClearBuffer();
    }
    if (!flag)
      return;
    IChartTransformer transformer = this.Series.CreateTransformer(finalSize, true);
    if (this.Series is CircularSeriesBase)
    {
      CircularSeriesBase series1 = this.Series as CircularSeriesBase;
      Rect rect = ChartLayoutUtils.Subtractthickness(new Rect(new Point(), finalSize), this.Series.Margin);
      transformer = this.Series.CreateTransformer(new Size(rect.Width, rect.Height), true);
      double radius = (series1 is PieSeries pieSeries ? pieSeries.InternalPieCoefficient : (series1 as DoughnutSeries).InternalDoughnutCoefficient) * Math.Min(transformer.Viewport.Width, transformer.Viewport.Height) / 2.0;
      series1.Center = series1.GetActualCenter(new Point(transformer.Viewport.Width * 0.5, transformer.Viewport.Height * 0.5), radius);
      if (this.series is DoughnutSeries)
      {
        DoughnutSeries series2 = this.series as DoughnutSeries;
        ContentControl centerView = series2.CenterView;
        if (!this.Children.Contains((UIElement) centerView))
          series2.AddCenterView(centerView);
        series2.PositioningCenterView();
      }
    }
    this.Series.Pixels.Clear();
    this.Series.bitmapPixels.Clear();
    this.Series.bitmapRects.Clear();
    foreach (ChartSegment segment in (Collection<ChartSegment>) this.Series.Segments)
      segment.Update(transformer);
    if (this.Series.CanAnimate && this.Series.Segments.Count > 0)
    {
      this.Series.Animate();
      this.Series.CanAnimate = false;
    }
    if (!this.Series.IsLoading)
      return;
    this.Series.IsLoading = false;
  }

  protected override Size ArrangeOverride(Size finalSize)
  {
    foreach (UIElement child in this.Children)
      child.Arrange(new Rect(0.0, 0.0, finalSize.Width, finalSize.Height));
    base.ArrangeOverride(finalSize);
    return finalSize;
  }

  private void OnSegmentsCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
  {
    if (e.Action == NotifyCollectionChangedAction.Add)
    {
      ChartSegment newItem = e.NewItems[0] as ChartSegment;
      if (newItem.IsAddedToVisualTree)
        return;
      UIElement segmentVisual = newItem.CreateSegmentVisual(Size.Empty);
      if (segmentVisual == null)
        return;
      if (this.Series is DoughnutSeries series)
        series.ManipulateAdditionalVisual(segmentVisual, e.Action);
      this.Children.Add(segmentVisual);
      newItem.IsAddedToVisualTree = true;
    }
    else if (e.Action == NotifyCollectionChangedAction.Remove)
    {
      ChartSegment oldItem = e.OldItems[0] as ChartSegment;
      if (!oldItem.IsAddedToVisualTree)
        return;
      UIElement renderedVisual = oldItem.GetRenderedVisual();
      if (renderedVisual == null || !this.Children.Contains(renderedVisual))
        return;
      if (this.Series is DoughnutSeries series)
        series.ManipulateAdditionalVisual(renderedVisual, e.Action);
      this.Children.Remove(renderedVisual);
      oldItem.IsAddedToVisualTree = false;
    }
    else if (e.Action == NotifyCollectionChangedAction.Reset)
    {
      for (int index = this.Children.Count - 1; index > -1; --index)
      {
        if (!(this.Children[index] is TrendlineBase))
          this.Children.RemoveAt(index);
      }
    }
    else
    {
      if (e.Action != NotifyCollectionChangedAction.Replace)
        return;
      ChartSegment newItem = e.NewItems[0] as ChartSegment;
      if (!newItem.IsAddedToVisualTree)
      {
        UIElement segmentVisual = newItem.CreateSegmentVisual(Size.Empty);
        if (this.Series is DoughnutSeries series)
          series.ManipulateAdditionalVisual(segmentVisual, e.Action);
        this.Children.Add(segmentVisual);
        newItem.IsAddedToVisualTree = true;
      }
      foreach (ChartSegment oldItem in (IEnumerable) e.OldItems)
      {
        UIElement renderedVisual = oldItem.GetRenderedVisual();
        if (this.Children.Contains(renderedVisual))
          this.Children.Remove(renderedVisual);
        oldItem.IsAddedToVisualTree = false;
      }
    }
  }

  private void AddItems()
  {
    foreach (ChartSegment segment in (Collection<ChartSegment>) this.Series.Segments)
    {
      if (!segment.IsAddedToVisualTree)
      {
        UIElement segmentVisual = segment.CreateSegmentVisual(Size.Empty);
        if (segmentVisual != null)
        {
          if (this.Series is DoughnutSeries series)
            series.ManipulateAdditionalVisual(segmentVisual, NotifyCollectionChangedAction.Add);
          this.Children.Add(segmentVisual);
          segment.IsAddedToVisualTree = true;
        }
      }
    }
  }
}
