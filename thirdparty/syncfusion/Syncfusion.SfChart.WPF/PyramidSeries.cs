// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.PyramidSeries
// Assembly: Syncfusion.SfChart.WPF, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 279D70C6-7936-4F3E-B6CF-79EFEF734826
// Assembly location: C:\Program Files\PDFgear\Syncfusion.SfChart.WPF.dll

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;

#nullable disable
namespace Syncfusion.UI.Xaml.Charts;

public class PyramidSeries : TriangularSeriesBase, ISegmentSelectable
{
  public static readonly DependencyProperty PyramidModeProperty = DependencyProperty.Register(nameof (PyramidMode), typeof (ChartPyramidMode), typeof (PyramidSeries), new PropertyMetadata((object) ChartPyramidMode.Linear, new PropertyChangedCallback(PyramidSeries.OnPyramidModeChanged)));
  private double currY;

  public PyramidSeries() => this.DefaultStyleKey = (object) typeof (PyramidSeries);

  public ChartPyramidMode PyramidMode
  {
    get => (ChartPyramidMode) this.GetValue(PyramidSeries.PyramidModeProperty);
    set => this.SetValue(PyramidSeries.PyramidModeProperty, (object) value);
  }

  public static double GetSurfaceHeight(double y, double surface)
  {
    double root1;
    double root2;
    return ChartMath.SolveQuadraticEquation(1.0, 2.0 * y, -surface, out root1, out root2) ? Math.Max(root1, root2) : double.NaN;
  }

  public override void CreateSegments()
  {
    this.Adornments.Clear();
    this.Segments.Clear();
    int dataCount = this.DataCount;
    List<double> xvalues = this.GetXValues();
    IList<double> doubleList = this.ToggledLegendIndex.Count <= 0 ? this.YValues : (IList<double>) this.GetYValues();
    double sumValues = 0.0;
    double gapRatio = this.GapRatio;
    ChartPyramidMode pyramidMode = this.PyramidMode;
    for (int index = 0; index < dataCount; ++index)
      sumValues += Math.Max(0.0, Math.Abs(double.IsNaN(doubleList[index]) ? 0.0 : doubleList[index]));
    double gapHeight = gapRatio / (double) (dataCount - 1);
    if (pyramidMode == ChartPyramidMode.Linear)
      this.CalculateLinearSegments(sumValues, gapRatio, dataCount, xvalues);
    else
      this.CalculateSurfaceSegments(sumValues, dataCount, gapHeight, xvalues);
    if (this.ShowEmptyPoints)
      this.UpdateEmptyPointSegments(xvalues, false);
    if (this.ActualArea == null)
      return;
    this.ActualArea.IsUpdateLegend = true;
  }

  protected internal override IChartTransformer CreateTransformer(Size size, bool create)
  {
    if (create || this.ChartTransformer == null)
      this.ChartTransformer = ChartTransform.CreateSimple(size);
    return this.ChartTransformer;
  }

  protected override ChartSegment CreateSegment() => (ChartSegment) new PyramidSegment();

  protected override ChartAdornment CreateAdornment(
    AdornmentSeries series,
    double xVal,
    double yVal,
    double height,
    double currY)
  {
    return (ChartAdornment) new TriangularAdornment(xVal, yVal, currY, height, series);
  }

  protected override void SetExplodeIndex(int i)
  {
    if (this.Segments.Count <= 0)
      return;
    foreach (PyramidSegment segment in (Collection<ChartSegment>) this.Segments)
    {
      int num = this.ActualData.IndexOf(segment.Item);
      if (i == num)
      {
        segment.isExploded = !segment.isExploded;
        this.UpdateSegments(i, NotifyCollectionChangedAction.Remove);
      }
      else if (i == -1)
      {
        segment.isExploded = false;
        this.UpdateSegments(i, NotifyCollectionChangedAction.Remove);
      }
    }
  }

  protected override DependencyObject CloneSeries(DependencyObject obj)
  {
    return base.CloneSeries((DependencyObject) new PyramidSeries()
    {
      PyramidMode = this.PyramidMode
    });
  }

  private static void OnPyramidModeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
  {
    if (!(d is PyramidSeries pyramidSeries) || pyramidSeries.Area == null)
      return;
    pyramidSeries.Area.ScheduleUpdate();
  }

  private void CalculateLinearSegments(
    double sumValues,
    double gapRatio,
    int count,
    List<double> xValues)
  {
    List<double> doubleList = this.YValues.ToList<double>();
    if (this.ToggledLegendIndex.Count > 0)
      doubleList = this.GetYValues();
    this.currY = 0.0;
    double num1 = 1.0 / (sumValues * (1.0 + gapRatio / (1.0 - gapRatio)));
    for (int index = 0; index < count; ++index)
    {
      if (!double.IsNaN(this.YValues[index]))
      {
        double num2 = num1 * Math.Abs(double.IsNaN(doubleList[index]) ? 0.0 : doubleList[index]);
        if (this.CreateSegment() is PyramidSegment segment)
        {
          segment.Series = (ChartSeriesBase) this;
          segment.SetData(this.currY, num2, this.ExplodeOffset);
          segment.isExploded = index == this.ExplodeIndex || this.ExplodeAll;
          segment.Item = this.ActualData[index];
          segment.XData = xValues[index];
          segment.YData = Math.Abs(this.YValues[index]);
          if (this.ToggledLegendIndex.Contains(index))
            segment.IsSegmentVisible = false;
          else
            segment.IsSegmentVisible = true;
          this.Segments.Add((ChartSegment) segment);
        }
        this.currY += gapRatio / (double) (count - 1) + num2;
        if (this.AdornmentsInfo != null)
        {
          this.Adornments.Add(this.CreateAdornment((AdornmentSeries) this, xValues[index], doubleList[index], 0.0, double.IsNaN(this.currY) ? 1.0 - num2 / 2.0 : this.currY - num2 / 2.0));
          this.Adornments[this.Segments.Count - 1].Item = this.ActualData[index];
        }
      }
    }
  }

  private void CalculateSurfaceSegments(
    double sumValues,
    int count,
    double gapHeight,
    List<double> xValues)
  {
    List<double> doubleList = this.YValues.ToList<double>();
    if (this.ToggledLegendIndex.Count > 0)
      doubleList = this.GetYValues();
    this.currY = 0.0;
    double[] numArray1 = new double[count];
    double[] numArray2 = new double[count];
    double surfaceHeight = PyramidSeries.GetSurfaceHeight(0.0, sumValues);
    for (int index = 0; index < count; ++index)
    {
      numArray1[index] = this.currY;
      numArray2[index] = PyramidSeries.GetSurfaceHeight(this.currY, Math.Abs(double.IsNaN(doubleList[index]) ? 0.0 : doubleList[index]));
      this.currY += numArray2[index] + gapHeight * surfaceHeight;
    }
    double num = 1.0 / (this.currY - gapHeight * surfaceHeight);
    for (int index = 0; index < count; ++index)
    {
      if (!double.IsNaN(this.YValues[index]))
      {
        double d = num * numArray1[index];
        if (this.CreateSegment() is PyramidSegment segment)
        {
          segment.Series = (ChartSeriesBase) this;
          segment.SetData(d, num * numArray2[index], this.ExplodeOffset);
          segment.isExploded = index == this.ExplodeIndex || this.ExplodeAll;
          segment.Item = this.ActualData[index];
          segment.XData = xValues[index];
          segment.YData = Math.Abs(this.YValues[index]);
          if (this.ToggledLegendIndex.Contains(index))
            segment.IsSegmentVisible = false;
          else
            segment.IsSegmentVisible = true;
          this.Segments.Add((ChartSegment) segment);
        }
        if (this.AdornmentsInfo != null)
          this.Adornments.Add(this.CreateAdornment((AdornmentSeries) this, xValues[index], doubleList[index], 0.0, double.IsNaN(d) ? 1.0 - numArray2[index] / 2.0 : d + num * numArray2[index] / 2.0));
      }
    }
  }
}
