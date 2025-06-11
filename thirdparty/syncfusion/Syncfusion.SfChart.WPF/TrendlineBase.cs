// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.TrendlineBase
// Assembly: Syncfusion.SfChart.WPF, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 279D70C6-7936-4F3E-B6CF-79EFEF734826
// Assembly location: C:\Program Files\PDFgear\Syncfusion.SfChart.WPF.dll

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

#nullable disable
namespace Syncfusion.UI.Xaml.Charts;

public abstract class TrendlineBase : Control
{
  private IList<double> _xValues;
  private double _xMin;
  private double _xMax;
  private IList<double> trendXValues;
  private IList<double> trendYValues;
  private List<double> trendXSegmentValues;
  private List<double> trendYSegmentValues;
  private IList<double> xNonEmptyValues;
  private IList<double> yNonEmptyValues;
  public static readonly DependencyProperty IsTrendlineVisibleProperty = DependencyProperty.Register(nameof (IsTrendlineVisible), typeof (bool), typeof (TrendlineBase), new PropertyMetadata((object) true, new PropertyChangedCallback(TrendlineBase.OnIsTrendlineVisibleChanged)));
  public static readonly DependencyProperty VisibilityOnLegendProperty = DependencyProperty.Register(nameof (VisibilityOnLegend), typeof (Visibility), typeof (TrendlineBase), new PropertyMetadata((object) Visibility.Visible, new PropertyChangedCallback(TrendlineBase.OnVisibilityOnLegendChanged)));
  public static readonly DependencyProperty LegendIconTemplateProperty = DependencyProperty.Register(nameof (LegendIconTemplate), typeof (DataTemplate), typeof (TrendlineBase), new PropertyMetadata((PropertyChangedCallback) null));
  public static readonly DependencyProperty LegendIconProperty = DependencyProperty.Register(nameof (LegendIcon), typeof (ChartLegendIcon), typeof (TrendlineBase), new PropertyMetadata((object) ChartLegendIcon.SeriesType, new PropertyChangedCallback(TrendlineBase.OnLegendIconChanged)));
  public static readonly DependencyProperty LabelProperty = DependencyProperty.Register(nameof (Label), typeof (string), typeof (TrendlineBase), new PropertyMetadata((object) string.Empty));
  public static readonly DependencyProperty TypeProperty = DependencyProperty.Register(nameof (Type), typeof (TrendlineType), typeof (TrendlineBase), new PropertyMetadata((object) TrendlineType.Linear, new PropertyChangedCallback(TrendlineBase.OnTypeChanged)));
  public static readonly DependencyProperty StrokeProperty = DependencyProperty.Register(nameof (Stroke), typeof (Brush), typeof (TrendlineBase), new PropertyMetadata((object) null, new PropertyChangedCallback(TrendlineBase.OnStrokeChanged)));
  public static readonly DependencyProperty StrokeThicknessProperty = DependencyProperty.Register(nameof (StrokeThickness), typeof (double), typeof (TrendlineBase), new PropertyMetadata((object) 1.0, new PropertyChangedCallback(TrendlineBase.OnStrokeThicknessChanged)));
  public static readonly DependencyProperty StrokeDashArrayProperty = DependencyProperty.Register(nameof (StrokeDashArray), typeof (DoubleCollection), typeof (TrendlineBase), new PropertyMetadata((object) null, new PropertyChangedCallback(TrendlineBase.OnStrokeDashArrayChanged)));
  public static readonly DependencyProperty ForwardForecastProperty = DependencyProperty.Register(nameof (ForwardForecast), typeof (double), typeof (TrendlineBase), new PropertyMetadata((object) 0.0, new PropertyChangedCallback(TrendlineBase.OnTypeChanged)));
  public static readonly DependencyProperty BackwardForecastProperty = DependencyProperty.Register(nameof (BackwardForecast), typeof (double), typeof (TrendlineBase), new PropertyMetadata((object) 0.0, new PropertyChangedCallback(TrendlineBase.OnTypeChanged)));
  public static readonly DependencyProperty PolynomialOrderProperty = DependencyProperty.Register(nameof (PolynomialOrder), typeof (int), typeof (TrendlineBase), new PropertyMetadata((object) 2, new PropertyChangedCallback(TrendlineBase.OnPolynomialOrderChanged)));

  public TrendlineBase()
  {
    this.Stroke = (Brush) new SolidColorBrush(Color.FromArgb(byte.MaxValue, (byte) 35, (byte) 137, byte.MaxValue));
    this.DefaultStyleKey = (object) typeof (TrendlineBase);
    this.TrendlineSegments = new ObservableCollection<ChartSegment>();
  }

  public double Slope { get; private set; }

  public double Intercept { get; private set; }

  internal ChartSeries Series { get; set; }

  public double[] PolynomialSlopes { get; private set; }

  internal ChartTrendlinePanel TrendlinePanel { get; set; }

  internal ObservableCollection<ChartSegment> TrendlineSegments { get; set; }

  public bool IsTrendlineVisible
  {
    get => (bool) this.GetValue(TrendlineBase.IsTrendlineVisibleProperty);
    set => this.SetValue(TrendlineBase.IsTrendlineVisibleProperty, (object) value);
  }

  private static void OnIsTrendlineVisibleChanged(
    DependencyObject obj,
    DependencyPropertyChangedEventArgs args)
  {
    if (!(obj is TrendlineBase trendlineBase))
      return;
    if ((bool) args.NewValue)
      trendlineBase.Visibility = Visibility.Visible;
    else
      trendlineBase.Visibility = Visibility.Collapsed;
    if (!(trendlineBase.Series is CartesianSeries series) || series.ActualArea == null)
      return;
    series.ActualArea.ScheduleUpdate();
  }

  public Visibility VisibilityOnLegend
  {
    get => (Visibility) this.GetValue(TrendlineBase.VisibilityOnLegendProperty);
    set => this.SetValue(TrendlineBase.VisibilityOnLegendProperty, (object) value);
  }

  private static void OnVisibilityOnLegendChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    TrendlineBase trendlineBase = d as TrendlineBase;
    if (trendlineBase.Series == null || trendlineBase.Series.ActualArea == null)
      return;
    trendlineBase.Series.ActualArea.IsUpdateLegend = true;
    trendlineBase.Series.ActualArea.ScheduleUpdate();
  }

  public DataTemplate LegendIconTemplate
  {
    get => (DataTemplate) this.GetValue(TrendlineBase.LegendIconTemplateProperty);
    set => this.SetValue(TrendlineBase.LegendIconTemplateProperty, (object) value);
  }

  public ChartLegendIcon LegendIcon
  {
    get => (ChartLegendIcon) this.GetValue(TrendlineBase.LegendIconProperty);
    set => this.SetValue(TrendlineBase.LegendIconProperty, (object) value);
  }

  private static void OnLegendIconChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    if (!(d is TrendlineBase trendlineBase) || trendlineBase.Series == null)
      return;
    trendlineBase.UpdateLegendIconTemplate(true);
  }

  public string Label
  {
    get => (string) this.GetValue(TrendlineBase.LabelProperty);
    set => this.SetValue(TrendlineBase.LabelProperty, (object) value);
  }

  public TrendlineType Type
  {
    get => (TrendlineType) this.GetValue(TrendlineBase.TypeProperty);
    set => this.SetValue(TrendlineBase.TypeProperty, (object) value);
  }

  private static void OnTypeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    if (!(d is TrendlineBase trendlineBase) || trendlineBase.Series == null || e.NewValue == null)
      return;
    trendlineBase.Series.ActualArea.ScheduleUpdate();
  }

  public Brush Stroke
  {
    get => (Brush) this.GetValue(TrendlineBase.StrokeProperty);
    set => this.SetValue(TrendlineBase.StrokeProperty, (object) value);
  }

  private static void OnStrokeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    Trendline trendline = d as Trendline;
    if (e.NewValue == null || trendline == null || trendline.TrendlineSegments == null)
      return;
    foreach (ChartSegment trendlineSegment in (Collection<ChartSegment>) trendline.TrendlineSegments)
      trendlineSegment.Interior = (Brush) e.NewValue;
  }

  public double StrokeThickness
  {
    get => (double) this.GetValue(TrendlineBase.StrokeThicknessProperty);
    set => this.SetValue(TrendlineBase.StrokeThicknessProperty, (object) value);
  }

  private static void OnStrokeThicknessChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    Trendline trendline = d as Trendline;
    if (e.NewValue == null)
      return;
    foreach (ChartSegment trendlineSegment in (Collection<ChartSegment>) trendline.TrendlineSegments)
      trendlineSegment.StrokeThickness = (double) e.NewValue;
  }

  public DoubleCollection StrokeDashArray
  {
    get => (DoubleCollection) this.GetValue(TrendlineBase.StrokeDashArrayProperty);
    set => this.SetValue(TrendlineBase.StrokeDashArrayProperty, (object) value);
  }

  private static void OnStrokeDashArrayChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    TrendlineBase trendlineBase = d as TrendlineBase;
    if (e.NewValue == null)
      return;
    foreach (ChartSegment trendlineSegment in (Collection<ChartSegment>) trendlineBase.TrendlineSegments)
    {
      DoubleCollection newValue = (DoubleCollection) e.NewValue;
      if (newValue != null && newValue.Count > 0)
      {
        DoubleCollection doubleCollection = new DoubleCollection();
        foreach (double num in newValue)
          doubleCollection.Add(num);
        if (trendlineSegment is SplineSegment)
          (trendlineSegment as SplineSegment).StrokeDashArray = doubleCollection;
        else if (trendlineSegment is LineSegment)
          (trendlineSegment as LineSegment).StrokeDashArray = doubleCollection;
      }
    }
  }

  public double ForwardForecast
  {
    get => (double) this.GetValue(TrendlineBase.ForwardForecastProperty);
    set => this.SetValue(TrendlineBase.ForwardForecastProperty, (object) value);
  }

  public double BackwardForecast
  {
    get => (double) this.GetValue(TrendlineBase.BackwardForecastProperty);
    set => this.SetValue(TrendlineBase.BackwardForecastProperty, (object) value);
  }

  public int PolynomialOrder
  {
    get => (int) this.GetValue(TrendlineBase.PolynomialOrderProperty);
    set => this.SetValue(TrendlineBase.PolynomialOrderProperty, (object) value);
  }

  private static void OnPolynomialOrderChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    if (!(d is TrendlineBase trendlineBase) || trendlineBase.Series == null || e.NewValue == null || trendlineBase.Type != TrendlineType.Polynomial || trendlineBase.Series.ActualData.Count <= 2 || trendlineBase.Series.ActualData.Count <= (int) e.NewValue)
      return;
    trendlineBase.Series.ActualArea.ScheduleUpdate();
  }

  private void GenerateNonEmptyXandYValues()
  {
    this.yNonEmptyValues = (IList<double>) new List<double>();
    this.xNonEmptyValues = (IList<double>) new List<double>();
    IList<double> doubleList = !(this.Series is FinancialSeriesBase) ? (!(this.Series is RangeSeriesBase) ? (this.Series as XyDataSeries).YValues : (this.Series as RangeSeriesBase).LowValues) : (this.Series as FinancialSeriesBase).CloseValues;
    if (doubleList != null && doubleList.Contains(double.NaN))
    {
      for (int index = 0; index < doubleList.Count; ++index)
      {
        if (!double.IsNaN(doubleList[index]))
        {
          this.yNonEmptyValues.Add(doubleList[index]);
          this.xNonEmptyValues.Add(this._xValues[index]);
        }
      }
    }
    else
    {
      this.yNonEmptyValues = doubleList;
      this.xNonEmptyValues = this._xValues;
    }
  }

  public override void OnApplyTemplate()
  {
    base.OnApplyTemplate();
    this.TrendlinePanel = this.GetTemplateChild("trendlinePanel") as ChartTrendlinePanel;
    if (this.TrendlinePanel == null)
      return;
    this.TrendlinePanel.Trend = this;
    foreach (ChartSegment trendlineSegment in (Collection<ChartSegment>) this.TrendlineSegments)
    {
      if (!trendlineSegment.IsAddedToVisualTree)
      {
        UIElement visual = trendlineSegment.CreateVisual(Size.Empty);
        if (visual != null)
        {
          this.TrendlinePanel.Children.Add(visual);
          trendlineSegment.IsAddedToVisualTree = true;
        }
      }
    }
  }

  internal void UpdateLegendIconTemplate(bool iconChanged)
  {
    string key = this.LegendIcon.ToString();
    if (this.LegendIcon == ChartLegendIcon.SeriesType)
      key = this.Type.ToString().Replace("Trend", "");
    if (this.LegendIconTemplate != null && !iconChanged || !ChartDictionaries.GenericLegendDictionary.Contains((object) key))
      return;
    this.LegendIconTemplate = ChartDictionaries.GenericLegendDictionary[(object) key] as DataTemplate;
  }

  internal void UpdateElements()
  {
    if (this.Series == null)
      return;
    this._xValues = (IList<double>) this.Series.GetXValues();
    this.TrendlineSegments.Clear();
    this.GenerateNonEmptyXandYValues();
    if (this.xNonEmptyValues.Count >= 2)
    {
      if (this.Series.ActualXAxis is DateTimeAxis && (this.Series.ActualXAxis as DateTimeAxis).IntervalType == DateTimeIntervalType.Auto)
        (this.Series.ActualXAxis as DateTimeAxis).ActualRangeChanged += new EventHandler<ActualRangeChangedEventArgs>(this.Trendline_ActualRangeChanged);
      else if (this.Series.ActualXAxis is DateTimeAxis)
        (this.Series.ActualXAxis as DateTimeAxis).ActualRangeChanged -= new EventHandler<ActualRangeChangedEventArgs>(this.Trendline_ActualRangeChanged);
      this._xMin = this._xValues.Min();
      this._xMax = this._xValues.Max();
      this.CheckTrendlineType();
    }
    this.UpdateLegendIconTemplate(true);
  }

  private void CheckTrendlineType()
  {
    switch (this.Type)
    {
      case TrendlineType.Linear:
        this.UpdateTrendSource();
        this.CalculateLinearTrendline();
        break;
      case TrendlineType.Exponential:
        this.UpdateExponentialTrendSource();
        this.CalculateExponentialTrendline();
        break;
      case TrendlineType.Power:
        this.UpdatePowerTrendSource();
        this.CalculatePowerTrendline();
        break;
      case TrendlineType.Logarithmic:
        this.UpdateLogarithmicTrendSource();
        this.CalculateLogarithmicTrendline();
        break;
      case TrendlineType.Polynomial:
        if (this.Series.ActualData.Count < this.PolynomialOrder || this.PolynomialOrder <= 1 || this.PolynomialOrder > 6)
          break;
        this.UpdatePolynomialTrendSource();
        this.CalculatePolynomialTrendLine();
        break;
    }
  }

  protected virtual DependencyObject CloneTrendline(DependencyObject obj)
  {
    TrendlineBase trendlineBase = (TrendlineBase) new Trendline();
    trendlineBase.IsTrendlineVisible = this.IsTrendlineVisible;
    trendlineBase.Type = this.Type;
    trendlineBase.Label = this.Label;
    trendlineBase.Stroke = this.Stroke;
    trendlineBase.StrokeDashArray = this.StrokeDashArray;
    trendlineBase.StrokeThickness = this.StrokeThickness;
    trendlineBase.PolynomialOrder = this.PolynomialOrder;
    return (DependencyObject) trendlineBase;
  }

  public DependencyObject Clone() => this.CloneTrendline((DependencyObject) null);

  private void UpdatePolynomialTrendSource()
  {
    this.trendXValues = (IList<double>) new List<double>();
    this.trendYValues = this.yNonEmptyValues;
    for (int index = 0; index < this.trendYValues.Count; ++index)
    {
      if (this.Series.ActualXAxis is CategoryAxis || this.Series.ActualXAxis is DateTimeCategoryAxis)
        this.trendXValues.Add(this.xNonEmptyValues[index] + 1.0);
      else
        this.trendXValues.Add(this.xNonEmptyValues[index]);
    }
  }

  private static bool GaussJordanEliminiation(double[,] a, double[] b)
  {
    int length = a.GetLength(0);
    int[] numArray1 = new int[length];
    int[] numArray2 = new int[length];
    int[] numArray3 = new int[length];
    for (int index = 0; index < length; ++index)
      numArray3[index] = 0;
    for (int index1 = 0; index1 < length; ++index1)
    {
      double num1 = 0.0;
      int index2 = 0;
      int index3 = 0;
      for (int index4 = 0; index4 < length; ++index4)
      {
        if (numArray3[index4] != 1)
        {
          for (int index5 = 0; index5 < length; ++index5)
          {
            if (numArray3[index5] == 0 && Math.Abs(a[index4, index5]) >= num1)
            {
              num1 = Math.Abs(a[index4, index5]);
              index2 = index4;
              index3 = index5;
            }
          }
        }
      }
      ++numArray3[index3];
      if (index2 != index3)
      {
        for (int index6 = 0; index6 < length; ++index6)
        {
          double num2 = a[index2, index6];
          a[index2, index6] = a[index3, index6];
          a[index3, index6] = num2;
        }
        double num3 = b[index2];
        b[index2] = b[index3];
        b[index3] = num3;
      }
      numArray2[index1] = index2;
      numArray1[index1] = index3;
      if (a[index3, index3] == 0.0)
        return false;
      double num4 = 1.0 / a[index3, index3];
      a[index3, index3] = 1.0;
      for (int index7 = 0; index7 < length; ++index7)
        a[index3, index7] *= num4;
      b[index3] *= num4;
      for (int index8 = 0; index8 < length; ++index8)
      {
        if (index8 != index3)
        {
          double num5 = a[index8, index3];
          a[index8, index3] = 0.0;
          for (int index9 = 0; index9 < length; ++index9)
            a[index8, index9] -= a[index3, index9] * num5;
          b[index8] -= b[index3] * num5;
        }
      }
    }
    for (int index10 = length - 1; index10 >= 0; --index10)
    {
      if (numArray2[index10] != numArray1[index10])
      {
        for (int index11 = 0; index11 < length; ++index11)
        {
          double num = a[index11, numArray2[index10]];
          a[index11, numArray2[index10]] = a[index11, numArray1[index10]];
          a[index11, numArray1[index10]] = num;
        }
      }
    }
    return true;
  }

  private void CalculatePolynomialTrendLine()
  {
    int polynomialOrder = this.PolynomialOrder;
    this.PolynomialSlopes = new double[polynomialOrder + 1];
    for (int index = 0; index < this.trendXValues.Count; ++index)
    {
      double trendXvalue = this.trendXValues[index];
      double trendYvalue = this.trendYValues[index];
      if (!double.IsNaN(trendXvalue) && !double.IsNaN(trendYvalue))
      {
        for (int y = 0; y <= polynomialOrder; ++y)
          this.PolynomialSlopes[y] += Math.Pow(trendXvalue, (double) y) * trendYvalue;
      }
    }
    double[] numArray = new double[1 + 2 * polynomialOrder];
    double[,] a = new double[polynomialOrder + 1, polynomialOrder + 1];
    int num1 = 0;
    for (int index1 = 0; index1 < this.trendXValues.Count; ++index1)
    {
      double num2 = 1.0;
      double trendXvalue = this.trendXValues[index1];
      if (!double.IsNaN(trendXvalue) && !double.IsNaN(this.trendYValues[index1]))
      {
        for (int index2 = 0; index2 < numArray.Length; ++index2)
        {
          numArray[index2] += num2;
          num2 *= trendXvalue;
          ++num1;
        }
      }
    }
    for (int index3 = 0; index3 <= polynomialOrder; ++index3)
    {
      for (int index4 = 0; index4 <= polynomialOrder; ++index4)
        a[index3, index4] = numArray[index3 + index4];
    }
    if (!TrendlineBase.GaussJordanEliminiation(a, this.PolynomialSlopes))
      this.PolynomialSlopes = (double[]) null;
    this.CreatePolynomialSegments();
  }

  private void CreatePolynomialSegments()
  {
    this.trendXSegmentValues = new List<double>();
    this.trendYSegmentValues = new List<double>();
    double count = (double) this.trendXValues.Count;
    double a = 1.0;
    if (this.PolynomialSlopes != null)
    {
      for (int index = 1; index <= this.PolynomialSlopes.Length; ++index)
      {
        ChartAxis actualXaxis = this.Series.ActualXAxis;
        if (index == 1)
        {
          double x = this._xMin - this.BackwardForecast;
          if (this.Series.ActualXAxis is CategoryAxis || this.Series.ActualXAxis is DateTimeCategoryAxis)
          {
            this.trendXSegmentValues.Add(x);
            this.trendYSegmentValues.Add(TrendlineBase.GetPolynomialYValue(this.PolynomialSlopes, (double) index - (count - 1.0) * (this.BackwardForecast / (this._xMax - this._xMin))));
          }
          else
          {
            if (actualXaxis is DateTimeAxis dateTimeAxis)
            {
              double timeForecastValue = TrendlineBase.CalculateDateTimeForecastValue(this._xMin, -this.BackwardForecast, dateTimeAxis.IntervalType);
              this.trendXSegmentValues.Add(timeForecastValue);
              x = timeForecastValue;
            }
            else
              this.trendXSegmentValues.Add(x);
            this.trendYSegmentValues.Add(TrendlineBase.GetPolynomialYValue(this.PolynomialSlopes, x));
          }
        }
        else if (index == this.PolynomialSlopes.Length)
        {
          double x = this._xMax + this.ForwardForecast;
          if (this.Series.ActualXAxis is CategoryAxis || this.Series.ActualXAxis is DateTimeCategoryAxis)
          {
            this.trendXSegmentValues.Add(x);
            this.trendYSegmentValues.Add(TrendlineBase.GetPolynomialYValue(this.PolynomialSlopes, count + (count - 1.0) * (this.ForwardForecast / (this._xMax - this._xMin))));
          }
          else
          {
            if (actualXaxis is DateTimeAxis dateTimeAxis)
            {
              double timeForecastValue = TrendlineBase.CalculateDateTimeForecastValue(this._xMax, this.ForwardForecast, dateTimeAxis.IntervalType);
              this.trendXSegmentValues.Add(timeForecastValue);
              x = timeForecastValue;
            }
            else
              this.trendXSegmentValues.Add(x);
            this.trendYSegmentValues.Add(TrendlineBase.GetPolynomialYValue(this.PolynomialSlopes, x));
          }
        }
        else
        {
          a += (count + (count - 1.0) * (this.ForwardForecast / (this._xMax - this._xMin))) / (double) this.PolynomialSlopes.Length;
          if (count != a)
          {
            if (this.Series.ActualXAxis is CategoryAxis || this.Series.ActualXAxis is DateTimeCategoryAxis)
            {
              this.trendXSegmentValues.Add(Math.Ceiling(a) - 1.0);
              this.trendYSegmentValues.Add(TrendlineBase.GetPolynomialYValue(this.PolynomialSlopes, Math.Ceiling(Math.Ceiling(a))));
            }
            else if (count > a)
            {
              double xValue = this._xValues[(int) Math.Ceiling(a) - 1];
              this.trendXSegmentValues.Add(xValue);
              this.trendYSegmentValues.Add(TrendlineBase.GetPolynomialYValue(this.PolynomialSlopes, xValue));
            }
          }
        }
      }
    }
    this.CreateSpline();
  }

  private void UpdateLogarithmicTrendSource()
  {
    this.trendXValues = (IList<double>) new List<double>();
    this.trendYValues = this.yNonEmptyValues;
    for (int index = 0; index < this.trendYValues.Count; ++index)
    {
      if (this.Series.ActualXAxis is CategoryAxis || this.Series.ActualXAxis is DateTimeCategoryAxis)
        this.trendXValues.Add(Math.Log(this.xNonEmptyValues[index] + 1.0));
      else
        this.trendXValues.Add(Math.Log(this.xNonEmptyValues[index]));
    }
    this.CalculateSumXAndYValue();
  }

  private void CalculateLogarithmicTrendline()
  {
    int count = this.xNonEmptyValues.Count;
    if (count <= 1)
      return;
    this.CalculateTrendXSegment(this._xValues.Count);
    if (this.Series.ActualXAxis is CategoryAxis || this.Series.ActualXAxis is DateTimeCategoryAxis)
    {
      this.trendYSegmentValues.Add(this.GetLogarithmicYValue(1.0));
      this.trendYSegmentValues.Add(this.GetLogarithmicYValue(Math.Round((double) count / 2.0)));
      this.trendYSegmentValues.Add(this.GetLogarithmicYValue((double) count + this.ForwardForecast));
    }
    else
    {
      this.trendYSegmentValues.Add(this.GetLogarithmicYValue(this.trendXSegmentValues[0]));
      this.trendYSegmentValues.Add(this.GetLogarithmicYValue(this.trendXSegmentValues[1]));
      this.trendYSegmentValues.Add(this.GetLogarithmicYValue(this.trendXSegmentValues[2]));
    }
    this.CreateSpline();
  }

  private void UpdateExponentialTrendSource()
  {
    this.trendXValues = (IList<double>) new List<double>();
    this.trendYValues = (IList<double>) new List<double>();
    for (int index = 0; index < this.yNonEmptyValues.Count; ++index)
    {
      this.trendYValues.Add(Math.Log(this.yNonEmptyValues[index]));
      if (this.Series.ActualXAxis is CategoryAxis || this.Series.ActualXAxis is DateTimeCategoryAxis)
        this.trendXValues.Add(this.xNonEmptyValues[index] + 1.0);
      else
        this.trendXValues.Add(this.xNonEmptyValues[index]);
    }
    this.CalculateSumXAndYValue();
  }

  private void CalculateExponentialTrendline()
  {
    if (this.xNonEmptyValues.Count <= 1)
      return;
    this.CalculateTrendXSegment(this._xValues.Count);
    this.trendYSegmentValues.Add(this.GetExponentialYValue(this.trendXSegmentValues[0]));
    this.trendYSegmentValues.Add(this.GetExponentialYValue(this.trendXSegmentValues[1]));
    this.trendYSegmentValues.Add(this.GetExponentialYValue(this.trendXSegmentValues[2]));
    this.CreateSpline();
  }

  private void UpdatePowerTrendSource()
  {
    this.trendXValues = (IList<double>) new List<double>();
    this.trendYValues = (IList<double>) new List<double>();
    for (int index = 0; index < this.yNonEmptyValues.Count; ++index)
    {
      this.trendYValues.Add(Math.Log(this.yNonEmptyValues[index]));
      if (this.Series.ActualXAxis is CategoryAxis || this.Series.ActualXAxis is DateTimeCategoryAxis)
        this.trendXValues.Add(Math.Log(this.xNonEmptyValues[index] + 1.0));
      else
        this.trendXValues.Add(Math.Log(this.xNonEmptyValues[index]));
    }
    this.CalculateSumXAndYValue();
  }

  private void CalculatePowerTrendline()
  {
    int count = this.xNonEmptyValues.Count;
    if (count <= 1)
      return;
    this.CalculateTrendXSegment(this._xValues.Count);
    if (this.Series.ActualXAxis is CategoryAxis || this.Series.ActualXAxis is DateTimeCategoryAxis)
    {
      this.trendYSegmentValues.Add(this.GetPowerYValue(1.0));
      this.trendYSegmentValues.Add(this.GetPowerYValue(Math.Round((double) count / 2.0)));
      this.trendYSegmentValues.Add(this.GetPowerYValue((double) count + this.ForwardForecast));
    }
    else
    {
      this.trendYSegmentValues.Add(this.GetPowerYValue(this.trendXSegmentValues[0]));
      this.trendYSegmentValues.Add(this.GetPowerYValue(this.trendXSegmentValues[1]));
      this.trendYSegmentValues.Add(this.GetPowerYValue(this.trendXSegmentValues[2]));
    }
    this.CreateSpline();
  }

  private void UpdateTrendSource()
  {
    this.trendXValues = (IList<double>) new List<double>();
    this.trendYValues = this.yNonEmptyValues;
    for (int index = 0; index < this.yNonEmptyValues.Count; ++index)
      this.trendXValues.Add(this.xNonEmptyValues[index]);
    this.CalculateSumXAndYValue();
  }

  private void CalculateLinearTrendline()
  {
    int count = this._xValues.Count;
    if (count <= 0)
      return;
    LineSegment lineSegment1;
    if (this.Series.ActualXAxis is CategoryAxis || this.Series.ActualXAxis is DateTimeCategoryAxis)
    {
      LineSegment lineSegment2 = new LineSegment();
      lineSegment2.Interior = this.Stroke;
      lineSegment2.Stroke = this.Stroke;
      lineSegment2.StrokeThickness = this.StrokeThickness;
      lineSegment2.StrokeDashArray = this.StrokeDashArray;
      lineSegment1 = lineSegment2;
      lineSegment1.X1 = 0.0 - this.BackwardForecast;
      lineSegment1.X2 = (double) (count - 1) + this.ForwardForecast;
      double linearYvalue1 = this.GetLinearYValue(lineSegment1.X1);
      double linearYvalue2 = this.GetLinearYValue(lineSegment1.X2);
      lineSegment1.Y1 = linearYvalue1;
      lineSegment1.Y2 = linearYvalue2;
      lineSegment1.Item = (object) this;
      lineSegment1.SetData(lineSegment1.X1, linearYvalue1, lineSegment1.X2, linearYvalue2);
    }
    else if (this.Series.ActualXAxis is DateTimeAxis actualXaxis)
    {
      double timeForecastValue1 = TrendlineBase.CalculateDateTimeForecastValue(this._xMin, -this.BackwardForecast, actualXaxis.IntervalType);
      double timeForecastValue2 = TrendlineBase.CalculateDateTimeForecastValue(this._xMax, this.ForwardForecast, actualXaxis.IntervalType);
      LineSegment lineSegment3 = new LineSegment();
      lineSegment3.Interior = this.Stroke;
      lineSegment3.Stroke = this.Stroke;
      lineSegment3.StrokeThickness = this.StrokeThickness;
      lineSegment3.StrokeDashArray = this.StrokeDashArray;
      lineSegment1 = lineSegment3;
      lineSegment1.X1 = timeForecastValue1;
      lineSegment1.X2 = timeForecastValue2;
      double linearYvalue3 = this.GetLinearYValue(timeForecastValue1);
      double linearYvalue4 = this.GetLinearYValue(timeForecastValue2);
      lineSegment1.Y1 = linearYvalue3;
      lineSegment1.Y2 = lineSegment1.Y2Value;
      lineSegment1.Item = (object) this;
      lineSegment1.SetData(timeForecastValue1, linearYvalue3, timeForecastValue2, linearYvalue4);
    }
    else
    {
      LineSegment lineSegment4 = new LineSegment();
      lineSegment4.Interior = this.Stroke;
      lineSegment4.Stroke = this.Stroke;
      lineSegment4.StrokeThickness = this.StrokeThickness;
      lineSegment4.StrokeDashArray = this.StrokeDashArray;
      lineSegment1 = lineSegment4;
      lineSegment1.X1 = this._xMin - this.BackwardForecast;
      lineSegment1.X2 = this._xMax + this.ForwardForecast;
      double linearYvalue5 = this.GetLinearYValue(this._xMin - this.BackwardForecast);
      double linearYvalue6 = this.GetLinearYValue(this._xMax + this.ForwardForecast);
      lineSegment1.Y1 = linearYvalue5;
      lineSegment1.Y2 = linearYvalue6;
      lineSegment1.Item = (object) this;
      lineSegment1.SetData(this._xMin - this.BackwardForecast, linearYvalue5, this._xMax + this.ForwardForecast, linearYvalue6);
    }
    lineSegment1.Series = (ChartSeriesBase) this.Series;
    this.TrendlineSegments.Add((ChartSegment) lineSegment1);
  }

  private void CalculateSumXAndYValue()
  {
    int count = this.trendXValues.Count;
    double num1 = this.trendXValues.Sum<double>((Func<double, double>) (x => x));
    double num2 = this.trendXValues.Sum<double>((Func<double, double>) (x => x * x));
    double num3 = this.trendYValues.Sum<double>((Func<double, double>) (y => double.IsNaN(y) ? 0.0 : y));
    double num4 = 0.0;
    for (int index = 0; index < count; ++index)
    {
      if (!double.IsNaN(this.trendYValues[index]))
        num4 += this.trendXValues[index] * this.trendYValues[index];
    }
    this.Slope = (num4 * (double) count - num1 * num3) / (num2 * (double) count - num1 * num1);
    if (this.Type == TrendlineType.Exponential || this.Type == TrendlineType.Power)
      this.Intercept = Math.Exp((num3 - this.Slope * num1) / (double) count);
    else
      this.Intercept = (num3 - this.Slope * num1) / (double) count;
  }

  private void CalculateTrendXSegment(int n)
  {
    this.trendXSegmentValues = new List<double>();
    this.trendYSegmentValues = new List<double>();
    if (this.Series.ActualXAxis is CategoryAxis || this.Series.ActualXAxis is DateTimeCategoryAxis)
    {
      this.trendXSegmentValues.Add(-this.BackwardForecast);
      this.trendXSegmentValues.Add(Math.Round((double) n / 2.0) - 1.0);
      this.trendXSegmentValues.Add((double) (n - 1) + this.ForwardForecast);
    }
    else if (this.Series.ActualXAxis is DateTimeAxis actualXaxis)
    {
      this.trendXSegmentValues.Add(TrendlineBase.CalculateDateTimeForecastValue(this._xMin, -this.BackwardForecast, actualXaxis.IntervalType));
      this.trendXSegmentValues.Add(this._xMin + (this._xMax - this._xMin) / 2.0);
      this.trendXSegmentValues.Add(TrendlineBase.CalculateDateTimeForecastValue(this._xMax, this.ForwardForecast, actualXaxis.IntervalType));
    }
    else
    {
      this.trendXSegmentValues.Add(this._xMin - this.BackwardForecast);
      this.trendXSegmentValues.Add(this._xMin + (this._xMax - this._xMin) / 2.0);
      this.trendXSegmentValues.Add(this._xMax + this.ForwardForecast);
    }
  }

  private static double CalculateDateTimeForecastValue(
    double value,
    double forecastValue,
    DateTimeIntervalType type)
  {
    return Convert.ToDateTime(DateTimeAxisHelper.IncreaseInterval(Convert.ToDouble(value).FromOADate(), forecastValue, type)).ToOADate();
  }

  private void Trendline_ActualRangeChanged(object sender, ActualRangeChangedEventArgs e)
  {
    if (!(sender is DateTimeAxis dateTimeAxis) || dateTimeAxis.IntervalType != DateTimeIntervalType.Auto)
      return;
    if (this.Type == TrendlineType.Linear)
    {
      double timeForecastValue1 = TrendlineBase.CalculateDateTimeForecastValue(this._xMin, -this.BackwardForecast, dateTimeAxis.ActualIntervalType);
      double timeForecastValue2 = TrendlineBase.CalculateDateTimeForecastValue(this._xMax, this.ForwardForecast, dateTimeAxis.ActualIntervalType);
      double linearYvalue1 = this.GetLinearYValue(timeForecastValue1);
      double linearYvalue2 = this.GetLinearYValue(timeForecastValue2);
      this.TrendlineSegments[0].SetData(timeForecastValue1, linearYvalue1, timeForecastValue2, linearYvalue2);
    }
    else
    {
      this.trendXSegmentValues[0] = TrendlineBase.CalculateDateTimeForecastValue(this._xMin, -this.BackwardForecast, dateTimeAxis.ActualIntervalType);
      this.trendXSegmentValues[this.trendXSegmentValues.Count - 1] = TrendlineBase.CalculateDateTimeForecastValue(this._xMax, this.ForwardForecast, dateTimeAxis.ActualIntervalType);
      this.CreateSpline();
    }
    double start = this.TrendlineSegments[0].XRange.Start;
    double end = this.TrendlineSegments[this.TrendlineSegments.Count - 1].XRange.End;
    DateTime dateTime = (DateTime) e.ActualMinimum;
    if (start < dateTime.ToOADate())
      e.ActualMinimum = (object) Convert.ToDouble(start).FromOADate();
    dateTime = (DateTime) e.ActualMaximum;
    if (end <= dateTime.ToOADate())
      return;
    e.ActualMaximum = (object) Convert.ToDouble(end).FromOADate();
  }

  private double GetLinearYValue(double xValue) => this.Intercept + this.Slope * xValue;

  private double GetLogarithmicYValue(double xValue)
  {
    return this.Intercept + this.Slope * Math.Log(xValue);
  }

  private double GetExponentialYValue(double xValue)
  {
    return this.Intercept * Math.Exp(this.Slope * xValue);
  }

  private double GetPowerYValue(double xValue) => this.Intercept * Math.Pow(xValue, this.Slope);

  private static double GetPolynomialYValue(double[] a, double x)
  {
    return ((IEnumerable<double>) a).Select<double, double>((Func<double, int, double>) ((t, index) => t * Math.Pow(x, (double) index))).Sum();
  }

  private void CreateSpline()
  {
    this.TrendlineSegments.Clear();
    double[] ys2 = (double[]) null;
    this.NaturalSpline(this.trendXSegmentValues, this.trendYSegmentValues, out ys2);
    for (int index1 = 0; index1 < this.trendXSegmentValues.Count; ++index1)
    {
      int index2 = index1 + 1;
      ChartPoint point1 = new ChartPoint(this.trendXSegmentValues[index1], this.trendYSegmentValues[index1]);
      if (index2 < this.trendXSegmentValues.Count)
      {
        ChartPoint chartPoint = new ChartPoint(this.trendXSegmentValues[index2], this.trendYSegmentValues[index2]);
        ChartPoint controlPoint1;
        ChartPoint controlPoint2;
        this.GetBezierControlPoints(point1, chartPoint, ys2[index1], ys2[index2], out controlPoint1, out controlPoint2);
        TrendlineSegment trendlineSegment1 = new TrendlineSegment(point1, controlPoint1, controlPoint2, chartPoint, (ChartSeriesBase) this.Series);
        trendlineSegment1.Interior = this.Stroke;
        trendlineSegment1.StrokeDashArray = this.StrokeDashArray;
        trendlineSegment1.StrokeThickness = this.StrokeThickness;
        trendlineSegment1.Series = (ChartSeriesBase) this.Series;
        TrendlineSegment trendlineSegment2 = trendlineSegment1;
        trendlineSegment2.SetData(point1, controlPoint1, controlPoint2, chartPoint);
        this.TrendlineSegments.Add((ChartSegment) trendlineSegment2);
      }
    }
  }

  protected void NaturalSpline(List<double> xValues, List<double> yValues, out double[] ys2)
  {
    int count = xValues.Count;
    ys2 = new double[count];
    double num1 = 6.0;
    double[] numArray = new double[count];
    ys2[0] = numArray[0] = 0.0;
    ys2[count - 1] = 0.0;
    for (int index = 1; index < count - 1; ++index)
    {
      double num2 = xValues[index] - xValues[index - 1];
      double num3 = xValues[index + 1] - xValues[index - 1];
      double num4 = xValues[index + 1] - xValues[index];
      double num5 = yValues[index + 1] - yValues[index];
      double num6 = yValues[index] - yValues[index - 1];
      if (xValues[index] == xValues[index - 1] || xValues[index] == xValues[index + 1])
      {
        ys2[index] = 0.0;
        numArray[index] = 0.0;
      }
      else
      {
        double num7 = 1.0 / (num2 * ys2[index - 1] + 2.0 * num3);
        ys2[index] = -num7 * num4;
        numArray[index] = num7 * (num1 * (num5 / num4 - num6 / num2) - num2 * numArray[index - 1]);
      }
    }
    for (int index = count - 2; index >= 0; --index)
      ys2[index] = ys2[index] * ys2[index + 1] + numArray[index];
  }

  protected void GetBezierControlPoints(
    ChartPoint point1,
    ChartPoint point2,
    double ys1,
    double ys2,
    out ChartPoint controlPoint1,
    out ChartPoint controlPoint2)
  {
    double num1 = point2.X - point1.X;
    double num2 = num1 * num1;
    double num3 = 2.0 * point1.X + point2.X;
    double num4 = point1.X + 2.0 * point2.X;
    double num5 = 2.0 * point1.Y + point2.Y;
    double num6 = point1.Y + 2.0 * point2.Y;
    double y1 = 1.0 / 3.0 * (num5 - 1.0 / 3.0 * num2 * (ys1 + 0.5 * ys2));
    double y2 = 1.0 / 3.0 * (num6 - 1.0 / 3.0 * num2 * (0.5 * ys1 + ys2));
    controlPoint1 = new ChartPoint(num3 * (1.0 / 3.0), y1);
    controlPoint2 = new ChartPoint(num4 * (1.0 / 3.0), y2);
  }
}
