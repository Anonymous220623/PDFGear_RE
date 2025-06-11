// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.XyDataSeries
// Assembly: Syncfusion.SfChart.WPF, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 279D70C6-7936-4F3E-B6CF-79EFEF734826
// Assembly location: C:\Program Files\PDFgear\Syncfusion.SfChart.WPF.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media.Imaging;

#nullable disable
namespace Syncfusion.UI.Xaml.Charts;

public abstract class XyDataSeries : CartesianSeries
{
  public static readonly DependencyProperty YBindingPathProperty = DependencyProperty.Register(nameof (YBindingPath), typeof (string), typeof (XyDataSeries), new PropertyMetadata((object) null, new PropertyChangedCallback(XyDataSeries.OnYBindingPathChanged)));

  public XyDataSeries() => this.YValues = (IList<double>) new List<double>();

  public string YBindingPath
  {
    get => (string) this.GetValue(XyDataSeries.YBindingPathProperty);
    set => this.SetValue(XyDataSeries.YBindingPathProperty, (object) value);
  }

  protected internal IList<double> YValues { get; set; }

  internal void GenerateColumnPixels()
  {
    if (double.IsNaN(this.dataPoint.YData))
      return;
    WriteableBitmap fastRenderSurface = this.Area.fastRenderSurface;
    IChartTransformer transformer = this.CreateTransformer(new Size(this.Area.SeriesClipRect.Width, this.Area.SeriesClipRect.Height), true);
    bool isInversed1 = this.ActualXAxis.IsInversed;
    bool isInversed2 = this.ActualYAxis.IsInversed;
    DoubleRange sideBySideInfo = this.GetSideBySideInfo((ChartSeriesBase) this);
    double num1 = this.ActualXAxis != null ? this.ActualXAxis.Origin : 0.0;
    double x1 = isInversed1 ? this.dataPoint.XData + sideBySideInfo.End : this.dataPoint.XData + sideBySideInfo.Start;
    double x2 = isInversed1 ? this.dataPoint.XData + sideBySideInfo.Start : this.dataPoint.XData + sideBySideInfo.End;
    double y1 = isInversed2 ? num1 : this.dataPoint.YData;
    double y2 = isInversed2 ? this.dataPoint.YData : num1;
    Point visible1 = transformer.TransformToVisible(x1, y1);
    Point visible2 = transformer.TransformToVisible(x2, y2);
    double num2 = visible1.X;
    double num3 = visible2.X;
    double num4 = y1 > 0.0 ? visible1.Y : visible2.Y;
    double num5 = y1 > 0.0 ? visible2.Y : visible1.Y;
    if (this is ISegmentSpacing segmentSpacing)
    {
      double segmentSpacing1 = segmentSpacing.SegmentSpacing;
      if (segmentSpacing1 > 0.0 && segmentSpacing1 <= 1.0)
      {
        double segmentSpacing2 = segmentSpacing.CalculateSegmentSpacing(segmentSpacing1, num3, num2);
        double segmentSpacing3 = segmentSpacing.CalculateSegmentSpacing(segmentSpacing1, num2, num3);
        num2 = segmentSpacing2;
        num3 = segmentSpacing3;
      }
    }
    this.selectedSegmentPixels.Clear();
    if (num4 < num5)
      this.selectedSegmentPixels = fastRenderSurface.GetRectangle((int) num2, (int) num4, (int) num3, (int) num5, this.selectedSegmentPixels);
    else
      this.selectedSegmentPixels = fastRenderSurface.GetRectangle((int) num2, (int) num5, (int) num3, (int) num4, this.selectedSegmentPixels);
  }

  internal void GenerateBarPixels()
  {
    WriteableBitmap fastRenderSurface = this.Area.fastRenderSurface;
    ChartTransform.ChartCartesianTransformer transformer = this.CreateTransformer(new Size(this.Area.SeriesClipRect.Width, this.Area.SeriesClipRect.Height), true) as ChartTransform.ChartCartesianTransformer;
    DoubleRange sideBySideInfo = this.GetSideBySideInfo((ChartSeriesBase) this);
    double a1 = this.dataPoint.XData + sideBySideInfo.Start;
    double a2 = this.dataPoint.XData + sideBySideInfo.End;
    double ydata = this.dataPoint.YData;
    double a3 = this.ActualXAxis != null ? this.ActualXAxis.Origin : 0.0;
    double num1 = transformer.XAxis.VisibleRange.Start;
    double num2 = transformer.XAxis.VisibleRange.End;
    double num3 = transformer.YAxis.VisibleRange.Start;
    double num4 = transformer.YAxis.VisibleRange.End;
    double height1 = transformer.XAxis.RenderedRect.Height;
    double width1 = transformer.YAxis.RenderedRect.Width;
    double num5 = this.Area.SeriesClipRect.Right - transformer.YAxis.RenderedRect.Right;
    double num6 = this.Area.SeriesClipRect.Bottom - transformer.XAxis.RenderedRect.Bottom;
    Size size = new Size(height1, width1);
    bool flag = transformer.XAxis.IsLogarithmic || transformer.YAxis.IsLogarithmic;
    if (this.ActualXAxis.IsInversed)
    {
      double num7 = num1;
      num1 = num2;
      num2 = num7;
    }
    if (this.ActualYAxis.IsInversed)
    {
      double num8 = num3;
      num3 = num4;
      num4 = num8;
    }
    float num9;
    float num10;
    float num11;
    float num12;
    if (!flag)
    {
      double num13 = this.ActualXAxis.IsInversed ? (a2 < num2 ? num2 : a2) : (a1 < num1 ? num1 : a1);
      double num14 = this.ActualXAxis.IsInversed ? (a1 > num1 ? num1 : a1) : (a2 > num2 ? num2 : a2);
      double num15 = this.ActualYAxis.IsInversed ? (a3 > num3 ? num3 : (a3 < num4 ? num4 : a3)) : (ydata > num4 ? num4 : (ydata < num3 ? num3 : ydata));
      double num16 = this.ActualYAxis.IsInversed ? (ydata < num4 ? num4 : (ydata > num3 ? num3 : ydata)) : (a3 < num3 ? num3 : (a3 > num4 ? num4 : a3));
      num9 = (float) (num6 + size.Width * transformer.XAxis.ValueToCoefficientCalc(num13));
      num10 = (float) (num6 + size.Width * transformer.XAxis.ValueToCoefficientCalc(num14));
      num11 = (float) (num5 + size.Height * (1.0 - transformer.YAxis.ValueToCoefficientCalc(num15)));
      num12 = (float) (num5 + size.Height * (1.0 - transformer.YAxis.ValueToCoefficientCalc(num16)));
    }
    else
    {
      double newBase1 = transformer.XAxis.IsLogarithmic ? (transformer.XAxis as LogarithmicAxis).LogarithmicBase : 1.0;
      double newBase2 = transformer.YAxis.IsLogarithmic ? (transformer.YAxis as LogarithmicAxis).LogarithmicBase : 1.0;
      double num17 = newBase1 == 1.0 ? a1 : Math.Log(a1, newBase1);
      double num18 = newBase1 == 1.0 ? a2 : Math.Log(a2, newBase1);
      double num19 = newBase2 == 1.0 ? ydata : Math.Log(ydata, newBase2);
      double num20 = newBase2 == 1.0 ? a3 : Math.Log(a3, newBase2);
      double num21 = this.ActualXAxis.IsInversed ? (num18 < num2 ? num2 : num18) : (num17 < num1 ? num1 : num17);
      double num22 = this.ActualXAxis.IsInversed ? (num17 > num1 ? num1 : num17) : (num18 > num2 ? num2 : num18);
      double num23 = this.ActualYAxis.IsInversed ? (num20 > num3 ? num3 : (num20 < num4 ? num4 : num20)) : (num19 > num4 ? num4 : (num19 < num3 ? num3 : num19));
      double num24 = this.ActualYAxis.IsInversed ? (num19 < num4 ? num4 : (num19 > num3 ? num3 : num19)) : (num20 < num3 ? num3 : (num20 > num4 ? num4 : num20));
      num9 = (float) (num6 + size.Width * transformer.XAxis.ValueToCoefficientCalc(num21));
      num10 = (float) (num6 + size.Width * transformer.XAxis.ValueToCoefficientCalc(num22));
      num11 = (float) (num5 + size.Height * (1.0 - transformer.YAxis.ValueToCoefficientCalc(num23)));
      num12 = (float) (num5 + size.Height * (1.0 - transformer.YAxis.ValueToCoefficientCalc(num24)));
    }
    double num25 = (double) num9;
    double num26 = (double) num10;
    double num27 = ydata > 0.0 ? (double) num11 : (double) num12;
    double num28 = ydata > 0.0 ? (double) num12 : (double) num11;
    if (this is ISegmentSpacing segmentSpacing)
    {
      double segmentSpacing1 = segmentSpacing.SegmentSpacing;
      if (segmentSpacing1 > 0.0 && segmentSpacing1 <= 1.0)
      {
        double segmentSpacing2 = segmentSpacing.CalculateSegmentSpacing(segmentSpacing1, num26, num25);
        double segmentSpacing3 = segmentSpacing.CalculateSegmentSpacing(segmentSpacing1, num25, num26);
        num25 = segmentSpacing2;
        num26 = segmentSpacing3;
      }
    }
    double num29 = num26 - num25;
    double width2 = (double) (int) this.Area.SeriesClipRect.Width;
    double height2 = (double) (int) this.Area.SeriesClipRect.Height;
    this.selectedSegmentPixels.Clear();
    if (num27 < num28)
      this.selectedSegmentPixels = fastRenderSurface.GetRectangle((int) (width2 - num28), (int) (height2 - num25 - num29), (int) (width2 - num27), (int) (height2 - num25), this.selectedSegmentPixels);
    else
      this.selectedSegmentPixels = fastRenderSurface.GetRectangle((int) (width2 - num27), (int) (height2 - num25 - num29), (int) (width2 - num28), (int) (height2 - num25), this.selectedSegmentPixels);
  }

  internal override void ValidateYValues()
  {
    foreach (double yvalue in (IEnumerable<double>) this.YValues)
    {
      if (double.IsNaN(yvalue) && this.ShowEmptyPoints)
      {
        this.ValidateDataPoints(this.YValues);
        break;
      }
    }
  }

  internal override void ReValidateYValues(List<int>[] emptyPointIndex)
  {
    foreach (List<int> intList in emptyPointIndex)
    {
      foreach (int index in intList)
        this.YValues[index] = double.NaN;
    }
  }

  internal override ChartDataPointInfo GetDataPoint(int index)
  {
    if (this.ActualXAxis is CategoryAxis && !(this.ActualXAxis as CategoryAxis).IsIndexed)
    {
      IList<double> groupedXvaluesIndexes = (IList<double>) this.GroupedXValuesIndexes;
      this.dataPoint = (ChartDataPointInfo) null;
      if (index >= 0 && index < groupedXvaluesIndexes.Count)
      {
        this.dataPoint = new ChartDataPointInfo();
        if (groupedXvaluesIndexes.Count > index)
          this.dataPoint.XData = groupedXvaluesIndexes[index];
        this.dataPoint.Index = index;
        this.dataPoint.Series = (ChartSeriesBase) this;
        switch (this)
        {
          case ColumnSeries _:
          case FastColumnBitmapSeries _:
          case FastStackingColumnBitmapSeries _:
          case StackingColumnSeries _:
            if (this.GroupedSeriesYValues[0].Count > index)
              this.dataPoint.YData = this.GroupedSeriesYValues[0][index];
            if (this.GroupedActualData.Count > index)
            {
              this.dataPoint.Item = this.GroupedActualData[index];
              break;
            }
            break;
          default:
            if (this.YValues.Count > index)
              this.dataPoint.YData = this.YValues[index];
            if (this.ActualData.Count > index)
            {
              this.dataPoint.Item = this.ActualData[index];
              break;
            }
            break;
        }
      }
      return this.dataPoint;
    }
    IList<double> xvalues = (IList<double>) this.GetXValues();
    this.dataPoint = (ChartDataPointInfo) null;
    if (index >= 0 && index < xvalues.Count)
    {
      this.dataPoint = new ChartDataPointInfo();
      if (xvalues.Count > index)
        this.dataPoint.XData = this.IsIndexed ? (double) index : xvalues[index];
      if (this.YValues.Count > index)
        this.dataPoint.YData = this.YValues[index];
      this.dataPoint.Index = index;
      this.dataPoint.Series = (ChartSeriesBase) this;
      if (this.ActualData.Count > index)
        this.dataPoint.Item = this.ActualData[index];
    }
    return this.dataPoint;
  }

  internal override void GeneratePixels()
  {
    if (this.Area == null || this.dataPoint == null)
      return;
    switch (this)
    {
      case FastColumnBitmapSeries _:
        if (!this.IsTransposed)
        {
          this.GenerateColumnPixels();
          break;
        }
        this.GenerateBarPixels();
        break;
      case FastBarBitmapSeries _:
        if (!this.IsTransposed)
        {
          this.GenerateBarPixels();
          break;
        }
        this.GenerateColumnPixels();
        break;
    }
  }

  protected internal override void GeneratePoints()
  {
    if (this.YBindingPath == null)
      return;
    this.GeneratePoints(new string[1]{ this.YBindingPath }, this.YValues);
  }

  protected override void OnDataSourceChanged(IEnumerable oldValue, IEnumerable newValue)
  {
    base.OnDataSourceChanged(oldValue, newValue);
    this.YValues.Clear();
    this.GeneratePoints(new string[1]{ this.YBindingPath }, this.YValues);
    this.isPointValidated = false;
    if (this.ActualXAxis is ChartAxisBase2D actualXaxis)
      actualXaxis.CanAutoScroll = true;
    if (this.ActualYAxis is ChartAxisBase2D actualYaxis)
      actualYaxis.CanAutoScroll = true;
    this.UpdateArea();
  }

  protected override void OnBindingPathChanged(DependencyPropertyChangedEventArgs args)
  {
    this.YValues.Clear();
    base.OnBindingPathChanged(args);
  }

  protected override DependencyObject CloneSeries(DependencyObject obj)
  {
    (obj as XyDataSeries).YBindingPath = this.YBindingPath;
    return base.CloneSeries(obj);
  }

  private static void OnYBindingPathChanged(
    DependencyObject d,
    DependencyPropertyChangedEventArgs e)
  {
    (d as XyDataSeries).OnBindingPathChanged(e);
  }
}
