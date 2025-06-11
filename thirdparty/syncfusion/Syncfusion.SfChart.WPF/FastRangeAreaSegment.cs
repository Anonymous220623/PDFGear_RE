// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.FastRangeAreaSegment
// Assembly: Syncfusion.SfChart.WPF, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 279D70C6-7936-4F3E-B6CF-79EFEF734826
// Assembly location: C:\Program Files\PDFgear\Syncfusion.SfChart.WPF.dll

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

#nullable disable
namespace Syncfusion.UI.Xaml.Charts;

public class FastRangeAreaSegment : ChartSegment
{
  private bool isSeriesSelected;
  private Color seriesSelectionColor = Colors.Transparent;
  private WriteableBitmap bitmap;
  private double highValue;
  private double lowValue;
  private List<int> areaPoints;
  private int[] points1 = new int[10];
  private int[] points2 = new int[10];
  private Point intersectingPoint;

  internal bool IsHighLow { get; set; }

  public FastRangeAreaSegment() => this.areaPoints = new List<int>();

  public FastRangeAreaSegment(
    List<ChartPoint> areaValues,
    bool isHighLow,
    FastRangeAreaBitmapSeries series)
  {
    this.IsHighLow = isHighLow;
    this.Series = (ChartSeriesBase) series;
    this.areaPoints = new List<int>();
    this.AreaValues = areaValues;
  }

  public double High
  {
    get => this.highValue;
    set
    {
      this.highValue = value;
      this.OnPropertyChanged(nameof (High));
    }
  }

  public double Low
  {
    get => this.lowValue;
    set
    {
      this.lowValue = value;
      this.OnPropertyChanged(nameof (Low));
    }
  }

  internal List<ChartPoint> AreaValues { get; set; }

  internal EmptyStroke EmptyStroke { get; set; }

  public override UIElement GetRenderedVisual() => (UIElement) null;

  public override UIElement CreateVisual(Size size)
  {
    this.bitmap = (this.Series as FastRangeAreaBitmapSeries).Area.GetFastRenderSurface();
    return (UIElement) null;
  }

  public override void SetData(List<ChartPoint> areaValues)
  {
    this.AreaValues = areaValues;
    double end1 = this.AreaValues.Max<ChartPoint>((Func<ChartPoint, double>) (x => x.X));
    double end2 = this.AreaValues.Max<ChartPoint>((Func<ChartPoint, double>) (y => y.Y));
    double start1 = this.AreaValues.Min<ChartPoint>((Func<ChartPoint, double>) (x => x.X));
    double d = this.AreaValues.Min<ChartPoint>((Func<ChartPoint, double>) (item => item.Y));
    double start2;
    if (double.IsNaN(d))
    {
      IEnumerable<ChartPoint> source = this.AreaValues.Where<ChartPoint>((Func<ChartPoint, bool>) (item => !double.IsNaN(item.Y)));
      start2 = !source.Any<ChartPoint>() ? 0.0 : source.Min<ChartPoint>((Func<ChartPoint, double>) (item => item.Y));
    }
    else
      start2 = d;
    this.XRange = new DoubleRange(start1, end1);
    this.YRange = new DoubleRange(start2, end2);
  }

  public override void Update(IChartTransformer transformer)
  {
    this.bitmap = (this.Series as FastRangeAreaBitmapSeries).Area.GetFastRenderSurface();
    if (transformer == null || this.Series.DataCount <= 0)
      return;
    this.CalculatePoints(transformer as ChartTransform.ChartCartesianTransformer);
    this.UpdateVisual();
  }

  public override void OnSizeChanged(Size size)
  {
    this.bitmap = (this.Series as FastRangeAreaBitmapSeries).Area.GetFastRenderSurface();
  }

  internal void UpdateVisual()
  {
    FastRangeAreaBitmapSeries series = this.Series as FastRangeAreaBitmapSeries;
    SfChart area = series.Area;
    this.isSeriesSelected = false;
    Color segmentColor = this.GetSegmentColor();
    if (area.GetEnableSeriesSelection())
    {
      Brush seriesSelectionBrush = area.GetSeriesSelectionBrush((ChartSeriesBase) series);
      if (seriesSelectionBrush != null && area.SelectedSeriesCollection.Contains((ChartSeriesBase) series))
      {
        this.isSeriesSelected = true;
        this.seriesSelectionColor = ((SolidColorBrush) seriesSelectionBrush).Color;
      }
    }
    Color color = this.isSeriesSelected ? this.seriesSelectionColor : segmentColor;
    if (this.bitmap != null)
    {
      int width = (int) series.Area.SeriesClipRect.Width;
      int height = (int) series.Area.SeriesClipRect.Height;
      int leftThickness = (int) series.StrokeThickness / 2;
      int rightThickness = series.StrokeThickness % 2.0 == 0.0 ? (int) (series.StrokeThickness / 2.0) : (int) (series.StrokeThickness / 2.0 + 1.0);
      this.bitmap.BeginWrite();
      if (series != null)
      {
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
        this.UpdateVisual(width, height, color, leftThickness, rightThickness);
      }
      this.bitmap.EndWrite();
    }
    series.Area.CanRenderToBuffer = true;
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

  private Color GetSegmentColor()
  {
    FastRangeAreaBitmapSeries series = this.Series as FastRangeAreaBitmapSeries;
    if (series.Interior != null)
      return ChartSegment.GetColor(series.Interior);
    if (this.IsHighLow && series.HighValueInterior != null)
      return ChartSegment.GetColor(series.HighValueInterior);
    if (!this.IsHighLow && series.LowValueInterior != null)
      return ChartSegment.GetColor(series.LowValueInterior);
    if (series.Palette != ChartColorPalette.None)
      return ChartSegment.GetColor(series.GetInteriorColor(0));
    return this.Interior != null ? ChartSegment.GetColor(this.Interior) : new SolidColorBrush(Colors.Transparent).Color;
  }

  private void CalculatePoints(
    ChartTransform.ChartCartesianTransformer cartesianTransformer)
  {
    int index1 = 0;
    int num1 = 0;
    ChartAxis actualXaxis = this.Series.ActualXAxis;
    double num2 = Math.Floor(actualXaxis is LogarithmicAxis logarithmicAxis ? Math.Pow(logarithmicAxis.LogarithmicBase, actualXaxis.VisibleRange.Start) : actualXaxis.VisibleRange.Start);
    double num3 = Math.Ceiling(logarithmicAxis != null ? Math.Pow(logarithmicAxis.LogarithmicBase, actualXaxis.VisibleRange.End) : actualXaxis.VisibleRange.End);
    if (this.Series.IsIndexed)
    {
      bool flag = actualXaxis is CategoryAxis categoryAxis && categoryAxis.IsIndexed;
      int num4 = this.AreaValues.Count / 2;
      int num5;
      int num6;
      if (!flag)
      {
        num5 = 0;
        num6 = num4 - 1;
      }
      else
      {
        int num7 = (int) Math.Floor(actualXaxis.VisibleRange.Start);
        int num8 = (int) Math.Ceiling(actualXaxis.VisibleRange.End);
        num6 = num8 > num4 - 1 ? num4 - 1 : num8;
        num5 = num7 < 0 ? 0 : num7;
      }
      for (int index2 = num5 * 2; index2 < this.AreaValues.Count; index2 += 2)
      {
        Point visible = cartesianTransformer.TransformToVisible(this.AreaValues[index2].X, this.AreaValues[index2].Y);
        this.areaPoints.Add((int) visible.X);
        this.areaPoints.Add((int) visible.Y);
      }
      num1 = num6 * 2 + 1;
      for (int index3 = this.AreaValues.Count - 1; index3 >= 1; index3 -= 2)
      {
        Point visible = cartesianTransformer.TransformToVisible(this.AreaValues[index3].X, this.AreaValues[index3].Y);
        this.areaPoints.Add((int) visible.X);
        this.areaPoints.Add((int) visible.Y);
      }
      if (this.areaPoints.Count <= 1)
        return;
      this.areaPoints.Add(this.areaPoints[0]);
      this.areaPoints.Add(this.areaPoints[1]);
    }
    else
    {
      this.areaPoints.Clear();
      Point point = new Point();
      bool flag1 = false;
      for (int index4 = 0; index4 < this.AreaValues.Count; index4 += 2)
      {
        double x = this.AreaValues[index4].X;
        if (num2 <= x && x <= num3)
        {
          Point visible = cartesianTransformer.TransformToVisible(this.AreaValues[index4].X, this.AreaValues[index4].Y);
          this.areaPoints.Add((int) visible.X);
          this.areaPoints.Add((int) visible.Y);
        }
        else if (x < num2)
        {
          flag1 = true;
          index1 = index4;
        }
        else if (x > num3)
        {
          Point visible = cartesianTransformer.TransformToVisible(this.AreaValues[index4].X, this.AreaValues[index4].Y);
          this.areaPoints.Add((int) visible.X);
          this.areaPoints.Add((int) visible.Y);
          break;
        }
      }
      if (flag1)
      {
        Point visible = cartesianTransformer.TransformToVisible(this.AreaValues[index1].X, this.AreaValues[index1].Y);
        this.areaPoints.Insert(0, (int) visible.Y);
        this.areaPoints.Insert(0, (int) visible.X);
      }
      int count = this.areaPoints.Count;
      int index5 = this.AreaValues.Count - 1;
      bool flag2 = false;
      for (int index6 = this.AreaValues.Count - 1; index6 >= 1; index6 -= 2)
      {
        double x = this.AreaValues[index6].X;
        if (num2 <= x && x <= num3)
        {
          Point visible = cartesianTransformer.TransformToVisible(this.AreaValues[index6].X, this.AreaValues[index6].Y);
          this.areaPoints.Add((int) visible.X);
          this.areaPoints.Add((int) visible.Y);
        }
        else if (x > num3)
        {
          flag2 = true;
          index5 = index6;
        }
        else if (x < num2)
        {
          Point visible = cartesianTransformer.TransformToVisible(this.AreaValues[index6].X, this.AreaValues[index6].Y);
          this.areaPoints.Add((int) visible.X);
          this.areaPoints.Add((int) visible.Y);
          break;
        }
      }
      if (count < this.areaPoints.Count && flag2)
      {
        Point visible = cartesianTransformer.TransformToVisible(this.AreaValues[index5].X, this.AreaValues[index5].Y);
        this.areaPoints.Insert(count, (int) visible.Y);
        this.areaPoints.Insert(count, (int) visible.X);
      }
      if (this.areaPoints.Count <= 1)
        return;
      this.areaPoints.Add(this.areaPoints[0]);
      this.areaPoints.Add(this.areaPoints[1]);
    }
  }

  private void UpdateVisual(
    int width,
    int height,
    Color color,
    int leftThickness,
    int rightThickness)
  {
    FastRangeAreaBitmapSeries series = this.Series as FastRangeAreaBitmapSeries;
    if (series.EnableAntiAliasing)
      this.DrawAntiAliasingPointsAroundArea(color, series.bitmapPixels);
    this.bitmap.FillPolygon(this.areaPoints.ToArray(), color, series.bitmapPixels);
    if (series.StrokeThickness < 1.0 || series.Stroke == null)
      return;
    this.DrawStroke(series.bitmapPixels, width, height);
  }

  private void DrawStroke(List<int> bitmapPixels, int width, int height)
  {
    FastRangeAreaBitmapSeries series = this.Series as FastRangeAreaBitmapSeries;
    Color color = (series.Stroke as SolidColorBrush).Color;
    double leftThickness = series.StrokeThickness / 2.0;
    double rightThickness = series.StrokeThickness % 2.0 == 0.0 ? series.StrokeThickness / 2.0 - 1.0 : series.StrokeThickness / 2.0;
    int areaPoint1 = this.areaPoints[0];
    int areaPoint2 = this.areaPoints[1];
    int areaPoint3 = this.areaPoints[2];
    int areaPoint4 = this.areaPoints[3];
    if (this.points1 == null)
      this.points1 = new int[10];
    FastRangeAreaSegment.GetLinePoints((double) areaPoint1, (double) areaPoint2, (double) areaPoint3, (double) areaPoint4, leftThickness, rightThickness, this.points1);
    double[] emptyStrokeIndexes = this.GetEmptyStrokeIndexes();
    int num1 = series.Area.IsMultipleArea ? 1 : 0;
    int index = 2;
    int num2 = 0;
    while (index < this.areaPoints.Count)
    {
      this.points2 = new int[10];
      int xStart = areaPoint3;
      int yStart = areaPoint4;
      index += 2;
      if (index + 1 < this.areaPoints.Count)
      {
        areaPoint3 = this.areaPoints[index];
        areaPoint4 = this.areaPoints[index + 1];
        this.UpdatePoints2((double) xStart, (double) yStart, (double) areaPoint3, (double) areaPoint4, leftThickness, rightThickness);
      }
      if (emptyStrokeIndexes[0] != (double) num2 && emptyStrokeIndexes[1] != (double) num2)
      {
        this.bitmap.DrawLineAa(this.points1[0], this.points1[1], this.points1[2], this.points1[3], color, bitmapPixels);
        this.bitmap.FillPolygon(this.points1, color, bitmapPixels);
        this.bitmap.DrawLineAa(this.points1[4], this.points1[5], this.points1[6], this.points1[7], color, bitmapPixels);
      }
      num2 += 2;
      this.points1 = this.points2;
      this.points2 = (int[]) null;
    }
    this.points1 = (int[]) null;
    this.points2 = (int[]) null;
  }

  private double[] GetEmptyStrokeIndexes()
  {
    double[] emptyStrokeIndexes = new double[2]
    {
      double.NaN,
      double.NaN
    };
    if (this.EmptyStroke == EmptyStroke.Right || this.EmptyStroke == EmptyStroke.Both)
      emptyStrokeIndexes[0] = (double) (this.areaPoints.Count / 2 - 3);
    if (this.EmptyStroke == EmptyStroke.Left || this.EmptyStroke == EmptyStroke.Both)
      emptyStrokeIndexes[1] = (double) (this.areaPoints.Count - 4);
    return emptyStrokeIndexes;
  }

  private void UpdatePoints2(
    double xStart,
    double yStart,
    double xEnd,
    double yEnd,
    double leftThickness,
    double rightThickness)
  {
    FastRangeAreaSegment.GetLinePoints(xStart, yStart, xEnd, yEnd, leftThickness, rightThickness, this.points2);
    bool flag1 = false;
    if (this.points1[0] == this.points1[2])
      flag1 = this.points2[0] != this.points2[2] || this.points1[0] == this.points2[0];
    else if (this.points2[0] == this.points2[2])
      flag1 = true;
    else if (Math.Floor(WriteableBitmapExtensions.Slope((double) this.points1[2], (double) this.points1[1], (double) this.points1[0], (double) this.points1[3])) != Math.Floor(WriteableBitmapExtensions.Slope((double) this.points2[2], (double) this.points2[1], (double) this.points2[0], (double) this.points2[3])))
      flag1 = true;
    if (flag1 && this.FindIntersectingPoints((double) this.points1[0], (double) this.points1[1], (double) this.points1[2], (double) this.points1[3], (double) this.points2[0], (double) this.points2[1], (double) this.points2[2], (double) this.points2[3]))
    {
      this.points1[2] = this.points2[0] = this.points2[8] = (int) this.intersectingPoint.X;
      this.points1[3] = this.points2[1] = this.points2[9] = (int) this.intersectingPoint.Y;
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
    this.points1[5] = this.points2[7] = (int) this.intersectingPoint.Y;
    this.points1[4] = this.points2[6] = (int) this.intersectingPoint.X;
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

  private void DrawAntiAliasingPointsAroundArea(Color color, List<int> bitmapPixels)
  {
    ChartSeriesBase series = this.Series;
    for (int index = 0; index < this.areaPoints.Count; index += 2)
    {
      if (index + 3 < this.areaPoints.Count)
        this.bitmap.DrawLineAa(this.areaPoints[index], this.areaPoints[index + 1], this.areaPoints[index + 2], this.areaPoints[index + 3], color, bitmapPixels);
    }
  }
}
