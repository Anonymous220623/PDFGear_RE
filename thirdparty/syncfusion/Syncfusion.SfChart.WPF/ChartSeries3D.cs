// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.ChartSeries3D
// Assembly: Syncfusion.SfChart.WPF, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 279D70C6-7936-4F3E-B6CF-79EFEF734826
// Assembly location: C:\Program Files\PDFgear\Syncfusion.SfChart.WPF.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

#nullable disable
namespace Syncfusion.UI.Xaml.Charts;

public abstract class ChartSeries3D : ChartSeriesBase
{
  public static readonly DependencyProperty SegmentSelectionBrushProperty = DependencyProperty.Register(nameof (SegmentSelectionBrush), typeof (Brush), typeof (ChartSeries3D), new PropertyMetadata((object) null, new PropertyChangedCallback(ChartSeries3D.OnSegmentSelectionBrush)));
  public static readonly DependencyProperty AdornmentsInfoProperty = DependencyProperty.Register(nameof (AdornmentsInfo), typeof (ChartAdornmentInfo3D), typeof (ChartSeries3D), new PropertyMetadata((object) null, new PropertyChangedCallback(ChartSeries3D.OnAdornmentsInfoChanged)));
  public static readonly DependencyProperty SelectedIndexProperty = DependencyProperty.Register(nameof (SelectedIndex), typeof (int), typeof (ChartSeries3D), new PropertyMetadata((object) -1, new PropertyChangedCallback(ChartSeries3D.OnSelectedIndexChanged)));
  public static readonly DependencyProperty SelectionModeProperty = DependencyProperty.Register(nameof (SelectionMode), typeof (SelectionMode), typeof (ChartSeries3D), new PropertyMetadata((object) SelectionMode.MouseClick));

  public ChartSeries3D() => this.PrevSelectedIndex = -1;

  public Brush SegmentSelectionBrush
  {
    get => (Brush) this.GetValue(ChartSeries3D.SegmentSelectionBrushProperty);
    set => this.SetValue(ChartSeries3D.SegmentSelectionBrushProperty, (object) value);
  }

  public ChartAdornmentInfo3D AdornmentsInfo
  {
    get => (ChartAdornmentInfo3D) this.GetValue(ChartSeries3D.AdornmentsInfoProperty);
    set => this.SetValue(ChartSeries3D.AdornmentsInfoProperty, (object) value);
  }

  public int SelectedIndex
  {
    get => (int) this.GetValue(ChartSeries3D.SelectedIndexProperty);
    set => this.SetValue(ChartSeries3D.SelectedIndexProperty, (object) value);
  }

  public SelectionMode SelectionMode
  {
    get => (SelectionMode) this.GetValue(ChartSeries3D.SelectionModeProperty);
    set => this.SetValue(ChartSeries3D.SelectionModeProperty, (object) value);
  }

  internal int PrevSelectedIndex { get; set; }

  internal Storyboard AnimationStoryboard { get; set; }

  public SfChart3D Area
  {
    get => this.ActualArea as SfChart3D;
    internal set => this.ActualArea = (ChartBase) value;
  }

  protected internal override List<ChartSegment> SelectedSegments
  {
    get
    {
      return this.SelectedSegmentsIndexes.Count > 0 ? this.SelectedSegmentsIndexes.Where<int>((Func<int, bool>) (index => index < this.Segments.Count)).Select<int, ChartSegment>((Func<int, ChartSegment>) (index => this.Segments[index])).ToList<ChartSegment>() : (List<ChartSegment>) null;
    }
  }

  protected internal override ChartSegment SelectedSegment
  {
    get
    {
      return this.SelectedIndex >= 0 && this.SelectedIndex < this.Segments.Count ? this.Segments[this.SelectedIndex] : (ChartSegment) null;
    }
  }

  internal override void Dispose()
  {
    this.Area = (SfChart3D) null;
    this.ActualArea = (ChartBase) null;
    if (this.AnimationStoryboard != null)
    {
      this.AnimationStoryboard.Stop();
      this.AnimationStoryboard.Children.Clear();
      this.AnimationStoryboard = (Storyboard) null;
    }
    base.Dispose();
  }

  public virtual Brush GetSeriesSelectionBrush(ChartSeriesBase series)
  {
    return series.SeriesSelectionBrush;
  }

  internal DoubleRange GetZSideBySideInfo(ChartSeriesBase currentseries)
  {
    if (this.ActualArea == null || this.ActualArea.InternalPrimaryAxis == null || this.ActualArea.InternalSecondaryAxis == null || this.ActualArea.InternalDepthAxis == null)
      return DoubleRange.Empty;
    if (!this.ActualArea.SBSInfoCalculated || !this.ActualArea.SeriesPosition.ContainsKey((object) currentseries))
      this.CalculateSideBySidePositions(false);
    double num1 = 1.0 - ChartSeriesBase.GetSpacing((DependencyObject) this);
    double minWidth = 0.0;
    double zminPointsDelta = (this.ActualArea as SfChart3D).ZMinPointsDelta;
    if (!double.IsNaN(zminPointsDelta))
      minWidth = zminPointsDelta;
    XyzDataSeries3D xyzDataSeries3D = currentseries as XyzDataSeries3D;
    int num2 = currentseries.IsActualTransposed ? this.ActualArea.GetActualRow((UIElement) xyzDataSeries3D.ActualZAxis) : this.ActualArea.GetActualRow((UIElement) xyzDataSeries3D.ActualYAxis);
    int num3 = currentseries.IsActualTransposed ? this.ActualArea.GetActualColumn((UIElement) xyzDataSeries3D.ActualYAxis) : this.ActualArea.GetActualColumn((UIElement) xyzDataSeries3D.ActualZAxis);
    int index1 = currentseries.ActualYAxis == null ? 0 : num2;
    int index2 = xyzDataSeries3D.ActualZAxis == null ? 0 : num3;
    if (index1 >= this.ActualArea.SbsSeriesCount.GetLength(0) || index2 >= this.ActualArea.SbsSeriesCount.GetLength(1))
      return DoubleRange.Empty;
    int all = this.ActualArea.SbsSeriesCount[index1, index2];
    if (!this.ActualArea.SeriesPosition.ContainsKey((object) currentseries))
      return DoubleRange.Empty;
    int pos = this.ActualArea.SeriesPosition[(object) currentseries];
    if (all == 0)
    {
      all = 1;
      pos = 1;
    }
    double num4 = minWidth * num1 / (double) all;
    double start = num4 * (double) (pos - 1) - minWidth * num1 / 2.0;
    double end = start + num4;
    this.CalculateSideBySideInfoPadding(minWidth, all, pos, false);
    return new DoubleRange(start, end);
  }

  internal override void SelectedSegmentsIndexes_CollectionChanged(
    object sender,
    NotifyCollectionChangedEventArgs e)
  {
    switch (e.Action)
    {
      case NotifyCollectionChangedAction.Add:
        if (e.NewItems == null || (this.ActualArea as SfChart3D).SelectionStyle == SelectionStyle3D.Single)
          break;
        int previousSelectedIndex = this.PreviousSelectedIndex;
        int newItem = (int) e.NewItems[0];
        if (newItem < 0 || !(this.ActualArea as SfChart3D).EnableSegmentSelection)
          break;
        if (newItem < this.Segments.Count && this.SegmentSelectionBrush != null)
        {
          ChartSegment3D segment = this.Segments[newItem] as ChartSegment3D;
          segment.BindProperties();
          foreach (Polygon3D polygon in segment.Polygons)
          {
            polygon.Fill = this.SegmentSelectionBrush;
            polygon.ReDraw();
          }
        }
        if (newItem < this.Segments.Count)
        {
          ChartSelectionChangedEventArgs eventArgs = new ChartSelectionChangedEventArgs()
          {
            SelectedSegment = this.SelectedSegment,
            SelectedSegments = this.Area.SelectedSegments,
            SelectedSeries = (ChartSeriesBase) this,
            SelectedIndex = newItem,
            PreviousSelectedIndex = previousSelectedIndex,
            NewPointInfo = this.Segments[newItem].Item,
            PreviousSelectedSegment = (ChartSegment) null,
            PreviousSelectedSeries = (ChartSeriesBase) null,
            IsSelected = true
          };
          eventArgs.PreviousSelectedSeries = this.Area.PreviousSelectedSeries;
          if (previousSelectedIndex != -1 && previousSelectedIndex < this.Segments.Count)
          {
            eventArgs.PreviousSelectedSegment = this.Segments[previousSelectedIndex];
            eventArgs.OldPointInfo = this.Segments[previousSelectedIndex].Item;
          }
          (this.ActualArea as SfChart3D).OnSelectionChanged(eventArgs);
          this.PreviousSelectedIndex = newItem;
          break;
        }
        if (this.Segments.Count != 0)
          break;
        this.triggerSelectionChangedEventOnLoad = true;
        break;
      case NotifyCollectionChangedAction.Remove:
        if (e.OldItems == null || (this.ActualArea as SfChart3D).SelectionStyle == SelectionStyle3D.Single)
          break;
        int oldItem = (int) e.OldItems[0];
        ChartSelectionChangedEventArgs eventArgs1 = new ChartSelectionChangedEventArgs()
        {
          SelectedSegment = (ChartSegment) null,
          SelectedSegments = this.Area.SelectedSegments,
          SelectedSeries = (ChartSeriesBase) null,
          SelectedIndex = oldItem,
          PreviousSelectedIndex = this.PreviousSelectedIndex,
          PreviousSelectedSegment = (ChartSegment) null,
          PreviousSelectedSeries = (ChartSeriesBase) this,
          IsSelected = false
        };
        if (this.PreviousSelectedIndex != -1 && this.PreviousSelectedIndex < this.Segments.Count)
        {
          eventArgs1.PreviousSelectedSegment = this.Segments[this.PreviousSelectedIndex];
          eventArgs1.OldPointInfo = this.Segments[this.PreviousSelectedIndex].Item;
        }
        (this.ActualArea as SfChart3D).OnSelectionChanged(eventArgs1);
        this.PreviousSelectedIndex = oldItem;
        break;
    }
  }

  internal override void OnResetSegment(int index)
  {
    if (index >= this.Segments.Count || index < 0)
      return;
    ChartSegment3D segment = this.Segments[index] as ChartSegment3D;
    segment.BindProperties();
    foreach (Polygon3D polygon in segment.Polygons)
    {
      polygon.Fill = segment.Interior;
      polygon.ReDraw();
    }
  }

  internal override bool RaiseSelectionChanging(int newIndex, int oldIndex)
  {
    this.selectionChangingEventArgs.SelectedSegments = (List<ChartSegment>) null;
    if (this is AreaSeries3D || this is LineSeries3D)
      this.selectionChangingEventArgs.SelectedSegment = (ChartSegment) this.GetDataPoint(newIndex);
    else if (newIndex >= 0 && newIndex < this.Segments.Count)
      this.selectionChangingEventArgs.SelectedSegment = this.Segments[newIndex];
    else
      this.selectionChangingEventArgs.SelectedSegment = (ChartSegment) null;
    this.SetSelectionChangingEventArgs(newIndex, oldIndex);
    this.ActualArea.OnSelectionChanging(this.selectionChangingEventArgs);
    return this.selectionChangingEventArgs.Cancel;
  }

  internal void OnMouseMoveSelection(FrameworkElement element, Point pos)
  {
    if (element == null || !(element.Tag is ChartSegment3D))
      return;
    ChartSegment3D tag = element.Tag as ChartSegment3D;
    int newIndex1 = -1;
    if (this.IsSideBySide || tag.Series is CircularSeriesBase3D)
    {
      newIndex1 = this.Segments.IndexOf((ChartSegment) tag);
    }
    else
    {
      this.dataPoint = this.GetDataPoint(pos);
      if (this.dataPoint != null)
        newIndex1 = this.dataPoint.Index;
    }
    if (this.Area.EnableSeriesSelection && (tag.Series.IsSideBySide || tag.Series is ScatterSeries3D || tag.Series is CircularSeriesBase3D || !tag.Series.IsSideBySide && (!this.Area.EnableSegmentSelection || this.Area.EnableSegmentSelection && newIndex1 == -1)))
    {
      int newIndex2 = this.Area.Series.IndexOf(tag.Series as ChartSeries3D);
      if (newIndex2 <= -1 || this.RaiseSelectionChanging(newIndex2, this.Area.SeriesSelectedIndex))
        return;
      this.Area.SeriesSelectedIndex = newIndex2;
      this.Area.PreviousSelectedSeries = (ChartSeriesBase) this;
    }
    else
    {
      if (!this.Area.EnableSegmentSelection || newIndex1 <= -1 || this.RaiseSelectionChanging(newIndex1, this.SelectedIndex))
        return;
      if (this.Area.SelectionStyle == SelectionStyle3D.Single)
        this.SelectedIndex = newIndex1;
      else
        this.SelectedSegmentsIndexes.Add(newIndex1);
      this.Area.PreviousSelectedSeries = (ChartSeriesBase) this;
    }
  }

  internal void OnMouseDownSelection(int currentIndex)
  {
    if (this.Area.EnableSeriesSelection && (this.IsSideBySide || this is ScatterSeries3D || this is CircularSeriesBase3D || !this.IsSideBySide && (!this.Area.EnableSegmentSelection || this.Area.EnableSegmentSelection && currentIndex == -1)))
    {
      int newIndex = this.Area.Series.IndexOf(this);
      this.selectionChangingEventArgs.IsDataPointSelection = false;
      if (this.RaiseSelectionChanging(newIndex, this.Area.SeriesSelectedIndex))
        return;
      if (this.Area.SelectionStyle != SelectionStyle3D.Single && this.Area.SelectedSeriesCollection.Contains((ChartSeriesBase) this))
      {
        this.Area.SelectedSeriesCollection.Remove((ChartSeriesBase) this);
        if (this.Area.SeriesSelectedIndex == newIndex)
          this.Area.SeriesSelectedIndex = -1;
        SfChart3D.OnResetSeries((ChartSeriesBase) this);
      }
      else if (this.Area.SeriesSelectedIndex == newIndex)
      {
        this.Area.SeriesSelectedIndex = -1;
      }
      else
      {
        if (newIndex <= -1)
          return;
        this.Area.SeriesSelectedIndex = newIndex;
        this.Area.PreviousSelectedSeries = (ChartSeriesBase) this;
      }
    }
    else
    {
      if (!this.Area.EnableSegmentSelection)
        return;
      this.selectionChangingEventArgs.IsDataPointSelection = true;
      if (this.RaiseSelectionChanging(currentIndex, this.SelectedIndex))
        return;
      if (this.Area.SelectionStyle != SelectionStyle3D.Single && this.SelectedSegmentsIndexes.Contains(currentIndex))
      {
        this.SelectedSegmentsIndexes.Remove(currentIndex);
        if (this.SelectedIndex == currentIndex)
          this.SelectedIndex = -1;
        this.OnResetSegment(currentIndex);
      }
      else if (this.SelectedIndex == currentIndex)
      {
        this.SelectedIndex = -1;
      }
      else
      {
        if (currentIndex <= -1)
          return;
        this.SelectedIndex = currentIndex;
        this.Area.PreviousSelectedSeries = (ChartSeriesBase) this;
      }
    }
  }

  internal override void UpdateOnSeriesBoundChanged(Size size)
  {
    if (this.AdornmentsInfo != null)
    {
      this.AdornmentsInfo.UpdateElements();
      this.AdornmentsInfo.Measure(size, (Panel) null);
    }
    if (this is ISupportAxes supportAxes && (supportAxes == null || this.ActualXAxis == null || this.ActualYAxis == null))
      return;
    IChartTransformer transformer = this.CreateTransformer(size, true);
    foreach (ChartSegment segment in (Collection<ChartSegment>) this.Segments)
    {
      segment.CreateSegmentVisual(size);
      segment.Update(transformer);
    }
  }

  internal override void CalculateSegments()
  {
    base.CalculateSegments();
    if (this.DataCount != 0 || this.AdornmentsInfo == null)
      return;
    this.ClearUnUsedAdornments(this.DataCount);
  }

  protected internal override void SelectedIndexChanged(int newIndex, int oldIndex)
  {
    if (this.ActualArea == null)
      return;
    if (this.Area.SelectionStyle == SelectionStyle3D.Single)
    {
      if (this.SelectedSegmentsIndexes.Contains(oldIndex))
        this.SelectedSegmentsIndexes.Remove(oldIndex);
      this.OnResetSegment(oldIndex);
    }
    if (newIndex >= 0 && (this.ActualArea as SfChart3D).EnableSegmentSelection)
    {
      if (!this.SelectedSegmentsIndexes.Contains(newIndex))
        this.SelectedSegmentsIndexes.Add(newIndex);
      if (newIndex < this.Segments.Count && this.SegmentSelectionBrush != null)
      {
        ChartSegment3D segment = this.Segments[newIndex] as ChartSegment3D;
        segment.BindProperties();
        foreach (Polygon3D polygon in segment.Polygons)
        {
          polygon.Fill = this.SegmentSelectionBrush;
          polygon.ReDraw();
        }
      }
      if (newIndex < this.Segments.Count)
      {
        ChartSelectionChangedEventArgs eventArgs = new ChartSelectionChangedEventArgs()
        {
          SelectedSegment = this.SelectedSegment,
          SelectedSegments = this.Area.SelectedSegments,
          SelectedSeries = (ChartSeriesBase) this,
          SelectedIndex = newIndex,
          PreviousSelectedIndex = oldIndex,
          NewPointInfo = this.Segments[newIndex].Item,
          PreviousSelectedSegment = (ChartSegment) null,
          PreviousSelectedSeries = (ChartSeriesBase) null,
          IsSelected = true
        };
        eventArgs.PreviousSelectedSeries = this.Area.PreviousSelectedSeries;
        if (oldIndex >= 0 && oldIndex < this.Segments.Count)
        {
          eventArgs.PreviousSelectedSegment = this.Segments[oldIndex];
          eventArgs.OldPointInfo = this.Segments[oldIndex].Item;
        }
        (this.ActualArea as SfChart3D).OnSelectionChanged(eventArgs);
        this.PreviousSelectedIndex = newIndex;
      }
      else
      {
        if (this.Segments.Count != 0)
          return;
        this.triggerSelectionChangedEventOnLoad = true;
      }
    }
    else
    {
      if (newIndex != -1 || this.ActualArea == null)
        return;
      ChartSelectionChangedEventArgs eventArgs = new ChartSelectionChangedEventArgs()
      {
        SelectedSegment = (ChartSegment) null,
        SelectedSegments = this.Area.SelectedSegments,
        SelectedSeries = (ChartSeriesBase) null,
        SelectedIndex = newIndex,
        PreviousSelectedIndex = oldIndex,
        PreviousSelectedSegment = (ChartSegment) null,
        PreviousSelectedSeries = (ChartSeriesBase) this,
        IsSelected = false
      };
      if (oldIndex != -1 && oldIndex < this.Segments.Count)
      {
        eventArgs.PreviousSelectedSegment = this.Segments[oldIndex];
        eventArgs.OldPointInfo = this.Segments[oldIndex].Item;
      }
      (this.ActualArea as SfChart3D).OnSelectionChanged(eventArgs);
      this.PreviousSelectedIndex = newIndex;
    }
  }

  protected internal override void OnSeriesMouseDown(object source, Point position)
  {
  }

  protected internal override void OnSeriesMouseUp(object source, Point position)
  {
    FrameworkElement frameworkElement = source as FrameworkElement;
    if (this.SelectionMode != SelectionMode.MouseClick || frameworkElement == null || !(frameworkElement.Tag is ChartSegment3D))
      return;
    ChartSegment3D tag = frameworkElement.Tag as ChartSegment3D;
    int currentIndex = -1;
    if (this.IsSideBySide || tag.Series is CircularSeriesBase3D)
    {
      currentIndex = this.Segments.IndexOf((ChartSegment) tag);
    }
    else
    {
      this.dataPoint = this.GetDataPoint(position);
      if (this.dataPoint != null)
        currentIndex = this.dataPoint.Index;
    }
    this.OnMouseDownSelection(currentIndex);
  }

  protected internal virtual void OnSeriesMouseMove(object source, Point pos)
  {
    if (this.SelectionMode == SelectionMode.MouseMove && this.Area.SelectionStyle == SelectionStyle3D.Single)
      this.OnMouseMoveSelection(source as FrameworkElement, pos);
    this.mousePos = pos;
    this.UpdateTooltip(source);
  }

  protected internal virtual void OnSeriesMouseLeave(object source, Point pos)
  {
    this.RemoveTooltip();
    this.Timer.Stop();
    if (ChartTooltip.GetInitialShowDelay((DependencyObject) this) <= 0)
      return;
    this.InitialDelayTimer.Stop();
  }

  public override void OnApplyTemplate() => base.OnApplyTemplate();

  protected virtual ChartAdornment CreateAdornment(
    ChartSeriesBase series,
    double xVal,
    double yVal,
    double xPos,
    double yPos,
    double startDepth)
  {
    return (ChartAdornment) new ChartAdornment3D(xVal, yVal, xPos, yPos, startDepth, series);
  }

  protected virtual void AddAdornments(double xValue, double yValue, int index, double depth)
  {
    double num1 = xValue;
    double num2 = yValue;
    if (index < this.Adornments.Count)
      this.Adornments[index].SetData(num1, num2, num1, num2, depth);
    else
      this.Adornments.Add(this.CreateAdornment((ChartSeriesBase) this, num1, num2, num1, num2, depth));
    this.Adornments[index].Item = this.ActualData[index];
  }

  protected virtual void AddColumnAdornments(params double[] values)
  {
    double xPos = values[2] + values[5];
    double yPos = values[3];
    int index = (int) values[4];
    if (index < this.Adornments.Count)
      this.Adornments[index].SetData(values[0], values[1], xPos, yPos, values[6]);
    else
      this.Adornments.Add(this.CreateAdornment((ChartSeriesBase) this, values[0], values[1], xPos, yPos, values[6]));
    if (this.ActualXAxis is CategoryAxis3D && !(this.ActualXAxis as CategoryAxis3D).IsIndexed)
      this.Adornments[index].Item = this.GroupedActualData[index];
    else
      this.Adornments[index].Item = this.ActualData[index];
  }

  protected virtual void AddAdornmentAtXY(double x, double y, int pointindex, double startDepth)
  {
    double xPos = x;
    double yPos = y;
    if (pointindex < this.Adornments.Count)
      this.Adornments[pointindex].SetData(x, y, xPos, yPos);
    else
      this.Adornments.Add(this.CreateAdornment((ChartSeriesBase) this, x, y, xPos, yPos, startDepth));
  }

  protected override void OnDataSourceChanged(IEnumerable oldValue, IEnumerable newValue)
  {
    if (this.AdornmentsInfo != null)
    {
      this.Adornments.Clear();
      this.VisibleAdornments.Clear();
      this.AdornmentsInfo.UpdateElements();
    }
    base.OnDataSourceChanged(oldValue, newValue);
  }

  protected void ClearUnUsedAdornments(int startIndex)
  {
    if (this.Adornments.Count <= startIndex)
      return;
    int count = this.Adornments.Count;
    for (int index = startIndex; index < count; ++index)
      this.Adornments.RemoveAt(startIndex);
  }

  protected override DependencyObject CloneSeries(DependencyObject obj)
  {
    ChartSeries3D chartSeries3D = (ChartSeries3D) obj;
    if (this.AdornmentsInfo != null)
      chartSeries3D.AdornmentsInfo = (ChartAdornmentInfo3D) this.AdornmentsInfo.Clone();
    chartSeries3D.SegmentSelectionBrush = this.SegmentSelectionBrush;
    chartSeries3D.SelectedIndex = this.SelectedIndex;
    return base.CloneSeries(obj);
  }

  private static void OnSegmentSelectionBrush(
    DependencyObject d,
    DependencyPropertyChangedEventArgs args)
  {
    ((ChartSeriesBase) d).UpdateArea();
  }

  private static void OnAdornmentsInfoChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    ChartSeries3D chartSeries3D = d as ChartSeries3D;
    if (e.OldValue != null)
    {
      ChartAdornmentInfoBase oldValue = e.OldValue as ChartAdornmentInfoBase;
      if (chartSeries3D != null)
      {
        chartSeries3D.Adornments.Clear();
        chartSeries3D.VisibleAdornments.Clear();
      }
      if (oldValue != null)
      {
        oldValue.ClearChildren();
        oldValue.Series = (ChartSeriesBase) null;
      }
    }
    if (e.NewValue == null || chartSeries3D == null)
      return;
    chartSeries3D.adornmentInfo = e.NewValue as ChartAdornmentInfoBase;
    if (chartSeries3D.AdornmentsInfo == null)
      return;
    chartSeries3D.AdornmentsInfo.Series = (ChartSeriesBase) chartSeries3D;
    chartSeries3D.AdornmentsInfo.PanelChanged((Panel) null);
    if (chartSeries3D.Area == null)
      return;
    chartSeries3D.Area.ScheduleUpdate();
  }

  private static void OnSelectedIndexChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    ChartSeries3D chartSeries3D = d as ChartSeries3D;
    if (chartSeries3D.ActualArea == null)
      return;
    if ((chartSeries3D.ActualArea as SfChart3D).SelectionStyle == SelectionStyle3D.Single)
    {
      chartSeries3D.SelectedIndexChanged((int) e.NewValue, (int) e.OldValue);
    }
    else
    {
      if ((int) e.NewValue == -1)
        return;
      chartSeries3D.SelectedSegmentsIndexes.Add((int) e.NewValue);
    }
  }
}
