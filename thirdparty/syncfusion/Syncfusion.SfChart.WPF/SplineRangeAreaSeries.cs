// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.SplineRangeAreaSeries
// Assembly: Syncfusion.SfChart.WPF, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 279D70C6-7936-4F3E-B6CF-79EFEF734826
// Assembly location: C:\Program Files\PDFgear\Syncfusion.SfChart.WPF.dll

using System;
using System.Collections.Generic;
using System.Windows;

#nullable disable
namespace Syncfusion.UI.Xaml.Charts;

public class SplineRangeAreaSeries : RangeAreaSeries
{
  public static readonly DependencyProperty SplineTypeProperty = DependencyProperty.Register(nameof (SplineType), typeof (SplineType), typeof (SplineRangeAreaSeries), new PropertyMetadata((object) SplineType.Natural, new PropertyChangedCallback(SplineRangeAreaSeries.OnSplineTypeChanged)));
  private List<ChartPoint> startControlPoints;
  private List<ChartPoint> endControlPoints;

  public SplineType SplineType
  {
    get => (SplineType) this.GetValue(SplineRangeAreaSeries.SplineTypeProperty);
    set => this.SetValue(SplineRangeAreaSeries.SplineTypeProperty, (object) value);
  }

  public override void CreateSegments()
  {
    double[] ys2_1 = (double[]) null;
    double[] ys2_2 = (double[]) null;
    List<ChartPoint> AreaPoints = new List<ChartPoint>();
    List<ChartPoint> chartPointList1 = new List<ChartPoint>();
    List<ChartPoint> chartPointList2 = new List<ChartPoint>();
    List<ChartPoint> chartPointList3 = new List<ChartPoint>();
    List<ChartPoint> chartPointList4 = new List<ChartPoint>();
    List<double> xValues = !(this.ActualXAxis is CategoryAxis) || (this.ActualXAxis as CategoryAxis).IsIndexed ? this.GetXValues() : this.GroupedXValuesIndexes;
    if (xValues == null)
      return;
    if (this.ActualXAxis is CategoryAxis && !(this.ActualXAxis as CategoryAxis).IsIndexed)
    {
      int count = xValues.Count;
      this.Segments.Clear();
      this.Adornments.Clear();
      if (this.SplineType == SplineType.Monotonic)
      {
        this.GetMonotonicSpline(xValues, this.GroupedSeriesYValues[0]);
        chartPointList1 = this.startControlPoints;
        chartPointList2 = this.endControlPoints;
        this.GetMonotonicSpline(xValues, this.GroupedSeriesYValues[1]);
        chartPointList3 = this.startControlPoints;
        chartPointList4 = this.endControlPoints;
      }
      else if (this.SplineType == SplineType.Cardinal)
      {
        this.GetCardinalSpline(xValues, this.GroupedSeriesYValues[0]);
        chartPointList1 = this.startControlPoints;
        chartPointList2 = this.endControlPoints;
        this.GetCardinalSpline(xValues, this.GroupedSeriesYValues[1]);
        chartPointList3 = this.startControlPoints;
        chartPointList4 = this.endControlPoints;
      }
      else
      {
        this.NaturalSpline(xValues, this.GroupedSeriesYValues[0], out ys2_1);
        this.NaturalSpline(xValues, this.GroupedSeriesYValues[1], out ys2_2);
      }
      for (int index = 0; index < count; ++index)
      {
        if (!double.IsNaN(this.GroupedSeriesYValues[1][index]) && !double.IsNaN(this.GroupedSeriesYValues[0][index]))
        {
          if (index == 0 || index < this.DataCount - 1 && (double.IsNaN(this.GroupedSeriesYValues[1][index - 1]) || double.IsNaN(this.GroupedSeriesYValues[0][index - 1])))
          {
            ChartPoint chartPoint1 = new ChartPoint(xValues[index], this.GroupedSeriesYValues[1][index]);
            AreaPoints.Add(chartPoint1);
            ChartPoint chartPoint2 = new ChartPoint(xValues[index], this.GroupedSeriesYValues[0][index]);
            AreaPoints.Add(chartPoint2);
          }
          else
          {
            ChartPoint point1_1 = new ChartPoint(xValues[index - 1], this.GroupedSeriesYValues[0][index - 1]);
            ChartPoint point2_1 = new ChartPoint(xValues[index], this.GroupedSeriesYValues[0][index]);
            ChartPoint point1_2 = new ChartPoint(xValues[index - 1], this.GroupedSeriesYValues[1][index - 1]);
            ChartPoint point2_2 = new ChartPoint(xValues[index], this.GroupedSeriesYValues[1][index]);
            if (this.SplineType == SplineType.Monotonic)
            {
              AreaPoints.AddRange((IEnumerable<ChartPoint>) new ChartPoint[3]
              {
                chartPointList1[index - 1],
                chartPointList2[index - 1],
                point2_1
              });
              AreaPoints.AddRange((IEnumerable<ChartPoint>) new ChartPoint[3]
              {
                point1_2,
                chartPointList3[index - 1],
                chartPointList4[index - 1]
              });
            }
            else if (this.SplineType == SplineType.Cardinal)
            {
              AreaPoints.AddRange((IEnumerable<ChartPoint>) new ChartPoint[3]
              {
                chartPointList1[index - 1],
                chartPointList2[index - 1],
                point2_1
              });
              AreaPoints.AddRange((IEnumerable<ChartPoint>) new ChartPoint[3]
              {
                point1_2,
                chartPointList3[index - 1],
                chartPointList4[index - 1]
              });
            }
            else
            {
              ChartPoint controlPoint1;
              ChartPoint controlPoint2;
              this.GetBezierControlPoints(point1_1, point2_1, ys2_1[index - 1], ys2_1[index], out controlPoint1, out controlPoint2);
              AreaPoints.AddRange((IEnumerable<ChartPoint>) new ChartPoint[3]
              {
                controlPoint1,
                controlPoint2,
                point2_1
              });
              this.GetBezierControlPoints(point1_2, point2_2, ys2_2[index - 1], ys2_2[index], out controlPoint1, out controlPoint2);
              AreaPoints.AddRange((IEnumerable<ChartPoint>) new ChartPoint[3]
              {
                point1_2,
                controlPoint1,
                controlPoint2
              });
            }
          }
        }
        else
        {
          if (AreaPoints.Count > 0)
          {
            ChartPoint chartPoint = new ChartPoint(xValues[index - 1], this.GroupedSeriesYValues[1][index - 1]);
            AreaPoints.Add(chartPoint);
            this.CreateSegment(AreaPoints);
          }
          AreaPoints = new List<ChartPoint>();
        }
      }
      if (AreaPoints.Count > 0)
      {
        ChartPoint chartPoint = new ChartPoint(xValues[count - 1], this.GroupedSeriesYValues[1][count - 1]);
        AreaPoints.Add(chartPoint);
        this.CreateSegment(AreaPoints);
      }
      for (int index = 0; index < xValues.Count; ++index)
      {
        if (this.AdornmentsInfo != null)
          this.AddAdornments(xValues[index], 0.0, this.GroupedSeriesYValues[0][index], this.GroupedSeriesYValues[1][index], index);
      }
    }
    else
    {
      this.Segments.Clear();
      if (this.AdornmentsInfo != null)
      {
        if (this.adornmentInfo.GetAdornmentPosition() == AdornmentsPosition.TopAndBottom)
          this.ClearUnUsedAdornments(this.DataCount * 2);
        else
          this.ClearUnUsedAdornments(this.DataCount);
      }
      if (this.SplineType == SplineType.Monotonic)
      {
        this.GetMonotonicSpline(xValues, this.HighValues);
        chartPointList1 = this.startControlPoints;
        chartPointList2 = this.endControlPoints;
        this.GetMonotonicSpline(xValues, this.LowValues);
        chartPointList3 = this.startControlPoints;
        chartPointList4 = this.endControlPoints;
      }
      else if (this.SplineType == SplineType.Cardinal)
      {
        this.GetCardinalSpline(xValues, this.HighValues);
        chartPointList1 = this.startControlPoints;
        chartPointList2 = this.endControlPoints;
        this.GetCardinalSpline(xValues, this.LowValues);
        chartPointList3 = this.startControlPoints;
        chartPointList4 = this.endControlPoints;
      }
      else
      {
        this.NaturalSpline(xValues, this.HighValues, out ys2_1);
        this.NaturalSpline(xValues, this.LowValues, out ys2_2);
      }
      for (int index = 0; index < this.DataCount; ++index)
      {
        if (!double.IsNaN(this.LowValues[index]) && !double.IsNaN(this.HighValues[index]))
        {
          if (index == 0 || index < this.DataCount - 1 && (double.IsNaN(this.LowValues[index - 1]) || double.IsNaN(this.HighValues[index - 1])))
          {
            ChartPoint chartPoint3 = new ChartPoint(xValues[index], this.LowValues[index]);
            AreaPoints.Add(chartPoint3);
            ChartPoint chartPoint4 = new ChartPoint(xValues[index], this.HighValues[index]);
            AreaPoints.Add(chartPoint4);
          }
          else
          {
            ChartPoint point1_3 = new ChartPoint(xValues[index - 1], this.HighValues[index - 1]);
            ChartPoint point2_3 = new ChartPoint(xValues[index], this.HighValues[index]);
            ChartPoint point1_4 = new ChartPoint(xValues[index - 1], this.LowValues[index - 1]);
            ChartPoint point2_4 = new ChartPoint(xValues[index], this.LowValues[index]);
            if (this.SplineType == SplineType.Monotonic)
            {
              AreaPoints.AddRange((IEnumerable<ChartPoint>) new ChartPoint[3]
              {
                chartPointList1[index - 1],
                chartPointList2[index - 1],
                point2_3
              });
              AreaPoints.AddRange((IEnumerable<ChartPoint>) new ChartPoint[3]
              {
                point1_4,
                chartPointList3[index - 1],
                chartPointList4[index - 1]
              });
            }
            else if (this.SplineType == SplineType.Cardinal)
            {
              AreaPoints.AddRange((IEnumerable<ChartPoint>) new ChartPoint[3]
              {
                chartPointList1[index - 1],
                chartPointList2[index - 1],
                point2_3
              });
              AreaPoints.AddRange((IEnumerable<ChartPoint>) new ChartPoint[3]
              {
                point1_4,
                chartPointList3[index - 1],
                chartPointList4[index - 1]
              });
            }
            else
            {
              ChartPoint controlPoint1;
              ChartPoint controlPoint2;
              this.GetBezierControlPoints(point1_3, point2_3, ys2_1[index - 1], ys2_1[index], out controlPoint1, out controlPoint2);
              AreaPoints.AddRange((IEnumerable<ChartPoint>) new ChartPoint[3]
              {
                controlPoint1,
                controlPoint2,
                point2_3
              });
              this.GetBezierControlPoints(point1_4, point2_4, ys2_2[index - 1], ys2_2[index], out controlPoint1, out controlPoint2);
              AreaPoints.AddRange((IEnumerable<ChartPoint>) new ChartPoint[3]
              {
                point1_4,
                controlPoint1,
                controlPoint2
              });
            }
          }
        }
        else
        {
          if (AreaPoints.Count > 0)
          {
            ChartPoint chartPoint = new ChartPoint(xValues[index - 1], this.LowValues[index - 1]);
            AreaPoints.Add(chartPoint);
            this.CreateSegment(AreaPoints);
          }
          AreaPoints = new List<ChartPoint>();
        }
      }
      if (AreaPoints.Count > 0)
      {
        ChartPoint chartPoint = new ChartPoint(xValues[this.DataCount - 1], this.LowValues[this.DataCount - 1]);
        AreaPoints.Add(chartPoint);
        this.CreateSegment(AreaPoints);
      }
      for (int index = 0; index < xValues.Count; ++index)
      {
        if (this.AdornmentsInfo != null)
          this.AddAdornments(xValues[index], 0.0, this.HighValues[index], this.LowValues[index], index);
      }
    }
  }

  protected override ChartSegment CreateSegment() => (ChartSegment) new SplineRangeAreaSegment();

  private void CreateSegment(List<ChartPoint> AreaPoints)
  {
    if (!(this.CreateSegment() is SplineRangeAreaSegment segment))
      return;
    segment.Series = (ChartSeriesBase) this;
    segment.SetData(AreaPoints);
    this.Segments.Add((ChartSegment) segment);
  }

  internal void GetCardinalSpline(List<double> xValues, IList<double> yValues)
  {
    this.startControlPoints = new List<ChartPoint>(this.DataCount);
    this.endControlPoints = new List<ChartPoint>(this.DataCount);
    int length = !(this.ActualXAxis is CategoryAxis) || (this.ActualXAxis as CategoryAxis).IsIndexed ? this.DataCount : xValues.Count;
    double[] numArray1 = new double[length];
    double[] numArray2 = new double[length];
    for (int index = 0; index < length; ++index)
    {
      if (index == 0 && xValues.Count > 2)
        numArray1[index] = 0.5 * (xValues[index + 2] - xValues[index]);
      else if (index == length - 1 && length - 3 >= 0)
        numArray1[index] = 0.5 * (xValues[length - 1] - xValues[length - 3]);
      else if (index - 1 >= 0 && xValues.Count > index + 1)
        numArray1[index] = 0.5 * (xValues[index + 1] - xValues[index - 1]);
      if (double.IsNaN(numArray1[index]))
        numArray1[index] = 0.0;
      if (this.ActualXAxis is DateTimeAxis)
      {
        DateTime dateTime = xValues[index].FromOADate();
        if ((this.ActualXAxis as DateTimeAxis).IntervalType == DateTimeIntervalType.Auto || (this.ActualXAxis as DateTimeAxis).IntervalType == DateTimeIntervalType.Years)
        {
          int num = DateTime.IsLeapYear(dateTime.Year) ? 366 : 365;
          numArray2[index] = numArray1[index] / (double) num;
        }
        else if ((this.ActualXAxis as DateTimeAxis).IntervalType == DateTimeIntervalType.Months)
        {
          double num = (double) DateTime.DaysInMonth(dateTime.Year, dateTime.Month);
          numArray2[index] = numArray1[index] / num;
        }
      }
      else if (this.ActualXAxis is LogarithmicAxis)
      {
        numArray1[index] = Math.Log(numArray1[index], (this.ActualXAxis as LogarithmicAxis).LogarithmicBase);
        numArray2[index] = numArray1[index];
      }
      else
        numArray2[index] = numArray1[index];
    }
    for (int index = 0; index < numArray1.Length - 1; ++index)
    {
      this.startControlPoints.Add(new ChartPoint(xValues[index] + numArray1[index] / 3.0, yValues[index] + numArray2[index] / 3.0));
      this.endControlPoints.Add(new ChartPoint(xValues[index + 1] - numArray1[index + 1] / 3.0, yValues[index + 1] - numArray2[index + 1] / 3.0));
    }
  }

  internal void GetMonotonicSpline(List<double> xValues, IList<double> yValues)
  {
    this.startControlPoints = new List<ChartPoint>(this.DataCount);
    this.endControlPoints = new List<ChartPoint>(this.DataCount);
    int num1 = !(this.ActualXAxis is CategoryAxis) || (this.ActualXAxis as CategoryAxis).IsIndexed ? this.DataCount : xValues.Count;
    double[] numArray1 = new double[num1 - 1];
    double[] numArray2 = new double[num1 - 1];
    List<double> doubleList = new List<double>();
    for (int index = 0; index < num1 - 1; ++index)
    {
      if (!double.IsNaN(yValues[index + 1]) && !double.IsNaN(yValues[index]) && !double.IsNaN(xValues[index + 1]) && !double.IsNaN(xValues[index]))
      {
        numArray1[index] = xValues[index + 1] - xValues[index];
        numArray2[index] = (yValues[index + 1] - yValues[index]) / numArray1[index];
        if (double.IsInfinity(numArray2[index]))
          numArray2[index] = 0.0;
      }
    }
    if (numArray2.Length == 0)
      return;
    doubleList.Add(double.IsNaN(numArray2[0]) ? 0.0 : numArray2[0]);
    for (int index = 0; index < numArray1.Length - 1; ++index)
    {
      if (numArray2.Length > index + 1)
      {
        double num2 = numArray2[index];
        double num3 = numArray2[index + 1];
        if (num2 * num3 <= 0.0)
          doubleList.Add(0.0);
        else if (numArray1[index] == 0.0)
        {
          doubleList.Add(0.0);
        }
        else
        {
          double num4 = numArray1[index];
          double num5 = numArray1[index + 1];
          double num6 = num4 + num5;
          doubleList.Add(3.0 * num6 / ((num6 + num5) / num2 + (num6 + num4) / num3));
        }
      }
    }
    doubleList.Add(double.IsNaN(numArray2[numArray2.Length - 1]) ? 0.0 : numArray2[numArray2.Length - 1]);
    for (int index = 0; index < doubleList.Count; ++index)
    {
      if (index + 1 < doubleList.Count && numArray1.Length > 0)
      {
        double num7 = numArray1[index] / 3.0;
        this.startControlPoints.Add(new ChartPoint(xValues[index] + num7, yValues[index] + doubleList[index] * num7));
        this.endControlPoints.Add(new ChartPoint(xValues[index + 1] - num7, yValues[index + 1] - doubleList[index + 1] * num7));
      }
    }
  }

  internal void NaturalSpline(List<double> xValues, IList<double> yValues, out double[] ys2)
  {
    int length = !(this.ActualXAxis is CategoryAxis) || (this.ActualXAxis as CategoryAxis).IsIndexed ? this.DataCount : xValues.Count;
    ys2 = new double[length];
    if (length == 1)
      return;
    double num1 = 6.0;
    double[] numArray = new double[length - 1];
    if (this.SplineType == SplineType.Natural)
    {
      ys2[0] = numArray[0] = 0.0;
      ys2[length - 1] = 0.0;
    }
    else if (xValues.Count > 1)
    {
      double num2 = (xValues[1] - xValues[0]) / (yValues[1] - yValues[0]);
      double num3 = (xValues[length - 1] - xValues[length - 2]) / (yValues[length - 1] - yValues[length - 2]);
      numArray[0] = 0.5;
      ys2[0] = 3.0 * (yValues[1] - yValues[0]) / (xValues[1] - xValues[0]) - 3.0 * num2;
      ys2[length - 1] = 3.0 * num3 - 3.0 * (yValues[length - 1] - yValues[length - 2]) / (xValues[length - 1] - xValues[length - 2]);
      if (double.IsInfinity(ys2[0]) || double.IsNaN(ys2[0]))
        ys2[0] = 0.0;
      if (double.IsInfinity(ys2[length - 1]) || double.IsNaN(ys2[length - 1]))
        ys2[length - 1] = 0.0;
    }
    for (int index = 1; index < length - 1; ++index)
    {
      if (yValues.Count > index + 1 && !double.IsNaN(yValues[index + 1]) && !double.IsNaN(yValues[index - 1]) && !double.IsNaN(yValues[index]))
      {
        double num4 = xValues[index] - xValues[index - 1];
        double num5 = xValues[index + 1] - xValues[index - 1];
        double num6 = xValues[index + 1] - xValues[index];
        double num7 = yValues[index + 1] - yValues[index];
        double num8 = yValues[index] - yValues[index - 1];
        if (xValues[index] == xValues[index - 1] || xValues[index] == xValues[index + 1])
        {
          ys2[index] = 0.0;
          numArray[index] = 0.0;
        }
        else
        {
          double num9 = 1.0 / (num4 * ys2[index - 1] + 2.0 * num5);
          ys2[index] = -num9 * num6;
          numArray[index] = num9 * (num1 * (num7 / num6 - num8 / num4) - num4 * numArray[index - 1]);
        }
      }
    }
    for (int index = length - 2; index >= 0; --index)
      ys2[index] = ys2[index] * ys2[index + 1] + numArray[index];
  }

  internal void GetBezierControlPoints(
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

  private static void OnSplineTypeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    ((ChartSeriesBase) d).UpdateArea();
  }
}
