// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.FastStepLineBitmapSeries
// Assembly: Syncfusion.SfChart.WPF, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 279D70C6-7936-4F3E-B6CF-79EFEF734826
// Assembly location: C:\Program Files\PDFgear\Syncfusion.SfChart.WPF.dll

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

#nullable disable
namespace Syncfusion.UI.Xaml.Charts;

public class FastStepLineBitmapSeries : XyDataSeries, ISegmentSelectable
{
  public static readonly DependencyProperty EnableAntiAliasingProperty = DependencyProperty.Register(nameof (EnableAntiAliasing), typeof (bool), typeof (FastStepLineBitmapSeries), new PropertyMetadata((object) false));
  public static readonly DependencyProperty SegmentSelectionBrushProperty = DependencyProperty.Register(nameof (SegmentSelectionBrush), typeof (Brush), typeof (FastStepLineBitmapSeries), new PropertyMetadata((object) null, new PropertyChangedCallback(FastStepLineBitmapSeries.OnSegmentSelectionBrush)));
  public static readonly DependencyProperty SelectedIndexProperty = DependencyProperty.Register(nameof (SelectedIndex), typeof (int), typeof (FastStepLineBitmapSeries), new PropertyMetadata((object) -1, new PropertyChangedCallback(FastStepLineBitmapSeries.OnSelectedIndexChanged)));
  private IList<double> xValues;
  private Point hitPoint = new Point();
  private bool isAdornmentPending;

  public bool EnableAntiAliasing
  {
    get => (bool) this.GetValue(FastStepLineBitmapSeries.EnableAntiAliasingProperty);
    set => this.SetValue(FastStepLineBitmapSeries.EnableAntiAliasingProperty, (object) value);
  }

  public Brush SegmentSelectionBrush
  {
    get => (Brush) this.GetValue(FastStepLineBitmapSeries.SegmentSelectionBrushProperty);
    set => this.SetValue(FastStepLineBitmapSeries.SegmentSelectionBrushProperty, (object) value);
  }

  public int SelectedIndex
  {
    get => (int) this.GetValue(FastStepLineBitmapSeries.SelectedIndexProperty);
    set => this.SetValue(FastStepLineBitmapSeries.SelectedIndexProperty, (object) value);
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

  private FastStepLineBitmapSegment Segment { get; set; }

  public override void CreateSegments()
  {
    bool flag = !(this.ActualXAxis is CategoryAxis) || (this.ActualXAxis as CategoryAxis).IsIndexed;
    this.xValues = flag ? (IList<double>) this.GetXValues() : (IList<double>) this.GroupedXValuesIndexes;
    if (!flag)
    {
      this.Segments.Clear();
      this.Adornments.Clear();
      if (this.Segment == null || this.Segments.Count == 0)
        this.CreateSegment(this.xValues, this.GroupedSeriesYValues[0]);
    }
    else
    {
      this.ClearUnUsedAdornments(this.DataCount);
      if (this.Segment == null || this.Segments.Count == 0)
        this.CreateSegment(this.xValues, this.YValues);
      else if (this.ActualXValues != null)
      {
        this.Segment.SetData(this.xValues, this.YValues);
        this.Segment.Item = (object) this.ActualData;
      }
    }
    this.isAdornmentPending = true;
  }

  private void CreateSegment(IList<double> xValues, IList<double> yValues)
  {
    this.Segment = this.CreateSegment() as FastStepLineBitmapSegment;
    if (this.Segment == null)
      return;
    this.Segment.Series = (ChartSeriesBase) this;
    this.Segment.SetData(xValues, yValues);
    this.Segment.Item = (object) this.ActualData;
    this.Segments.Add((ChartSegment) this.Segment);
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
    if (this.ActualXAxis is CategoryAxis && !(this.ActualXAxis as CategoryAxis).IsIndexed)
    {
      if (this.GroupedXValuesIndexes.Count > adornmentIndex)
        customTag.XData = this.GroupedXValuesIndexes[adornmentIndex];
      if (this.GroupedSeriesYValues[0].Count > adornmentIndex)
        customTag.YData = this.GroupedSeriesYValues[0][adornmentIndex];
    }
    else
    {
      if (this.xValues.Count > adornmentIndex)
        customTag.XData = this.xValues[adornmentIndex];
      if (this.YValues.Count > adornmentIndex)
        customTag.YData = this.YValues[adornmentIndex];
    }
    customTag.Series = (ChartSeriesBase) this;
    if (this.ActualData.Count > adornmentIndex)
      customTag.Item = this.ActualData[adornmentIndex];
    this.UpdateSeriesTooltip((object) customTag);
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

  internal override Point GetDataPointPosition(ChartTooltip tooltip)
  {
    ChartDataPointInfo toolTipTag = this.ToolTipTag as ChartDataPointInfo;
    Point dataPointPosition = new Point();
    Point visible = this.ChartTransformer.TransformToVisible(toolTipTag.XData, toolTipTag.YData);
    dataPointPosition.X = visible.X + this.ActualArea.SeriesClipRect.Left;
    dataPointPosition.Y = visible.Y + this.ActualArea.SeriesClipRect.Top;
    return dataPointPosition;
  }

  protected override ChartSegment CreateSegment() => (ChartSegment) new FastStepLineBitmapSegment();

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
    Canvas adorningCanvas = this.ActualArea.GetAdorningCanvas();
    this.mousePos = e.GetPosition((IInputElement) adorningCanvas);
    this.UpdateTooltip(e.OriginalSource);
  }

  protected override void OnVisibleRangeChanged(VisibleRangeChangedEventArgs e)
  {
    if (this.AdornmentsInfo == null || !this.isAdornmentPending)
      return;
    bool flag = this.ActualXAxis is CategoryAxis && !(this.ActualXAxis as CategoryAxis).IsIndexed;
    List<double> doubleList = !flag ? this.GetXValues() : this.GroupedXValuesIndexes;
    if (doubleList != null && this.ActualXAxis != null && !this.ActualXAxis.VisibleRange.IsEmpty)
    {
      double newBase = this.ActualXAxis.IsLogarithmic ? (this.ActualXAxis as LogarithmicAxis).LogarithmicBase : 1.0;
      double start = this.ActualXAxis.VisibleRange.Start;
      double end = this.ActualXAxis.VisibleRange.End;
      if (flag)
      {
        for (int index = 0; index < doubleList.Count; ++index)
        {
          if (index < doubleList.Count)
          {
            double num1 = doubleList[index];
            double num2 = this.ActualXAxis.IsLogarithmic ? Math.Log(num1, newBase) : num1;
            if (num2 >= start && num2 <= end)
            {
              double num3 = this.GroupedSeriesYValues[0][index];
              if (index < this.Adornments.Count)
                this.Adornments[index].SetData(num1, num3, num1, num3);
              else
                this.Adornments.Add(this.CreateAdornment((AdornmentSeries) this, num1, num3, num1, num3));
              this.Adornments[index].Item = this.ActualData[index];
            }
          }
        }
      }
      else
      {
        for (int index = 0; index < this.DataCount; ++index)
        {
          double num4 = doubleList[index];
          double num5 = this.ActualXAxis.IsLogarithmic ? Math.Log(num4, newBase) : num4;
          if (num5 >= start && num5 <= end)
          {
            double yvalue = this.YValues[index];
            if (index < this.Adornments.Count)
              this.Adornments[index].SetData(num4, yvalue, num4, yvalue);
            else
              this.Adornments.Add(this.CreateAdornment((AdornmentSeries) this, num4, yvalue, num4, yvalue));
            this.Adornments[index].Item = this.ActualData[index];
          }
        }
      }
    }
    this.isAdornmentPending = false;
  }

  protected override DependencyObject CloneSeries(DependencyObject obj)
  {
    return base.CloneSeries((DependencyObject) new FastStepLineBitmapSeries()
    {
      EnableAntiAliasing = this.EnableAntiAliasing,
      SegmentSelectionBrush = this.SegmentSelectionBrush,
      SelectedIndex = this.SelectedIndex
    });
  }

  private static void OnSegmentSelectionBrush(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    (d as FastStepLineBitmapSeries).UpdateArea();
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
