// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.BollingerBandIndicator
// Assembly: Syncfusion.SfChart.WPF, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 279D70C6-7936-4F3E-B6CF-79EFEF734826
// Assembly location: C:\Program Files\PDFgear\Syncfusion.SfChart.WPF.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Media;

#nullable disable
namespace Syncfusion.UI.Xaml.Charts;

public class BollingerBandIndicator : FinancialTechnicalIndicator
{
  private IList<double> CloseValues = (IList<double>) new List<double>();
  private List<double> xValues;
  private List<double> upperXPoints = new List<double>();
  private List<double> upperYPoints = new List<double>();
  private List<double> lowerXPoints = new List<double>();
  private List<double> lowerYPoints = new List<double>();
  private List<double> signalXPoints = new List<double>();
  private List<double> signalYPoints = new List<double>();
  private TechnicalIndicatorSegment upperLineSegment;
  private TechnicalIndicatorSegment lowerLineSegment;
  private TechnicalIndicatorSegment signalLineSegment;
  public static readonly DependencyProperty PeriodProperty = DependencyProperty.Register(nameof (Period), typeof (int), typeof (BollingerBandIndicator), new PropertyMetadata((object) 20, new PropertyChangedCallback(BollingerBandIndicator.OnMovingAverageChanged)));
  public static readonly DependencyProperty UpperLineColorProperty = DependencyProperty.Register(nameof (UpperLineColor), typeof (Brush), typeof (BollingerBandIndicator), new PropertyMetadata((object) new SolidColorBrush(Colors.Red), new PropertyChangedCallback(BollingerBandIndicator.OnColorChanged)));
  public static readonly DependencyProperty LowerLineColorProperty = DependencyProperty.Register(nameof (LowerLineColor), typeof (Brush), typeof (BollingerBandIndicator), new PropertyMetadata((object) new SolidColorBrush(Colors.Red), new PropertyChangedCallback(BollingerBandIndicator.OnColorChanged)));
  public static readonly DependencyProperty SignalLineColorProperty = DependencyProperty.Register(nameof (SignalLineColor), typeof (Brush), typeof (BollingerBandIndicator), new PropertyMetadata((object) new SolidColorBrush(Colors.Blue), new PropertyChangedCallback(BollingerBandIndicator.OnColorChanged)));

  public int Period
  {
    get => (int) this.GetValue(BollingerBandIndicator.PeriodProperty);
    set => this.SetValue(BollingerBandIndicator.PeriodProperty, (object) value);
  }

  public Brush UpperLineColor
  {
    get => (Brush) this.GetValue(BollingerBandIndicator.UpperLineColorProperty);
    set => this.SetValue(BollingerBandIndicator.UpperLineColorProperty, (object) value);
  }

  public Brush LowerLineColor
  {
    get => (Brush) this.GetValue(BollingerBandIndicator.LowerLineColorProperty);
    set => this.SetValue(BollingerBandIndicator.LowerLineColorProperty, (object) value);
  }

  public Brush SignalLineColor
  {
    get => (Brush) this.GetValue(BollingerBandIndicator.SignalLineColorProperty);
    set => this.SetValue(BollingerBandIndicator.SignalLineColorProperty, (object) value);
  }

  internal override void SetIndicatorInfo(
    ChartPointInfo info,
    List<double> yValue,
    bool seriesPalette)
  {
    if (yValue.Count <= 0)
      return;
    info.UpperLine = double.IsNaN(yValue[0]) ? "null" : Math.Round(yValue[0], 2).ToString();
    info.LowerLine = double.IsNaN(yValue[1]) ? "null" : Math.Round(yValue[1], 2).ToString();
    info.SignalLine = double.IsNaN(yValue[2]) ? "null" : Math.Round(yValue[2], 2).ToString();
  }

  private static void OnColorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    (d as BollingerBandIndicator).UpdateArea();
  }

  private static void OnMovingAverageChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    (d as BollingerBandIndicator).UpdateArea();
  }

  protected override void OnDataSourceChanged(IEnumerable oldValue, IEnumerable newValue)
  {
    base.OnDataSourceChanged(oldValue, newValue);
    this.signalLineSegment = (TechnicalIndicatorSegment) null;
    this.upperLineSegment = (TechnicalIndicatorSegment) null;
    this.lowerLineSegment = (TechnicalIndicatorSegment) null;
    this.CloseValues.Clear();
    this.GeneratePoints(new string[1]{ this.Close }, this.CloseValues);
    this.UpdateArea();
  }

  protected override void OnBindingPathChanged(DependencyPropertyChangedEventArgs args)
  {
    this.CloseValues.Clear();
    base.OnBindingPathChanged(args);
  }

  protected internal override void SetSeriesItemSource(ChartSeriesBase series)
  {
    if (series.ActualSeriesYValues.Length <= 0)
      return;
    this.ActualXValues = this.Clone(series.ActualXValues);
    this.CloseValues = (IList<double>) ChartSeriesBase.Clone(series.ActualSeriesYValues[0]);
    this.Area.ScheduleUpdate();
  }

  protected internal override void GeneratePoints()
  {
    this.GeneratePoints(new string[1]{ this.Close }, this.CloseValues);
  }

  public override void CreateSegments()
  {
    this.xValues = this.GetXValues();
    if (this.Period < 1)
      return;
    this.Period = this.Period < this.xValues.Count ? this.Period : this.xValues.Count - 1;
    this.AddBollinger();
    List<double> xVals1 = new List<double>();
    List<double> yVals1 = new List<double>();
    List<double> xVals2 = new List<double>();
    List<double> yVals2 = new List<double>();
    List<double> xVals3 = new List<double>();
    List<double> yVals3 = new List<double>();
    for (int index = 0; index < this.upperXPoints.Count; ++index)
    {
      xVals1.Add(this.upperXPoints[index]);
      yVals1.Add(this.upperYPoints[index]);
      xVals2.Add(this.lowerXPoints[index]);
      yVals2.Add(this.lowerYPoints[index]);
      xVals3.Add(this.signalXPoints[index]);
      yVals3.Add(this.signalYPoints[index]);
    }
    if (this.upperLineSegment == null || this.lowerLineSegment == null || this.signalLineSegment == null)
    {
      this.upperLineSegment = new TechnicalIndicatorSegment(xVals1, yVals1, this.UpperLineColor, (ChartSeriesBase) this, this.Period);
      this.upperLineSegment.SetValues(xVals1, yVals1, this.UpperLineColor, (ChartSeriesBase) this, this.Period);
      this.Segments.Add((ChartSegment) this.upperLineSegment);
      this.lowerLineSegment = new TechnicalIndicatorSegment(xVals2, yVals2, this.LowerLineColor, (ChartSeriesBase) this, this.Period);
      this.lowerLineSegment.SetValues(xVals2, yVals2, this.LowerLineColor, (ChartSeriesBase) this, this.Period);
      this.Segments.Add((ChartSegment) this.lowerLineSegment);
      this.signalLineSegment = new TechnicalIndicatorSegment(xVals3, yVals3, this.SignalLineColor, (ChartSeriesBase) this, this.Period);
      this.signalLineSegment.SetValues(xVals3, yVals3, this.SignalLineColor, (ChartSeriesBase) this, this.Period);
      this.Segments.Add((ChartSegment) this.signalLineSegment);
    }
    else
    {
      this.upperLineSegment.SetData((IList<double>) xVals1, (IList<double>) yVals1, this.UpperLineColor, this.Period);
      this.upperLineSegment.SetRange();
      this.lowerLineSegment.SetData((IList<double>) xVals2, (IList<double>) yVals2, this.LowerLineColor, this.Period);
      this.lowerLineSegment.SetRange();
      this.signalLineSegment.SetData((IList<double>) xVals3, (IList<double>) yVals3, this.SignalLineColor, this.Period);
      this.signalLineSegment.SetRange();
    }
  }

  public override void UpdateSegments(int index, NotifyCollectionChangedAction action)
  {
    this.Area.ScheduleUpdate();
  }

  private void AddBollinger()
  {
    this.signalXPoints.Clear();
    this.signalYPoints.Clear();
    this.upperXPoints.Clear();
    this.upperYPoints.Clear();
    this.lowerXPoints.Clear();
    this.lowerYPoints.Clear();
    this.ComputeSMAandBollingerBands(this.Period, BollingerBandIndicator.GetDouble(1, new object[2]
    {
      (object) this.Period,
      (object) 2.0
    }, 2.0));
  }

  private static int GetInt(int i, object[] parms, int def)
  {
    int result = def;
    if (parms != null && parms.GetLength(0) > i && parms[i] != null)
      int.TryParse(parms[i].ToString(), out result);
    return result;
  }

  private static double GetDouble(int i, object[] parms, double def)
  {
    double result = def;
    if (parms != null && parms.GetLength(0) > i && parms[i] != null)
      double.TryParse(parms[i].ToString(), out result);
    return result;
  }

  private void ComputeSMAandBollingerBands(int len, double bandWidth)
  {
    double num1 = 0.0;
    double[] numArray = new double[this.DataCount];
    double num2 = 0.0;
    for (int index = 0; index < len; ++index)
      num1 += this.CloseValues[index];
    double num3 = num1 / (double) len;
    for (int index1 = 0; index1 < this.DataCount; ++index1)
    {
      if (index1 >= len - 1)
      {
        double xValue1;
        double num4;
        double num5;
        if (index1 - len >= 0)
        {
          double num6 = this.CloseValues[index1] - this.CloseValues[index1 - len];
          num1 += num6;
          num3 = num1 / (double) len;
          xValue1 = this.xValues[index1];
          num4 = num3;
          numArray[index1] = Math.Pow(this.CloseValues[index1] - num3, 2.0);
          num2 += numArray[index1] - numArray[index1 - len];
          num5 = Math.Sqrt(num2 / (double) len);
        }
        else
        {
          numArray[index1] = Math.Pow(this.CloseValues[index1] - num3, 2.0);
          num2 += numArray[index1];
          num5 = Math.Sqrt(num2 / (double) len);
          for (int index2 = 0; index2 < len - 1; ++index2)
          {
            double xValue2 = this.xValues[index1];
            double num7 = num3;
            this.signalXPoints.Add(xValue2);
            this.signalYPoints.Add(num7);
            this.upperXPoints.Add(xValue2);
            this.lowerXPoints.Add(xValue2);
            this.upperYPoints.Add(this.signalYPoints[index2] + bandWidth * num5);
            this.lowerYPoints.Add(this.signalYPoints[index2] - bandWidth * num5);
          }
          xValue1 = this.xValues[index1];
          num4 = num3;
        }
        this.signalXPoints.Add(xValue1);
        this.signalYPoints.Add(num4);
        this.upperXPoints.Add(xValue1);
        this.lowerXPoints.Add(xValue1);
        this.upperYPoints.Add(num4 + bandWidth * num5);
        this.lowerYPoints.Add(num4 - bandWidth * num5);
      }
      else
      {
        numArray[index1] = Math.Pow(this.CloseValues[index1] - num3, 2.0);
        num2 += numArray[index1];
      }
    }
  }

  protected override DependencyObject CloneSeries(DependencyObject obj)
  {
    obj = (DependencyObject) new BollingerBandIndicator()
    {
      Period = this.Period,
      SignalLineColor = this.SignalLineColor,
      LowerLineColor = this.LowerLineColor,
      UpperLineColor = this.UpperLineColor
    };
    return base.CloneSeries(obj);
  }

  protected override ChartSegment CreateSegment() => (ChartSegment) null;
}
