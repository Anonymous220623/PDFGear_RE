// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.FastColumnBitmapSegment
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

public class FastColumnBitmapSegment : ChartSegment
{
  internal IList<double> y1ChartVals;
  private List<int> actualIndexes;
  private WriteableBitmap bitmap;
  private IList<double> x1ChartVals;
  private IList<double> x2ChartVals;
  private IList<double> y2ChartVals;
  private Color seriesSelectionColor = Colors.Transparent;
  private Color segmentSelectionColor = Colors.Transparent;
  private bool isSeriesSelected;
  private List<float> x1Values;
  private List<float> x2Values;
  private List<float> y1Values;
  private List<float> y2Values;
  private int startIndex;

  public FastColumnBitmapSegment()
  {
    this.x1Values = new List<float>();
    this.x2Values = new List<float>();
    this.y1Values = new List<float>();
    this.y2Values = new List<float>();
    this.actualIndexes = new List<int>();
  }

  public FastColumnBitmapSegment(ChartSeries series)
  {
    this.x1Values = new List<float>();
    this.x2Values = new List<float>();
    this.y1Values = new List<float>();
    this.y2Values = new List<float>();
    this.actualIndexes = new List<int>();
    this.Series = (ChartSeriesBase) series;
  }

  public FastColumnBitmapSegment(
    IList<double> x1Values,
    IList<double> y1Values,
    IList<double> x2Values,
    IList<double> y2Values,
    ChartSeries series)
    : this(series)
  {
    this.Series = (ChartSeriesBase) series;
    if (this.Series.ActualXAxis is CategoryAxis && !(this.Series.ActualXAxis as CategoryAxis).IsIndexed)
      this.Item = (object) series.GroupedActualData;
    else
      this.Item = (object) series.ActualData;
  }

  public double XData { get; internal set; }

  public double YData { get; internal set; }

  public override UIElement CreateVisual(Size size)
  {
    this.bitmap = (this.Series as ChartSeries).Area.GetFastRenderSurface();
    return (UIElement) null;
  }

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
    double end = x2Values.Max();
    double num1 = y1Values.Max();
    double start = x1Values.Min();
    double d = this.y1ChartVals.Min();
    double num2;
    if (double.IsNaN(d))
    {
      IEnumerable<double> source = this.y1ChartVals.Where<double>((Func<double, bool>) (e => !double.IsNaN(e)));
      num2 = !source.Any<double>() ? 0.0 : source.Min();
    }
    else
      num2 = d;
    double num3 = this.y2ChartVals.Min();
    this.XRange = new DoubleRange(start, end);
    this.YRange = new DoubleRange(num2 >= num3 || num1 <= num3 ? num3 : num2, num3 < num1 ? num1 : num2);
  }

  public override UIElement GetRenderedVisual() => (UIElement) null;

  public override void Update(IChartTransformer transformer)
  {
    this.bitmap = (this.Series as ChartSeries).Area.GetFastRenderSurface();
    if (transformer == null || this.Series.DataCount == 0)
      return;
    ChartTransform.ChartCartesianTransformer cartesianTransformer = transformer as ChartTransform.ChartCartesianTransformer;
    this.x_isInversed = cartesianTransformer.XAxis.IsInversed;
    this.y_isInversed = cartesianTransformer.YAxis.IsInversed;
    this.x1Values.Clear();
    this.x2Values.Clear();
    this.y1Values.Clear();
    this.y2Values.Clear();
    this.actualIndexes.Clear();
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
    bool flag = series.Palette != ChartColorPalette.None && series.Interior == null;
    SfChart area = series.Area;
    Color color1 = ChartSegment.GetColor(this.Interior);
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
    int count = this.x1Values.Count;
    if (this.bitmap != null && this.x1Values.Count != 0)
    {
      this.bitmap.BeginWrite();
      switch (series)
      {
        case FastColumnBitmapSeries _:
        case FastStackingColumnBitmapSeries _:
        case FastBarBitmapSeries _:
        case MACDTechnicalIndicator _:
          for (int index = 0; index < count; ++index)
          {
            if (!double.IsNaN((double) this.y1Values[index]))
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
                color2 = !flag ? color1 : ChartSegment.GetColor(series.ColorModel.GetBrush(this.actualIndexes[index]));
              ++this.startIndex;
              float num1 = this.x1Values[index];
              float num2 = this.x2Values[index];
              float num3 = this.y1ChartVals[index] > 0.0 ? this.y1Values[index] : this.y2Values[index];
              float num4 = this.y1ChartVals[index] > 0.0 ? this.y2Values[index] : this.y1Values[index];
              if ((double) num3 != 0.0 || (double) num4 != 0.0)
              {
                if (!(series is MACDTechnicalIndicator))
                {
                  double segmentSpacing1 = (series as ISegmentSpacing).SegmentSpacing;
                  if (segmentSpacing1 > 0.0 && segmentSpacing1 <= 1.0)
                  {
                    double segmentSpacing2 = (this.Series as ISegmentSpacing).CalculateSegmentSpacing(segmentSpacing1, (double) num2, (double) num1);
                    double segmentSpacing3 = (this.Series as ISegmentSpacing).CalculateSegmentSpacing(segmentSpacing1, (double) num1, (double) num2);
                    num1 = (float) segmentSpacing2;
                    num2 = (float) segmentSpacing3;
                  }
                }
                if ((double) num3 < (double) num4)
                {
                  this.bitmap.FillRectangle((int) num1, (int) num3, (int) num2, (int) num4, color2, series.bitmapPixels);
                  this.Series.bitmapRects.Add(new Rect(new Point((double) num1, (double) num3), new Point((double) num2, (double) num4)));
                }
                else
                {
                  this.bitmap.FillRectangle((int) num1, (int) num4, (int) num2, (int) num3, color2, series.bitmapPixels);
                  this.Series.bitmapRects.Add(new Rect(new Point((double) num1, (double) num4), new Point((double) num2, (double) num3)));
                }
              }
            }
          }
          break;
      }
      this.bitmap.EndWrite();
    }
    series.Area.CanRenderToBuffer = true;
    this.x1Values.Clear();
    this.y1Values.Clear();
    this.y2Values.Clear();
    this.x2Values.Clear();
    this.actualIndexes.Clear();
  }

  protected override void SetVisualBindings(Shape element)
  {
    base.SetVisualBindings(element);
    element.SetBinding(Shape.StrokeProperty, (BindingBase) new Binding()
    {
      Source = (object) this,
      Path = new PropertyPath("Stroke", new object[0])
    });
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
      this.startIndex = num4;
      for (int index = num4; index <= num2; ++index)
        this.AddDataPoint(cartesianTransformer, index);
    }
    else if (this.Series.isLinearData)
    {
      double start = xaxis.VisibleRange.Start;
      double end = xaxis.VisibleRange.End;
      this.startIndex = 0;
      int index1 = this.x1ChartVals.Count - 1;
      double newBase = cartesianTransformer.XAxis.IsLogarithmic ? (cartesianTransformer.XAxis as LogarithmicAxis).LogarithmicBase : 0.0;
      int index2;
      for (index2 = 1; index2 < index1; ++index2)
      {
        double a = this.x1ChartVals[index2];
        if (cartesianTransformer.XAxis.IsLogarithmic)
          a = Math.Log(a, newBase);
        if (a >= start && a <= end)
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
    this.GetXYPoints(index, out x1Value, out x2Value, out y1Value, out y2Value);
    Point visible1 = cartesianTransformer.TransformToVisible(x1Value, y1Value);
    Point visible2 = cartesianTransformer.TransformToVisible(x2Value, y2Value);
    this.x1Values.Add((float) visible1.X);
    this.x2Values.Add((float) visible2.X);
    this.y1Values.Add((float) visible1.Y);
    this.y2Values.Add((float) visible2.Y);
    this.actualIndexes.Add(index);
  }

  private void InsertDataPoint(
    ChartTransform.ChartCartesianTransformer cartesianTransformer,
    int index)
  {
    double x1Value = 0.0;
    double x2Value = 0.0;
    double y2Value = 0.0;
    double y1Value = 0.0;
    this.GetXYPoints(index, out x1Value, out x2Value, out y1Value, out y2Value);
    Point visible1 = cartesianTransformer.TransformToVisible(x1Value, y1Value);
    Point visible2 = cartesianTransformer.TransformToVisible(x2Value, y2Value);
    this.x1Values.Insert(0, (float) visible1.X);
    this.x2Values.Insert(0, (float) visible2.X);
    this.y1Values.Insert(0, (float) visible1.Y);
    this.y2Values.Insert(0, (float) visible2.Y);
    this.actualIndexes.Insert(0, index);
  }

  private void GetXYPoints(
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
