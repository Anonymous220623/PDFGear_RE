// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.CandleSeries
// Assembly: Syncfusion.SfChart.WPF, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 279D70C6-7936-4F3E-B6CF-79EFEF734826
// Assembly location: C:\Program Files\PDFgear\Syncfusion.SfChart.WPF.dll

using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;

#nullable disable
namespace Syncfusion.UI.Xaml.Charts;

public class CandleSeries : FinancialSeriesBase, ISegmentSpacing, ISegmentSelectable
{
  public static readonly DependencyProperty SegmentSpacingProperty = DependencyProperty.Register(nameof (SegmentSpacing), typeof (double), typeof (CandleSeries), new PropertyMetadata((object) 0.0, new PropertyChangedCallback(CandleSeries.OnSegmentSpacingChanged)));
  public static readonly DependencyProperty SegmentSelectionBrushProperty = DependencyProperty.Register(nameof (SegmentSelectionBrush), typeof (Brush), typeof (CandleSeries), new PropertyMetadata((object) null, new PropertyChangedCallback(CandleSeries.OnSegmentSelectionBrush)));
  public static readonly DependencyProperty SelectedIndexProperty = DependencyProperty.Register(nameof (SelectedIndex), typeof (int), typeof (CandleSeries), new PropertyMetadata((object) -1, new PropertyChangedCallback(CandleSeries.OnSelectedIndexChanged)));

  public CandleSeries() => this.DefaultStyleKey = (object) typeof (CandleSeries);

  public double SegmentSpacing
  {
    get => (double) this.GetValue(CandleSeries.SegmentSpacingProperty);
    set => this.SetValue(CandleSeries.SegmentSpacingProperty, (object) value);
  }

  public Brush SegmentSelectionBrush
  {
    get => (Brush) this.GetValue(CandleSeries.SegmentSelectionBrushProperty);
    set => this.SetValue(CandleSeries.SegmentSelectionBrushProperty, (object) value);
  }

  public int SelectedIndex
  {
    get => (int) this.GetValue(CandleSeries.SelectedIndexProperty);
    set => this.SetValue(CandleSeries.SelectedIndexProperty, (object) value);
  }

  internal override bool IsMultipleYPathRequired => true;

  protected internal override bool IsSideBySide => true;

  double ISegmentSpacing.CalculateSegmentSpacing(double spacing, double Right, double Left)
  {
    return this.CalculateSegmentSpacing(spacing, Right, Left);
  }

  public override void CreateSegments()
  {
    DoubleRange sideBySideInfo = this.GetSideBySideInfo((ChartSeriesBase) this);
    double median = sideBySideInfo.Median;
    List<double> xValues = !(this.ActualXAxis is CategoryAxis) || (this.ActualXAxis as CategoryAxis).IsIndexed ? this.GetXValues() : this.GroupedXValuesIndexes;
    IList<double> comparisionModeValues = this.GetComparisionModeValues();
    if (xValues == null)
      return;
    this.ClearUnUsedSegments(this.DataCount);
    if (this.AdornmentsInfo != null)
    {
      if (this.adornmentInfo.GetAdornmentPosition() == AdornmentsPosition.TopAndBottom)
        this.ClearUnUsedAdornments(this.DataCount * 4);
      else
        this.ClearUnUsedAdornments(this.DataCount * 2);
    }
    if (this.ActualXAxis is CategoryAxis && !(this.ActualXAxis as CategoryAxis).IsIndexed)
    {
      this.Segments.Clear();
      this.Adornments.Clear();
      for (int index = 0; index < xValues.Count; ++index)
      {
        if (index < xValues.Count && this.GroupedSeriesYValues[0].Count > index)
        {
          double x1 = xValues[index] + sideBySideInfo.Start;
          double x2 = xValues[index] + sideBySideInfo.End;
          double y1 = this.GroupedSeriesYValues[2][index];
          double y2 = this.GroupedSeriesYValues[3][index];
          bool isBull = index == 0 || this.ComparisonMode == FinancialPrice.None ? this.GroupedSeriesYValues[3][index] > this.GroupedSeriesYValues[2][index] : comparisionModeValues[index] >= comparisionModeValues[index - 1];
          ChartPoint chartPoint1 = new ChartPoint(x1, y1);
          ChartPoint chartPoint2 = new ChartPoint(x2, y2);
          ChartPoint chartPoint3 = new ChartPoint(xValues[index] + median, this.GroupedSeriesYValues[0][index]);
          ChartPoint chartPoint4 = new ChartPoint(xValues[index] + median, this.GroupedSeriesYValues[1][index]);
          if (this.CreateSegment() is CandleSegment segment)
          {
            segment.Series = (ChartSeriesBase) this;
            segment.BullFillColor = this.BullFillColor;
            segment.BearFillColor = this.BearFillColor;
            segment.Item = this.ActualData[index];
            segment.SetData(chartPoint1, chartPoint2, chartPoint3, chartPoint4, isBull);
            segment.High = this.GroupedSeriesYValues[0][index];
            segment.Low = this.GroupedSeriesYValues[1][index];
            segment.Open = this.GroupedSeriesYValues[2][index];
            segment.Close = this.GroupedSeriesYValues[3][index];
            this.Segments.Add((ChartSegment) segment);
          }
          if (this.AdornmentsInfo != null)
            this.AddAdornments(xValues[index], chartPoint3, chartPoint4, chartPoint1, chartPoint1, chartPoint2, chartPoint2, index, 0.0);
        }
      }
    }
    else
    {
      for (int index = 0; index < this.DataCount; ++index)
      {
        double x3 = xValues[index] + sideBySideInfo.Start;
        double x4 = xValues[index] + sideBySideInfo.End;
        double openValue = this.OpenValues[index];
        double closeValue = this.CloseValues[index];
        bool isBull = index == 0 || this.ComparisonMode == FinancialPrice.None ? this.CloseValues[index] > this.OpenValues[index] : comparisionModeValues[index] >= comparisionModeValues[index - 1];
        ChartPoint chartPoint5 = new ChartPoint(x3, openValue);
        ChartPoint chartPoint6 = new ChartPoint(x4, closeValue);
        ChartPoint chartPoint7 = new ChartPoint(xValues[index] + median, this.HighValues[index]);
        ChartPoint chartPoint8 = new ChartPoint(xValues[index] + median, this.LowValues[index]);
        if (index < this.Segments.Count)
        {
          this.Segments[index].SetData(chartPoint5, chartPoint6, chartPoint7, chartPoint8, isBull);
          (this.Segments[index] as CandleSegment).High = this.HighValues[index];
          (this.Segments[index] as CandleSegment).Low = this.LowValues[index];
          (this.Segments[index] as CandleSegment).Open = this.OpenValues[index];
          (this.Segments[index] as CandleSegment).Close = this.CloseValues[index];
          this.Segments[index].Item = this.ActualData[index];
        }
        else if (this.CreateSegment() is CandleSegment segment)
        {
          segment.Series = (ChartSeriesBase) this;
          segment.BullFillColor = this.BullFillColor;
          segment.BearFillColor = this.BearFillColor;
          segment.Item = this.ActualData[index];
          segment.SetData(chartPoint5, chartPoint6, chartPoint7, chartPoint8, isBull);
          segment.High = this.HighValues[index];
          segment.Low = this.LowValues[index];
          segment.Open = this.OpenValues[index];
          segment.Close = this.CloseValues[index];
          this.Segments.Add((ChartSegment) segment);
        }
        if (this.AdornmentsInfo != null)
          this.AddAdornments(xValues[index], chartPoint7, chartPoint8, chartPoint5, chartPoint5, chartPoint6, chartPoint6, index, 0.0);
      }
    }
    if (!this.ShowEmptyPoints)
      return;
    this.UpdateEmptyPointSegments(xValues, true);
  }

  protected override ChartSegment CreateSegment() => (ChartSegment) new CandleSegment();

  protected override DependencyObject CloneSeries(DependencyObject obj)
  {
    return base.CloneSeries((DependencyObject) new CandleSeries()
    {
      SegmentSelectionBrush = this.SegmentSelectionBrush,
      SelectedIndex = this.SelectedIndex,
      SegmentSpacing = this.SegmentSpacing
    });
  }

  internal override Point GetDataPointPosition(ChartTooltip tooltip)
  {
    CandleSegment toolTipTag = this.ToolTipTag as CandleSegment;
    Point dataPointPosition = new Point();
    Point visible = this.ChartTransformer.TransformToVisible(toolTipTag.XRange.Median, toolTipTag.High);
    dataPointPosition.X = visible.X + this.ActualArea.SeriesClipRect.Left;
    dataPointPosition.Y = visible.Y + this.ActualArea.SeriesClipRect.Top;
    return dataPointPosition;
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
    CandleSeries candleSeries = d as CandleSeries;
    if (candleSeries.Area == null)
      return;
    candleSeries.Area.ScheduleUpdate();
  }

  private static void OnSegmentSelectionBrush(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    (d as CandleSeries).UpdateArea();
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

  private bool PointIsOnLine(Point startPoint, Point endPoint, Point checkPoint)
  {
    return !this.IsTransposed ? (Math.Round(startPoint.X) == Math.Ceiling(checkPoint.X) || Math.Round(startPoint.X) == Math.Floor(checkPoint.X)) && endPoint.Y - startPoint.Y == endPoint.Y - checkPoint.Y + (checkPoint.Y - startPoint.Y) : (Math.Round(startPoint.Y) == Math.Ceiling(checkPoint.Y) || Math.Round(startPoint.Y) == Math.Floor(checkPoint.Y)) && endPoint.X - startPoint.X == endPoint.X - checkPoint.X + (checkPoint.X - startPoint.X);
  }
}
