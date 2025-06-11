// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.CartesianSeries
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

#nullable disable
namespace Syncfusion.UI.Xaml.Charts;

public abstract class CartesianSeries : AdornmentSeries, ISupportAxes2D, ISupportAxes
{
  public static readonly DependencyProperty TrendlinesProperty = DependencyProperty.Register(nameof (Trendlines), typeof (ChartTrendLineCollection), typeof (CartesianSeries), new PropertyMetadata((object) null, new PropertyChangedCallback(CartesianSeries.OnTrendlinesChanged)));
  public static readonly DependencyProperty XAxisProperty = DependencyProperty.Register(nameof (XAxis), typeof (ChartAxisBase2D), typeof (CartesianSeries), new PropertyMetadata((object) null, new PropertyChangedCallback(CartesianSeries.OnXAxisChanged)));
  public static readonly DependencyProperty YAxisProperty = DependencyProperty.Register(nameof (YAxis), typeof (RangeAxisBase), typeof (CartesianSeries), new PropertyMetadata((object) null, new PropertyChangedCallback(CartesianSeries.OnYAxisChanged)));
  public static readonly DependencyProperty IsTransposedProperty = DependencyProperty.Register(nameof (IsTransposed), typeof (bool), typeof (CartesianSeries), new PropertyMetadata((object) false, new PropertyChangedCallback(CartesianSeries.OnTransposeChanged)));
  public static readonly DependencyProperty ShowTrackballInfoProperty = DependencyProperty.Register(nameof (ShowTrackballInfo), typeof (bool), typeof (CartesianSeries), new PropertyMetadata((object) true));

  public CartesianSeries() => this.Trendlines = new ChartTrendLineCollection();

  public ChartTrendLineCollection Trendlines
  {
    get => (ChartTrendLineCollection) this.GetValue(CartesianSeries.TrendlinesProperty);
    set => this.SetValue(CartesianSeries.TrendlinesProperty, (object) value);
  }

  public DoubleRange XRange { get; internal set; }

  public DoubleRange YRange { get; internal set; }

  public ChartAxisBase2D XAxis
  {
    get => (ChartAxisBase2D) this.GetValue(CartesianSeries.XAxisProperty);
    set => this.SetValue(CartesianSeries.XAxisProperty, (object) value);
  }

  public RangeAxisBase YAxis
  {
    get => (RangeAxisBase) this.GetValue(CartesianSeries.YAxisProperty);
    set => this.SetValue(CartesianSeries.YAxisProperty, (object) value);
  }

  public bool IsTransposed
  {
    get => (bool) this.GetValue(CartesianSeries.IsTransposedProperty);
    set => this.SetValue(CartesianSeries.IsTransposedProperty, (object) value);
  }

  public bool ShowTrackballInfo
  {
    get => (bool) this.GetValue(CartesianSeries.ShowTrackballInfoProperty);
    set => this.SetValue(CartesianSeries.ShowTrackballInfoProperty, (object) value);
  }

  ChartAxis ISupportAxes.ActualXAxis => this.ActualXAxis;

  ChartAxis ISupportAxes.ActualYAxis => this.ActualYAxis;

  internal virtual void CreateTrendline()
  {
    if (this.Trendlines == null)
      return;
    foreach (Trendline trendline in (Collection<Trendline>) this.Trendlines)
    {
      if (this.IsSeriesVisible && trendline.IsTrendlineVisible)
      {
        trendline.ApplyTemplate();
        trendline.UpdateElements();
      }
    }
  }

  internal override void OnTransposeChanged(bool val) => this.IsActualTransposed = val;

  internal override void UpdateRange()
  {
    this.XRange = DoubleRange.Empty;
    this.YRange = DoubleRange.Empty;
    if (this.Segments == null)
      return;
    if (this.Segments.Count > 0)
    {
      foreach (ChartSegment segment in (Collection<ChartSegment>) this.Segments)
      {
        this.XRange += segment.XRange;
        this.YRange += segment.YRange;
      }
      if (this.IsSideBySide)
      {
        DoubleRange sideInfoRangePad = this.SideBySideInfoRangePad;
        if (!this.SideBySideInfoRangePad.IsEmpty)
          this.XRange = this.ActualXAxis is NumericalAxis && (this.ActualXAxis as NumericalAxis).RangePadding == NumericalPadding.None || this.ActualXAxis is DateTimeAxis && (this.ActualXAxis as DateTimeAxis).RangePadding == DateTimeRangePadding.None ? new DoubleRange(this.XRange.Start - this.SideBySideInfoRangePad.Start, this.XRange.End - this.SideBySideInfoRangePad.End) : new DoubleRange(this.XRange.Start + this.SideBySideInfoRangePad.Start, this.XRange.End + this.SideBySideInfoRangePad.End);
      }
    }
    else if (this.DataCount == 1)
    {
      List<double> xvalues = this.GetXValues();
      List<double> actualSeriesYvalue = this.ActualSeriesYValues[0] as List<double>;
      if (xvalues.Count > 0 && actualSeriesYvalue.Count > 0)
      {
        double num1 = xvalues[0];
        double num2 = actualSeriesYvalue[0];
        this.XRange = new DoubleRange(num1 - 1.0, num1 + 1.0);
        this.YRange = new DoubleRange(num2 - 1.0, num2 + 1.0);
      }
    }
    foreach (TrendlineBase trendlineBase in this.Trendlines.Where<Trendline>((Func<Trendline, bool>) (line => line.Visibility == Visibility.Visible)))
    {
      foreach (ChartSegment trendlineSegment in (Collection<ChartSegment>) trendlineBase.TrendlineSegments)
      {
        this.XRange += trendlineSegment.XRange;
        this.YRange += trendlineSegment.YRange;
      }
    }
  }

  internal override void UpdateOnSeriesBoundChanged(Size size)
  {
    base.UpdateOnSeriesBoundChanged(size);
    if (this.Segments == null)
      return;
    foreach (Trendline trendline in (Collection<Trendline>) this.Trendlines)
    {
      if (trendline.TrendlinePanel != null)
      {
        foreach (ChartSegment trendlineSegment in (Collection<ChartSegment>) trendline.TrendlineSegments)
          trendlineSegment.OnSizeChanged(size);
        trendline.TrendlinePanel.Update(size);
      }
    }
  }

  internal override void CalculateSegments()
  {
    base.CalculateSegments();
    this.CreateTrendline();
  }

  internal override int GetDataPointIndex(Point point)
  {
    Canvas adorningCanvas = this.Area.GetAdorningCanvas();
    double num1 = this.Area.ActualWidth - adorningCanvas.ActualWidth;
    double num2 = this.Area.ActualHeight - adorningCanvas.ActualHeight;
    point.X = point.X - num1 - this.Area.SeriesClipRect.Left + this.Area.Margin.Left;
    point.Y = point.Y - num2 - this.Area.SeriesClipRect.Top + this.Area.Margin.Top;
    double x;
    this.FindNearestChartPoint(point, out x, out double _, out double _);
    return this.GetXValues().IndexOf(x);
  }

  internal override void Dispose()
  {
    this.Trendlines = (ChartTrendLineCollection) null;
    base.Dispose();
  }

  internal void OnVisibleRangeChanged(object sender, VisibleRangeChangedEventArgs e)
  {
    this.OnVisibleRangeChanged(e);
  }

  protected virtual void OnVisibleRangeChanged(VisibleRangeChangedEventArgs e)
  {
  }

  protected virtual void OnYAxisChanged(ChartAxis oldAxis, ChartAxis newAxis)
  {
    if (newAxis != null)
      newAxis.IsSecondaryAxis = true;
    if (this.XAxis != null && this.Area != null && this.Area.InternalSecondaryAxis != null && this.Area.InternalSecondaryAxis.AssociatedAxes != null && this.Area.InternalSecondaryAxis.AssociatedAxes.Contains((ChartAxis) this.XAxis))
    {
      this.Area.InternalSecondaryAxis.AssociatedAxes.Remove((ChartAxis) this.XAxis);
      if (this.XAxis.AssociatedAxes.Contains(this.Area.InternalSecondaryAxis))
        this.XAxis.AssociatedAxes.Remove(this.Area.InternalSecondaryAxis);
    }
    if (oldAxis != null && oldAxis.RegisteredSeries != null)
    {
      if (oldAxis is NumericalAxis numericalAxis && numericalAxis.AxisScaleBreaks.Count > 0)
        numericalAxis.ClearBreakElements();
      oldAxis.RegisteredSeries.Remove((ISupportAxes) this);
      if (oldAxis.RegisteredSeries.Count == 0 && this.Area != null && this.Area.Axes.Contains(oldAxis) && this.Area.PrimaryAxis != oldAxis && this.Area.SecondaryAxis != oldAxis)
      {
        this.Area.Axes.RemoveItem(oldAxis, this.Area.DependentSeriesAxes.Contains(oldAxis));
        this.Area.DependentSeriesAxes.Remove(oldAxis);
      }
    }
    else if (this.ActualArea != null && this.ActualArea.InternalSecondaryAxis != null && this.ActualArea.InternalSecondaryAxis.RegisteredSeries.Contains((ISupportAxes) this))
      this.ActualArea.InternalSecondaryAxis.RegisteredSeries.Remove((ISupportAxes) this);
    if (newAxis != null && this.Area != null)
    {
      if (!this.Area.Axes.Contains(newAxis))
      {
        this.Area.Axes.Add(newAxis);
        this.Area.DependentSeriesAxes.Add(newAxis);
        newAxis.Area = (ChartBase) this.Area;
      }
      if (!newAxis.RegisteredSeries.Contains((ISupportAxes) this))
        newAxis.RegisteredSeries.Add((ISupportAxes) this);
    }
    if (this.Area != null)
      this.Area.ScheduleUpdate();
    if (newAxis == null)
      return;
    newAxis.Orientation = this.IsActualTransposed ? Orientation.Horizontal : Orientation.Vertical;
  }

  protected virtual void OnXAxisChanged(ChartAxis oldAxis, ChartAxis newAxis)
  {
    if (this.YAxis != null && this.Area != null && this.Area.InternalPrimaryAxis != null && this.Area.InternalPrimaryAxis.AssociatedAxes != null && this.Area.InternalPrimaryAxis.AssociatedAxes.Contains((ChartAxis) this.YAxis))
    {
      this.Area.InternalPrimaryAxis.AssociatedAxes.Remove((ChartAxis) this.YAxis);
      if (this.YAxis.AssociatedAxes.Contains(this.ActualArea.InternalPrimaryAxis))
        this.YAxis.AssociatedAxes.Remove(this.ActualArea.InternalPrimaryAxis);
    }
    if (oldAxis != null && oldAxis.RegisteredSeries != null)
    {
      oldAxis.VisibleRangeChanged -= new EventHandler<VisibleRangeChangedEventArgs>(this.OnVisibleRangeChanged);
      oldAxis.RegisteredSeries.Remove((ISupportAxes) this);
      if (oldAxis.RegisteredSeries.Count == 0 && this.Area != null && this.Area.Axes.Contains(oldAxis) && this.Area.PrimaryAxis != oldAxis && this.Area.SecondaryAxis != oldAxis)
      {
        this.Area.Axes.RemoveItem(oldAxis, this.Area.DependentSeriesAxes.Contains(oldAxis));
        this.Area.DependentSeriesAxes.Remove(oldAxis);
      }
    }
    else if (this.ActualArea != null && this.ActualArea.InternalPrimaryAxis != null && this.ActualArea.InternalPrimaryAxis.RegisteredSeries.Contains((ISupportAxes) this))
      this.ActualArea.InternalPrimaryAxis.RegisteredSeries.Remove((ISupportAxes) this);
    if (newAxis != null)
    {
      if (this.Area != null)
      {
        if (!this.Area.Axes.Contains(newAxis) && newAxis != this.Area.InternalPrimaryAxis)
        {
          this.Area.Axes.Add(newAxis);
          this.Area.DependentSeriesAxes.Add(newAxis);
          newAxis.Area = (ChartBase) this.Area;
        }
        if (!newAxis.RegisteredSeries.Contains((ISupportAxes) this))
          newAxis.RegisteredSeries.Add((ISupportAxes) this);
      }
      newAxis.VisibleRangeChanged += new EventHandler<VisibleRangeChangedEventArgs>(this.OnVisibleRangeChanged);
    }
    if (this.Area != null)
    {
      this.Area.SBSInfoCalculated = false;
      this.Area.ScheduleUpdate();
    }
    if (newAxis == null)
      return;
    newAxis.Orientation = this.IsActualTransposed ? Orientation.Vertical : Orientation.Horizontal;
  }

  protected override DependencyObject CloneSeries(DependencyObject obj)
  {
    CartesianSeries cartesianSeries1 = this;
    CartesianSeries cartesianSeries2 = obj as CartesianSeries;
    if (this.XAxis != null && cartesianSeries1.XAxis != this.Area.InternalPrimaryAxis)
      cartesianSeries2.XAxis = (ChartAxisBase2D) cartesianSeries1.XAxis.Clone();
    if (cartesianSeries1.YAxis != null && cartesianSeries1.YAxis != this.Area.InternalSecondaryAxis)
      cartesianSeries2.YAxis = (RangeAxisBase) cartesianSeries1.YAxis.Clone();
    cartesianSeries2.IsTransposed = this.IsTransposed;
    foreach (Trendline trendline in (Collection<Trendline>) this.Trendlines)
      cartesianSeries2.Trendlines.Add((Trendline) trendline.Clone());
    return base.CloneSeries(obj);
  }

  public override void OnApplyTemplate()
  {
    base.OnApplyTemplate();
    foreach (Trendline trendline in (Collection<Trendline>) this.Trendlines)
    {
      if (!this.SeriesPanel.Children.Contains((UIElement) trendline))
      {
        trendline.Series = (ChartSeries) this;
        trendline.UpdateLegendIconTemplate(true);
        this.SeriesPanel.Children.Add((UIElement) trendline);
        Panel.SetZIndex((UIElement) trendline, 1);
      }
    }
  }

  private static void OnTransposeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    (d as CartesianSeries).OnTransposeChanged(Convert.ToBoolean(e.NewValue));
  }

  private static void OnYAxisChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    (d as CartesianSeries).OnYAxisChanged(e.OldValue as ChartAxis, e.NewValue as ChartAxis);
  }

  private static void OnXAxisChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    (d as CartesianSeries).OnXAxisChanged(e.OldValue as ChartAxis, e.NewValue as ChartAxis);
  }

  private static void OnTrendlinesChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    if (!(d is CartesianSeries cartesianSeries))
      return;
    cartesianSeries.OnTrendlinesChanged(e);
  }

  private void OnTrendlinesChanged(DependencyPropertyChangedEventArgs e)
  {
    if (e.OldValue != null)
    {
      if (this.SeriesPanel != null)
      {
        foreach (Trendline element in (Collection<Trendline>) (e.OldValue as ChartTrendLineCollection))
        {
          if (this.SeriesPanel.Children.Contains((UIElement) element))
            this.SeriesPanel.Children.Remove((UIElement) element);
        }
      }
      (e.OldValue as ChartTrendLineCollection).Clear();
      (e.OldValue as ChartTrendLineCollection).CollectionChanged -= new NotifyCollectionChangedEventHandler(this.Trendlines_CollectionChanged);
    }
    if (e.NewValue != null)
    {
      foreach (Trendline trendline in (Collection<Trendline>) (e.NewValue as ChartTrendLineCollection))
      {
        if (trendline != null)
          trendline.Series = (ChartSeries) this;
      }
      this.Trendlines.CollectionChanged += new NotifyCollectionChangedEventHandler(this.Trendlines_CollectionChanged);
    }
    if (this.Area == null)
      return;
    this.Area.ScheduleUpdate();
  }

  private void Trendlines_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
  {
    if (e.Action == NotifyCollectionChangedAction.Add)
    {
      if (e.NewItems[0] is TrendlineBase newItem)
      {
        newItem.Series = (ChartSeries) this;
        if (this.SeriesPanel != null && this.Area != null)
        {
          newItem.UpdateLegendIconTemplate(true);
          this.SeriesPanel.Children.Add((UIElement) newItem);
          this.Area.UpdateLegend(this.Area.Legend, false);
        }
      }
    }
    else if (e.Action == NotifyCollectionChangedAction.Remove)
    {
      TrendlineBase trend = e.OldItems[0] as TrendlineBase;
      if (trend != null && this.SeriesPanel != null && this.SeriesPanel.Children.Contains((UIElement) trend))
      {
        if (this.Area.Legend != null && this.Area.LegendItems != null)
        {
          List<ObservableCollection<LegendItem>> list1 = this.Area.LegendItems.Where<ObservableCollection<LegendItem>>((Func<ObservableCollection<LegendItem>, bool>) (item => item.Where<LegendItem>((Func<LegendItem, bool>) (it => it.Series == this)).Count<LegendItem>() > 0)).ToList<ObservableCollection<LegendItem>>();
          if (list1.Count > 0)
          {
            int index = this.Area.LegendItems.IndexOf(list1[0]);
            List<LegendItem> list2 = this.Area.LegendItems[index].Where<LegendItem>((Func<LegendItem, bool>) (it => it.Trendline == trend)).ToList<LegendItem>();
            if (list2.Count<LegendItem>() > 0 && this.Area.LegendItems[index].Contains(list2[0]))
              this.Area.LegendItems[index].Remove(list2[0]);
          }
        }
        this.SeriesPanel.Children.Remove((UIElement) trend);
      }
    }
    else if (e.Action == NotifyCollectionChangedAction.Reset)
    {
      if (this.SeriesPanel != null)
      {
        UIElement[] uiElementArray = new UIElement[this.SeriesPanel.Children.Count];
        this.SeriesPanel.Children.CopyTo(uiElementArray, 0);
        foreach (UIElement uiElement in ((IEnumerable<UIElement>) uiElementArray).Where<UIElement>((Func<UIElement, bool>) (it => it is TrendlineBase)))
        {
          UIElement trend = uiElement;
          if (this.SeriesPanel.Children.Contains(trend))
          {
            if (this.Area.Legend != null && this.Area.LegendItems != null)
            {
              List<ObservableCollection<LegendItem>> list3 = this.Area.LegendItems.Where<ObservableCollection<LegendItem>>((Func<ObservableCollection<LegendItem>, bool>) (item => item.Where<LegendItem>((Func<LegendItem, bool>) (it => it.Series == this)).Count<LegendItem>() > 0)).ToList<ObservableCollection<LegendItem>>();
              if (list3.Count > 0)
              {
                int index = this.Area.LegendItems.IndexOf(list3[0]);
                List<LegendItem> list4 = this.Area.LegendItems[index].Where<LegendItem>((Func<LegendItem, bool>) (it => it.Trendline == trend)).ToList<LegendItem>();
                if (list4.Count<LegendItem>() > 0 && this.Area.LegendItems[index].Contains(list4[0]))
                  this.Area.LegendItems[index].Remove(list4[0]);
              }
            }
            this.SeriesPanel.Children.Remove(trend);
          }
        }
      }
    }
    else if (e.Action == NotifyCollectionChangedAction.Replace)
    {
      TrendlineBase trend = e.OldItems[0] as TrendlineBase;
      if (trend != null && this.SeriesPanel != null && this.SeriesPanel.Children.Contains((UIElement) trend))
      {
        if (this.Area.Legend != null && this.Area.LegendItems != null)
        {
          List<ObservableCollection<LegendItem>> list5 = this.Area.LegendItems.Where<ObservableCollection<LegendItem>>((Func<ObservableCollection<LegendItem>, bool>) (item => item.Where<LegendItem>((Func<LegendItem, bool>) (it => it.Series == this)).Count<LegendItem>() > 0)).ToList<ObservableCollection<LegendItem>>();
          if (list5.Count > 0)
          {
            int index = this.Area.LegendItems.IndexOf(list5[0]);
            List<LegendItem> list6 = this.Area.LegendItems[index].Where<LegendItem>((Func<LegendItem, bool>) (it => it.Trendline == trend)).ToList<LegendItem>();
            if (list6.Count<LegendItem>() > 0 && this.Area.LegendItems[index].Contains(list6[0]))
              this.Area.LegendItems[index].Remove(list6[0]);
          }
        }
        this.SeriesPanel.Children.Remove((UIElement) trend);
      }
      trend = e.NewItems[0] as TrendlineBase;
      if (trend != null)
      {
        trend.Series = (ChartSeries) this;
        if (this.SeriesPanel != null)
        {
          trend.UpdateLegendIconTemplate(true);
          this.SeriesPanel.Children.Add((UIElement) trend);
          this.Area.UpdateLegend(this.Area.Legend, false);
        }
      }
    }
    if (this.Area == null)
      return;
    this.Area.ScheduleUpdate();
  }

  public List<object> GetDataPoints(Rect rectangle)
  {
    if (this.Area != null)
    {
      Rect seriesClipRect = this.Area.SeriesClipRect;
      if (this.ActualXAxis != null && this.ActualYAxis != null)
      {
        rectangle.Intersect(this.Area.SeriesClipRect);
        List<double> xvalues = this.GetXValues();
        if (xvalues == null || rectangle.Height <= 0.0 && rectangle.Width <= 0.0)
          return (List<object>) null;
        double startX = double.NaN;
        double startY = double.NaN;
        double endX = double.NaN;
        double endY = double.NaN;
        this.ConvertRectToValue(ref startX, ref endX, ref startY, ref endY, rectangle);
        bool flag = false;
        if (this.ActualXAxis.Orientation == Orientation.Vertical)
          flag = true;
        if (rectangle.Width > 0.0 && rectangle.Height > 0.0)
          return this.GetDataPoints(startX, endX, startY, endY);
        if ((flag || rectangle.Height > 0.0) && (!flag || rectangle.Width > 0.0))
          return this.GetDataPoints(startX, endX, startY, endY, 0, xvalues.Count - 1, xvalues, true);
        if (this.isLinearData)
        {
          int minimum = 0;
          int maximum = xvalues.Count - 1;
          CartesianSeries.CalculateNearestIndex(ref minimum, ref maximum, xvalues, startX, endX);
          return this.ActualData.GetRange(minimum, maximum - minimum + 1);
        }
        List<object> dataPoints = new List<object>();
        for (int index = 0; index < xvalues.Count; ++index)
        {
          double num = xvalues[index];
          if (startX <= num && num <= endX)
            dataPoints.Add(this.ActualData[index]);
        }
        return dataPoints;
      }
    }
    return (List<object>) null;
  }

  public List<object> GetDataPoints(double startX, double endX, double startY, double endY)
  {
    List<double> xvalues = this.GetXValues();
    int minimum = 0;
    int maximum = xvalues.Count - 1;
    if (this.isLinearData)
      CartesianSeries.CalculateNearestIndex(ref minimum, ref maximum, xvalues, startX, endX);
    return this.GetDataPoints(startX, endX, startY, endY, minimum, maximum, xvalues, this.isLinearData);
  }

  private static void CalculateNearestIndex(
    ref int minimum,
    ref int maximum,
    List<double> xValues,
    double startX,
    double endX)
  {
    minimum = ChartExtensionUtils.BinarySearch(xValues, startX, 0, maximum);
    maximum = ChartExtensionUtils.BinarySearch(xValues, endX, 0, maximum);
    minimum = startX <= xValues[minimum] ? minimum : minimum + 1;
    maximum = endX >= xValues[maximum] ? maximum : maximum - 1;
  }

  private void ConvertRectToValue(
    ref double startX,
    ref double endX,
    ref double startY,
    ref double endY,
    Rect rect)
  {
    Rect seriesClipRect = this.Area.SeriesClipRect;
    double x = rect.X + rect.Width - seriesClipRect.Left;
    double y = rect.Y + rect.Height - seriesClipRect.Top;
    startX = this.Area.PointToValue(this.ActualXAxis, new Point(rect.X - seriesClipRect.Left, rect.Y - seriesClipRect.Top));
    startY = this.Area.PointToValue(this.ActualYAxis, new Point(rect.X - seriesClipRect.Left, rect.Y - seriesClipRect.Top));
    if (this.ActualXAxis.Orientation == Orientation.Vertical)
    {
      endX = this.Area.PointToValue(this.ActualXAxis, new Point(rect.X - seriesClipRect.Left, y));
      endY = this.Area.PointToValue(this.ActualYAxis, new Point(x, rect.Y - seriesClipRect.Top));
    }
    else
    {
      endX = this.Area.PointToValue(this.ActualXAxis, new Point(x, rect.Y - seriesClipRect.Top));
      endY = this.Area.PointToValue(this.ActualYAxis, new Point(rect.X - seriesClipRect.Left, y));
    }
    if (startX > endX)
    {
      double num = endX;
      endX = startX;
      startX = num;
    }
    if (startY <= endY)
      return;
    double num1 = endY;
    endY = startY;
    startY = num1;
  }

  internal virtual List<object> GetDataPoints(
    double startX,
    double endX,
    double startY,
    double endY,
    int minimum,
    int maximum,
    List<double> xValues,
    bool validateYValues)
  {
    List<object> dataPoints = new List<object>();
    if (xValues.Count != this.ActualSeriesYValues[0].Count)
      return (List<object>) null;
    IList<double> actualSeriesYvalue = this.ActualSeriesYValues[0];
    for (int index = minimum; index <= maximum; ++index)
    {
      double xValue = xValues[index];
      if (validateYValues || startX <= xValue && xValue <= endX)
      {
        double num = actualSeriesYvalue[index];
        if (startY <= num && num <= endY)
          dataPoints.Add(this.ActualData[index]);
      }
    }
    return dataPoints;
  }
}
