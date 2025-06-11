// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.BoxAndWhiskerSeries
// Assembly: Syncfusion.SfChart.WPF, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 279D70C6-7936-4F3E-B6CF-79EFEF734826
// Assembly location: C:\Program Files\PDFgear\Syncfusion.SfChart.WPF.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

#nullable disable
namespace Syncfusion.UI.Xaml.Charts;

public class BoxAndWhiskerSeries : XyDataSeries, ISegmentSpacing, ISegmentSelectable
{
  public static readonly DependencyProperty SelectedIndexProperty = DependencyProperty.Register(nameof (SelectedIndex), typeof (int), typeof (BoxAndWhiskerSeries), new PropertyMetadata((object) -1, new PropertyChangedCallback(BoxAndWhiskerSeries.OnSelectedIndexChanged)));
  public static readonly DependencyProperty BoxPlotModeProperty = DependencyProperty.Register(nameof (BoxPlotMode), typeof (BoxPlotMode), typeof (BoxAndWhiskerSeries), new PropertyMetadata((object) BoxPlotMode.Exclusive, new PropertyChangedCallback(BoxAndWhiskerSeries.OnPropertyChanged)));
  public static readonly DependencyProperty OutlierTemplateProperty = DependencyProperty.Register("SymbolTemplate", typeof (DataTemplate), typeof (BoxAndWhiskerSeries), new PropertyMetadata((object) null, new PropertyChangedCallback(BoxAndWhiskerSeries.OnOutlierTemplateChanged)));
  public static readonly DependencyProperty ShowMedianProperty = DependencyProperty.Register(nameof (ShowMedian), typeof (bool), typeof (BoxAndWhiskerSeries), new PropertyMetadata((object) false, new PropertyChangedCallback(BoxAndWhiskerSeries.OnShowMedianChanged)));
  public static readonly DependencyProperty SegmentSpacingProperty = DependencyProperty.Register(nameof (SegmentSpacing), typeof (double), typeof (BoxAndWhiskerSeries), new PropertyMetadata((object) 0.0, new PropertyChangedCallback(BoxAndWhiskerSeries.OnSegmentSpacingChanged)));
  public static readonly DependencyProperty SegmentSelectionBrushProperty = DependencyProperty.Register(nameof (SegmentSelectionBrush), typeof (Brush), typeof (BoxAndWhiskerSeries), new PropertyMetadata((object) null, new PropertyChangedCallback(BoxAndWhiskerSeries.OnPropertyChanged)));
  public static readonly DependencyProperty ShowOutlierProperty = DependencyProperty.Register(nameof (ShowOutlier), typeof (bool), typeof (BoxAndWhiskerSeries), new PropertyMetadata((object) true, new PropertyChangedCallback(BoxAndWhiskerSeries.OnPropertyChanged)));
  private double whiskerWidth;
  private bool isEvenList;
  private List<ChartSegment> outlierSegments;
  private ActualLabelPosition topLabelPosition;
  private ActualLabelPosition bottomLabelPosition;
  private DataTemplate boxwhiskerTooltipTemplate;

  public int SelectedIndex
  {
    get => (int) this.GetValue(BoxAndWhiskerSeries.SelectedIndexProperty);
    set => this.SetValue(BoxAndWhiskerSeries.SelectedIndexProperty, (object) value);
  }

  public BoxPlotMode BoxPlotMode
  {
    get => (BoxPlotMode) this.GetValue(BoxAndWhiskerSeries.BoxPlotModeProperty);
    set => this.SetValue(BoxAndWhiskerSeries.BoxPlotModeProperty, (object) value);
  }

  public DataTemplate OutlierTemplate
  {
    get => (DataTemplate) this.GetValue(BoxAndWhiskerSeries.OutlierTemplateProperty);
    set => this.SetValue(BoxAndWhiskerSeries.OutlierTemplateProperty, (object) value);
  }

  public bool ShowMedian
  {
    get => (bool) this.GetValue(BoxAndWhiskerSeries.ShowMedianProperty);
    set => this.SetValue(BoxAndWhiskerSeries.ShowMedianProperty, (object) value);
  }

  public double SegmentSpacing
  {
    get => (double) this.GetValue(BoxAndWhiskerSeries.SegmentSpacingProperty);
    set => this.SetValue(BoxAndWhiskerSeries.SegmentSpacingProperty, (object) value);
  }

  public Brush SegmentSelectionBrush
  {
    get => (Brush) this.GetValue(BoxAndWhiskerSeries.SegmentSelectionBrushProperty);
    set => this.SetValue(BoxAndWhiskerSeries.SegmentSelectionBrushProperty, (object) value);
  }

  public bool ShowOutlier
  {
    get => (bool) this.GetValue(BoxAndWhiskerSeries.ShowOutlierProperty);
    set => this.SetValue(BoxAndWhiskerSeries.ShowOutlierProperty, (object) value);
  }

  protected internal double? WhiskerWidth { get; set; }

  protected internal override bool IsSideBySide => true;

  protected internal List<IList<double>> YCollection { get; set; }

  double ISegmentSpacing.CalculateSegmentSpacing(double spacing, double Right, double Left)
  {
    return this.CalculateSegmentSpacing(spacing, Right, Left);
  }

  public override void FindNearestChartPoint(
    Point point,
    out double x,
    out double y,
    out double stackedYValue)
  {
    x = double.NaN;
    y = 0.0;
    stackedYValue = double.NaN;
    Point point1 = new Point();
    if (this.IsIndexed || !(this.ActualXValues is IList<double>))
    {
      if (this.ActualArea == null)
        return;
      double start = this.ActualXAxis.VisibleRange.Start;
      double end = this.ActualXAxis.VisibleRange.End;
      point = new Point(this.ActualArea.PointToValue(this.ActualXAxis, point), this.ActualArea.PointToValue(this.ActualYAxis, point));
      double num = Math.Round(point.X);
      int count = this.YCollection.Count;
      if (num > end || num < start || num >= (double) count || num < 0.0)
        return;
      x = num;
      this.NearestSegmentIndex = (int) x;
    }
    else
    {
      IList<double> actualXvalues = this.ActualXValues as IList<double>;
      point1.X = this.ActualXAxis.VisibleRange.Start;
      if (this.IsSideBySide)
      {
        DoubleRange sideBySideInfo = this.GetSideBySideInfo((ChartSeriesBase) this);
        point1.X = this.ActualXAxis.VisibleRange.Start + sideBySideInfo.Start;
      }
      point = new Point(this.ActualArea.PointToValue(this.ActualXAxis, point), this.ActualArea.PointToValue(this.ActualYAxis, point));
      for (int index = 0; index < this.DataCount; ++index)
      {
        double x1 = actualXvalues[index];
        double y1 = 0.0;
        if (this.ActualXAxis is LogarithmicAxis)
        {
          LogarithmicAxis actualXaxis = this.ActualXAxis as LogarithmicAxis;
          if (Math.Abs(point.X - x1) <= Math.Abs(point.X - point1.X) && Math.Log(point.X, actualXaxis.LogarithmicBase) > this.ActualXAxis.VisibleRange.Start && Math.Log(point.X, actualXaxis.LogarithmicBase) < this.ActualXAxis.VisibleRange.End)
          {
            point1 = new Point(x1, y1);
            x = actualXvalues[index];
            this.NearestSegmentIndex = index;
          }
        }
        else if (Math.Abs(point.X - x1) <= Math.Abs(point.X - point1.X) && point.X > this.ActualXAxis.VisibleRange.Start && point.X < this.ActualXAxis.VisibleRange.End)
        {
          point1 = new Point(x1, y1);
          x = actualXvalues[index];
          this.NearestSegmentIndex = index;
        }
      }
    }
  }

  public override void CreateSegments()
  {
    this.ClearUnUsedSegments(this.DataCount);
    this.ClearUnUsedAdornments(this.DataCount * 5);
    DoubleRange sideBySideInfo = this.GetSideBySideInfo((ChartSeriesBase) this);
    double start = sideBySideInfo.Start;
    double end = sideBySideInfo.End;
    List<double> xvalues = this.GetXValues();
    this.UpdateWhiskerWidth();
    if (this.adornmentInfo != null)
      this.UpdateAdornmentLabelPositiion();
    this.outlierSegments = new List<ChartSegment>();
    if (this.YCollection == null || this.YCollection.Count == 0)
      return;
    for (int index = 0; index < this.DataCount; ++index)
    {
      double num1 = 0.0;
      List<double> doubleList = new List<double>();
      double[] array = this.YCollection[index].Where<double>((System.Func<double, bool>) (x => !double.IsNaN(x))).ToArray<double>();
      if (((IEnumerable<double>) array).Count<double>() > 0)
      {
        Array.Sort<double>(array);
        num1 = ((IEnumerable<double>) array).Average();
      }
      int length = array.Length;
      this.isEvenList = length % 2 == 0;
      double x1 = xvalues[index] + start;
      double num2 = xvalues[index] + end;
      if (length == 0)
      {
        if (index < this.Segments.Count)
        {
          BoxAndWhiskerSegment segment = this.Segments[index] as BoxAndWhiskerSegment;
          segment.SetData(x1, num2, double.NaN, double.NaN, double.NaN, double.NaN, double.NaN, double.NaN, double.NaN, xvalues[index] + sideBySideInfo.Median, double.NaN);
          segment.Item = this.ActualData[index];
        }
        else if (this.CreateSegment() is BoxAndWhiskerSegment segment1)
        {
          segment1.SetData(x1, num2, double.NaN, double.NaN, double.NaN, double.NaN, double.NaN, double.NaN, double.NaN, xvalues[index] + sideBySideInfo.Median, double.NaN);
          segment1.Item = this.ActualData[index];
          segment1.Series = (ChartSeriesBase) this;
          this.Segments.Add((ChartSegment) segment1);
        }
        if (this.AdornmentsInfo != null)
        {
          if (index < this.Adornments.Count / 5)
            this.SetBoxWhiskerAdornments(sideBySideInfo, xvalues[index], double.NaN, double.NaN, x1, double.NaN, double.NaN, double.NaN, index);
          else
            this.AddBoxWhiskerAdornments(sideBySideInfo, xvalues[index], double.NaN, double.NaN, x1, double.NaN, double.NaN, double.NaN, index);
        }
      }
      else
      {
        double lowerQuartile;
        double upperQuartile;
        double median;
        if (this.BoxPlotMode == BoxPlotMode.Exclusive)
        {
          lowerQuartile = BoxAndWhiskerSeries.GetExclusiveQuartileValue(array, length, 0.25);
          upperQuartile = BoxAndWhiskerSeries.GetExclusiveQuartileValue(array, length, 0.75);
          median = BoxAndWhiskerSeries.GetExclusiveQuartileValue(array, length, 0.5);
        }
        else if (this.BoxPlotMode == BoxPlotMode.Inclusive)
        {
          lowerQuartile = BoxAndWhiskerSeries.GetInclusiveQuartileValue(array, length, 0.25);
          upperQuartile = BoxAndWhiskerSeries.GetInclusiveQuartileValue(array, length, 0.75);
          median = BoxAndWhiskerSeries.GetInclusiveQuartileValue(array, length, 0.5);
        }
        else
        {
          median = BoxAndWhiskerSeries.GetMedianValue(array);
          this.GetQuartileValues(array, length, out lowerQuartile, out upperQuartile);
        }
        double minimum;
        double maximum;
        if (this.ShowOutlier)
        {
          BoxAndWhiskerSeries.GetMinMaxandOutlier(lowerQuartile, upperQuartile, array, out minimum, out maximum, doubleList);
        }
        else
        {
          minimum = ((IEnumerable<double>) array).Min();
          maximum = ((IEnumerable<double>) array).Max();
        }
        double val2_1 = minimum;
        double val2_2 = maximum;
        if (doubleList.Count > 0)
        {
          val2_1 = Math.Min(doubleList.Min(), val2_1);
          val2_2 = Math.Max(doubleList.Max(), val2_2);
        }
        ChartSegment currentSegment = (ChartSegment) null;
        if (index < this.Segments.Count)
        {
          currentSegment = this.Segments[index];
          BoxAndWhiskerSegment segment = this.Segments[index] as BoxAndWhiskerSegment;
          segment.SetData(x1, num2, val2_1, minimum, lowerQuartile, median, upperQuartile, maximum, val2_2, xvalues[index] + sideBySideInfo.Median, num1);
          this.WhiskerWidth = new double?(this.whiskerWidth);
          segment.Item = this.ActualData[index];
        }
        else if (this.CreateSegment() is BoxAndWhiskerSegment segment2)
        {
          segment2.Series = (ChartSeriesBase) this;
          segment2.SetData(x1, num2, val2_1, minimum, lowerQuartile, median, upperQuartile, maximum, val2_2, xvalues[index] + sideBySideInfo.Median, num1);
          segment2.Item = this.ActualData[index];
          segment2.Outliers = doubleList;
          segment2.WhiskerWidth = this.whiskerWidth;
          currentSegment = (ChartSegment) segment2;
          this.Segments.Add((ChartSegment) segment2);
        }
        if (this.AdornmentsInfo != null)
        {
          if (index < this.Adornments.Count / 5)
            this.SetBoxWhiskerAdornments(sideBySideInfo, xvalues[index], minimum, maximum, x1, median, lowerQuartile, upperQuartile, index);
          else
            this.AddBoxWhiskerAdornments(sideBySideInfo, xvalues[index], minimum, maximum, x1, median, lowerQuartile, upperQuartile, index);
        }
        if (doubleList.Count > 0)
        {
          foreach (double num3 in doubleList)
          {
            ScatterSegment scatterSegment = new ScatterSegment();
            double num4 = x1 + (num2 - x1) / 2.0;
            scatterSegment.Series = (ChartSeriesBase) this;
            scatterSegment.SetData(num4, num3);
            scatterSegment.ScatterWidth = 10.0;
            scatterSegment.ScatterHeight = 10.0;
            scatterSegment.Item = this.ActualData[index];
            scatterSegment.CustomTemplate = this.OutlierTemplate;
            BoxAndWhiskerSeries.BindProperties(scatterSegment, currentSegment);
            this.outlierSegments.Add((ChartSegment) scatterSegment);
          }
        }
        this.isEvenList = false;
      }
    }
    foreach (ScatterSegment outlierSegment in this.outlierSegments)
    {
      this.Segments.Add((ChartSegment) outlierSegment);
      if (this.AdornmentsInfo != null)
      {
        ChartAdornment adornment = this.CreateAdornment((AdornmentSeries) this, outlierSegment.XData, outlierSegment.YData, outlierSegment.XData, outlierSegment.YData);
        adornment.ActualLabelPosition = this.topLabelPosition;
        adornment.Item = outlierSegment.Item;
        this.Adornments.Add(adornment);
      }
    }
  }

  internal override object GetTooltipTag(FrameworkElement element)
  {
    object tooltipTag = (object) null;
    if (element.Tag is ChartSegment)
      tooltipTag = element.Tag;
    else if (element.DataContext is ChartSegment && !(element.DataContext is ChartAdornment))
      tooltipTag = element.DataContext;
    else if (element.DataContext is ChartAdornmentContainer)
      tooltipTag = this.GetBoxAndWhiskerSegment((element.DataContext as ChartAdornmentContainer).Adornment);
    else if (VisualTreeHelper.GetParent((DependencyObject) element) is ContentPresenter parent && parent.Content is ChartAdornment)
    {
      tooltipTag = this.GetBoxAndWhiskerSegment(parent.Content as ChartAdornment);
    }
    else
    {
      int adornmentIndex = ChartExtensionUtils.GetAdornmentIndex((object) element);
      if (adornmentIndex != -1 && adornmentIndex < this.Adornments.Count && adornmentIndex < this.Segments.Count)
        tooltipTag = this.GetSegment(this.Adornments[adornmentIndex].Item);
    }
    return tooltipTag;
  }

  internal override Point GetDataPointPosition(ChartTooltip tooltip)
  {
    BoxAndWhiskerSegment toolTipTag1 = this.ToolTipTag as BoxAndWhiskerSegment;
    Point dataPointPosition = new Point();
    if (toolTipTag1 != null)
    {
      Point visible = this.ChartTransformer.TransformToVisible(toolTipTag1.Center, toolTipTag1.Maximum);
      dataPointPosition.X = visible.X + this.ActualArea.SeriesClipRect.Left;
      dataPointPosition.Y = visible.Y + this.ActualArea.SeriesClipRect.Top;
    }
    else if (this.ToolTipTag is ScatterSegment toolTipTag2)
    {
      Point visible = this.ChartTransformer.TransformToVisible(toolTipTag2.XData, toolTipTag2.YData);
      dataPointPosition.X = visible.X + this.ActualArea.SeriesClipRect.Left;
      dataPointPosition.Y = visible.Y + this.ActualArea.SeriesClipRect.Top - toolTipTag2.ScatterHeight / 2.0;
      if (dataPointPosition.Y - tooltip.DesiredSize.Height < this.ActualArea.SeriesClipRect.Top)
        dataPointPosition.Y += toolTipTag2.ScatterHeight;
    }
    return dataPointPosition;
  }

  internal override void GeneratePropertyPoints(string[] yPaths, IList<double>[] yLists)
  {
    IEnumerator enumerator = (this.ItemsSource as IEnumerable).GetEnumerator();
    if (enumerator.MoveNext())
    {
      for (int index = 0; index < this.UpdateStartedIndex; ++index)
        enumerator.MoveNext();
      PropertyInfo propertyInfo1 = ChartDataUtils.GetPropertyInfo(enumerator.Current, this.XBindingPath);
      IPropertyAccessor propertyAccessor1 = (IPropertyAccessor) null;
      if (propertyInfo1 != (PropertyInfo) null)
        propertyAccessor1 = FastReflectionCaches.PropertyAccessorCache.Get(propertyInfo1);
      if (propertyAccessor1 == null)
        return;
      System.Func<object, object> getMethod1 = propertyAccessor1.GetMethod;
      this.XAxisValueType = ChartSeriesBase.GetDataType(propertyAccessor1, this.ItemsSource as IEnumerable);
      if (this.XAxisValueType == ChartValueType.DateTime || this.XAxisValueType == ChartValueType.Double || this.XAxisValueType == ChartValueType.Logarithmic || this.XAxisValueType == ChartValueType.TimeSpan)
      {
        if (!(this.ActualXValues is List<double>))
          this.ActualXValues = this.XValues = (IEnumerable) new List<double>();
      }
      else if (!(this.ActualXValues is List<string>))
        this.ActualXValues = this.XValues = (IEnumerable) new List<string>();
      PropertyInfo propertyInfo2 = ChartDataUtils.GetPropertyInfo(enumerator.Current, yPaths[0]);
      IPropertyAccessor propertyAccessor2 = (IPropertyAccessor) null;
      if (propertyInfo2 != (PropertyInfo) null)
        propertyAccessor2 = FastReflectionCaches.PropertyAccessorCache.Get(propertyInfo2);
      if (propertyAccessor2 == null || propertyAccessor2 == null)
        return;
      System.Func<object, object> getMethod2 = propertyAccessor2.GetMethod;
      if (this.XAxisValueType == ChartValueType.String)
      {
        IList<string> xvalues = (IList<string>) (this.XValues as List<string>);
        do
        {
          object obj1 = getMethod1(enumerator.Current);
          object obj2 = getMethod2(enumerator.Current);
          xvalues.Add(obj1 != null ? (string) obj1 : string.Empty);
          this.YCollection.Add((IList<double>) obj2 ?? (IList<double>) new double[0]);
          this.ActualData.Add(enumerator.Current);
        }
        while (enumerator.MoveNext());
        this.DataCount = xvalues.Count;
      }
      else if (this.XAxisValueType == ChartValueType.Double || this.XAxisValueType == ChartValueType.Logarithmic)
      {
        IList<double> xvalues = (IList<double>) (this.XValues as List<double>);
        do
        {
          object obj3 = getMethod1(enumerator.Current);
          object obj4 = getMethod2(enumerator.Current);
          this.XData = Convert.ToDouble(obj3 ?? (object) double.NaN);
          xvalues.Add(this.XData);
          this.YCollection.Add((IList<double>) obj4 ?? (IList<double>) new double[0]);
          this.ActualData.Add(enumerator.Current);
        }
        while (enumerator.MoveNext());
        this.DataCount = xvalues.Count;
      }
      else if (this.XAxisValueType == ChartValueType.DateTime)
      {
        IList<double> xvalues = (IList<double>) (this.XValues as List<double>);
        do
        {
          object obj5 = getMethod1(enumerator.Current);
          object obj6 = getMethod2(enumerator.Current);
          this.XData = ((DateTime) obj5).ToOADate();
          xvalues.Add(this.XData);
          this.YCollection.Add((IList<double>) obj6 ?? (IList<double>) new double[0]);
          this.ActualData.Add(enumerator.Current);
        }
        while (enumerator.MoveNext());
        this.DataCount = xvalues.Count;
      }
      else if (this.XAxisValueType == ChartValueType.TimeSpan)
      {
        IList<double> xvalues = (IList<double>) (this.XValues as List<double>);
        do
        {
          object obj7 = getMethod1(enumerator.Current);
          object obj8 = getMethod2(enumerator.Current);
          this.XData = ((TimeSpan) obj7).TotalMilliseconds;
          xvalues.Add(this.XData);
          this.YCollection.Add((IList<double>) obj8 ?? (IList<double>) new double[0]);
          this.ActualData.Add(enumerator.Current);
        }
        while (enumerator.MoveNext());
        this.DataCount = xvalues.Count;
      }
    }
    this.IsPointGenerated = true;
  }

  internal override void GenerateComplexPropertyPoints(
    string[] yPaths,
    IList<double>[] yLists,
    ChartSeriesBase.GetReflectedProperty getPropertyValue)
  {
    IEnumerator enumerator = (this.ItemsSource as IEnumerable).GetEnumerator();
    if (enumerator.MoveNext())
    {
      for (int index = 0; index < this.UpdateStartedIndex; ++index)
        enumerator.MoveNext();
      this.XAxisValueType = this.GetDataType(this.ItemsSource as IEnumerable, this.XComplexPaths);
      if (this.XAxisValueType == ChartValueType.DateTime || this.XAxisValueType == ChartValueType.Double || this.XAxisValueType == ChartValueType.Logarithmic || this.XAxisValueType == ChartValueType.TimeSpan)
      {
        if (!(this.XValues is List<double>))
          this.ActualXValues = this.XValues = (IEnumerable) new List<double>();
      }
      else if (!(this.XValues is List<string>))
        this.ActualXValues = this.XValues = (IEnumerable) new List<string>();
      string[] ycomplexPath = this.YComplexPaths[0];
      if (string.IsNullOrEmpty(yPaths[0]))
        return;
      if (this.XAxisValueType == ChartValueType.String)
      {
        IList<string> xvalues = (IList<string>) (this.XValues as List<string>);
        do
        {
          object obj1 = getPropertyValue(enumerator.Current, this.XComplexPaths);
          object obj2 = getPropertyValue(enumerator.Current, ycomplexPath);
          if (obj1 == null)
            return;
          xvalues.Add((string) obj1);
          this.YCollection.Add((IList<double>) obj2 ?? (IList<double>) new double[0]);
          this.ActualData.Add(enumerator.Current);
        }
        while (enumerator.MoveNext());
        this.DataCount = xvalues.Count;
      }
      else if (this.XAxisValueType == ChartValueType.Double || this.XAxisValueType == ChartValueType.Logarithmic)
      {
        IList<double> xvalues = (IList<double>) (this.XValues as List<double>);
        do
        {
          object obj3 = getPropertyValue(enumerator.Current, this.XComplexPaths);
          object obj4 = getPropertyValue(enumerator.Current, ycomplexPath);
          if (obj3 == null)
            return;
          this.XData = Convert.ToDouble(obj3);
          xvalues.Add(this.XData);
          this.YCollection.Add((IList<double>) obj4 ?? (IList<double>) new double[0]);
          this.ActualData.Add(enumerator.Current);
        }
        while (enumerator.MoveNext());
        this.DataCount = xvalues.Count;
      }
      else if (this.XAxisValueType == ChartValueType.DateTime)
      {
        IList<double> xvalues = (IList<double>) (this.XValues as List<double>);
        do
        {
          object obj5 = getPropertyValue(enumerator.Current, this.XComplexPaths);
          object obj6 = getPropertyValue(enumerator.Current, ycomplexPath);
          if (obj5 == null)
            return;
          this.XData = ((DateTime) obj5).ToOADate();
          xvalues.Add(this.XData);
          this.YCollection.Add((IList<double>) obj6 ?? (IList<double>) new double[0]);
          this.ActualData.Add(enumerator.Current);
        }
        while (enumerator.MoveNext());
        this.DataCount = xvalues.Count;
      }
      else if (this.XAxisValueType == ChartValueType.TimeSpan)
      {
        IList<double> xvalues = (IList<double>) (this.XValues as List<double>);
        do
        {
          object obj7 = getPropertyValue(enumerator.Current, this.XComplexPaths);
          object obj8 = getPropertyValue(enumerator.Current, ycomplexPath);
          if (obj7 == null)
            return;
          this.XData = ((TimeSpan) obj7).TotalMilliseconds;
          xvalues.Add(this.XData);
          this.YCollection.Add((IList<double>) obj8 ?? (IList<double>) new double[0]);
          this.ActualData.Add(enumerator.Current);
        }
        while (enumerator.MoveNext());
        this.DataCount = xvalues.Count;
      }
    }
    this.IsPointGenerated = true;
  }

  internal override void GenerateDataTablePoints(string[] yPaths, IList<double>[] yLists)
  {
    IEnumerator enumerator = (this.ItemsSource as DataTable).Rows.GetEnumerator();
    if (enumerator.MoveNext())
    {
      for (int index = 0; index < this.UpdateStartedIndex; ++index)
        enumerator.MoveNext();
      this.XAxisValueType = ChartSeriesBase.GetDataType((enumerator.Current as DataRow).Field<object>(this.XBindingPath));
      if (this.XAxisValueType == ChartValueType.DateTime || this.XAxisValueType == ChartValueType.Double || this.XAxisValueType == ChartValueType.Logarithmic || this.XAxisValueType == ChartValueType.TimeSpan)
      {
        if (!(this.XValues is List<double>))
          this.ActualXValues = this.XValues = (IEnumerable) new List<double>();
      }
      else if (!(this.XValues is List<string>))
        this.ActualXValues = this.XValues = (IEnumerable) new List<string>();
      if (string.IsNullOrEmpty(yPaths[0]))
        return;
      if (this.XAxisValueType == ChartValueType.String)
      {
        IList<string> xvalues = (IList<string>) (this.XValues as List<string>);
        do
        {
          object obj1 = (enumerator.Current as DataRow).Field<object>(this.XBindingPath);
          object obj2 = (enumerator.Current as DataRow).Field<object>(yPaths[0]);
          xvalues.Add((string) obj1);
          this.YCollection.Add((IList<double>) obj2 ?? (IList<double>) new double[0]);
          this.ActualData.Add(enumerator.Current);
        }
        while (enumerator.MoveNext());
        this.DataCount = xvalues.Count;
      }
      else if (this.XAxisValueType == ChartValueType.Double || this.XAxisValueType == ChartValueType.Logarithmic)
      {
        IList<double> xvalues = (IList<double>) (this.XValues as List<double>);
        do
        {
          object obj3 = (enumerator.Current as DataRow).Field<object>(this.XBindingPath);
          object obj4 = (enumerator.Current as DataRow).Field<object>(yPaths[0]);
          this.XData = Convert.ToDouble(obj3 ?? (object) double.NaN);
          xvalues.Add(this.XData);
          this.YCollection.Add((IList<double>) obj4 ?? (IList<double>) new double[0]);
          this.ActualData.Add(enumerator.Current);
        }
        while (enumerator.MoveNext());
        this.DataCount = xvalues.Count;
      }
      else if (this.XAxisValueType == ChartValueType.DateTime)
      {
        IList<double> xvalues = (IList<double>) (this.XValues as List<double>);
        do
        {
          object obj5 = (enumerator.Current as DataRow).Field<object>(this.XBindingPath);
          object obj6 = (enumerator.Current as DataRow).Field<object>(yPaths[0]);
          this.XData = ((DateTime) obj5).ToOADate();
          xvalues.Add(this.XData);
          this.YCollection.Add((IList<double>) obj6 ?? (IList<double>) new double[0]);
          this.ActualData.Add(enumerator.Current);
        }
        while (enumerator.MoveNext());
        this.DataCount = xvalues.Count;
      }
      else if (this.XAxisValueType == ChartValueType.TimeSpan)
      {
        IList<double> xvalues = (IList<double>) (this.XValues as List<double>);
        do
        {
          object obj7 = (enumerator.Current as DataRow).Field<object>(this.XBindingPath);
          object obj8 = (enumerator.Current as DataRow).Field<object>(yPaths[0]);
          this.XData = ((TimeSpan) obj7).TotalMilliseconds;
          xvalues.Add(((TimeSpan) obj7).TotalMilliseconds);
          this.YCollection.Add((IList<double>) obj8 ?? (IList<double>) new double[0]);
          this.ActualData.Add(enumerator.Current);
        }
        while (enumerator.MoveNext());
        this.DataCount = xvalues.Count;
      }
    }
    this.IsPointGenerated = true;
  }

  internal override void OnDataCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
  {
  }

  internal override void DataTableRowChanged(object sender, DataRowChangeEventArgs e)
  {
  }

  protected internal override void GeneratePoints()
  {
    if (this.YCollection != null)
      this.YCollection.Clear();
    else
      this.YCollection = new List<IList<double>>();
    base.GeneratePoints();
  }

  protected override ChartSegment CreateSegment() => (ChartSegment) new BoxAndWhiskerSegment();

  internal override DataTemplate GetDefaultTooltipTemplate()
  {
    if (this.ToolTipTag is BoxAndWhiskerSegment)
    {
      if (this.boxwhiskerTooltipTemplate == null)
        this.boxwhiskerTooltipTemplate = ChartDictionaries.GenericCommonDictionary[(object) "BoxWhiskerTooltipTemplate"] as DataTemplate;
      return this.boxwhiskerTooltipTemplate;
    }
    if (this.OutlierTooltipTemplate == null)
      this.OutlierTooltipTemplate = ChartDictionaries.GenericCommonDictionary[(object) "DefaultTooltipTemplate"] as DataTemplate;
    return this.OutlierTooltipTemplate;
  }

  protected override void OnBindingPathChanged(DependencyPropertyChangedEventArgs args)
  {
    base.OnBindingPathChanged(args);
  }

  protected override void OnDataSourceChanged(IEnumerable oldValue, IEnumerable newValue)
  {
    if (this.YCollection != null)
      this.YCollection.Clear();
    else
      this.YCollection = new List<IList<double>>();
    base.OnDataSourceChanged(oldValue, newValue);
  }

  protected override void ClearUnUsedSegments(int startIndex)
  {
    List<ChartSegment> chartSegmentList = new List<ChartSegment>();
    foreach (ChartSegment chartSegment in this.Segments.Where<ChartSegment>((System.Func<ChartSegment, bool>) (item => item is ScatterSegment)))
      chartSegmentList.Add(chartSegment);
    foreach (ChartSegment chartSegment in chartSegmentList)
      this.Segments.Remove(chartSegment);
    if (this.Segments.Count <= startIndex)
      return;
    int count = this.Segments.Count;
    for (int index = startIndex; index < count; ++index)
      this.Segments.RemoveAt(startIndex);
  }

  protected override DependencyObject CloneSeries(DependencyObject obj)
  {
    BoxAndWhiskerSeries andWhiskerSeries = new BoxAndWhiskerSeries();
    andWhiskerSeries.SegmentSpacing = this.SegmentSpacing;
    andWhiskerSeries.SelectedIndex = this.SelectedIndex;
    andWhiskerSeries.SegmentSelectionBrush = this.SegmentSelectionBrush;
    andWhiskerSeries.SeriesSelectionBrush = this.SeriesSelectionBrush;
    andWhiskerSeries.BoxPlotMode = this.BoxPlotMode;
    return base.CloneSeries((DependencyObject) andWhiskerSeries);
  }

  protected double CalculateSegmentSpacing(double spacing, double Right, double Left)
  {
    double num = (Right - Left) * spacing / 2.0;
    Left += num;
    Right -= num;
    return Left;
  }

  private static void OnShowMedianChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    BoxAndWhiskerSeries andWhiskerSeries = d as BoxAndWhiskerSeries;
    IEnumerable<ChartSegment> chartSegments = andWhiskerSeries.Segments.Where<ChartSegment>((System.Func<ChartSegment, bool>) (x => x is BoxAndWhiskerSegment));
    bool newValue = (bool) e.NewValue;
    foreach (BoxAndWhiskerSegment andWhiskerSegment in chartSegments)
      andWhiskerSegment.UpdateMeanSymbol(andWhiskerSeries.ChartTransformer, newValue);
  }

  private static void OnYBindingPathChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    (d as BoxAndWhiskerSeries).OnBindingPathChanged(e);
  }

  private static void OnOutlierTemplateChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    BoxAndWhiskerSeries andWhiskerSeries = d as BoxAndWhiskerSeries;
    if (andWhiskerSeries.Area == null)
      return;
    andWhiskerSeries.Segments.Clear();
    andWhiskerSeries.Area.ScheduleUpdate();
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

  private static void OnSegmentSpacingChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    BoxAndWhiskerSeries andWhiskerSeries = d as BoxAndWhiskerSeries;
    if (andWhiskerSeries.Area == null)
      return;
    andWhiskerSeries.Area.ScheduleUpdate();
  }

  private static void OnPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    (d as BoxAndWhiskerSeries).UpdateArea();
  }

  private static void BindProperties(ScatterSegment scatterSegment, ChartSegment currentSegment)
  {
    BindingOperations.SetBinding((DependencyObject) scatterSegment, ChartSegment.InteriorProperty, (BindingBase) new Binding()
    {
      Source = (object) currentSegment,
      Path = new PropertyPath("Interior", new object[0])
    });
  }

  private static double GetExclusiveQuartileValue(double[] ylist, int count, double percentile)
  {
    if (count == 0)
      return 0.0;
    if (count == 1)
      return ylist[0];
    double num1 = percentile * (double) (count + 1);
    int index = (int) Math.Abs(num1);
    double num2 = num1 - (double) index;
    return index != 0 ? (index <= count - 1 ? num2 * (ylist[index] - ylist[index - 1]) + ylist[index - 1] : ylist[count - 1]) : ylist[0];
  }

  private static double GetInclusiveQuartileValue(double[] ylist, int count, double percentile)
  {
    if (count == 0)
      return 0.0;
    if (count == 1)
      return ylist[0];
    double num = percentile * (double) (count - 1);
    int index = (int) Math.Abs(num);
    return (num - (double) index) * (ylist[index + 1] - ylist[index]) + ylist[index];
  }

  private static void GetMinMaxandOutlier(
    double lowerQuartile,
    double upperQuartile,
    double[] ylist,
    out double minimum,
    out double maximum,
    List<double> outliers)
  {
    minimum = 0.0;
    maximum = 0.0;
    double num = 1.5 * (upperQuartile - lowerQuartile);
    for (int index = 0; index < ylist.Length; ++index)
    {
      if (ylist[index] < lowerQuartile - num)
      {
        outliers.Add(ylist[index]);
      }
      else
      {
        minimum = ylist[index];
        break;
      }
    }
    for (int index = ylist.Length - 1; index >= 0; --index)
    {
      if (ylist[index] > upperQuartile + num)
      {
        outliers.Add(ylist[index]);
      }
      else
      {
        maximum = ylist[index];
        break;
      }
    }
  }

  private object GetBoxAndWhiskerSegment(ChartAdornment adornment)
  {
    return (object) this.Segments.Where<ChartSegment>((System.Func<ChartSegment, bool>) (segment => segment is ScatterSegment && (segment as ScatterSegment).XData == adornment.XData && (segment as ScatterSegment).YData == adornment.YData)).FirstOrDefault<ChartSegment>() ?? this.GetSegment(adornment.Item);
  }

  private void UpdateAdornmentLabelPositiion()
  {
    if (this.IsActualTransposed)
    {
      this.topLabelPosition = this.ActualYAxis.IsInversed ? ActualLabelPosition.Left : ActualLabelPosition.Right;
      this.bottomLabelPosition = this.ActualYAxis.IsInversed ? ActualLabelPosition.Right : ActualLabelPosition.Left;
    }
    else
    {
      this.topLabelPosition = this.ActualYAxis.IsInversed ? ActualLabelPosition.Bottom : ActualLabelPosition.Top;
      this.bottomLabelPosition = this.ActualYAxis.IsInversed ? ActualLabelPosition.Top : ActualLabelPosition.Bottom;
    }
  }

  private void UpdateWhiskerWidth()
  {
    if (!this.WhiskerWidth.HasValue)
    {
      this.whiskerWidth = 1.0;
    }
    else
    {
      double? whiskerWidth1 = this.WhiskerWidth;
      if ((whiskerWidth1.GetValueOrDefault() >= 0.0 ? 0 : (whiskerWidth1.HasValue ? 1 : 0)) != 0)
      {
        this.whiskerWidth = 0.0;
      }
      else
      {
        double? whiskerWidth2 = this.WhiskerWidth;
        if ((whiskerWidth2.GetValueOrDefault() <= 1.0 ? 0 : (whiskerWidth2.HasValue ? 1 : 0)) != 0)
          this.whiskerWidth = 1.0;
        else
          this.whiskerWidth = this.WhiskerWidth.Value;
      }
    }
  }

  private void SetBoxWhiskerAdornments(
    DoubleRange sbsInfo,
    double xValue,
    double minimum,
    double maximum,
    double x1,
    double median,
    double lowerQuartile,
    double upperQuartile,
    int index)
  {
    if (this.AdornmentsInfo == null)
      return;
    double num = sbsInfo.Delta / 2.0;
    int index1 = index * 5;
    this.Adornments[index1].SetData(xValue, minimum, x1 + num, minimum);
    this.Adornments[index1].ActualLabelPosition = this.bottomLabelPosition;
    this.Adornments[index1].Item = this.ActualData[index];
    int index2;
    this.Adornments[index2 = index1 + 1].SetData(xValue, lowerQuartile, x1 + num, lowerQuartile);
    this.Adornments[index2].ActualLabelPosition = this.bottomLabelPosition;
    this.Adornments[index2].Item = this.ActualData[index];
    int index3;
    this.Adornments[index3 = index2 + 1].SetData(xValue, median, x1 + num, median);
    this.Adornments[index3].ActualLabelPosition = this.topLabelPosition;
    this.Adornments[index3].Item = this.ActualData[index];
    int index4;
    this.Adornments[index4 = index3 + 1].SetData(xValue, upperQuartile, x1 + num, upperQuartile);
    this.Adornments[index4].ActualLabelPosition = this.topLabelPosition;
    this.Adornments[index4].Item = this.ActualData[index];
    int index5;
    this.Adornments[index5 = index4 + 1].SetData(xValue, maximum, x1 + num, maximum);
    this.Adornments[index5].ActualLabelPosition = this.topLabelPosition;
    this.Adornments[index5].Item = this.ActualData[index];
  }

  private void AddBoxWhiskerAdornments(
    DoubleRange sbsInfo,
    double xValue,
    double minimum,
    double maximum,
    double x1,
    double median,
    double lowerQuartile,
    double upperQuartile,
    int index)
  {
    if (this.AdornmentsInfo == null)
      return;
    double num = sbsInfo.Delta / 2.0;
    ChartAdornment adornment1 = this.CreateAdornment((AdornmentSeries) this, xValue, minimum, x1 + num, minimum);
    adornment1.ActualLabelPosition = this.bottomLabelPosition;
    adornment1.Item = this.ActualData[index];
    this.Adornments.Add(adornment1);
    ChartAdornment adornment2 = this.CreateAdornment((AdornmentSeries) this, xValue, lowerQuartile, x1 + num, lowerQuartile);
    adornment2.ActualLabelPosition = this.bottomLabelPosition;
    adornment2.Item = this.ActualData[index];
    this.Adornments.Add(adornment2);
    ChartAdornment adornment3 = this.CreateAdornment((AdornmentSeries) this, xValue, median, x1 + num, median);
    adornment3.ActualLabelPosition = this.topLabelPosition;
    adornment3.Item = this.ActualData[index];
    this.Adornments.Add(adornment3);
    ChartAdornment adornment4 = this.CreateAdornment((AdornmentSeries) this, xValue, upperQuartile, x1 + num, upperQuartile);
    adornment4.ActualLabelPosition = this.topLabelPosition;
    adornment4.Item = this.ActualData[index];
    this.Adornments.Add(adornment4);
    ChartAdornment adornment5 = this.CreateAdornment((AdornmentSeries) this, xValue, maximum, x1 + num, maximum);
    adornment5.ActualLabelPosition = this.topLabelPosition;
    adornment5.Item = this.ActualData[index];
    this.Adornments.Add(adornment5);
  }

  private void GetQuartileValues(
    double[] ylist,
    int len,
    out double lowerQuartile,
    out double upperQuartile)
  {
    if (len == 1)
    {
      lowerQuartile = ylist[0];
      upperQuartile = ylist[0];
    }
    else
    {
      int length = len / 2;
      double[] numArray1 = new double[length];
      double[] numArray2 = new double[length];
      Array.Copy((Array) ylist, 0, (Array) numArray1, 0, length);
      Array.Copy((Array) ylist, this.isEvenList ? length : length + 1, (Array) numArray2, 0, length);
      lowerQuartile = BoxAndWhiskerSeries.GetMedianValue(numArray1);
      upperQuartile = BoxAndWhiskerSeries.GetMedianValue(numArray2);
    }
  }

  private static double GetMedianValue(double[] ylist)
  {
    int length = ylist.Length;
    switch (length)
    {
      case 0:
        return 0.0;
      case 1:
        return ylist[0];
      default:
        int index = (int) Math.Round((double) length / 2.0, MidpointRounding.AwayFromZero);
        return length % 2 != 0 ? ylist[index - 1] : (ylist[index - 1] + ylist[index]) / 2.0;
    }
  }
}
