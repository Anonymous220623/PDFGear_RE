// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.RangeSeriesBase
// Assembly: Syncfusion.SfChart.WPF, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 279D70C6-7936-4F3E-B6CF-79EFEF734826
// Assembly location: C:\Program Files\PDFgear\Syncfusion.SfChart.WPF.dll

using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

#nullable disable
namespace Syncfusion.UI.Xaml.Charts;

public abstract class RangeSeriesBase : CartesianSeries
{
  public static readonly DependencyProperty HighProperty = DependencyProperty.Register(nameof (High), typeof (string), typeof (RangeSeriesBase), new PropertyMetadata((object) null, new PropertyChangedCallback(RangeSeriesBase.OnYPathChanged)));
  public static readonly DependencyProperty LowProperty = DependencyProperty.Register(nameof (Low), typeof (string), typeof (RangeSeriesBase), new PropertyMetadata((object) null, new PropertyChangedCallback(RangeSeriesBase.OnYPathChanged)));
  private DataTemplate rangeTooltipTemplate;

  public RangeSeriesBase()
  {
    this.HighValues = (IList<double>) new List<double>();
    this.LowValues = (IList<double>) new List<double>();
  }

  public string High
  {
    get => (string) this.GetValue(RangeSeriesBase.HighProperty);
    set => this.SetValue(RangeSeriesBase.HighProperty, (object) value);
  }

  public string Low
  {
    get => (string) this.GetValue(RangeSeriesBase.LowProperty);
    set => this.SetValue(RangeSeriesBase.LowProperty, (object) value);
  }

  protected internal IList<double> HighValues { get; set; }

  protected internal IList<double> LowValues { get; set; }

  protected ChartSegment Segment { get; set; }

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
    }
    this.dataPoint.Index = index;
    this.dataPoint.Series = (ChartSeriesBase) this;
    if (this.ActualData.Count > index)
      this.dataPoint.Item = this.ActualData[index];
    return this.dataPoint;
  }

  internal override void ValidateYValues()
  {
    using (IEnumerator<double> enumerator = this.HighValues.GetEnumerator())
    {
      if (enumerator.MoveNext())
      {
        if (double.IsNaN(enumerator.Current))
        {
          if (this.ShowEmptyPoints)
            this.ValidateDataPoints(this.HighValues);
        }
      }
    }
    using (IEnumerator<double> enumerator = this.LowValues.GetEnumerator())
    {
      if (!enumerator.MoveNext() || !double.IsNaN(enumerator.Current) || !this.ShowEmptyPoints)
        return;
      this.ValidateDataPoints(this.LowValues);
    }
  }

  internal override void ReValidateYValues(List<int>[] emptyPointIndexs)
  {
    foreach (int index in emptyPointIndexs[0])
      this.HighValues[index] = double.NaN;
    foreach (int index in emptyPointIndexs[1])
      this.LowValues[index] = double.NaN;
  }

  internal void AddAdornments(double xVal, double xOffset, double high, double low, int index)
  {
    bool flag = this is RangeColumnSeries && !this.IsMultipleYPathRequired;
    AdornmentsPosition adornmentPosition = this.adornmentInfo.GetAdornmentPosition();
    if (adornmentPosition == AdornmentsPosition.Top && !flag)
    {
      double xPos = xVal + xOffset;
      double num = high <= low ? low : high;
      ActualLabelPosition actualLabelPosition = this.IsActualTransposed ? (this.ActualYAxis.IsInversed ? ActualLabelPosition.Left : ActualLabelPosition.Right) : (this.ActualYAxis.IsInversed ? ActualLabelPosition.Bottom : ActualLabelPosition.Top);
      if (index < this.Adornments.Count)
      {
        this.Adornments[index].ActualLabelPosition = actualLabelPosition;
        this.Adornments[index].SetData(xVal, num, xPos, num);
      }
      else
      {
        ChartAdornment adornment = this.CreateAdornment((AdornmentSeries) this, xVal, num, xPos, num);
        adornment.ActualLabelPosition = actualLabelPosition;
        this.Adornments.Add(adornment);
      }
      this.Adornments[index].Item = this.ActualData[index];
    }
    else if (adornmentPosition == AdornmentsPosition.Bottom && !flag)
    {
      double xPos = xVal + xOffset;
      double num = high <= low ? high : low;
      ActualLabelPosition actualLabelPosition = this.IsActualTransposed ? (this.ActualYAxis.IsInversed ? ActualLabelPosition.Right : ActualLabelPosition.Left) : (this.ActualYAxis.IsInversed ? ActualLabelPosition.Top : ActualLabelPosition.Bottom);
      if (index < this.Adornments.Count)
      {
        this.Adornments[index].ActualLabelPosition = actualLabelPosition;
        this.Adornments[index].SetData(xVal, num, xPos, num);
      }
      else
      {
        ChartAdornment adornment = this.CreateAdornment((AdornmentSeries) this, xVal, num, xPos, num);
        adornment.ActualLabelPosition = actualLabelPosition;
        this.Adornments.Add(adornment);
      }
      this.Adornments[index].Item = this.ActualData[index];
    }
    else
    {
      double xPos = xVal + xOffset;
      ActualLabelPosition actualLabelPosition1;
      ActualLabelPosition actualLabelPosition2;
      if (this.IsActualTransposed)
      {
        if (this.ActualYAxis.IsInversed)
        {
          actualLabelPosition1 = ActualLabelPosition.Left;
          actualLabelPosition2 = ActualLabelPosition.Right;
        }
        else
        {
          actualLabelPosition1 = ActualLabelPosition.Right;
          actualLabelPosition2 = ActualLabelPosition.Left;
        }
      }
      else if (this.ActualYAxis.IsInversed)
      {
        actualLabelPosition1 = ActualLabelPosition.Bottom;
        actualLabelPosition2 = ActualLabelPosition.Top;
      }
      else
      {
        actualLabelPosition1 = ActualLabelPosition.Top;
        actualLabelPosition2 = ActualLabelPosition.Bottom;
      }
      double num1;
      double num2;
      if (high > low)
      {
        num1 = high;
        num2 = low;
      }
      else
      {
        num1 = low;
        num2 = high;
      }
      int num3 = flag ? this.Adornments.Count : this.Adornments.Count / 2;
      if (index < num3)
      {
        if (flag)
        {
          this.Adornments[index].ActualLabelPosition = actualLabelPosition1;
          this.Adornments[index].SetData(xVal, num1, xPos, num1);
        }
        else
        {
          int index1 = 2 * index;
          this.Adornments[index1].ActualLabelPosition = actualLabelPosition1;
          this.Adornments[index1].GrandTotal = this.Adornments[index1].CalculateSumOfValues(this.HighValues);
          ObservableCollection<ChartAdornment> adornments = this.Adornments;
          int index2 = index1;
          int index3 = index2 + 1;
          adornments[index2].SetData(xVal, num1, xPos, num1);
          this.Adornments[index3].ActualLabelPosition = actualLabelPosition2;
          this.Adornments[index3].GrandTotal = this.Adornments[index3].CalculateSumOfValues(this.LowValues);
          this.Adornments[index3].SetData(xVal, num2, xPos, num2);
        }
      }
      else
      {
        ChartAdornment adornment1 = this.CreateAdornment((AdornmentSeries) this, xVal, num1, xPos, num1);
        adornment1.ActualLabelPosition = actualLabelPosition1;
        this.Adornments.Add(adornment1);
        adornment1.CalculateSumOfValues(this.HighValues);
        if (flag)
        {
          this.Adornments[index].Item = this.ActualData[index];
          return;
        }
        ChartAdornment adornment2 = this.CreateAdornment((AdornmentSeries) this, xVal, num2, xPos, num2);
        adornment2.ActualLabelPosition = actualLabelPosition2;
        this.Adornments.Add(adornment2);
        adornment2.CalculateSumOfValues(this.LowValues);
      }
      if (!flag)
      {
        int num4 = 2 * index;
        ObservableCollection<ChartAdornment> adornments = this.Adornments;
        int index4 = num4;
        int index5 = index4 + 1;
        adornments[index4].Item = this.ActualData[index];
        this.Adornments[index5].Item = this.ActualData[index];
      }
      else
        this.Adornments[index].Item = this.ActualData[index];
    }
  }

  protected internal override void GeneratePoints()
  {
    this.GeneratePoints(new string[2]{ this.High, this.Low }, this.HighValues, this.LowValues);
  }

  protected override void OnDataSourceChanged(IEnumerable oldValue, IEnumerable newValue)
  {
    base.OnDataSourceChanged(oldValue, newValue);
    this.HighValues.Clear();
    this.LowValues.Clear();
    this.Segment = (ChartSegment) null;
    this.GeneratePoints(new string[2]{ this.High, this.Low }, this.HighValues, this.LowValues);
    this.isPointValidated = false;
    this.UpdateArea();
  }

  protected override void OnBindingPathChanged(DependencyPropertyChangedEventArgs args)
  {
    this.HighValues.Clear();
    this.LowValues.Clear();
    this.Segment = (ChartSegment) null;
    base.OnBindingPathChanged(args);
  }

  protected override DependencyObject CloneSeries(DependencyObject obj)
  {
    if (obj is RangeSeriesBase rangeSeriesBase)
    {
      rangeSeriesBase.High = this.High;
      rangeSeriesBase.Low = this.Low;
    }
    return base.CloneSeries(obj);
  }

  internal override DataTemplate GetDefaultTooltipTemplate()
  {
    if (this.rangeTooltipTemplate == null)
      this.rangeTooltipTemplate = !(this is RangeColumnSeries) || this.IsMultipleYPathRequired ? ChartDictionaries.GenericCommonDictionary[(object) "RangeTooltipTemplate"] as DataTemplate : ChartDictionaries.GenericCommonDictionary[(object) "DefaultTooltipTemplate"] as DataTemplate;
    return this.rangeTooltipTemplate;
  }

  private static void OnYPathChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    (d as RangeSeriesBase).OnBindingPathChanged(e);
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
    if (count != this.HighValues.Count || count != this.LowValues.Count)
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
      }
    }
    return dataPoints;
  }
}
