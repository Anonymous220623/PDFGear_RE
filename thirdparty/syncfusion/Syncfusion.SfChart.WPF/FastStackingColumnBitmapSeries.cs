// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.FastStackingColumnBitmapSeries
// Assembly: Syncfusion.SfChart.WPF, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 279D70C6-7936-4F3E-B6CF-79EFEF734826
// Assembly location: C:\Program Files\PDFgear\Syncfusion.SfChart.WPF.dll

using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

#nullable disable
namespace Syncfusion.UI.Xaml.Charts;

public class FastStackingColumnBitmapSeries : StackingSeriesBase, ISegmentSpacing, ISegmentSelectable
{
  public static readonly DependencyProperty SegmentSpacingProperty = DependencyProperty.Register(nameof (SegmentSpacing), typeof (double), typeof (FastStackingColumnBitmapSeries), new PropertyMetadata((object) 0.0, new PropertyChangedCallback(FastStackingColumnBitmapSeries.OnSegmentSpacingChanged)));
  public static readonly DependencyProperty SegmentSelectionBrushProperty = DependencyProperty.Register(nameof (SegmentSelectionBrush), typeof (Brush), typeof (FastStackingColumnBitmapSeries), new PropertyMetadata((object) null, new PropertyChangedCallback(FastStackingColumnBitmapSeries.OnSegmentSelectionBrush)));
  public static readonly DependencyProperty SelectedIndexProperty = DependencyProperty.Register(nameof (SelectedIndex), typeof (int), typeof (FastStackingColumnBitmapSeries), new PropertyMetadata((object) -1, new PropertyChangedCallback(FastStackingColumnBitmapSeries.OnSelectedIndexChanged)));
  private List<double> xValues;

  public double SegmentSpacing
  {
    get => (double) this.GetValue(FastStackingColumnBitmapSeries.SegmentSpacingProperty);
    set => this.SetValue(FastStackingColumnBitmapSeries.SegmentSpacingProperty, (object) value);
  }

  public Brush SegmentSelectionBrush
  {
    get => (Brush) this.GetValue(FastStackingColumnBitmapSeries.SegmentSelectionBrushProperty);
    set
    {
      this.SetValue(FastStackingColumnBitmapSeries.SegmentSelectionBrushProperty, (object) value);
    }
  }

  public int SelectedIndex
  {
    get => (int) this.GetValue(FastStackingColumnBitmapSeries.SelectedIndexProperty);
    set => this.SetValue(FastStackingColumnBitmapSeries.SelectedIndexProperty, (object) value);
  }

  protected internal override bool IsSideBySide => true;

  protected internal override bool IsBitmapSeries => true;

  protected internal override List<ChartSegment> SelectedSegments
  {
    get
    {
      if (this.SelectedSegmentsIndexes.Count <= 0)
        return (List<ChartSegment>) null;
      this.selectedSegments.Clear();
      foreach (int selectedSegmentsIndex in (Collection<int>) this.SelectedSegmentsIndexes)
        this.selectedSegments.Add((ChartSegment) this.GetDataPoint(selectedSegmentsIndex));
      return this.selectedSegments;
    }
  }

  protected override bool IsStacked => true;

  private ChartSegment Segment { get; set; }

  double ISegmentSpacing.CalculateSegmentSpacing(double spacing, double Right, double Left)
  {
    return this.CalculateSegmentSpacing(spacing, Right, Left);
  }

  public override void CreateSegments()
  {
    bool flag = this.ActualXAxis is CategoryAxis && !(this.ActualXAxis as CategoryAxis).IsIndexed;
    this.xValues = !flag ? (this.ActualXValues is List<double> ? this.ActualXValues as List<double> : this.GetXValues()) : this.GroupedXValuesIndexes;
    IList<double> x1Values = (IList<double>) new List<double>();
    IList<double> x2Values = (IList<double>) new List<double>();
    IList<double> y1Values = (IList<double>) new List<double>();
    IList<double> y2Values = (IList<double>) new List<double>();
    StackingValues cumulativeStackValues = this.GetCumulativeStackValues((ChartSeriesBase) this);
    if (cumulativeStackValues == null)
      return;
    this.YRangeStartValues = cumulativeStackValues.StartValues;
    this.YRangeEndValues = cumulativeStackValues.EndValues;
    if (this.xValues == null)
      return;
    DoubleRange sideBySideInfo = this.GetSideBySideInfo((ChartSeriesBase) this);
    if (flag)
    {
      this.Segments.Clear();
      this.Adornments.Clear();
      int index1 = 0;
      for (int key = 0; key < this.DistinctValuesIndexes.Count; ++key)
      {
        for (int index2 = 0; index2 < this.DistinctValuesIndexes[(double) key].Count; ++index2)
        {
          if (index2 < this.xValues.Count)
          {
            x1Values.Add((double) key + sideBySideInfo.Start);
            x2Values.Add((double) key + sideBySideInfo.End);
            y1Values.Add(this.YRangeEndValues[index1]);
            y2Values.Add(this.YRangeStartValues[index1]);
            ++index1;
          }
        }
      }
      if (this.Segment != null && this.IsActualTransposed && this.Segment is FastStackingColumnSegment || !this.IsActualTransposed && this.Segment is FastBarBitmapSegment)
        this.Segments.Clear();
      if (this.Segment == null || this.Segments.Count == 0)
      {
        if (this.IsActualTransposed)
        {
          this.Segment = (ChartSegment) (this.CreateSegment() as FastBarBitmapSegment);
          if (this.Segment != null)
            this.Segment.Item = !(this.ActualXAxis is CategoryAxis) || (this.ActualXAxis as CategoryAxis).IsIndexed ? (object) this.ActualData : (object) this.GroupedActualData;
        }
        else
        {
          this.Segment = (ChartSegment) (this.CreateSegment() as FastStackingColumnSegment);
          if (this.Segment != null)
            this.Segment.Item = (object) this.ActualData;
        }
        if (this.Segment != null)
        {
          this.Segment.Series = (ChartSeriesBase) this;
          this.Segment.SetData(x1Values, y1Values, x2Values, y2Values);
          this.Segments.Add(this.Segment);
        }
      }
      if (this.AdornmentsInfo == null)
        return;
      int index3 = 0;
      AdornmentsPosition adornmentPosition = this.adornmentInfo.GetAdornmentPosition();
      for (int key = 0; key < this.DistinctValuesIndexes.Count; ++key)
      {
        for (int index4 = 0; index4 < this.DistinctValuesIndexes[(double) key].Count; ++index4)
        {
          int index5 = this.DistinctValuesIndexes[(double) key][index4];
          switch (adornmentPosition)
          {
            case AdornmentsPosition.Top:
              this.AddColumnAdornments((double) key, this.GroupedSeriesYValues[0][index5], x1Values[index3], y1Values[index3], (double) index3, sideBySideInfo.Delta / 2.0);
              break;
            case AdornmentsPosition.Bottom:
              this.AddColumnAdornments((double) key, this.GroupedSeriesYValues[0][index5], x1Values[index3], y2Values[index3], (double) index3, sideBySideInfo.Delta / 2.0);
              break;
            default:
              this.AddColumnAdornments((double) key, this.GroupedSeriesYValues[0][index5], x1Values[index3], y1Values[index3] + (y2Values[index3] - y1Values[index3]) / 2.0, (double) index3, sideBySideInfo.Delta / 2.0);
              break;
          }
          ++index3;
        }
      }
    }
    else
    {
      this.ClearUnUsedAdornments(this.DataCount);
      if (!this.IsIndexed)
      {
        for (int index = 0; index < this.DataCount; ++index)
        {
          x1Values.Add(this.xValues[index] + sideBySideInfo.Start);
          x2Values.Add(this.xValues[index] + sideBySideInfo.End);
          y1Values.Add(this.YRangeEndValues[index]);
          y2Values.Add(this.YRangeStartValues[index]);
        }
      }
      else
      {
        for (int index = 0; index < this.DataCount; ++index)
        {
          x1Values.Add((double) index + sideBySideInfo.Start);
          x2Values.Add((double) index + sideBySideInfo.End);
          y1Values.Add(this.YRangeEndValues[index]);
          y2Values.Add(this.YRangeStartValues[index]);
        }
      }
      if (this.Segment != null && this.IsActualTransposed && this.Segment is FastStackingColumnSegment || !this.IsActualTransposed && this.Segment is FastBarBitmapSegment)
        this.Segments.Clear();
      if (this.Segment == null || this.Segments.Count == 0)
      {
        if (this.IsActualTransposed)
        {
          this.Segment = (ChartSegment) (this.CreateSegment() as FastBarBitmapSegment);
          this.Segment.Series = (ChartSeriesBase) this;
          this.Segment.Item = !(this.ActualXAxis is CategoryAxis) || (this.ActualXAxis as CategoryAxis).IsIndexed ? (object) this.ActualData : (object) this.GroupedActualData;
        }
        else
        {
          this.Segment = (ChartSegment) (this.CreateSegment() as FastStackingColumnSegment);
          this.Segment.Series = (ChartSeriesBase) this;
          this.Segment.Item = (object) this.ActualData;
        }
        this.Segment.SetData(x1Values, y1Values, x2Values, y2Values);
        this.Segments.Add(this.Segment);
      }
      else if (this.xValues != null)
      {
        if (this.Segment is FastBarBitmapSegment)
          (this.Segment as FastBarBitmapSegment).SetData(x1Values, y1Values, x2Values, y2Values);
        else
          (this.Segment as FastStackingColumnSegment).SetData(x1Values, y1Values, x2Values, y2Values);
      }
      if (this.AdornmentsInfo == null)
        return;
      AdornmentsPosition adornmentPosition = this.adornmentInfo.GetAdornmentPosition();
      for (int index = 0; index < this.DataCount; ++index)
      {
        if (index < this.DataCount)
        {
          switch (adornmentPosition)
          {
            case AdornmentsPosition.Top:
              this.AddColumnAdornments(this.xValues[index], this.YValues[index], x1Values[index], y1Values[index], (double) index, sideBySideInfo.Delta / 2.0);
              continue;
            case AdornmentsPosition.Bottom:
              this.AddColumnAdornments(this.xValues[index], this.YValues[index], x1Values[index], y2Values[index], (double) index, sideBySideInfo.Delta / 2.0);
              continue;
            default:
              this.AddColumnAdornments(this.xValues[index], this.YValues[index], x1Values[index], y1Values[index] + (y2Values[index] - y1Values[index]) / 2.0, (double) index, sideBySideInfo.Delta / 2.0);
              continue;
          }
        }
      }
    }
  }

  internal override bool IsHitTestSeries()
  {
    Point point = new Point(this.Area.adorningCanvasPoint.X - this.ActualArea.SeriesClipRect.Left, this.Area.adorningCanvasPoint.Y - this.ActualArea.SeriesClipRect.Top);
    foreach (Rect bitmapRect in this.bitmapRects)
    {
      if (bitmapRect.Contains(point))
        return true;
    }
    return false;
  }

  internal override void SelectedSegmentsIndexes_CollectionChanged(
    object sender,
    NotifyCollectionChangedEventArgs e)
  {
    switch (e.Action)
    {
      case NotifyCollectionChangedAction.Add:
        if (e.NewItems == null || this.ActualArea.SelectionBehaviour.SelectionStyle == SelectionStyle.Single)
          break;
        int previousSelectedIndex = this.PreviousSelectedIndex;
        int newItem = (int) e.NewItems[0];
        if (newItem < 0 || !this.ActualArea.GetEnableSegmentSelection())
          break;
        this.dataPoint = this.GetDataPoint(newItem);
        if (this.dataPoint != null && this.SegmentSelectionBrush != null)
        {
          if (this.adornmentInfo != null && this.adornmentInfo.HighlightOnSelection)
            this.UpdateAdornmentSelection(newItem);
          if (this.Segments.Count > 0)
            this.GeneratePixels();
          this.OnBitmapSelection(this.selectedSegmentPixels, this.SegmentSelectionBrush, true);
        }
        if (this.ActualArea != null && this.Segments.Count > 0)
        {
          ChartSelectionChangedEventArgs eventArgs = new ChartSelectionChangedEventArgs()
          {
            SelectedSegment = this.Segments[0],
            SelectedSegments = this.Area.SelectedSegments,
            SelectedSeries = (ChartSeriesBase) this,
            SelectedIndex = newItem,
            PreviousSelectedIndex = previousSelectedIndex,
            NewPointInfo = (object) this.GetDataPoint(newItem),
            IsSelected = true
          };
          eventArgs.PreviousSelectedSeries = this.ActualArea.PreviousSelectedSeries;
          if (previousSelectedIndex != -1)
          {
            eventArgs.PreviousSelectedSegment = this.Segments[0];
            eventArgs.OldPointInfo = (object) this.GetDataPoint(previousSelectedIndex);
          }
          (this.ActualArea as SfChart).OnSelectionChanged(eventArgs);
          this.PreviousSelectedIndex = newItem;
          break;
        }
        if (this.Segments.Count != 0)
          break;
        this.triggerSelectionChangedEventOnLoad = true;
        break;
      case NotifyCollectionChangedAction.Remove:
        if (e.OldItems == null || this.ActualArea.SelectionBehaviour.SelectionStyle == SelectionStyle.Single)
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
        if (this.PreviousSelectedIndex != -1)
        {
          eventArgs1.PreviousSelectedSegment = this.Segments[0];
          eventArgs1.OldPointInfo = (object) this.GetDataPoint(this.PreviousSelectedIndex);
        }
        (this.ActualArea as SfChart).OnSelectionChanged(eventArgs1);
        this.OnResetSegment(oldItem);
        this.PreviousSelectedIndex = oldItem;
        break;
    }
  }

  internal override void OnResetSegment(int index)
  {
    if (index < 0)
      return;
    this.dataPoint = this.GetDataPoint(index);
    if (this.dataPoint == null)
      return;
    if (this.adornmentInfo != null)
      this.AdornmentPresenter.ResetAdornmentSelection(new int?(index), false);
    if (this.SegmentSelectionBrush == null)
      return;
    if (this.Segments.Count > 0)
      this.GeneratePixels();
    this.OnBitmapSelection(this.selectedSegmentPixels, (Brush) null, false);
    this.selectedSegmentPixels.Clear();
    this.dataPoint = (ChartDataPointInfo) null;
  }

  internal override Point GetDataPointPosition(ChartTooltip tooltip)
  {
    ChartDataPointInfo toolTipTag = this.ToolTipTag as ChartDataPointInfo;
    Point dataPointPosition = new Point();
    Point visible = this.ChartTransformer.TransformToVisible(toolTipTag.XData, this.YRangeEndValues[toolTipTag.Index]);
    dataPointPosition.X = visible.X + this.ActualArea.SeriesClipRect.Left;
    dataPointPosition.Y = visible.Y + this.ActualArea.SeriesClipRect.Top;
    return dataPointPosition;
  }

  protected internal override void SelectedIndexChanged(int newIndex, int oldIndex)
  {
    if (this.ActualArea != null && this.ActualArea.SelectionBehaviour != null && !this.ActualArea.GetEnableSeriesSelection())
    {
      if (this.ActualArea.SelectionBehaviour.SelectionStyle == SelectionStyle.Single)
      {
        if (this.SelectedSegmentsIndexes.Contains(oldIndex))
          this.SelectedSegmentsIndexes.Remove(oldIndex);
        this.OnResetSegment(oldIndex);
      }
      if (this.IsItemsSourceChanged)
        return;
      if (newIndex >= 0 && this.ActualArea.GetEnableSegmentSelection())
      {
        if (!this.SelectedSegmentsIndexes.Contains(newIndex))
          this.SelectedSegmentsIndexes.Add(newIndex);
        this.dataPoint = this.GetDataPoint(newIndex);
        if (this.dataPoint != null && this.SegmentSelectionBrush != null)
        {
          if (this.adornmentInfo != null && this.adornmentInfo.HighlightOnSelection)
            this.UpdateAdornmentSelection(newIndex);
          if (this.Segments.Count > 0)
            this.GeneratePixels();
          this.OnBitmapSelection(this.selectedSegmentPixels, this.SegmentSelectionBrush, true);
        }
        if (this.ActualArea != null && this.Segments.Count > 0)
        {
          ChartSelectionChangedEventArgs eventArgs = new ChartSelectionChangedEventArgs()
          {
            SelectedSegment = this.Segments[0],
            SelectedSegments = this.Area.SelectedSegments,
            SelectedSeries = (ChartSeriesBase) this,
            SelectedIndex = newIndex,
            PreviousSelectedIndex = oldIndex,
            NewPointInfo = (object) this.GetDataPoint(newIndex),
            IsSelected = true
          };
          eventArgs.PreviousSelectedSeries = this.ActualArea.PreviousSelectedSeries;
          if (oldIndex != -1)
          {
            eventArgs.PreviousSelectedSegment = this.Segments[0];
            eventArgs.OldPointInfo = (object) this.GetDataPoint(oldIndex);
          }
          (this.ActualArea as SfChart).OnSelectionChanged(eventArgs);
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
        if (oldIndex != -1)
        {
          eventArgs.PreviousSelectedSegment = this.Segments[0];
          eventArgs.OldPointInfo = (object) this.GetDataPoint(oldIndex);
        }
        (this.ActualArea as SfChart).OnSelectionChanged(eventArgs);
        this.PreviousSelectedIndex = newIndex;
      }
    }
    else
    {
      if (newIndex < 0 || this.Segments.Count != 0)
        return;
      this.triggerSelectionChangedEventOnLoad = true;
    }
  }

  protected override ChartSegment CreateSegment()
  {
    return this.IsActualTransposed ? (ChartSegment) new FastBarBitmapSegment() : (ChartSegment) new FastStackingColumnSegment();
  }

  protected override void OnMouseMove(MouseEventArgs e)
  {
    if (!this.ShowTooltip)
      return;
    Canvas adorningCanvas = this.ActualArea.GetAdorningCanvas();
    this.mousePos = e.GetPosition((IInputElement) adorningCanvas);
    ChartDataPointInfo customTag = new ChartDataPointInfo();
    int adornmentIndex = ChartExtensionUtils.GetAdornmentIndex(e.OriginalSource);
    if (adornmentIndex <= -1)
      return;
    if (this.ActualXAxis is CategoryAxis && !(this.ActualXAxis as CategoryAxis).IsIndexed)
    {
      if (this.xValues.Count > adornmentIndex)
        customTag.XData = this.GroupedXValuesIndexes[adornmentIndex];
      if (this.GroupedSeriesYValues[0].Count > adornmentIndex)
        customTag.YData = this.GroupedSeriesYValues[0][adornmentIndex];
      customTag.Index = adornmentIndex;
      customTag.Series = (ChartSeriesBase) this;
      if (this.ActualData.Count > adornmentIndex)
        customTag.Item = this.GroupedActualData[adornmentIndex];
    }
    else
    {
      if (this.xValues.Count > adornmentIndex)
        customTag.XData = this.xValues[adornmentIndex];
      if (this.YValues.Count > adornmentIndex)
        customTag.YData = this.YValues[adornmentIndex];
      customTag.Index = adornmentIndex;
      customTag.Series = (ChartSeriesBase) this;
      if (this.ActualData.Count > adornmentIndex)
        customTag.Item = this.ActualData[adornmentIndex];
    }
    this.UpdateSeriesTooltip((object) customTag);
  }

  protected override void OnDataSourceChanged(IEnumerable oldValue, IEnumerable newValue)
  {
    this.Segment = (ChartSegment) null;
    base.OnDataSourceChanged(oldValue, newValue);
  }

  protected override DependencyObject CloneSeries(DependencyObject obj)
  {
    return base.CloneSeries((DependencyObject) new FastStackingColumnBitmapSeries()
    {
      SegmentSelectionBrush = this.SegmentSelectionBrush,
      SelectedIndex = this.SelectedIndex,
      SegmentSpacing = this.SegmentSpacing
    });
  }

  protected double CalculateSegmentSpacing(double spacing, double Right, double Left)
  {
    double num = (Right - Left) * spacing / 2.0;
    Left += num;
    Right -= num;
    return Left;
  }

  private static void OnSegmentSpacingChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    FastStackingColumnBitmapSeries columnBitmapSeries = d as FastStackingColumnBitmapSeries;
    if (columnBitmapSeries.Area == null)
      return;
    columnBitmapSeries.Area.ScheduleUpdate();
  }

  private static void OnSegmentSelectionBrush(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    (d as FastStackingColumnBitmapSeries).UpdateArea();
  }

  private static void OnSelectedIndexChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    ChartSeries chartSeries = d as ChartSeries;
    if (chartSeries.ActualArea == null || chartSeries.ActualArea.SelectionBehaviour == null)
      return;
    if (chartSeries.ActualArea.SelectionBehaviour.SelectionStyle == SelectionStyle.Single)
    {
      chartSeries.SelectedIndexChanged((int) e.NewValue, (int) e.OldValue);
    }
    else
    {
      if ((int) e.NewValue == -1)
        return;
      chartSeries.SelectedSegmentsIndexes.Add((int) e.NewValue);
    }
  }
}
