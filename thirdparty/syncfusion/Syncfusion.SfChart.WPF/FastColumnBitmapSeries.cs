// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.FastColumnBitmapSeries
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
using System.Windows.Input;
using System.Windows.Media;

#nullable disable
namespace Syncfusion.UI.Xaml.Charts;

public class FastColumnBitmapSeries : XyDataSeries, ISegmentSpacing, ISegmentSelectable
{
  public static readonly DependencyProperty SegmentSelectionBrushProperty = DependencyProperty.Register(nameof (SegmentSelectionBrush), typeof (Brush), typeof (FastColumnBitmapSeries), new PropertyMetadata((object) null, new PropertyChangedCallback(FastColumnBitmapSeries.OnSegmentSelectionBrush)));
  public static readonly DependencyProperty SelectedIndexProperty = DependencyProperty.Register(nameof (SelectedIndex), typeof (int), typeof (FastColumnBitmapSeries), new PropertyMetadata((object) -1, new PropertyChangedCallback(FastColumnBitmapSeries.OnSelectedIndexChanged)));
  public static readonly DependencyProperty SegmentSpacingProperty = DependencyProperty.Register(nameof (SegmentSpacing), typeof (double), typeof (FastColumnBitmapSeries), new PropertyMetadata((object) 0.0, new PropertyChangedCallback(FastColumnBitmapSeries.OnSegmentSpacingChanged)));
  private List<double> xValues;

  public Brush SegmentSelectionBrush
  {
    get => (Brush) this.GetValue(FastColumnBitmapSeries.SegmentSelectionBrushProperty);
    set => this.SetValue(FastColumnBitmapSeries.SegmentSelectionBrushProperty, (object) value);
  }

  public int SelectedIndex
  {
    get => (int) this.GetValue(FastColumnBitmapSeries.SelectedIndexProperty);
    set => this.SetValue(FastColumnBitmapSeries.SelectedIndexProperty, (object) value);
  }

  public double SegmentSpacing
  {
    get => (double) this.GetValue(FastColumnBitmapSeries.SegmentSpacingProperty);
    set => this.SetValue(FastColumnBitmapSeries.SegmentSpacingProperty, (object) value);
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

  private ChartSegment Segment { get; set; }

  double ISegmentSpacing.CalculateSegmentSpacing(double spacing, double Right, double Left)
  {
    return this.CalculateSegmentSpacing(spacing, Right, Left);
  }

  public override void CreateSegments()
  {
    bool flag = this.ActualXAxis is CategoryAxis && !(this.ActualXAxis as CategoryAxis).IsIndexed;
    this.xValues = !flag ? this.GetXValues() : this.GroupedXValuesIndexes;
    IList<double> x1Values = (IList<double>) new List<double>();
    IList<double> x2Values = (IList<double>) new List<double>();
    IList<double> y1Values = (IList<double>) new List<double>();
    IList<double> y2Values = (IList<double>) new List<double>();
    x1Values.Clear();
    x2Values.Clear();
    y1Values.Clear();
    y2Values.Clear();
    if (this.xValues == null)
      return;
    this.ClearUnUsedAdornments(this.DataCount);
    DoubleRange sideBySideInfo = this.GetSideBySideInfo((ChartSeriesBase) this);
    double num1 = this.ActualXAxis != null ? this.ActualXAxis.Origin : 0.0;
    if (this.ActualXAxis != null && this.ActualXAxis.Origin == 0.0 && this.ActualYAxis is LogarithmicAxis && (this.ActualYAxis as LogarithmicAxis).Minimum.HasValue)
      num1 = (this.ActualYAxis as LogarithmicAxis).Minimum.Value;
    if (flag)
    {
      this.Segments.Clear();
      this.Adornments.Clear();
      this.GroupedActualData.Clear();
      for (int key = 0; key < this.DistinctValuesIndexes.Count; ++key)
      {
        List<List<double>> list = this.DistinctValuesIndexes[(double) key].Where<int>((Func<int, bool>) (index => this.GroupedSeriesYValues[0].Count > index)).Select<int, List<double>>((Func<int, List<double>>) (index => new List<double>()
        {
          this.GroupedSeriesYValues[0][index],
          (double) index
        })).OrderByDescending<List<double>, double>((Func<List<double>, double>) (val => val[0])).ToList<List<double>>();
        for (int index = 0; index < list.Count; ++index)
        {
          double num2 = list[index][0];
          this.GroupedActualData.Add(this.ActualData[(int) list[index][1]]);
          if (key < this.xValues.Count)
          {
            x1Values.Add((double) key + sideBySideInfo.Start);
            x2Values.Add((double) key + sideBySideInfo.End);
            y1Values.Add(num2);
            y2Values.Add(num1);
          }
        }
      }
      if (this.Segment != null && this.IsActualTransposed && this.Segment is FastColumnBitmapSegment || !this.IsActualTransposed && this.Segment is FastBarBitmapSegment)
        this.Segments.Clear();
      if (this.Segment == null || this.Segments.Count == 0)
      {
        this.Segment = !this.IsActualTransposed ? (ChartSegment) (this.CreateSegment() as FastColumnBitmapSegment) : (ChartSegment) (this.CreateSegment() as FastBarBitmapSegment);
        if (this.Segment != null)
        {
          this.Segment.Series = (ChartSeriesBase) this;
          this.Segment.Item = !(this.ActualXAxis is CategoryAxis) || (this.ActualXAxis as CategoryAxis).IsIndexed ? (object) this.ActualData : (object) this.GroupedActualData;
          this.Segment.SetData(x1Values, y1Values, x2Values, y2Values);
          this.Segments.Add(this.Segment);
        }
      }
      if (this.AdornmentsInfo == null)
        return;
      int index1 = 0;
      for (int key = 0; key < this.DistinctValuesIndexes.Count; ++key)
      {
        List<List<double>> list = this.DistinctValuesIndexes[(double) key].Select<int, List<double>>((Func<int, List<double>>) (index => new List<double>()
        {
          this.GroupedSeriesYValues[0][index],
          (double) index
        })).OrderByDescending<List<double>, double>((Func<List<double>, double>) (val => val[0])).ToList<List<double>>();
        for (int index2 = 0; index2 < this.DistinctValuesIndexes[(double) key].Count; ++index2)
        {
          double num3 = list[index2][0];
          if (key < this.xValues.Count)
          {
            switch (this.adornmentInfo.GetAdornmentPosition())
            {
              case AdornmentsPosition.Top:
                this.AddColumnAdornments((double) key, num3, x1Values[index1], y1Values[index1], (double) index1, sideBySideInfo.Delta / 2.0);
                break;
              case AdornmentsPosition.Bottom:
                this.AddColumnAdornments((double) key, num3, x1Values[index1], y2Values[index1], (double) index1, sideBySideInfo.Delta / 2.0);
                break;
              default:
                this.AddColumnAdornments((double) key, num3, x1Values[index1], y1Values[index1] + (y2Values[index1] - y1Values[index1]) / 2.0, (double) index1, sideBySideInfo.Delta / 2.0);
                break;
            }
          }
          ++index1;
        }
      }
    }
    else
    {
      if (!this.IsIndexed)
      {
        this.ClearUnUsedAdornments(this.DataCount);
        for (int index = 0; index < this.DataCount; ++index)
        {
          x1Values.Add(this.xValues[index] + sideBySideInfo.Start);
          x2Values.Add(this.xValues[index] + sideBySideInfo.End);
          y1Values.Add(this.YValues[index]);
          y2Values.Add(num1);
        }
      }
      else
      {
        for (int index = 0; index < this.DataCount; ++index)
        {
          x1Values.Add((double) index + sideBySideInfo.Start);
          x2Values.Add((double) index + sideBySideInfo.End);
          y1Values.Add(this.YValues[index]);
          y2Values.Add(num1);
        }
      }
      if (this.Segment != null && this.IsActualTransposed && this.Segment is FastColumnBitmapSegment || !this.IsActualTransposed && this.Segment is FastBarBitmapSegment)
        this.Segments.Clear();
      if (this.Segment == null || this.Segments.Count == 0)
      {
        this.Segment = !this.IsActualTransposed ? (ChartSegment) (this.CreateSegment() as FastColumnBitmapSegment) : (ChartSegment) (this.CreateSegment() as FastBarBitmapSegment);
        if (this.Segment != null)
        {
          this.Segment.Series = (ChartSeriesBase) this;
          this.Segment.Item = !(this.ActualXAxis is CategoryAxis) || (this.ActualXAxis as CategoryAxis).IsIndexed ? (object) this.ActualData : (object) this.GroupedActualData;
          this.Segment.SetData(x1Values, y1Values, x2Values, y2Values);
          this.Segments.Add(this.Segment);
        }
      }
      else if (this.Segment is FastBarBitmapSegment)
      {
        (this.Segment as FastBarBitmapSegment).Item = (object) this.ActualData;
        (this.Segment as FastBarBitmapSegment).SetData(x1Values, y1Values, x2Values, y2Values);
      }
      else
      {
        (this.Segment as FastColumnBitmapSegment).Item = (object) this.ActualData;
        (this.Segment as FastColumnBitmapSegment).SetData(x1Values, y1Values, x2Values, y2Values);
      }
      if (this.AdornmentsInfo == null)
        return;
      for (int index = 0; index < this.DataCount; ++index)
      {
        if (index < this.DataCount)
        {
          switch (this.adornmentInfo.GetAdornmentPosition())
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

  internal override int GetDataPointIndex(Point point)
  {
    Canvas adorningCanvas = this.Area.GetAdorningCanvas();
    double num1 = this.Area.ActualWidth - adorningCanvas.ActualWidth;
    double num2 = this.Area.ActualHeight - adorningCanvas.ActualHeight;
    ChartDataPointInfo chartDataPointInfo = (ChartDataPointInfo) null;
    point.X = point.X - num1 + this.Area.Margin.Left;
    point.Y = point.Y - num2 + this.Area.Margin.Top;
    Point point1 = new Point(point.X - this.Area.SeriesClipRect.Left, point.Y - this.Area.SeriesClipRect.Top);
    double num3 = (double) (this.Area.fastRenderSurface.PixelWidth * (int) point1.Y + (int) point1.X);
    if (!this.Area.isBitmapPixelsConverted)
      this.Area.ConvertBitmapPixels();
    if (this.Pixels.Contains((int) num3))
      chartDataPointInfo = this.GetDataPoint(point);
    return chartDataPointInfo != null ? chartDataPointInfo.Index : -1;
  }

  internal override Point GetDataPointPosition(ChartTooltip tooltip)
  {
    ChartDataPointInfo toolTipTag = this.ToolTipTag as ChartDataPointInfo;
    Point dataPointPosition = new Point();
    Point visible = this.ChartTransformer.TransformToVisible(toolTipTag.XData, toolTipTag.YData);
    dataPointPosition.X = visible.X + this.ActualArea.SeriesClipRect.Left;
    dataPointPosition.Y = visible.Y + this.ActualArea.SeriesClipRect.Top;
    return dataPointPosition;
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
        if (newIndex != -1)
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
    return this.IsActualTransposed ? (ChartSegment) new FastBarBitmapSegment() : (ChartSegment) new FastColumnBitmapSegment();
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
      if (this.GroupedActualData.Count > adornmentIndex)
        customTag.Item = this.GroupedActualData[adornmentIndex];
    }
    else
    {
      if (this.xValues.Count > adornmentIndex)
        customTag.XData = this.xValues[adornmentIndex];
      if (this.YValues.Count > adornmentIndex)
        customTag.YData = this.YValues[adornmentIndex];
      if (this.ActualData.Count > adornmentIndex)
        customTag.Item = this.ActualData[adornmentIndex];
    }
    customTag.Index = adornmentIndex;
    customTag.Series = (ChartSeriesBase) this;
    this.UpdateSeriesTooltip((object) customTag);
  }

  protected override void OnDataSourceChanged(IEnumerable oldValue, IEnumerable newValue)
  {
    this.Segment = (ChartSegment) null;
    base.OnDataSourceChanged(oldValue, newValue);
  }

  protected override DependencyObject CloneSeries(DependencyObject obj)
  {
    return base.CloneSeries((DependencyObject) new FastColumnBitmapSeries()
    {
      SelectedIndex = this.SelectedIndex,
      SegmentSpacing = this.SegmentSpacing,
      SegmentSelectionBrush = this.SegmentSelectionBrush
    });
  }

  protected double CalculateSegmentSpacing(double spacing, double Right, double Left)
  {
    double num = (Right - Left) * spacing / 2.0;
    Left += num;
    Right -= num;
    return Left;
  }

  private static void OnSegmentSelectionBrush(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    (d as FastColumnBitmapSeries).UpdateArea();
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

  private static void OnSegmentSpacingChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    FastColumnBitmapSeries columnBitmapSeries = d as FastColumnBitmapSeries;
    if (columnBitmapSeries.Area == null)
      return;
    columnBitmapSeries.Area.ScheduleUpdate();
  }
}
