// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.FastRangeAreaBitmapSeries
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

public class FastRangeAreaBitmapSeries : RangeSeriesBase, ISegmentSelectable
{
  public static readonly DependencyProperty EnableAntiAliasingProperty = DependencyProperty.Register(nameof (EnableAntiAliasing), typeof (bool), typeof (FastRangeAreaBitmapSeries), new PropertyMetadata((object) false, new PropertyChangedCallback(FastRangeAreaBitmapSeries.OnSeriesPropertyChanged)));
  public static readonly DependencyProperty SegmentSelectionBrushProperty = DependencyProperty.Register(nameof (SegmentSelectionBrush), typeof (Brush), typeof (FastRangeAreaBitmapSeries), new PropertyMetadata((object) null, new PropertyChangedCallback(FastRangeAreaBitmapSeries.OnSeriesPropertyChanged)));
  public static readonly DependencyProperty SelectedIndexProperty = DependencyProperty.Register(nameof (SelectedIndex), typeof (int), typeof (FastRangeAreaBitmapSeries), new PropertyMetadata((object) -1, new PropertyChangedCallback(FastRangeAreaBitmapSeries.OnSelectedIndexChanged)));
  public static readonly DependencyProperty HighValueInteriorProperty = DependencyProperty.Register(nameof (HighValueInterior), typeof (Brush), typeof (FastRangeAreaBitmapSeries), new PropertyMetadata((object) null, new PropertyChangedCallback(FastRangeAreaBitmapSeries.OnSeriesPropertyChanged)));
  public static readonly DependencyProperty LowValueInteriorProperty = DependencyProperty.Register(nameof (LowValueInterior), typeof (Brush), typeof (FastRangeAreaBitmapSeries), new PropertyMetadata((object) null, new PropertyChangedCallback(FastRangeAreaBitmapSeries.OnSeriesPropertyChanged)));

  public bool EnableAntiAliasing
  {
    get => (bool) this.GetValue(FastRangeAreaBitmapSeries.EnableAntiAliasingProperty);
    set => this.SetValue(FastRangeAreaBitmapSeries.EnableAntiAliasingProperty, (object) value);
  }

  public Brush SegmentSelectionBrush
  {
    get => (Brush) this.GetValue(FastRangeAreaBitmapSeries.SegmentSelectionBrushProperty);
    set => this.SetValue(FastRangeAreaBitmapSeries.SegmentSelectionBrushProperty, (object) value);
  }

  public int SelectedIndex
  {
    get => (int) this.GetValue(FastRangeAreaBitmapSeries.SelectedIndexProperty);
    set => this.SetValue(FastRangeAreaBitmapSeries.SelectedIndexProperty, (object) value);
  }

  public Brush HighValueInterior
  {
    get => (Brush) this.GetValue(FastRangeAreaBitmapSeries.HighValueInteriorProperty);
    set => this.SetValue(FastRangeAreaBitmapSeries.HighValueInteriorProperty, (object) value);
  }

  public Brush LowValueInterior
  {
    get => (Brush) this.GetValue(FastRangeAreaBitmapSeries.LowValueInteriorProperty);
    set => this.SetValue(FastRangeAreaBitmapSeries.LowValueInteriorProperty, (object) value);
  }

  internal override bool IsMultipleYPathRequired => true;

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

  protected internal override bool IsLinear => true;

  protected internal override bool IsAreaTypeSeries => true;

  public override void CreateSegments()
  {
    ChartPoint? nullable = new ChartPoint?();
    List<ChartPoint> segPoints1 = new List<ChartPoint>();
    List<double> doubleList = !(this.ActualXAxis is CategoryAxis) || (this.ActualXAxis as CategoryAxis).IsIndexed ? this.GetXValues() : this.GroupedXValuesIndexes;
    if (doubleList != null)
    {
      if (this.ActualXAxis is CategoryAxis && !(this.ActualXAxis as CategoryAxis).IsIndexed)
      {
        this.Segments.Clear();
        this.Adornments.Clear();
        if (double.IsNaN(this.GroupedSeriesYValues[1][0]) || !double.IsNaN(this.GroupedSeriesYValues[0][0]))
        {
          segPoints1.Add(new ChartPoint(doubleList[0], this.GroupedSeriesYValues[1][0]));
          segPoints1.Add(new ChartPoint(doubleList[0], this.GroupedSeriesYValues[0][0]));
          this.AddSegment(segPoints1, false, this.GroupedSeriesYValues[0][0], this.GroupedSeriesYValues[1][0], this.ActualData[0]);
        }
        List<ChartPoint> segPoints2 = new List<ChartPoint>();
        int index1;
        for (index1 = 0; index1 < doubleList.Count - 1; ++index1)
        {
          if (!double.IsNaN(this.GroupedSeriesYValues[1][index1]) && !double.IsNaN(this.GroupedSeriesYValues[0][index1]))
          {
            ChartPoint p11 = new ChartPoint(doubleList[index1], this.GroupedSeriesYValues[1][index1]);
            ChartPoint p21 = new ChartPoint(doubleList[index1], this.GroupedSeriesYValues[0][index1]);
            if (index1 == 0 || index1 < doubleList.Count - 1 && (double.IsNaN(this.GroupedSeriesYValues[1][index1 - 1]) || double.IsNaN(this.GroupedSeriesYValues[0][index1 - 1])))
            {
              segPoints2.Add(p11);
              segPoints2.Add(p21);
            }
            if (!double.IsNaN(this.GroupedSeriesYValues[1][index1 + 1]) && !double.IsNaN(this.GroupedSeriesYValues[0][index1 + 1]))
            {
              ChartPoint p12 = new ChartPoint(doubleList[index1 + 1], this.GroupedSeriesYValues[1][index1 + 1]);
              ChartPoint p22 = new ChartPoint(doubleList[index1 + 1], this.GroupedSeriesYValues[0][index1 + 1]);
              ChartPoint? crossPoint = ChartMath.GetCrossPoint(p11, p12, p21, p22);
              if (crossPoint.HasValue)
              {
                ChartPoint chartPoint = new ChartPoint(crossPoint.Value.X, crossPoint.Value.Y);
                segPoints2.Add(chartPoint);
                segPoints2.Add(chartPoint);
                this.AddSegment(segPoints2, this.GroupedSeriesYValues[1][index1] > this.GroupedSeriesYValues[0][index1], this.GroupedSeriesYValues[0][index1], this.GroupedSeriesYValues[1][index1], this.ActualData[index1]);
                segPoints2 = new List<ChartPoint>();
                segPoints2.Add(chartPoint);
                segPoints2.Add(chartPoint);
              }
              segPoints2.Add(p12);
              segPoints2.Add(p22);
            }
            else if (index1 != 0 && !double.IsNaN(this.GroupedSeriesYValues[1][index1 - 1]) && !double.IsNaN(this.GroupedSeriesYValues[0][index1 - 1]))
            {
              segPoints2.Add(p11);
              segPoints2.Add(p21);
            }
          }
          else
          {
            if (segPoints2.Count > 0 && !double.IsNaN(this.GroupedSeriesYValues[1][index1 - 1]) && !double.IsNaN(this.GroupedSeriesYValues[0][index1 - 1]))
              this.AddSegment(segPoints2, false, this.GroupedSeriesYValues[0][index1 - 1], this.GroupedSeriesYValues[1][index1 - 1], this.ActualData[index1 - 1]);
            segPoints2 = new List<ChartPoint>();
          }
        }
        if (segPoints2.Count > 0)
          this.AddSegment(segPoints2, this.GroupedSeriesYValues[1][index1] > this.GroupedSeriesYValues[0][index1], this.GroupedSeriesYValues[0][index1], this.GroupedSeriesYValues[1][index1], this.ActualData[index1]);
        else if (index1 == doubleList.Count - 1 && (double.IsNaN(this.GroupedSeriesYValues[1][index1]) || double.IsNaN(this.GroupedSeriesYValues[0][index1])))
        {
          segPoints2.Add(new ChartPoint(doubleList[index1], this.GroupedSeriesYValues[1][index1]));
          segPoints2.Add(new ChartPoint(doubleList[index1], this.GroupedSeriesYValues[0][index1]));
          this.AddSegment(segPoints2, false, this.GroupedSeriesYValues[0][index1], this.GroupedSeriesYValues[1][index1], this.ActualData[index1]);
        }
        for (int index2 = 0; index2 < doubleList.Count; ++index2)
        {
          if (this.AdornmentsInfo != null)
            this.AddAdornments(doubleList[index2], 0.0, this.GroupedSeriesYValues[0][index2], this.GroupedSeriesYValues[1][index2], index2);
        }
      }
      else
      {
        this.Segments.Clear();
        if (this.AdornmentsInfo != null)
        {
          if (this.adornmentInfo.GetAdornmentPosition() == AdornmentsPosition.TopAndBottom)
            this.ClearUnUsedAdornments(this.DataCount * 2);
          else
            this.ClearUnUsedAdornments(this.DataCount);
        }
        if (doubleList != null)
        {
          if (double.IsNaN(this.LowValues[0]) || double.IsNaN(this.HighValues[0]))
          {
            segPoints1.Add(new ChartPoint(doubleList[0], this.LowValues[0]));
            segPoints1.Add(new ChartPoint(doubleList[0], this.HighValues[0]));
            this.AddSegment(segPoints1, false, this.HighValues[0], this.LowValues[0], this.ActualData[0]);
          }
          List<ChartPoint> segPoints3 = new List<ChartPoint>();
          int index;
          for (index = 0; index < this.DataCount - 1; ++index)
          {
            if (!double.IsNaN(this.LowValues[index]) && !double.IsNaN(this.HighValues[index]))
            {
              ChartPoint p11 = new ChartPoint(doubleList[index], this.LowValues[index]);
              ChartPoint p21 = new ChartPoint(doubleList[index], this.HighValues[index]);
              if (index == 0 || index < this.DataCount - 1 && (double.IsNaN(this.LowValues[index - 1]) || double.IsNaN(this.HighValues[index - 1])))
              {
                segPoints3.Add(p11);
                segPoints3.Add(p21);
              }
              if (!double.IsNaN(this.LowValues[index + 1]) && !double.IsNaN(this.HighValues[index + 1]))
              {
                ChartPoint p12 = new ChartPoint(doubleList[index + 1], this.LowValues[index + 1]);
                ChartPoint p22 = new ChartPoint(doubleList[index + 1], this.HighValues[index + 1]);
                ChartPoint? crossPoint = ChartMath.GetCrossPoint(p11, p12, p21, p22);
                if (crossPoint.HasValue)
                {
                  ChartPoint chartPoint = new ChartPoint(crossPoint.Value.X, crossPoint.Value.Y);
                  segPoints3.Add(chartPoint);
                  segPoints3.Add(chartPoint);
                  this.AddSegment(segPoints3, this.LowValues[index] > this.HighValues[index], this.HighValues[index], this.LowValues[index], this.ActualData[index]);
                  segPoints3 = new List<ChartPoint>();
                  segPoints3.Add(chartPoint);
                  segPoints3.Add(chartPoint);
                }
                segPoints3.Add(p12);
                segPoints3.Add(p22);
              }
            }
            else
            {
              if (segPoints3.Count > 0 && !double.IsNaN(this.LowValues[index - 1]) && !double.IsNaN(this.HighValues[index - 1]))
                this.AddSegment(segPoints3, false, this.HighValues[index - 1], this.LowValues[index - 1], this.ActualData[index - 1]);
              segPoints3 = new List<ChartPoint>();
            }
          }
          if (segPoints3.Count > 0)
            this.AddSegment(segPoints3, this.LowValues[index] > this.HighValues[index], this.HighValues[index], this.LowValues[index], this.ActualData[index]);
          else if (index == this.DataCount - 1 && (double.IsNaN(this.LowValues[index]) || double.IsNaN(this.HighValues[index])))
          {
            segPoints3.Add(new ChartPoint(doubleList[index], this.LowValues[index]));
            segPoints3.Add(new ChartPoint(doubleList[index], this.HighValues[index]));
            this.AddSegment(segPoints3, false, this.HighValues[index], this.LowValues[index], this.ActualData[index]);
          }
        }
        for (int index = 0; index < doubleList.Count; ++index)
        {
          if (this.AdornmentsInfo != null)
            this.AddAdornments(doubleList[index], 0.0, this.HighValues[index], this.LowValues[index], index);
        }
      }
    }
    if (this.Segments.Count <= 1)
      return;
    this.UpdateEmptyPointSegments();
  }

  internal override ChartDataPointInfo GetDataPoint(Point mousePos)
  {
    List<int> intList = new List<int>();
    IList<double> doubleList = this.ActualXValues is IList<double> ? this.ActualXValues as IList<double> : (IList<double>) this.GetXValues();
    int startIndex;
    int endIndex;
    Rect rect;
    this.CalculateHittestRect(mousePos, out startIndex, out endIndex, out rect);
    Point point1 = new Point();
    Point point2 = new Point();
    for (int index = startIndex; index <= endIndex; ++index)
    {
      point1.X = this.IsIndexed ? (double) index : doubleList[index];
      point1.Y = this.ActualSeriesYValues[0][index];
      point2.X = this.IsIndexed ? (double) index : doubleList[index];
      point2.Y = this.ActualSeriesYValues[1][index];
      if (rect.Contains(point1) || rect.Contains(point2))
        intList.Add(index);
    }
    if (intList.Count <= 0)
      return this.dataPoint;
    int index1 = intList[intList.Count / 2];
    this.dataPoint = new ChartDataPointInfo();
    this.dataPoint.Index = index1;
    this.dataPoint.XData = doubleList[index1];
    this.dataPoint.High = this.ActualSeriesYValues[0][index1];
    this.dataPoint.Low = this.ActualSeriesYValues[1][index1];
    this.dataPoint.Series = (ChartSeriesBase) this;
    if (index1 > -1 && this.ActualData.Count > index1)
      this.dataPoint.Item = this.ActualData[index1];
    return this.dataPoint;
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
    object toolTipTag = this.ToolTipTag;
    double x = 0.0;
    double y = 0.0;
    double stackedYValue = double.NaN;
    Point dataPointPosition = new Point();
    if (!this.Area.SeriesClipRect.Contains(this.mousePos))
      return dataPointPosition;
    this.FindNearestChartPoint(new Point(this.mousePos.X - this.Area.SeriesClipRect.Left, this.mousePos.Y - this.Area.SeriesClipRect.Top), out x, out y, out stackedYValue);
    if (double.IsNaN(x))
      return dataPointPosition;
    Point visible = this.ChartTransformer.TransformToVisible(x, y);
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

  protected override ChartSegment CreateSegment() => (ChartSegment) new FastRangeAreaSegment();

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

  protected override void OnDataSourceChanged(IEnumerable oldValue, IEnumerable newValue)
  {
    this.Segment = (ChartSegment) null;
    base.OnDataSourceChanged(oldValue, newValue);
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
      if (this.GroupedSeriesYValues[0].Count > adornmentIndex)
        customTag.High = this.GroupedSeriesYValues[0][adornmentIndex];
      if (this.GroupedSeriesYValues[1].Count > adornmentIndex)
        customTag.Low = this.GroupedSeriesYValues[1][adornmentIndex];
    }
    else
    {
      if (this.ActualSeriesYValues[0].Count > adornmentIndex)
        customTag.High = this.ActualSeriesYValues[0][adornmentIndex];
      if (this.ActualSeriesYValues[1].Count > adornmentIndex)
        customTag.Low = this.ActualSeriesYValues[1][adornmentIndex];
    }
    customTag.Index = adornmentIndex;
    customTag.Series = (ChartSeriesBase) this;
    if (this.ActualData.Count > adornmentIndex)
      customTag.Item = this.ActualData[adornmentIndex];
    this.UpdateSeriesTooltip((object) customTag);
  }

  protected override DependencyObject CloneSeries(DependencyObject obj)
  {
    return base.CloneSeries((DependencyObject) new RangeAreaSeries()
    {
      SegmentSelectionBrush = this.SegmentSelectionBrush,
      SelectedIndex = this.SelectedIndex,
      HighValueInterior = this.HighValueInterior,
      LowValueInterior = this.LowValueInterior
    });
  }

  private static void OnSeriesPropertyChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    (d as FastRangeAreaBitmapSeries).UpdateArea();
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

  private void AddSegment(
    List<ChartPoint> segPoints,
    bool isHighLow,
    double highValue,
    double lowValue,
    object actualData)
  {
    if (!(this.CreateSegment() is FastRangeAreaSegment segment))
      return;
    segment.Series = (ChartSeriesBase) this;
    segment.SetData(segPoints);
    segment.High = highValue;
    segment.Low = lowValue;
    segment.Item = actualData;
    segment.IsHighLow = isHighLow;
    this.Segments.Add((ChartSegment) segment);
  }

  private void UpdateEmptyPointSegments()
  {
    FastRangeAreaSegment rangeAreaSegment = (FastRangeAreaSegment) null;
    for (int index = 0; index < this.Segments.Count; ++index)
    {
      FastRangeAreaSegment segment = this.Segments[index] as FastRangeAreaSegment;
      int num = segment.AreaValues.Count<ChartPoint>((Func<ChartPoint, bool>) (value => !double.IsNaN(value.Y)));
      if (num > 1 && num == segment.AreaValues.Count)
      {
        if (rangeAreaSegment == null)
          segment.EmptyStroke = EmptyStroke.Right;
        else if (rangeAreaSegment.EmptyStroke == EmptyStroke.Right)
          segment.EmptyStroke = EmptyStroke.Left;
        else if (rangeAreaSegment.EmptyStroke == EmptyStroke.Left)
        {
          rangeAreaSegment.EmptyStroke = EmptyStroke.Both;
          segment.EmptyStroke = EmptyStroke.Left;
        }
        rangeAreaSegment = segment;
      }
    }
  }
}
