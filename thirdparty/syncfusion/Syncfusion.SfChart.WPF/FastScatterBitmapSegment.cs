// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.FastScatterBitmapSegment
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

public class FastScatterBitmapSegment : ChartSegment
{
  private List<double> xValues;
  private List<double> yValues;
  private IList<double> xChartVals;
  private IList<double> yChartVals;
  private int startIndex;
  private WriteableBitmap bitmap;
  private Color seriesSelectionColor = Colors.Transparent;
  private Color segmentSelectionColor = Colors.Transparent;
  private bool isSeriesSelected;

  public FastScatterBitmapSegment()
  {
    this.xValues = new List<double>();
    this.yValues = new List<double>();
  }

  public FastScatterBitmapSegment(
    IList<double> xVals,
    IList<double> yVals,
    FastScatterBitmapSeries series)
  {
    this.xValues = new List<double>();
    this.yValues = new List<double>();
    this.Series = (ChartSeriesBase) series;
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

  public override UIElement CreateVisual(Size size) => (UIElement) null;

  public override void OnSizeChanged(Size size)
  {
  }

  public override UIElement GetRenderedVisual() => (UIElement) null;

  public override void Update(IChartTransformer transformer)
  {
    this.bitmap = (this.Series as FastScatterBitmapSeries).Area.GetFastRenderSurface();
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
    FastScatterBitmapSeries series = this.Series as FastScatterBitmapSeries;
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
      double end1 = !(series.ActualXAxis is CategoryAxis) || (series.ActualXAxis as CategoryAxis).IsIndexed ? (double) (series.DataCount - 1) : this.xChartVals.Max();
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
    FastScatterBitmapSeries series = this.Series as FastScatterBitmapSeries;
    double scatterWidth = series.ScatterWidth;
    double scatterHeight = series.ScatterHeight;
    bool isMultiColor = this.Series.Palette != ChartColorPalette.None && series.Interior == null;
    Color color = ChartSegment.GetColor(this.Interior);
    if (this.bitmap != null && this.xValues.Count > 0)
    {
      int width = (int) series.Area.SeriesClipRect.Width;
      int height = (int) series.Area.SeriesClipRect.Height;
      this.bitmap.BeginWrite();
      if (series != null)
      {
        this.isSeriesSelected = false;
        SfChart area = series.Area;
        if (area.GetEnableSeriesSelection())
        {
          Brush seriesSelectionBrush = area.GetSeriesSelectionBrush((ChartSeriesBase) series);
          if (seriesSelectionBrush != null && area.SelectedSeriesCollection.Contains((ChartSeriesBase) series))
          {
            this.isSeriesSelected = true;
            this.seriesSelectionColor = ((SolidColorBrush) seriesSelectionBrush).Color;
          }
        }
        else if (area.GetEnableSegmentSelection())
        {
          Brush segmentSelectionBrush = series.SegmentSelectionBrush;
          if (segmentSelectionBrush != null)
            this.segmentSelectionColor = ((SolidColorBrush) segmentSelectionBrush).Color;
        }
        this.DrawScatterType(series.ShapeType, width, height, color, isMultiColor, scatterWidth, scatterHeight);
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

  private Color GetSegmentInterior(Color color, bool isMultiColor)
  {
    FastScatterBitmapSeries series = this.Series as FastScatterBitmapSeries;
    if (this.isSeriesSelected)
      color = this.seriesSelectionColor;
    else if (series.SelectedSegmentsIndexes.Contains(this.startIndex) && series.SegmentSelectionBrush != null)
      color = this.segmentSelectionColor;
    else if (this.Series.SegmentColorPath != null && series.Interior == null)
      color = this.Series.ColorValues.Count <= 0 || this.Series.ColorValues[this.startIndex] == null ? (this.Series.Palette != ChartColorPalette.None ? ChartSegment.GetColor(this.Series.ColorModel.GetBrush(this.startIndex)) : ChartSegment.GetColor(this.Series.ActualArea.ColorModel.GetBrush(this.Series.ActualArea.GetSeriesIndex(this.Series)))) : ChartSegment.GetColor(this.Series.ColorValues[this.startIndex]);
    else if (isMultiColor)
      color = ChartSegment.GetColor(this.Series.ColorModel.GetBrush(this.startIndex));
    return color;
  }

  private void DrawScatterType(
    ChartSymbol shapeType,
    int width,
    int height,
    Color color,
    bool isMultiColor,
    double xr,
    double yr)
  {
    for (int index = 0; index < this.xValues.Count; ++index)
    {
      double xValue = this.xValues[index];
      double yValue = this.yValues[index];
      Color segmentInterior = this.GetSegmentInterior(color, isMultiColor);
      ++this.startIndex;
      if (yValue > -1.0)
      {
        switch (shapeType)
        {
          case ChartSymbol.Ellipse:
            this.DrawEllipse(width, height, segmentInterior, xValue, yValue, xr, yr);
            continue;
          case ChartSymbol.Cross:
            this.DrawCross(width, height, segmentInterior, xValue, yValue, xr, yr);
            continue;
          case ChartSymbol.Diamond:
            this.DrawDiamond(width, height, segmentInterior, xValue, yValue, xr, yr);
            continue;
          case ChartSymbol.Hexagon:
            this.DrawHexagon(width, height, segmentInterior, xValue, yValue, xr, yr);
            continue;
          case ChartSymbol.InvertedTriangle:
            this.DrawInvertedTriangle(width, height, segmentInterior, xValue, yValue, xr, yr);
            continue;
          case ChartSymbol.Pentagon:
            this.DrawPentagon(width, height, segmentInterior, xValue, yValue, xr, yr);
            continue;
          case ChartSymbol.Plus:
            this.DrawPlus(width, height, segmentInterior, xValue, yValue, xr, yr);
            continue;
          case ChartSymbol.Square:
            this.DrawRectangle(width, height, segmentInterior, xValue, yValue, xr, yr);
            continue;
          case ChartSymbol.Triangle:
            this.DrawTriangle(width, height, segmentInterior, xValue, yValue, xr, yr);
            continue;
          default:
            continue;
        }
      }
    }
  }

  private void DrawEllipse(
    int width,
    int height,
    Color color,
    double xValue,
    double yValue,
    double scatterWidth,
    double scatterHeight)
  {
    FastScatterBitmapSeries series = this.Series as FastScatterBitmapSeries;
    if (series.IsActualTransposed)
      this.bitmap.FillEllipseCentered((int) yValue, (int) xValue, (int) scatterWidth, (int) scatterHeight, color, series.bitmapPixels);
    else
      this.bitmap.FillEllipseCentered((int) xValue, (int) yValue, (int) scatterWidth, (int) scatterHeight, color, series.bitmapPixels);
  }

  private void DrawRectangle(
    int width,
    int height,
    Color color,
    double xValue,
    double yValue,
    double scatterWidth,
    double scatterHeight)
  {
    FastScatterBitmapSeries series = this.Series as FastScatterBitmapSeries;
    double num1 = xValue - scatterWidth / 2.0;
    double num2 = yValue - scatterHeight / 2.0;
    double num3 = xValue + scatterWidth / 2.0;
    double num4 = yValue + scatterHeight / 2.0;
    if (num2 < num4)
    {
      if (series.IsActualTransposed)
      {
        this.bitmap.FillRectangle((int) num2, (int) num1, (int) num4, (int) num3, color, series.bitmapPixels);
        this.Series.bitmapRects.Add(new Rect(new Point(num2, num1), new Point(num4, num3)));
      }
      else
      {
        this.bitmap.FillRectangle((int) num1, (int) num2, (int) num3, (int) num4, color, series.bitmapPixels);
        this.Series.bitmapRects.Add(new Rect(new Point(num1, num2), new Point(num3, num4)));
      }
    }
    else if (series.IsActualTransposed)
    {
      this.bitmap.FillRectangle((int) num4, (int) num1, (int) num2, (int) num3, color, series.bitmapPixels);
      this.Series.bitmapRects.Add(new Rect(new Point(num4, num1), new Point(num2, num3)));
    }
    else
    {
      this.bitmap.FillRectangle((int) num1, (int) num4, (int) num3, (int) num2, color, series.bitmapPixels);
      this.Series.bitmapRects.Add(new Rect(new Point(num1, num4), new Point(num3, num2)));
    }
  }

  private void DrawTriangle(
    int width,
    int height,
    Color color,
    double xValue,
    double yValue,
    double scatterWidth,
    double scatterHeight)
  {
    FastScatterBitmapSeries series = this.Series as FastScatterBitmapSeries;
    double num1 = xValue - scatterWidth / 2.0;
    double num2 = !series.IsActualTransposed ? yValue + scatterHeight / 2.0 : yValue - scatterHeight / 2.0;
    double num3 = xValue + scatterWidth / 2.0;
    double num4 = !series.IsActualTransposed ? yValue + scatterHeight / 2.0 : yValue - scatterHeight / 2.0;
    double num5 = xValue;
    double num6 = !series.IsActualTransposed ? yValue - scatterHeight / 2.0 : yValue + scatterHeight / 2.0;
    if (series.IsActualTransposed)
      this.bitmap.FillPolygon(new int[8]
      {
        (int) num2,
        (int) num1,
        (int) num4,
        (int) num3,
        (int) num6,
        (int) num5,
        (int) num2,
        (int) num1
      }, color, series.bitmapPixels);
    else
      this.bitmap.FillPolygon(new int[8]
      {
        (int) num1,
        (int) num2,
        (int) num3,
        (int) num4,
        (int) num5,
        (int) num6,
        (int) num1,
        (int) num2
      }, color, series.bitmapPixels);
  }

  private void DrawInvertedTriangle(
    int width,
    int height,
    Color color,
    double xValue,
    double yValue,
    double scatterWidth,
    double scatterHeight)
  {
    FastScatterBitmapSeries series = this.Series as FastScatterBitmapSeries;
    double num1 = xValue - scatterWidth / 2.0;
    double num2 = !series.IsActualTransposed ? yValue - scatterHeight / 2.0 : yValue + scatterHeight / 2.0;
    double num3 = xValue + scatterWidth / 2.0;
    double num4 = !series.IsActualTransposed ? yValue - scatterHeight / 2.0 : yValue + scatterHeight / 2.0;
    double num5 = xValue;
    double num6 = !series.IsActualTransposed ? yValue + scatterHeight / 2.0 : yValue - scatterHeight / 2.0;
    if (series.IsActualTransposed)
      this.bitmap.FillPolygon(new int[8]
      {
        (int) num2,
        (int) num1,
        (int) num4,
        (int) num3,
        (int) num6,
        (int) num5,
        (int) num2,
        (int) num1
      }, color, series.bitmapPixels);
    else
      this.bitmap.FillPolygon(new int[8]
      {
        (int) num1,
        (int) num2,
        (int) num3,
        (int) num4,
        (int) num5,
        (int) num6,
        (int) num1,
        (int) num2
      }, color, series.bitmapPixels);
  }

  private void DrawDiamond(
    int width,
    int height,
    Color color,
    double xValue,
    double yValue,
    double scatterWidth,
    double scatterHeight)
  {
    FastScatterBitmapSeries series = this.Series as FastScatterBitmapSeries;
    double num1 = xValue - scatterWidth / 2.0;
    double num2 = yValue;
    double num3 = xValue;
    double num4 = yValue - scatterHeight / 2.0;
    double num5 = xValue + scatterWidth / 2.0;
    double num6 = yValue;
    double num7 = xValue;
    double num8 = yValue + scatterHeight / 2.0;
    if (series.IsActualTransposed)
      this.bitmap.FillPolygon(new int[10]
      {
        (int) num2,
        (int) num1,
        (int) num4,
        (int) num3,
        (int) num6,
        (int) num5,
        (int) num8,
        (int) num7,
        (int) num2,
        (int) num1
      }, color, series.bitmapPixels);
    else
      this.bitmap.FillPolygon(new int[10]
      {
        (int) num1,
        (int) num2,
        (int) num3,
        (int) num4,
        (int) num5,
        (int) num6,
        (int) num7,
        (int) num8,
        (int) num1,
        (int) num2
      }, color, series.bitmapPixels);
  }

  private void DrawHexagon(
    int width,
    int height,
    Color color,
    double xValue,
    double yValue,
    double scatterWidth,
    double scatterHeight)
  {
    FastScatterBitmapSeries series = this.Series as FastScatterBitmapSeries;
    double num1 = xValue - scatterWidth / 2.0;
    double num2 = yValue;
    double num3 = xValue - scatterWidth / 4.0;
    double num4 = yValue - scatterHeight / 2.0;
    double num5 = xValue + scatterWidth / 4.0;
    double num6 = yValue - scatterHeight / 2.0;
    double num7 = xValue + scatterWidth / 2.0;
    double num8 = yValue;
    double num9 = xValue + scatterWidth / 4.0;
    double num10 = yValue + scatterHeight / 2.0;
    double num11 = xValue - scatterWidth / 4.0;
    double num12 = yValue + scatterHeight / 2.0;
    if (series.IsActualTransposed)
      this.bitmap.FillPolygon(new int[14]
      {
        (int) num2,
        (int) num1,
        (int) num4,
        (int) num3,
        (int) num6,
        (int) num5,
        (int) num8,
        (int) num7,
        (int) num10,
        (int) num9,
        (int) num12,
        (int) num11,
        (int) num2,
        (int) num1
      }, color, series.bitmapPixels);
    else
      this.bitmap.FillPolygon(new int[14]
      {
        (int) num1,
        (int) num2,
        (int) num3,
        (int) num4,
        (int) num5,
        (int) num6,
        (int) num7,
        (int) num8,
        (int) num9,
        (int) num10,
        (int) num11,
        (int) num12,
        (int) num1,
        (int) num2
      }, color, series.bitmapPixels);
  }

  private void DrawPentagon(
    int width,
    int height,
    Color color,
    double xValue,
    double yValue,
    double scatterWidth,
    double scatterHeight)
  {
    FastScatterBitmapSeries series = this.Series as FastScatterBitmapSeries;
    double num1 = xValue - scatterWidth / 4.0;
    double num2 = !series.IsActualTransposed ? yValue + scatterHeight / 2.0 : yValue - scatterHeight / 2.0;
    double num3 = xValue - scatterWidth / 2.0;
    double num4 = !series.IsActualTransposed ? yValue - scatterHeight / 8.0 : yValue + scatterHeight / 8.0;
    double num5 = xValue;
    double num6 = !series.IsActualTransposed ? yValue - scatterHeight / 2.0 : yValue + scatterHeight / 2.0;
    double num7 = xValue + scatterWidth / 2.0;
    double num8 = !series.IsActualTransposed ? yValue - scatterHeight / 8.0 : yValue + scatterHeight / 8.0;
    double num9 = xValue + scatterWidth / 4.0;
    double num10 = !series.IsActualTransposed ? yValue + scatterHeight / 2.0 : yValue - scatterHeight / 2.0;
    if (series.IsActualTransposed)
      this.bitmap.FillPolygon(new int[12]
      {
        (int) num2,
        (int) num1,
        (int) num4,
        (int) num3,
        (int) num6,
        (int) num5,
        (int) num8,
        (int) num7,
        (int) num10,
        (int) num9,
        (int) num2,
        (int) num1
      }, color, series.bitmapPixels);
    else
      this.bitmap.FillPolygon(new int[12]
      {
        (int) num1,
        (int) num2,
        (int) num3,
        (int) num4,
        (int) num5,
        (int) num6,
        (int) num7,
        (int) num8,
        (int) num9,
        (int) num10,
        (int) num1,
        (int) num2
      }, color, series.bitmapPixels);
  }

  private void DrawPlus(
    int width,
    int height,
    Color color,
    double xValue,
    double yValue,
    double scatterWidth,
    double scatterHeight)
  {
    FastScatterBitmapSeries series = this.Series as FastScatterBitmapSeries;
    double num1 = xValue - scatterWidth / 8.0;
    double num2 = yValue + scatterHeight / 8.0;
    double num3 = xValue - scatterWidth / 2.0;
    double num4 = yValue + scatterHeight / 8.0;
    double num5 = xValue - scatterWidth / 2.0;
    double num6 = yValue - scatterHeight / 8.0;
    double num7 = xValue - scatterWidth / 8.0;
    double num8 = yValue - scatterHeight / 8.0;
    double num9 = xValue - scatterWidth / 8.0;
    double num10 = yValue - scatterHeight / 2.0;
    double num11 = xValue + scatterWidth / 8.0;
    double num12 = yValue - scatterHeight / 2.0;
    double num13 = xValue + scatterWidth / 8.0;
    double num14 = yValue - scatterHeight / 8.0;
    double num15 = xValue + scatterWidth / 2.0;
    double num16 = yValue - scatterHeight / 8.0;
    double num17 = xValue + scatterWidth / 2.0;
    double num18 = yValue + scatterHeight / 8.0;
    double num19 = xValue + scatterWidth / 8.0;
    double num20 = yValue + scatterHeight / 8.0;
    double num21 = xValue + scatterWidth / 8.0;
    double num22 = yValue + scatterHeight / 2.0;
    double num23 = xValue - scatterWidth / 8.0;
    double num24 = yValue + scatterHeight / 2.0;
    if (series.IsActualTransposed)
      this.bitmap.FillPolygon(new int[26]
      {
        (int) num2,
        (int) num1,
        (int) num4,
        (int) num3,
        (int) num6,
        (int) num5,
        (int) num8,
        (int) num7,
        (int) num10,
        (int) num9,
        (int) num12,
        (int) num11,
        (int) num14,
        (int) num13,
        (int) num16,
        (int) num15,
        (int) num18,
        (int) num17,
        (int) num20,
        (int) num19,
        (int) num22,
        (int) num21,
        (int) num24,
        (int) num23,
        (int) num2,
        (int) num1
      }, color, series.bitmapPixels);
    else
      this.bitmap.FillPolygon(new int[26]
      {
        (int) num1,
        (int) num2,
        (int) num3,
        (int) num4,
        (int) num5,
        (int) num6,
        (int) num7,
        (int) num8,
        (int) num9,
        (int) num10,
        (int) num11,
        (int) num12,
        (int) num13,
        (int) num14,
        (int) num15,
        (int) num16,
        (int) num17,
        (int) num18,
        (int) num19,
        (int) num20,
        (int) num21,
        (int) num22,
        (int) num23,
        (int) num24,
        (int) num1,
        (int) num2
      }, color, series.bitmapPixels);
  }

  private void DrawCross(
    int width,
    int height,
    Color color,
    double xValue,
    double yValue,
    double scatterWidth,
    double scatterHeight)
  {
    FastScatterBitmapSeries series = this.Series as FastScatterBitmapSeries;
    double num1 = xValue;
    double num2 = yValue + scatterHeight / 4.0;
    double num3 = xValue - scatterWidth / 4.0;
    double num4 = yValue + scatterHeight / 2.0;
    double num5 = xValue - scatterWidth / 2.0;
    double num6 = yValue + scatterHeight / 4.0;
    double num7 = xValue - scatterWidth / 4.0;
    double num8 = yValue;
    double num9 = xValue - scatterWidth / 2.0;
    double num10 = yValue - scatterHeight / 4.0;
    double num11 = xValue - scatterWidth / 4.0;
    double num12 = yValue - scatterHeight / 2.0;
    double num13 = xValue;
    double num14 = yValue - scatterHeight / 4.0;
    double num15 = xValue + scatterWidth / 4.0;
    double num16 = yValue - scatterHeight / 2.0;
    double num17 = xValue + scatterWidth / 2.0;
    double num18 = yValue - scatterHeight / 4.0;
    double num19 = xValue + scatterWidth / 4.0;
    double num20 = yValue;
    double num21 = xValue + scatterWidth / 2.0;
    double num22 = yValue + scatterHeight / 4.0;
    double num23 = xValue + scatterWidth / 4.0;
    double num24 = yValue + scatterHeight / 2.0;
    if (series.IsActualTransposed)
      this.bitmap.FillPolygon(new int[26]
      {
        (int) num2,
        (int) num1,
        (int) num4,
        (int) num3,
        (int) num6,
        (int) num5,
        (int) num8,
        (int) num7,
        (int) num10,
        (int) num9,
        (int) num12,
        (int) num11,
        (int) num14,
        (int) num13,
        (int) num16,
        (int) num15,
        (int) num18,
        (int) num17,
        (int) num20,
        (int) num19,
        (int) num22,
        (int) num21,
        (int) num24,
        (int) num23,
        (int) num2,
        (int) num1
      }, color, series.bitmapPixels);
    else
      this.bitmap.FillPolygon(new int[26]
      {
        (int) num1,
        (int) num2,
        (int) num3,
        (int) num4,
        (int) num5,
        (int) num6,
        (int) num7,
        (int) num8,
        (int) num9,
        (int) num10,
        (int) num11,
        (int) num12,
        (int) num13,
        (int) num14,
        (int) num15,
        (int) num16,
        (int) num17,
        (int) num18,
        (int) num19,
        (int) num20,
        (int) num21,
        (int) num22,
        (int) num23,
        (int) num24,
        (int) num1,
        (int) num2
      }, color, series.bitmapPixels);
  }

  private void CalculatePoints(
    ChartTransform.ChartCartesianTransformer cartesianTransformer)
  {
    FastScatterBitmapSeries series = this.Series as FastScatterBitmapSeries;
    Thickness areaBorderThickness = (this.Series.ActualArea as SfChart).AreaBorderThickness;
    ChartAxis xaxis = cartesianTransformer.XAxis;
    if (series.IsIndexed)
    {
      int num1;
      int num2;
      if (!(series.ActualXAxis is CategoryAxis) || !(series.ActualXAxis as CategoryAxis).IsIndexed)
      {
        num1 = 0;
        num2 = this.xChartVals.Count - 1;
      }
      else
      {
        num1 = (int) Math.Floor(xaxis.VisibleRange.Start);
        int num3 = (int) Math.Ceiling(xaxis.VisibleRange.End);
        num2 = num3 > this.yChartVals.Count - 1 ? this.yChartVals.Count - 1 : num3;
      }
      int num4 = num1 < 0 ? 0 : num1;
      this.startIndex = num4;
      for (int index = num4; index <= num2; ++index)
        this.AddDataPoint(cartesianTransformer, areaBorderThickness, index);
    }
    else if (series.isLinearData)
    {
      double start = xaxis.VisibleRange.Start;
      double end = xaxis.VisibleRange.End;
      this.startIndex = 0;
      int index1 = this.xChartVals.Count - 1;
      double newBase = cartesianTransformer.XAxis.IsLogarithmic ? (cartesianTransformer.XAxis as LogarithmicAxis).LogarithmicBase : 0.0;
      int index2;
      for (index2 = 1; index2 < index1; ++index2)
      {
        double a = this.xChartVals[index2];
        if (cartesianTransformer.XAxis.IsLogarithmic)
          a = Math.Log(a, newBase);
        if (a >= start && a <= end)
          this.AddDataPoint(cartesianTransformer, areaBorderThickness, index2);
        else if (a < start)
          this.startIndex = index2;
        else if (a > end)
        {
          this.AddDataPoint(cartesianTransformer, areaBorderThickness, index2);
          break;
        }
      }
      this.InsertDataPoint(cartesianTransformer, areaBorderThickness, this.startIndex);
      if (index2 != index1)
        return;
      this.AddDataPoint(cartesianTransformer, areaBorderThickness, index1);
    }
    else
    {
      this.startIndex = 0;
      for (int index = 0; index < this.Series.DataCount; ++index)
        this.AddDataPoint(cartesianTransformer, areaBorderThickness, index);
    }
  }

  private void AddDataPoint(
    ChartTransform.ChartCartesianTransformer cartesianTransformer,
    Thickness areaBorderThickness,
    int index)
  {
    double xValue;
    double yValue;
    this.GetXYValue(index, out xValue, out yValue);
    Point visible = cartesianTransformer.TransformToVisible(xValue, yValue);
    if (!(this.Series as FastScatterBitmapSeries).IsTransposed)
    {
      this.xValues.Add(visible.X - areaBorderThickness.Left);
      this.yValues.Add(visible.Y - areaBorderThickness.Top);
    }
    else
    {
      this.xValues.Add(visible.Y - areaBorderThickness.Left);
      this.yValues.Add(visible.X - areaBorderThickness.Top);
    }
  }

  private void InsertDataPoint(
    ChartTransform.ChartCartesianTransformer cartesianTransformer,
    Thickness areaBorderThickness,
    int index)
  {
    double xValue;
    double yValue;
    this.GetXYValue(index, out xValue, out yValue);
    Point visible = cartesianTransformer.TransformToVisible(xValue, yValue);
    if (!(this.Series as FastScatterBitmapSeries).IsTransposed)
    {
      this.xValues.Insert(0, visible.X - areaBorderThickness.Left);
      this.yValues.Insert(0, visible.Y - areaBorderThickness.Top);
    }
    else
    {
      this.xValues.Insert(0, visible.Y - areaBorderThickness.Left);
      this.yValues.Insert(0, visible.X - areaBorderThickness.Top);
    }
  }

  private void GetXYValue(int index, out double xValue, out double yValue)
  {
    xValue = this.xChartVals[index];
    yValue = this.yChartVals[index];
  }
}
