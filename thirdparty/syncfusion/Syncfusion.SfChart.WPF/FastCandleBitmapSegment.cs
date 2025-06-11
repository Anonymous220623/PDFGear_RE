// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.FastCandleBitmapSegment
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

public class FastCandleBitmapSegment : ChartSegment
{
  private DoubleRange sbsInfo;
  private WriteableBitmap bitmap;
  private IList<double> xChartVals;
  private IList<double> openChartVals;
  private IList<double> closeChartVals;
  private IList<double> highChartVals;
  private IList<double> lowChartVals;
  private double xStart;
  private double xEnd;
  private double yStart;
  private double yEnd;
  private double xSize;
  private double ySize;
  private double xOffset;
  private double yOffset;
  private int count;
  private Color seriesSelectionColor = Colors.Transparent;
  private Color segmentSelectionColor = Colors.Transparent;
  private bool isSeriesSelected;
  private List<float> xValues;
  private List<float> x1Values;
  private List<float> x2Values;
  private List<float> openValue;
  private List<float> closeValue;
  private List<float> highValue;
  private List<float> highValue1;
  private List<float> lowValue;
  private List<float> lowValue1;
  private List<bool> isBull;
  private List<bool> isHollow;
  private int startIndex;

  public FastCandleBitmapSegment()
  {
    this.xValues = new List<float>();
    this.x1Values = new List<float>();
    this.x2Values = new List<float>();
    this.openValue = new List<float>();
    this.closeValue = new List<float>();
    this.highValue = new List<float>();
    this.highValue1 = new List<float>();
    this.lowValue = new List<float>();
    this.lowValue1 = new List<float>();
    this.isBull = new List<bool>();
    this.isHollow = new List<bool>();
  }

  public FastCandleBitmapSegment(ChartSeriesBase series)
  {
    this.xValues = new List<float>();
    this.x1Values = new List<float>();
    this.x2Values = new List<float>();
    this.openValue = new List<float>();
    this.closeValue = new List<float>();
    this.highValue = new List<float>();
    this.highValue1 = new List<float>();
    this.lowValue = new List<float>();
    this.lowValue1 = new List<float>();
    this.isBull = new List<bool>();
    this.isHollow = new List<bool>();
    this.Series = (ChartSeriesBase) (series as ChartSeries);
  }

  public FastCandleBitmapSegment(
    IList<double> xValues,
    IList<double> OpenValues,
    IList<double> CloseValues,
    IList<double> highValues,
    IList<double> LowValues,
    ChartSeriesBase series)
    : this(series)
  {
  }

  public override UIElement CreateVisual(Size size) => (UIElement) null;

  public override void SetData(
    IList<double> xValues,
    IList<double> openValue,
    IList<double> closeValue,
    IList<double> highValue,
    IList<double> lowValue)
  {
    this.sbsInfo = this.Series.GetSideBySideInfo(this.Series);
    this.xChartVals = xValues;
    this.openChartVals = openValue;
    this.closeChartVals = closeValue;
    this.highChartVals = highValue;
    this.lowChartVals = lowValue;
    double end1 = xValues.Max() + this.sbsInfo.End;
    double end2 = highValue.Max();
    double start1 = xValues.Min() + this.sbsInfo.Start;
    double d = lowValue.Min();
    double start2;
    if (double.IsNaN(d))
    {
      IEnumerable<double> source = lowValue.Where<double>((Func<double, bool>) (e => !double.IsNaN(e)));
      start2 = !source.Any<double>() ? 0.0 : source.Min();
    }
    else
      start2 = d;
    this.XRange = new DoubleRange(start1, end1);
    this.YRange = new DoubleRange(start2, end2);
  }

  public override UIElement GetRenderedVisual() => (UIElement) null;

  public override void Update(IChartTransformer transformer)
  {
    ChartSeries series = this.Series as ChartSeries;
    this.bitmap = series.Area.GetFastRenderSurface();
    if (transformer == null || series.DataCount <= 0)
      return;
    ChartTransform.ChartCartesianTransformer cartesianTransformer = transformer as ChartTransform.ChartCartesianTransformer;
    this.count = (int) Math.Ceiling(this.xEnd);
    this.count = Math.Min(this.count, this.xChartVals.Count);
    this.x_isInversed = cartesianTransformer.XAxis.IsInversed;
    this.y_isInversed = cartesianTransformer.YAxis.IsInversed;
    this.xStart = cartesianTransformer.XAxis.VisibleRange.Start;
    this.xEnd = cartesianTransformer.XAxis.VisibleRange.End;
    this.yStart = cartesianTransformer.YAxis.VisibleRange.Start;
    this.yEnd = cartesianTransformer.YAxis.VisibleRange.End;
    if (series.IsActualTransposed)
    {
      this.ySize = cartesianTransformer.YAxis.RenderedRect.Width;
      this.xSize = cartesianTransformer.XAxis.RenderedRect.Height;
      this.yOffset = cartesianTransformer.YAxis.RenderedRect.Left - series.Area.SeriesClipRect.Left;
      this.xOffset = cartesianTransformer.XAxis.RenderedRect.Top - series.Area.SeriesClipRect.Top;
    }
    else
    {
      this.ySize = cartesianTransformer.YAxis.RenderedRect.Height;
      this.xSize = cartesianTransformer.XAxis.RenderedRect.Width;
      this.yOffset = cartesianTransformer.YAxis.RenderedRect.Top - series.Area.SeriesClipRect.Top;
      this.xOffset = cartesianTransformer.XAxis.RenderedRect.Left - series.Area.SeriesClipRect.Left;
    }
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
    this.isHollow.Clear();
    this.isBull.Clear();
    this.xValues.Clear();
    this.x1Values.Clear();
    this.x2Values.Clear();
    this.openValue.Clear();
    this.closeValue.Clear();
    this.highValue.Clear();
    this.highValue1.Clear();
    this.lowValue.Clear();
    this.lowValue1.Clear();
    this.CalculatePoints(cartesianTransformer);
    this.UpdateVisual();
  }

  public override void OnSizeChanged(Size size)
  {
  }

  internal void UpdateVisual()
  {
    ChartSeries series = this.Series as ChartSeries;
    Color color = new Color();
    if (this.bitmap != null && this.xValues.Count != 0)
    {
      int width = (int) series.Area.SeriesClipRect.Width;
      int height = (int) series.Area.SeriesClipRect.Height;
      int leftThickness = (int) series.StrokeThickness / 2;
      int rightThickness = series.StrokeThickness % 2.0 == 0.0 ? (int) (series.StrokeThickness / 2.0) : (int) (series.StrokeThickness / 2.0 + 1.0);
      this.bitmap.BeginWrite();
      if (series is FastCandleBitmapSeries)
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
          this.UpdateVisualHorizontal(width, height, color, leftThickness, rightThickness);
        else
          this.UpdateVisualVertical(width, height, color, leftThickness, rightThickness);
      }
      this.bitmap.EndWrite();
    }
    series.Area.CanRenderToBuffer = true;
    this.xValues.Clear();
    this.x1Values.Clear();
    this.x2Values.Clear();
    this.openValue.Clear();
    this.closeValue.Clear();
    this.highValue.Clear();
    this.lowValue.Clear();
  }

  internal Color GetSegmentBrush(int index)
  {
    ChartSeries series = this.Series as ChartSeries;
    Color color1;
    Color segmentBrush;
    if (this.isBull.Count > index && this.isBull[index])
    {
      Color color2;
      if ((series as FastCandleBitmapSeries).BullFillColor == null)
        color2 = ((SolidColorBrush) this.Interior).Color;
      else
        color1 = color2 = ((SolidColorBrush) (series as FastCandleBitmapSeries).BullFillColor).Color;
      segmentBrush = color2;
    }
    else
    {
      Color color3;
      if ((series as FastCandleBitmapSeries).BearFillColor == null)
        color3 = ((SolidColorBrush) this.Interior).Color;
      else
        color1 = color3 = ((SolidColorBrush) (series as FastCandleBitmapSeries).BearFillColor).Color;
      segmentBrush = color3;
    }
    return segmentBrush;
  }

  private void CalculatePoints(
    ChartTransform.ChartCartesianTransformer cartesianTransformer)
  {
    IList<double> comparisionModeValues = (this.Series as FastCandleBitmapSeries).GetComparisionModeValues();
    ChartAxis xaxis = cartesianTransformer.XAxis;
    if (this.Series.IsIndexed)
    {
      int num1;
      int num2;
      if (!(this.Series.ActualXAxis is CategoryAxis) || !(this.Series.ActualXAxis as CategoryAxis).IsIndexed)
      {
        num1 = 0;
        num2 = this.xChartVals.Count - 1;
      }
      else
      {
        num1 = (int) Math.Floor(xaxis.VisibleRange.Start);
        int num3 = (int) Math.Ceiling(xaxis.VisibleRange.End);
        num2 = num3 > this.highChartVals.Count - 1 ? this.highChartVals.Count - 1 : num3;
      }
      int num4 = num1 < 0 ? 0 : num1;
      this.startIndex = num4;
      if (!this.Series.IsActualTransposed)
      {
        for (int index = num4; index <= num2; ++index)
          this.AddHorizontalPoint(cartesianTransformer, index, comparisionModeValues);
      }
      else
      {
        double xBase = cartesianTransformer.XAxis.IsLogarithmic ? (cartesianTransformer.XAxis as LogarithmicAxis).LogarithmicBase : 1.0;
        double yBase = cartesianTransformer.YAxis.IsLogarithmic ? (cartesianTransformer.YAxis as LogarithmicAxis).LogarithmicBase : 1.0;
        for (int index = num4; index <= num2; ++index)
          this.AddVerticalPoint(cartesianTransformer, index, comparisionModeValues, xBase, yBase);
      }
    }
    else
    {
      this.startIndex = 0;
      if (this.Series.isLinearData)
      {
        double start = xaxis.VisibleRange.Start;
        double end = xaxis.VisibleRange.End;
        int num5 = this.xChartVals.Count - 1;
        if (!this.Series.IsActualTransposed)
        {
          double newBase = cartesianTransformer.XAxis.IsLogarithmic ? (cartesianTransformer.XAxis as LogarithmicAxis).LogarithmicBase : 0.0;
          int index;
          for (index = 1; index < num5; ++index)
          {
            double a = this.xChartVals[index];
            if (cartesianTransformer.XAxis.IsLogarithmic)
              a = Math.Log(a, newBase);
            if (a <= end && a >= start)
              this.AddHorizontalPoint(cartesianTransformer, index, comparisionModeValues);
            else if (a < start)
              this.startIndex = index;
            else if (a > end)
            {
              this.AddHorizontalPoint(cartesianTransformer, index, comparisionModeValues);
              break;
            }
          }
          this.InsertHorizontalPoint(cartesianTransformer, this.startIndex, comparisionModeValues);
          if (index != num5)
            return;
          this.AddHorizontalPoint(cartesianTransformer, index, comparisionModeValues);
        }
        else
        {
          double num6 = cartesianTransformer.XAxis.IsLogarithmic ? (cartesianTransformer.XAxis as LogarithmicAxis).LogarithmicBase : 1.0;
          double yBase = cartesianTransformer.YAxis.IsLogarithmic ? (cartesianTransformer.YAxis as LogarithmicAxis).LogarithmicBase : 1.0;
          int index;
          for (index = 1; index < num5; ++index)
          {
            double a = this.xChartVals[index];
            if (cartesianTransformer.XAxis.IsLogarithmic)
              a = Math.Log(a, num6);
            if (a <= end && a >= start)
              this.AddVerticalPoint(cartesianTransformer, index, comparisionModeValues, num6, yBase);
            else if (a < start)
              this.startIndex = index;
            else if (a > end)
            {
              this.AddVerticalPoint(cartesianTransformer, index, comparisionModeValues, num6, yBase);
              break;
            }
          }
          this.InsertVerticalPoint(cartesianTransformer, this.startIndex, comparisionModeValues, num6, yBase);
          if (index != num5)
            return;
          this.AddVerticalPoint(cartesianTransformer, index, comparisionModeValues, num6, yBase);
        }
      }
      else
      {
        this.startIndex = 0;
        for (int index = 0; index < this.Series.DataCount; ++index)
        {
          if (!this.Series.IsActualTransposed)
          {
            this.AddHorizontalPoint(cartesianTransformer, index, comparisionModeValues);
          }
          else
          {
            double xBase = cartesianTransformer.XAxis.IsLogarithmic ? (cartesianTransformer.XAxis as LogarithmicAxis).LogarithmicBase : 1.0;
            double yBase = cartesianTransformer.YAxis.IsLogarithmic ? (cartesianTransformer.YAxis as LogarithmicAxis).LogarithmicBase : 1.0;
            this.AddVerticalPoint(cartesianTransformer, index, comparisionModeValues, xBase, yBase);
          }
        }
      }
    }
  }

  private void AddVerticalPoint(
    ChartTransform.ChartCartesianTransformer cartesianTransformer,
    int index,
    IList<double> values,
    double xBase,
    double yBase)
  {
    double median = this.sbsInfo.Median;
    double start = this.sbsInfo.Start;
    double end = this.sbsInfo.End;
    double highChartVal = this.highChartVals[index];
    double lowChartVal = this.lowChartVals[index];
    double openChartVal = this.openChartVals[index];
    double closeChartVal = this.closeChartVals[index];
    double[] numArray = this.AlignHiLoSegment(this.openChartVals[index], this.closeChartVals[index], highChartVal, lowChartVal);
    double num1 = numArray[0];
    double num2 = numArray[1];
    double x1Val;
    double x2Val;
    this.GetPoints(index, out x1Val, out x2Val, start, end);
    double a1 = this.y_isInversed ? this.closeChartVals[index] : this.openChartVals[index];
    double a2 = this.y_isInversed ? this.openChartVals[index] : this.closeChartVals[index];
    this.isBull.Add(index == 0 || (this.Series as FastCandleBitmapSeries).ComparisonMode == FinancialPrice.None ? openChartVal < closeChartVal : values[index] >= values[index - 1]);
    this.isHollow.Add(closeChartVal > openChartVal && (this.Series as FastCandleBitmapSeries).ComparisonMode != FinancialPrice.None);
    double num3 = xBase == 1.0 ? x1Val : Math.Log(x1Val, xBase);
    double num4 = xBase == 1.0 ? x2Val : Math.Log(x2Val, xBase);
    double num5 = yBase == 1.0 ? a1 : Math.Log(a1, yBase);
    double num6 = yBase == 1.0 ? a2 : Math.Log(a2, yBase);
    if ((this.y_isInversed ? (num5 > num6 ? 1 : 0) : (num5 < num6 ? 1 : 0)) != 0)
    {
      double num7 = num5;
      num5 = num6;
      num6 = num7;
    }
    double y1 = num1;
    double y2 = num2;
    double y3;
    double y4;
    if (openChartVal > closeChartVal)
    {
      y3 = openChartVal;
      y4 = closeChartVal;
    }
    else
    {
      y3 = closeChartVal;
      y4 = openChartVal;
    }
    Point visible1;
    Point visible2;
    Point visible3;
    Point visible4;
    if (this.Series.IsIndexed)
    {
      visible1 = cartesianTransformer.TransformToVisible((double) index + median, y1);
      visible2 = cartesianTransformer.TransformToVisible((double) index + median, y2);
      visible3 = cartesianTransformer.TransformToVisible((double) index + median, y3);
      visible4 = cartesianTransformer.TransformToVisible((double) index + median, y4);
    }
    else
    {
      visible1 = cartesianTransformer.TransformToVisible(this.xChartVals[index] + median, y1);
      visible2 = cartesianTransformer.TransformToVisible(this.xChartVals[index] + median, y2);
      visible3 = cartesianTransformer.TransformToVisible((double) index + median, y3);
      visible4 = cartesianTransformer.TransformToVisible((double) index + median, y4);
    }
    this.xValues.Add((float) visible1.Y);
    this.x1Values.Add((float) (this.xOffset + this.xSize * cartesianTransformer.XAxis.ValueToCoefficientCalc(num3)));
    this.x2Values.Add((float) (this.xOffset + this.xSize * cartesianTransformer.XAxis.ValueToCoefficientCalc(num4)));
    this.openValue.Add((float) (this.yOffset + this.ySize * (1.0 - cartesianTransformer.YAxis.ValueToCoefficientCalc(num5))));
    this.closeValue.Add((float) (this.yOffset + this.ySize * (1.0 - cartesianTransformer.YAxis.ValueToCoefficientCalc(num6))));
    this.highValue.Add((float) visible1.X);
    this.lowValue.Add((float) visible2.X);
    this.highValue1.Add((float) visible3.X);
    this.lowValue1.Add((float) visible4.X);
  }

  private void InsertVerticalPoint(
    ChartTransform.ChartCartesianTransformer cartesianTransformer,
    int index,
    IList<double> values,
    double xBase,
    double yBase)
  {
    double median = this.sbsInfo.Median;
    double start = this.sbsInfo.Start;
    double end = this.sbsInfo.End;
    double highChartVal = this.highChartVals[index];
    double lowChartVal = this.lowChartVals[index];
    double openChartVal = this.openChartVals[index];
    double closeChartVal = this.closeChartVals[index];
    double[] numArray = this.AlignHiLoSegment(this.openChartVals[index], this.closeChartVals[index], highChartVal, lowChartVal);
    double num1 = numArray[0];
    double num2 = numArray[1];
    double x1Val;
    double x2Val;
    this.GetPoints(index, out x1Val, out x2Val, start, end);
    double a1 = this.y_isInversed ? this.closeChartVals[index] : this.openChartVals[index];
    double a2 = this.y_isInversed ? this.openChartVals[index] : this.closeChartVals[index];
    this.isBull.Insert(0, index == 0 || (this.Series as FastCandleBitmapSeries).ComparisonMode == FinancialPrice.None ? openChartVal < closeChartVal : values[index] >= values[index - 1]);
    this.isHollow.Insert(0, closeChartVal > openChartVal && (this.Series as FastCandleBitmapSeries).ComparisonMode != FinancialPrice.None);
    double num3 = xBase == 1.0 ? x1Val : Math.Log(x1Val, xBase);
    double num4 = xBase == 1.0 ? x2Val : Math.Log(x2Val, xBase);
    double num5 = yBase == 1.0 ? a1 : Math.Log(a1, yBase);
    double num6 = yBase == 1.0 ? a2 : Math.Log(a2, yBase);
    if ((this.y_isInversed ? (num5 > num6 ? 1 : 0) : (num5 < num6 ? 1 : 0)) != 0)
    {
      double num7 = num5;
      num5 = num6;
      num6 = num7;
    }
    double y1 = num1;
    double y2 = num2;
    double y3;
    double y4;
    if (openChartVal > closeChartVal)
    {
      y3 = openChartVal;
      y4 = closeChartVal;
    }
    else
    {
      y3 = closeChartVal;
      y4 = openChartVal;
    }
    Point visible1 = cartesianTransformer.TransformToVisible(this.xChartVals[index] + median, y1);
    Point visible2 = cartesianTransformer.TransformToVisible(this.xChartVals[index] + median, y2);
    Point visible3 = cartesianTransformer.TransformToVisible((double) index + median, y3);
    Point visible4 = cartesianTransformer.TransformToVisible((double) index + median, y4);
    this.xValues.Insert(0, (float) visible1.Y);
    this.x1Values.Insert(0, (float) (this.xOffset + this.xSize * cartesianTransformer.XAxis.ValueToCoefficientCalc(num3)));
    this.x2Values.Insert(0, (float) (this.xOffset + this.xSize * cartesianTransformer.XAxis.ValueToCoefficientCalc(num4)));
    this.openValue.Insert(0, (float) (this.yOffset + this.ySize * (1.0 - cartesianTransformer.YAxis.ValueToCoefficientCalc(num5))));
    this.closeValue.Insert(0, (float) (this.yOffset + this.ySize * (1.0 - cartesianTransformer.YAxis.ValueToCoefficientCalc(num6))));
    this.highValue.Insert(0, (float) visible1.X);
    this.lowValue.Insert(0, (float) visible2.X);
    this.highValue1.Insert(0, (float) visible3.X);
    this.lowValue1.Insert(0, (float) visible4.X);
  }

  private void AddHorizontalPoint(
    ChartTransform.ChartCartesianTransformer cartesianTransformer,
    int index,
    IList<double> values)
  {
    double median = this.sbsInfo.Median;
    double start = this.sbsInfo.Start;
    double end = this.sbsInfo.End;
    double highChartVal = this.highChartVals[index];
    double lowChartVal = this.lowChartVals[index];
    double openChartVal = this.openChartVals[index];
    double closeChartVal = this.closeChartVals[index];
    double[] numArray = this.AlignHiLoSegment(this.openChartVals[index], this.closeChartVals[index], highChartVal, lowChartVal);
    double num1 = numArray[0];
    double num2 = numArray[1];
    double x1Val;
    double x2Val;
    this.GetPoints(index, out x1Val, out x2Val, start, end);
    double y1 = this.y_isInversed ? this.closeChartVals[index] : this.openChartVals[index];
    double y2 = this.y_isInversed ? this.openChartVals[index] : this.closeChartVals[index];
    this.isBull.Add(index == 0 || (this.Series as FastCandleBitmapSeries).ComparisonMode == FinancialPrice.None ? openChartVal < closeChartVal : values[index] >= values[index - 1]);
    this.isHollow.Add(closeChartVal > openChartVal && (this.Series as FastCandleBitmapSeries).ComparisonMode != FinancialPrice.None);
    if ((this.y_isInversed ? (y1 > y2 ? 1 : 0) : (y1 < y2 ? 1 : 0)) != 0)
    {
      double num3 = y1;
      y1 = y2;
      y2 = num3;
    }
    double y3 = num1;
    double y4 = num2;
    double y5;
    double y6;
    if (openChartVal > closeChartVal)
    {
      y5 = openChartVal;
      y6 = closeChartVal;
    }
    else
    {
      y5 = closeChartVal;
      y6 = openChartVal;
    }
    Point visible1 = cartesianTransformer.TransformToVisible(x1Val, y1);
    Point visible2 = cartesianTransformer.TransformToVisible(x2Val, y2);
    Point visible3;
    Point visible4;
    if (this.Series.IsIndexed)
    {
      visible3 = cartesianTransformer.TransformToVisible((double) index + median, y3);
      visible4 = cartesianTransformer.TransformToVisible((double) index + median, y4);
    }
    else
    {
      visible3 = cartesianTransformer.TransformToVisible(this.xChartVals[index] + median, y3);
      visible4 = cartesianTransformer.TransformToVisible(this.xChartVals[index] + median, y4);
    }
    Point visible5 = cartesianTransformer.TransformToVisible((double) index + median, y5);
    Point visible6 = cartesianTransformer.TransformToVisible((double) index + median, y6);
    this.xValues.Add((float) visible3.X);
    this.x1Values.Add((float) visible1.X);
    this.x2Values.Add((float) visible2.X);
    this.openValue.Add((float) visible1.Y);
    this.closeValue.Add((float) visible2.Y);
    this.highValue.Add((float) visible3.Y);
    this.lowValue.Add((float) visible4.Y);
    this.highValue1.Add((float) visible5.Y);
    this.lowValue1.Add((float) visible6.Y);
  }

  private void InsertHorizontalPoint(
    ChartTransform.ChartCartesianTransformer cartesianTransformer,
    int index,
    IList<double> values)
  {
    double median = this.sbsInfo.Median;
    double start = this.sbsInfo.Start;
    double end = this.sbsInfo.End;
    double highChartVal = this.highChartVals[index];
    double lowChartVal = this.lowChartVals[index];
    double openChartVal = this.openChartVals[index];
    double closeChartVal = this.closeChartVals[index];
    double[] numArray = this.AlignHiLoSegment(this.openChartVals[index], this.closeChartVals[index], highChartVal, lowChartVal);
    double num1 = numArray[0];
    double num2 = numArray[1];
    double x1Val;
    double x2Val;
    this.GetPoints(index, out x1Val, out x2Val, start, end);
    double y1 = this.y_isInversed ? this.closeChartVals[index] : this.openChartVals[index];
    double y2 = this.y_isInversed ? this.openChartVals[index] : this.closeChartVals[index];
    this.isBull.Insert(0, index == 0 || (this.Series as FastCandleBitmapSeries).ComparisonMode == FinancialPrice.None ? openChartVal < closeChartVal : values[index] >= values[index - 1]);
    this.isHollow.Insert(0, closeChartVal > openChartVal && (this.Series as FastCandleBitmapSeries).ComparisonMode != FinancialPrice.None);
    if ((this.y_isInversed ? (y1 > y2 ? 1 : 0) : (y1 < y2 ? 1 : 0)) != 0)
    {
      double num3 = y1;
      y1 = y2;
      y2 = num3;
    }
    double y3 = num1;
    double y4 = num2;
    double y5;
    double y6;
    if (openChartVal > closeChartVal)
    {
      y5 = openChartVal;
      y6 = closeChartVal;
    }
    else
    {
      y5 = closeChartVal;
      y6 = openChartVal;
    }
    Point visible1 = cartesianTransformer.TransformToVisible(x1Val, y1);
    Point visible2 = cartesianTransformer.TransformToVisible(x2Val, y2);
    Point visible3;
    Point visible4;
    if (this.Series.IsIndexed)
    {
      visible3 = cartesianTransformer.TransformToVisible((double) index + median, y3);
      visible4 = cartesianTransformer.TransformToVisible((double) index + median, y4);
    }
    else
    {
      visible3 = cartesianTransformer.TransformToVisible(this.xChartVals[index] + median, y3);
      visible4 = cartesianTransformer.TransformToVisible(this.xChartVals[index] + median, y4);
    }
    Point visible5 = cartesianTransformer.TransformToVisible((double) index + median, y5);
    Point visible6 = cartesianTransformer.TransformToVisible((double) index + median, y6);
    this.xValues.Insert(0, (float) visible3.X);
    this.x1Values.Insert(0, (float) visible1.X);
    this.x2Values.Insert(0, (float) visible2.X);
    this.openValue.Insert(0, (float) visible1.Y);
    this.closeValue.Insert(0, (float) visible2.Y);
    this.highValue.Insert(0, (float) visible3.Y);
    this.lowValue.Insert(0, (float) visible4.Y);
    this.highValue1.Insert(0, (float) visible5.Y);
    this.lowValue1.Insert(0, (float) visible6.Y);
  }

  private void GetPoints(
    int index,
    out double x1Val,
    out double x2Val,
    double sbsStart,
    double sbsEnd)
  {
    if (this.Series.IsIndexed)
    {
      x1Val = !this.x_isInversed ? sbsStart + (double) index : sbsEnd + (double) index;
      x2Val = !this.x_isInversed ? sbsEnd + (double) index : sbsStart + (double) index;
    }
    else
    {
      x1Val = this.x_isInversed ? this.xChartVals[index] + sbsEnd : this.xChartVals[index] + sbsStart;
      x2Val = this.x_isInversed ? this.xChartVals[index] + sbsStart : this.xChartVals[index] + sbsEnd;
    }
  }

  private void UpdateVisualVertical(
    int width,
    int height,
    Color color,
    int leftThickness,
    int rightThickness)
  {
    ChartSeries series = this.Series as ChartSeries;
    int count = this.xValues.Count;
    Brush bullFillColor = (series as FastCandleBitmapSeries).BullFillColor;
    Brush bearFillColor = (series as FastCandleBitmapSeries).BearFillColor;
    for (int index = 0; index < count; ++index)
    {
      if (this.isSeriesSelected)
        color = this.seriesSelectionColor;
      else if (series.SelectedSegmentsIndexes.Contains(this.startIndex) && (series as ISegmentSelectable).SegmentSelectionBrush != null)
        color = this.segmentSelectionColor;
      else if (this.Series.Interior != null)
        color = (this.Series.Interior as SolidColorBrush).Color;
      else if (this.isBull[index])
      {
        Color color1;
        if (bullFillColor == null)
          color1 = ((SolidColorBrush) this.Interior).Color;
        else
          color = color1 = ((SolidColorBrush) bullFillColor).Color;
        color = color1;
      }
      else
      {
        Color color2;
        if (bearFillColor == null)
          color2 = ((SolidColorBrush) this.Interior).Color;
        else
          color = color2 = ((SolidColorBrush) bearFillColor).Color;
        color = color2;
      }
      ++this.startIndex;
      float xValue = this.xValues[index];
      float num1 = this.x1Values[index];
      float num2 = this.x2Values[index];
      float num3 = this.openValue[index];
      float num4 = this.closeValue[index];
      float x2_1 = this.y_isInversed ? this.lowValue[index] : this.highValue[index];
      float x1_1 = this.y_isInversed ? this.highValue[index] : this.lowValue[index];
      float x1_2 = this.y_isInversed ? this.lowValue1[index] : this.highValue1[index];
      float x2_2 = this.y_isInversed ? this.highValue1[index] : this.lowValue1[index];
      int y1 = (int) xValue - leftThickness;
      int y2 = (int) xValue + rightThickness;
      double segmentSpacing1 = (series as ISegmentSpacing).SegmentSpacing;
      if (segmentSpacing1 > 0.0 && segmentSpacing1 <= 1.0)
      {
        double segmentSpacing2 = (this.Series as ISegmentSpacing).CalculateSegmentSpacing(segmentSpacing1, (double) num1, (double) num2);
        double segmentSpacing3 = (this.Series as ISegmentSpacing).CalculateSegmentSpacing(segmentSpacing1, (double) num2, (double) num1);
        num2 = (float) segmentSpacing2;
        num1 = (float) segmentSpacing3;
      }
      float num5 = (double) num3 == (double) num4 ? num4 + 1f : num4;
      if (this.isHollow[index])
        this.bitmap.DrawRectangle((int) ((double) width - (double) num5), (int) ((double) height - (double) num2), (int) ((double) width - (double) num3), (int) ((double) height - (double) num1), color, series.bitmapPixels);
      else
        this.bitmap.FillRectangle((int) ((double) width - (double) num5), (int) ((double) height - (double) num2), (int) ((double) width - (double) num3), (int) ((double) height - (double) num1), color, series.bitmapPixels);
      this.bitmap.FillRectangle((int) x1_2, y1, (int) x2_1, y2, color, series.bitmapPixels);
      this.bitmap.FillRectangle((int) x1_1, y1, (int) x2_2, y2, color, series.bitmapPixels);
    }
  }

  private void UpdateVisualHorizontal(
    int width,
    int height,
    Color color,
    int leftThickness,
    int rightThickness)
  {
    ChartSeries series = this.Series as ChartSeries;
    int count = this.xValues.Count;
    Brush bullFillColor = (series as FastCandleBitmapSeries).BullFillColor;
    Brush bearFillColor = (series as FastCandleBitmapSeries).BearFillColor;
    for (int index = 0; index < count; ++index)
    {
      if (this.isSeriesSelected)
        color = this.seriesSelectionColor;
      else if (series.SelectedSegmentsIndexes.Contains(this.startIndex) && (series as ISegmentSelectable).SegmentSelectionBrush != null)
        color = this.segmentSelectionColor;
      else if (this.Series.Interior != null)
        color = (this.Series.Interior as SolidColorBrush).Color;
      else if (this.isBull[index])
      {
        Color color1;
        if (bullFillColor == null)
          color1 = ((SolidColorBrush) this.Interior).Color;
        else
          color = color1 = ((SolidColorBrush) bullFillColor).Color;
        color = color1;
      }
      else
      {
        Color color2;
        if (bearFillColor == null)
          color2 = ((SolidColorBrush) this.Interior).Color;
        else
          color = color2 = ((SolidColorBrush) bearFillColor).Color;
        color = color2;
      }
      ++this.startIndex;
      float xValue = this.xValues[index];
      float num1 = this.x1Values[index];
      float num2 = this.x2Values[index];
      float y1_1 = this.openValue[index];
      float num3 = this.closeValue[index];
      float y1_2 = this.y_isInversed ? this.lowValue[index] : this.highValue[index];
      float y2_1 = this.y_isInversed ? this.highValue[index] : this.lowValue[index];
      float y2_2 = this.y_isInversed ? this.lowValue1[index] : this.highValue1[index];
      float y1_3 = this.y_isInversed ? this.highValue1[index] : this.lowValue1[index];
      float x1 = xValue - (float) leftThickness;
      float x2 = xValue + (float) rightThickness;
      double segmentSpacing1 = (series as ISegmentSpacing).SegmentSpacing;
      if (segmentSpacing1 > 0.0 && segmentSpacing1 <= 1.0)
      {
        double segmentSpacing2 = (this.Series as ISegmentSpacing).CalculateSegmentSpacing(segmentSpacing1, (double) num2, (double) num1);
        double segmentSpacing3 = (this.Series as ISegmentSpacing).CalculateSegmentSpacing(segmentSpacing1, (double) num1, (double) num2);
        num1 = (float) segmentSpacing2;
        num2 = (float) segmentSpacing3;
      }
      float y2_3 = (double) y1_1 == (double) num3 ? num3 + 1f : num3;
      if (this.isHollow[index])
        this.bitmap.DrawRectangle((int) num1, (int) y1_1, (int) num2, (int) y2_3, color, series.bitmapPixels);
      else
        this.bitmap.FillRectangle((int) num1, (int) y1_1, (int) num2, (int) y2_3, color, series.bitmapPixels);
      this.bitmap.FillRectangle((int) x1, (int) y1_2, (int) x2, (int) y2_2, color, series.bitmapPixels);
      this.bitmap.FillRectangle((int) x1, (int) y1_3, (int) x2, (int) y2_1, color, series.bitmapPixels);
    }
  }
}
