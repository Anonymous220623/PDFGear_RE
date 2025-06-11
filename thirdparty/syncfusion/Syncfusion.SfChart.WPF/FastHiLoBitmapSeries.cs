// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.FastHiLoBitmapSeries
// Assembly: Syncfusion.SfChart.WPF, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 279D70C6-7936-4F3E-B6CF-79EFEF734826
// Assembly location: C:\Program Files\PDFgear\Syncfusion.SfChart.WPF.dll

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

#nullable disable
namespace Syncfusion.UI.Xaml.Charts;

public class FastHiLoBitmapSeries : RangeSeriesBase, ISegmentSelectable
{
  public static readonly DependencyProperty SegmentSelectionBrushProperty = DependencyProperty.Register(nameof (SegmentSelectionBrush), typeof (Brush), typeof (FastHiLoBitmapSeries), new PropertyMetadata((object) null, new PropertyChangedCallback(FastHiLoBitmapSeries.OnSegmentSelectionBrush)));
  public static readonly DependencyProperty SelectedIndexProperty = DependencyProperty.Register(nameof (SelectedIndex), typeof (int), typeof (FastHiLoBitmapSeries), new PropertyMetadata((object) -1, new PropertyChangedCallback(FastHiLoBitmapSeries.OnSelectedIndexChanged)));

  public Brush SegmentSelectionBrush
  {
    get => (Brush) this.GetValue(FastHiLoBitmapSeries.SegmentSelectionBrushProperty);
    set => this.SetValue(FastHiLoBitmapSeries.SegmentSelectionBrushProperty, (object) value);
  }

  public int SelectedIndex
  {
    get => (int) this.GetValue(FastHiLoBitmapSeries.SelectedIndexProperty);
    set => this.SetValue(FastHiLoBitmapSeries.SelectedIndexProperty, (object) value);
  }

  internal override bool IsMultipleYPathRequired => true;

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

  public override void CreateSegments()
  {
    bool flag = this.ActualXAxis is CategoryAxis && !(this.ActualXAxis as CategoryAxis).IsIndexed;
    List<double> xValues = !flag ? this.GetXValues() : this.GroupedXValuesIndexes;
    if (xValues == null)
      return;
    if (flag)
    {
      this.Segments.Clear();
      this.Adornments.Clear();
      if ((this.Segment == null || this.Segments.Count == 0) && this.CreateSegment() is FastHiLoSegment segment)
      {
        segment.Series = (ChartSeriesBase) this;
        segment.Item = (object) this.ActualData;
        segment.SetData((IList<double>) xValues, this.GroupedSeriesYValues[0], this.GroupedSeriesYValues[1]);
        this.Segment = (ChartSegment) segment;
        this.Segments.Add((ChartSegment) segment);
      }
      double median = this.GetSideBySideInfo((ChartSeriesBase) this).Median;
      for (int index1 = 0; index1 < xValues.Count; ++index1)
      {
        if (index1 < xValues.Count)
        {
          List<double> doubleList;
          int index2;
          (doubleList = xValues)[index2 = index1] = doubleList[index2] + median;
          if (this.AdornmentsInfo != null && this.GroupedSeriesYValues[0].Count > index1)
            this.AddAdornments(xValues[index1], 0.0, this.GroupedSeriesYValues[0][index1], this.GroupedSeriesYValues[1][index1], index1);
        }
      }
    }
    else
    {
      if (this.AdornmentsInfo != null)
      {
        if (this.adornmentInfo.GetAdornmentPosition() == AdornmentsPosition.TopAndBottom)
          this.ClearUnUsedAdornments(this.DataCount * 2);
        else
          this.ClearUnUsedAdornments(this.DataCount);
      }
      if (this.Segment == null || this.Segments.Count == 0)
      {
        if (this.CreateSegment() is FastHiLoSegment segment)
        {
          segment.Series = (ChartSeriesBase) this;
          segment.Item = (object) this.ActualData;
          segment.SetData((IList<double>) xValues, this.HighValues, this.LowValues);
          this.Segment = (ChartSegment) segment;
          this.Segments.Add((ChartSegment) segment);
        }
      }
      else if (xValues != null)
      {
        (this.Segment as FastHiLoSegment).Item = (object) this.ActualData;
        (this.Segment as FastHiLoSegment).SetData((IList<double>) xValues, this.HighValues, this.LowValues);
      }
      if (this.AdornmentsInfo != null)
      {
        if (this.adornmentInfo.GetAdornmentPosition() == AdornmentsPosition.TopAndBottom)
          this.ClearUnUsedAdornments(this.DataCount * 2);
        else
          this.ClearUnUsedAdornments(this.DataCount);
      }
      double median = this.GetSideBySideInfo((ChartSeriesBase) this).Median;
      for (int index3 = 0; index3 < this.DataCount; ++index3)
      {
        List<double> doubleList;
        int index4;
        (doubleList = xValues)[index4 = index3] = doubleList[index4] + median;
        if (this.AdornmentsInfo != null)
          this.AddAdornments(xValues[index3], 0.0, this.HighValues[index3], this.LowValues[index3], index3);
      }
    }
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

  internal override void GeneratePixels()
  {
    WriteableBitmap fastRenderSurface = this.Area.fastRenderSurface;
    ChartTransform.ChartCartesianTransformer transformer = this.CreateTransformer(new Size(this.Area.SeriesClipRect.Width, this.Area.SeriesClipRect.Height), true) as ChartTransform.ChartCartesianTransformer;
    bool isInversed = transformer.YAxis.IsInversed;
    double xdata = this.dataPoint.XData;
    double high = this.dataPoint.High;
    double low = this.dataPoint.Low;
    int index = this.dataPoint.Index;
    float num1;
    float num2;
    float num3;
    if (this.IsIndexed)
    {
      if (!this.IsActualTransposed)
      {
        Point visible1 = transformer.TransformToVisible((double) index, high);
        Point visible2 = transformer.TransformToVisible((double) index, low);
        num1 = (float) visible1.X;
        num2 = (float) visible1.Y;
        num3 = (float) visible2.Y;
      }
      else
      {
        Point visible3 = transformer.TransformToVisible((double) index, high);
        Point visible4 = transformer.TransformToVisible((double) index, low);
        num1 = (float) visible3.Y;
        num2 = (float) visible3.X;
        num3 = (float) visible4.X;
      }
    }
    else if (!this.IsActualTransposed)
    {
      Point visible5 = transformer.TransformToVisible(xdata, high);
      Point visible6 = transformer.TransformToVisible(xdata, low);
      num1 = (float) visible5.X;
      num2 = (float) visible5.Y;
      num3 = (float) visible6.Y;
    }
    else
    {
      Point visible7 = transformer.TransformToVisible(xdata, high);
      Point visible8 = transformer.TransformToVisible(xdata, low);
      num1 = (float) visible7.Y;
      num2 = (float) visible7.X;
      num3 = (float) visible8.X;
    }
    int num4 = (int) this.StrokeThickness / 2;
    int num5 = this.StrokeThickness % 2.0 == 0.0 ? (int) (this.StrokeThickness / 2.0) : (int) (this.StrokeThickness / 2.0 + 1.0);
    float num6 = num1;
    float num7 = isInversed ? num3 : num2;
    float num8 = num1;
    float num9 = isInversed ? num2 : num3;
    float num10 = num6 - (float) num4;
    this.selectedSegmentPixels.Clear();
    if (!this.IsActualTransposed)
    {
      float x2 = num6 + (float) num5;
      this.selectedSegmentPixels = fastRenderSurface.GetRectangle((int) num10, (int) num7, (int) x2, (int) num9, this.selectedSegmentPixels);
    }
    else
    {
      int y2 = (int) num8 + num5;
      this.selectedSegmentPixels = fastRenderSurface.GetRectangle((int) num9, (int) num10, (int) num7, y2, this.selectedSegmentPixels);
    }
  }

  internal override ChartDataPointInfo GetDataPoint(Point mousePos)
  {
    double x = 0.0;
    double y = 0.0;
    double stackedYValue = double.NaN;
    this.dataPoint = (ChartDataPointInfo) null;
    this.dataPoint = new ChartDataPointInfo();
    if (this.Area.SeriesClipRect.Contains(mousePos))
    {
      this.FindNearestChartPoint(new Point(mousePos.X - this.Area.SeriesClipRect.Left, mousePos.Y - this.Area.SeriesClipRect.Top), out x, out y, out stackedYValue);
      this.dataPoint.XData = x;
      int index = this.GetXValues().IndexOf(x);
      if (index > -1)
      {
        this.dataPoint.High = this.ActualSeriesYValues[0][index];
        this.dataPoint.Low = this.ActualSeriesYValues[1][index];
        this.dataPoint.Index = index;
        this.dataPoint.Series = (ChartSeriesBase) this;
        if (index > -1 && this.ActualData.Count > index)
          this.dataPoint.Item = this.ActualData[index];
      }
    }
    return this.dataPoint;
  }

  internal override Point GetDataPointPosition(ChartTooltip tooltip)
  {
    ChartDataPointInfo toolTipTag = this.ToolTipTag as ChartDataPointInfo;
    Point dataPointPosition = new Point();
    Point visible = this.ChartTransformer.TransformToVisible(toolTipTag.XData, toolTipTag.High);
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

  protected override ChartSegment CreateSegment() => (ChartSegment) new FastHiLoSegment();

  protected override DependencyObject CloneSeries(DependencyObject obj)
  {
    return base.CloneSeries((DependencyObject) new FastHiLoBitmapSeries()
    {
      SegmentSelectionBrush = this.SegmentSelectionBrush,
      SelectedIndex = this.SelectedIndex
    });
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

  private static void OnSegmentSelectionBrush(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    (d as FastHiLoBitmapSeries).UpdateArea();
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
