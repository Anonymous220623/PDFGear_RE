// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.FastHiLoSegment
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

public class FastHiLoSegment : ChartSegment
{
  private WriteableBitmap bitmap;
  private IList<double> xChartVals;
  private IList<double> yHiChartVals;
  private IList<double> yLoChartVals;
  private Color seriesSelectionColor = Colors.Transparent;
  private Color segmentSelectionColor = Colors.Transparent;
  private bool isSeriesSelected;
  private List<float> xValues;
  private List<float> yHiValues;
  private List<float> yLoValues;
  private int startIndex;

  public FastHiLoSegment()
  {
    this.xValues = new List<float>();
    this.yHiValues = new List<float>();
    this.yLoValues = new List<float>();
  }

  public FastHiLoSegment(ChartSeries series)
  {
    this.xValues = new List<float>();
    this.yHiValues = new List<float>();
    this.yLoValues = new List<float>();
    this.Series = (ChartSeriesBase) series;
  }

  public FastHiLoSegment(
    IList<double> xValues,
    IList<double> hiValues,
    IList<double> loValues,
    AdornmentSeries series)
    : this((ChartSeries) series)
  {
  }

  public override UIElement CreateVisual(Size size)
  {
    this.bitmap = (this.Series as ChartSeries).Area.GetFastRenderSurface();
    return (UIElement) null;
  }

  public override void SetData(IList<double> xVals, IList<double> hiVals, IList<double> lowVals)
  {
    this.xChartVals = xVals;
    this.yHiChartVals = hiVals;
    this.yLoChartVals = lowVals;
    List<double> source1 = new List<double>();
    source1.AddRange((IEnumerable<double>) (hiVals as List<double>));
    source1.AddRange((IEnumerable<double>) (lowVals as List<double>));
    if (this.Series.DataCount <= 0)
      return;
    double d = source1.Min();
    double start;
    if (double.IsNaN(d))
    {
      IEnumerable<double> source2 = source1.Where<double>((Func<double, bool>) (e => !double.IsNaN(e)));
      start = !source2.Any<double>() ? 0.0 : source2.Min();
    }
    else
      start = d;
    if (this.Series.IsIndexed)
    {
      double end1 = !(this.Series.ActualXAxis is CategoryAxis) || (this.Series.ActualXAxis as CategoryAxis).IsIndexed ? (double) (this.Series.DataCount - 1) : this.xChartVals.Max();
      double end2 = source1.Max();
      this.XRange = new DoubleRange(0.0, end1);
      this.YRange = new DoubleRange(start, end2);
    }
    else
    {
      double end3 = this.xChartVals.Max();
      double end4 = source1.Max();
      this.XRange = new DoubleRange(this.xChartVals.Min(), end3);
      this.YRange = new DoubleRange(start, end4);
    }
  }

  public override UIElement GetRenderedVisual() => (UIElement) null;

  public override void Update(IChartTransformer transformer)
  {
    this.bitmap = (this.Series as ChartSeries).Area.GetFastRenderSurface();
    if (transformer == null || this.Series.DataCount <= 0)
      return;
    ChartTransform.ChartCartesianTransformer cartesianTransformer = transformer as ChartTransform.ChartCartesianTransformer;
    this.xValues.Clear();
    this.yHiValues.Clear();
    this.yLoValues.Clear();
    this.x_isInversed = cartesianTransformer.XAxis.IsInversed;
    this.y_isInversed = cartesianTransformer.YAxis.IsInversed;
    this.CalculatePoints(cartesianTransformer);
    this.UpdateVisual(true);
  }

  public override void OnSizeChanged(Size size)
  {
    this.bitmap = (this.Series as ChartSeries).Area.GetFastRenderSurface();
  }

  internal void UpdateVisual(bool updateHiLoLine)
  {
    ChartSeries series = this.Series as ChartSeries;
    bool isMultiColor = series.Palette != ChartColorPalette.None && series.Interior == null;
    Color color = ChartSegment.GetColor(this.Interior);
    int dataCount = this.yLoValues.Count >= this.xValues.Count ? this.xValues.Count : this.yLoValues.Count;
    if (this.bitmap != null && this.xValues.Count > 0)
    {
      int width = (int) series.Area.SeriesClipRect.Width;
      int height = (int) series.Area.SeriesClipRect.Height;
      int leftThickness = (int) series.StrokeThickness / 2;
      int rightThickness = series.StrokeThickness % 2.0 == 0.0 ? (int) (series.StrokeThickness / 2.0) : (int) (series.StrokeThickness / 2.0 + 1.0);
      this.bitmap.BeginWrite();
      if (series is FastHiLoBitmapSeries)
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
        else if (area.GetEnableSegmentSelection())
        {
          Brush segmentSelectionBrush = (series as ISegmentSelectable).SegmentSelectionBrush;
          if (segmentSelectionBrush != null)
            this.segmentSelectionColor = ((SolidColorBrush) segmentSelectionBrush).Color;
        }
        if (!series.IsActualTransposed)
          this.UpdateVisualHorizontal(width, height, color, leftThickness, rightThickness, isMultiColor, dataCount);
        else
          this.UpdateVisualVertical(width, height, color, leftThickness, rightThickness, isMultiColor, dataCount);
      }
      this.bitmap.EndWrite();
    }
    series.Area.CanRenderToBuffer = true;
    this.xValues.Clear();
    this.yHiValues.Clear();
    this.yLoValues.Clear();
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

  private void AddDataPoint(
    ChartTransform.ChartCartesianTransformer cartesianTransformer,
    int index)
  {
    Point hipoint;
    Point lopoint;
    this.GetHiLoPoints(cartesianTransformer, index, out hipoint, out lopoint);
    if (!this.Series.IsActualTransposed)
    {
      this.xValues.Add((float) hipoint.X);
      this.yHiValues.Add((float) hipoint.Y);
      this.yLoValues.Add((float) lopoint.Y);
    }
    else
    {
      this.xValues.Add((float) hipoint.Y);
      this.yHiValues.Add((float) hipoint.X);
      this.yLoValues.Add((float) lopoint.X);
    }
  }

  private void InsertDataPoint(
    ChartTransform.ChartCartesianTransformer cartesianTransformer,
    int index)
  {
    Point hipoint;
    Point lopoint;
    this.GetHiLoPoints(cartesianTransformer, index, out hipoint, out lopoint);
    if (!this.Series.IsActualTransposed)
    {
      this.xValues.Insert(0, (float) hipoint.X);
      this.yHiValues.Insert(0, (float) hipoint.Y);
      this.yLoValues.Insert(0, (float) lopoint.Y);
    }
    else
    {
      this.xValues.Insert(0, (float) hipoint.Y);
      this.yHiValues.Insert(0, (float) hipoint.X);
      this.yLoValues.Insert(0, (float) lopoint.X);
    }
  }

  private void GetHiLoPoints(
    ChartTransform.ChartCartesianTransformer cartesianTransformer,
    int index,
    out Point hipoint,
    out Point lopoint)
  {
    if (this.Series.IsIndexed)
    {
      hipoint = cartesianTransformer.TransformToVisible((double) index, this.yHiChartVals[index]);
      lopoint = cartesianTransformer.TransformToVisible((double) index, this.yLoChartVals[index]);
    }
    else
    {
      hipoint = cartesianTransformer.TransformToVisible(this.xChartVals[index], this.yHiChartVals[index]);
      lopoint = cartesianTransformer.TransformToVisible(this.xChartVals[index], this.yLoChartVals[index]);
    }
  }

  private void CalculatePoints(
    ChartTransform.ChartCartesianTransformer cartesianTransformer)
  {
    ChartSeries series = this.Series as ChartSeries;
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
        num2 = num3 > this.yHiChartVals.Count - 1 ? this.yHiChartVals.Count - 1 : num3;
      }
      int num4 = num1 < 0 ? 0 : num1;
      this.startIndex = num4;
      for (int index = num4; index <= num2; ++index)
      {
        if (index < this.yHiChartVals.Count)
          this.AddDataPoint(cartesianTransformer, index);
      }
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
        if (a <= end && a >= start)
          this.AddDataPoint(cartesianTransformer, index2);
        else if (a < start)
          this.startIndex = index2;
        else if (a > end)
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
      for (int index = 0; index < this.Series.DataCount; ++index)
        this.AddDataPoint(cartesianTransformer, index);
    }
  }

  private void UpdateVisualVertical(
    int width,
    int height,
    Color color,
    int leftThickness,
    int rightThickness,
    bool isMultiColor,
    int dataCount)
  {
    ChartSeries series = this.Series as ChartSeries;
    Color color1 = color;
    for (int index = 0; index < dataCount; ++index)
    {
      float xValue1 = this.xValues[index];
      float x2 = this.y_isInversed ? this.yLoValues[index] : this.yHiValues[index];
      float xValue2 = this.xValues[index];
      float x1 = this.y_isInversed ? this.yHiValues[index] : this.yLoValues[index];
      int y1 = (int) xValue1 - leftThickness;
      int y2 = (int) xValue2 + rightThickness;
      color = !this.isSeriesSelected ? (!series.SelectedSegmentsIndexes.Contains(this.startIndex) || (series as ISegmentSelectable).SegmentSelectionBrush == null ? (!isMultiColor ? color1 : ChartSegment.GetColor(series.ColorModel.GetBrush(this.startIndex))) : this.segmentSelectionColor) : this.seriesSelectionColor;
      ++this.startIndex;
      this.bitmap.FillRectangle((int) x1, y1, (int) x2, y2, color, series.bitmapPixels);
    }
  }

  private void UpdateVisualHorizontal(
    int width,
    int height,
    Color color,
    int leftThickness,
    int rightThickness,
    bool isMultiColor,
    int dataCount)
  {
    ChartSeries series = this.Series as ChartSeries;
    Color color1 = color;
    for (int index = 0; index < dataCount; ++index)
    {
      float xValue = this.xValues[index];
      float y1 = this.y_isInversed ? this.yLoValues[index] : this.yHiValues[index];
      float y2 = this.y_isInversed ? this.yHiValues[index] : this.yLoValues[index];
      float x1 = xValue - (float) leftThickness;
      float x2 = xValue + (float) rightThickness;
      color = !this.isSeriesSelected ? (!series.SelectedSegmentsIndexes.Contains(this.startIndex) || (series as ISegmentSelectable).SegmentSelectionBrush == null ? (!isMultiColor ? color1 : ChartSegment.GetColor(series.ColorModel.GetBrush(this.startIndex))) : this.segmentSelectionColor) : this.seriesSelectionColor;
      ++this.startIndex;
      this.bitmap.FillRectangle((int) x1, (int) y1, (int) x2, (int) y2, color, series.bitmapPixels);
    }
  }
}
