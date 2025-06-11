// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.FastBarBitmapSegment
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

public class FastBarBitmapSegment : ChartSegment
{
  private WriteableBitmap bitmap;
  private Size availableSize;
  private IList<double> x1ChartVals;
  private IList<double> y1ChartVals;
  private IList<double> x2ChartVals;
  private IList<double> y2ChartVals;
  private double xStart;
  private double xEnd;
  private double yStart;
  private double yEnd;
  private double width;
  private double height;
  private double left;
  private double top;
  private Color seriesSelectionColor = Colors.Transparent;
  private Color segmentSelectionColor = Colors.Transparent;
  private bool isSeriesSelected;
  private List<float> x1Values;
  private List<float> x2Values;
  private List<float> y1Values;
  private List<float> y2Values;
  private int startIndex;

  public FastBarBitmapSegment()
  {
    this.x1Values = new List<float>();
    this.x2Values = new List<float>();
    this.y1Values = new List<float>();
    this.y2Values = new List<float>();
  }

  public FastBarBitmapSegment(ChartSeriesBase series)
  {
    this.x1Values = new List<float>();
    this.x2Values = new List<float>();
    this.y1Values = new List<float>();
    this.y2Values = new List<float>();
    this.Series = (ChartSeriesBase) (series as ChartSeries);
  }

  public FastBarBitmapSegment(
    IList<double> x1Values,
    IList<double> y1Values,
    IList<double> x2Values,
    IList<double> y2Values,
    ChartSeriesBase series)
    : this(series)
  {
    this.Series = series;
    if (this.Series.ActualXAxis is CategoryAxis && !(this.Series.ActualXAxis as CategoryAxis).IsIndexed)
      this.Item = (object) series.GroupedActualData;
    else
      this.Item = (object) series.ActualData;
  }

  public override UIElement CreateVisual(Size size) => (UIElement) null;

  public override void SetData(
    IList<double> x1Values,
    IList<double> y1Values,
    IList<double> x2Values,
    IList<double> y2Values)
  {
    this.x1ChartVals = x1Values;
    this.y1ChartVals = y1Values;
    this.x2ChartVals = x2Values;
    this.y2ChartVals = y2Values;
    double end1 = x2Values.Max();
    double end2 = y1Values.Max();
    double start = x1Values.Min();
    double d = this.y1ChartVals.Min();
    double num1;
    if (double.IsNaN(d))
    {
      IEnumerable<double> source = this.y1ChartVals.Where<double>((Func<double, bool>) (e => !double.IsNaN(e)));
      num1 = !source.Any<double>() ? 0.0 : source.Min();
    }
    else
      num1 = d;
    double num2 = this.y2ChartVals.Min();
    this.XRange = new DoubleRange(start, end1);
    this.YRange = new DoubleRange(num1 > num2 ? num2 : num1, end2);
  }

  public override UIElement GetRenderedVisual() => (UIElement) null;

  public override void Update(IChartTransformer transformer)
  {
    this.bitmap = (this.Series as ChartSeries).Area.GetFastRenderSurface();
    if (transformer == null || this.Series.DataCount == 0)
      return;
    ChartTransform.ChartCartesianTransformer cartesianTransformer = transformer as ChartTransform.ChartCartesianTransformer;
    bool flag = cartesianTransformer.XAxis.IsLogarithmic || cartesianTransformer.YAxis.IsLogarithmic;
    this.x_isInversed = cartesianTransformer.XAxis.IsInversed;
    this.y_isInversed = cartesianTransformer.YAxis.IsInversed;
    this.xStart = cartesianTransformer.XAxis.VisibleRange.Start;
    this.xEnd = cartesianTransformer.XAxis.VisibleRange.End;
    this.yStart = cartesianTransformer.YAxis.VisibleRange.Start;
    this.yEnd = cartesianTransformer.YAxis.VisibleRange.End;
    this.width = cartesianTransformer.XAxis.RenderedRect.Height;
    this.height = cartesianTransformer.YAxis.RenderedRect.Width;
    this.left = (this.Series as ChartSeries).Area.SeriesClipRect.Right - cartesianTransformer.YAxis.RenderedRect.Right;
    this.top = (this.Series as ChartSeries).Area.SeriesClipRect.Bottom - cartesianTransformer.XAxis.RenderedRect.Bottom;
    this.availableSize = new Size(this.width, this.height);
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
    this.x1Values.Clear();
    this.x2Values.Clear();
    this.y1Values.Clear();
    this.y2Values.Clear();
    if (!flag)
      this.CalculatePoints(cartesianTransformer);
    else
      this.CalculateLogPoints(cartesianTransformer);
    this.UpdateVisual();
  }

  public override void OnSizeChanged(Size size)
  {
  }

  internal void UpdateVisual()
  {
    ChartSeries series = this.Series as ChartSeries;
    bool flag = series.Palette != ChartColorPalette.None && series.Interior == null;
    SfChart area = series.Area;
    this.isSeriesSelected = false;
    Color color1 = ChartSegment.GetColor(this.Interior);
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
    int count = this.x1Values.Count;
    if (this.bitmap != null && this.x1Values.Count != 0)
    {
      double width = (double) (int) area.SeriesClipRect.Width;
      double height = (double) (int) area.SeriesClipRect.Height;
      this.bitmap.BeginWrite();
      for (int index = 0; index < count; ++index)
      {
        Color color2;
        if (this.isSeriesSelected)
          color2 = this.seriesSelectionColor;
        else if (series.SelectedSegmentsIndexes.Contains(this.startIndex) && (series as ISegmentSelectable).SegmentSelectionBrush != null)
          color2 = this.segmentSelectionColor;
        else if (series.SegmentColorPath != null && series.Interior == null)
        {
          if (series.ColorValues.Count > 0 && series.ColorValues[this.startIndex] != null)
            color2 = ChartSegment.GetColor(series.ColorValues[this.startIndex]);
          else if (series.Palette == ChartColorPalette.None)
          {
            int seriesIndex = series.ActualArea.GetSeriesIndex(this.Series);
            color2 = ChartSegment.GetColor(series.ActualArea.ColorModel.GetBrush(seriesIndex));
          }
          else
            color2 = ChartSegment.GetColor(series.ColorModel.GetBrush(this.startIndex));
        }
        else
          color2 = !flag ? color1 : ChartSegment.GetColor(series.ColorModel.GetBrush(this.startIndex));
        ++this.startIndex;
        float num1 = this.x1Values[index];
        float num2 = this.x2Values[index];
        float num3 = this.y1ChartVals[index] > 0.0 ? this.y1Values[index] : this.y2Values[index];
        float num4 = this.y1ChartVals[index] > 0.0 ? this.y2Values[index] : this.y1Values[index];
        double segmentSpacing1 = (series as ISegmentSpacing).SegmentSpacing;
        if (segmentSpacing1 > 0.0 && segmentSpacing1 <= 1.0)
        {
          double segmentSpacing2 = (this.Series as ISegmentSpacing).CalculateSegmentSpacing(segmentSpacing1, (double) num1, (double) num2);
          double segmentSpacing3 = (this.Series as ISegmentSpacing).CalculateSegmentSpacing(segmentSpacing1, (double) num2, (double) num1);
          num2 = (float) segmentSpacing2;
          num1 = (float) segmentSpacing3;
        }
        float num5 = num2 - num1;
        if ((double) num3 < (double) num4)
        {
          this.bitmap.FillRectangle((int) (width - (double) num4), (int) (height - (double) num1 - (double) num5), (int) (width - (double) num3), (int) (height - (double) num1), color2, series.bitmapPixels);
          this.Series.bitmapRects.Add(new Rect(new Point(width - (double) num4, height - (double) num1 - (double) num5), new Point(width - (double) num3, height - (double) num1)));
        }
        else
        {
          this.bitmap.FillRectangle((int) (width - (double) num3), (int) (height - (double) num1 - (double) num5), (int) (width - (double) num4), (int) (height - (double) num1), color2, series.bitmapPixels);
          this.Series.bitmapRects.Add(new Rect(new Point(width - (double) num3, height - (double) num1 - (double) num5), new Point(width - (double) num4, height - (double) num1)));
        }
      }
      this.bitmap.EndWrite();
    }
    area.CanRenderToBuffer = true;
    this.x1Values.Clear();
    this.x2Values.Clear();
    this.y1Values.Clear();
    this.y2Values.Clear();
  }

  protected override void SetVisualBindings(Shape element)
  {
  }

  private void CalculateLogPoints(
    ChartTransform.ChartCartesianTransformer cartesianTransformer)
  {
    double newBase1 = cartesianTransformer.XAxis.IsLogarithmic ? (cartesianTransformer.XAxis as LogarithmicAxis).LogarithmicBase : 1.0;
    double newBase2 = cartesianTransformer.YAxis.IsLogarithmic ? (cartesianTransformer.YAxis as LogarithmicAxis).LogarithmicBase : 1.0;
    for (int index = 0; index < this.Series.DataCount; ++index)
    {
      double num1 = newBase1 == 1.0 ? this.x1ChartVals[index] : Math.Log(this.x1ChartVals[index], newBase1);
      double num2 = newBase1 == 1.0 ? this.x2ChartVals[index] : Math.Log(this.x2ChartVals[index], newBase1);
      double num3 = newBase2 == 1.0 ? this.y1ChartVals[index] : Math.Log(this.y1ChartVals[index], newBase2);
      double num4 = newBase2 == 1.0 ? this.y2ChartVals[index] : Math.Log(this.y2ChartVals[index], newBase2);
      double num5 = this.x_isInversed ? (num2 < this.xEnd ? this.xEnd : num2) : (num1 < this.xStart ? this.xStart : num1);
      double num6 = this.x_isInversed ? (num1 > this.xStart ? this.xStart : num1) : (num2 > this.xEnd ? this.xEnd : num2);
      double num7 = this.y_isInversed ? (num4 > this.yStart ? this.yStart : (num4 < this.yEnd ? this.yEnd : num4)) : (num3 > this.yEnd ? this.yEnd : (num3 < this.yStart ? this.yStart : num3));
      double num8 = this.y_isInversed ? (num3 < this.yEnd ? this.yEnd : (num3 > this.yStart ? this.yStart : num3)) : (num4 < this.yStart ? this.yStart : (num4 > this.yEnd ? this.yEnd : num4));
      float num9 = (float) (this.top + this.availableSize.Width * cartesianTransformer.XAxis.ValueToCoefficientCalc(num5));
      float num10 = (float) (this.top + this.availableSize.Width * cartesianTransformer.XAxis.ValueToCoefficientCalc(num6));
      float num11 = (float) (this.left + this.availableSize.Height * (1.0 - cartesianTransformer.YAxis.ValueToCoefficientCalc(num7)));
      float num12 = (float) (this.left + this.availableSize.Height * (1.0 - cartesianTransformer.YAxis.ValueToCoefficientCalc(num8)));
      this.x1Values.Add(num9);
      this.x2Values.Add(num10);
      this.y1Values.Add(num11);
      this.y2Values.Add(num12);
    }
  }

  private void CalculatePoints(
    ChartTransform.ChartCartesianTransformer cartesianTransformer)
  {
    ChartAxis xaxis = cartesianTransformer.XAxis;
    if (this.Series.IsIndexed)
    {
      int num1;
      int num2;
      if (!(this.Series.ActualXAxis is CategoryAxis) || !(this.Series.ActualXAxis as CategoryAxis).IsIndexed)
      {
        num1 = 0;
        num2 = this.x1ChartVals.Count - 1;
      }
      else
      {
        num1 = (int) Math.Floor(xaxis.VisibleRange.Start);
        int num3 = (int) Math.Ceiling(xaxis.VisibleRange.End);
        num2 = num3 > this.y1ChartVals.Count - 1 ? this.y1ChartVals.Count - 1 : num3;
      }
      int num4 = num1 < 0 ? 0 : num1;
      for (int index = num4; index <= num2; ++index)
        this.AddDataPoint(cartesianTransformer, index);
      this.startIndex = num4;
    }
    else if (this.Series.isLinearData)
    {
      double start = xaxis.VisibleRange.Start;
      double end = xaxis.VisibleRange.End;
      int index1 = this.x1ChartVals.Count - 1;
      this.startIndex = 0;
      int index2;
      for (index2 = 1; index2 < index1; ++index2)
      {
        double x1ChartVal = this.x1ChartVals[index2];
        if (x1ChartVal >= start && x1ChartVal <= end)
          this.AddDataPoint(cartesianTransformer, index2);
        else if (x1ChartVal < start)
          this.startIndex = index2;
        else if (x1ChartVal > end)
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
      for (int index = 0; index < this.Series.DataCount; ++index)
        this.AddDataPoint(cartesianTransformer, index);
    }
  }

  private void AddDataPoint(
    ChartTransform.ChartCartesianTransformer cartesianTransformer,
    int index)
  {
    double x1Value = 0.0;
    double x2Value = 0.0;
    double y2Value = 0.0;
    double y1Value = 0.0;
    this.GetPoints(index, out x1Value, out x2Value, out y1Value, out y2Value);
    float num1 = (float) (this.top + this.availableSize.Width * cartesianTransformer.XAxis.ValueToCoefficientCalc(x1Value));
    float num2 = (float) (this.top + this.availableSize.Width * cartesianTransformer.XAxis.ValueToCoefficientCalc(x2Value));
    float num3 = (float) (this.left + this.availableSize.Height * (1.0 - cartesianTransformer.YAxis.ValueToCoefficientCalc(y1Value)));
    float num4 = (float) (this.left + this.availableSize.Height * (1.0 - cartesianTransformer.YAxis.ValueToCoefficientCalc(y2Value)));
    this.x1Values.Add(num1);
    this.x2Values.Add(num2);
    this.y1Values.Add(num3);
    this.y2Values.Add(num4);
  }

  private void InsertDataPoint(
    ChartTransform.ChartCartesianTransformer cartesianTransformer,
    int index)
  {
    double x1Value = 0.0;
    double x2Value = 0.0;
    double y2Value = 0.0;
    double y1Value = 0.0;
    this.GetPoints(index, out x1Value, out x2Value, out y1Value, out y2Value);
    this.x1Values.Insert(0, (float) (this.top + this.availableSize.Width * cartesianTransformer.XAxis.ValueToCoefficientCalc(x1Value)));
    this.x2Values.Insert(0, (float) (this.top + this.availableSize.Width * cartesianTransformer.XAxis.ValueToCoefficientCalc(x2Value)));
    this.y1Values.Insert(0, (float) (this.left + this.availableSize.Height * (1.0 - cartesianTransformer.YAxis.ValueToCoefficientCalc(y1Value))));
    this.y2Values.Insert(0, (float) (this.left + this.availableSize.Height * (1.0 - cartesianTransformer.YAxis.ValueToCoefficientCalc(y2Value))));
  }

  private void GetPoints(
    int index,
    out double x1Value,
    out double x2Value,
    out double y1Value,
    out double y2Value)
  {
    if (this.x_isInversed)
    {
      x1Value = this.x2ChartVals[index];
      x2Value = this.x1ChartVals[index];
    }
    else
    {
      x1Value = this.x1ChartVals[index];
      x2Value = this.x2ChartVals[index];
    }
    if (this.y_isInversed)
    {
      y1Value = this.y2ChartVals[index];
      y2Value = this.y1ChartVals[index];
    }
    else
    {
      y1Value = this.y1ChartVals[index];
      y2Value = this.y2ChartVals[index];
    }
  }
}
