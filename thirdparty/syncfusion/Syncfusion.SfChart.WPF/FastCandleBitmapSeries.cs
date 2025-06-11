// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.FastCandleBitmapSeries
// Assembly: Syncfusion.SfChart.WPF, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 279D70C6-7936-4F3E-B6CF-79EFEF734826
// Assembly location: C:\Program Files\PDFgear\Syncfusion.SfChart.WPF.dll

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

#nullable disable
namespace Syncfusion.UI.Xaml.Charts;

public class FastCandleBitmapSeries : FinancialSeriesBase, ISegmentSpacing, ISegmentSelectable
{
  public static readonly DependencyProperty SegmentSpacingProperty = DependencyProperty.Register(nameof (SegmentSpacing), typeof (double), typeof (FastCandleBitmapSeries), new PropertyMetadata((object) 0.0, new PropertyChangedCallback(FastCandleBitmapSeries.OnSegmentSpacingChanged)));
  public static readonly DependencyProperty SegmentSelectionBrushProperty = DependencyProperty.Register(nameof (SegmentSelectionBrush), typeof (Brush), typeof (FastCandleBitmapSeries), new PropertyMetadata((object) null, new PropertyChangedCallback(FastCandleBitmapSeries.OnSegmentSelectionBrush)));
  public static readonly DependencyProperty SelectedIndexProperty = DependencyProperty.Register(nameof (SelectedIndex), typeof (int), typeof (FastCandleBitmapSeries), new PropertyMetadata((object) -1, new PropertyChangedCallback(FastCandleBitmapSeries.OnSelectedIndexChanged)));
  private bool isFill;
  private List<int> selectedBorderPixels = new List<int>();

  public FastCandleBitmapSeries()
  {
    this.DefaultStyleKey = (object) typeof (FastCandleBitmapSeries);
  }

  public double SegmentSpacing
  {
    get => (double) this.GetValue(FastCandleBitmapSeries.SegmentSpacingProperty);
    set => this.SetValue(FastCandleBitmapSeries.SegmentSpacingProperty, (object) value);
  }

  public Brush SegmentSelectionBrush
  {
    get => (Brush) this.GetValue(FastCandleBitmapSeries.SegmentSelectionBrushProperty);
    set => this.SetValue(FastCandleBitmapSeries.SegmentSelectionBrushProperty, (object) value);
  }

  public int SelectedIndex
  {
    get => (int) this.GetValue(FastCandleBitmapSeries.SelectedIndexProperty);
    set => this.SetValue(FastCandleBitmapSeries.SelectedIndexProperty, (object) value);
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

  double ISegmentSpacing.CalculateSegmentSpacing(double spacing, double Right, double Left)
  {
    return this.CalculateSegmentSpacing(spacing, Right, Left);
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
      double median = this.GetSideBySideInfo((ChartSeriesBase) this).Median;
      if (this.AdornmentsInfo != null)
      {
        for (int index1 = 0; index1 < xValues.Count; ++index1)
        {
          if (index1 < xValues.Count && this.GroupedSeriesYValues[0].Count > index1)
          {
            List<double> doubleList;
            int index2;
            (doubleList = xValues)[index2 = index1] = doubleList[index2] + median;
            ChartPoint highPt = new ChartPoint(xValues[index1], this.GroupedSeriesYValues[0][index1]);
            ChartPoint lowPt = new ChartPoint(xValues[index1], this.GroupedSeriesYValues[1][index1]);
            ChartPoint chartPoint1 = new ChartPoint(xValues[index1], this.GroupedSeriesYValues[2][index1]);
            ChartPoint chartPoint2 = new ChartPoint(xValues[index1], this.GroupedSeriesYValues[3][index1]);
            this.AddAdornments(xValues[index1], highPt, lowPt, chartPoint1, chartPoint1, chartPoint2, chartPoint2, index1, 0.0);
          }
        }
      }
      if (this.Segment != null && this.Segments.Count != 0)
        return;
      this.Segment = (ChartSegment) (this.CreateSegment() as FastCandleBitmapSegment);
      if (this.Segment == null)
        return;
      this.Segment.Series = (ChartSeriesBase) this;
      this.Segment.Item = (object) this.ActualData;
      this.Segment.SetData((IList<double>) xValues, this.GroupedSeriesYValues[2], this.GroupedSeriesYValues[3], this.GroupedSeriesYValues[0], this.GroupedSeriesYValues[1]);
      this.Segments.Add(this.Segment);
    }
    else
    {
      this.ClearUnUsedSegments(this.DataCount);
      if (this.AdornmentsInfo != null)
      {
        if (this.adornmentInfo.GetAdornmentPosition() == AdornmentsPosition.TopAndBottom)
          this.ClearUnUsedAdornments(this.DataCount * 4);
        else
          this.ClearUnUsedAdornments(this.DataCount * 2);
        double median = this.GetSideBySideInfo((ChartSeriesBase) this).Median;
        for (int index3 = 0; index3 < this.DataCount; ++index3)
        {
          List<double> doubleList;
          int index4;
          (doubleList = xValues)[index4 = index3] = doubleList[index4] + median;
          ChartPoint highPt = new ChartPoint(xValues[index3], this.HighValues[index3]);
          ChartPoint lowPt = new ChartPoint(xValues[index3], this.LowValues[index3]);
          ChartPoint chartPoint3 = new ChartPoint(xValues[index3], this.OpenValues[index3]);
          ChartPoint chartPoint4 = new ChartPoint(xValues[index3], this.CloseValues[index3]);
          this.AddAdornments(xValues[index3], highPt, lowPt, chartPoint3, chartPoint3, chartPoint4, chartPoint4, index3, 0.0);
        }
      }
      if (this.Segment == null || this.Segments.Count == 0)
      {
        this.Segment = (ChartSegment) (this.CreateSegment() as FastCandleBitmapSegment);
        if (this.Segment == null)
          return;
        this.Segment.Series = (ChartSeriesBase) this;
        this.Segment.Item = (object) this.ActualData;
        this.Segment.SetData((IList<double>) xValues, this.OpenValues, this.CloseValues, this.HighValues, this.LowValues);
        this.Segments.Add(this.Segment);
      }
      else
      {
        (this.Segment as FastCandleBitmapSegment).Item = (object) this.ActualData;
        (this.Segment as FastCandleBitmapSegment).SetData((IList<double>) xValues, this.OpenValues, this.CloseValues, this.HighValues, this.LowValues);
      }
    }
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
    Point visible = this.ChartTransformer.TransformToVisible(toolTipTag.XData, toolTipTag.High);
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
    if (this.ComparisonMode == FinancialPrice.None || this.isFill)
      this.OnBitmapSelection(this.selectedSegmentPixels, (Brush) null, false);
    else
      this.OnBitmapHollowSelection(this.selectedSegmentPixels, this.selectedBorderPixels);
    this.selectedSegmentPixels.Clear();
    this.selectedBorderPixels.Clear();
    this.dataPoint = (ChartDataPointInfo) null;
  }

  internal override void GeneratePixels()
  {
    WriteableBitmap fastRenderSurface = this.Area.fastRenderSurface;
    Rect seriesClipRect = this.Area.SeriesClipRect;
    double width1 = seriesClipRect.Width;
    seriesClipRect = this.Area.SeriesClipRect;
    double height1 = seriesClipRect.Height;
    ChartTransform.ChartCartesianTransformer transformer = this.CreateTransformer(new Size(width1, height1), true) as ChartTransform.ChartCartesianTransformer;
    bool isInversed1 = transformer.XAxis.IsInversed;
    bool isInversed2 = transformer.YAxis.IsInversed;
    double start1 = transformer.XAxis.VisibleRange.Start;
    double end1 = transformer.XAxis.VisibleRange.End;
    double start2 = transformer.YAxis.VisibleRange.Start;
    double end2 = transformer.YAxis.VisibleRange.End;
    double num1;
    double num2;
    double num3;
    double num4;
    if (this.IsActualTransposed)
    {
      Rect renderedRect = transformer.YAxis.RenderedRect;
      num1 = renderedRect.Width;
      renderedRect = transformer.XAxis.RenderedRect;
      num2 = renderedRect.Height;
      renderedRect = transformer.YAxis.RenderedRect;
      num3 = renderedRect.Left - this.Area.SeriesClipRect.Left;
      num4 = transformer.XAxis.RenderedRect.Top - this.Area.SeriesClipRect.Top;
    }
    else
    {
      Rect renderedRect = transformer.YAxis.RenderedRect;
      num1 = renderedRect.Height;
      renderedRect = transformer.XAxis.RenderedRect;
      num2 = renderedRect.Width;
      renderedRect = transformer.YAxis.RenderedRect;
      num3 = renderedRect.Top - this.Area.SeriesClipRect.Top;
      num4 = transformer.XAxis.RenderedRect.Left - this.Area.SeriesClipRect.Left;
    }
    if (isInversed1)
      ;
    if (isInversed2)
      ;
    DoubleRange sideBySideInfo = this.GetSideBySideInfo((ChartSeriesBase) this);
    double median = sideBySideInfo.Median;
    double start3 = sideBySideInfo.Start;
    double end3 = sideBySideInfo.End;
    double xdata = this.dataPoint.XData;
    double open = this.dataPoint.Open;
    double close = this.dataPoint.Close;
    double high = this.dataPoint.High;
    double low = this.dataPoint.Low;
    int index = this.dataPoint.Index;
    double[] numArray = this.Segments[0].AlignHiLoSegment(open, close, high, low);
    double num5 = numArray[0];
    double num6 = numArray[1];
    this.isFill = open > close;
    float num7;
    float num8;
    float num9;
    float num10;
    float num11;
    float num12;
    float num13;
    float num14;
    float num15;
    if (this.IsIndexed)
    {
      if (!this.IsActualTransposed)
      {
        double x1 = !isInversed1 ? start3 + (double) index : end3 + (double) index;
        double x2 = !isInversed1 ? end3 + (double) index : start3 + (double) index;
        double y1 = isInversed2 ? close : open;
        double y2 = isInversed2 ? open : close;
        if ((isInversed2 ? (y1 > y2 ? 1 : 0) : (y1 < y2 ? 1 : 0)) != 0)
        {
          double num16 = y1;
          y1 = y2;
          y2 = num16;
        }
        double y3 = num5;
        double y4 = num6;
        double y5;
        double y6;
        if (open > close)
        {
          y5 = open;
          y6 = close;
        }
        else
        {
          y5 = close;
          y6 = open;
        }
        Point visible1 = transformer.TransformToVisible(x1, y1);
        Point visible2 = transformer.TransformToVisible(x2, y2);
        Point visible3 = transformer.TransformToVisible((double) index + median, y3);
        Point visible4 = transformer.TransformToVisible((double) index + median, y4);
        Point visible5 = transformer.TransformToVisible((double) index + median, y5);
        Point visible6 = transformer.TransformToVisible((double) index + median, y6);
        num7 = (float) visible3.X;
        num8 = (float) visible1.X;
        num9 = (float) visible2.X;
        num10 = (float) visible1.Y;
        num11 = (float) visible2.Y;
        num12 = (float) visible3.Y;
        num13 = (float) visible4.Y;
        num14 = (float) visible5.Y;
        num15 = (float) visible6.Y;
      }
      else
      {
        double newBase1 = transformer.XAxis.IsLogarithmic ? (transformer.XAxis as LogarithmicAxis).LogarithmicBase : 1.0;
        double newBase2 = transformer.YAxis.IsLogarithmic ? (transformer.YAxis as LogarithmicAxis).LogarithmicBase : 1.0;
        double a1 = !isInversed1 ? start3 + (double) index : end3 + (double) index;
        double a2 = !isInversed1 ? end3 + (double) index : start3 + (double) index;
        double a3 = isInversed2 ? close : open;
        double a4 = isInversed2 ? open : close;
        double num17 = newBase1 == 1.0 ? a1 : Math.Log(a1, newBase1);
        double num18 = newBase1 == 1.0 ? a2 : Math.Log(a2, newBase1);
        double num19 = newBase2 == 1.0 ? a3 : Math.Log(a3, newBase2);
        double num20 = newBase2 == 1.0 ? a4 : Math.Log(a4, newBase2);
        if ((isInversed2 ? (num19 > num20 ? 1 : 0) : (num19 < num20 ? 1 : 0)) != 0)
        {
          double num21 = num19;
          num19 = num20;
          num20 = num21;
        }
        double y7 = num5;
        double y8 = num6;
        double y9;
        double y10;
        if (open > close)
        {
          y9 = open;
          y10 = close;
        }
        else
        {
          y9 = close;
          y10 = open;
        }
        Point visible7 = transformer.TransformToVisible((double) index + median, y7);
        Point visible8 = transformer.TransformToVisible((double) index + median, y8);
        Point visible9 = transformer.TransformToVisible((double) index + median, y9);
        Point visible10 = transformer.TransformToVisible((double) index + median, y10);
        num7 = (float) visible7.Y;
        num8 = (float) (num4 + num2 * transformer.XAxis.ValueToCoefficientCalc(num17));
        num9 = (float) (num4 + num2 * transformer.XAxis.ValueToCoefficientCalc(num18));
        num10 = (float) (num3 + num1 * (1.0 - transformer.YAxis.ValueToCoefficientCalc(num19)));
        num11 = (float) (num3 + num1 * (1.0 - transformer.YAxis.ValueToCoefficientCalc(num20)));
        num12 = (float) visible7.X;
        num13 = (float) visible8.X;
        num14 = (float) visible9.X;
        num15 = (float) visible10.X;
      }
    }
    else if (!this.IsActualTransposed)
    {
      double x3 = isInversed1 ? xdata + end3 : xdata + start3;
      double x4 = isInversed1 ? xdata + start3 : xdata + end3;
      double y11 = isInversed2 ? close : open;
      double y12 = isInversed2 ? open : close;
      if ((isInversed2 ? (y11 > y12 ? 1 : 0) : (y11 < y12 ? 1 : 0)) != 0)
      {
        double num22 = y11;
        y11 = y12;
        y12 = num22;
      }
      double y13 = num5;
      double y14 = num6;
      double y15;
      double y16;
      if (open > close)
      {
        y15 = open;
        y16 = close;
      }
      else
      {
        y15 = close;
        y16 = open;
      }
      Point visible11 = transformer.TransformToVisible(x3, y11);
      Point visible12 = transformer.TransformToVisible(x4, y12);
      Point visible13 = transformer.TransformToVisible(xdata + median, y13);
      Point visible14 = transformer.TransformToVisible(xdata + median, y14);
      Point visible15 = transformer.TransformToVisible((double) index + median, y15);
      Point visible16 = transformer.TransformToVisible((double) index + median, y16);
      num7 = (float) visible13.X;
      num8 = (float) visible11.X;
      num9 = (float) visible12.X;
      num10 = (float) visible11.Y;
      num11 = (float) visible12.Y;
      num12 = (float) visible13.Y;
      num13 = (float) visible14.Y;
      num14 = (float) visible15.Y;
      num15 = (float) visible16.Y;
    }
    else
    {
      double newBase3 = transformer.XAxis.IsLogarithmic ? (transformer.XAxis as LogarithmicAxis).LogarithmicBase : 1.0;
      double newBase4 = transformer.YAxis.IsLogarithmic ? (transformer.YAxis as LogarithmicAxis).LogarithmicBase : 1.0;
      double a5 = isInversed1 ? xdata + end3 : xdata + start3;
      double a6 = isInversed1 ? xdata + start3 : xdata + end3;
      double a7 = isInversed2 ? close : open;
      double a8 = isInversed2 ? open : close;
      double num23 = newBase3 == 1.0 ? a5 : Math.Log(a5, newBase3);
      double num24 = newBase3 == 1.0 ? a6 : Math.Log(a6, newBase3);
      double num25 = newBase4 == 1.0 ? a7 : Math.Log(a7, newBase4);
      double num26 = newBase4 == 1.0 ? a8 : Math.Log(a8, newBase4);
      if ((isInversed2 ? (num25 > num26 ? 1 : 0) : (num25 < num26 ? 1 : 0)) != 0)
      {
        double num27 = num25;
        num25 = num26;
        num26 = num27;
      }
      double y17 = num5;
      double y18 = num6;
      double y19;
      double y20;
      if (open > close)
      {
        y19 = open;
        y20 = close;
      }
      else
      {
        y19 = close;
        y20 = open;
      }
      Point visible17 = transformer.TransformToVisible(xdata + median, y17);
      Point visible18 = transformer.TransformToVisible(xdata + median, y18);
      Point visible19 = transformer.TransformToVisible((double) index + median, y19);
      Point visible20 = transformer.TransformToVisible((double) index + median, y20);
      num7 = (float) visible17.Y;
      num8 = (float) (num4 + num2 * transformer.XAxis.ValueToCoefficientCalc(num23));
      num9 = (float) (num4 + num2 * transformer.XAxis.ValueToCoefficientCalc(num24));
      num10 = (float) (num3 + num1 * (1.0 - transformer.YAxis.ValueToCoefficientCalc(num25)));
      num11 = (float) (num3 + num1 * (1.0 - transformer.YAxis.ValueToCoefficientCalc(num26)));
      num12 = (float) visible17.X;
      num13 = (float) visible18.X;
      num14 = (float) visible19.X;
      num15 = (float) visible20.X;
    }
    double segmentSpacing1 = this.SegmentSpacing;
    int width2 = (int) this.Area.SeriesClipRect.Width;
    int height2 = (int) this.Area.SeriesClipRect.Height;
    int num28 = (int) this.StrokeThickness / 2;
    int num29 = this.StrokeThickness % 2.0 == 0.0 ? (int) (this.StrokeThickness / 2.0) : (int) (this.StrokeThickness / 2.0 + 1.0);
    float num30 = num7;
    float num31 = num8;
    float num32 = num9;
    float y1_1 = num10;
    float num33 = num11;
    float num34 = isInversed2 ? num13 : num12;
    float num35 = isInversed2 ? num12 : num13;
    float num36 = isInversed2 ? num15 : num14;
    float num37 = isInversed2 ? num14 : num15;
    float num38 = num30 - (float) num28;
    float num39 = num30 + (float) num29;
    if (segmentSpacing1 > 0.0 && segmentSpacing1 <= 1.0)
    {
      double segmentSpacing2 = ((ISegmentSpacing) this).CalculateSegmentSpacing(segmentSpacing1, (double) num32, (double) num31);
      double segmentSpacing3 = ((ISegmentSpacing) this).CalculateSegmentSpacing(segmentSpacing1, (double) num31, (double) num32);
      num31 = (float) segmentSpacing2;
      num32 = (float) segmentSpacing3;
    }
    this.selectedSegmentPixels.Clear();
    this.selectedBorderPixels.Clear();
    float y2_1 = (double) y1_1 == (double) num33 ? num33 + 1f : num33;
    if (!this.IsActualTransposed)
    {
      if (this.ComparisonMode != FinancialPrice.None)
      {
        this.selectedBorderPixels = fastRenderSurface.GetDrawRectangle((int) num31, (int) y1_1, (int) num32, (int) y2_1, this.selectedBorderPixels);
        this.selectedBorderPixels = fastRenderSurface.GetRectangle((int) num38, (int) num34, (int) num39, (int) num36, this.selectedBorderPixels);
        this.selectedBorderPixels = fastRenderSurface.GetRectangle((int) num38, (int) num37, (int) num39, (int) num35, this.selectedBorderPixels);
      }
      this.selectedSegmentPixels = fastRenderSurface.GetRectangle((int) num31, (int) y1_1, (int) num32, (int) y2_1, this.selectedSegmentPixels);
      this.selectedSegmentPixels = fastRenderSurface.GetRectangle((int) num38, (int) num34, (int) num39, (int) num36, this.selectedSegmentPixels);
      this.selectedSegmentPixels = fastRenderSurface.GetRectangle((int) num38, (int) num37, (int) num39, (int) num35, this.selectedSegmentPixels);
    }
    else
    {
      if (this.ComparisonMode != FinancialPrice.None)
      {
        this.selectedBorderPixels = fastRenderSurface.GetDrawRectangle((int) ((double) width2 - (double) y2_1), (int) ((double) height2 - (double) num32), (int) ((double) width2 - (double) y1_1), (int) ((double) height2 - (double) num31), this.selectedBorderPixels);
        this.selectedBorderPixels = fastRenderSurface.GetRectangle((int) num34, (int) num38, (int) num36, (int) num39, this.selectedBorderPixels);
        this.selectedBorderPixels = fastRenderSurface.GetRectangle((int) num37, (int) num38, (int) num35, (int) num39, this.selectedBorderPixels);
      }
      this.selectedSegmentPixels = fastRenderSurface.GetRectangle((int) ((double) width2 - (double) y2_1), (int) ((double) height2 - (double) num32), (int) ((double) width2 - (double) y1_1), (int) ((double) height2 - (double) num31), this.selectedSegmentPixels);
      this.selectedSegmentPixels = fastRenderSurface.GetRectangle((int) num34, (int) num38, (int) num36, (int) num39, this.selectedSegmentPixels);
      this.selectedSegmentPixels = fastRenderSurface.GetRectangle((int) num37, (int) num38, (int) num35, (int) num39, this.selectedSegmentPixels);
    }
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

  protected override ChartSegment CreateSegment() => (ChartSegment) new FastCandleBitmapSegment();

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
      if (this.GroupedSeriesYValues[2].Count > adornmentIndex)
        customTag.Open = this.GroupedSeriesYValues[2][adornmentIndex];
      if (this.GroupedSeriesYValues[3].Count > adornmentIndex)
        customTag.Close = this.GroupedSeriesYValues[3][adornmentIndex];
    }
    else
    {
      if (this.ActualSeriesYValues[0].Count > adornmentIndex)
        customTag.High = this.ActualSeriesYValues[0][adornmentIndex];
      if (this.ActualSeriesYValues[1].Count > adornmentIndex)
        customTag.Low = this.ActualSeriesYValues[1][adornmentIndex];
      if (this.ActualSeriesYValues[2].Count > adornmentIndex)
        customTag.Open = this.ActualSeriesYValues[2][adornmentIndex];
      if (this.ActualSeriesYValues[3].Count > adornmentIndex)
        customTag.Close = this.ActualSeriesYValues[3][adornmentIndex];
    }
    customTag.Index = adornmentIndex;
    customTag.Series = (ChartSeriesBase) this;
    if (this.ActualData.Count > adornmentIndex)
      customTag.Item = this.ActualData[adornmentIndex];
    this.UpdateSeriesTooltip((object) customTag);
  }

  protected override DependencyObject CloneSeries(DependencyObject obj)
  {
    return base.CloneSeries((DependencyObject) new FastCandleBitmapSeries()
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
    FastCandleBitmapSeries candleBitmapSeries = d as FastCandleBitmapSeries;
    if (candleBitmapSeries.Area == null)
      return;
    candleBitmapSeries.Area.ScheduleUpdate();
  }

  private static void OnSegmentSelectionBrush(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    (d as FastCandleBitmapSeries).UpdateArea();
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

  private unsafe void OnBitmapHollowSelection(List<int> SegmentPixels, List<int> BorderPixels)
  {
    if (SegmentPixels == null || SegmentPixels.Count <= 0)
      return;
    int seriesIndex = this.Area.Series.IndexOf((ChartSeries) this);
    if (!this.Area.isBitmapPixelsConverted)
      this.Area.ConvertBitmapPixels();
    foreach (ChartSeriesBase chartSeriesBase in this.Area.Series.Where<ChartSeries>((Func<ChartSeries, bool>) (series => this.Area.Series.IndexOf(series) > seriesIndex)).ToList<ChartSeries>())
      this.upperSeriesPixels.UnionWith((IEnumerable<int>) chartSeriesBase.Pixels);
    WriteableBitmap fastRenderSurface = this.Area.fastRenderSurface;
    fastRenderSurface.Lock();
    int* pixels = fastRenderSurface.GetPixels();
    int num1 = ChartSeries.ConvertColor(Colors.Transparent);
    int num2 = ChartSeries.ConvertColor((this.Segments[0] as FastCandleBitmapSegment).GetSegmentBrush(this.dataPoint.Index));
    foreach (int segmentPixel in SegmentPixels)
    {
      if (this.Pixels.Contains(segmentPixel) && !this.upperSeriesPixels.Contains(segmentPixel))
        pixels[segmentPixel] = num1;
    }
    foreach (int borderPixel in BorderPixels)
    {
      if (this.Pixels.Contains(borderPixel) && !this.upperSeriesPixels.Contains(borderPixel))
        pixels[borderPixel] = num2;
    }
    fastRenderSurface.AddDirtyRect(new Int32Rect(0, 0, fastRenderSurface.PixelWidth, fastRenderSurface.PixelHeight));
    fastRenderSurface.Unlock();
    this.upperSeriesPixels.Clear();
  }
}
