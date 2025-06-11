// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.FastHiLoOpenCloseSegment
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

public class FastHiLoOpenCloseSegment : ChartSegment
{
  private WriteableBitmap bitmap;
  private IList<double> xChartVals;
  private IList<double> yHiChartVals;
  private IList<double> yLoChartVals;
  private IList<double> yOpenChartVals;
  private IList<double> yCloseChartVals;
  private DoubleRange sbsInfo;
  private double center;
  private double Left;
  private double Right;
  private Color seriesSelectionColor = Colors.Transparent;
  private Color segmentSelectionColor = Colors.Transparent;
  private bool isSeriesSelected;
  private List<float> xValues;
  private List<float> yHiValues;
  private List<float> yLoValues;
  private List<float> yOpenStartValues;
  private List<float> yOpenEndValues;
  private List<float> yCloseValues;
  private List<float> yCloseEndValues;
  private List<bool> isBull;
  private int startIndex;

  public FastHiLoOpenCloseSegment()
  {
    this.xValues = new List<float>();
    this.yHiValues = new List<float>();
    this.yLoValues = new List<float>();
    this.yOpenStartValues = new List<float>();
    this.yOpenEndValues = new List<float>();
    this.yCloseValues = new List<float>();
    this.yCloseEndValues = new List<float>();
    this.isBull = new List<bool>();
  }

  public FastHiLoOpenCloseSegment(AdornmentSeries series)
  {
    this.xValues = new List<float>();
    this.yHiValues = new List<float>();
    this.yLoValues = new List<float>();
    this.yOpenStartValues = new List<float>();
    this.yOpenEndValues = new List<float>();
    this.yCloseValues = new List<float>();
    this.yCloseEndValues = new List<float>();
    this.isBull = new List<bool>();
    this.Series = (ChartSeriesBase) series;
  }

  public FastHiLoOpenCloseSegment(
    List<double> xValues,
    IList<double> highValues,
    IList<double> lowValues,
    IList<double> openValues,
    IList<double> closeValues,
    AdornmentSeries series)
    : this(series)
  {
  }

  public override void OnSizeChanged(Size size)
  {
    this.bitmap = (this.Series as AdornmentSeries).Area.GetFastRenderSurface();
  }

  public override UIElement GetRenderedVisual() => (UIElement) null;

  public override void Update(IChartTransformer transformer)
  {
    AdornmentSeries series = this.Series as AdornmentSeries;
    this.bitmap = series.Area.GetFastRenderSurface();
    if (transformer == null || series.DataCount <= 0)
      return;
    ChartTransform.ChartCartesianTransformer cartesianTransformer = transformer as ChartTransform.ChartCartesianTransformer;
    this.xValues.Clear();
    this.yHiValues.Clear();
    this.yLoValues.Clear();
    this.yOpenStartValues.Clear();
    this.yOpenEndValues.Clear();
    this.yCloseValues.Clear();
    this.yCloseEndValues.Clear();
    this.isBull.Clear();
    this.x_isInversed = cartesianTransformer.XAxis.IsInversed;
    this.y_isInversed = cartesianTransformer.YAxis.IsInversed;
    this.sbsInfo = series.GetSideBySideInfo((ChartSeriesBase) series);
    this.center = this.sbsInfo.Median;
    this.Left = this.sbsInfo.Start;
    this.Right = this.sbsInfo.End;
    this.CalculatePoint(cartesianTransformer);
    this.UpdateVisual(true);
  }

  public override void SetData(
    IList<double> xValues,
    IList<double> yHiValues,
    IList<double> yLowValues,
    IList<double> yOpenValues,
    IList<double> yCloseValues)
  {
    DoubleRange sideBySideInfo = this.Series.GetSideBySideInfo(this.Series);
    this.xChartVals = xValues;
    this.yHiChartVals = yHiValues;
    this.yLoChartVals = yLowValues;
    this.yOpenChartVals = yOpenValues;
    this.yCloseChartVals = yCloseValues;
    List<double> source1 = new List<double>();
    source1.AddRange((IEnumerable<double>) yHiValues);
    source1.AddRange((IEnumerable<double>) yLowValues);
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
    double num1;
    double num2;
    if (this.Series.IsIndexed)
    {
      num1 = !(this.Series.ActualXAxis is CategoryAxis) || (this.Series.ActualXAxis as CategoryAxis).IsIndexed ? (double) (this.Series.DataCount - 1) : this.Series.GroupedXValuesIndexes.Max();
      num2 = 0.0;
    }
    else
    {
      num1 = this.xChartVals.Max();
      num2 = this.xChartVals.Min();
    }
    double end = source1.Max();
    this.XRange = new DoubleRange(num2 + sideBySideInfo.Start, num1 + sideBySideInfo.End);
    this.YRange = new DoubleRange(start, end);
  }

  public override UIElement CreateVisual(Size size)
  {
    this.bitmap = (this.Series as AdornmentSeries).Area.GetFastRenderSurface();
    return (UIElement) null;
  }

  internal Color GetSegmentBrush(int index)
  {
    AdornmentSeries series = this.Series as AdornmentSeries;
    Color color1;
    Color segmentBrush;
    if (this.isBull.Count > index && this.isBull[index])
    {
      Color color2;
      if ((series as FastHiLoOpenCloseBitmapSeries).BullFillColor == null)
        color2 = ((SolidColorBrush) this.Interior).Color;
      else
        color1 = color2 = ((SolidColorBrush) (series as FastHiLoOpenCloseBitmapSeries).BullFillColor).Color;
      segmentBrush = color2;
    }
    else
    {
      Color color3;
      if ((series as FastHiLoOpenCloseBitmapSeries).BearFillColor == null)
        color3 = ((SolidColorBrush) this.Interior).Color;
      else
        color1 = color3 = ((SolidColorBrush) (series as FastHiLoOpenCloseBitmapSeries).BearFillColor).Color;
      segmentBrush = color3;
    }
    return segmentBrush;
  }

  internal void UpdateVisual(bool updateHiLoLine)
  {
    AdornmentSeries series = this.Series as AdornmentSeries;
    if (this.bitmap != null && this.xValues.Count > 0)
    {
      int width = (int) series.Area.SeriesClipRect.Width;
      int height = (int) series.Area.SeriesClipRect.Height;
      int leftThickness = (int) series.StrokeThickness / 2;
      int rightThickness = series.StrokeThickness % 2.0 == 0.0 ? (int) (series.StrokeThickness / 2.0) : (int) (series.StrokeThickness / 2.0 + 1.0);
      this.bitmap.BeginWrite();
      if (series is FastHiLoOpenCloseBitmapSeries)
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
          this.UpdateVisualHorizontal(width, height, leftThickness, rightThickness);
        else
          this.UpdateVisualVertical(width, height, leftThickness, rightThickness);
      }
      this.bitmap.EndWrite();
    }
    series.Area.CanRenderToBuffer = true;
    this.xValues.Clear();
    this.yHiValues.Clear();
    this.yLoValues.Clear();
    this.yOpenStartValues.Clear();
    this.yOpenEndValues.Clear();
    this.yCloseValues.Clear();
    this.yCloseEndValues.Clear();
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

  private void AddHorizontalPoint(
    ChartTransform.ChartCartesianTransformer cartesianTransformer,
    int index,
    IList<double> values)
  {
    double yHiChartVal = this.yHiChartVals[index];
    double yLoChartVal = this.yLoChartVals[index];
    double[] numArray = this.AlignHiLoSegment(this.yOpenChartVals[index], this.yCloseChartVals[index], yHiChartVal, yLoChartVal);
    double highValues = numArray[0];
    double lowValues = numArray[1];
    Point hiPoint;
    Point loPoint;
    Point startopenpoint;
    Point endopenpoint;
    Point endclosepoint;
    this.GetPoints(cartesianTransformer, index, out hiPoint, out loPoint, out startopenpoint, out endopenpoint, out Point _, out endclosepoint, highValues, lowValues);
    this.xValues.Add((float) hiPoint.X);
    this.yHiValues.Add((float) hiPoint.Y);
    this.yLoValues.Add((float) loPoint.Y);
    this.yOpenStartValues.Add((float) startopenpoint.Y);
    this.yOpenEndValues.Add((float) endopenpoint.X);
    this.yCloseValues.Add((float) endclosepoint.Y);
    this.yCloseEndValues.Add((float) endclosepoint.X);
    this.isBull.Add(index == 0 || (this.Series as FastHiLoOpenCloseBitmapSeries).ComparisonMode == FinancialPrice.None ? this.yOpenChartVals[index] <= this.yCloseChartVals[index] : values[index] >= values[index - 1]);
  }

  private void InsertHorizontalPoint(
    ChartTransform.ChartCartesianTransformer cartesianTransformer,
    int index,
    IList<double> values)
  {
    double yHiChartVal = this.yHiChartVals[index];
    double yLoChartVal = this.yLoChartVals[index];
    double[] numArray = this.AlignHiLoSegment(this.yOpenChartVals[index], this.yCloseChartVals[index], yHiChartVal, yLoChartVal);
    double highValues = numArray[0];
    double lowValues = numArray[1];
    Point hiPoint;
    Point loPoint;
    Point startopenpoint;
    Point endopenpoint;
    Point endclosepoint;
    this.GetPoints(cartesianTransformer, index, out hiPoint, out loPoint, out startopenpoint, out endopenpoint, out Point _, out endclosepoint, highValues, lowValues);
    this.xValues.Insert(0, (float) hiPoint.X);
    this.yHiValues.Insert(0, (float) hiPoint.Y);
    this.yLoValues.Insert(0, (float) loPoint.Y);
    this.yOpenStartValues.Insert(0, (float) startopenpoint.Y);
    this.yOpenEndValues.Insert(0, (float) endopenpoint.X);
    this.yCloseValues.Insert(0, (float) endclosepoint.Y);
    this.yCloseEndValues.Insert(0, (float) endclosepoint.X);
    this.isBull.Insert(0, index == 0 || (this.Series as FastHiLoOpenCloseBitmapSeries).ComparisonMode == FinancialPrice.None ? this.yOpenChartVals[index] <= this.yCloseChartVals[index] : values[index] >= values[index - 1]);
  }

  private void GetPoints(
    ChartTransform.ChartCartesianTransformer cartesianTransformer,
    int index,
    out Point hiPoint,
    out Point loPoint,
    out Point startopenpoint,
    out Point endopenpoint,
    out Point startclosepoint,
    out Point endclosepoint,
    double highValues,
    double lowValues)
  {
    if (this.Series.IsIndexed)
    {
      hiPoint = cartesianTransformer.TransformToVisible((double) index + this.center, highValues);
      loPoint = cartesianTransformer.TransformToVisible(this.xChartVals[index] + this.center, lowValues);
      startopenpoint = cartesianTransformer.TransformToVisible(this.xChartVals[index] + this.center, this.yOpenChartVals[index]);
      endopenpoint = cartesianTransformer.TransformToVisible(this.xChartVals[index] + this.Left, this.yOpenChartVals[index]);
      startclosepoint = cartesianTransformer.TransformToVisible((double) index + this.center, this.yCloseChartVals[index]);
      endclosepoint = cartesianTransformer.TransformToVisible((double) index + this.Right, this.yCloseChartVals[index]);
    }
    else
    {
      hiPoint = cartesianTransformer.TransformToVisible(this.xChartVals[index] + this.center, highValues);
      loPoint = cartesianTransformer.TransformToVisible(this.xChartVals[index] + this.center, lowValues);
      startopenpoint = cartesianTransformer.TransformToVisible(this.xChartVals[index] + this.center, this.yOpenChartVals[index]);
      endopenpoint = cartesianTransformer.TransformToVisible(this.xChartVals[index] + this.Left, this.yOpenChartVals[index]);
      startclosepoint = cartesianTransformer.TransformToVisible(this.xChartVals[index] + this.center, this.yCloseChartVals[index]);
      endclosepoint = cartesianTransformer.TransformToVisible(this.xChartVals[index] + this.Right, this.yCloseChartVals[index]);
    }
  }

  private void AddVerticalPoint(
    ChartTransform.ChartCartesianTransformer cartesianTransformer,
    int index,
    IList<double> values)
  {
    double yHiChartVal = this.yHiChartVals[index];
    double yLoChartVal = this.yLoChartVals[index];
    double[] numArray = this.AlignHiLoSegment(this.yOpenChartVals[index], this.yCloseChartVals[index], yHiChartVal, yLoChartVal);
    double highValues = numArray[0];
    double lowValues = numArray[1];
    Point hiPoint;
    Point loPoint;
    Point startopenpoint;
    Point endopenpoint;
    Point endclosepoint;
    this.GetPoints(cartesianTransformer, index, out hiPoint, out loPoint, out startopenpoint, out endopenpoint, out Point _, out endclosepoint, highValues, lowValues);
    this.xValues.Add((float) hiPoint.Y);
    this.yHiValues.Add((float) hiPoint.X);
    this.yLoValues.Add((float) loPoint.X);
    this.yOpenStartValues.Add((float) startopenpoint.X);
    this.yOpenEndValues.Add((float) endopenpoint.Y);
    this.yCloseValues.Add((float) endclosepoint.X);
    this.yCloseEndValues.Add((float) endclosepoint.Y);
    this.isBull.Add(index == 0 || (this.Series as FastHiLoOpenCloseBitmapSeries).ComparisonMode == FinancialPrice.None ? this.yOpenChartVals[index] <= this.yCloseChartVals[index] : values[index] >= values[index - 1]);
  }

  private void InsertVerticalPoint(
    ChartTransform.ChartCartesianTransformer cartesianTransformer,
    int index,
    IList<double> values)
  {
    double yHiChartVal = this.yHiChartVals[index];
    double yLoChartVal = this.yLoChartVals[index];
    double[] numArray = this.AlignHiLoSegment(this.yOpenChartVals[index], this.yCloseChartVals[index], yHiChartVal, yLoChartVal);
    double highValues = numArray[0];
    double lowValues = numArray[1];
    Point hiPoint;
    Point loPoint;
    Point startopenpoint;
    Point endopenpoint;
    Point endclosepoint;
    this.GetPoints(cartesianTransformer, index, out hiPoint, out loPoint, out startopenpoint, out endopenpoint, out Point _, out endclosepoint, highValues, lowValues);
    this.xValues.Insert(0, (float) hiPoint.Y);
    this.yHiValues.Insert(0, (float) hiPoint.X);
    this.yLoValues.Insert(0, (float) loPoint.X);
    this.yOpenStartValues.Insert(0, (float) startopenpoint.X);
    this.yOpenEndValues.Insert(0, (float) endopenpoint.Y);
    this.yCloseValues.Insert(0, (float) endclosepoint.X);
    this.yCloseEndValues.Insert(0, (float) endclosepoint.Y);
    this.isBull.Insert(0, index == 0 || (this.Series as FastHiLoOpenCloseBitmapSeries).ComparisonMode == FinancialPrice.None ? this.yOpenChartVals[index] <= this.yCloseChartVals[index] : values[index] >= values[index - 1]);
  }

  private void CalculatePoint(
    ChartTransform.ChartCartesianTransformer cartesianTransformer)
  {
    AdornmentSeries series = this.Series as AdornmentSeries;
    IList<double> comparisionModeValues = (this.Series as FastHiLoOpenCloseBitmapSeries).GetComparisionModeValues();
    double segmentSpacing1 = (this.Series as ISegmentSpacing).SegmentSpacing;
    double segmentSpacing2 = (this.Series as ISegmentSpacing).CalculateSegmentSpacing(segmentSpacing1, this.Right, this.Left);
    double segmentSpacing3 = (this.Series as ISegmentSpacing).CalculateSegmentSpacing(segmentSpacing1, this.Left, this.Right);
    if (segmentSpacing1 > 0.0 && segmentSpacing1 <= 1.0)
    {
      this.Left = segmentSpacing2;
      this.Right = segmentSpacing3;
    }
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
      if (!series.IsActualTransposed)
      {
        for (int index = num4; index <= num2; ++index)
          this.AddHorizontalPoint(cartesianTransformer, index, comparisionModeValues);
      }
      else
      {
        for (int index = num4; index <= num2; ++index)
          this.AddVerticalPoint(cartesianTransformer, index, comparisionModeValues);
      }
    }
    else if (series.isLinearData)
    {
      double start = xaxis.VisibleRange.Start;
      double end = xaxis.VisibleRange.End;
      this.startIndex = 0;
      int index1 = this.xChartVals.Count - 1;
      if (!series.IsActualTransposed)
      {
        double newBase = cartesianTransformer.XAxis.IsLogarithmic ? (cartesianTransformer.XAxis as LogarithmicAxis).LogarithmicBase : 0.0;
        int index2;
        for (index2 = 1; index2 < index1; ++index2)
        {
          double a = this.xChartVals[index2];
          if (cartesianTransformer.XAxis.IsLogarithmic)
            a = Math.Log(a, newBase);
          if (a <= end && a >= start)
            this.AddHorizontalPoint(cartesianTransformer, index2, comparisionModeValues);
          else if (a < start)
            this.startIndex = index2;
          else if (a > end)
          {
            this.AddHorizontalPoint(cartesianTransformer, index2, comparisionModeValues);
            break;
          }
        }
        this.InsertHorizontalPoint(cartesianTransformer, this.startIndex, comparisionModeValues);
        if (index2 != index1)
          return;
        this.AddHorizontalPoint(cartesianTransformer, index1, comparisionModeValues);
      }
      else
      {
        double newBase = cartesianTransformer.XAxis.IsLogarithmic ? (cartesianTransformer.XAxis as LogarithmicAxis).LogarithmicBase : 0.0;
        int index3;
        for (index3 = 1; index3 < index1; ++index3)
        {
          double a = this.xChartVals[index3];
          if (cartesianTransformer.XAxis.IsLogarithmic)
            a = Math.Log(a, newBase);
          if (a <= end && a >= start)
            this.AddVerticalPoint(cartesianTransformer, index3, comparisionModeValues);
          else if (a < start)
            this.startIndex = index3;
          else if (a > end)
          {
            this.AddVerticalPoint(cartesianTransformer, index3, comparisionModeValues);
            break;
          }
        }
        this.InsertVerticalPoint(cartesianTransformer, this.startIndex, comparisionModeValues);
        if (index3 != index1)
          return;
        this.AddVerticalPoint(cartesianTransformer, index1, comparisionModeValues);
      }
    }
    else
    {
      this.startIndex = 0;
      for (int index = 0; index < this.Series.DataCount; ++index)
      {
        if (!series.IsActualTransposed)
          this.AddHorizontalPoint(cartesianTransformer, index, comparisionModeValues);
        else
          this.AddVerticalPoint(cartesianTransformer, index, comparisionModeValues);
      }
    }
  }

  private void UpdateVisualVertical(int width, int height, int leftThickness, int rightThickness)
  {
    AdornmentSeries series = this.Series as AdornmentSeries;
    Brush bullFillColor = (series as FastHiLoOpenCloseBitmapSeries).BullFillColor;
    Brush bearFillColor = (series as FastHiLoOpenCloseBitmapSeries).BearFillColor;
    for (int index = 0; index < this.xValues.Count; ++index)
    {
      Color color1;
      Color color2;
      if (this.isSeriesSelected)
        color1 = this.seriesSelectionColor;
      else if (series.SelectedSegmentsIndexes.Contains(this.startIndex) && (series as ISegmentSelectable).SegmentSelectionBrush != null)
        color1 = this.segmentSelectionColor;
      else if (this.Series.Interior != null)
        color1 = (this.Series.Interior as SolidColorBrush).Color;
      else if (this.isBull[index])
      {
        Color color3;
        if (bullFillColor == null)
          color3 = ((SolidColorBrush) this.Interior).Color;
        else
          color2 = color3 = ((SolidColorBrush) bullFillColor).Color;
        color1 = color3;
      }
      else
      {
        Color color4;
        if (bearFillColor == null)
          color4 = ((SolidColorBrush) this.Interior).Color;
        else
          color2 = color4 = ((SolidColorBrush) bearFillColor).Color;
        color1 = color4;
      }
      ++this.startIndex;
      float xValue = this.xValues[index];
      float num1 = this.y_isInversed ? this.yLoValues[index] : this.yHiValues[index];
      float num2 = this.y_isInversed ? this.yHiValues[index] : this.yLoValues[index];
      float d1 = this.x_isInversed ? this.yCloseValues[index] : this.yOpenStartValues[index];
      float y2_1 = this.x_isInversed ? this.yCloseEndValues[index] : this.yOpenEndValues[index];
      float d2 = this.x_isInversed ? this.yOpenStartValues[index] : this.yCloseValues[index];
      float y1_1 = this.x_isInversed ? this.yOpenEndValues[index] : this.yCloseEndValues[index];
      int y1_2 = (int) xValue - leftThickness;
      int y2_2 = (int) xValue + rightThickness;
      if (!double.IsNaN((double) num1) && !double.IsNaN((double) num2))
        this.bitmap.FillRectangle((int) num2, y1_2, (int) num1, y2_2, color1, series.bitmapPixels);
      if (!double.IsNaN((double) d1))
      {
        int x1 = (int) d1 - leftThickness;
        int x2 = (int) d1 + rightThickness;
        this.bitmap.FillRectangle(x1, (int) xValue + leftThickness, x2, (int) y2_1, color1, series.bitmapPixels);
      }
      if (!double.IsNaN((double) d2))
      {
        int x1 = (int) d2 - leftThickness;
        int x2 = (int) d2 + rightThickness;
        this.bitmap.FillRectangle(x1, (int) y1_1, x2, (int) xValue - leftThickness, color1, series.bitmapPixels);
      }
    }
  }

  private void UpdateVisualHorizontal(
    int width,
    int height,
    int leftThickness,
    int rightThickness)
  {
    AdornmentSeries series = this.Series as AdornmentSeries;
    Brush bullFillColor = (series as FastHiLoOpenCloseBitmapSeries).BullFillColor;
    Brush bearFillColor = (series as FastHiLoOpenCloseBitmapSeries).BearFillColor;
    for (int index = 0; index < this.xValues.Count; ++index)
    {
      Color color1;
      Color color2;
      if (this.isSeriesSelected)
        color1 = this.seriesSelectionColor;
      else if (series.SelectedSegmentsIndexes.Contains(this.startIndex) && (series as ISegmentSelectable).SegmentSelectionBrush != null)
        color1 = this.segmentSelectionColor;
      else if (this.Series.Interior != null)
        color1 = (this.Series.Interior as SolidColorBrush).Color;
      else if (this.isBull[index])
      {
        Color color3;
        if (bullFillColor == null)
          color3 = ((SolidColorBrush) this.Interior).Color;
        else
          color2 = color3 = ((SolidColorBrush) bullFillColor).Color;
        color1 = color3;
      }
      else
      {
        Color color4;
        if (bearFillColor == null)
          color4 = ((SolidColorBrush) this.Interior).Color;
        else
          color2 = color4 = ((SolidColorBrush) bearFillColor).Color;
        color1 = color4;
      }
      ++this.startIndex;
      float xValue = this.xValues[index];
      float num1 = this.y_isInversed ? this.yLoValues[index] : this.yHiValues[index];
      float num2 = this.y_isInversed ? this.yHiValues[index] : this.yLoValues[index];
      float d1 = this.x_isInversed ? this.yCloseValues[index] : this.yOpenStartValues[index];
      float x1_1 = this.x_isInversed ? this.yCloseEndValues[index] : this.yOpenEndValues[index];
      float d2 = this.x_isInversed ? this.yOpenStartValues[index] : this.yCloseValues[index];
      float x2_1 = this.x_isInversed ? this.yOpenEndValues[index] : this.yCloseEndValues[index];
      int x1_2 = (int) xValue - leftThickness;
      int x2_2 = (int) xValue + rightThickness;
      if (!double.IsNaN((double) num1) && !double.IsNaN((double) num2))
      {
        if ((double) num1 > (double) num2)
          this.bitmap.FillRectangle(x1_2, (int) num2, x2_2, (int) num1, color1, series.bitmapPixels);
        else
          this.bitmap.FillRectangle(x1_2, (int) num1, x2_2, (int) num2, color1, series.bitmapPixels);
      }
      if (!double.IsNaN((double) d1))
      {
        int y1 = (int) d1 - leftThickness;
        int y2 = (int) d1 + rightThickness;
        this.bitmap.FillRectangle((int) x1_1, y1, (int) xValue - leftThickness, y2, color1, series.bitmapPixels);
      }
      if (!double.IsNaN((double) d2))
      {
        int y1 = (int) d2 - leftThickness;
        int y2 = (int) d2 + rightThickness;
        if (!double.IsNaN((double) num1) && !double.IsNaN((double) num2))
          this.bitmap.FillRectangle((int) xValue + leftThickness, y1, (int) x2_1, y2, color1, series.bitmapPixels);
      }
    }
  }
}
