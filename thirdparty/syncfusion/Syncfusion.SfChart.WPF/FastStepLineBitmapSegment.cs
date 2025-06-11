// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.FastStepLineBitmapSegment
// Assembly: Syncfusion.SfChart.WPF, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 279D70C6-7936-4F3E-B6CF-79EFEF734826
// Assembly location: C:\Program Files\PDFgear\Syncfusion.SfChart.WPF.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

#nullable disable
namespace Syncfusion.UI.Xaml.Charts;

public class FastStepLineBitmapSegment : ChartSegment
{
  private IList<double> xChartVals;
  private IList<double> yChartVals;
  private WriteableBitmap bitmap;
  private List<double> xValues;
  private List<double> yValues;
  private int startIndex;
  private Color seriesSelectionColor = Colors.Transparent;
  private bool isSeriesSelected;

  public FastStepLineBitmapSegment()
  {
    this.xValues = new List<double>();
    this.yValues = new List<double>();
  }

  public FastStepLineBitmapSegment(ChartSeriesBase series)
  {
    this.Series = (ChartSeriesBase) (series as ChartSeries);
  }

  public FastStepLineBitmapSegment(
    IList<double> xVals,
    IList<double> yVals,
    AdornmentSeries series)
    : this((ChartSeriesBase) series)
  {
    this.xValues = new List<double>();
    this.yValues = new List<double>();
    this.Series = (ChartSeriesBase) series;
    this.xChartVals = xVals;
    this.yChartVals = yVals;
    this.Item = (object) series.ActualData;
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
  }

  public override UIElement GetRenderedVisual() => (UIElement) null;

  public override void Update(IChartTransformer transformer)
  {
    this.bitmap = (this.Series as ChartSeries).Area.GetFastRenderSurface();
    if (transformer == null || this.Series.DataCount <= 0)
      return;
    ChartTransform.ChartCartesianTransformer cartesianTransformer = transformer as ChartTransform.ChartCartesianTransformer;
    this.xValues.Clear();
    this.yValues.Clear();
    this.CalculatePoints(cartesianTransformer);
    this.UpdateVisual();
  }

  internal void SetRange()
  {
    ChartSeries series = this.Series as ChartSeries;
    bool flag = !(series.ActualXAxis is CategoryAxis) || (series.ActualXAxis as CategoryAxis).IsIndexed;
    if (series.DataCount <= 0)
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
    if (series.IsIndexed)
    {
      double end1 = !flag ? this.xChartVals.Max() : (double) (series.DataCount - 1);
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

  internal void UpdateVisual()
  {
    ChartSeries series = this.Series as ChartSeries;
    bool isMultiColor = this.Series.Palette != ChartColorPalette.None && series.Interior == null;
    Color color = ChartSegment.GetColor(this.Interior);
    if (series.StrokeThickness <= 0.0)
    {
      this.bitmap.BeginWrite();
      this.bitmap.EndWrite();
    }
    if (this.bitmap != null && this.xValues.Count > 0 && series.StrokeThickness > 0.0)
    {
      double xValue = this.xValues[0];
      double yValue = this.yValues[0];
      int width = (int) series.Area.SeriesClipRect.Width;
      int height = (int) series.Area.SeriesClipRect.Height;
      int leftThickness = (int) series.StrokeThickness / 2;
      int rightThickness = series.StrokeThickness % 2.0 == 0.0 ? (int) (series.StrokeThickness / 2.0 - 1.0) : (int) (series.StrokeThickness / 2.0);
      this.bitmap.BeginWrite();
      if (series is FastStepLineBitmapSeries)
      {
        SfChart area = series.Area;
        this.isSeriesSelected = false;
        if (area.GetEnableSeriesSelection())
        {
          Brush seriesSelectionBrush = area.GetSeriesSelectionBrush((ChartSeriesBase) series);
          if (seriesSelectionBrush != null && area.SelectedSeriesCollection.Contains((ChartSeriesBase) series))
          {
            this.isSeriesSelected = true;
            this.seriesSelectionColor = ((SolidColorBrush) seriesSelectionBrush).Color;
          }
        }
        if (!series.IsActualTransposed)
          this.UpdateVisualHorizontal(xValue, yValue, width, height, color, isMultiColor, leftThickness, rightThickness);
        else
          this.UpdateVisualVertical(xValue, yValue, width, height, color, isMultiColor, leftThickness, rightThickness);
      }
      this.bitmap.EndWrite();
    }
    series.Area.CanRenderToBuffer = true;
    this.xValues.Clear();
    this.yValues.Clear();
  }

  protected override void SetVisualBindings(Shape element)
  {
  }

  private void AddDataPoint(
    ChartTransform.ChartCartesianTransformer cartesianTransformer,
    int index)
  {
    double xValue;
    double yValue;
    this.GetXYValue(index, out xValue, out yValue);
    Point visible = cartesianTransformer.TransformToVisible(xValue, yValue);
    if (!this.Series.IsActualTransposed)
    {
      this.xValues.Add(visible.X);
      this.yValues.Add(visible.Y);
    }
    else
    {
      this.xValues.Add(visible.Y);
      this.yValues.Add(visible.X);
    }
  }

  private void InsertDataPoint(
    ChartTransform.ChartCartesianTransformer cartesianTransformer,
    int index)
  {
    double xValue;
    double yValue;
    this.GetXYValue(index, out xValue, out yValue);
    Point visible = cartesianTransformer.TransformToVisible(xValue, yValue);
    if (!this.Series.IsActualTransposed)
    {
      this.xValues.Insert(0, visible.X);
      this.yValues.Insert(0, visible.Y);
    }
    else
    {
      this.xValues.Insert(0, visible.Y);
      this.yValues.Insert(0, visible.X);
    }
  }

  private void GetXYValue(int index, out double xValue, out double yValue)
  {
    xValue = this.xChartVals[index];
    yValue = this.yChartVals[index];
  }

  private void CalculatePoints(
    ChartTransform.ChartCartesianTransformer cartesianTransformer)
  {
    ChartSeries series = this.Series as ChartSeries;
    ChartAxis xaxis = cartesianTransformer.XAxis;
    int num1 = this.xChartVals.Count - 1;
    if (series.IsIndexed)
    {
      int num2;
      int num3;
      if (!(series.ActualXAxis is CategoryAxis) || !(series.ActualXAxis as CategoryAxis).IsIndexed)
      {
        num2 = 0;
        num3 = this.xChartVals.Count - 1;
      }
      else
      {
        num2 = (int) Math.Floor(xaxis.VisibleRange.Start);
        int num4 = (int) Math.Ceiling(xaxis.VisibleRange.End);
        num3 = num4 > this.yChartVals.Count - 1 ? this.yChartVals.Count - 1 : num4;
      }
      int num5 = num2 < 0 ? 0 : num2;
      this.startIndex = num5;
      for (int index = num5; index <= num3; ++index)
        this.AddDataPoint(cartesianTransformer, index);
    }
    else if (series.isLinearData)
    {
      double num6 = Math.Floor(xaxis.VisibleRange.Start);
      double num7 = Math.Ceiling(xaxis.VisibleRange.End);
      this.startIndex = 0;
      int index1 = this.xChartVals.Count - 1;
      double newBase = cartesianTransformer.XAxis.IsLogarithmic ? (cartesianTransformer.XAxis as LogarithmicAxis).LogarithmicBase : 0.0;
      int index2;
      for (index2 = 1; index2 < index1; ++index2)
      {
        double a = this.xChartVals[index2];
        if (cartesianTransformer.XAxis.IsLogarithmic)
          a = Math.Log(a, newBase);
        if (a >= num6 && a <= num7)
          this.AddDataPoint(cartesianTransformer, index2);
        else if (a < num6)
          this.startIndex = index2;
        else if (a > num7)
        {
          this.AddDataPoint(cartesianTransformer, index2);
          break;
        }
      }
      this.InsertDataPoint(cartesianTransformer, this.startIndex);
      if (index2 != index1)
        return;
      this.AddDataPoint(cartesianTransformer, index1);
    }
    else
    {
      this.startIndex = 0;
      for (int index = 0; index <= num1; ++index)
        this.AddDataPoint(cartesianTransformer, index);
    }
  }

  private void UpdateVisualVertical(
    double xStart,
    double yStart,
    int width,
    int height,
    Color color,
    bool isMultiColor,
    int leftThickness,
    int rightThickness)
  {
    ChartSeries series = this.Series as ChartSeries;
    if (((FastStepLineBitmapSeries) series).EnableAntiAliasing)
    {
      for (int index = 1; index < this.xValues.Count; ++index)
      {
        if (this.isSeriesSelected)
          color = this.seriesSelectionColor;
        else if (this.Series.SegmentColorPath != null && series.Interior == null)
          color = this.Series.ColorValues.Count <= 0 || this.Series.ColorValues[this.startIndex] == null ? (this.Series.Palette != ChartColorPalette.None ? ChartSegment.GetColor(this.Series.ColorModel.GetBrush(this.startIndex)) : ChartSegment.GetColor(this.Series.ActualArea.ColorModel.GetBrush(this.Series.ActualArea.GetSeriesIndex(this.Series)))) : ChartSegment.GetColor(this.Series.ColorValues[this.startIndex]);
        else if (isMultiColor)
          color = ChartSegment.GetColor(this.Series.ColorModel.GetBrush(this.startIndex));
        double xValue = this.xValues[index];
        double yValue = this.yValues[index];
        ++this.startIndex;
        if (!double.IsNaN(yStart) && !double.IsNaN(yValue))
        {
          double num1 = xStart - (double) leftThickness;
          double num2 = xStart + (double) rightThickness;
          if (yValue < yStart)
            this.bitmap.FillRectangle((int) yValue, (int) num1, (int) yStart, (int) num2, color, series.bitmapPixels);
          else
            this.bitmap.FillRectangle((int) yStart, (int) num1, (int) yValue, (int) num2, color, series.bitmapPixels);
          this.bitmap.DrawLineAa((int) yStart, (int) num1, (int) yValue, (int) num1, color, series.bitmapPixels);
          this.bitmap.DrawLineAa((int) yStart, (int) num1, (int) yStart, (int) num2, color, series.bitmapPixels);
          this.bitmap.DrawLineAa((int) yStart, (int) num2, (int) yValue, (int) num2, color, series.bitmapPixels);
          this.bitmap.DrawLineAa((int) yValue, (int) num1, (int) yValue, (int) num2, color, series.bitmapPixels);
          double num3 = yValue - (double) leftThickness;
          double num4 = yValue + (double) rightThickness;
          this.bitmap.FillRectangle((int) num3, (int) xValue, (int) num4, (int) xStart, color, series.bitmapPixels);
          this.bitmap.DrawLineAa((int) num3, (int) xValue, (int) num4, (int) xValue, color, series.bitmapPixels);
          this.bitmap.DrawLineAa((int) num3, (int) xValue, (int) num3, (int) xStart, color, series.bitmapPixels);
          this.bitmap.DrawLineAa((int) num3, (int) xStart, (int) num4, (int) xStart, color, series.bitmapPixels);
          this.bitmap.DrawLineAa((int) num4, (int) xValue, (int) num4, (int) xStart, color, series.bitmapPixels);
        }
        xStart = xValue;
        yStart = yValue;
      }
    }
    else
    {
      for (int index = 1; index < this.xValues.Count; ++index)
      {
        if (this.isSeriesSelected)
          color = this.seriesSelectionColor;
        else if (this.Series.SegmentColorPath != null && series.Interior == null)
          color = this.Series.ColorValues.Count <= 0 || this.Series.ColorValues[this.startIndex] == null ? (this.Series.Palette != ChartColorPalette.None ? ChartSegment.GetColor(this.Series.ColorModel.GetBrush(this.startIndex)) : ChartSegment.GetColor(this.Series.ActualArea.ColorModel.GetBrush(this.Series.ActualArea.GetSeriesIndex(this.Series)))) : ChartSegment.GetColor(this.Series.ColorValues[this.startIndex]);
        else if (isMultiColor)
          color = ChartSegment.GetColor(this.Series.ColorModel.GetBrush(this.startIndex));
        double xValue = this.xValues[index];
        double yValue = this.yValues[index];
        ++this.startIndex;
        if (!double.IsNaN(yStart) && !double.IsNaN(yValue))
        {
          double y1 = xStart - (double) leftThickness;
          double y2 = xStart + (double) rightThickness;
          if (yValue < yStart)
            this.bitmap.FillRectangle((int) yValue, (int) y1, (int) yStart, (int) y2, color, series.bitmapPixels);
          else
            this.bitmap.FillRectangle((int) yStart, (int) y1, (int) yValue, (int) y2, color, series.bitmapPixels);
          double x1 = yValue - (double) leftThickness;
          double x2 = yValue + (double) rightThickness;
          if (xValue < xStart)
            this.bitmap.FillRectangle((int) x1, (int) xValue, (int) x2, (int) xStart, color, series.bitmapPixels);
          else
            this.bitmap.FillRectangle((int) x1, (int) xStart, (int) x2, (int) xValue, color, series.bitmapPixels);
        }
        xStart = xValue;
        yStart = yValue;
      }
    }
  }

  private void UpdateVisualHorizontal(
    double xStart,
    double yStart,
    int width,
    int height,
    Color color,
    bool isMultiColor,
    int leftThickness,
    int rightThickness)
  {
    ChartSeries series = this.Series as ChartSeries;
    if (((FastStepLineBitmapSeries) series).EnableAntiAliasing)
    {
      for (int index = 1; index < this.xValues.Count; ++index)
      {
        if (this.isSeriesSelected)
          color = this.seriesSelectionColor;
        else if (this.Series.SegmentColorPath != null && series.Interior == null)
          color = this.Series.ColorValues.Count <= 0 || this.Series.ColorValues[this.startIndex] == null ? (this.Series.Palette != ChartColorPalette.None ? ChartSegment.GetColor(this.Series.ColorModel.GetBrush(this.startIndex)) : ChartSegment.GetColor(this.Series.ActualArea.ColorModel.GetBrush(this.Series.ActualArea.GetSeriesIndex(this.Series)))) : ChartSegment.GetColor(this.Series.ColorValues[this.startIndex]);
        else if (isMultiColor)
          color = ChartSegment.GetColor(this.Series.ColorModel.GetBrush(this.startIndex));
        ++this.startIndex;
        double xValue = this.xValues[index];
        double yValue = this.yValues[index];
        if (!double.IsNaN(yStart) && !double.IsNaN(yValue))
        {
          double num1 = yStart - (double) leftThickness;
          double num2 = yStart + (double) rightThickness;
          this.bitmap.DrawLineAa((int) xStart, (int) num1, (int) xValue, (int) num1, color, series.bitmapPixels);
          this.bitmap.DrawLineAa((int) xStart, (int) num1, (int) xStart, (int) num2, color, series.bitmapPixels);
          this.bitmap.DrawLineAa((int) xStart, (int) num2, (int) xValue, (int) num2, color, series.bitmapPixels);
          this.bitmap.DrawLineAa((int) xValue, (int) num1, (int) xValue, (int) num2, color, series.bitmapPixels);
          this.bitmap.FillRectangle((int) xStart, (int) num1, (int) xValue, (int) num2, color, series.bitmapPixels);
          double num3 = xValue - (double) leftThickness;
          double num4 = xValue + (double) rightThickness;
          this.bitmap.DrawLineAa((int) num3, (int) yStart, (int) num4, (int) yStart, color, series.bitmapPixels);
          this.bitmap.DrawLineAa((int) num3, (int) yStart, (int) num3, (int) yValue, color, series.bitmapPixels);
          this.bitmap.DrawLineAa((int) num3, (int) yValue, (int) num4, (int) yValue, color, series.bitmapPixels);
          this.bitmap.DrawLineAa((int) num4, (int) yStart, (int) num4, (int) yValue, color, series.bitmapPixels);
          if (yStart < yValue)
            this.bitmap.FillRectangle((int) num3, (int) yStart, (int) num4, (int) yValue, color, series.bitmapPixels);
          else
            this.bitmap.FillRectangle((int) num3, (int) yValue, (int) num4, (int) yStart, color, series.bitmapPixels);
        }
        xStart = xValue;
        yStart = yValue;
      }
    }
    else
    {
      for (int index = 1; index < this.xValues.Count; ++index)
      {
        if (this.isSeriesSelected)
          color = this.seriesSelectionColor;
        else if (this.Series.SegmentColorPath != null && series.Interior == null)
          color = this.Series.ColorValues.Count <= 0 || this.Series.ColorValues[this.startIndex] == null ? (this.Series.Palette != ChartColorPalette.None ? ChartSegment.GetColor(this.Series.ColorModel.GetBrush(this.startIndex)) : ChartSegment.GetColor(this.Series.ActualArea.ColorModel.GetBrush(this.Series.ActualArea.GetSeriesIndex(this.Series)))) : ChartSegment.GetColor(this.Series.ColorValues[this.startIndex]);
        else if (isMultiColor)
          color = ChartSegment.GetColor(this.Series.ColorModel.GetBrush(this.startIndex));
        double xValue = this.xValues[index];
        double yValue = this.yValues[index];
        ++this.startIndex;
        if (!double.IsNaN(yStart) && !double.IsNaN(yValue))
        {
          double y1 = yStart - (double) leftThickness;
          double y2 = yStart + (double) rightThickness;
          if (xStart < xValue)
            this.bitmap.FillRectangle((int) xStart, (int) y1, (int) xValue, (int) y2, color, series.bitmapPixels);
          else
            this.bitmap.FillRectangle((int) xValue, (int) y1, (int) xStart, (int) y2, color, series.bitmapPixels);
          double x1 = xValue - (double) leftThickness;
          double x2 = xValue + (double) rightThickness;
          if (yStart < yValue)
            this.bitmap.FillRectangle((int) x1, (int) yStart, (int) x2, (int) yValue, color, series.bitmapPixels);
          else
            this.bitmap.FillRectangle((int) x1, (int) yValue, (int) x2, (int) yStart, color, series.bitmapPixels);
        }
        xStart = xValue;
        yStart = yValue;
      }
    }
  }
}
