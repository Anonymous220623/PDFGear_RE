// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.HiLoOpenCloseSeries
// Assembly: Syncfusion.SfChart.WPF, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 279D70C6-7936-4F3E-B6CF-79EFEF734826
// Assembly location: C:\Program Files\PDFgear\Syncfusion.SfChart.WPF.dll

using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;

#nullable disable
namespace Syncfusion.UI.Xaml.Charts;

public class HiLoOpenCloseSeries : FinancialSeriesBase, ISegmentSpacing, ISegmentSelectable
{
  public static readonly DependencyProperty SegmentSpacingProperty = DependencyProperty.Register(nameof (SegmentSpacing), typeof (double), typeof (HiLoOpenCloseSeries), new PropertyMetadata((object) 0.0, new PropertyChangedCallback(HiLoOpenCloseSeries.OnSegmentSpacingChanged)));
  public static readonly DependencyProperty SegmentSelectionBrushProperty = DependencyProperty.Register(nameof (SegmentSelectionBrush), typeof (Brush), typeof (HiLoOpenCloseSeries), new PropertyMetadata((object) null, new PropertyChangedCallback(HiLoOpenCloseSeries.OnSegmentSelectionBrush)));
  public static readonly DependencyProperty SelectedIndexProperty = DependencyProperty.Register(nameof (SelectedIndex), typeof (int), typeof (HiLoOpenCloseSeries), new PropertyMetadata((object) -1, new PropertyChangedCallback(HiLoOpenCloseSeries.OnSelectedIndexChanged)));

  public double SegmentSpacing
  {
    get => (double) this.GetValue(HiLoOpenCloseSeries.SegmentSpacingProperty);
    set => this.SetValue(HiLoOpenCloseSeries.SegmentSpacingProperty, (object) value);
  }

  public Brush SegmentSelectionBrush
  {
    get => (Brush) this.GetValue(HiLoOpenCloseSeries.SegmentSelectionBrushProperty);
    set => this.SetValue(HiLoOpenCloseSeries.SegmentSelectionBrushProperty, (object) value);
  }

  public int SelectedIndex
  {
    get => (int) this.GetValue(HiLoOpenCloseSeries.SelectedIndexProperty);
    set => this.SetValue(HiLoOpenCloseSeries.SelectedIndexProperty, (object) value);
  }

  internal override bool IsMultipleYPathRequired => true;

  protected internal override bool IsSideBySide => true;

  double ISegmentSpacing.CalculateSegmentSpacing(double spacing, double Right, double Left)
  {
    return this.CalculateSegmentSpacing(spacing, Right, Left);
  }

  public override void CreateSegments()
  {
    bool flag = !(this.ActualXAxis is CategoryAxis) || (this.ActualXAxis as CategoryAxis).IsIndexed;
    List<double> xValues = flag ? this.GetXValues() : this.GroupedXValuesIndexes;
    IList<double> comparisionModeValues = this.GetComparisionModeValues();
    if (xValues == null)
      return;
    DoubleRange sideBySideInfo = this.GetSideBySideInfo((ChartSeriesBase) this);
    double median1 = sideBySideInfo.Delta / 2.0;
    double median2 = sideBySideInfo.Median;
    double num1 = sideBySideInfo.Start;
    double num2 = sideBySideInfo.End;
    double segmentSpacing1 = ((ISegmentSpacing) this).CalculateSegmentSpacing(this.SegmentSpacing, num2, num1);
    double segmentSpacing2 = ((ISegmentSpacing) this).CalculateSegmentSpacing(this.SegmentSpacing, num1, num2);
    if (this.SegmentSpacing > 0.0 && this.SegmentSpacing <= 1.0)
    {
      num1 = segmentSpacing1;
      num2 = segmentSpacing2;
    }
    if (!flag)
    {
      this.Segments.Clear();
      this.Adornments.Clear();
      for (int index = 0; index < xValues.Count; ++index)
      {
        ChartPoint chartPoint1 = new ChartPoint(xValues[index] + median2, this.GroupedSeriesYValues[0][index]);
        ChartPoint chartPoint2 = new ChartPoint(xValues[index] + median2, this.GroupedSeriesYValues[1][index]);
        ChartPoint chartPoint3 = new ChartPoint(xValues[index] + num1, this.GroupedSeriesYValues[2][index]);
        ChartPoint chartPoint4 = new ChartPoint(xValues[index] + median2, this.GroupedSeriesYValues[2][index]);
        ChartPoint chartPoint5 = new ChartPoint(xValues[index] + num2, this.GroupedSeriesYValues[3][index]);
        ChartPoint chartPoint6 = new ChartPoint(xValues[index] + median2, this.GroupedSeriesYValues[3][index]);
        bool isBull = index == 0 || this.ComparisonMode == FinancialPrice.None ? this.GroupedSeriesYValues[2][index] < this.GroupedSeriesYValues[3][index] : comparisionModeValues[index] >= comparisionModeValues[index - 1];
        if (this.CreateSegment() is HiLoOpenCloseSegment segment)
        {
          segment.Series = (ChartSeriesBase) this;
          segment.Item = this.ActualData[index];
          segment.SetData(chartPoint1, chartPoint2, chartPoint3, chartPoint4, chartPoint5, chartPoint6, isBull);
          segment.BullFillColor = this.BullFillColor;
          segment.BearFillColor = this.BearFillColor;
          segment.High = this.GroupedSeriesYValues[0][index];
          segment.Low = this.GroupedSeriesYValues[1][index];
          segment.Open = this.GroupedSeriesYValues[2][index];
          segment.Close = this.GroupedSeriesYValues[3][index];
          segment.Item = this.ActualData[index];
          this.Segments.Add((ChartSegment) segment);
        }
        if (this.AdornmentsInfo != null)
          this.AddAdornments(xValues[index], chartPoint1, chartPoint2, chartPoint3, chartPoint4, chartPoint5, chartPoint6, index, median1);
      }
    }
    else
    {
      if (this.Segments.Count > this.DataCount)
        this.ClearUnUsedSegments(this.DataCount);
      if (this.AdornmentsInfo != null)
      {
        if (this.adornmentInfo.GetAdornmentPosition() == AdornmentsPosition.TopAndBottom)
          this.ClearUnUsedAdornments(this.DataCount * 4);
        else
          this.ClearUnUsedAdornments(this.DataCount * 2);
      }
      for (int index = 0; index < this.DataCount; ++index)
      {
        ChartPoint chartPoint7 = new ChartPoint(xValues[index] + median2, this.HighValues[index]);
        ChartPoint chartPoint8 = new ChartPoint(xValues[index] + median2, this.LowValues[index]);
        ChartPoint chartPoint9 = new ChartPoint(xValues[index] + num1, this.OpenValues[index]);
        ChartPoint chartPoint10 = new ChartPoint(xValues[index] + median2, this.OpenValues[index]);
        ChartPoint chartPoint11 = new ChartPoint(xValues[index] + num2, this.CloseValues[index]);
        ChartPoint chartPoint12 = new ChartPoint(xValues[index] + median2, this.CloseValues[index]);
        bool isBull = index == 0 || this.ComparisonMode == FinancialPrice.None ? this.OpenValues[index] < this.CloseValues[index] : comparisionModeValues[index] >= comparisionModeValues[index - 1];
        if (index < this.Segments.Count)
        {
          this.Segments[index].Item = this.ActualData[index];
          this.Segments[index].SetData(chartPoint7, chartPoint8, chartPoint9, chartPoint10, chartPoint11, chartPoint12, isBull);
          (this.Segments[index] as HiLoOpenCloseSegment).High = this.HighValues[index];
          (this.Segments[index] as HiLoOpenCloseSegment).Low = this.LowValues[index];
          (this.Segments[index] as HiLoOpenCloseSegment).Open = this.OpenValues[index];
          (this.Segments[index] as HiLoOpenCloseSegment).Close = this.CloseValues[index];
        }
        else if (this.CreateSegment() is HiLoOpenCloseSegment segment)
        {
          segment.Series = (ChartSeriesBase) this;
          segment.Item = this.ActualData[index];
          segment.SetData(chartPoint7, chartPoint8, chartPoint9, chartPoint10, chartPoint11, chartPoint12, isBull);
          segment.BullFillColor = this.BullFillColor;
          segment.BearFillColor = this.BearFillColor;
          segment.High = this.HighValues[index];
          segment.Low = this.LowValues[index];
          segment.Open = this.OpenValues[index];
          segment.Close = this.CloseValues[index];
          segment.Item = this.ActualData[index];
          this.Segments.Add((ChartSegment) segment);
        }
        if (this.AdornmentsInfo != null)
          this.AddAdornments(xValues[index], chartPoint7, chartPoint8, chartPoint9, chartPoint10, chartPoint11, chartPoint12, index, median1);
      }
    }
    if (!this.ShowEmptyPoints)
      return;
    this.UpdateEmptyPointSegments(xValues, true);
  }

  internal override Point GetDataPointPosition(ChartTooltip tooltip)
  {
    HiLoOpenCloseSegment toolTipTag = this.ToolTipTag as HiLoOpenCloseSegment;
    Point dataPointPosition = new Point();
    Point visible = this.ChartTransformer.TransformToVisible(toolTipTag.XRange.Median, toolTipTag.YRange.End);
    dataPointPosition.X = visible.X + this.ActualArea.SeriesClipRect.Left;
    dataPointPosition.Y = visible.Y + this.ActualArea.SeriesClipRect.Top;
    return dataPointPosition;
  }

  protected override ChartSegment CreateSegment() => (ChartSegment) new HiLoOpenCloseSegment();

  protected override DependencyObject CloneSeries(DependencyObject obj)
  {
    return base.CloneSeries((DependencyObject) new HiLoOpenCloseSeries()
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
    HiLoOpenCloseSeries loOpenCloseSeries = d as HiLoOpenCloseSeries;
    if (loOpenCloseSeries.Area == null)
      return;
    loOpenCloseSeries.Area.ScheduleUpdate();
  }

  private static void OnSegmentSelectionBrush(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    (d as HiLoOpenCloseSeries).UpdateArea();
  }

  private static void OnSelectedIndexChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    ChartSeries chartSeries = d as ChartSeries;
    chartSeries.OnPropertyChanged("SelectedIndex");
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

  private bool IsPointOnLine(
    Point startPoint,
    Point endPoint,
    Point checkPoint,
    bool isOpenCloseLine)
  {
    return !this.IsTransposed ? (!isOpenCloseLine ? (Math.Round(startPoint.X) == Math.Ceiling(checkPoint.X) || Math.Round(startPoint.X) == Math.Floor(checkPoint.X)) && endPoint.Y - startPoint.Y == endPoint.Y - checkPoint.Y + (checkPoint.Y - startPoint.Y) : (Math.Round(startPoint.Y) == Math.Ceiling(checkPoint.Y) || Math.Round(startPoint.Y) == Math.Floor(checkPoint.Y)) && endPoint.X - startPoint.X == endPoint.X - checkPoint.X + (checkPoint.X - startPoint.X)) : (!isOpenCloseLine ? (Math.Round(startPoint.Y) == Math.Ceiling(checkPoint.Y) || Math.Round(startPoint.Y) == Math.Floor(checkPoint.Y)) && endPoint.X - startPoint.X == endPoint.X - checkPoint.X + (checkPoint.X - startPoint.X) : (Math.Round(startPoint.X) == Math.Ceiling(checkPoint.X) || Math.Round(startPoint.X) == Math.Floor(checkPoint.X)) && endPoint.Y - startPoint.Y == endPoint.Y - checkPoint.Y + (checkPoint.Y - startPoint.Y));
  }
}
