// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.FastLineBitmapSeries
// Assembly: Syncfusion.SfChart.WPF, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 279D70C6-7936-4F3E-B6CF-79EFEF734826
// Assembly location: C:\Program Files\PDFgear\Syncfusion.SfChart.WPF.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

#nullable disable
namespace Syncfusion.UI.Xaml.Charts;

public class FastLineBitmapSeries : XyDataSeries, ISegmentSelectable
{
  public static readonly DependencyProperty EnableAntiAliasingProperty = DependencyProperty.Register(nameof (EnableAntiAliasing), typeof (bool), typeof (FastLineBitmapSeries), new PropertyMetadata((object) false, new PropertyChangedCallback(FastLineBitmapSeries.OnSeriesPropertyChanged)));
  public static readonly DependencyProperty StrokeDashArrayProperty = DependencyProperty.Register(nameof (StrokeDashArray), typeof (DoubleCollection), typeof (FastLineBitmapSeries), new PropertyMetadata((object) null, new PropertyChangedCallback(FastLineBitmapSeries.OnSeriesPropertyChanged)));
  public static readonly DependencyProperty SegmentSelectionBrushProperty = DependencyProperty.Register(nameof (SegmentSelectionBrush), typeof (Brush), typeof (FastLineBitmapSeries), new PropertyMetadata((object) null, new PropertyChangedCallback(FastLineBitmapSeries.OnSegmentSelectionBrush)));
  public static readonly DependencyProperty SelectedIndexProperty = DependencyProperty.Register(nameof (SelectedIndex), typeof (int), typeof (FastLineBitmapSeries), new PropertyMetadata((object) -1, new PropertyChangedCallback(FastLineBitmapSeries.OnSelectedIndexChanged)));
  private IList<double> xValues;
  private Point hitPoint = new Point();
  private bool isAdornmentPending;
  private Polygon polygon = new Polygon();
  private PointCollection polygonPoints = new PointCollection();

  public int SelectedIndex
  {
    get => (int) this.GetValue(FastLineBitmapSeries.SelectedIndexProperty);
    set => this.SetValue(FastLineBitmapSeries.SelectedIndexProperty, (object) value);
  }

  public Brush SegmentSelectionBrush
  {
    get => (Brush) this.GetValue(FastLineBitmapSeries.SegmentSelectionBrushProperty);
    set => this.SetValue(FastLineBitmapSeries.SegmentSelectionBrushProperty, (object) value);
  }

  public bool EnableAntiAliasing
  {
    get => (bool) this.GetValue(FastLineBitmapSeries.EnableAntiAliasingProperty);
    set => this.SetValue(FastLineBitmapSeries.EnableAntiAliasingProperty, (object) value);
  }

  public DoubleCollection StrokeDashArray
  {
    get => (DoubleCollection) this.GetValue(FastLineBitmapSeries.StrokeDashArrayProperty);
    set => this.SetValue(FastLineBitmapSeries.StrokeDashArrayProperty, (object) value);
  }

  protected internal override bool IsBitmapSeries => true;

  protected internal override bool IsLinear => true;

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

  private FastLineBitmapSegment Segment { get; set; }

  public override void CreateSegments()
  {
    bool flag = this.ActualXAxis is CategoryAxis && !(this.ActualXAxis as CategoryAxis).IsIndexed;
    this.xValues = !flag ? (this.ActualXValues is IList<double> ? this.ActualXValues as IList<double> : (IList<double>) this.GetXValues()) : (IList<double>) this.GroupedXValuesIndexes;
    if (flag)
    {
      this.Segments.Clear();
      this.Adornments.Clear();
      if ((this.Segment == null || this.Segments.Count == 0) && this.CreateSegment() is FastLineBitmapSegment segment)
      {
        segment.Series = (ChartSeriesBase) this;
        if (this.ActualXAxis is CategoryAxis && !(this.ActualXAxis as CategoryAxis).IsIndexed)
          segment.Item = (object) this.GroupedActualData;
        else
          segment.Item = (object) this.ActualData;
        segment.SetData(this.xValues, this.GroupedSeriesYValues[0]);
        this.Segment = segment;
        this.Segments.Add((ChartSegment) segment);
      }
    }
    else
    {
      this.ClearUnUsedAdornments(this.DataCount);
      if (this.Segment == null || this.Segments.Count == 0)
      {
        if (this.CreateSegment() is FastLineBitmapSegment segment)
        {
          segment.Series = (ChartSeriesBase) this;
          if (this.ActualXAxis is CategoryAxis && !(this.ActualXAxis as CategoryAxis).IsIndexed)
            segment.Item = (object) this.GroupedActualData;
          else
            segment.Item = (object) this.ActualData;
          segment.SetData(this.xValues, this.YValues);
          this.Segment = segment;
          this.Segments.Add((ChartSegment) segment);
        }
      }
      else if (this.ActualXValues != null)
      {
        this.Segment.SetData(this.xValues, this.YValues);
        this.Segment.SetRange();
        this.Segment.Item = (object) this.ActualData;
      }
    }
    this.isAdornmentPending = true;
  }

  internal override bool IsHitTestSeries()
  {
    Point adorningCanvasPoint = this.Area.adorningCanvasPoint;
    int index1 = 0;
    int index2 = this.DataCount - 1;
    List<double> xvalues = this.GetXValues();
    IList<double> yvalues = this.YValues;
    double num1 = this.ActualXAxis.PointToValue(adorningCanvasPoint);
    while (index1 <= index2)
    {
      int index3 = (index1 + index2) / 2;
      if (num1 < xvalues[index3])
        index2 = index3 - 1;
      else if (num1 > xvalues[index3])
        index1 = index3 + 1;
      else if (num1 == xvalues[index3])
        return false;
    }
    if (index2 > -1 && index1 > -1 && index2 < xvalues.Count && index1 < xvalues.Count)
    {
      double point1 = this.ActualXAxis.ValueToPoint(xvalues[index2]);
      double point2 = this.ActualYAxis.ValueToPoint(yvalues[index2]);
      double point3 = this.ActualXAxis.ValueToPoint(xvalues[index1]);
      double point4 = this.ActualYAxis.ValueToPoint(yvalues[index1]);
      if (Math.Abs(point1 - point3) > 2.0)
      {
        double num2 = this.StrokeThickness / 2.0;
        this.polygon.Points = this.GetPolygonPoints(point1, point2, point3, point4, num2, num2);
        if (FastLineBitmapSeries.PointInsidePolygon(this.polygon, adorningCanvasPoint.X, adorningCanvasPoint.Y))
          return true;
      }
      else if (Math.Round(point2) == Math.Round(adorningCanvasPoint.Y) || Math.Round(point4) == Math.Round(adorningCanvasPoint.Y))
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
        if (this.adornmentInfo != null && this.adornmentInfo.HighlightOnSelection)
          this.UpdateAdornmentSelection(newItem);
        if (this.ActualArea != null && this.Segments.Count > 0)
        {
          ChartSelectionChangedEventArgs changedEventArgs = new ChartSelectionChangedEventArgs()
          {
            SelectedSegment = this.Segments[0],
            SelectedSegments = this.Area.SelectedSegments,
            SelectedSeries = (ChartSeriesBase) this,
            SelectedIndex = newItem,
            PreviousSelectedIndex = previousSelectedIndex,
            NewPointInfo = (object) this.GetDataPoint(newItem),
            IsSelected = true
          };
          changedEventArgs.PreviousSelectedSeries = this.ActualArea.PreviousSelectedSeries;
          if (previousSelectedIndex == -1)
            break;
          changedEventArgs.PreviousSelectedSegment = this.Segments[0];
          changedEventArgs.OldPointInfo = (object) this.GetDataPoint(previousSelectedIndex);
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
        ChartSelectionChangedEventArgs eventArgs = new ChartSelectionChangedEventArgs()
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
          eventArgs.PreviousSelectedSegment = this.Segments[0];
          eventArgs.OldPointInfo = (object) this.GetDataPoint(this.PreviousSelectedIndex);
        }
        (this.ActualArea as SfChart).OnSelectionChanged(eventArgs);
        this.OnResetSegment(oldItem);
        this.PreviousSelectedIndex = oldItem;
        break;
    }
  }

  internal override void OnResetSegment(int index)
  {
    if (index < 0 || this.adornmentInfo == null)
      return;
    this.AdornmentPresenter.ResetAdornmentSelection(new int?(index), false);
  }

  internal override ChartDataPointInfo GetDataPoint(Point mousePos)
  {
    List<int> intList = new List<int>();
    IList<double> xvalues = (IList<double>) this.GetXValues();
    int startIndex;
    int endIndex;
    Rect rect;
    this.CalculateHittestRect(mousePos, out startIndex, out endIndex, out rect);
    for (int index = startIndex; index <= endIndex; ++index)
    {
      this.hitPoint.X = this.IsIndexed ? (double) index : xvalues[index];
      this.hitPoint.Y = this.YValues[index];
      if (rect.Contains(this.hitPoint))
        intList.Add(index);
    }
    if (intList.Count <= 0)
      return this.dataPoint;
    int index1 = intList[intList.Count / 2];
    this.dataPoint = new ChartDataPointInfo();
    this.dataPoint.Index = index1;
    this.dataPoint.XData = xvalues[index1];
    this.dataPoint.YData = this.YValues[index1];
    this.dataPoint.Series = (ChartSeriesBase) this;
    if (index1 > -1 && this.ActualData.Count > index1)
      this.dataPoint.Item = this.ActualData[index1];
    return this.dataPoint;
  }

  internal override void UpdateTooltip(object originalSource)
  {
    if (!this.ShowTooltip)
      return;
    ChartDataPointInfo customTag = new ChartDataPointInfo();
    int adornmentIndex = ChartExtensionUtils.GetAdornmentIndex(originalSource);
    if (adornmentIndex <= -1)
      return;
    customTag.Index = adornmentIndex;
    if (this.xValues.Count > adornmentIndex)
      customTag.XData = this.xValues[adornmentIndex];
    if (this.ActualXAxis is CategoryAxis && !(this.ActualXAxis as CategoryAxis).IsIndexed && this.GroupedSeriesYValues[0].Count > adornmentIndex)
      customTag.YData = this.GroupedSeriesYValues[0][adornmentIndex];
    else if (this.YValues.Count > adornmentIndex)
      customTag.YData = this.YValues[adornmentIndex];
    customTag.Series = (ChartSeriesBase) this;
    if (this.ActualData.Count > adornmentIndex)
      customTag.Item = this.ActualData[adornmentIndex];
    this.UpdateSeriesTooltip((object) customTag);
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

  protected internal override void SelectedIndexChanged(int newIndex, int oldIndex)
  {
    if (this.ActualArea != null && this.ActualArea.SelectionBehaviour != null)
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
        if (this.adornmentInfo != null && this.adornmentInfo.HighlightOnSelection)
          this.UpdateAdornmentSelection(newIndex);
        if (this.ActualArea != null && this.Segments.Count > 0)
        {
          ChartSelectionChangedEventArgs changedEventArgs = new ChartSelectionChangedEventArgs()
          {
            SelectedSegment = this.Segments[0],
            SelectedSegments = this.Area.SelectedSegments,
            SelectedSeries = (ChartSeriesBase) this,
            SelectedIndex = newIndex,
            PreviousSelectedIndex = oldIndex,
            NewPointInfo = (object) this.GetDataPoint(newIndex),
            IsSelected = true
          };
          changedEventArgs.PreviousSelectedSeries = this.ActualArea.PreviousSelectedSeries;
          if (oldIndex == -1)
            return;
          changedEventArgs.PreviousSelectedSegment = this.Segments[0];
          changedEventArgs.OldPointInfo = (object) this.GetDataPoint(oldIndex);
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

  protected override ChartSegment CreateSegment() => (ChartSegment) new FastLineBitmapSegment();

  protected override void OnVisibleRangeChanged(VisibleRangeChangedEventArgs e)
  {
    if (this.AdornmentsInfo == null || !this.isAdornmentPending)
      return;
    List<double> doubleList1 = new List<double>();
    List<double> doubleList2 = !(this.ActualXAxis is CategoryAxis) || (this.ActualXAxis as CategoryAxis).IsIndexed ? this.GetXValues() : this.GroupedXValuesIndexes;
    if (doubleList2 != null && this.ActualXAxis != null && !this.ActualXAxis.VisibleRange.IsEmpty)
    {
      double newBase = this.ActualXAxis.IsLogarithmic ? (this.ActualXAxis as LogarithmicAxis).LogarithmicBase : 1.0;
      bool isLogarithmic = this.ActualXAxis.IsLogarithmic;
      double start = this.ActualXAxis.VisibleRange.Start;
      double end = this.ActualXAxis.VisibleRange.End;
      for (int index = 0; index < this.DataCount; ++index)
      {
        double yvalue;
        double num1;
        if (this.ActualXAxis is CategoryAxis && !(this.ActualXAxis as CategoryAxis).IsIndexed && (this.ActualXAxis as CategoryAxis).AggregateFunctions != AggregateFunctions.None)
        {
          if (index >= doubleList2.Count || this.GroupedSeriesYValues[0].Count <= index)
            return;
          yvalue = this.GroupedSeriesYValues[0][index];
          num1 = doubleList2[index];
        }
        else
        {
          num1 = doubleList2[index];
          yvalue = this.YValues[index];
        }
        double num2 = isLogarithmic ? Math.Log(num1, newBase) : num1;
        if (num2 >= start && num2 <= end)
        {
          if (index < this.Adornments.Count)
            this.Adornments[index].SetData(num1, yvalue, num1, yvalue);
          else
            this.Adornments.Add(this.CreateAdornment((AdornmentSeries) this, num1, yvalue, num1, yvalue));
          this.Adornments[index].Item = this.ActualData[index];
        }
      }
    }
    this.isAdornmentPending = false;
  }

  protected override DependencyObject CloneSeries(DependencyObject obj)
  {
    return base.CloneSeries((DependencyObject) new FastLineBitmapSeries()
    {
      EnableAntiAliasing = this.EnableAntiAliasing,
      StrokeDashArray = this.StrokeDashArray,
      SelectedIndex = this.SelectedIndex,
      SegmentSelectionBrush = this.SegmentSelectionBrush
    });
  }

  protected override void OnDataSourceChanged(IEnumerable oldValue, IEnumerable newValue)
  {
    this.Segment = (FastLineBitmapSegment) null;
    base.OnDataSourceChanged(oldValue, newValue);
  }

  protected override void OnMouseMove(MouseEventArgs e)
  {
    Canvas adorningCanvas = this.ActualArea.GetAdorningCanvas();
    this.mousePos = e.GetPosition((IInputElement) adorningCanvas);
    this.UpdateTooltip(e.OriginalSource);
  }

  private static bool PointInsidePolygon(Polygon polygon, double pointX, double pointY)
  {
    bool flag = false;
    PointCollection points = polygon.Points;
    int index1 = 0;
    int index2 = points.Count - 1;
    for (; index1 < points.Count; index2 = index1++)
    {
      if (points[index1].Y > pointY != points[index2].Y > pointY && pointX < (points[index2].X - points[index1].X) * (pointY - points[index1].Y) / (points[index2].Y - points[index1].Y) + points[index1].X)
        flag = !flag;
    }
    return flag;
  }

  private static void OnSegmentSelectionBrush(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    (d as FastLineBitmapSeries).UpdateArea();
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

  private static void OnSeriesPropertyChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    (d as FastLineBitmapSeries).UpdateArea();
  }

  private PointCollection GetPolygonPoints(
    double x1,
    double y1,
    double x2,
    double y2,
    double leftThickness,
    double rightThickness)
  {
    this.polygonPoints.Clear();
    double x3 = x2 - x1;
    double num1 = Math.Atan2(y2 - y1, x3);
    double num2 = Math.Cos(-num1);
    double num3 = Math.Sin(-num1);
    double num4 = x1 * num2 - y1 * num3;
    double num5 = x1 * num3 + y1 * num2;
    double num6 = x2 * num2 - y2 * num3;
    double num7 = x2 * num3 + y2 * num2;
    double num8 = Math.Cos(num1);
    double num9 = Math.Sin(num1);
    double x4 = num4 * num8 - (num5 + leftThickness) * num9;
    double y3 = num4 * num9 + (num5 + leftThickness) * num8;
    double x5 = num6 * num8 - (num7 + leftThickness) * num9;
    double y4 = num6 * num9 + (num7 + leftThickness) * num8;
    double x6 = num4 * num8 - (num5 - rightThickness) * num9;
    double y5 = num4 * num9 + (num5 - rightThickness) * num8;
    double x7 = num6 * num8 - (num7 - rightThickness) * num9;
    double y6 = num6 * num9 + (num7 - rightThickness) * num8;
    this.polygonPoints.Add(new Point(x4, y3));
    this.polygonPoints.Add(new Point(x5, y4));
    this.polygonPoints.Add(new Point(x7, y6));
    this.polygonPoints.Add(new Point(x6, y5));
    this.polygonPoints.Add(new Point(x4, y3));
    return this.polygonPoints;
  }
}
