// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.AccumulationSeriesBase
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
using System.Windows.Media;
using System.Windows.Shapes;

#nullable disable
namespace Syncfusion.UI.Xaml.Charts;

public abstract class AccumulationSeriesBase : AdornmentSeries
{
  public static readonly DependencyProperty YBindingPathProperty = DependencyProperty.Register(nameof (YBindingPath), typeof (string), typeof (AccumulationSeriesBase), new PropertyMetadata((object) null, new PropertyChangedCallback(AccumulationSeriesBase.OnYPathChanged)));
  public static readonly DependencyProperty ExplodeIndexProperty = DependencyProperty.Register(nameof (ExplodeIndex), typeof (int), typeof (AccumulationSeriesBase), new PropertyMetadata((object) -1, new PropertyChangedCallback(AccumulationSeriesBase.OnExplodeIndexChanged)));
  public static readonly DependencyProperty ExplodeAllProperty = DependencyProperty.Register(nameof (ExplodeAll), typeof (bool), typeof (AccumulationSeriesBase), new PropertyMetadata((object) false, new PropertyChangedCallback(AccumulationSeriesBase.OnExplodeAllChanged)));
  public static readonly DependencyProperty ExplodeOnMouseClickProperty = DependencyProperty.Register(nameof (ExplodeOnMouseClick), typeof (bool), typeof (AccumulationSeriesBase), new PropertyMetadata((object) false));
  public static readonly DependencyProperty SegmentSelectionBrushProperty = DependencyProperty.Register(nameof (SegmentSelectionBrush), typeof (Brush), typeof (AccumulationSeriesBase), new PropertyMetadata((object) null, new PropertyChangedCallback(AccumulationSeriesBase.OnSegmentSelectionBrush)));
  public static readonly DependencyProperty SelectedIndexProperty = DependencyProperty.Register(nameof (SelectedIndex), typeof (int), typeof (AccumulationSeriesBase), new PropertyMetadata((object) -1, new PropertyChangedCallback(AccumulationSeriesBase.OnSelectedIndexChanged)));
  private bool allowExplode;
  private ChartSegment mouseUnderSegment;

  public AccumulationSeriesBase() => this.YValues = (IList<double>) new List<double>();

  public string YBindingPath
  {
    get => (string) this.GetValue(AccumulationSeriesBase.YBindingPathProperty);
    set => this.SetValue(AccumulationSeriesBase.YBindingPathProperty, (object) value);
  }

  public int ExplodeIndex
  {
    get => (int) this.GetValue(AccumulationSeriesBase.ExplodeIndexProperty);
    set => this.SetValue(AccumulationSeriesBase.ExplodeIndexProperty, (object) value);
  }

  public bool ExplodeAll
  {
    get => (bool) this.GetValue(AccumulationSeriesBase.ExplodeAllProperty);
    set => this.SetValue(AccumulationSeriesBase.ExplodeAllProperty, (object) value);
  }

  public bool ExplodeOnMouseClick
  {
    get => (bool) this.GetValue(AccumulationSeriesBase.ExplodeOnMouseClickProperty);
    set => this.SetValue(AccumulationSeriesBase.ExplodeOnMouseClickProperty, (object) value);
  }

  public Brush SegmentSelectionBrush
  {
    get => (Brush) this.GetValue(AccumulationSeriesBase.SegmentSelectionBrushProperty);
    set => this.SetValue(AccumulationSeriesBase.SegmentSelectionBrushProperty, (object) value);
  }

  public int SelectedIndex
  {
    get => (int) this.GetValue(AccumulationSeriesBase.SelectedIndexProperty);
    set => this.SetValue(AccumulationSeriesBase.SelectedIndexProperty, (object) value);
  }

  protected internal override ChartSegment SelectedSegment
  {
    get
    {
      int selectedIndex = this.SelectedIndex;
      if (!(this is ISegmentSelectable) || selectedIndex < 0 || selectedIndex >= this.ActualData.Count)
        return (ChartSegment) null;
      CircularSeriesBase circularSeriesBase = this as CircularSeriesBase;
      object item = circularSeriesBase == null || double.IsNaN(circularSeriesBase.GroupTo) ? this.ActualData[selectedIndex] : this.Segments[selectedIndex].Item;
      return this.Segments.FirstOrDefault<ChartSegment>((Func<ChartSegment, bool>) (segments => segments.Item == item));
    }
  }

  protected internal override List<ChartSegment> SelectedSegments
  {
    get
    {
      if (this.SelectedSegmentsIndexes.Count <= 0)
        return (List<ChartSegment>) null;
      List<ChartSegment> selectedSegments = new List<ChartSegment>();
      foreach (int selectedSegmentsIndex in (Collection<int>) this.SelectedSegmentsIndexes)
      {
        CircularSeriesBase circularSeriesBase = this as CircularSeriesBase;
        object item = circularSeriesBase == null || double.IsNaN(circularSeriesBase.GroupTo) ? this.ActualData[selectedSegmentsIndex] : this.Segments[selectedSegmentsIndex].Item;
        ChartSegment chartSegment = this.Segments.Where<ChartSegment>((Func<ChartSegment, bool>) (segment => segment.Item == item)).FirstOrDefault<ChartSegment>();
        if (chartSegment != null)
          selectedSegments.Add(chartSegment);
      }
      return selectedSegments;
    }
  }

  protected internal IList<double> YValues { get; set; }

  internal override void SelectedSegmentsIndexes_CollectionChanged(
    object sender,
    NotifyCollectionChangedEventArgs e)
  {
    ChartSegment chartSegment1 = (ChartSegment) null;
    ChartSegment chartSegment2 = (ChartSegment) null;
    bool flag = this is CircularSeriesBase circularSeriesBase && !double.IsNaN(circularSeriesBase.GroupTo);
    switch (e.Action)
    {
      case NotifyCollectionChangedAction.Add:
        if (e.NewItems == null || this.ActualArea.SelectionBehaviour.SelectionStyle == SelectionStyle.Single)
          break;
        int previousSelectedIndex = this.PreviousSelectedIndex;
        int newItem1 = (int) e.NewItems[0];
        if (previousSelectedIndex != -1 && previousSelectedIndex < this.ActualData.Count)
        {
          object oldItem = flag ? this.Segments[previousSelectedIndex].Item : this.ActualData[previousSelectedIndex];
          chartSegment2 = this.Segments.Where<ChartSegment>((Func<ChartSegment, bool>) (segment => segment.Item == oldItem)).FirstOrDefault<ChartSegment>();
        }
        if (newItem1 < 0 || !this.ActualArea.GetEnableSegmentSelection())
          break;
        if (this.Segments.Count == 0)
        {
          this.triggerSelectionChangedEventOnLoad = true;
          break;
        }
        if (newItem1 < this.Segments.Count || newItem1 < this.ActualData.Count)
        {
          if (this.adornmentInfo is ChartAdornmentInfo && this.adornmentInfo.HighlightOnSelection)
            this.UpdateAdornmentSelection(newItem1);
          if (this is ISegmentSelectable)
          {
            object newItem = flag ? this.Segments[newItem1].Item : this.ActualData[newItem1];
            chartSegment1 = this.Segments.Where<ChartSegment>((Func<ChartSegment, bool>) (segment => segment.Item == newItem)).FirstOrDefault<ChartSegment>();
            if (chartSegment1 != null)
            {
              if (this.SegmentSelectionBrush != null)
                chartSegment1.BindProperties();
              chartSegment1.IsSelectedSegment = true;
            }
          }
        }
        if (newItem1 >= this.Segments.Count && newItem1 >= this.ActualData.Count)
          break;
        ChartSelectionChangedEventArgs eventArgs = new ChartSelectionChangedEventArgs()
        {
          SelectedSegment = chartSegment1,
          SelectedSegments = this.Area.SelectedSegments,
          SelectedSeries = (ChartSeriesBase) this,
          SelectedIndex = newItem1,
          PreviousSelectedIndex = previousSelectedIndex,
          PreviousSelectedSegment = chartSegment2,
          NewPointInfo = chartSegment1?.Item,
          OldPointInfo = chartSegment2?.Item,
          PreviousSelectedSeries = (ChartSeriesBase) null,
          IsSelected = true
        };
        if (this.ActualArea.PreviousSelectedSeries != null && previousSelectedIndex != -1)
          eventArgs.PreviousSelectedSeries = this.ActualArea.PreviousSelectedSeries;
        (this.ActualArea as SfChart).OnSelectionChanged(eventArgs);
        this.PreviousSelectedIndex = newItem1;
        break;
      case NotifyCollectionChangedAction.Remove:
        if (e.OldItems == null || this.ActualArea.SelectionBehaviour.SelectionStyle == SelectionStyle.Single)
          break;
        int oldItem1 = (int) e.OldItems[0];
        if (this.PreviousSelectedIndex != -1 && this.PreviousSelectedIndex < this.ActualData.Count)
        {
          object oldItem = flag ? this.Segments[this.PreviousSelectedIndex].Item : this.ActualData[this.PreviousSelectedIndex];
          chartSegment2 = this.Segments.Where<ChartSegment>((Func<ChartSegment, bool>) (segment => segment.Item == oldItem)).FirstOrDefault<ChartSegment>();
        }
        (this.ActualArea as SfChart).OnSelectionChanged(new ChartSelectionChangedEventArgs()
        {
          SelectedSegment = (ChartSegment) null,
          SelectedSegments = this.Area.SelectedSegments,
          SelectedSeries = (ChartSeriesBase) null,
          SelectedIndex = oldItem1,
          PreviousSelectedIndex = this.PreviousSelectedIndex,
          PreviousSelectedSegment = chartSegment2,
          NewPointInfo = (object) null,
          OldPointInfo = chartSegment2.Item,
          PreviousSelectedSeries = (ChartSeriesBase) this,
          IsSelected = false
        });
        this.PreviousSelectedIndex = oldItem1;
        this.OnResetSegment(oldItem1);
        break;
    }
  }

  internal override void OnResetSegment(int index)
  {
    if (index < 0)
      return;
    CircularSeriesBase circularSeriesBase = this as CircularSeriesBase;
    object item = circularSeriesBase == null || double.IsNaN(circularSeriesBase.GroupTo) ? this.ActualData[index] : this.Segments[index].Item;
    ChartSegment chartSegment = this.Segments.Where<ChartSegment>((Func<ChartSegment, bool>) (segment => segment.Item == item)).FirstOrDefault<ChartSegment>();
    if (chartSegment != null)
    {
      chartSegment.BindProperties();
      chartSegment.IsSelectedSegment = false;
    }
    if (!(this.adornmentInfo is ChartAdornmentInfo))
      return;
    this.AdornmentPresenter.ResetAdornmentSelection(new int?(index), false);
  }

  internal override ChartDataPointInfo GetDataPoint(int index)
  {
    IList<double> doubleList = this.ActualXValues is IList<double> ? this.ActualXValues as IList<double> : (IList<double>) this.GetXValues();
    this.dataPoint = (ChartDataPointInfo) null;
    if (index < doubleList.Count)
    {
      this.dataPoint = new ChartDataPointInfo();
      if (doubleList.Count > index)
        this.dataPoint.XData = this.IsIndexed ? (double) index : doubleList[index];
      if (this.YValues.Count > index)
        this.dataPoint.YData = this.YValues[index];
      this.dataPoint.Index = index;
      this.dataPoint.Series = (ChartSeriesBase) this;
      if (this.ActualData.Count > index)
        this.dataPoint.Item = this.ActualData[index];
    }
    return this.dataPoint;
  }

  internal override int GetDataPointIndex(Point point)
  {
    Canvas adorningCanvas = this.Area.GetAdorningCanvas();
    double num1 = this.Area.ActualWidth - adorningCanvas.ActualWidth;
    double num2 = this.Area.ActualHeight - adorningCanvas.ActualHeight;
    point.X = point.X - num1 + this.Area.Margin.Left;
    point.Y = point.Y - num2 + this.Area.Margin.Top;
    foreach (ChartSegment segment in (Collection<ChartSegment>) this.Segments)
    {
      if (VisualTreeHelper.HitTest((Visual) (segment.GetRenderedVisual() as Path), point) != null)
        return this.Segments.IndexOf(segment);
    }
    return -1;
  }

  internal override void ValidateYValues()
  {
    foreach (double yvalue in (IEnumerable<double>) this.YValues)
    {
      if (double.IsNaN(yvalue) && this.ShowEmptyPoints)
      {
        this.ValidateDataPoints(this.YValues);
        break;
      }
    }
  }

  internal override void ReValidateYValues(List<int>[] emptyPointIndex)
  {
    foreach (List<int> intList in emptyPointIndex)
    {
      foreach (int index in intList)
        this.YValues[index] = double.NaN;
    }
  }

  protected internal override void SelectedIndexChanged(int newIndex, int oldIndex)
  {
    bool flag = this is CircularSeriesBase circularSeriesBase && !double.IsNaN(circularSeriesBase.GroupTo);
    if (this.ActualArea == null || this.ActualArea.SelectionBehaviour == null)
      return;
    ChartSegment chartSegment1 = (ChartSegment) null;
    ChartSegment chartSegment2 = (ChartSegment) null;
    if (this.ActualArea.SelectionBehaviour.SelectionStyle == SelectionStyle.Single)
    {
      if (this.SelectedSegmentsIndexes.Contains(oldIndex))
        this.SelectedSegmentsIndexes.Remove(oldIndex);
      this.OnResetSegment(oldIndex);
    }
    if (this.IsItemsSourceChanged)
      return;
    if (oldIndex != -1 && oldIndex < this.ActualData.Count)
    {
      object oldItem = flag ? this.Segments[oldIndex].Item : this.ActualData[oldIndex];
      chartSegment2 = this.Segments.Where<ChartSegment>((Func<ChartSegment, bool>) (segment => segment.Item == oldItem)).FirstOrDefault<ChartSegment>();
    }
    if (newIndex >= 0 && this.ActualArea.GetEnableSegmentSelection())
    {
      if (!this.SelectedSegmentsIndexes.Contains(newIndex))
        this.SelectedSegmentsIndexes.Add(newIndex);
      if (this.Segments.Count == 0)
      {
        this.triggerSelectionChangedEventOnLoad = true;
      }
      else
      {
        if (newIndex < this.Segments.Count || newIndex < this.ActualData.Count)
        {
          if (this.adornmentInfo is ChartAdornmentInfo && this.adornmentInfo.HighlightOnSelection)
            this.UpdateAdornmentSelection(newIndex);
          if (this is ISegmentSelectable)
          {
            object newItem = flag ? this.Segments[newIndex].Item : this.ActualData[newIndex];
            chartSegment1 = this.Segments.Where<ChartSegment>((Func<ChartSegment, bool>) (segment => segment.Item == newItem)).FirstOrDefault<ChartSegment>();
            if (chartSegment1 != null)
            {
              if (this.SegmentSelectionBrush != null)
                chartSegment1.BindProperties();
              chartSegment1.IsSelectedSegment = true;
            }
          }
        }
        if (newIndex >= this.Segments.Count && newIndex >= this.ActualData.Count)
          return;
        ChartSelectionChangedEventArgs eventArgs = new ChartSelectionChangedEventArgs()
        {
          SelectedSegment = chartSegment1,
          SelectedSegments = this.Area.SelectedSegments,
          SelectedSeries = (ChartSeriesBase) this,
          SelectedIndex = newIndex,
          PreviousSelectedIndex = oldIndex,
          PreviousSelectedSegment = chartSegment2,
          NewPointInfo = chartSegment1?.Item,
          OldPointInfo = chartSegment2?.Item,
          PreviousSelectedSeries = (ChartSeriesBase) null,
          IsSelected = true
        };
        if (this.ActualArea.PreviousSelectedSeries != null && oldIndex != -1)
          eventArgs.PreviousSelectedSeries = this.ActualArea.PreviousSelectedSeries;
        (this.ActualArea as SfChart).OnSelectionChanged(eventArgs);
        this.PreviousSelectedIndex = newIndex;
      }
    }
    else
    {
      if (newIndex != -1)
        return;
      (this.ActualArea as SfChart).OnSelectionChanged(new ChartSelectionChangedEventArgs()
      {
        SelectedSegment = (ChartSegment) null,
        SelectedSegments = this.Area.SelectedSegments,
        SelectedSeries = (ChartSeriesBase) null,
        SelectedIndex = newIndex,
        PreviousSelectedIndex = oldIndex,
        PreviousSelectedSegment = chartSegment2,
        NewPointInfo = (object) null,
        OldPointInfo = chartSegment2.Item,
        PreviousSelectedSeries = (ChartSeriesBase) this,
        IsSelected = false
      });
      this.PreviousSelectedIndex = newIndex;
    }
  }

  protected internal override void GeneratePoints()
  {
    this.GeneratePoints(new string[1]{ this.YBindingPath }, this.YValues);
  }

  protected internal override void OnSeriesMouseUp(object source, Point position)
  {
    ChartSegment tag = source is FrameworkElement frameworkElement ? frameworkElement.Tag as ChartSegment : (ChartSegment) null;
    int num1 = -1;
    if (!this.ExplodeOnMouseClick || !this.allowExplode || this.mouseUnderSegment != tag)
      return;
    if (tag != null && tag.Series is AccumulationSeriesBase)
      num1 = !(tag.Series is CircularSeriesBase) || double.IsNaN(((CircularSeriesBase) tag.Series).GroupTo) ? this.ActualData.IndexOf(tag.Item) : this.Segments.IndexOf(tag);
    else if (this.Adornments.Count > 0)
      num1 = ChartExtensionUtils.GetAdornmentIndex(source);
    int num2 = num1;
    int explodeIndex = this.ExplodeIndex;
    if (num2 != explodeIndex)
      this.ExplodeIndex = num2;
    else if (this.ExplodeIndex >= 0)
      this.ExplodeIndex = -1;
    this.allowExplode = false;
  }

  protected internal override void OnSeriesMouseDown(object source, Point position)
  {
    if (this.GetAnimationIsActive())
      return;
    this.allowExplode = true;
    this.mouseUnderSegment = (source as FrameworkElement).Tag as ChartSegment;
  }

  protected virtual void SetExplodeIndex(int i)
  {
  }

  protected virtual void SetExplodeRadius()
  {
  }

  protected virtual void SetExplodeAll()
  {
  }

  protected override void OnDataSourceChanged(IEnumerable oldValue, IEnumerable newValue)
  {
    base.OnDataSourceChanged(oldValue, newValue);
    if (this.YValues != null)
      this.YValues.Clear();
    if (this.Segments != null)
      this.Segments.Clear();
    this.GeneratePoints(new string[1]{ this.YBindingPath }, this.YValues);
    this.isPointValidated = false;
    if (this.Area != null)
      this.Area.IsUpdateLegend = true;
    this.UpdateArea();
  }

  protected override void OnBindingPathChanged(DependencyPropertyChangedEventArgs args)
  {
    this.YValues.Clear();
    this.Segments.Clear();
    if (this.Area != null)
      this.Area.IsUpdateLegend = true;
    base.OnBindingPathChanged(args);
  }

  protected override DependencyObject CloneSeries(DependencyObject obj)
  {
    if (!(obj is AccumulationSeriesBase accumulationSeriesBase))
      return (DependencyObject) null;
    accumulationSeriesBase.YBindingPath = this.YBindingPath;
    accumulationSeriesBase.ExplodeIndex = this.ExplodeIndex;
    accumulationSeriesBase.ExplodeAll = this.ExplodeAll;
    accumulationSeriesBase.ExplodeOnMouseClick = this.ExplodeOnMouseClick;
    accumulationSeriesBase.SegmentSelectionBrush = this.SegmentSelectionBrush;
    accumulationSeriesBase.SelectedIndex = this.SelectedIndex;
    return base.CloneSeries(obj);
  }

  private static void OnYPathChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    (d as AccumulationSeriesBase).OnBindingPathChanged(e);
  }

  private static void OnSegmentSelectionBrush(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    (d as AccumulationSeriesBase).UpdateArea();
  }

  private static void OnSelectedIndexChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    ChartSeries chartSeries = d as ChartSeries;
    chartSeries.OnPropertyChanged("SelectedIndex");
    if (chartSeries.ActualArea == null || chartSeries.ActualArea.SelectionBehaviour == null)
      return;
    if ((chartSeries.ActualArea as SfChart).SelectionBehaviour.SelectionStyle == SelectionStyle.Single)
      chartSeries.SelectedIndexChanged((int) e.NewValue, (int) e.OldValue);
    else if ((int) e.NewValue != -1)
      chartSeries.SelectedSegmentsIndexes.Add((int) e.NewValue);
    if (!(chartSeries is CircularSeriesBase circularSeriesBase) || double.IsNaN(circularSeriesBase.GroupTo) || chartSeries.ActualArea.Legend == null)
      return;
    chartSeries.ActualArea.UpdateLegend(chartSeries.ActualArea.Legend, false);
  }

  private static void OnExplodeIndexChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    if (d is AccumulationSeriesBase accumulationSeriesBase)
      accumulationSeriesBase.SetExplodeIndex((int) e.NewValue);
    accumulationSeriesBase.OnPropertyChanged("ExplodeIndex");
  }

  private static void OnExplodeAllChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    if (!(d is CircularSeriesBase circularSeriesBase))
      return;
    circularSeriesBase.SetExplodeAll();
  }
}
