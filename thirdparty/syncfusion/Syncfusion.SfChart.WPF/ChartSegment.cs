// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.ChartSegment
// Assembly: Syncfusion.SfChart.WPF, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 279D70C6-7936-4F3E-B6CF-79EFEF734826
// Assembly location: C:\Program Files\PDFgear\Syncfusion.SfChart.WPF.dll

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Shapes;

#nullable disable
namespace Syncfusion.UI.Xaml.Charts;

public abstract class ChartSegment : DependencyObject, INotifyPropertyChanged
{
  public static readonly DependencyProperty InteriorProperty = DependencyProperty.Register(nameof (Interior), typeof (Brush), typeof (ChartSegment), new PropertyMetadata((object) null, new PropertyChangedCallback(ChartSegment.OnInteriorChanged)));
  public static readonly DependencyProperty StrokeThicknessProperty = DependencyProperty.Register(nameof (StrokeThickness), typeof (double), typeof (ChartSegment), new PropertyMetadata((object) 1.0, new PropertyChangedCallback(ChartSegment.OnStrokeThicknessChanged)));
  public static readonly DependencyProperty StrokeDashArrayProperty = DependencyProperty.Register(nameof (StrokeDashArray), typeof (DoubleCollection), typeof (ChartSegment), new PropertyMetadata((object) null, new PropertyChangedCallback(ChartSegment.OnStrokeDashArrayChanged)));
  public static readonly DependencyProperty StrokeProperty = DependencyProperty.Register(nameof (Stroke), typeof (Brush), typeof (ChartSegment), new PropertyMetadata((object) null, new PropertyChangedCallback(ChartSegment.OnStrokeValueChanged)));
  internal bool IsAddedToVisualTree;
  internal bool IsSegmentVisible = true;
  internal bool x_isInversed;
  internal bool y_isInversed;
  internal Rect rect;
  private bool isEmptySegmentInterior;
  private object item;
  private PointCollection polygonPoints;

  public event PropertyChangedEventHandler PropertyChanged;

  public DoubleRange XRange { get; set; }

  public DoubleRange YRange { get; set; }

  public object Item
  {
    get => this.item;
    set
    {
      this.item = value;
      this.OnPropertyChanged(nameof (Item));
    }
  }

  public Brush Interior
  {
    get => (Brush) this.GetValue(ChartSegment.InteriorProperty);
    set => this.SetValue(ChartSegment.InteriorProperty, (object) value);
  }

  public double StrokeThickness
  {
    get => (double) this.GetValue(ChartSegment.StrokeThicknessProperty);
    set => this.SetValue(ChartSegment.StrokeThicknessProperty, (object) value);
  }

  public DoubleCollection StrokeDashArray
  {
    get => (DoubleCollection) this.GetValue(ChartSegment.StrokeDashArrayProperty);
    set => this.SetValue(ChartSegment.StrokeDashArrayProperty, (object) value);
  }

  public Brush Stroke
  {
    get => (Brush) this.GetValue(ChartSegment.StrokeProperty);
    set => this.SetValue(ChartSegment.StrokeProperty, (object) value);
  }

  public ChartSeriesBase Series { get; protected internal set; }

  internal bool IsSelectedSegment { get; set; }

  public PointCollection PolygonPoints
  {
    get => this.polygonPoints;
    set
    {
      this.polygonPoints = value;
      this.OnPropertyChanged(nameof (PolygonPoints));
    }
  }

  protected internal bool IsEmptySegmentInterior
  {
    get => this.isEmptySegmentInterior;
    internal set
    {
      if (this.isEmptySegmentInterior == value)
        return;
      this.isEmptySegmentInterior = value;
      if (this.Series == null)
        return;
      this.BindProperties();
    }
  }

  public abstract UIElement CreateVisual(Size size);

  public abstract UIElement GetRenderedVisual();

  public abstract void Update(IChartTransformer transformer);

  public abstract void OnSizeChanged(Size size);

  public virtual void SetData(IList<double> xVals, IList<double> yVals)
  {
  }

  public virtual void SetData(
    List<double> xValues,
    IList<double> yValues,
    double startDepth,
    double endDepth)
  {
  }

  public virtual void SetData(IList<double> xVals, IList<double> yVals, Brush strokeBrush)
  {
  }

  public virtual void SetData(
    IList<double> xVals,
    IList<double> yVals,
    Brush strokeBrush,
    int length)
  {
  }

  public virtual void SetData(params double[] Values)
  {
  }

  [Obsolete("Use SetData(List<ChartPoint> AreaPoints)")]
  public virtual void SetData(List<Point> AreaPoints)
  {
  }

  public virtual void SetData(List<ChartPoint> AreaPoints)
  {
  }

  [Obsolete("Use SetData(ChartPoint point1, ChartPoint point2, ChartPoint point3, ChartPoint point4)")]
  public virtual void SetData(Point point1, Point point2, Point point3, Point point4)
  {
  }

  public virtual void SetData(
    ChartPoint point1,
    ChartPoint point2,
    ChartPoint point3,
    ChartPoint point4)
  {
  }

  public virtual void SetData(
    IList<double> xValues,
    IList<double> yHiValues,
    IList<double> yLowValues)
  {
  }

  public virtual void SetData(
    IList<double> xValues,
    IList<double> yHiValues,
    IList<double> yLowValues,
    IList<double> yOpenValues,
    IList<double> yCloseValues)
  {
  }

  public virtual void SetData(
    IList<double> x1Values,
    IList<double> y1Values,
    IList<double> x2Values,
    IList<double> y2Values)
  {
  }

  public virtual void SetData(
    Point leftpoint,
    Point rightpoint,
    Point toppoint,
    Point bottompoint,
    Point vercappoint,
    Point horcappoint)
  {
  }

  [Obsolete("Use SetData(ChartPoint point1, ChartPoint point2, ChartPoint point3,ChartPoint point4,bool isBull)")]
  public virtual void SetData(
    Point BottomLeft,
    Point RightTop,
    Point hipoint,
    Point loPoint,
    bool isBull)
  {
  }

  public virtual void SetData(
    ChartPoint BottomLeft,
    ChartPoint RightTop,
    ChartPoint hipoint,
    ChartPoint loPoint,
    bool isBull)
  {
  }

  [Obsolete("Use SetData(ChartPoint point1, ChartPoint point2, ChartPoint point3, ChartPoint point4, ChartPoint point5, ChartPoint point6, bool isbull)")]
  public virtual void SetData(
    Point hipoint,
    Point lopoint,
    Point sopoint,
    Point eopoint,
    Point scpoint,
    Point ecpoint,
    bool isBull)
  {
  }

  public virtual void SetData(
    ChartPoint hipoint,
    ChartPoint lopoint,
    ChartPoint sopoint,
    ChartPoint eopoint,
    ChartPoint scpoint,
    ChartPoint ecpoint,
    bool isBull)
  {
  }

  internal virtual void Dispose()
  {
    if (this.PropertyChanged != null)
    {
      foreach (Delegate invocation in this.PropertyChanged.GetInvocationList())
        this.PropertyChanged -= invocation as PropertyChangedEventHandler;
      this.PropertyChanged = (PropertyChangedEventHandler) null;
    }
    if (this.PolygonPoints != null)
    {
      this.PolygonPoints.Clear();
      this.PolygonPoints = (PointCollection) null;
    }
    this.Item = (object) null;
    this.Series = (ChartSeriesBase) null;
  }

  internal void BindProperties()
  {
    bool flag1 = this.Series is CircularSeriesBase series && !double.IsNaN(series.GroupTo);
    if (this is ScatterSegment && this.Series is BoxAndWhiskerSeries)
      return;
    bool flag2 = this.Series.ActualXAxis is CategoryAxis ? (this.Series.ActualXAxis as CategoryAxis).IsIndexed : !(this.Series.ActualXAxis is CategoryAxis3D) || (this.Series.ActualXAxis as CategoryAxis3D).IsIndexed;
    if (this.Series.ActualArea.GetEnableSeriesSelection() && this.Series.ActualArea.SelectedSeriesCollection.Contains(this.Series))
      BindingOperations.SetBinding((DependencyObject) this, ChartSegment.InteriorProperty, (BindingBase) new Binding()
      {
        Source = (object) this.Series,
        Path = new PropertyPath("SeriesSelectionBrush", new object[0]),
        Converter = (IValueConverter) new SeriesSelectionBrushConverter(this.Series),
        ConverterParameter = (object) (!(this.Series is AccumulationSeriesBase) && !(this.Series is CircularSeriesBase3D) || flag1 ? this.Series.Segments.IndexOf(this) : this.Series.ActualData.IndexOf(this.Item))
      });
    else if (this.Series.ActualArea.GetEnableSegmentSelection() && this.IsSegmentSelected())
      BindingOperations.SetBinding((DependencyObject) this, ChartSegment.InteriorProperty, (BindingBase) new Binding()
      {
        Source = (object) this.Series,
        Path = new PropertyPath("SegmentSelectionBrush", new object[0]),
        Converter = (IValueConverter) new SegmentSelectionBrushConverter(this.Series),
        ConverterParameter = (flag2 || !this.Series.IsSideBySide || this.Series is RangeSeriesBase || this.Series is FinancialSeriesBase || this.Series is WaterfallSeries ? (object) (!(this.Series is AccumulationSeriesBase) && !(this.Series is CircularSeriesBase3D) || flag1 ? this.Series.Segments.IndexOf(this) : this.Series.ActualData.IndexOf(this.Item)) : (object) this.Series.GroupedActualData.IndexOf(this.Item))
      });
    else if (this is WaterfallSegment)
      this.BindWaterfallSegmentInterior(this as WaterfallSegment);
    else if (!this.IsEmptySegmentInterior)
      BindingOperations.SetBinding((DependencyObject) this, ChartSegment.InteriorProperty, (BindingBase) new Binding()
      {
        Source = (object) this.Series,
        Path = new PropertyPath("Interior", new object[0]),
        Converter = (IValueConverter) new InteriorConverter(this.Series),
        ConverterParameter = (flag2 || !this.Series.IsSideBySide || this.Series is RangeSeriesBase || this.Series is FinancialSeriesBase ? (object) (!(this.Series is AccumulationSeriesBase) && !(this.Series is CircularSeriesBase3D) || flag1 ? (!(this.Series is ChartSeries3D) || !(this.Series is ColumnSeries3D) ? this.Series.Segments.IndexOf(this) : this.Series.ActualData.IndexOf(this.Item)) : this.Series.ActualData.IndexOf(this.Item)) : (object) this.Series.GroupedActualData.IndexOf(this.Item))
      });
    else
      BindingOperations.SetBinding((DependencyObject) this, ChartSegment.InteriorProperty, (BindingBase) new Binding()
      {
        Source = (object) this.Series,
        ConverterParameter = (object) this.Series.Interior,
        Path = new PropertyPath("EmptyPointInterior", new object[0]),
        Converter = (IValueConverter) new MultiInteriorConverter()
      });
    if (this.Series is ChartSeries)
      BindingOperations.SetBinding((DependencyObject) this, ChartSegment.StrokeProperty, (BindingBase) new Binding()
      {
        Source = (object) this.Series,
        Path = new PropertyPath("Stroke", new object[0])
      });
    if (this.Series is ChartSeries || this.Series is LineSeries3D)
      BindingOperations.SetBinding((DependencyObject) this, ChartSegment.StrokeThicknessProperty, (BindingBase) new Binding()
      {
        Source = (object) this.Series,
        Path = new PropertyPath("StrokeThickness", new object[0])
      });
    if (!(this.Series is DoughnutSeries))
      return;
    BindingOperations.SetBinding((DependencyObject) this, DoughnutSegment.TrackBorderColorProperty, (BindingBase) new Binding()
    {
      Source = (object) this.Series,
      Path = new PropertyPath("TrackBorderColor", new object[0])
    });
    BindingOperations.SetBinding((DependencyObject) this, DoughnutSegment.TrackBorderWidthProperty, (BindingBase) new Binding()
    {
      Source = (object) this.Series,
      Path = new PropertyPath("TrackBorderWidth", new object[0])
    });
  }

  internal void BindWaterfallSegmentInterior(WaterfallSegment segment)
  {
    Binding binding = new Binding();
    binding.Source = (object) segment.Series;
    if (segment.SegmentType == WaterfallSegmentType.Negative && (segment.Series as WaterfallSeries).NegativeSegmentBrush != null)
    {
      binding.Path = new PropertyPath("NegativeSegmentBrush", new object[0]);
      BindingOperations.SetBinding((DependencyObject) this, ChartSegment.InteriorProperty, (BindingBase) binding);
    }
    else if (segment.SegmentType == WaterfallSegmentType.Sum && (segment.Series as WaterfallSeries).SummarySegmentBrush != null)
    {
      binding.Path = new PropertyPath("SummarySegmentBrush", new object[0]);
      BindingOperations.SetBinding((DependencyObject) this, ChartSegment.InteriorProperty, (BindingBase) binding);
    }
    else
    {
      binding.Path = new PropertyPath("Interior", new object[0]);
      binding.Converter = (IValueConverter) new InteriorConverter(segment.Series);
      binding.ConverterParameter = (object) segment.Series.Segments.IndexOf((ChartSegment) segment);
      BindingOperations.SetBinding((DependencyObject) this, ChartSegment.InteriorProperty, (BindingBase) binding);
    }
  }

  internal bool IsSegmentSelected()
  {
    bool flag = this.Series.ActualXAxis is CategoryAxis ? (this.Series.ActualXAxis as CategoryAxis).IsIndexed : !(this.Series.ActualXAxis is CategoryAxis3D) || (this.Series.ActualXAxis as CategoryAxis3D).IsIndexed;
    int num = !(this.Series is CircularSeriesBase) || double.IsNaN(((CircularSeriesBase) this.Series).GroupTo) ? (flag || !this.Series.IsSideBySide || this.Series is RangeSeriesBase || this.Series is FinancialSeriesBase || this.Series is WaterfallSeries ? this.Series.ActualData.IndexOf(this.Item) : this.Series.GroupedActualData.IndexOf(this.Item)) : this.Series.Segments.IndexOf(this);
    if (this.Series is ISegmentSelectable && !this.Series.IsBitmapSeries && this.Series.SelectedSegmentsIndexes.Contains(num) && (this.Series.IsSideBySide || this.Series is AccumulationSeriesBase || this.Series is BubbleSeries || this.Series is ScatterSeries))
      return true;
    if (!(this.Series is ChartSeries3D) || (this.Series as ChartSeries3D).SegmentSelectionBrush == null || !this.Series.SelectedSegmentsIndexes.Contains(num))
      return false;
    return this.Series.IsSideBySide || this.Series is CircularSeriesBase3D;
  }

  internal static Color GetColor(Brush brush)
  {
    switch (brush)
    {
      case SolidColorBrush _:
        return ((SolidColorBrush) brush).Color;
      case LinearGradientBrush _:
        if ((brush as LinearGradientBrush).GradientStops.Count > 0)
          return (brush as LinearGradientBrush).GradientStops[0].Color;
        break;
    }
    return new SolidColorBrush(Colors.Transparent).Color;
  }

  internal virtual UIElement CreateSegmentVisual(Size size)
  {
    this.BindProperties();
    return this.CreateVisual(size);
  }

  internal bool GetEnableSegmentSelection(Brush segmentSelectionBrush)
  {
    return segmentSelectionBrush != null && this.Series.ActualArea.GetEnableSegmentSelection();
  }

  protected internal double[] AlignHiLoSegment(
    double openValues,
    double closeValues,
    double highValues,
    double lowValues)
  {
    double[] numArray = new double[2];
    if (highValues < openValues)
    {
      double num = highValues;
      if (highValues < lowValues)
      {
        highValues = lowValues;
        lowValues = num;
      }
      if (highValues < openValues)
        highValues = openValues;
      if (highValues < closeValues)
        highValues = closeValues;
    }
    if (lowValues > closeValues)
    {
      double num = highValues;
      if (lowValues > highValues)
      {
        highValues = lowValues;
        lowValues = num;
      }
      if (lowValues > closeValues)
        lowValues = closeValues;
      if (lowValues > openValues)
        lowValues = openValues;
    }
    numArray[0] = highValues;
    numArray[1] = lowValues;
    return numArray;
  }

  protected virtual void OnPropertyChanged(string name)
  {
    PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
    if (propertyChanged == null)
      return;
    propertyChanged((object) this, new PropertyChangedEventArgs(name));
  }

  protected virtual void SetVisualBindings(Shape element)
  {
    Binding binding = new Binding();
    binding.Source = (object) this;
    binding.Path = new PropertyPath("Interior", new object[0]);
    element.SetBinding(Shape.FillProperty, (BindingBase) binding);
    element.SetBinding(Shape.StrokeProperty, (BindingBase) binding);
    element.SetBinding(Shape.StrokeThicknessProperty, (BindingBase) new Binding()
    {
      Source = (object) this,
      Path = new PropertyPath("StrokeThickness", new object[0])
    });
    DoubleCollection strokeDashArray = this.StrokeDashArray;
    if (strokeDashArray == null || strokeDashArray.Count <= 0)
      return;
    DoubleCollection doubleCollection = new DoubleCollection();
    foreach (double num in strokeDashArray)
      doubleCollection.Add(num);
    element.StrokeDashArray = doubleCollection;
  }

  private static void OnInteriorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    (d as ChartSegment).OnPropertyChanged("Interior");
  }

  private static void OnStrokeDashArrayChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    if (!(d is ChartSegment chartSegment))
      return;
    DoubleCollection newValue = e.NewValue as DoubleCollection;
    if (!(chartSegment.GetRenderedVisual() is Shape renderedVisual))
      return;
    DoubleCollection doubleCollection = newValue == null ? (DoubleCollection) null : ChartSegment.GetDoubleCollection(newValue);
    renderedVisual.StrokeDashArray = doubleCollection;
  }

  private static DoubleCollection GetDoubleCollection(DoubleCollection collection)
  {
    DoubleCollection doubleCollection = new DoubleCollection();
    foreach (double num in collection)
      doubleCollection.Add(num);
    return doubleCollection;
  }

  private static void OnStrokeValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    (d as ChartSegment).OnPropertyChanged("Stroke");
  }

  private static void OnStrokeThicknessChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    (d as ChartSegment).OnPropertyChanged("StrokeThickness");
  }
}
