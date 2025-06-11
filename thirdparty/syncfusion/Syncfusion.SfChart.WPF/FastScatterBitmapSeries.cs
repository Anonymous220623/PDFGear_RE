// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.FastScatterBitmapSeries
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

public class FastScatterBitmapSeries : XyDataSeries, ISegmentSelectable
{
  public static readonly DependencyProperty ScatterWidthProperty = DependencyProperty.Register(nameof (ScatterWidth), typeof (double), typeof (FastScatterBitmapSeries), new PropertyMetadata((object) 3.0, new PropertyChangedCallback(FastScatterBitmapSeries.OnScatterWidthChanged)));
  public static readonly DependencyProperty ScatterHeightProperty = DependencyProperty.Register(nameof (ScatterHeight), typeof (double), typeof (FastScatterBitmapSeries), new PropertyMetadata((object) 3.0, new PropertyChangedCallback(FastScatterBitmapSeries.OnScatterHeightChanged)));
  public static readonly DependencyProperty SegmentSelectionBrushProperty = DependencyProperty.Register(nameof (SegmentSelectionBrush), typeof (Brush), typeof (FastScatterBitmapSeries), new PropertyMetadata((object) null, new PropertyChangedCallback(FastScatterBitmapSeries.OnSegmentSelectionBrush)));
  public static readonly DependencyProperty SelectedIndexProperty = DependencyProperty.Register(nameof (SelectedIndex), typeof (int), typeof (FastScatterBitmapSeries), new PropertyMetadata((object) -1, new PropertyChangedCallback(FastScatterBitmapSeries.OnSelectedIndexChanged)));
  public static readonly DependencyProperty ShapeTypeProperty = DependencyProperty.Register(nameof (ShapeType), typeof (ChartSymbol), typeof (FastScatterBitmapSeries), new PropertyMetadata((object) ChartSymbol.Ellipse, new PropertyChangedCallback(FastScatterBitmapSeries.OnShapeTypePropertyChanged)));
  private IList<double> xValues;
  private Point hitPoint = new Point();
  private Point startPoint = new Point();
  private Point endPoint = new Point();
  private bool isAdornmentsBending;

  public double ScatterWidth
  {
    get => (double) this.GetValue(FastScatterBitmapSeries.ScatterWidthProperty);
    set => this.SetValue(FastScatterBitmapSeries.ScatterWidthProperty, (object) value);
  }

  public double ScatterHeight
  {
    get => (double) this.GetValue(FastScatterBitmapSeries.ScatterHeightProperty);
    set => this.SetValue(FastScatterBitmapSeries.ScatterHeightProperty, (object) value);
  }

  public Brush SegmentSelectionBrush
  {
    get => (Brush) this.GetValue(FastScatterBitmapSeries.SegmentSelectionBrushProperty);
    set => this.SetValue(FastScatterBitmapSeries.SegmentSelectionBrushProperty, (object) value);
  }

  public int SelectedIndex
  {
    get => (int) this.GetValue(FastScatterBitmapSeries.SelectedIndexProperty);
    set => this.SetValue(FastScatterBitmapSeries.SelectedIndexProperty, (object) value);
  }

  public ChartSymbol ShapeType
  {
    get => (ChartSymbol) this.GetValue(FastScatterBitmapSeries.ShapeTypeProperty);
    set => this.SetValue(FastScatterBitmapSeries.ShapeTypeProperty, (object) value);
  }

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

  private FastScatterBitmapSegment Segment { get; set; }

  public override void CreateSegments()
  {
    bool flag = this.ActualXAxis is CategoryAxis && !(this.ActualXAxis as CategoryAxis).IsIndexed;
    this.xValues = !flag ? (IList<double>) this.GetXValues() : (IList<double>) this.GroupedXValuesIndexes;
    if (flag)
    {
      this.Segments.Clear();
      this.Adornments.Clear();
      if (this.Segments == null || this.Segments.Count == 0)
      {
        this.Segment = this.CreateSegment() as FastScatterBitmapSegment;
        if (this.Segment != null)
        {
          this.Segment.Series = (ChartSeriesBase) this;
          this.Segment.Item = (object) this.ActualData;
          this.Segment.SetData(this.xValues, this.GroupedSeriesYValues[0]);
          this.Segments.Add((ChartSegment) this.Segment);
        }
      }
    }
    else
    {
      this.ClearUnUsedAdornments(this.DataCount);
      if (this.Segments == null || this.Segments.Count == 0)
      {
        this.Segment = this.CreateSegment() as FastScatterBitmapSegment;
        if (this.Segment != null)
        {
          this.Segment.Series = (ChartSeriesBase) this;
          this.Segment.Item = (object) this.ActualData;
          this.Segment.SetData(this.xValues, this.YValues);
          this.Segments.Add((ChartSegment) this.Segment);
        }
      }
      else if (this.ActualXValues != null)
      {
        this.Segment.SetData(this.xValues, this.YValues);
        this.Segment.Item = (object) this.ActualData;
      }
    }
    this.isAdornmentsBending = true;
  }

  internal override bool IsHitTestSeries()
  {
    return this.GetDataPoint(this.Area.adorningCanvasPoint) != null;
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
    double xdata = this.dataPoint.XData;
    double ydata = this.dataPoint.YData;
    int index = this.dataPoint.Index;
    double num1;
    double num2;
    if (this.IsIndexed)
    {
      bool flag = this.ActualXAxis is CategoryAxis && !(this.ActualXAxis as CategoryAxis).IsIndexed;
      if (!this.IsActualTransposed)
      {
        double x = flag ? xdata : (double) index;
        double y = ydata;
        Point visible = transformer.TransformToVisible(x, y);
        num1 = visible.X;
        num2 = visible.Y;
      }
      else
      {
        double x = flag ? xdata : (double) index;
        double y = ydata;
        Point visible = transformer.TransformToVisible(x, y);
        num1 = visible.Y;
        num2 = visible.X;
      }
    }
    else if (!this.IsActualTransposed)
    {
      double x = xdata;
      double y = ydata;
      Point visible = transformer.TransformToVisible(x, y);
      num1 = visible.X;
      num2 = visible.Y;
    }
    else
    {
      double x = xdata;
      double y = ydata;
      Point visible = transformer.TransformToVisible(x, y);
      num1 = visible.Y;
      num2 = visible.X;
    }
    double scatterHeight = this.ScatterHeight;
    double scatterWidth = this.ScatterWidth;
    this.selectedSegmentPixels.Clear();
    if (this.IsActualTransposed)
    {
      if (num2 <= -1.0)
        return;
      this.selectedSegmentPixels = fastRenderSurface.GetEllipseCentered((int) num2, (int) num1, (int) scatterHeight, (int) scatterWidth, this.selectedSegmentPixels);
    }
    else
    {
      if (num2 <= -1.0)
        return;
      this.selectedSegmentPixels = fastRenderSurface.GetEllipseCentered((int) num1, (int) num2, (int) scatterHeight, (int) scatterWidth, this.selectedSegmentPixels);
    }
  }

  internal override ChartDataPointInfo GetDataPoint(Point mousePos)
  {
    bool flag = this.ActualXAxis is CategoryAxis && !(this.ActualXAxis as CategoryAxis).IsIndexed;
    this.xValues = !flag ? (this.ActualXValues is IList<double> ? this.ActualXValues as IList<double> : (IList<double>) this.GetXValues()) : (IList<double>) this.GroupedXValuesIndexes;
    this.hitPoint.X = mousePos.X - this.Area.SeriesClipRect.Left;
    this.hitPoint.Y = mousePos.Y - this.Area.SeriesClipRect.Top;
    this.hitPoint.X -= this.ScatterWidth / 2.0;
    this.hitPoint.Y -= this.ScatterHeight / 2.0;
    double num1 = this.ActualArea.PointToValue(this.ActualXAxis, this.hitPoint);
    double num2 = this.ActualArea.PointToValue(this.ActualYAxis, this.hitPoint);
    this.startPoint.X = num1;
    this.startPoint.Y = num2;
    this.hitPoint.X += this.ScatterWidth;
    this.hitPoint.Y += this.ScatterHeight;
    double num3 = this.ActualArea.PointToValue(this.ActualXAxis, this.hitPoint);
    double num4 = this.ActualArea.PointToValue(this.ActualYAxis, this.hitPoint);
    this.endPoint.X = num3;
    this.endPoint.Y = num4;
    Rect rect = new Rect(this.startPoint, this.endPoint);
    this.dataPoint = (ChartDataPointInfo) null;
    for (int index = 0; index < this.YValues.Count; ++index)
    {
      if (flag)
      {
        if (index >= this.xValues.Count)
          return this.dataPoint;
        this.hitPoint.X = this.xValues[index];
        this.hitPoint.Y = this.GroupedSeriesYValues[0][index];
      }
      else
      {
        this.hitPoint.X = this.IsIndexed ? (double) index : this.xValues[index];
        this.hitPoint.Y = this.YValues[index];
      }
      if (num1 <= this.xValues[index] && this.xValues[index] <= num3 && num2 >= this.YValues[index] && this.YValues[index] >= num4)
      {
        this.dataPoint = new ChartDataPointInfo();
        this.dataPoint.Index = index;
        this.dataPoint.XData = this.xValues[index];
        this.dataPoint.YData = flag ? this.GroupedSeriesYValues[0][index] : this.YValues[index];
        this.dataPoint.Series = (ChartSeriesBase) this;
        if (index > -1 && this.ActualData.Count > index)
        {
          this.dataPoint.Item = this.ActualData[index];
          break;
        }
        break;
      }
    }
    return this.dataPoint;
  }

  internal override Point GetDataPointPosition(ChartTooltip tooltip)
  {
    ChartDataPointInfo toolTipTag = this.ToolTipTag as ChartDataPointInfo;
    Point dataPointPosition = new Point();
    Point visible = this.ChartTransformer.TransformToVisible(toolTipTag.XData, toolTipTag.YData);
    dataPointPosition.X = visible.X + this.ActualArea.SeriesClipRect.Left;
    dataPointPosition.Y = visible.Y + this.ActualArea.SeriesClipRect.Top - this.ScatterHeight / 2.0;
    if (dataPointPosition.Y - tooltip.DesiredSize.Height < this.ActualArea.SeriesClipRect.Top)
      dataPointPosition.Y += this.ScatterHeight;
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

  protected override ChartSegment CreateSegment() => (ChartSegment) new FastScatterBitmapSegment();

  protected override void OnVisibleRangeChanged(VisibleRangeChangedEventArgs e)
  {
    bool flag = this.ActualXAxis is CategoryAxis && !(this.ActualXAxis as CategoryAxis).IsIndexed;
    if (this.AdornmentsInfo == null || !this.isAdornmentsBending)
      return;
    List<double> doubleList = !flag ? this.GetXValues() : this.GroupedXValuesIndexes;
    if (doubleList != null && this.ActualXAxis != null && !this.ActualXAxis.VisibleRange.IsEmpty)
    {
      for (int index = 0; index < this.DataCount; ++index)
      {
        if (flag)
        {
          if (index >= doubleList.Count)
            return;
          this.AddAdornments(doubleList[index], this.GroupedSeriesYValues[0][index], index);
        }
        else
          this.AddAdornments(doubleList[index], this.YValues[index], index);
      }
    }
    this.isAdornmentsBending = false;
  }

  protected override DependencyObject CloneSeries(DependencyObject obj)
  {
    return base.CloneSeries((DependencyObject) new FastScatterBitmapSeries()
    {
      ScatterHeight = this.ScatterHeight,
      ScatterWidth = this.ScatterWidth,
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
    customTag.Index = adornmentIndex;
    if (this.ActualXAxis is CategoryAxis && !(this.ActualXAxis as CategoryAxis).IsIndexed && this.GroupedXValuesIndexes.Count > adornmentIndex)
    {
      customTag.XData = this.GroupedXValuesIndexes[adornmentIndex];
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

  private static void OnSegmentSelectionBrush(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    (d as FastScatterBitmapSeries).UpdateArea();
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

  private static void OnScatterWidthChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    if (!(d is FastScatterBitmapSeries scatterBitmapSeries))
      return;
    scatterBitmapSeries.UpdateArea();
  }

  private static void OnScatterHeightChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    if (!(d is FastScatterBitmapSeries scatterBitmapSeries))
      return;
    scatterBitmapSeries.UpdateArea();
  }

  private static void OnShapeTypePropertyChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    if (!(d is FastScatterBitmapSeries scatterBitmapSeries))
      return;
    if (scatterBitmapSeries.ShapeType == ChartSymbol.Custom || scatterBitmapSeries.ShapeType == ChartSymbol.HorizontalLine || scatterBitmapSeries.ShapeType == ChartSymbol.VerticalLine)
      scatterBitmapSeries.ShapeType = ChartSymbol.Ellipse;
    if (scatterBitmapSeries.LegendIcon == ChartLegendIcon.SeriesType)
      scatterBitmapSeries.UpdateLegendIconTemplate(true);
    scatterBitmapSeries.UpdateArea();
  }

  private void AddAdornments(double x, double yValue, int i)
  {
    double num1 = x;
    double num2 = yValue;
    if (i < this.Adornments.Count)
      this.Adornments[i].SetData(num1, num2, num1, num2);
    else
      this.Adornments.Add(this.CreateAdornment((AdornmentSeries) this, num1, num2, num1, num2));
    this.Adornments[i].Item = this.ActualData[i];
  }
}
