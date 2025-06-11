// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.HiLoSeries
// Assembly: Syncfusion.SfChart.WPF, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 279D70C6-7936-4F3E-B6CF-79EFEF734826
// Assembly location: C:\Program Files\PDFgear\Syncfusion.SfChart.WPF.dll

using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;

#nullable disable
namespace Syncfusion.UI.Xaml.Charts;

public class HiLoSeries : RangeSeriesBase, ISegmentSelectable
{
  public static readonly DependencyProperty SegmentSelectionBrushProperty = DependencyProperty.Register(nameof (SegmentSelectionBrush), typeof (Brush), typeof (HiLoSeries), new PropertyMetadata((object) null, new PropertyChangedCallback(HiLoSeries.OnSegmentSelectionBrush)));
  public static readonly DependencyProperty SelectedIndexProperty = DependencyProperty.Register(nameof (SelectedIndex), typeof (int), typeof (HiLoSeries), new PropertyMetadata((object) -1, new PropertyChangedCallback(HiLoSeries.OnSelectedIndexChanged)));
  private Point y1Value = new Point();
  private Point y2Value = new Point();

  public Brush SegmentSelectionBrush
  {
    get => (Brush) this.GetValue(HiLoSeries.SegmentSelectionBrushProperty);
    set => this.SetValue(HiLoSeries.SegmentSelectionBrushProperty, (object) value);
  }

  public int SelectedIndex
  {
    get => (int) this.GetValue(HiLoSeries.SelectedIndexProperty);
    set => this.SetValue(HiLoSeries.SelectedIndexProperty, (object) value);
  }

  internal override bool IsMultipleYPathRequired => true;

  protected internal override bool IsSideBySide => true;

  public override void CreateSegments()
  {
    bool flag = !(this.ActualXAxis is CategoryAxis) || (this.ActualXAxis as CategoryAxis).IsIndexed;
    List<double> xValues = flag ? this.GetXValues() : this.GroupedXValuesIndexes;
    if (xValues == null)
      return;
    double median = this.GetSideBySideInfo((ChartSeriesBase) this).Median;
    if (!flag)
    {
      this.Segments.Clear();
      this.Adornments.Clear();
      for (int index1 = 0; index1 < xValues.Count; ++index1)
      {
        if (index1 < xValues.Count && this.GroupedSeriesYValues[0].Count > index1)
        {
          List<double> doubleList;
          int index2;
          (doubleList = xValues)[index2 = index1] = doubleList[index2] + median;
          if (this.CreateSegment() is HiLoSegment segment)
          {
            segment.Series = (ChartSeriesBase) this;
            segment.Item = this.ActualData[index1];
            segment.SetData(xValues[index1], this.GroupedSeriesYValues[0][index1], this.GroupedSeriesYValues[1][index1]);
            segment.High = this.GroupedSeriesYValues[0][index1];
            segment.Low = this.GroupedSeriesYValues[1][index1];
            segment.XValue = (object) xValues[index1];
            this.Segments.Add((ChartSegment) segment);
          }
          if (this.AdornmentsInfo != null)
            this.AddAdornments(xValues[index1], 0.0, this.GroupedSeriesYValues[0][index1], this.GroupedSeriesYValues[1][index1], index1);
        }
      }
    }
    else
    {
      if (this.Segments.Count > this.DataCount)
        this.ClearUnUsedSegments(this.DataCount);
      if (this.AdornmentsInfo != null)
      {
        if (this.adornmentInfo.GetAdornmentPosition() == AdornmentsPosition.TopAndBottom)
          this.ClearUnUsedAdornments(this.DataCount * 2);
        else
          this.ClearUnUsedAdornments(this.DataCount);
      }
      for (int index3 = 0; index3 < this.DataCount; ++index3)
      {
        if (index3 < this.Segments.Count)
        {
          List<double> doubleList;
          int index4;
          (doubleList = xValues)[index4 = index3] = doubleList[index4] + median;
          this.Segments[index3].SetData(xValues[index3], this.HighValues[index3], this.LowValues[index3]);
          this.Segments[index3].Item = this.ActualData[index3];
          (this.Segments[index3] as HiLoSegment).High = this.HighValues[index3];
          (this.Segments[index3] as HiLoSegment).Low = this.LowValues[index3];
        }
        else
        {
          List<double> doubleList;
          int index5;
          (doubleList = xValues)[index5 = index3] = doubleList[index5] + median;
          if (this.CreateSegment() is HiLoSegment segment)
          {
            segment.Series = (ChartSeriesBase) this;
            segment.Item = this.ActualData[index3];
            segment.SetData(xValues[index3], this.HighValues[index3], this.LowValues[index3]);
            segment.High = this.HighValues[index3];
            segment.Low = this.LowValues[index3];
            segment.XValue = (object) xValues[index3];
            this.Segments.Add((ChartSegment) segment);
          }
        }
        if (this.AdornmentsInfo != null)
          this.AddAdornments(xValues[index3], 0.0, this.HighValues[index3], this.LowValues[index3], index3);
      }
    }
    if (!this.ShowEmptyPoints)
      return;
    this.UpdateEmptyPointSegments(xValues, true);
  }

  internal override ChartDataPointInfo GetDataPoint(Point mousePos)
  {
    List<int> intList = new List<int>();
    IList<double> doubleList = this.ActualXValues is IList<double> ? this.ActualXValues as IList<double> : (IList<double>) this.GetXValues();
    int startIndex;
    int endIndex;
    Rect rect;
    this.CalculateHittestRect(mousePos, out startIndex, out endIndex, out rect);
    for (int index = startIndex; index <= endIndex; ++index)
    {
      this.y1Value.X = this.IsIndexed ? (double) index : doubleList[index];
      this.y1Value.Y = this.ActualSeriesYValues[0][index];
      this.y2Value.X = this.IsIndexed ? (double) index : doubleList[index];
      this.y2Value.Y = this.ActualSeriesYValues[1][index];
      if (rect.Contains(this.y1Value) || rect.Contains(this.y2Value))
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

  internal override Point GetDataPointPosition(ChartTooltip tooltip)
  {
    HiLoSegment toolTipTag = this.ToolTipTag as HiLoSegment;
    Point dataPointPosition = new Point();
    Point visible = this.ChartTransformer.TransformToVisible((double) toolTipTag.XValue, toolTipTag.High);
    dataPointPosition.X = visible.X + this.ActualArea.SeriesClipRect.Left;
    dataPointPosition.Y = visible.Y + this.ActualArea.SeriesClipRect.Top;
    return dataPointPosition;
  }

  protected override ChartSegment CreateSegment() => (ChartSegment) new HiLoSegment();

  protected override DependencyObject CloneSeries(DependencyObject obj)
  {
    return base.CloneSeries((DependencyObject) new HiLoSeries()
    {
      SegmentSelectionBrush = this.SegmentSelectionBrush,
      SelectedIndex = this.SelectedIndex
    });
  }

  private static void OnSegmentSelectionBrush(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    (d as HiLoSeries).UpdateArea();
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

  private bool IsPointOnLine(Point startPoint, Point endPoint, Point checkPoint)
  {
    return !this.IsTransposed ? (Math.Round(startPoint.X) == Math.Ceiling(checkPoint.X) || Math.Round(startPoint.X) == Math.Floor(checkPoint.X)) && endPoint.Y - startPoint.Y == endPoint.Y - checkPoint.Y + (checkPoint.Y - startPoint.Y) : (Math.Round(startPoint.Y) == Math.Ceiling(checkPoint.Y) || Math.Round(startPoint.X) == Math.Floor(checkPoint.X)) && endPoint.X - startPoint.X == endPoint.X - checkPoint.X + (checkPoint.X - startPoint.X);
  }
}
