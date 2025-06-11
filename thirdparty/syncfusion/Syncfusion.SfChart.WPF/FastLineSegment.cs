// Decompiled with JetBrains decompiler
// Type: Syncfusion.UI.Xaml.Charts.FastLineSegment
// Assembly: Syncfusion.SfChart.WPF, Version=19.3460.0.57, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89
// MVID: 279D70C6-7936-4F3E-B6CF-79EFEF734826
// Assembly location: C:\Program Files\PDFgear\Syncfusion.SfChart.WPF.dll

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Shapes;

#nullable disable
namespace Syncfusion.UI.Xaml.Charts;

public class FastLineSegment : ChartSegment
{
  internal DataTemplate customTemplate;
  internal Polyline polyline;
  protected List<double> xValues = new List<double>();
  protected List<double> yValues = new List<double>();
  private bool segmentUpdated;
  private PointCollection points;
  private ContentControl control;
  private double xStart;
  private double xEnd;
  private double yStart;
  private double yEnd;
  private double xDelta;
  private double yDelta;
  private double xSize;
  private double ySize;
  private double xOffset;
  private double yOffset;
  private double xTolerance;
  private double yTolerance;

  public FastLineSegment()
  {
  }

  public FastLineSegment(ChartSeries series) => this.Series = (ChartSeriesBase) series;

  public FastLineSegment(IList<double> xVals, IList<double> yVals, AdornmentSeries series)
    : this((ChartSeries) series)
  {
    this.Series = (ChartSeriesBase) series;
    this.xChartVals = xVals;
    this.yChartVals = yVals;
    this.Item = (object) series.ActualData;
    if (series is FastLineSeries fastLineSeries)
      this.customTemplate = fastLineSeries.CustomTemplate;
    this.SetRange();
  }

  public RenderingMode RenderingMode { get; set; }

  public PointCollection Points
  {
    get => this.points;
    set
    {
      this.points = value;
      this.OnPropertyChanged(nameof (Points));
    }
  }

  protected internal IList<double> xChartVals { get; set; }

  protected internal IList<double> yChartVals { get; set; }

  public override void SetData(IList<double> xVals, IList<double> yVals)
  {
    this.xChartVals = xVals;
    this.yChartVals = yVals;
    this.SetRange();
  }

  public override UIElement CreateVisual(Size size)
  {
    if (this.customTemplate == null)
    {
      this.polyline = new Polyline();
      this.polyline.Tag = (object) this;
      this.SetVisualBindings((Shape) this.polyline);
      return (UIElement) this.polyline;
    }
    this.control = new ContentControl();
    this.control.Content = (object) this;
    this.control.Tag = (object) this;
    this.control.ContentTemplate = this.customTemplate;
    return (UIElement) this.control;
  }

  public override void OnSizeChanged(Size size)
  {
  }

  public override UIElement GetRenderedVisual()
  {
    return this.customTemplate == null ? (UIElement) this.polyline : (UIElement) this.control;
  }

  public override void Update(IChartTransformer transformer)
  {
    ChartSeries series = this.Series as ChartSeries;
    if (transformer == null || series.DataCount <= 1)
      return;
    ChartTransform.ChartCartesianTransformer cartesianTransformer = transformer as ChartTransform.ChartCartesianTransformer;
    if (cartesianTransformer.XAxis is DateTimeAxis xaxis && xaxis.EnableBusinessHours)
    {
      this.CalculatePonts(cartesianTransformer);
    }
    else
    {
      bool flag = cartesianTransformer.XAxis.IsLogarithmic || cartesianTransformer.YAxis.IsLogarithmic;
      this.x_isInversed = cartesianTransformer.XAxis.IsInversed;
      this.y_isInversed = cartesianTransformer.YAxis.IsInversed;
      this.xStart = cartesianTransformer.XAxis.VisibleRange.Start;
      this.xEnd = cartesianTransformer.XAxis.VisibleRange.End;
      this.yStart = cartesianTransformer.YAxis.VisibleRange.Start;
      this.yEnd = cartesianTransformer.YAxis.VisibleRange.End;
      this.xDelta = this.x_isInversed ? this.xStart - this.xEnd : this.xEnd - this.xStart;
      this.yDelta = this.y_isInversed ? this.yStart - this.yEnd : this.yEnd - this.yStart;
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
      this.xTolerance = Math.Abs(this.xDelta * 1.0 / this.xSize);
      this.yTolerance = Math.Abs(this.yDelta * 1.0 / this.ySize);
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
      if (!flag)
        this.TransformToScreenCo();
      else
        this.TransformToScreenCoInLog(cartesianTransformer);
    }
    this.UpdateVisual(true);
  }

  internal void SetRange()
  {
    ChartSeries series = this.Series as ChartSeries;
    bool flag = !(series.ActualXAxis is CategoryAxis) || (series.ActualXAxis as CategoryAxis).IsIndexed;
    if (series.DataCount <= 0)
      return;
    double end1;
    double end2;
    double start1;
    double start2;
    if (series.IsIndexed)
    {
      end1 = flag ? (double) (series.DataCount - 1) : (double) (this.Series.GroupedXValues.Count - 1);
      end2 = this.yChartVals.Max();
      start1 = 0.0;
      start2 = this.yChartVals.Min();
    }
    else
    {
      end1 = this.xChartVals.Max();
      end2 = this.yChartVals.Max();
      start1 = this.xChartVals.Min();
      start2 = this.yChartVals.Min();
    }
    this.XRange = new DoubleRange(start1, end1);
    this.YRange = new DoubleRange(start2, end2);
  }

  internal void UpdateSegment(
    int index,
    NotifyCollectionChangedAction action,
    IChartTransformer transformer)
  {
    if (action == NotifyCollectionChangedAction.Remove)
    {
      this.Points.RemoveAt(index);
    }
    else
    {
      if (action != NotifyCollectionChangedAction.Add)
        return;
      this.Points.Add(transformer.TransformToVisible(this.xChartVals[index], this.yChartVals[index]));
      this.UpdateVisual(false);
    }
  }

  internal void UpdateVisual(bool updatePolyline)
  {
    if (!updatePolyline || this.polyline == null)
      return;
    if (this.segmentUpdated)
      this.Series.SeriesRootPanel.Clip = (Geometry) null;
    this.polyline.Points = this.Points;
    this.segmentUpdated = true;
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
    if (!(this.Series is FastLineSeries))
      return;
    BindingOperations.SetBinding((DependencyObject) this, ChartSegment.StrokeDashArrayProperty, (BindingBase) new Binding()
    {
      Source = (object) this.Series,
      Path = new PropertyPath("StrokeDashArray", new object[0])
    });
    element.SetBinding(Shape.StrokeDashOffsetProperty, (BindingBase) new Binding()
    {
      Source = (object) this.Series,
      Path = new PropertyPath("StrokeDashOffset", new object[0])
    });
    element.SetBinding(Shape.StrokeDashCapProperty, (BindingBase) new Binding()
    {
      Source = (object) this.Series,
      Path = new PropertyPath("StrokeDashCap", new object[0])
    });
    element.SetBinding(Shape.StrokeLineJoinProperty, (BindingBase) new Binding()
    {
      Source = (object) this.Series,
      Path = new PropertyPath("StrokeLineJoin", new object[0])
    });
  }

  private void CalculatePonts(
    ChartTransform.ChartCartesianTransformer cartesianTransformer)
  {
    int num = this.xChartVals.Count - 1;
    this.Points = new PointCollection();
    for (int index = 0; index <= num; ++index)
    {
      double xChartVal = this.xChartVals[index];
      double yChartVal = this.yChartVals[index];
      this.Points.Add(cartesianTransformer.TransformToVisible(xChartVal, yChartVal));
    }
  }

  private void TransformToScreenCo()
  {
    if (this.Series.IsActualTransposed)
      this.TransformToScreenCoVertical();
    else
      this.TransformToScreenCoHorizontal();
  }

  private void TransformToScreenCoHorizontal()
  {
    ChartSeries series = this.Series as ChartSeries;
    this.Points = new PointCollection();
    double num1 = 0.0;
    this.xValues = !(this.Series.ActualXValues is List<double>) || this.Series.IsIndexed ? this.Series.GetXValues() : this.Series.ActualXValues as List<double>;
    if (series.ActualXAxis is CategoryAxis)
    {
      int num2 = (series.ActualXAxis as CategoryAxis).IsIndexed ? 1 : 0;
    }
    bool flag = series.ActualYAxis is NumericalAxis actualYaxis && actualYaxis.AxisRanges != null && actualYaxis.AxisRanges.Count > 0;
    int index1 = 0;
    double num3 = series.IsIndexed ? 1.0 : this.xChartVals[0];
    int index2 = this.xChartVals.Count - 1;
    int index3;
    if (series.isLinearData)
    {
      for (index3 = 1; index3 < index2; ++index3)
      {
        double xChartVal = this.xChartVals[index3];
        double yChartVal = this.yChartVals[index3];
        if (xChartVal <= this.xEnd && xChartVal >= this.xStart)
        {
          if (Math.Abs(num3 - xChartVal) >= this.xTolerance || Math.Abs(num1 - yChartVal) >= this.yTolerance)
          {
            double num4 = 1.0 - (flag ? actualYaxis.CalculateValueToCoefficient(yChartVal) : (yChartVal - this.yStart) / this.yDelta);
            this.Points.Add(new Point(this.xOffset + this.xSize * ((xChartVal - this.xStart) / this.xDelta), this.yOffset + this.ySize * num4));
            num3 = xChartVal;
            num1 = yChartVal;
          }
        }
        else if (xChartVal < this.xStart)
        {
          if (this.x_isInversed)
          {
            double num5 = 1.0 - (flag ? actualYaxis.CalculateValueToCoefficient(yChartVal) : (yChartVal - this.yStart) / this.yDelta);
            this.Points.Add(new Point(this.xOffset + this.xSize * ((xChartVal - this.xStart) / this.xDelta), this.yOffset + this.ySize * num5));
          }
          else
            index1 = index3;
        }
        else if (xChartVal > this.xEnd)
        {
          double interpolarationPoint = ChartMath.GetInterpolarationPoint(this.xChartVals[index3 - 1], this.xChartVals[index3], this.yChartVals[index3 - 1], this.yChartVals[index3], this.xEnd);
          this.Points.Add(new Point(this.xOffset + this.xSize, this.yOffset + this.ySize * (1.0 - (flag ? actualYaxis.CalculateValueToCoefficient(interpolarationPoint) : (interpolarationPoint - this.yStart) / this.yDelta))));
          break;
        }
      }
    }
    else
    {
      for (index3 = 1; index3 < index2; ++index3)
      {
        double xChartVal = this.xChartVals[index3];
        double yChartVal = this.yChartVals[index3];
        if (Math.Abs(num3 - xChartVal) >= this.xTolerance || Math.Abs(num1 - yChartVal) >= this.yTolerance)
        {
          double num6 = 1.0 - (flag ? actualYaxis.CalculateValueToCoefficient(yChartVal) : (yChartVal - this.yStart) / this.yDelta);
          this.Points.Add(new Point(this.xOffset + this.xSize * ((xChartVal - this.xStart) / this.xDelta), this.yOffset + this.ySize * num6));
          num3 = xChartVal;
          num1 = yChartVal;
        }
      }
    }
    if (index1 > 0)
    {
      double interpolarationPoint = ChartMath.GetInterpolarationPoint(this.xChartVals[index1], this.xChartVals[index1 + 1], this.yChartVals[index1], this.yChartVals[index1 + 1], this.xStart);
      this.Points.Insert(0, new Point(this.xOffset, this.yOffset + this.ySize * (1.0 - (flag ? actualYaxis.CalculateValueToCoefficient(interpolarationPoint) : (interpolarationPoint - this.yStart) / this.yDelta))));
    }
    else
    {
      double num7 = 1.0 - (flag ? actualYaxis.CalculateValueToCoefficient(this.yChartVals[index1]) : (this.yChartVals[index1] - this.yStart) / this.yDelta);
      this.Points.Insert(0, new Point(this.xOffset + this.xSize * ((this.xChartVals[index1] - this.xStart) / this.xDelta), this.yOffset + this.ySize * num7));
    }
    if (index3 != index2)
      return;
    double num8 = 1.0 - (flag ? actualYaxis.CalculateValueToCoefficient(this.yChartVals[index2]) : (this.yChartVals[index2] - this.yStart) / this.yDelta);
    this.Points.Add(new Point(this.xOffset + this.xSize * ((this.xChartVals[index2] - this.xStart) / this.xDelta), this.yOffset + this.ySize * num8));
  }

  private void TransformToScreenCoVertical()
  {
    ChartSeries series = this.Series as ChartSeries;
    this.Points = new PointCollection();
    bool flag = series.ActualYAxis is NumericalAxis actualYaxis && actualYaxis.AxisRanges != null && actualYaxis.AxisRanges.Count > 0;
    this.xValues = !(this.Series.ActualXValues is List<double>) || this.Series.IsIndexed ? this.Series.GetXValues() : this.Series.ActualXValues as List<double>;
    if (series.ActualXAxis is CategoryAxis)
    {
      int num1 = (series.ActualXAxis as CategoryAxis).IsIndexed ? 1 : 0;
    }
    int index1 = 0;
    double num2 = series.IsIndexed ? 1.0 : this.xChartVals[0];
    double num3 = this.yChartVals[0];
    int index2 = this.xChartVals.Count - 1;
    int index3;
    if (series.isLinearData)
    {
      for (index3 = 1; index3 < index2; ++index3)
      {
        double xChartVal = this.xChartVals[index3];
        double yChartVal = this.yChartVals[index3];
        if (xChartVal <= this.xEnd && xChartVal >= this.xStart)
        {
          if (Math.Abs(num2 - xChartVal) >= this.xTolerance || Math.Abs(num3 - yChartVal) >= this.yTolerance)
          {
            this.Points.Add(new Point(this.yOffset + this.ySize * (flag ? actualYaxis.CalculateValueToCoefficient(yChartVal) : 1.0 - (this.yEnd - yChartVal) / this.yDelta), this.xOffset + this.xSize * ((this.xEnd - xChartVal) / this.xDelta)));
            num2 = xChartVal;
            num3 = yChartVal;
          }
        }
        else if (xChartVal < this.xStart)
        {
          if (this.x_isInversed)
            this.Points.Add(new Point(this.yOffset + this.ySize * (flag ? actualYaxis.CalculateValueToCoefficient(yChartVal) : 1.0 - (this.yEnd - yChartVal) / this.yDelta), this.xOffset + this.xSize * ((this.xEnd - xChartVal) / this.xDelta)));
          else
            index1 = index3;
        }
        else if (xChartVal > this.xEnd)
        {
          double interpolarationPoint = ChartMath.GetInterpolarationPoint(this.xChartVals[index3 - 1], this.xChartVals[index3], this.yChartVals[index3 - 1], this.yChartVals[index3], this.x_isInversed ? this.xStart : this.xEnd);
          this.Points.Add(new Point(this.yOffset + this.ySize * (flag ? actualYaxis.CalculateValueToCoefficient(interpolarationPoint) : 1.0 - (this.yEnd - interpolarationPoint) / this.yDelta), this.xOffset + this.xSize * ((this.xEnd - xChartVal) / this.xDelta)));
          break;
        }
      }
    }
    else
    {
      for (index3 = 1; index3 < index2; ++index3)
      {
        double xChartVal = this.xChartVals[index3];
        double yChartVal = this.yChartVals[index3];
        if (Math.Abs(num2 - xChartVal) >= this.xTolerance || Math.Abs(num3 - yChartVal) >= this.yTolerance)
        {
          this.Points.Add(new Point(this.yOffset + this.ySize * (flag ? actualYaxis.CalculateValueToCoefficient(yChartVal) : 1.0 - (this.yEnd - yChartVal) / this.yDelta), this.xOffset + this.xSize * ((this.xEnd - xChartVal) / this.xDelta)));
          num2 = xChartVal;
          num3 = yChartVal;
        }
      }
    }
    if (index1 > 0)
    {
      double interpolarationPoint = ChartMath.GetInterpolarationPoint(this.xChartVals[index1], this.xChartVals[index1 + 1], this.yChartVals[index1], this.yChartVals[index1 + 1], this.xStart);
      this.Points.Insert(0, new Point(this.yOffset + this.ySize * (flag ? actualYaxis.CalculateValueToCoefficient(interpolarationPoint) : 1.0 - (this.yEnd - interpolarationPoint) / this.yDelta), this.xOffset + this.xSize * ((this.xEnd - this.xStart) / this.xDelta)));
    }
    else
      this.Points.Insert(0, new Point(this.yOffset + this.ySize * (flag ? actualYaxis.CalculateValueToCoefficient(this.yChartVals[index1]) : 1.0 - (this.yEnd - this.yChartVals[index1]) / this.yDelta), this.xOffset + this.xSize * ((this.xEnd - this.xChartVals[index1]) / this.xDelta)));
    if (index3 != index2)
      return;
    this.Points.Add(new Point(this.yOffset + this.ySize * (flag ? actualYaxis.CalculateValueToCoefficient(this.yChartVals[index2]) : 1.0 - (this.yEnd - this.yChartVals[index2]) / this.yDelta), this.xOffset + this.xSize * ((this.xEnd - this.xChartVals[index2]) / this.xDelta)));
  }

  private void TransformToScreenCoInLog(
    ChartTransform.ChartCartesianTransformer cartesianTransformer)
  {
    double xBase = cartesianTransformer.XAxis.IsLogarithmic ? (cartesianTransformer.XAxis as LogarithmicAxis).LogarithmicBase : 1.0;
    double yBase = cartesianTransformer.YAxis.IsLogarithmic ? (cartesianTransformer.YAxis as LogarithmicAxis).LogarithmicBase : 1.0;
    if (!this.Series.IsActualTransposed)
      this.TransformToScreenCoInLogHorizontal(xBase, yBase);
    else
      this.TransformToScreenCoInLogVertical(xBase, yBase);
  }

  private void TransformToScreenCoInLogHorizontal(double xBase, double yBase)
  {
    this.Points = new PointCollection();
    int index1 = 0;
    double num1 = this.Series.IsIndexed ? 1.0 : this.xChartVals[0];
    double num2 = this.yChartVals[0];
    int index2 = this.xChartVals.Count - 1;
    int index3;
    if (this.Series.isLinearData)
    {
      for (index3 = 1; index3 < index2; ++index3)
      {
        double num3 = xBase == 1.0 || this.xChartVals[index3] <= 0.0 ? this.xChartVals[index3] : Math.Log(this.xChartVals[index3], xBase);
        double num4 = yBase == 1.0 || this.yChartVals[index3] <= 0.0 ? this.yChartVals[index3] : Math.Log(this.yChartVals[index3], yBase);
        if (num3 <= this.xEnd && num3 >= this.xStart)
        {
          if (Math.Abs(num1 - num3) >= this.xTolerance || Math.Abs(num2 - num4) >= this.yTolerance)
          {
            this.Points.Add(new Point(this.xOffset + this.xSize * ((num3 - this.xStart) / this.xDelta), this.yOffset + this.ySize * (1.0 - (num4 - this.yStart) / this.yDelta)));
            num1 = num3;
            num2 = num4;
          }
        }
        else if (num3 < this.xStart)
        {
          if (this.x_isInversed)
            this.Points.Add(new Point(this.xOffset + this.xSize * ((num3 - this.xStart) / this.xDelta), this.yOffset + this.ySize * (1.0 - (num4 - this.yStart) / this.yDelta)));
          else
            index1 = index3;
        }
        else if (num3 > this.xEnd)
        {
          this.Points.Add(new Point(this.xOffset + this.xSize * ((num3 - this.xStart) / this.xDelta), this.yOffset + this.ySize * (1.0 - (num4 - this.yStart) / this.yDelta)));
          break;
        }
      }
    }
    else
    {
      for (index3 = 1; index3 < index2; ++index3)
      {
        double num5 = xBase == 1.0 || this.xChartVals[index3] <= 0.0 ? this.xChartVals[index3] : Math.Log(this.xChartVals[index3], xBase);
        double num6 = yBase == 1.0 || this.yChartVals[index3] <= 0.0 ? this.yChartVals[index3] : Math.Log(this.yChartVals[index3], yBase);
        if (Math.Abs(num1 - num5) >= this.xTolerance || Math.Abs(num2 - num6) >= this.yTolerance)
        {
          this.Points.Add(new Point(this.xOffset + this.xSize * ((num5 - this.xStart) / this.xDelta), this.yOffset + this.ySize * (1.0 - (num6 - this.yStart) / this.yDelta)));
          num1 = num5;
          num2 = num6;
        }
      }
    }
    this.Points.Insert(0, new Point(this.xOffset + this.xSize * (((xBase == 1.0 || this.xChartVals[index1] <= 0.0 ? this.xChartVals[index1] : Math.Log(this.xChartVals[index1], xBase)) - this.xStart) / this.xDelta), this.yOffset + this.ySize * (1.0 - ((yBase == 1.0 || this.yChartVals[index1] <= 0.0 ? this.yChartVals[index1] : Math.Log(this.yChartVals[index1], yBase)) - this.yStart) / this.yDelta)));
    if (index3 != index2)
      return;
    this.Points.Add(new Point(this.xOffset + this.xSize * (((xBase == 1.0 || this.xChartVals[index2] <= 0.0 ? this.xChartVals[index2] : Math.Log(this.xChartVals[index2], xBase)) - this.xStart) / this.xDelta), this.yOffset + this.ySize * (1.0 - ((yBase == 1.0 || this.yChartVals[index2] <= 0.0 ? this.yChartVals[index2] : Math.Log(this.yChartVals[index2], yBase)) - this.yStart) / this.yDelta)));
  }

  private void TransformToScreenCoInLogVertical(double xBase, double yBase)
  {
    this.Points = new PointCollection();
    int index1 = 0;
    double num1 = this.Series.IsIndexed ? 1.0 : this.xChartVals[0];
    double num2 = this.yChartVals[0];
    int index2 = this.xChartVals.Count - 1;
    int index3;
    if (this.Series.isLinearData)
    {
      for (index3 = 1; index3 < index2; ++index3)
      {
        double num3 = xBase == 1.0 || this.xChartVals[index3] <= 0.0 ? this.xChartVals[index3] : Math.Log(this.xChartVals[index3], xBase);
        double num4 = yBase == 1.0 || this.yChartVals[index3] <= 0.0 ? this.yChartVals[index3] : Math.Log(this.yChartVals[index3], yBase);
        if (num3 <= this.xEnd && num3 >= this.xStart)
        {
          if (Math.Abs(num1 - num3) >= this.xTolerance || Math.Abs(num2 - num4) >= this.yTolerance)
          {
            this.Points.Add(new Point(this.yOffset + this.ySize * (1.0 - (this.yEnd - num4) / this.yDelta), this.xOffset + this.xSize * ((this.xEnd - num3) / this.xDelta)));
            num1 = num3;
            num2 = num4;
          }
        }
        else if (num3 < this.xStart)
        {
          if (this.x_isInversed)
            this.Points.Add(new Point(this.yOffset + this.ySize * (1.0 - (this.yEnd - num4) / this.yDelta), this.xOffset + this.xSize * ((this.xEnd - num3) / this.xDelta)));
          else
            index1 = index3;
        }
        else if (num3 > this.xEnd)
        {
          this.Points.Add(new Point(this.yOffset + this.ySize * (1.0 - (this.yEnd - num4) / this.yDelta), this.xOffset + this.xSize * ((this.xEnd - num3) / this.xDelta)));
          break;
        }
      }
    }
    else
    {
      for (index3 = 1; index3 < index2; ++index3)
      {
        double num5 = xBase == 1.0 || this.xChartVals[index3] <= 0.0 ? this.xChartVals[index3] : Math.Log(this.xChartVals[index3], xBase);
        double num6 = yBase == 1.0 || this.yChartVals[index3] <= 0.0 ? this.yChartVals[index3] : Math.Log(this.yChartVals[index3], yBase);
        if (Math.Abs(num1 - num5) >= this.xTolerance || Math.Abs(num2 - num6) >= this.yTolerance)
        {
          this.Points.Add(new Point(this.yOffset + this.ySize * (1.0 - (this.yEnd - num6) / this.yDelta), this.xOffset + this.xSize * ((this.xEnd - num5) / this.xDelta)));
          num1 = num5;
          num2 = num6;
        }
      }
    }
    double num7 = xBase == 1.0 || this.xChartVals[index1] <= 0.0 ? this.xChartVals[index1] : Math.Log(this.xChartVals[index1], xBase);
    this.Points.Insert(0, new Point(this.yOffset + this.ySize * (1.0 - (this.yEnd - (yBase == 1.0 || this.yChartVals[index1] <= 0.0 ? this.yChartVals[index1] : Math.Log(this.yChartVals[index1], yBase))) / this.yDelta), this.xOffset + this.xSize * ((this.xEnd - num7) / this.xDelta)));
    if (index3 != index2)
      return;
    double num8 = xBase == 1.0 || this.xChartVals[index2] <= 0.0 ? this.xChartVals[index2] : Math.Log(this.xChartVals[index2], xBase);
    this.Points.Add(new Point(this.yOffset + this.ySize * (1.0 - (this.yEnd - (yBase == 1.0 || this.yChartVals[index2] <= 0.0 ? this.yChartVals[index2] : Math.Log(this.yChartVals[index2], yBase))) / this.yDelta), this.xOffset + this.xSize * ((this.xEnd - num8) / this.xDelta)));
  }

  internal override void Dispose()
  {
    if (this.polyline != null)
    {
      this.polyline.Tag = (object) null;
      this.polyline = (Polyline) null;
    }
    base.Dispose();
  }
}
