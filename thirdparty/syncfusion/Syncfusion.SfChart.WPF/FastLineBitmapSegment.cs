// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.FastLineBitmapSegment
// Assembly: Syncfusion.SfChart.WPF, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 279D70C6-7936-4F3E-B6CF-79EFEF734826
// Assembly location: C:\Program Files\PDFgear\Syncfusion.SfChart.WPF.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

#nullable disable
namespace Syncfusion.UI.Xaml.Charts;

public class FastLineBitmapSegment : ChartSegment
{
  private List<double> xValues = new List<double>();
  private List<double> yValues = new List<double>();
  private IList<double> xChartVals;
  private IList<double> yChartVals;
  private int[] points1;
  private int[] points2;
  private Point intersectingPoint;
  private WriteableBitmap bitmap;
  private double xStart;
  private double xEnd;
  private double yStart;
  private double yEnd;
  private double xDelta;
  private double yDelta;
  private double xSize;
  private double ySize;
  private double xOffset;
  private double yOffset;
  private double xTolerance;
  private double yTolerance;
  private int count;
  private int start;
  private bool isSeriesSelected;
  private Color seriesSelectionColor = Colors.Transparent;
  private bool isGrouping = true;

  public FastLineBitmapSegment()
  {
  }

  public FastLineBitmapSegment(ChartSeries series) => this.Series = (ChartSeriesBase) series;

  public FastLineBitmapSegment(IList<double> xVals, IList<double> yVals, AdornmentSeries series)
    : this((ChartSeries) series)
  {
    this.Series = (ChartSeriesBase) series;
    if (this.Series.ActualXAxis is CategoryAxis && !(this.Series.ActualXAxis as CategoryAxis).IsIndexed)
      this.Item = (object) series.GroupedActualData;
    else
      this.Item = (object) series.ActualData;
    this.xChartVals = xVals;
    this.yChartVals = yVals;
    this.SetRange();
  }

  public override void SetData(IList<double> xVals, IList<double> yVals)
  {
    this.xChartVals = xVals;
    this.yChartVals = yVals;
    this.SetRange();
  }

  public override UIElement CreateVisual(Size size)
  {
    this.bitmap = (this.Series as ChartSeries).Area.GetFastRenderSurface();
    return (UIElement) null;
  }

  public override void OnSizeChanged(Size size)
  {
    this.bitmap = (this.Series as ChartSeries).Area.GetFastRenderSurface();
  }

  public override UIElement GetRenderedVisual() => (UIElement) null;

  public override void Update(IChartTransformer transformer)
  {
    ChartSeries series = this.Series as ChartSeries;
    this.bitmap = series.Area.GetFastRenderSurface();
    if (transformer == null || series.DataCount <= 1)
      return;
    ChartTransform.ChartCartesianTransformer cartesianTransformer = transformer as ChartTransform.ChartCartesianTransformer;
    if (cartesianTransformer.XAxis is DateTimeAxis xaxis && xaxis.EnableBusinessHours)
    {
      this.CalculatePoints(cartesianTransformer);
    }
    else
    {
      bool flag = cartesianTransformer.XAxis.IsLogarithmic || cartesianTransformer.YAxis.IsLogarithmic;
      this.x_isInversed = cartesianTransformer.XAxis.IsInversed;
      this.y_isInversed = cartesianTransformer.YAxis.IsInversed;
      this.xStart = cartesianTransformer.XAxis.VisibleRange.Start;
      this.xEnd = cartesianTransformer.XAxis.VisibleRange.End;
      this.yStart = cartesianTransformer.YAxis.VisibleRange.Start;
      this.yEnd = cartesianTransformer.YAxis.VisibleRange.End;
      this.xDelta = this.x_isInversed ? this.xStart - this.xEnd : this.xEnd - this.xStart;
      this.yDelta = this.y_isInversed ? this.yStart - this.yEnd : this.yEnd - this.yStart;
      if (series.IsActualTransposed)
      {
        this.ySize = (double) (int) cartesianTransformer.YAxis.RenderedRect.Width;
        this.xSize = (double) (int) cartesianTransformer.XAxis.RenderedRect.Height;
        this.yOffset = cartesianTransformer.YAxis.RenderedRect.Left - series.Area.SeriesClipRect.Left - series.Area.AreaBorderThickness.Left;
        this.xOffset = cartesianTransformer.XAxis.RenderedRect.Top - series.Area.SeriesClipRect.Top - series.Area.AreaBorderThickness.Top;
      }
      else
      {
        this.ySize = (double) (int) cartesianTransformer.YAxis.RenderedRect.Height;
        this.xSize = (double) (int) cartesianTransformer.XAxis.RenderedRect.Width;
        this.yOffset = cartesianTransformer.YAxis.RenderedRect.Top - series.Area.SeriesClipRect.Top - series.Area.AreaBorderThickness.Top;
        this.xOffset = cartesianTransformer.XAxis.RenderedRect.Left - series.Area.SeriesClipRect.Left - series.Area.AreaBorderThickness.Left;
      }
      this.xTolerance = Math.Abs(this.xDelta * 1.0 / this.xSize);
      this.yTolerance = Math.Abs(this.yDelta * 1.0 / this.ySize);
      this.count = (int) Math.Ceiling(this.xEnd);
      this.start = (int) Math.Floor(this.xStart);
      if (this.x_isInversed)
      {
        double xStart = this.xStart;
        this.xStart = this.xEnd;
        this.xEnd = xStart;
      }
      if (this.y_isInversed)
      {
        double yStart = this.yStart;
        this.yStart = this.yEnd;
        this.yEnd = yStart;
      }
      this.xValues.Clear();
      this.yValues.Clear();
      if (!flag)
        this.TransformToScreenCo();
      else
        this.TransformToScreenCoInLog(cartesianTransformer);
    }
    this.UpdateVisual(true);
  }

  internal void SetRange()
  {
    this.isGrouping = !(this.Series.ActualXAxis is CategoryAxis) || (this.Series.ActualXAxis as CategoryAxis).IsIndexed;
    if (this.Series.DataCount <= 0)
      return;
    double d = this.yChartVals.Min();
    double start;
    if (double.IsNaN(d))
    {
      IEnumerable<double> source = this.yChartVals.Where<double>((Func<double, bool>) (e => !double.IsNaN(e)));
      start = !source.Any<double>() ? 0.0 : source.Min();
    }
    else
      start = d;
    if (this.Series.IsIndexed)
    {
      double end1 = !this.isGrouping ? this.xChartVals.Max() : (double) (this.Series.DataCount - 1);
      double end2 = this.yChartVals.Max();
      this.XRange = new DoubleRange(0.0, end1);
      this.YRange = new DoubleRange(start, end2);
    }
    else
    {
      double end3 = this.xChartVals.Max();
      double end4 = this.yChartVals.Max();
      this.XRange = new DoubleRange(this.xChartVals.Min(), end3);
      this.YRange = new DoubleRange(start, end4);
    }
  }

  internal void UpdateVisual(bool updatePolyline)
  {
    ChartSeries series = this.Series as ChartSeries;
    SfChart area = series.Area;
    this.isSeriesSelected = false;
    bool isMultiColor = series.SegmentColorPath != null || series.Palette != ChartColorPalette.None;
    Color color1 = isMultiColor ? ChartSegment.GetColor(series.GetInteriorColor(0)) : (this.Interior == null ? new SolidColorBrush(Colors.Transparent).Color : ((SolidColorBrush) this.Interior).Color);
    if (area.GetEnableSeriesSelection())
    {
      Brush seriesSelectionBrush = area.GetSeriesSelectionBrush((ChartSeriesBase) series);
      if (seriesSelectionBrush != null && area.SelectedSeriesCollection.Contains((ChartSeriesBase) series))
      {
        this.isSeriesSelected = true;
        this.seriesSelectionColor = ((SolidColorBrush) seriesSelectionBrush).Color;
      }
    }
    Color color2 = this.isSeriesSelected ? this.seriesSelectionColor : color1;
    if (series.StrokeThickness <= 0.0)
    {
      this.bitmap.BeginWrite();
      this.bitmap.EndWrite();
    }
    if (this.bitmap != null && this.xValues.Count > 1 && series.StrokeThickness > 0.0)
    {
      this.xStart = this.xValues[0];
      this.yStart = this.yValues[0];
      int width = (int) series.Area.SeriesClipRect.Width;
      int height = (int) series.Area.SeriesClipRect.Height;
      double leftThickness = series.StrokeThickness / 2.0;
      double rightThickness = series.StrokeThickness % 2.0 == 0.0 ? series.StrokeThickness / 2.0 - 1.0 : series.StrokeThickness / 2.0;
      this.bitmap.BeginWrite();
      if (series is FastLineBitmapSeries)
      {
        FastLineBitmapSeries lineBitmapSeries = (FastLineBitmapSeries) series;
        if (((FastLineBitmapSeries) series).EnableAntiAliasing)
        {
          if (lineBitmapSeries.StrokeDashArray == null || lineBitmapSeries.StrokeDashArray != null && lineBitmapSeries.StrokeDashArray.Count <= 1)
          {
            if (series.IsActualTransposed)
              this.DrawLineAa(this.yValues, this.xValues, width, height, color2, leftThickness, rightThickness, isMultiColor);
            else
              this.DrawLineAa(this.xValues, this.yValues, width, height, color2, leftThickness, rightThickness, isMultiColor);
          }
          else
            this.DrawDashedAaLines(width, height, color2, leftThickness, rightThickness);
        }
        else if (lineBitmapSeries.StrokeDashArray == null || lineBitmapSeries.StrokeDashArray != null && lineBitmapSeries.StrokeDashArray.Count <= 1)
        {
          if (series.IsActualTransposed)
            this.DrawLine(this.yValues, this.xValues, width, height, color2, leftThickness, rightThickness, isMultiColor);
          else
            this.DrawLine(this.xValues, this.yValues, width, height, color2, leftThickness, rightThickness, isMultiColor);
        }
        else
          this.DrawDashedLines(width, height, color2, leftThickness, rightThickness);
      }
      this.bitmap.EndWrite();
    }
    series.Area.CanRenderToBuffer = true;
    this.xValues.Clear();
    this.yValues.Clear();
  }

  protected override void SetVisualBindings(Shape element)
  {
    element.SetBinding(Shape.StrokeProperty, (BindingBase) new Binding()
    {
      Source = (object) this,
      Path = new PropertyPath("Interior", new object[0])
    });
    element.SetBinding(Shape.StrokeThicknessProperty, (BindingBase) new Binding()
    {
      Source = (object) this,
      Path = new PropertyPath("StrokeThickness", new object[0])
    });
  }

  private static void GetLinePoints(
    double x1,
    double y1,
    double x2,
    double y2,
    double leftThickness,
    double rightThickness,
    int[] points)
  {
    double x = x2 - x1;
    double num1 = Math.Atan2(y2 - y1, x);
    double num2 = Math.Cos(-num1);
    double num3 = Math.Sin(-num1);
    double num4 = x1 * num2 - y1 * num3;
    double num5 = x1 * num3 + y1 * num2;
    double num6 = x2 * num2 - y2 * num3;
    double num7 = x2 * num3 + y2 * num2;
    double num8 = Math.Cos(num1);
    double num9 = Math.Sin(num1);
    double num10 = num4 * num8 - (num5 + leftThickness) * num9;
    double num11 = num4 * num9 + (num5 + leftThickness) * num8;
    double num12 = num6 * num8 - (num7 + leftThickness) * num9;
    double num13 = num6 * num9 + (num7 + leftThickness) * num8;
    double num14 = num4 * num8 - (num5 - rightThickness) * num9;
    double num15 = num4 * num9 + (num5 - rightThickness) * num8;
    double num16 = num6 * num8 - (num7 - rightThickness) * num9;
    double num17 = num6 * num9 + (num7 - rightThickness) * num8;
    points[0] = (int) num10;
    points[1] = (int) num11;
    points[2] = (int) num12;
    points[3] = (int) num13;
    points[4] = (int) num16;
    points[5] = (int) num17;
    points[6] = (int) num14;
    points[7] = (int) num15;
    points[8] = (int) num10;
    points[9] = (int) num11;
  }

  private static double CalcLenOfLine(double x1, double x2, double y1, double y2)
  {
    double num1 = x2 - x1;
    double num2 = y2 - y1;
    return Math.Sqrt(num1 * num1 + num2 * num2);
  }

  private void CalculatePoints(
    ChartTransform.ChartCartesianTransformer cartesianTransformer)
  {
    this.xValues.Clear();
    this.yValues.Clear();
    int num = this.xChartVals.Count - 1;
    if (!this.Series.IsActualTransposed)
    {
      for (int index = 0; index <= num; ++index)
      {
        double xChartVal = this.xChartVals[index];
        double yChartVal = this.yChartVals[index];
        Point visible = cartesianTransformer.TransformToVisible(xChartVal, yChartVal);
        this.xValues.Add(visible.X);
        this.yValues.Add(visible.Y);
      }
    }
    else
    {
      for (int index = 0; index <= num; ++index)
      {
        double xChartVal = this.xChartVals[index];
        double yChartVal = this.yChartVals[index];
        Point visible = cartesianTransformer.TransformToVisible(xChartVal, yChartVal);
        this.xValues.Add(visible.Y);
        this.yValues.Add(visible.X);
      }
    }
  }

  private void TransformToScreenCo()
  {
    if (!this.Series.IsActualTransposed)
      this.TransformToScreenCoHorizontal();
    else
      this.TransformToScreenCoVertical();
  }

  private void TransformToScreenCoHorizontal()
  {
    ChartSeries series = this.Series as ChartSeries;
    double num1 = 0.0;
    bool flag = series.ActualYAxis is NumericalAxis actualYaxis && actualYaxis.AxisRanges != null && actualYaxis.AxisRanges.Count > 0;
    if (series.IsIndexed)
    {
      this.start = this.start < 0 ? 0 : this.start;
      double num2 = 1.0;
      this.count = this.isGrouping ? (this.count > this.yChartVals.Count - 1 ? this.yChartVals.Count - 1 : this.count) : this.xChartVals.Count - 1;
      for (int start = this.start; start <= this.count; ++start)
      {
        double num3 = 0.0;
        if (this.yChartVals.Count > start)
          num3 = this.yChartVals[start];
        if (Math.Abs(num2 - (double) start) >= this.xTolerance || Math.Abs(num1 - num3) >= this.yTolerance)
        {
          this.xValues.Add(this.xOffset + this.xSize * ((!this.isGrouping ? this.xChartVals[start] : (double) start - this.xStart) / this.xDelta));
          this.yValues.Add(this.yOffset + this.ySize * (1.0 - (flag ? actualYaxis.CalculateValueToCoefficient(num3) : (num3 - this.yStart) / this.yDelta)));
          num2 = (double) start;
          num1 = num3;
        }
      }
      if (this.start > 0 && this.start < this.yChartVals.Count)
      {
        int index = this.start - 1;
        double yChartVal = this.yChartVals[index];
        this.xValues.Insert(0, this.xOffset + this.xSize * (((double) index - this.xStart) / this.xDelta));
        this.yValues.Insert(0, this.yOffset + this.ySize * (1.0 - (flag ? actualYaxis.CalculateValueToCoefficient(yChartVal) : (yChartVal - this.yStart) / this.yDelta)));
      }
      if (this.count >= this.yChartVals.Count - 1)
        return;
      int index1 = this.count + 1;
      double yChartVal1 = this.yChartVals[index1];
      this.xValues.Add(this.xOffset + this.xSize * (((double) index1 - this.xStart) / this.xDelta));
      this.yValues.Add(this.yOffset + this.ySize * (1.0 - (flag ? actualYaxis.CalculateValueToCoefficient(yChartVal1) : (yChartVal1 - this.yStart) / this.yDelta)));
    }
    else
    {
      int index2 = 0;
      double num4 = this.xChartVals[0];
      double num5 = this.yChartVals[0];
      int index3 = this.xChartVals.Count - 1;
      int index4;
      if (series.isLinearData)
      {
        double num6 = Math.Floor(this.xStart);
        double num7 = Math.Ceiling(this.xEnd);
        for (index4 = 1; index4 < index3; ++index4)
        {
          double xChartVal = this.xChartVals[index4];
          double yChartVal = this.yChartVals[index4];
          if (xChartVal <= num7 && xChartVal >= num6)
          {
            if (Math.Abs(num4 - xChartVal) >= this.xTolerance || Math.Abs(num5 - yChartVal) >= this.yTolerance)
            {
              this.xValues.Add(this.xOffset + this.xSize * ((xChartVal - this.xStart) / this.xDelta));
              this.yValues.Add(this.yOffset + this.ySize * (1.0 - (flag ? actualYaxis.CalculateValueToCoefficient(yChartVal) : (yChartVal - this.yStart) / this.yDelta)));
              num4 = xChartVal;
              num5 = yChartVal;
            }
          }
          else if (xChartVal < num6)
          {
            if (this.x_isInversed)
            {
              this.xValues.Add(this.xOffset + this.xSize * ((xChartVal - this.xStart) / this.xDelta));
              this.yValues.Add(this.yOffset + this.ySize * (1.0 - (flag ? actualYaxis.CalculateValueToCoefficient(yChartVal) : (yChartVal - this.yStart) / this.yDelta)));
            }
            else
              index2 = index4;
          }
          else if (xChartVal > num7)
          {
            this.xValues.Add(this.xOffset + this.xSize * ((xChartVal - this.xStart) / this.xDelta));
            this.yValues.Add(this.yOffset + this.ySize * (1.0 - (flag ? actualYaxis.CalculateValueToCoefficient(yChartVal) : (yChartVal - this.yStart) / this.yDelta)));
            break;
          }
        }
      }
      else
      {
        for (index4 = 1; index4 < index3; ++index4)
        {
          double xChartVal = this.xChartVals[index4];
          double yChartVal = this.yChartVals[index4];
          if (Math.Abs(num4 - xChartVal) >= this.xTolerance || Math.Abs(num5 - yChartVal) >= this.yTolerance)
          {
            this.xValues.Add(this.xOffset + this.xSize * ((xChartVal - this.xStart) / this.xDelta));
            this.yValues.Add(this.yOffset + this.ySize * (1.0 - (flag ? actualYaxis.CalculateValueToCoefficient(yChartVal) : (yChartVal - this.yStart) / this.yDelta)));
            num4 = xChartVal;
            num5 = yChartVal;
          }
        }
      }
      this.xValues.Insert(0, this.xOffset + this.xSize * ((this.xChartVals[index2] - this.xStart) / this.xDelta));
      this.yValues.Insert(0, this.yOffset + this.ySize * (1.0 - (flag ? actualYaxis.CalculateValueToCoefficient(this.yChartVals[index2]) : (this.yChartVals[index2] - this.yStart) / this.yDelta)));
      if (index4 != index3)
        return;
      this.xValues.Add(this.xOffset + this.xSize * ((this.xChartVals[index3] - this.xStart) / this.xDelta));
      this.yValues.Add(this.yOffset + this.ySize * (1.0 - (flag ? actualYaxis.CalculateValueToCoefficient(this.yChartVals[index3]) : (this.yChartVals[index3] - this.yStart) / this.yDelta)));
    }
  }

  private void TransformToScreenCoVertical()
  {
    double num1 = 0.0;
    bool flag = this.Series.ActualYAxis is NumericalAxis actualYaxis && actualYaxis.AxisRanges != null && actualYaxis.AxisRanges.Count > 0;
    if (this.Series.IsIndexed)
    {
      this.start = this.start < 0 ? 0 : this.start;
      this.count = this.isGrouping ? (this.count > this.yChartVals.Count - 1 ? this.yChartVals.Count - 1 : this.count) : this.xChartVals.Count - 1;
      double num2 = 1.0;
      for (int start = this.start; start <= this.count; ++start)
      {
        double yChartVal = this.yChartVals[start];
        if (Math.Abs(num2 - (double) start) >= this.xTolerance || Math.Abs(num1 - yChartVal) >= this.yTolerance)
        {
          this.xValues.Add(this.xOffset + this.xSize * ((this.xEnd - (double) start) / this.xDelta));
          this.yValues.Add(this.yOffset + this.ySize * (flag ? actualYaxis.CalculateValueToCoefficient(yChartVal) : 1.0 - (this.yEnd - yChartVal) / this.yDelta));
          num2 = (double) start;
          num1 = yChartVal;
        }
      }
      if (this.start > 0 && this.start < this.yChartVals.Count)
      {
        int index = this.start - 1;
        double yChartVal = this.yChartVals[index];
        this.xValues.Insert(0, this.xOffset + this.xSize * ((this.xEnd - (double) index) / this.xDelta));
        this.yValues.Insert(0, this.yOffset + this.ySize * (flag ? actualYaxis.CalculateValueToCoefficient(yChartVal) : 1.0 - (this.yEnd - yChartVal) / this.yDelta));
      }
      if (this.count >= this.yChartVals.Count - 1)
        return;
      int index1 = this.count + 1;
      double yChartVal1 = this.yChartVals[index1];
      this.xValues.Add(this.xOffset + this.xSize * ((this.xEnd - (double) index1) / this.xDelta));
      this.yValues.Add(this.yOffset + this.ySize * (flag ? actualYaxis.CalculateValueToCoefficient(yChartVal1) : 1.0 - (this.yEnd - yChartVal1) / this.yDelta));
    }
    else
    {
      int index2 = 0;
      double num3 = this.xChartVals[0];
      double num4 = this.yChartVals[0];
      int index3 = this.xChartVals.Count - 1;
      int index4;
      if (this.Series.isLinearData)
      {
        for (index4 = 1; index4 < index3; ++index4)
        {
          double xChartVal = this.xChartVals[index4];
          double yChartVal = this.yChartVals[index4];
          if (xChartVal <= this.xEnd && xChartVal >= this.xStart)
          {
            if (Math.Abs(num3 - xChartVal) >= this.xTolerance || Math.Abs(num4 - yChartVal) >= this.yTolerance)
            {
              this.xValues.Add(this.xOffset + this.xSize * ((this.xEnd - xChartVal) / this.xDelta));
              this.yValues.Add(this.yOffset + this.ySize * (flag ? actualYaxis.CalculateValueToCoefficient(yChartVal) : 1.0 - (this.yEnd - yChartVal) / this.yDelta));
              num3 = xChartVal;
              num4 = yChartVal;
            }
          }
          else if (xChartVal < this.xStart)
          {
            if (this.x_isInversed)
            {
              this.xValues.Add(this.xOffset + this.xSize * ((this.xEnd - xChartVal) / this.xDelta));
              this.yValues.Add(this.yOffset + this.ySize * (flag ? actualYaxis.CalculateValueToCoefficient(yChartVal) : 1.0 - (this.yEnd - yChartVal) / this.yDelta));
            }
            else
              index2 = index4;
          }
          else if (xChartVal > this.xEnd)
          {
            this.xValues.Add(this.xOffset + this.xSize * ((this.xEnd - xChartVal) / this.xDelta));
            this.yValues.Add(this.yOffset + this.ySize * (flag ? actualYaxis.CalculateValueToCoefficient(yChartVal) : 1.0 - (this.yEnd - yChartVal) / this.yDelta));
            break;
          }
        }
      }
      else
      {
        for (index4 = 1; index4 < index3; ++index4)
        {
          double xChartVal = this.xChartVals[index4];
          double yChartVal = this.yChartVals[index4];
          if (Math.Abs(num3 - xChartVal) >= this.xTolerance || Math.Abs(num4 - yChartVal) >= this.yTolerance)
          {
            this.xValues.Add(this.xOffset + this.xSize * ((this.xEnd - xChartVal) / this.xDelta));
            this.yValues.Add(this.yOffset + this.ySize * (flag ? actualYaxis.CalculateValueToCoefficient(yChartVal) : 1.0 - (this.yEnd - yChartVal) / this.yDelta));
            num3 = xChartVal;
            num4 = yChartVal;
          }
        }
      }
      this.xValues.Insert(0, this.xOffset + this.xSize * ((this.xEnd - this.xChartVals[index2]) / this.xDelta));
      this.yValues.Insert(0, this.yOffset + this.ySize * (flag ? actualYaxis.CalculateValueToCoefficient(this.yChartVals[index2]) : 1.0 - (this.yEnd - this.yChartVals[index2]) / this.yDelta));
      if (index4 != index3)
        return;
      this.xValues.Add(this.xOffset + this.xSize * ((this.xEnd - this.xChartVals[index3]) / this.xDelta));
      this.yValues.Add(this.yOffset + this.ySize * (flag ? actualYaxis.CalculateValueToCoefficient(this.yChartVals[index3]) : 1.0 - (this.yEnd - this.yChartVals[index3]) / this.yDelta));
    }
  }

  private void TransformToScreenCoInLog(
    ChartTransform.ChartCartesianTransformer cartesianTransformer)
  {
    double xBase = cartesianTransformer.XAxis.IsLogarithmic ? (cartesianTransformer.XAxis as LogarithmicAxis).LogarithmicBase : 1.0;
    double yBase = cartesianTransformer.YAxis.IsLogarithmic ? (cartesianTransformer.YAxis as LogarithmicAxis).LogarithmicBase : 1.0;
    if (!this.Series.IsActualTransposed)
      this.TransformToScreenCoInLogHorizontal(xBase, yBase);
    else
      this.TransformToScreenCoInLogVertical(xBase, yBase);
  }

  private void TransformToScreenCoInLogVertical(double xBase, double yBase)
  {
    int index1 = 0;
    double num1 = 0.0;
    if (this.Series.IsIndexed)
    {
      this.start = this.start < 0 ? 0 : this.start;
      this.count = this.count > this.yChartVals.Count - 1 ? this.yChartVals.Count - 1 : this.count;
      double num2 = 1.0;
      for (int start = this.start; start <= this.count; ++start)
      {
        double num3 = yBase == 1.0 || this.yChartVals[start] == 0.0 ? this.yChartVals[start] : Math.Log(this.yChartVals[start], yBase);
        double num4 = xBase == 1.0 ? (double) start : Math.Log((double) start, xBase);
        if (Math.Abs(num2 - (double) start) >= 1.0 || Math.Abs(num1 - num3) >= 1.0)
        {
          this.xValues.Add(this.xOffset + this.xSize * ((this.xEnd - num4) / this.xDelta));
          this.yValues.Add(this.yOffset + this.ySize * (1.0 - (this.yEnd - num3) / this.yDelta));
          num2 = (double) start;
          num1 = num3;
        }
      }
      if (this.start > 0 && this.start < this.yChartVals.Count)
      {
        int num5 = this.start - 1;
        double num6 = yBase == 1.0 || this.yChartVals[num5] == 0.0 ? this.yChartVals[num5] : Math.Log(this.yChartVals[num5], yBase);
        this.xValues.Insert(0, this.xOffset + this.xSize * ((this.xEnd - (xBase == 1.0 ? (double) num5 : Math.Log((double) num5, xBase))) / this.xDelta));
        this.yValues.Insert(0, this.yOffset + this.ySize * (1.0 - (this.yEnd - num6) / this.yDelta));
      }
      if (this.count >= this.yChartVals.Count - 1)
        return;
      int num7 = this.count + 1;
      double num8 = yBase == 1.0 || this.yChartVals[num7] == 0.0 ? this.yChartVals[num7] : Math.Log(this.yChartVals[num7], yBase);
      this.xValues.Add(this.xOffset + this.xSize * ((this.xEnd - (xBase == 1.0 ? (double) num7 : Math.Log((double) num7, xBase))) / this.xDelta));
      this.yValues.Add(this.yOffset + this.ySize * (1.0 - (this.yEnd - num8) / this.yDelta));
    }
    else
    {
      double num9 = xBase == 1.0 || this.xChartVals[0] == 0.0 ? this.xChartVals[0] : Math.Log(this.xChartVals[0], xBase);
      double num10 = yBase == 1.0 || this.yChartVals[0] == 0.0 ? this.yChartVals[0] : Math.Log(this.yChartVals[0], yBase);
      int index2 = this.xChartVals.Count - 1;
      int index3;
      if (this.Series.isLinearData)
      {
        for (index3 = 1; index3 < index2; ++index3)
        {
          double num11 = xBase == 1.0 || this.xChartVals[index3] == 0.0 ? this.xChartVals[index3] : Math.Log(this.xChartVals[index3], xBase);
          double num12 = yBase == 1.0 || this.yChartVals[index3] == 0.0 ? this.yChartVals[index3] : Math.Log(this.yChartVals[index3], yBase);
          if (num11 <= (double) this.count && num11 >= (double) this.start)
          {
            if (Math.Abs(num9 - num11) >= this.xTolerance || Math.Abs(num10 - num12) >= this.yTolerance)
            {
              this.xValues.Add(this.xOffset + this.xSize * ((this.xEnd - num11) / this.xDelta));
              this.yValues.Add(this.yOffset + this.ySize * (1.0 - (this.yEnd - num12) / this.yDelta));
              num9 = num11;
              num10 = num12;
            }
          }
          else if (num11 < (double) this.start)
          {
            if (this.x_isInversed)
            {
              this.xValues.Add(this.xOffset + this.xSize * ((this.xEnd - num11) / this.xDelta));
              this.yValues.Add(this.yOffset + this.ySize * (1.0 - (this.yEnd - num12) / this.yDelta));
            }
            else
              index1 = index3;
          }
          else if (num11 > (double) this.count)
          {
            this.xValues.Add(this.xOffset + this.xSize * ((this.xEnd - num11) / this.xDelta));
            this.yValues.Add(this.yOffset + this.ySize * (1.0 - (this.yEnd - num12) / this.yDelta));
            break;
          }
        }
      }
      else
      {
        for (index3 = 1; index3 < index2; ++index3)
        {
          double num13 = xBase == 1.0 || this.xChartVals[index3] == 0.0 ? this.xChartVals[index3] : Math.Log(this.xChartVals[index3], xBase);
          double num14 = yBase == 1.0 || this.yChartVals[index3] == 0.0 ? this.yChartVals[index3] : Math.Log(this.yChartVals[index3], yBase);
          if (Math.Abs(num9 - num13) >= this.xTolerance || Math.Abs(num10 - num14) >= this.yTolerance)
          {
            this.xValues.Add(this.xOffset + this.xSize * ((this.xEnd - num13) / this.xDelta));
            this.yValues.Add(this.yOffset + this.ySize * (1.0 - (this.yEnd - num14) / this.yDelta));
            num9 = num13;
            num10 = num14;
          }
        }
      }
      double num15 = xBase == 1.0 || this.xChartVals[index1] == 0.0 ? this.xChartVals[index1] : Math.Log(this.xChartVals[index1], xBase);
      double num16 = yBase == 1.0 || this.yChartVals[index1] == 0.0 ? this.yChartVals[index1] : Math.Log(this.yChartVals[index1], yBase);
      this.xValues.Insert(0, this.xOffset + this.xSize * ((this.xEnd - num15) / this.xDelta));
      this.yValues.Insert(0, this.yOffset + this.ySize * (1.0 - (this.yEnd - num16) / this.yDelta));
      if (index3 != index2)
        return;
      double num17 = xBase == 1.0 || this.xChartVals[index2] == 0.0 ? this.xChartVals[index2] : Math.Log(this.xChartVals[index2], xBase);
      double num18 = yBase == 1.0 || this.yChartVals[index2] == 0.0 ? this.yChartVals[index2] : Math.Log(this.yChartVals[index2], yBase);
      this.xValues.Add(this.xOffset + this.xSize * ((this.xEnd - num17) / this.xDelta));
      this.yValues.Add(this.yOffset + this.ySize * (1.0 - (this.yEnd - num18) / this.yDelta));
    }
  }

  private void TransformToScreenCoInLogHorizontal(double xBase, double yBase)
  {
    ChartSeries series = this.Series as ChartSeries;
    int index1 = 0;
    double num1 = 0.0;
    if (series.IsIndexed)
    {
      this.start = this.start < 0 ? 0 : this.start;
      this.count = this.count > this.yChartVals.Count - 1 ? this.yChartVals.Count - 1 : this.count;
      double num2 = 1.0;
      for (int start = this.start; start <= this.count; ++start)
      {
        double num3 = yBase == 1.0 || this.yChartVals[start] == 0.0 ? this.yChartVals[start] : Math.Log(this.yChartVals[start], yBase);
        double num4 = xBase == 1.0 ? (double) start : Math.Log((double) start, xBase);
        if (Math.Abs(num2 - (double) start) >= 1.0 || Math.Abs(num1 - num3) >= 1.0)
        {
          this.xValues.Add(this.xOffset + this.xSize * ((num4 - this.xStart) / this.xDelta));
          this.yValues.Add(this.yOffset + this.ySize * (1.0 - (num3 - this.yStart) / this.yDelta));
          num2 = (double) start;
          num1 = num3;
        }
      }
      if (this.start > 0 && this.start < this.yChartVals.Count)
      {
        int num5 = this.start - 1;
        double num6 = yBase == 1.0 || this.yChartVals[num5] == 0.0 ? this.yChartVals[num5] : Math.Log(this.yChartVals[num5], yBase);
        this.xValues.Insert(0, this.xOffset + this.xSize * (((xBase == 1.0 ? (double) num5 : Math.Log((double) num5, xBase)) - this.xStart) / this.xDelta));
        this.yValues.Insert(0, this.yOffset + this.ySize * (1.0 - (num6 - this.yStart) / this.yDelta));
      }
      if (this.count >= this.yChartVals.Count - 1)
        return;
      int num7 = this.count + 1;
      double num8 = yBase == 1.0 || this.yChartVals[num7] == 0.0 ? this.yChartVals[num7] : Math.Log(this.yChartVals[num7], yBase);
      this.xValues.Add(this.xOffset + this.xSize * (((xBase == 1.0 ? (double) num7 : Math.Log((double) num7, xBase)) - this.xStart) / this.xDelta));
      this.yValues.Add(this.yOffset + this.ySize * (1.0 - (num8 - this.yStart) / this.yDelta));
    }
    else
    {
      double num9 = xBase == 1.0 || this.xChartVals[0] == 0.0 ? this.xChartVals[0] : Math.Log(this.xChartVals[0], xBase);
      double num10 = yBase == 1.0 || this.yChartVals[0] == 0.0 ? this.yChartVals[0] : Math.Log(this.yChartVals[0], yBase);
      int index2 = this.xChartVals.Count - 1;
      int index3;
      if (series.isLinearData)
      {
        for (index3 = 1; index3 < index2; ++index3)
        {
          double num11 = xBase == 1.0 || this.xChartVals[index3] == 0.0 ? this.xChartVals[index3] : Math.Log(this.xChartVals[index3], xBase);
          double num12 = yBase == 1.0 || this.yChartVals[index3] == 0.0 ? this.yChartVals[index3] : Math.Log(this.yChartVals[index3], yBase);
          if (num11 <= (double) this.count && num11 >= (double) this.start)
          {
            if (Math.Abs(num9 - num11) >= this.xTolerance || Math.Abs(num10 - num12) >= this.yTolerance)
            {
              this.xValues.Add(this.xOffset + this.xSize * ((num11 - this.xStart) / this.xDelta));
              this.yValues.Add(this.yOffset + this.ySize * (1.0 - (num12 - this.yStart) / this.yDelta));
              num9 = num11;
              num10 = num12;
            }
          }
          else if (num11 < (double) this.start)
          {
            if (this.x_isInversed)
            {
              this.xValues.Add(this.xOffset + this.xSize * ((num11 - this.xStart) / this.xDelta));
              this.yValues.Add(this.yOffset + this.ySize * (1.0 - (num12 - this.yStart) / this.yDelta));
            }
            else
              index1 = index3;
          }
          else if (num11 > (double) this.count)
          {
            this.xValues.Add(this.xOffset + this.xSize * ((num11 - this.xStart) / this.xDelta));
            this.yValues.Add(this.yOffset + this.ySize * (1.0 - (num12 - this.yStart) / this.yDelta));
            break;
          }
        }
      }
      else
      {
        for (index3 = 1; index3 < index2; ++index3)
        {
          double num13 = xBase == 1.0 || this.xChartVals[index3] == 0.0 ? this.xChartVals[index3] : Math.Log(this.xChartVals[index3], xBase);
          double num14 = yBase == 1.0 || this.yChartVals[index3] == 0.0 ? this.yChartVals[index3] : Math.Log(this.yChartVals[index3], yBase);
          if (Math.Abs(num9 - num13) >= this.xTolerance || Math.Abs(num10 - num14) >= this.yTolerance)
          {
            this.xValues.Add(this.xOffset + this.xSize * ((num13 - this.xStart) / this.xDelta));
            this.yValues.Add(this.yOffset + this.ySize * (1.0 - (num14 - this.yStart) / this.yDelta));
            num9 = num13;
            num10 = num14;
          }
        }
      }
      double num15 = xBase == 1.0 || this.xChartVals[index1] == 0.0 ? this.xChartVals[index1] : Math.Log(this.xChartVals[index1], xBase);
      double num16 = yBase == 1.0 || this.yChartVals[index1] == 0.0 ? this.yChartVals[index1] : Math.Log(this.yChartVals[index1], yBase);
      this.xValues.Insert(0, this.xOffset + this.xSize * ((num15 - this.xStart) / this.xDelta));
      this.yValues.Insert(0, this.yOffset + this.ySize * (1.0 - (num16 - this.yStart) / this.yDelta));
      if (index3 != index2)
        return;
      double num17 = xBase == 1.0 || this.xChartVals[index2] == 0.0 ? this.xChartVals[index2] : Math.Log(this.xChartVals[index2], xBase);
      double num18 = yBase == 1.0 || this.yChartVals[index2] == 0.0 ? this.yChartVals[index2] : Math.Log(this.yChartVals[index2], yBase);
      this.xValues.Add(this.xOffset + this.xSize * ((num17 - this.xStart) / this.xDelta));
      this.yValues.Add(this.yOffset + this.ySize * (1.0 - (num18 - this.yStart) / this.yDelta));
    }
  }

  private void DrawLine(
    List<double> xVals,
    List<double> yVals,
    int width,
    int height,
    Color color,
    double leftThickness,
    double rightThickness,
    bool isMultiColor)
  {
    ChartSeries series = this.Series as ChartSeries;
    this.xStart = xVals[0];
    this.yStart = yVals[0];
    this.xEnd = 0.0;
    this.yEnd = 0.0;
    if (series.StrokeThickness <= 1.0)
    {
      for (int index = 1; index < xVals.Count; ++index)
      {
        this.xEnd = xVals[index];
        this.yEnd = yVals[index];
        if (this.CheckEmptyPoint())
          this.bitmap.DrawLineBresenham((int) this.xStart, (int) this.yStart, (int) this.xEnd, (int) this.yEnd, color, series.bitmapPixels);
        this.xStart = this.xEnd;
        this.yStart = this.yEnd;
        if (this.isSeriesSelected)
          color = this.seriesSelectionColor;
        else if (isMultiColor)
          color = ChartSegment.GetColor(series.GetInteriorColor(index - 1));
      }
    }
    else
    {
      if (this.points1 == null)
        this.points1 = new int[10];
      this.xEnd = xVals[1];
      this.yEnd = yVals[1];
      FastLineBitmapSegment.GetLinePoints(this.xStart, this.yStart, this.xEnd, this.yEnd, leftThickness, rightThickness, this.points1);
      int index = 1;
      while (index < xVals.Count)
      {
        this.points2 = new int[10];
        this.xStart = this.xEnd;
        this.yStart = this.yEnd;
        ++index;
        if (index < xVals.Count)
        {
          this.xEnd = xVals[index];
          this.yEnd = yVals[index];
          this.UpdatePoints2(this.xStart, this.yStart, this.xEnd, this.yEnd, leftThickness, rightThickness);
        }
        this.DrawLine(color, width, height);
        this.points1 = this.points2;
        this.points2 = (int[]) null;
        if (this.isSeriesSelected)
          color = this.seriesSelectionColor;
        else if (isMultiColor)
          color = ChartSegment.GetColor(series.GetInteriorColor(index - 1));
      }
      this.points1 = (int[]) null;
      this.points2 = (int[]) null;
    }
  }

  private bool FindIntersectingPoints(
    double x11,
    double y11,
    double x12,
    double y12,
    double x21,
    double y21,
    double x22,
    double y22)
  {
    double pixelHeight = (double) this.bitmap.PixelHeight;
    if (y11 <= -pixelHeight)
      y11 = -2.0 * pixelHeight;
    if (y12 <= -pixelHeight)
      y12 = -2.0 * pixelHeight;
    if (y21 <= -pixelHeight)
      y21 = -2.0 * pixelHeight;
    if (y22 <= -pixelHeight)
      y22 = -2.0 * pixelHeight;
    if (y11 >= 2.0 * pixelHeight)
      y11 = 4.0 * pixelHeight;
    if (y12 >= 2.0 * pixelHeight)
      y12 = 4.0 * pixelHeight;
    if (y21 >= 2.0 * pixelHeight)
      y21 = 4.0 * pixelHeight;
    if (y22 >= 2.0 * pixelHeight)
      y22 = 4.0 * pixelHeight;
    this.intersectingPoint = new Point();
    double num1 = WriteableBitmapExtensions.Slope(x11, y11, x12, y12);
    double num2 = double.NaN;
    if (!double.IsInfinity(num1))
      num2 = WriteableBitmapExtensions.Intersect(x12, y12, num1);
    double num3 = WriteableBitmapExtensions.Slope(x21, y21, x22, y22);
    double num4 = double.NaN;
    if (!double.IsInfinity(num3))
      num4 = WriteableBitmapExtensions.Intersect(x21, y21, num3);
    double d = (num4 - num2) / (num1 - num3);
    this.intersectingPoint.X = (double) (int) d;
    this.intersectingPoint.Y = (double) (int) (num1 * d) + num2;
    return !double.IsNaN(d) && !double.IsNaN(this.intersectingPoint.Y);
  }

  private void UpdatePoints2(
    double xStart,
    double yStart,
    double xEnd,
    double yEnd,
    double leftThickness,
    double rightThickness)
  {
    FastLineBitmapSegment.GetLinePoints(xStart, yStart, xEnd, yEnd, leftThickness, rightThickness, this.points2);
    bool flag1 = false;
    if (this.points1[0] == this.points1[2])
      flag1 = this.points2[0] != this.points2[2] || this.points1[0] == this.points2[0];
    else if (this.points2[0] == this.points2[2])
      flag1 = true;
    else if (Math.Floor(WriteableBitmapExtensions.Slope((double) this.points1[2], (double) this.points1[1], (double) this.points1[0], (double) this.points1[3])) != Math.Floor(WriteableBitmapExtensions.Slope((double) this.points2[2], (double) this.points2[1], (double) this.points2[0], (double) this.points2[3])))
      flag1 = true;
    if (flag1 && this.FindIntersectingPoints((double) this.points1[0], (double) this.points1[1], (double) this.points1[2], (double) this.points1[3], (double) this.points2[0], (double) this.points2[1], (double) this.points2[2], (double) this.points2[3]))
    {
      int x = (int) this.intersectingPoint.X;
      if (this.points1[0] < x && x < this.points2[2] || this.points1[2] > this.points2[2] || this.points1[0] > this.points2[0])
      {
        this.points1[2] = this.points2[0] = this.points2[8] = x;
        this.points1[3] = this.points2[1] = this.points2[9] = (int) this.intersectingPoint.Y;
      }
    }
    bool flag2 = false;
    if (this.points1[4] == this.points1[6])
      flag2 = this.points2[4] != this.points2[6] || this.points1[4] == this.points2[4];
    else if (this.points2[4] == this.points2[6])
      flag2 = true;
    else if (Math.Floor(WriteableBitmapExtensions.Slope((double) this.points1[6], (double) this.points1[5], (double) this.points1[4], (double) this.points1[7])) != Math.Floor(WriteableBitmapExtensions.Slope((double) this.points2[6], (double) this.points2[5], (double) this.points2[4], (double) this.points2[7])))
      flag2 = true;
    if (!flag2 || !this.FindIntersectingPoints((double) this.points1[4], (double) this.points1[5], (double) this.points1[6], (double) this.points1[7], (double) this.points2[4], (double) this.points2[5], (double) this.points2[6], (double) this.points2[7]))
      return;
    int x1 = (int) this.intersectingPoint.X;
    if ((this.points1[6] >= x1 || x1 >= this.points2[4]) && this.points1[4] <= this.points2[4] && this.points1[6] <= this.points2[6])
      return;
    this.points1[5] = this.points2[7] = (int) this.intersectingPoint.Y;
    this.points1[4] = this.points2[6] = (int) this.intersectingPoint.X;
  }

  private bool CheckEmptyPoint()
  {
    return !this.Series.IsActualTransposed ? !double.IsNaN(this.yStart) && !double.IsNaN(this.yEnd) : !double.IsNaN(this.xStart) && !double.IsNaN(this.xEnd);
  }

  private void DrawLineAa(
    List<double> xVals,
    List<double> yVals,
    int width,
    int height,
    Color color,
    double leftThickness,
    double rightThickness,
    bool isMultiColor)
  {
    ChartSeries series = this.Series as ChartSeries;
    this.xStart = xVals[0];
    this.yStart = yVals[0];
    this.xEnd = 0.0;
    this.yEnd = 0.0;
    if (series.StrokeThickness <= 1.0)
    {
      for (int index = 1; index <= xVals.Count - 1; ++index)
      {
        this.xEnd = xVals[index];
        this.yEnd = yVals[index];
        if (this.CheckEmptyPoint())
          this.bitmap.DrawLineAa((int) this.xStart, (int) this.yStart, (int) this.xEnd, (int) this.yEnd, color, series.bitmapPixels);
        if (this.isSeriesSelected)
          color = this.seriesSelectionColor;
        else if (isMultiColor)
          color = ChartSegment.GetColor(series.GetInteriorColor(index - 1));
        this.xStart = this.xEnd;
        this.yStart = this.yEnd;
      }
    }
    else
    {
      if (this.points1 == null)
        this.points1 = new int[10];
      this.xEnd = xVals[1];
      this.yEnd = yVals[1];
      FastLineBitmapSegment.GetLinePoints(this.xStart, this.yStart, this.xEnd, this.yEnd, leftThickness, rightThickness, this.points1);
      int index = 1;
      while (index < xVals.Count)
      {
        this.points2 = new int[10];
        this.xStart = this.xEnd;
        this.yStart = this.yEnd;
        ++index;
        if (index < xVals.Count)
        {
          this.xEnd = xVals[index];
          this.yEnd = yVals[index];
          this.UpdatePoints2(this.xStart, this.yStart, this.xEnd, this.yEnd, leftThickness, rightThickness);
        }
        this.DrawLineAa(color, width, height);
        this.points1 = this.points2;
        this.points2 = (int[]) null;
        if (this.isSeriesSelected)
          color = this.seriesSelectionColor;
        else if (isMultiColor)
          color = ChartSegment.GetColor(series.GetInteriorColor(index - 1));
      }
      this.points1 = (int[]) null;
      this.points2 = (int[]) null;
    }
  }

  private void DrawDashedAaLines(
    int width,
    int height,
    Color color,
    double leftThickness,
    double rightThickness)
  {
    if (!this.Series.IsActualTransposed)
      this.DrawDashedAaLines(this.xValues, this.yValues, width, height, color, leftThickness, rightThickness);
    else
      this.DrawDashedAaLines(this.yValues, this.xValues, width, height, color, leftThickness, rightThickness);
  }

  private void DrawDashedAaLines(
    List<double> xVals,
    List<double> yVals,
    int w,
    int h,
    Color color,
    double leftThickness,
    double rightThickness)
  {
    ChartSeries series = this.Series as ChartSeries;
    FastLineBitmapSeries lineBitmapSeries = (FastLineBitmapSeries) series;
    double strokeThickness = lineBitmapSeries.StrokeThickness;
    this.xStart = xVals[0];
    this.yStart = yVals[0];
    DoubleCollection strokeDashArray = lineBitmapSeries.StrokeDashArray;
    bool flag1 = true;
    double num1 = strokeDashArray[0] * strokeThickness;
    int index1 = 1;
    bool flag2 = true;
    bool flag3 = false;
    if (this.points1 == null)
      this.points1 = new int[10];
    for (int index2 = 1; index2 < xVals.Count; ++index2)
    {
      double num2 = xVals[index2];
      double num3 = yVals[index2];
      if (this.xStart == 0.0 && num2 == 0.0)
      {
        this.yStart = yVals[index2];
        this.xStart = xVals[index2];
      }
      else if (this.xStart == num2 && this.yStart == num3)
      {
        this.yStart = yVals[index2];
        this.xStart = xVals[index2];
      }
      else if (this.yStart < 0.0 && num3 < 0.0)
      {
        this.yStart = yVals[index2];
        this.xStart = xVals[index2];
      }
      else if (this.yStart > (double) h && num3 > (double) h)
      {
        this.yStart = yVals[index2];
        this.xStart = xVals[index2];
      }
      else if (this.xStart < 0.0 && num2 < 0.0)
      {
        this.yStart = yVals[index2];
        this.xStart = xVals[index2];
      }
      else if (this.xStart > (double) w && num2 > (double) w)
      {
        this.yStart = yVals[index2];
        this.xStart = xVals[index2];
      }
      else
      {
        double yStart = this.yStart;
        double num4 = num3;
        double xStart = this.xStart;
        double num5 = num2;
        if (num3 < -0.5)
        {
          double num6 = WriteableBitmapExtensions.Slope(xStart, yStart, num5, num4);
          if (!double.IsInfinity(num6) && !double.IsNaN(num6))
            num2 = (double) (int) (-WriteableBitmapExtensions.Intersect(num5, num4, num6) / num6);
          num3 = 0.0;
        }
        if (this.yStart < -0.5)
        {
          double num7 = WriteableBitmapExtensions.Slope(xStart, yStart, num5, num4);
          if (!double.IsInfinity(num7) && !double.IsNaN(num7))
            this.xStart = (double) (int) (-WriteableBitmapExtensions.Intersect(xStart, yStart, num7) / num7);
          this.yStart = 0.0;
        }
        if (this.xStart < -0.5)
        {
          double num8 = WriteableBitmapExtensions.Slope(xStart, yStart, num5, num4);
          if (!double.IsInfinity(num8) && !double.IsNaN(num8))
            this.yStart = (double) (int) WriteableBitmapExtensions.Intersect(xStart, yStart, num8);
          this.xStart = 0.0;
        }
        if (num2 < -0.5)
        {
          double num9 = WriteableBitmapExtensions.Slope(xStart, yStart, num5, num4);
          if (!double.IsInfinity(num9) && !double.IsNaN(num9))
            num3 = (double) (int) WriteableBitmapExtensions.Intersect(num5, num4, num9);
          num2 = 0.0;
        }
        if (this.xStart > (double) w + 0.5)
        {
          double num10 = WriteableBitmapExtensions.Slope(xStart, yStart, num5, num4);
          if (!double.IsInfinity(num10) && !double.IsNaN(num10))
          {
            double num11 = WriteableBitmapExtensions.Intersect(xStart, yStart, num10);
            this.yStart = (double) (int) (num10 * (double) w + num11);
          }
          this.xStart = (double) w;
        }
        if (num2 > (double) w + 0.5)
        {
          double num12 = WriteableBitmapExtensions.Slope(xStart, yStart, num5, num4);
          if (!double.IsInfinity(num12) && !double.IsNaN(num12))
          {
            double num13 = WriteableBitmapExtensions.Intersect(num5, num4, num12);
            num3 = (double) (int) (num12 * (double) w + num13);
          }
          num2 = (double) w;
        }
        if (this.yStart > (double) h + 0.5)
        {
          double num14 = WriteableBitmapExtensions.Slope(xStart, yStart, num5, num4);
          if (!double.IsInfinity(num14) && !double.IsNaN(num14))
          {
            double num15 = WriteableBitmapExtensions.Intersect(xStart, yStart, num14);
            this.xStart = (double) (int) (((double) h - num15) / num14);
          }
          this.yStart = (double) h;
        }
        if (num3 > (double) h + 0.5)
        {
          double num16 = WriteableBitmapExtensions.Slope(xStart, yStart, num5, num4);
          if (!double.IsInfinity(num16) && !double.IsNaN(num16))
          {
            double num17 = WriteableBitmapExtensions.Intersect(num5, num4, num16);
            num2 = (double) (int) (((double) h - num17) / num16);
          }
          num3 = (double) h;
        }
        double num18 = this.xStart;
        double num19 = this.yStart;
        double num20 = num2;
        double num21 = num3;
        double d = FastLineBitmapSegment.CalcLenOfLine(num18, num20, num19, num21);
        if (!double.IsNaN(d))
        {
          if (this.isSeriesSelected)
            color = this.seriesSelectionColor;
          else if (series.Palette != ChartColorPalette.None)
            color = ChartSegment.GetColor(series.GetInteriorColor(index2 - 1));
          bool flag4 = false;
          while (!flag4)
          {
            this.points2 = new int[10];
            bool flag5;
            if (d < num1)
            {
              num1 -= d;
              flag5 = true;
              flag4 = true;
            }
            else
            {
              if (d != num1)
              {
                double num22 = num1 / d;
                num20 = num18 + num22 * (num20 - num18);
                num21 = num19 + num22 * (num21 - num19);
              }
              d -= num1;
              num1 = strokeDashArray[index1] * strokeThickness;
              index1 = index1 + 1 == strokeDashArray.Count<double>() ? 0 : index1 + 1;
              flag4 = d == 0.0;
              flag5 = false;
            }
            if (flag2)
              FastLineBitmapSegment.GetLinePoints(num18, num19, num20, num21, leftThickness, rightThickness, this.points1);
            else
              this.UpdatePoints2(num18, num19, num20, num21, leftThickness, rightThickness);
            if (flag3)
              this.DrawLineAa(color, w, h);
            if (!flag2)
            {
              this.points1 = this.points2;
              this.points2 = (int[]) null;
            }
            flag3 = flag1;
            flag2 = false;
            flag1 = flag5 ? flag1 : !flag1;
            num18 = num20;
            num19 = num21;
            num20 = num2;
            num21 = num3;
          }
        }
        else
          this.points1 = new int[10];
        this.xStart = xVals[index2];
        this.yStart = yVals[index2];
      }
    }
    if (flag3)
      this.DrawLineAa(color, w, h);
    this.points1 = (int[]) null;
    this.points2 = (int[]) null;
  }

  private void DrawDashedLines(
    int width,
    int height,
    Color color,
    double leftThickness,
    double rightThickness)
  {
    if (!this.Series.IsActualTransposed)
      this.DrawDashedLines(this.xValues, this.yValues, width, height, color, leftThickness, rightThickness);
    else
      this.DrawDashedLines(this.yValues, this.xValues, width, height, color, leftThickness, rightThickness);
  }

  private void DrawDashedLines(
    List<double> xVals,
    List<double> yVals,
    int w,
    int h,
    Color color,
    double leftThickness,
    double rightThickness)
  {
    ChartSeries series = this.Series as ChartSeries;
    FastLineBitmapSeries lineBitmapSeries = (FastLineBitmapSeries) series;
    double strokeThickness = lineBitmapSeries.StrokeThickness;
    this.xStart = xVals[0];
    this.yStart = yVals[0];
    DoubleCollection strokeDashArray = lineBitmapSeries.StrokeDashArray;
    bool flag1 = true;
    double num1 = strokeDashArray[0] * strokeThickness;
    int index1 = 1;
    bool flag2 = true;
    bool flag3 = false;
    if (this.points1 == null)
      this.points1 = new int[10];
    for (int index2 = 1; index2 < xVals.Count; ++index2)
    {
      double num2 = xVals[index2];
      double num3 = yVals[index2];
      if (this.xStart == 0.0 && num2 == 0.0)
      {
        this.yStart = yVals[index2];
        this.xStart = xVals[index2];
      }
      else if (this.xStart == num2 && this.yStart == num3)
      {
        this.yStart = yVals[index2];
        this.xStart = xVals[index2];
      }
      else if (this.yStart < 0.0 && num3 < 0.0)
      {
        this.yStart = yVals[index2];
        this.xStart = xVals[index2];
      }
      else if (this.yStart > (double) h && num3 > (double) h)
      {
        this.yStart = yVals[index2];
        this.xStart = xVals[index2];
      }
      else if (this.xStart < 0.0 && num2 < 0.0)
      {
        this.yStart = yVals[index2];
        this.xStart = xVals[index2];
      }
      else if (this.xStart > (double) w && num2 > (double) w)
      {
        this.yStart = yVals[index2];
        this.xStart = xVals[index2];
      }
      else
      {
        double yStart = this.yStart;
        double num4 = num3;
        double xStart = this.xStart;
        double num5 = num2;
        if (num3 < -0.5)
        {
          double num6 = WriteableBitmapExtensions.Slope(xStart, yStart, num5, num4);
          if (!double.IsInfinity(num6) && !double.IsNaN(num6))
            num2 = (double) (int) (-WriteableBitmapExtensions.Intersect(num5, num4, num6) / num6);
          num3 = 0.0;
        }
        if (this.yStart < -0.5)
        {
          double num7 = WriteableBitmapExtensions.Slope(xStart, yStart, num5, num4);
          if (!double.IsInfinity(num7) && !double.IsNaN(num7))
            this.xStart = (double) (int) (-WriteableBitmapExtensions.Intersect(xStart, yStart, num7) / num7);
          this.yStart = 0.0;
        }
        if (this.xStart < -0.5)
        {
          double num8 = WriteableBitmapExtensions.Slope(xStart, yStart, num5, num4);
          if (!double.IsInfinity(num8) && !double.IsNaN(num8))
            this.yStart = (double) (int) WriteableBitmapExtensions.Intersect(xStart, yStart, num8);
          this.xStart = 0.0;
        }
        if (num2 < -0.5)
        {
          double num9 = WriteableBitmapExtensions.Slope(xStart, yStart, num5, num4);
          if (!double.IsInfinity(num9) && !double.IsNaN(num9))
            num3 = (double) (int) WriteableBitmapExtensions.Intersect(num5, num4, num9);
          num2 = 0.0;
        }
        if (this.xStart > (double) w + 0.5)
        {
          double num10 = WriteableBitmapExtensions.Slope(xStart, yStart, num5, num4);
          if (!double.IsInfinity(num10) && !double.IsNaN(num10))
          {
            double num11 = WriteableBitmapExtensions.Intersect(xStart, yStart, num10);
            this.yStart = (double) (int) (num10 * (double) w + num11);
          }
          this.xStart = (double) w;
        }
        if (num2 > (double) w + 0.5)
        {
          double num12 = WriteableBitmapExtensions.Slope(xStart, yStart, num5, num4);
          if (!double.IsInfinity(num12) && !double.IsNaN(num12))
          {
            double num13 = WriteableBitmapExtensions.Intersect(num5, num4, num12);
            num3 = (double) (int) (num12 * (double) w + num13);
          }
          num2 = (double) w;
        }
        if (this.yStart > (double) h + 0.5)
        {
          double num14 = WriteableBitmapExtensions.Slope(xStart, yStart, num5, num4);
          if (!double.IsInfinity(num14) && !double.IsNaN(num14))
          {
            double num15 = WriteableBitmapExtensions.Intersect(xStart, yStart, num14);
            this.xStart = (double) (int) (((double) h - num15) / num14);
          }
          this.yStart = (double) h;
        }
        if (num3 > (double) h + 0.5)
        {
          double num16 = WriteableBitmapExtensions.Slope(xStart, yStart, num5, num4);
          if (!double.IsInfinity(num16) && !double.IsNaN(num16))
          {
            double num17 = WriteableBitmapExtensions.Intersect(num5, num4, num16);
            num2 = (double) (int) (((double) h - num17) / num16);
          }
          num3 = (double) h;
        }
        double num18 = this.xStart;
        double num19 = this.yStart;
        double num20 = num2;
        double num21 = num3;
        double d = FastLineBitmapSegment.CalcLenOfLine(num18, num20, num19, num21);
        if (!double.IsNaN(d))
        {
          if (this.isSeriesSelected)
            color = this.seriesSelectionColor;
          else if (series.Palette != ChartColorPalette.None)
            color = ChartSegment.GetColor(series.GetInteriorColor(index2 - 1));
          bool flag4 = false;
          while (!flag4)
          {
            this.points2 = new int[10];
            bool flag5;
            if (d < num1)
            {
              num1 -= d;
              flag5 = true;
              flag4 = true;
            }
            else
            {
              if (d != num1)
              {
                double num22 = num1 / d;
                num20 = num18 + num22 * (num20 - num18);
                num21 = num19 + num22 * (num21 - num19);
              }
              d -= num1;
              num1 = strokeDashArray[index1] * strokeThickness;
              index1 = index1 + 1 == strokeDashArray.Count<double>() ? 0 : index1 + 1;
              flag4 = d == 0.0;
              flag5 = false;
            }
            if (flag2)
              FastLineBitmapSegment.GetLinePoints(num18, num19, num20, num21, leftThickness, rightThickness, this.points1);
            else
              this.UpdatePoints2(num18, num19, num20, num21, leftThickness, rightThickness);
            if (flag3)
              this.DrawLine(color, w, h);
            if (!flag2)
            {
              this.points1 = this.points2;
              this.points2 = (int[]) null;
            }
            flag3 = flag1;
            flag2 = false;
            flag1 = flag5 ? flag1 : !flag1;
            num18 = num20;
            num19 = num21;
            num20 = num2;
            num21 = num3;
          }
        }
        else
          this.points1 = new int[10];
        this.xStart = xVals[index2];
        this.yStart = yVals[index2];
      }
    }
    if (flag3)
      this.DrawLine(color, w, h);
    this.points1 = (int[]) null;
    this.points2 = (int[]) null;
  }

  private void DrawLine(Color color, int width, int height)
  {
    ChartSeries series = this.Series as ChartSeries;
    if (series.Area.IsMultipleArea && series.Clip != null)
    {
      Rect bounds = series.Clip.Bounds;
      this.bitmap.DrawLineBresenham(this.points1[0], this.points1[1], this.points1[2], this.points1[3], color, series.bitmapPixels, bounds);
      this.bitmap.FillPolygon(this.points1, color, series.bitmapPixels, bounds);
      this.bitmap.DrawLineBresenham(this.points1[4], this.points1[5], this.points1[6], this.points1[7], color, series.bitmapPixels);
    }
    else
    {
      this.bitmap.DrawLineBresenham(this.points1[0], this.points1[1], this.points1[2], this.points1[3], color, series.bitmapPixels);
      this.bitmap.FillPolygon(this.points1, color, series.bitmapPixels);
      this.bitmap.DrawLineBresenham(this.points1[4], this.points1[5], this.points1[6], this.points1[7], color, series.bitmapPixels);
    }
  }

  private void DrawLineAa(Color color, int width, int height)
  {
    ChartSeries series = this.Series as ChartSeries;
    if (series.Area.IsMultipleArea && series.Clip != null)
    {
      Rect bounds = series.Clip.Bounds;
      this.bitmap.DrawLineAa(this.points1[0], this.points1[1], this.points1[2], this.points1[3], color, series.bitmapPixels, bounds);
      this.bitmap.FillPolygon(this.points1, color, series.bitmapPixels, bounds);
      this.bitmap.DrawLineAa(this.points1[4], this.points1[5], this.points1[6], this.points1[7], color, series.bitmapPixels, bounds);
    }
    else
    {
      this.bitmap.DrawLineAa(this.points1[0], this.points1[1], this.points1[2], this.points1[3], color, series.bitmapPixels);
      this.bitmap.FillPolygon(this.points1, color, series.bitmapPixels);
      this.bitmap.DrawLineAa(this.points1[4], this.points1[5], this.points1[6], this.points1[7], color, series.bitmapPixels);
    }
  }
}
