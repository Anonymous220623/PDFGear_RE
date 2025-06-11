// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.FinancialSeriesBase
// Assembly: Syncfusion.SfChart.WPF, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 279D70C6-7936-4F3E-B6CF-79EFEF734826
// Assembly location: C:\Program Files\PDFgear\Syncfusion.SfChart.WPF.dll

using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Media;

#nullable disable
namespace Syncfusion.UI.Xaml.Charts;

public abstract class FinancialSeriesBase : CartesianSeries
{
  public static readonly DependencyProperty HighProperty = DependencyProperty.Register(nameof (High), typeof (string), typeof (FinancialSeriesBase), new PropertyMetadata((object) null, new PropertyChangedCallback(FinancialSeriesBase.OnYPathChanged)));
  public static readonly DependencyProperty LowProperty = DependencyProperty.Register(nameof (Low), typeof (string), typeof (FinancialSeriesBase), new PropertyMetadata((object) null, new PropertyChangedCallback(FinancialSeriesBase.OnYPathChanged)));
  public static readonly DependencyProperty OpenProperty = DependencyProperty.Register(nameof (Open), typeof (string), typeof (FinancialSeriesBase), new PropertyMetadata((object) null, new PropertyChangedCallback(FinancialSeriesBase.OnYPathChanged)));
  public static readonly DependencyProperty CloseProperty = DependencyProperty.Register(nameof (Close), typeof (string), typeof (FinancialSeriesBase), new PropertyMetadata((object) null, new PropertyChangedCallback(FinancialSeriesBase.OnYPathChanged)));
  public static readonly DependencyProperty BearFillColorProperty = DependencyProperty.Register(nameof (BearFillColor), typeof (Brush), typeof (FinancialSeriesBase), new PropertyMetadata((object) new SolidColorBrush(Colors.Red), new PropertyChangedCallback(FinancialSeriesBase.OnBearFillColorPropertyChanged)));
  public static readonly DependencyProperty BullFillColorProperty = DependencyProperty.Register(nameof (BullFillColor), typeof (Brush), typeof (FinancialSeriesBase), new PropertyMetadata((object) new SolidColorBrush(Colors.Green), new PropertyChangedCallback(FinancialSeriesBase.OnBullFillColorPropertyChanged)));
  public static readonly DependencyProperty ComparisonModeProperty = DependencyProperty.Register(nameof (ComparisonMode), typeof (FinancialPrice), typeof (FinancialSeriesBase), new PropertyMetadata((object) FinancialPrice.None, new PropertyChangedCallback(FinancialSeriesBase.OnComparisonModeChanged)));
  private DataTemplate financialTooltipTemplate;

  public FinancialSeriesBase()
  {
    this.OpenValues = (IList<double>) new List<double>();
    this.HighValues = (IList<double>) new List<double>();
    this.LowValues = (IList<double>) new List<double>();
    this.CloseValues = (IList<double>) new List<double>();
  }

  public string High
  {
    get => (string) this.GetValue(FinancialSeriesBase.HighProperty);
    set => this.SetValue(FinancialSeriesBase.HighProperty, (object) value);
  }

  public string Low
  {
    get => (string) this.GetValue(FinancialSeriesBase.LowProperty);
    set => this.SetValue(FinancialSeriesBase.LowProperty, (object) value);
  }

  public string Close
  {
    get => (string) this.GetValue(FinancialSeriesBase.CloseProperty);
    set => this.SetValue(FinancialSeriesBase.CloseProperty, (object) value);
  }

  public Brush BearFillColor
  {
    get => (Brush) this.GetValue(FinancialSeriesBase.BearFillColorProperty);
    set => this.SetValue(FinancialSeriesBase.BearFillColorProperty, (object) value);
  }

  public Brush BullFillColor
  {
    get => (Brush) this.GetValue(FinancialSeriesBase.BullFillColorProperty);
    set => this.SetValue(FinancialSeriesBase.BullFillColorProperty, (object) value);
  }

  public FinancialPrice ComparisonMode
  {
    get => (FinancialPrice) this.GetValue(FinancialSeriesBase.ComparisonModeProperty);
    set => this.SetValue(FinancialSeriesBase.ComparisonModeProperty, (object) value);
  }

  public string Open
  {
    get => (string) this.GetValue(FinancialSeriesBase.OpenProperty);
    set => this.SetValue(FinancialSeriesBase.OpenProperty, (object) value);
  }

  protected internal IList<double> OpenValues { get; set; }

  protected internal IList<double> HighValues { get; set; }

  protected internal IList<double> LowValues { get; set; }

  protected internal IList<double> CloseValues { get; set; }

  protected ChartSegment Segment { get; set; }

  internal override ChartDataPointInfo GetDataPoint(Point mousePos)
  {
    double x = 0.0;
    double y = 0.0;
    double stackedYValue = double.NaN;
    this.dataPoint = (ChartDataPointInfo) null;
    this.dataPoint = new ChartDataPointInfo();
    if (this.Area.SeriesClipRect.Contains(mousePos))
    {
      this.FindNearestChartPoint(new Point(mousePos.X - this.Area.SeriesClipRect.Left, mousePos.Y - this.Area.SeriesClipRect.Top), out x, out y, out stackedYValue);
      this.dataPoint.XData = x;
      int index = !(this.ActualXAxis is CategoryAxis) || (this.ActualXAxis as CategoryAxis).IsIndexed ? this.GetXValues().IndexOf(x) : this.GroupedXValuesIndexes.IndexOf(x);
      if (index == -1)
        return (ChartDataPointInfo) null;
      if (this.ActualXAxis is CategoryAxis && !(this.ActualXAxis as CategoryAxis).IsIndexed)
      {
        if (this.GroupedSeriesYValues[0].Count > index)
          this.dataPoint.High = this.GroupedSeriesYValues[0][index];
        if (this.GroupedSeriesYValues[1].Count > index)
          this.dataPoint.Low = this.GroupedSeriesYValues[1][index];
        if (this.GroupedSeriesYValues[2].Count > index)
          this.dataPoint.Open = this.GroupedSeriesYValues[2][index];
        if (this.GroupedSeriesYValues[3].Count > index)
          this.dataPoint.Close = this.GroupedSeriesYValues[3][index];
      }
      else
      {
        if (this.ActualSeriesYValues[0].Count > index)
          this.dataPoint.High = this.ActualSeriesYValues[0][index];
        if (this.ActualSeriesYValues[1].Count > index)
          this.dataPoint.Low = this.ActualSeriesYValues[1][index];
        if (this.ActualSeriesYValues[2].Count > index)
          this.dataPoint.Open = this.ActualSeriesYValues[2][index];
        if (this.ActualSeriesYValues[3].Count > index)
          this.dataPoint.Close = this.ActualSeriesYValues[3][index];
      }
      this.dataPoint.Index = index;
      this.dataPoint.Series = (ChartSeriesBase) this;
      if (this.ActualData.Count > index)
        this.dataPoint.Item = this.ActualData[index];
    }
    return this.dataPoint;
  }

  internal override ChartDataPointInfo GetDataPoint(int index)
  {
    IList<double> doubleList = this.ActualXValues is IList<double> ? this.ActualXValues as IList<double> : (IList<double>) this.GetXValues();
    this.dataPoint = (ChartDataPointInfo) null;
    this.dataPoint = new ChartDataPointInfo();
    if (doubleList.Count > index)
      this.dataPoint.XData = this.IsIndexed ? (double) index : doubleList[index];
    if (this.ActualSeriesYValues != null && ((IEnumerable<IList<double>>) this.ActualSeriesYValues).Count<IList<double>>() > 0)
    {
      if (this.ActualSeriesYValues[0].Count > index)
        this.dataPoint.High = this.ActualSeriesYValues[0][index];
      if (this.ActualSeriesYValues[1].Count > index)
        this.dataPoint.Low = this.ActualSeriesYValues[1][index];
      if (this.ActualSeriesYValues[2].Count > index)
        this.dataPoint.Open = this.ActualSeriesYValues[2][index];
      if (this.ActualSeriesYValues[3].Count > index)
        this.dataPoint.Close = this.ActualSeriesYValues[3][index];
    }
    this.dataPoint.Index = index;
    this.dataPoint.Series = (ChartSeriesBase) this;
    if (this.ActualData.Count > index)
      this.dataPoint.Item = this.ActualData[index];
    return this.dataPoint;
  }

  internal override void ReValidateYValues(List<int>[] emptyPointIndexs)
  {
    foreach (int index in emptyPointIndexs[0])
      this.HighValues[index] = double.NaN;
    foreach (int index in emptyPointIndexs[1])
      this.LowValues[index] = double.NaN;
    foreach (int index in emptyPointIndexs[2])
      this.OpenValues[index] = double.NaN;
    foreach (int index in emptyPointIndexs[3])
      this.CloseValues[index] = double.NaN;
  }

  internal override void ValidateYValues()
  {
    foreach (double highValue in (IEnumerable<double>) this.HighValues)
    {
      if (double.IsNaN(highValue) && this.ShowEmptyPoints)
      {
        this.ValidateDataPoints(this.HighValues);
        break;
      }
    }
    foreach (double lowValue in (IEnumerable<double>) this.LowValues)
    {
      if (double.IsNaN(lowValue) && this.ShowEmptyPoints)
      {
        this.ValidateDataPoints(this.LowValues);
        break;
      }
    }
    foreach (double openValue in (IEnumerable<double>) this.OpenValues)
    {
      if (double.IsNaN(openValue) && this.ShowEmptyPoints)
      {
        this.ValidateDataPoints(this.OpenValues);
        break;
      }
    }
    foreach (double closeValue in (IEnumerable<double>) this.CloseValues)
    {
      if (double.IsNaN(closeValue) && this.ShowEmptyPoints)
      {
        this.ValidateDataPoints(this.CloseValues);
        break;
      }
    }
  }

  internal IList<double> GetComparisionModeValues()
  {
    if (this.ActualXAxis is CategoryAxis && !(this.ActualXAxis as CategoryAxis).IsIndexed)
    {
      switch (this.ComparisonMode)
      {
        case FinancialPrice.High:
          return this.GroupedSeriesYValues[0];
        case FinancialPrice.Low:
          return this.GroupedSeriesYValues[1];
        case FinancialPrice.Open:
          return this.GroupedSeriesYValues[2];
        case FinancialPrice.Close:
          return this.GroupedSeriesYValues[3];
      }
    }
    else
    {
      switch (this.ComparisonMode)
      {
        case FinancialPrice.High:
          return this.HighValues;
        case FinancialPrice.Low:
          return this.LowValues;
        case FinancialPrice.Open:
          return this.OpenValues;
        case FinancialPrice.Close:
          return this.CloseValues;
      }
    }
    return (IList<double>) null;
  }

  internal void AddAdornments(
    double xVal,
    ChartPoint highPt,
    ChartPoint lowPt,
    ChartPoint startOpenPt,
    ChartPoint endOpenPt,
    ChartPoint startClosePt,
    ChartPoint endClosePt,
    int i,
    double median)
  {
    double y1;
    double y2;
    if (double.IsNaN(highPt.Y) || double.IsNaN(lowPt.Y) || highPt.Y > lowPt.Y)
    {
      y1 = highPt.Y;
      y2 = lowPt.Y;
    }
    else
    {
      y1 = lowPt.Y;
      y2 = highPt.Y;
    }
    bool flag = double.IsNaN(startOpenPt.Y) || double.IsNaN(startClosePt.Y);
    switch (this.adornmentInfo.GetAdornmentPosition())
    {
      case AdornmentsPosition.Top:
        double x1 = highPt.X;
        double y3;
        double xPos1;
        ActualLabelPosition actualLabelPosition1;
        ActualLabelPosition actualLabelPosition2;
        if (flag || startOpenPt.Y > startClosePt.Y)
        {
          y3 = startOpenPt.Y;
          switch (this)
          {
            case HiLoOpenCloseSeries _:
            case FastHiLoOpenCloseBitmapSeries _:
              xPos1 = startOpenPt.X;
              break;
            default:
              xPos1 = highPt.X - median;
              break;
          }
          if (this.IsActualTransposed)
          {
            actualLabelPosition1 = this.ActualYAxis.IsInversed ? ActualLabelPosition.Left : ActualLabelPosition.Right;
            actualLabelPosition2 = this.ActualXAxis.IsInversed ? ActualLabelPosition.Top : ActualLabelPosition.Bottom;
          }
          else
          {
            actualLabelPosition1 = this.ActualYAxis.IsInversed ? ActualLabelPosition.Bottom : ActualLabelPosition.Top;
            actualLabelPosition2 = this.ActualXAxis.IsInversed ? ActualLabelPosition.Right : ActualLabelPosition.Left;
          }
        }
        else
        {
          y3 = startClosePt.Y;
          switch (this)
          {
            case HiLoOpenCloseSeries _:
            case FastHiLoOpenCloseBitmapSeries _:
              xPos1 = startClosePt.X;
              break;
            default:
              xPos1 = highPt.X + median;
              break;
          }
          if (this.IsActualTransposed)
          {
            actualLabelPosition1 = this.ActualYAxis.IsInversed ? ActualLabelPosition.Left : ActualLabelPosition.Right;
            actualLabelPosition2 = this.ActualXAxis.IsInversed ? ActualLabelPosition.Bottom : ActualLabelPosition.Top;
          }
          else
          {
            actualLabelPosition1 = this.ActualYAxis.IsInversed ? ActualLabelPosition.Bottom : ActualLabelPosition.Top;
            actualLabelPosition2 = this.ActualXAxis.IsInversed ? ActualLabelPosition.Left : ActualLabelPosition.Right;
          }
        }
        if (this is CandleSeries || this is FastCandleBitmapSeries)
          actualLabelPosition2 = actualLabelPosition1;
        if (i < this.Adornments.Count / 2)
        {
          int index1 = 2 * i;
          this.Adornments[index1].ActualLabelPosition = actualLabelPosition1;
          ObservableCollection<ChartAdornment> adornments = this.Adornments;
          int index2 = index1;
          int index3 = index2 + 1;
          adornments[index2].SetData(xVal, y1, x1, y1);
          this.Adornments[index3].ActualLabelPosition = actualLabelPosition2;
          this.Adornments[index3].SetData(xVal, y3, xPos1, y3);
        }
        else
        {
          ChartAdornment adornment1 = this.CreateAdornment((AdornmentSeries) this, xVal, y1, x1, y1);
          adornment1.ActualLabelPosition = actualLabelPosition1;
          this.Adornments.Add(adornment1);
          ChartAdornment adornment2 = this.CreateAdornment((AdornmentSeries) this, xVal, y3, xPos1, y3);
          adornment2.ActualLabelPosition = actualLabelPosition2;
          this.Adornments.Add(adornment2);
        }
        int num1 = 2 * i;
        ObservableCollection<ChartAdornment> adornments1 = this.Adornments;
        int index4 = num1;
        int index5 = index4 + 1;
        adornments1[index4].Item = this.ActualData[i];
        this.Adornments[index5].Item = this.ActualData[i];
        break;
      case AdornmentsPosition.Bottom:
        double x2 = lowPt.X;
        double y4;
        double xPos2;
        ActualLabelPosition actualLabelPosition3;
        ActualLabelPosition actualLabelPosition4;
        if (flag || startClosePt.Y < startOpenPt.Y)
        {
          y4 = endClosePt.Y;
          switch (this)
          {
            case HiLoOpenCloseSeries _:
            case FastHiLoOpenCloseBitmapSeries _:
              xPos2 = startClosePt.X;
              break;
            default:
              xPos2 = lowPt.X + median;
              break;
          }
          if (this.IsActualTransposed)
          {
            actualLabelPosition3 = this.ActualYAxis.IsInversed ? ActualLabelPosition.Right : ActualLabelPosition.Left;
            actualLabelPosition4 = this.ActualXAxis.IsInversed ? ActualLabelPosition.Bottom : ActualLabelPosition.Top;
          }
          else
          {
            actualLabelPosition3 = this.ActualYAxis.IsInversed ? ActualLabelPosition.Top : ActualLabelPosition.Bottom;
            actualLabelPosition4 = this.ActualXAxis.IsInversed ? ActualLabelPosition.Left : ActualLabelPosition.Right;
          }
        }
        else
        {
          y4 = startOpenPt.Y;
          switch (this)
          {
            case HiLoOpenCloseSeries _:
            case FastHiLoOpenCloseBitmapSeries _:
              xPos2 = startOpenPt.X;
              break;
            default:
              xPos2 = lowPt.X - median;
              break;
          }
          if (this.IsActualTransposed)
          {
            actualLabelPosition3 = this.ActualYAxis.IsInversed ? ActualLabelPosition.Right : ActualLabelPosition.Left;
            actualLabelPosition4 = this.ActualXAxis.IsInversed ? ActualLabelPosition.Top : ActualLabelPosition.Bottom;
          }
          else
          {
            actualLabelPosition3 = this.ActualYAxis.IsInversed ? ActualLabelPosition.Top : ActualLabelPosition.Bottom;
            actualLabelPosition4 = this.ActualXAxis.IsInversed ? ActualLabelPosition.Right : ActualLabelPosition.Left;
          }
        }
        if (this is CandleSeries || this is FastCandleBitmapSeries)
          actualLabelPosition4 = actualLabelPosition3;
        if (i < this.Adornments.Count / 2)
        {
          int index6 = 2 * i;
          this.Adornments[index6].ActualLabelPosition = actualLabelPosition3;
          ObservableCollection<ChartAdornment> adornments2 = this.Adornments;
          int index7 = index6;
          int index8 = index7 + 1;
          adornments2[index7].SetData(xVal, y2, x2, y2);
          this.Adornments[index8].ActualLabelPosition = actualLabelPosition4;
          this.Adornments[index8].SetData(xVal, y4, xPos2, y4);
        }
        else
        {
          ChartAdornment adornment3 = this.CreateAdornment((AdornmentSeries) this, xVal, y2, x2, y2);
          adornment3.ActualLabelPosition = actualLabelPosition3;
          this.Adornments.Add(adornment3);
          ChartAdornment adornment4 = this.CreateAdornment((AdornmentSeries) this, xVal, y4, xPos2, y4);
          adornment4.ActualLabelPosition = actualLabelPosition4;
          this.Adornments.Add(adornment4);
        }
        int num2 = 2 * i;
        ObservableCollection<ChartAdornment> adornments3 = this.Adornments;
        int index9 = num2;
        int index10 = index9 + 1;
        adornments3[index9].Item = this.ActualData[i];
        this.Adornments[index10].Item = this.ActualData[i];
        break;
      default:
        double x3 = highPt.X;
        double xPos3;
        double xPos4;
        switch (this)
        {
          case HiLoOpenCloseSeries _:
          case FastHiLoOpenCloseBitmapSeries _:
            xPos3 = startClosePt.X;
            xPos4 = startOpenPt.X;
            break;
          default:
            xPos3 = highPt.X + median;
            xPos4 = lowPt.X - median;
            break;
        }
        double x4 = lowPt.X;
        double y5 = startOpenPt.Y;
        double y6 = endClosePt.Y;
        ActualLabelPosition actualLabelPosition5;
        ActualLabelPosition actualLabelPosition6;
        ActualLabelPosition actualLabelPosition7;
        ActualLabelPosition actualLabelPosition8;
        if (this.IsActualTransposed)
        {
          actualLabelPosition5 = this.ActualYAxis.IsInversed ? ActualLabelPosition.Left : ActualLabelPosition.Right;
          actualLabelPosition6 = this.ActualXAxis.IsInversed ? ActualLabelPosition.Top : ActualLabelPosition.Bottom;
          actualLabelPosition7 = this.ActualYAxis.IsInversed ? ActualLabelPosition.Right : ActualLabelPosition.Left;
          actualLabelPosition8 = this.ActualXAxis.IsInversed ? ActualLabelPosition.Bottom : ActualLabelPosition.Top;
        }
        else
        {
          actualLabelPosition5 = this.ActualYAxis.IsInversed ? ActualLabelPosition.Bottom : ActualLabelPosition.Top;
          actualLabelPosition6 = this.ActualXAxis.IsInversed ? ActualLabelPosition.Right : ActualLabelPosition.Left;
          actualLabelPosition7 = this.ActualYAxis.IsInversed ? ActualLabelPosition.Top : ActualLabelPosition.Bottom;
          actualLabelPosition8 = this.ActualXAxis.IsInversed ? ActualLabelPosition.Left : ActualLabelPosition.Right;
        }
        if (this is CandleSeries || this is FastCandleBitmapSeries)
        {
          actualLabelPosition6 = actualLabelPosition5;
          actualLabelPosition8 = actualLabelPosition7;
        }
        if (i < this.Adornments.Count / 4)
        {
          int index11 = 4 * i;
          this.Adornments[index11].ActualLabelPosition = actualLabelPosition5;
          this.Adornments[index11].GrandTotal = this.Adornments[index11].CalculateSumOfValues(this.HighValues);
          ObservableCollection<ChartAdornment> adornments4 = this.Adornments;
          int index12 = index11;
          int index13 = index12 + 1;
          adornments4[index12].SetData(xVal, y1, x3, y1);
          this.Adornments[index13].ActualLabelPosition = actualLabelPosition6;
          this.Adornments[index13].GrandTotal = this.Adornments[index13].CalculateSumOfValues(this.OpenValues);
          ObservableCollection<ChartAdornment> adornments5 = this.Adornments;
          int index14 = index13;
          int index15 = index14 + 1;
          adornments5[index14].SetData(xVal, y5, xPos4, y5);
          this.Adornments[index15].ActualLabelPosition = actualLabelPosition7;
          this.Adornments[index15].GrandTotal = this.Adornments[index15].CalculateSumOfValues(this.LowValues);
          ObservableCollection<ChartAdornment> adornments6 = this.Adornments;
          int index16 = index15;
          int index17 = index16 + 1;
          adornments6[index16].SetData(xVal, y2, x4, y2);
          this.Adornments[index17].ActualLabelPosition = actualLabelPosition8;
          this.Adornments[index17].GrandTotal = this.Adornments[index17].CalculateSumOfValues(this.CloseValues);
          this.Adornments[index17].SetData(xVal, y6, xPos3, y6);
        }
        else
        {
          ChartAdornment adornment5 = this.CreateAdornment((AdornmentSeries) this, xVal, y1, x3, y1);
          adornment5.ActualLabelPosition = actualLabelPosition5;
          this.Adornments.Add(adornment5);
          adornment5.CalculateSumOfValues(this.HighValues);
          ChartAdornment adornment6 = this.CreateAdornment((AdornmentSeries) this, xVal, y5, xPos4, y5);
          adornment6.ActualLabelPosition = actualLabelPosition6;
          this.Adornments.Add(adornment6);
          adornment6.CalculateSumOfValues(this.OpenValues);
          ChartAdornment adornment7 = this.CreateAdornment((AdornmentSeries) this, xVal, y2, x4, y2);
          adornment7.ActualLabelPosition = actualLabelPosition7;
          this.Adornments.Add(adornment7);
          adornment7.CalculateSumOfValues(this.LowValues);
          ChartAdornment adornment8 = this.CreateAdornment((AdornmentSeries) this, xVal, y6, xPos3, y6);
          adornment8.ActualLabelPosition = actualLabelPosition8;
          this.Adornments.Add(adornment8);
          adornment8.CalculateSumOfValues(this.CloseValues);
        }
        int num3 = 4 * i;
        ObservableCollection<ChartAdornment> adornments7 = this.Adornments;
        int index18 = num3;
        int num4 = index18 + 1;
        adornments7[index18].Item = this.ActualData[i];
        ObservableCollection<ChartAdornment> adornments8 = this.Adornments;
        int index19 = num4;
        int num5 = index19 + 1;
        adornments8[index19].Item = this.ActualData[i];
        ObservableCollection<ChartAdornment> adornments9 = this.Adornments;
        int index20 = num5;
        int index21 = index20 + 1;
        adornments9[index20].Item = this.ActualData[i];
        this.Adornments[index21].Item = this.ActualData[i];
        break;
    }
  }

  protected internal override void GeneratePoints()
  {
    this.GeneratePoints(new string[4]
    {
      this.High,
      this.Low,
      this.Open,
      this.Close
    }, this.HighValues, this.LowValues, this.OpenValues, this.CloseValues);
  }

  protected override void OnDataSourceChanged(IEnumerable oldValue, IEnumerable newValue)
  {
    this.OpenValues.Clear();
    this.HighValues.Clear();
    this.LowValues.Clear();
    this.CloseValues.Clear();
    this.Segment = (ChartSegment) null;
    this.GeneratePoints(new string[4]
    {
      this.High,
      this.Low,
      this.Open,
      this.Close
    }, this.HighValues, this.LowValues, this.OpenValues, this.CloseValues);
    this.isPointValidated = false;
    this.UpdateArea();
  }

  protected override void OnBindingPathChanged(DependencyPropertyChangedEventArgs args)
  {
    this.OpenValues.Clear();
    this.HighValues.Clear();
    this.LowValues.Clear();
    this.CloseValues.Clear();
    this.Segment = (ChartSegment) null;
    base.OnBindingPathChanged(args);
  }

  protected override DependencyObject CloneSeries(DependencyObject obj)
  {
    FinancialSeriesBase financialSeriesBase = obj as FinancialSeriesBase;
    financialSeriesBase.High = this.High;
    financialSeriesBase.Low = this.Low;
    financialSeriesBase.Open = this.Open;
    financialSeriesBase.Close = this.Close;
    financialSeriesBase.BearFillColor = this.BearFillColor;
    financialSeriesBase.BullFillColor = this.BullFillColor;
    return base.CloneSeries((DependencyObject) financialSeriesBase);
  }

  internal override DataTemplate GetDefaultTooltipTemplate()
  {
    if (this.financialTooltipTemplate == null)
      this.financialTooltipTemplate = ChartDictionaries.GenericCommonDictionary[(object) "FinancialTooltipTemplate"] as DataTemplate;
    return this.financialTooltipTemplate;
  }

  private static void OnYPathChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    (d as FinancialSeriesBase).OnBindingPathChanged(e);
  }

  private static void OnComparisonModeChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    FinancialSeriesBase financialSeriesBase = d as FinancialSeriesBase;
    if (financialSeriesBase.Segments == null)
      return;
    financialSeriesBase.Segments.Clear();
    financialSeriesBase.UpdateArea();
  }

  private static void OnBearFillColorPropertyChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    FinancialSeriesBase financialSeriesBase = d as FinancialSeriesBase;
    switch (financialSeriesBase)
    {
      case FastCandleBitmapSeries _:
      case FastHiLoOpenCloseBitmapSeries _:
        financialSeriesBase.UpdateArea();
        break;
      default:
        using (IEnumerator<ChartSegment> enumerator = financialSeriesBase.Segments.GetEnumerator())
        {
          while (enumerator.MoveNext())
          {
            ChartSegment current = enumerator.Current;
            if (current is CandleSegment)
              (current as CandleSegment).BearFillColor = financialSeriesBase.BearFillColor;
            else if (current is HiLoOpenCloseSegment)
              (current as HiLoOpenCloseSegment).BearFillColor = financialSeriesBase.BearFillColor;
          }
          break;
        }
    }
  }

  private static void OnBullFillColorPropertyChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    FinancialSeriesBase financialSeriesBase = d as FinancialSeriesBase;
    switch (financialSeriesBase)
    {
      case FastCandleBitmapSeries _:
      case FastHiLoOpenCloseBitmapSeries _:
        financialSeriesBase.UpdateArea();
        break;
      default:
        using (IEnumerator<ChartSegment> enumerator = financialSeriesBase.Segments.GetEnumerator())
        {
          while (enumerator.MoveNext())
          {
            ChartSegment current = enumerator.Current;
            if (current is CandleSegment)
              (current as CandleSegment).BullFillColor = financialSeriesBase.BullFillColor;
            else if (current is HiLoOpenCloseSegment)
              (current as HiLoOpenCloseSegment).BullFillColor = financialSeriesBase.BullFillColor;
          }
          break;
        }
    }
  }

  internal override List<object> GetDataPoints(
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
    int count = xValues.Count;
    if (count != this.HighValues.Count || count != this.LowValues.Count || count != this.OpenValues.Count || count != this.CloseValues.Count)
      return (List<object>) null;
    for (int index = minimum; index <= maximum; ++index)
    {
      double xValue = xValues[index];
      if (validateYValues || startX <= xValue && xValue <= endX)
      {
        if (startY <= this.HighValues[index] && this.HighValues[index] <= endY)
          dataPoints.Add(this.ActualData[index]);
        else if (startY <= this.LowValues[index] && this.LowValues[index] <= endY)
          dataPoints.Add(this.ActualData[index]);
        else if (startY <= this.OpenValues[index] && this.OpenValues[index] <= endY)
          dataPoints.Add(this.ActualData[index]);
        else if (startY <= this.CloseValues[index] && this.CloseValues[index] <= endY)
          dataPoints.Add(this.ActualData[index]);
      }
    }
    return dataPoints;
  }
}
