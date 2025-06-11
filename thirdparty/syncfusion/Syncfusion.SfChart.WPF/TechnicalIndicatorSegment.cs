// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.TechnicalIndicatorSegment
// Assembly: Syncfusion.SfChart.WPF, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 279D70C6-7936-4F3E-B6CF-79EFEF734826
// Assembly location: C:\Program Files\PDFgear\Syncfusion.SfChart.WPF.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;

#nullable disable
namespace Syncfusion.UI.Xaml.Charts;

public class TechnicalIndicatorSegment : FastLineSegment
{
  private Size availableSize;
  internal int Length;

  public TechnicalIndicatorSegment()
  {
  }

  public TechnicalIndicatorSegment(
    List<double> xVals,
    List<double> yVals,
    Brush stroke,
    ChartSeriesBase series)
  {
    this.Series = series;
    if (!(this.Series is FinancialTechnicalIndicator))
      return;
    this.customTemplate = (this.Series as FinancialTechnicalIndicator).CustomTemplate;
  }

  internal void SetValues(
    List<double> xVals,
    List<double> yVals,
    Brush stroke,
    ChartSeriesBase series)
  {
    this.xChartVals = (IList<double>) xVals;
    this.yChartVals = (IList<double>) yVals;
    if (series.DataCount > 1)
    {
      if (series.IsIndexed)
      {
        double end1 = (double) (series.DataCount - 1);
        double end2 = this.yChartVals.Max();
        double start1 = 0.0;
        double start2 = this.yChartVals.Min();
        this.XRange = new DoubleRange(start1, end1);
        this.YRange = new DoubleRange(start2, end2);
      }
      else
      {
        double end3 = this.xChartVals.Max();
        double end4 = this.yChartVals.Max();
        double start3 = this.xChartVals.Min();
        double start4 = this.yChartVals.Min();
        this.XRange = new DoubleRange(start3, end3);
        this.YRange = new DoubleRange(start4, end4);
      }
    }
    (this.Series as ChartSeries).Stroke = stroke;
    this.SetData((IList<double>) xVals, (IList<double>) yVals, this.Stroke);
    this.SetRange();
  }

  public TechnicalIndicatorSegment(
    List<double> xVals,
    List<double> yVals,
    Brush stroke,
    ChartSeriesBase series,
    int length)
  {
    this.Series = series;
    if (!(this.Series is FinancialTechnicalIndicator))
      return;
    this.customTemplate = (this.Series as FinancialTechnicalIndicator).CustomTemplate;
  }

  internal void SetValues(
    List<double> xVals,
    List<double> yVals,
    Brush stroke,
    ChartSeriesBase series,
    int length)
  {
    this.Length = length;
    this.xChartVals = (IList<double>) xVals;
    this.yChartVals = (IList<double>) yVals;
    if (series.DataCount > 1)
    {
      if (series.IsIndexed)
      {
        double end1 = (double) (series.DataCount - 1);
        double end2 = this.yChartVals.Max();
        double start1 = (double) length;
        double start2 = this.yChartVals.Min();
        this.XRange = new DoubleRange(start1, end1);
        this.YRange = new DoubleRange(start2, end2);
      }
      else
      {
        double end3 = this.xChartVals.Max();
        double end4 = this.yChartVals.Max();
        double start3 = this.xChartVals.Min();
        double start4 = this.yChartVals.Min();
        this.XRange = new DoubleRange(start3, end3);
        this.YRange = new DoubleRange(start4, end4);
      }
    }
    (this.Series as ChartSeries).Stroke = stroke;
    this.SetData((IList<double>) xVals, (IList<double>) yVals, this.Stroke);
  }

  public override void SetData(IList<double> xVals, IList<double> yVals, Brush strokeBrush)
  {
    this.xChartVals = xVals;
    this.yChartVals = yVals;
    if (strokeBrush == null || this.customTemplate != null)
      return;
    this.polyline.Stroke = strokeBrush;
  }

  public override void SetData(
    IList<double> xVals,
    IList<double> yVals,
    Brush strokeBrush,
    int length)
  {
    this.xChartVals = xVals;
    this.yChartVals = yVals;
    this.Length = length;
    if (strokeBrush == null || this.customTemplate != null)
      return;
    this.polyline.Stroke = strokeBrush;
  }

  public override UIElement CreateVisual(Size size)
  {
    UIElement visual = base.CreateVisual(size);
    if (this.customTemplate == null)
    {
      this.polyline.Stroke = this.Stroke;
      DoubleCollection strokeDashArray = (this.Series as FinancialTechnicalIndicator).StrokeDashArray;
      DoubleCollection doubleCollection = new DoubleCollection();
      if (strokeDashArray != null && strokeDashArray.Count > 0)
      {
        foreach (double num in strokeDashArray)
          doubleCollection.Add(num);
        this.polyline.StrokeDashArray = doubleCollection;
      }
    }
    else
      this.Stroke = this.Stroke;
    return visual;
  }

  public override void Update(IChartTransformer transformer)
  {
    if (this.Length == 0)
    {
      base.Update(transformer);
    }
    else
    {
      if (transformer == null || this.Series.DataCount <= 1)
        return;
      ChartTransform.ChartCartesianTransformer cartesianTransformer = transformer as ChartTransform.ChartCartesianTransformer;
      if (!cartesianTransformer.XAxis.IsLogarithmic && !cartesianTransformer.YAxis.IsLogarithmic)
        this.TransformToScreenCo(cartesianTransformer);
      else
        this.TransformToScreenCoInLog(cartesianTransformer);
      this.UpdateVisual(true);
    }
  }

  private void TransformToScreenCo(
    ChartTransform.ChartCartesianTransformer cartesianTransformer)
  {
    ChartSeries series = this.Series as ChartSeries;
    bool isInversed1 = cartesianTransformer.XAxis.IsInversed;
    bool isInversed2 = cartesianTransformer.YAxis.IsInversed;
    double num1 = cartesianTransformer.XAxis.VisibleRange.Start;
    double num2 = cartesianTransformer.XAxis.VisibleRange.End;
    double num3 = cartesianTransformer.YAxis.VisibleRange.Start;
    double end = cartesianTransformer.YAxis.VisibleRange.End;
    double num4 = isInversed1 ? num1 - num2 : num2 - num1;
    double num5 = isInversed2 ? num3 - end : end - num3;
    this.availableSize = new Size(cartesianTransformer.XAxis.RenderedRect.Width, cartesianTransformer.YAxis.RenderedRect.Height);
    double num6 = Math.Abs(num4 * 1.0 / this.availableSize.Width);
    double num7 = Math.Abs(num5 * 1.0 / this.availableSize.Height);
    double x = cartesianTransformer.XAxis.RenderedRect.Left - series.Area.SeriesClipRect.Left;
    double num8 = cartesianTransformer.YAxis.RenderedRect.Top - series.Area.SeriesClipRect.Top;
    int num9 = (int) Math.Ceiling(num2);
    int num10 = (int) Math.Floor(num1);
    if (isInversed1)
    {
      double num11 = num1;
      num1 = num2;
      num2 = num11;
    }
    if (isInversed2)
      num3 = end;
    if (this.RenderingMode == RenderingMode.Default)
    {
      this.Points = new PointCollection();
      double num12 = 0.0;
      double num13 = 0.0;
      if (series.IsIndexed)
      {
        int num14 = this.Length - 1;
        int num15 = num10 > num14 ? num10 : num14;
        int num16 = num15 < 0 ? 0 : num15;
        int num17 = num9 > this.yChartVals.Count ? this.yChartVals.Count : num9;
        for (int index = num16; index <= num17; ++index)
        {
          if (index >= 0 && index < this.yChartVals.Count)
          {
            double xChartVal = this.xChartVals[index];
            double yChartVal = this.yChartVals[index];
            if (Math.Abs(num12 - (double) index) >= num6 || Math.Abs(num13 - yChartVal) >= num7)
            {
              if (xChartVal <= num2 && xChartVal >= num1)
                this.Points.Add(new Point(x + this.availableSize.Width * (((double) index - num1) / num4), num8 + this.availableSize.Height * (1.0 - (yChartVal - num3) / num5)));
              else if (xChartVal < num1 && index + 1 < this.yChartVals.Count)
              {
                double interpolarationPoint = ChartMath.GetInterpolarationPoint(this.xChartVals[index], this.xChartVals[index + 1], yChartVal, this.yChartVals[index + 1], num1);
                this.Points.Add(new Point(x, num8 + this.availableSize.Height * (1.0 - (interpolarationPoint - num3) / num5)));
              }
              else if (xChartVal > num2 && index - 1 > 0)
              {
                double interpolarationPoint = ChartMath.GetInterpolarationPoint(this.xChartVals[index - 1], this.xChartVals[index], this.yChartVals[index - 1], yChartVal, num2);
                this.Points.Add(new Point(x + this.availableSize.Width, num8 + this.availableSize.Height * (1.0 - (interpolarationPoint - num3) / num5)));
              }
              num12 = (double) index;
              num13 = yChartVal;
            }
          }
        }
      }
      else
      {
        int index1 = 0;
        for (int index2 = this.Length - 1; index2 < this.xChartVals.Count; ++index2)
        {
          double xChartVal = this.xChartVals[index2];
          double yChartVal = this.yChartVals[index2];
          if (Math.Abs(num12 - xChartVal) >= num6 || Math.Abs(num13 - yChartVal) >= num7)
          {
            if (xChartVal <= num2 && xChartVal >= num1)
              this.Points.Add(new Point(x + this.availableSize.Width * ((xChartVal - num1) / num4), num8 + this.availableSize.Height * (1.0 - (yChartVal - num3) / num5)));
            else if (xChartVal < num1)
              index1 = index2;
            else if (xChartVal > num2 && index2 - 1 > -1)
            {
              double interpolarationPoint = ChartMath.GetInterpolarationPoint(this.xChartVals[index2 - 1], this.xChartVals[index2], this.yChartVals[index2 - 1], yChartVal, num2);
              this.Points.Add(new Point(x + this.availableSize.Width, num8 + this.availableSize.Height * (1.0 - (interpolarationPoint - num3) / num5)));
              break;
            }
          }
          num12 = xChartVal;
          num13 = yChartVal;
        }
        if (index1 <= 0 || index1 + 1 >= this.xChartVals.Count)
          return;
        double interpolarationPoint1 = ChartMath.GetInterpolarationPoint(this.xChartVals[index1], this.xChartVals[index1 + 1], this.yChartVals[index1], this.yChartVals[index1 + 1], num1);
        this.Points.Insert(0, new Point(x, num8 + this.availableSize.Height * (1.0 - (interpolarationPoint1 - num3) / num5)));
      }
    }
    else
    {
      this.xValues.Clear();
      this.yValues.Clear();
      double num18 = 0.0;
      double num19 = 0.0;
      if (series.IsIndexed)
      {
        double num20 = 1.0;
        for (int index = num10; index <= num9; ++index)
        {
          double yChartVal = this.yChartVals[index];
          if (Math.Abs(num20 - (double) index) >= num6 || Math.Abs(num19 - yChartVal) >= num7)
          {
            float num21 = (float) (x + this.availableSize.Width * (((double) index - num1) / num4));
            float num22 = (float) (num8 + this.availableSize.Height * (1.0 - (yChartVal - num3) / num5));
            this.xValues.Add((double) num21);
            this.yValues.Add((double) num22);
            num20 = (double) index;
            num19 = yChartVal;
          }
        }
        if (num10 > 0)
        {
          int index = num10 - 1;
          double yChartVal = this.yChartVals[index];
          float num23 = (float) (x + this.availableSize.Width * (((double) index - num1) / num4));
          float num24 = (float) (num8 + this.availableSize.Height * (1.0 - (yChartVal - num3) / num5));
          this.xValues.Insert(0, (double) num23);
          this.yValues.Insert(0, (double) num24);
        }
        if (num9 >= this.yChartVals.Count - 1)
          return;
        int index3 = num9 + 1;
        double yChartVal1 = this.yChartVals[index3];
        float num25 = (float) (x + this.availableSize.Width * (((double) index3 - num1) / num4));
        float num26 = (float) (num8 + this.availableSize.Height * (1.0 - (yChartVal1 - num3) / num5));
        this.xValues.Add((double) num25);
        this.yValues.Add((double) num26);
      }
      else
      {
        int index4 = 0;
        for (int index5 = this.Length - 1; index5 < this.xChartVals.Count; ++index5)
        {
          double xChartVal = this.xChartVals[index5];
          double yChartVal = this.yChartVals[index5];
          if (xChartVal <= (double) num9 && xChartVal >= (double) num10)
          {
            if (Math.Abs(num18 - xChartVal) >= num6 || Math.Abs(num19 - yChartVal) >= num7)
            {
              float num27 = (float) (x + this.availableSize.Width * ((xChartVal - num1) / num4));
              float num28 = (float) (num8 + this.availableSize.Height * (1.0 - (yChartVal - num3) / num5));
              this.xValues.Add((double) num27);
              this.yValues.Add((double) num28);
              num18 = xChartVal;
              num19 = yChartVal;
            }
          }
          else if (xChartVal < (double) num10)
            index4 = index5;
          else if (xChartVal > (double) num9)
          {
            float num29 = (float) (x + this.availableSize.Width * ((xChartVal - num1) / num4));
            float num30 = (float) (num8 + this.availableSize.Height * (1.0 - (yChartVal - num3) / num5));
            this.xValues.Add((double) num29);
            this.yValues.Add((double) num30);
            break;
          }
        }
        if (index4 <= 0)
          return;
        float num31 = (float) (x + this.availableSize.Width * ((this.xChartVals[index4] - num1) / num4));
        float num32 = (float) (num8 + this.availableSize.Height * (1.0 - (this.yChartVals[index4] - num3) / num5));
        this.xValues.Insert(0, (double) num31);
        this.yValues.Insert(0, (double) num32);
      }
    }
  }

  private void TransformToScreenCoInLog(
    ChartTransform.ChartCartesianTransformer cartesianTransformer)
  {
    ChartSeries series = this.Series as ChartSeries;
    bool isInversed1 = cartesianTransformer.XAxis.IsInversed;
    bool isInversed2 = cartesianTransformer.YAxis.IsInversed;
    double d = cartesianTransformer.XAxis.VisibleRange.Start;
    double end1 = cartesianTransformer.XAxis.VisibleRange.End;
    double num1 = cartesianTransformer.YAxis.VisibleRange.Start;
    double end2 = cartesianTransformer.YAxis.VisibleRange.End;
    double num2 = isInversed1 ? d - end1 : end1 - d;
    double num3 = isInversed2 ? num1 - end2 : end2 - num1;
    this.availableSize = new Size(cartesianTransformer.XAxis.RenderedRect.Width, cartesianTransformer.YAxis.RenderedRect.Height);
    double num4 = Math.Abs(num2 * 1.0 / this.availableSize.Width);
    double num5 = Math.Abs(num3 * 1.0 / this.availableSize.Height);
    double num6 = cartesianTransformer.XAxis.RenderedRect.Left - series.Area.SeriesClipRect.Left;
    double num7 = cartesianTransformer.YAxis.RenderedRect.Top - series.Area.SeriesClipRect.Top;
    double newBase1 = cartesianTransformer.XAxis.IsLogarithmic ? (cartesianTransformer.XAxis as LogarithmicAxis).LogarithmicBase : 1.0;
    double newBase2 = cartesianTransformer.YAxis.IsLogarithmic ? (cartesianTransformer.YAxis as LogarithmicAxis).LogarithmicBase : 1.0;
    int num8 = (int) Math.Ceiling(end1);
    int num9 = (int) Math.Floor(d);
    if (isInversed1)
      d = end1;
    if (isInversed2)
      num1 = end2;
    if (this.RenderingMode == RenderingMode.Default)
    {
      this.Points = new PointCollection();
      double num10 = 0.0;
      double num11 = 0.0;
      if (series.IsIndexed)
      {
        double num12 = 1.0;
        for (int index = num9; index <= num8; ++index)
        {
          double num13 = newBase2 == 1.0 ? this.yChartVals[index] : Math.Log(this.yChartVals[index], newBase2);
          double num14 = newBase1 == 1.0 ? (double) index : Math.Log((double) index, newBase1);
          if (Math.Abs(num12 - (double) index) >= num4 || Math.Abs(num11 - num13) >= num5)
          {
            this.Points.Add(new Point(num6 + this.availableSize.Width * ((num14 - d) / num2), num7 + this.availableSize.Height * (1.0 - (num13 - num1) / num3)));
            num12 = (double) index;
            num11 = num13;
          }
        }
        if (num9 > 0)
        {
          int index = num9 - 1;
          double num15 = newBase2 == 1.0 ? this.yChartVals[index] : Math.Log(this.yChartVals[index], newBase2);
          this.Points.Insert(0, new Point(num6 + this.availableSize.Width * (((double) index - d) / num2), num7 + this.availableSize.Height * (1.0 - (num15 - num1) / num3)));
        }
        if (num8 >= this.yChartVals.Count - 1)
          return;
        int index1 = num8 + 1;
        double num16 = newBase2 == 1.0 ? this.yChartVals[index1] : Math.Log(this.yChartVals[index1], newBase2);
        this.Points.Add(new Point(num6 + this.availableSize.Width * (((double) index1 - d) / num2), num7 + this.availableSize.Height * (1.0 - (num16 - num1) / num3)));
      }
      else
      {
        int index2 = 0;
        for (int index3 = 0; index3 < this.xChartVals.Count; ++index3)
        {
          double a1 = newBase1 == 1.0 ? this.xChartVals[index3] : Math.Log(this.xChartVals[index3], newBase1);
          double a2 = newBase2 == 1.0 ? this.yChartVals[index3] : Math.Log(this.yChartVals[index3], newBase2);
          if (a1 <= (double) num8 && a1 >= (double) num9)
          {
            if (Math.Abs(num10 - a1) >= num4 || Math.Abs(num11 - a2) >= num5)
            {
              this.Points.Add(new Point(num6 + this.availableSize.Width * ((a1 - d) / num2), num7 + this.availableSize.Height * (1.0 - (a2 - num1) / num3)));
              num10 = a1;
              num11 = a2;
            }
          }
          else if (a1 < (double) num9)
            index2 = index3;
          else if (a1 > (double) num8)
          {
            double num17 = newBase1 == 1.0 ? a1 : Math.Log(a1, newBase1);
            double num18 = newBase2 == 1.0 ? a2 : Math.Log(a2, newBase2);
            this.Points.Add(new Point(num6 + this.availableSize.Width * ((num17 - d) / num2), num7 + this.availableSize.Height * (1.0 - (num18 - num1) / num3)));
            break;
          }
        }
        if (index2 <= 0)
          return;
        double num19 = newBase1 == 1.0 ? this.xChartVals[index2] : Math.Log(this.xChartVals[index2], newBase1);
        double num20 = newBase2 == 1.0 ? this.yChartVals[index2] : Math.Log(this.yChartVals[index2], newBase2);
        this.Points.Insert(0, new Point(num6 + this.availableSize.Width * ((num19 - d) / num2), num7 + this.availableSize.Height * (1.0 - (num20 - num1) / num3)));
      }
    }
    else
    {
      this.xValues.Clear();
      this.yValues.Clear();
      double num21 = 0.0;
      double num22 = 0.0;
      if (series.IsIndexed)
      {
        double num23 = 1.0;
        for (int index = num9; index <= num8; ++index)
        {
          double num24 = newBase2 == 1.0 ? this.yChartVals[index] : Math.Log(this.yChartVals[index], newBase2);
          double num25 = newBase1 == 1.0 ? (double) index : Math.Log((double) index, newBase1);
          if (Math.Abs(num23 - (double) index) >= 1.0 || Math.Abs(num22 - num24) >= 1.0)
          {
            float num26 = (float) (num6 + this.availableSize.Width * ((num25 - d) / num2));
            float num27 = (float) (num7 + this.availableSize.Height * (1.0 - (num24 - num1) / num3));
            this.xValues.Add((double) num26);
            this.yValues.Add((double) num27);
            num23 = (double) index;
            num22 = num24;
          }
        }
        if (num9 > 0)
        {
          int num28 = num9 - 1;
          double num29 = newBase2 == 1.0 ? this.yChartVals[num28] : Math.Log(this.yChartVals[num28], newBase2);
          double num30 = newBase1 == 1.0 ? (double) num28 : Math.Log((double) num28, newBase1);
          float num31 = (float) (num6 + this.availableSize.Width * ((num30 - d) / num2));
          float num32 = (float) (num7 + this.availableSize.Height * (1.0 - (num29 - num1) / num3));
          this.xValues.Insert(0, (double) num31);
          this.yValues.Insert(0, (double) num32);
        }
        if (num8 >= this.yChartVals.Count - 1)
          return;
        int num33 = num8 + 1;
        double num34 = newBase2 == 1.0 ? this.yChartVals[num33] : Math.Log(this.yChartVals[num33], newBase2);
        double num35 = newBase1 == 1.0 ? (double) num33 : Math.Log((double) num33, newBase1);
        float num36 = (float) (num6 + this.availableSize.Width * ((num35 - d) / num2));
        float num37 = (float) (num7 + this.availableSize.Height * (1.0 - (num34 - num1) / num3));
        this.xValues.Add((double) num36);
        this.yValues.Add((double) num37);
      }
      else
      {
        int index4 = 0;
        for (int index5 = 0; index5 < this.xChartVals.Count; ++index5)
        {
          double a3 = newBase1 == 1.0 ? this.xChartVals[index5] : Math.Log(this.xChartVals[index5], newBase1);
          double a4 = newBase2 == 1.0 ? this.yChartVals[index5] : Math.Log(this.yChartVals[index5], newBase2);
          if (a3 <= (double) num8 && a3 >= (double) num9)
          {
            if (Math.Abs(num21 - a3) >= num4 || Math.Abs(num22 - a4) >= num5)
            {
              float num38 = (float) (num6 + this.availableSize.Width * ((a3 - d) / num2));
              float num39 = (float) (num7 + this.availableSize.Height * (1.0 - (a4 - num1) / num3));
              this.xValues.Add((double) num38);
              this.yValues.Add((double) num39);
              num21 = a3;
              num22 = a4;
            }
          }
          else if (a3 < (double) num9)
            index4 = index5;
          else if (a3 > (double) num8)
          {
            double num40 = newBase1 == 1.0 ? a3 : Math.Log(a3, newBase1);
            double num41 = newBase2 == 1.0 ? a4 : Math.Log(a4, newBase2);
            float num42 = (float) (num6 + this.availableSize.Width * ((num40 - d) / num2));
            float num43 = (float) (num7 + this.availableSize.Height * (1.0 - (num41 - num1) / num3));
            this.xValues.Add((double) num42);
            this.yValues.Add((double) num43);
            break;
          }
        }
        if (index4 <= 0)
          return;
        double num44 = newBase1 == 1.0 ? this.xChartVals[index4] : Math.Log(this.xChartVals[index4], newBase1);
        double num45 = newBase2 == 1.0 ? this.yChartVals[index4] : Math.Log(this.yChartVals[index4], newBase2);
        float num46 = (float) (num6 + this.availableSize.Width * ((num44 - d) / num2));
        float num47 = (float) (num7 + this.availableSize.Height * (1.0 - (num45 - num1) / num3));
        this.xValues.Insert(0, (double) num46);
        this.yValues.Insert(0, (double) num47);
      }
    }
  }
}
