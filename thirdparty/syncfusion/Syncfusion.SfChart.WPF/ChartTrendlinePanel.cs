// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.ChartTrendlinePanel
// Assembly: Syncfusion.SfChart.WPF, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 279D70C6-7936-4F3E-B6CF-79EFEF734826
// Assembly location: C:\Program Files\PDFgear\Syncfusion.SfChart.WPF.dll

using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;

#nullable disable
namespace Syncfusion.UI.Xaml.Charts;

public class ChartTrendlinePanel : Canvas
{
  private TrendlineBase trend;

  internal TrendlineBase Trend
  {
    get => this.trend;
    set
    {
      if (this.Trend != null)
        this.Trend.TrendlineSegments.CollectionChanged -= new NotifyCollectionChangedEventHandler(this.OnSegmentsCollectionChanged);
      this.trend = value;
      if (this.trend == null)
        return;
      this.trend.TrendlineSegments.CollectionChanged += new NotifyCollectionChangedEventHandler(this.OnSegmentsCollectionChanged);
    }
  }

  internal void Update(Size finalSize)
  {
    IChartTransformer transformer = this.Trend.Series.CreateTransformer(finalSize, true);
    foreach (ChartSegment trendlineSegment in (Collection<ChartSegment>) this.Trend.TrendlineSegments)
      trendlineSegment.Update(transformer);
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
      UIElement visual = newItem.CreateVisual(Size.Empty);
      if (visual == null)
        return;
      this.Children.Add(visual);
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
      this.Children.Remove(renderedVisual);
      oldItem.IsAddedToVisualTree = false;
    }
    else if (e.Action == NotifyCollectionChangedAction.Reset)
    {
      this.Children.Clear();
    }
    else
    {
      if (e.Action != NotifyCollectionChangedAction.Replace)
        return;
      ChartSegment newItem = e.NewItems[0] as ChartSegment;
      if (newItem.IsAddedToVisualTree)
        return;
      UIElement segmentVisual = newItem.CreateSegmentVisual(Size.Empty);
      if (segmentVisual == null)
        return;
      this.Children.Add(segmentVisual);
      newItem.IsAddedToVisualTree = true;
    }
  }
}
