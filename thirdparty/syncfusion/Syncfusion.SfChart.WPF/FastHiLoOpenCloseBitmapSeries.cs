// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.FastHiLoOpenCloseBitmapSeries
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

public class FastHiLoOpenCloseBitmapSeries : FinancialSeriesBase, ISegmentSpacing, ISegmentSelectable
{
  public static readonly DependencyProperty SegmentSpacingProperty = DependencyProperty.Register(nameof (SegmentSpacing), typeof (double), typeof (FastHiLoOpenCloseBitmapSeries), new PropertyMetadata((object) 0.0, new PropertyChangedCallback(FastHiLoOpenCloseBitmapSeries.OnSegmentSpacingChanged)));
  public static readonly DependencyProperty SegmentSelectionBrushProperty = DependencyProperty.Register(nameof (SegmentSelectionBrush), typeof (Brush), typeof (FastHiLoOpenCloseBitmapSeries), new PropertyMetadata((object) null, new PropertyChangedCallback(FastHiLoOpenCloseBitmapSeries.OnSegmentSelectionBrush)));
  public static readonly DependencyProperty SelectedIndexProperty = DependencyProperty.Register(nameof (SelectedIndex), typeof (int), typeof (FastHiLoOpenCloseBitmapSeries), new PropertyMetadata((object) -1, new PropertyChangedCallback(FastHiLoOpenCloseBitmapSeries.OnSelectedIndexChanged)));

  public int SelectedIndex
  {
    get => (int) this.GetValue(FastHiLoOpenCloseBitmapSeries.SelectedIndexProperty);
    set => this.SetValue(FastHiLoOpenCloseBitmapSeries.SelectedIndexProperty, (object) value);
  }

  public double SegmentSpacing
  {
    get => (double) this.GetValue(FastHiLoOpenCloseBitmapSeries.SegmentSpacingProperty);
    set => this.SetValue(FastHiLoOpenCloseBitmapSeries.SegmentSpacingProperty, (object) value);
  }

  public Brush SegmentSelectionBrush
  {
    get => (Brush) this.GetValue(FastHiLoOpenCloseBitmapSeries.SegmentSelectionBrushProperty);
    set
    {
      this.SetValue(FastHiLoOpenCloseBitmapSeries.SegmentSelectionBrushProperty, (object) value);
    }
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
      if ((this.Segment == null || this.Segments.Count == 0) && this.CreateSegment() is FastHiLoOpenCloseSegment segment)
      {
        segment.Series = (ChartSeriesBase) this;
        segment.Item = (object) this.ActualData;
        segment.SetData((IList<double>) xValues, this.GroupedSeriesYValues[0], this.GroupedSeriesYValues[1], this.GroupedSeriesYValues[2], this.GroupedSeriesYValues[3]);
        this.Segment = (ChartSegment) segment;
        this.Segments.Add(this.Segment);
      }
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
      for (int index = 0; index < xValues.Count; ++index)
      {
        if (index < xValues.Count && this.GroupedSeriesYValues[0].Count > index)
        {
          ChartPoint highPt = new ChartPoint(xValues[index] + median2, this.GroupedSeriesYValues[0][index]);
          ChartPoint lowPt = new ChartPoint(xValues[index] + median2, this.GroupedSeriesYValues[1][index]);
          ChartPoint startOpenPt = new ChartPoint(xValues[index] + num1, this.GroupedSeriesYValues[2][index]);
          ChartPoint endOpenPt = new ChartPoint(xValues[index] + median2, this.GroupedSeriesYValues[2][index]);
          ChartPoint startClosePt = new ChartPoint(xValues[index] + num2, this.GroupedSeriesYValues[3][index]);
          ChartPoint endClosePt = new ChartPoint(xValues[index] + median2, this.GroupedSeriesYValues[3][index]);
          if (this.AdornmentsInfo != null)
            this.AddAdornments(xValues[index], highPt, lowPt, startOpenPt, endOpenPt, startClosePt, endClosePt, index, median1);
        }
      }
    }
    else
    {
      if (this.AdornmentsInfo != null)
      {
        if (this.adornmentInfo.GetAdornmentPosition() == AdornmentsPosition.TopAndBottom)
          this.ClearUnUsedAdornments(this.DataCount * 4);
        else
          this.ClearUnUsedAdornments(this.DataCount * 2);
      }
      if (this.Segment == null || this.Segments.Count == 0)
      {
        if (this.CreateSegment() is FastHiLoOpenCloseSegment segment)
        {
          segment.Series = (ChartSeriesBase) this;
          segment.Item = (object) this.ActualData;
          segment.SetData((IList<double>) xValues, this.HighValues, this.LowValues, this.OpenValues, this.CloseValues);
          this.Segment = (ChartSegment) segment;
          this.Segments.Add(this.Segment);
        }
      }
      else if (xValues != null)
      {
        (this.Segment as FastHiLoOpenCloseSegment).Item = (object) this.ActualData;
        (this.Segment as FastHiLoOpenCloseSegment).SetData((IList<double>) xValues, this.HighValues, this.LowValues, this.OpenValues, this.CloseValues);
      }
      if (this.AdornmentsInfo != null)
      {
        if (this.adornmentInfo.GetAdornmentPosition() == AdornmentsPosition.TopAndBottom)
          this.ClearUnUsedAdornments(this.DataCount * 4);
        else
          this.ClearUnUsedAdornments(this.DataCount * 2);
      }
      DoubleRange sideBySideInfo = this.GetSideBySideInfo((ChartSeriesBase) this);
      double median3 = sideBySideInfo.Delta / 2.0;
      double median4 = sideBySideInfo.Median;
      double num3 = sideBySideInfo.Start;
      double num4 = sideBySideInfo.End;
      double segmentSpacing3 = ((ISegmentSpacing) this).CalculateSegmentSpacing(this.SegmentSpacing, num4, num3);
      double segmentSpacing4 = ((ISegmentSpacing) this).CalculateSegmentSpacing(this.SegmentSpacing, num3, num4);
      if (this.SegmentSpacing > 0.0 && this.SegmentSpacing <= 1.0)
      {
        num3 = segmentSpacing3;
        num4 = segmentSpacing4;
      }
      for (int index = 0; index < this.DataCount; ++index)
      {
        ChartPoint highPt = new ChartPoint(xValues[index] + median4, this.HighValues[index]);
        ChartPoint lowPt = new ChartPoint(xValues[index] + median4, this.LowValues[index]);
        ChartPoint startOpenPt = new ChartPoint(xValues[index] + num3, this.OpenValues[index]);
        ChartPoint endOpenPt = new ChartPoint(xValues[index] + median4, this.OpenValues[index]);
        ChartPoint startClosePt = new ChartPoint(xValues[index] + num4, this.CloseValues[index]);
        ChartPoint endClosePt = new ChartPoint(xValues[index] + median4, this.CloseValues[index]);
        if (this.AdornmentsInfo != null)
          this.AddAdornments(xValues[index], highPt, lowPt, startOpenPt, endOpenPt, startClosePt, endClosePt, index, median3);
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
            PreviousSelectedSeries = (ChartSeriesBase) null,
            PreviousSelectedSegment = (ChartSegment) null,
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
    bool isInversed1 = transformer.XAxis.IsInversed;
    bool isInversed2 = transformer.YAxis.IsInversed;
    DoubleRange sideBySideInfo = this.GetSideBySideInfo((ChartSeriesBase) this);
    double median = sideBySideInfo.Median;
    double num1 = sideBySideInfo.Start;
    double num2 = sideBySideInfo.End;
    double xdata = this.dataPoint.XData;
    double open = this.dataPoint.Open;
    double close = this.dataPoint.Close;
    double high = this.dataPoint.High;
    double low = this.dataPoint.Low;
    int index = this.dataPoint.Index;
    double segmentSpacing1 = this.SegmentSpacing;
    double segmentSpacing2 = ((ISegmentSpacing) this).CalculateSegmentSpacing(segmentSpacing1, num2, num1);
    double segmentSpacing3 = ((ISegmentSpacing) this).CalculateSegmentSpacing(segmentSpacing1, num1, num2);
    if (segmentSpacing1 > 0.0 && segmentSpacing1 <= 1.0)
    {
      num1 = segmentSpacing2;
      num2 = segmentSpacing3;
    }
    double highValues = high;
    double lowValues = low;
    double[] numArray = this.Segments[0].AlignHiLoSegment(open, close, highValues, lowValues);
    double y1 = numArray[0];
    double y2 = numArray[1];
    float num3;
    float num4;
    float num5;
    float num6;
    float num7;
    float num8;
    float num9;
    if (this.IsIndexed)
    {
      Point visible1 = transformer.TransformToVisible((double) index + median, y1);
      Point visible2 = transformer.TransformToVisible(xdata + median, y2);
      Point visible3 = transformer.TransformToVisible(xdata + median, open);
      Point visible4 = transformer.TransformToVisible(xdata + num1, open);
      Point visible5 = transformer.TransformToVisible((double) index + num2, close);
      if (!this.IsActualTransposed)
      {
        num3 = (float) visible1.X;
        num4 = (float) visible1.Y;
        num5 = (float) visible2.Y;
        num6 = (float) visible3.Y;
        num7 = (float) visible4.X;
        num8 = (float) visible5.Y;
        num9 = (float) visible5.X;
      }
      else
      {
        num3 = (float) visible1.Y;
        num4 = (float) visible1.X;
        num5 = (float) visible2.X;
        num6 = (float) visible3.X;
        num7 = (float) visible4.Y;
        num8 = (float) visible5.X;
        num9 = (float) visible5.Y;
      }
    }
    else
    {
      Point visible6 = transformer.TransformToVisible(xdata + median, y1);
      Point visible7 = transformer.TransformToVisible(xdata + median, y2);
      Point visible8 = transformer.TransformToVisible(xdata + median, open);
      Point visible9 = transformer.TransformToVisible(xdata + num1, open);
      Point visible10 = transformer.TransformToVisible(xdata + num2, close);
      if (!this.IsActualTransposed)
      {
        num3 = (float) visible6.X;
        num4 = (float) visible6.Y;
        num5 = (float) visible7.Y;
        num6 = (float) visible8.Y;
        num7 = (float) visible9.X;
        num8 = (float) visible10.Y;
        num9 = (float) visible10.X;
      }
      else
      {
        num3 = (float) visible6.Y;
        num4 = (float) visible6.X;
        num5 = (float) visible7.X;
        num6 = (float) visible8.X;
        num7 = (float) visible9.Y;
        num8 = (float) visible10.X;
        num9 = (float) visible10.Y;
      }
    }
    int num10 = (int) this.StrokeThickness / 2;
    int num11 = this.StrokeThickness % 2.0 == 0.0 ? (int) (this.StrokeThickness / 2.0) : (int) (this.StrokeThickness / 2.0 + 1.0);
    this.selectedSegmentPixels.Clear();
    float num12 = num3;
    float num13 = isInversed2 ? num5 : num4;
    float num14 = isInversed2 ? num4 : num5;
    float num15 = isInversed1 ? num8 : num6;
    float num16 = isInversed1 ? num9 : num7;
    float num17 = isInversed1 ? num6 : num8;
    float num18 = isInversed1 ? num7 : num9;
    if (!this.IsActualTransposed)
    {
      int x1 = (int) num12 - num10;
      int x2 = (int) num12 + num11;
      this.selectedSegmentPixels = fastRenderSurface.GetRectangle(x1, (int) num13, x2, (int) num14, this.selectedSegmentPixels);
      int y1_1 = (int) num15 - num10;
      int y2_1 = (int) num15 + num11;
      this.selectedSegmentPixels = fastRenderSurface.GetRectangle((int) num16, y1_1, (int) num12 - num10, y2_1, this.selectedSegmentPixels);
      int y1_2 = (int) num17 - num10;
      int y2_2 = (int) num17 + num11;
      this.selectedSegmentPixels = fastRenderSurface.GetRectangle((int) num12 + num10, y1_2, (int) num18, y2_2, this.selectedSegmentPixels);
    }
    else
    {
      int y1_3 = (int) num12 - num10;
      int y2_3 = (int) num12 + num11;
      this.selectedSegmentPixels = fastRenderSurface.GetRectangle((int) num14, y1_3, (int) num13, y2_3, this.selectedSegmentPixels);
      int x1_1 = (int) num15 - num10;
      int x2_1 = (int) num15 + num11;
      this.selectedSegmentPixels = fastRenderSurface.GetRectangle(x1_1, (int) num12 + num10, x2_1, (int) num16, this.selectedSegmentPixels);
      int x1_2 = (int) num17 - num10;
      int x2_2 = (int) num17 + num11;
      this.selectedSegmentPixels = fastRenderSurface.GetRectangle(x1_2, (int) num18, x2_2, (int) num12 - num10, this.selectedSegmentPixels);
    }
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

  protected override ChartSegment CreateSegment() => (ChartSegment) new FastHiLoOpenCloseSegment();

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
            PreviousSelectedSeries = (ChartSeriesBase) null,
            PreviousSelectedSegment = (ChartSegment) null,
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
    return base.CloneSeries((DependencyObject) new FastHiLoOpenCloseBitmapSeries()
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
    FastHiLoOpenCloseBitmapSeries closeBitmapSeries = d as FastHiLoOpenCloseBitmapSeries;
    if (closeBitmapSeries.Area == null)
      return;
    closeBitmapSeries.Area.ScheduleUpdate();
  }

  private static void OnSegmentSelectionBrush(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    (d as FastHiLoOpenCloseBitmapSeries).UpdateArea();
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
